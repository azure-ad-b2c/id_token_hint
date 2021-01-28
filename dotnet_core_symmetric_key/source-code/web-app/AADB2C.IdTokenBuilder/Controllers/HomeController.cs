using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AADB2C.IdTokenBuilder.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace AADB2C.IdTokenBuilder.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettingsModel AppSettings;

        public HomeController(IOptions<AppSettingsModel> appSettings)
        {
            this.AppSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            ViewData["appName"] = "WoodGrove Groceries";
            ViewData["displayName"] = "Western Miller";

            return View();
        }


        [HttpGet]
        public ActionResult Index(string appName, string displayName)
        {

            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(displayName))
            {
                ViewData["Message"] = "";
                return View();
            }

            ViewData["appName"] = appName;
            ViewData["displayName"] = displayName;

            string token = BuildIdToken(appName, displayName);
            ViewData["token"] = token;
            ViewData["link"] = BuildUrl(token);


            return View();
        }


        private string BuildIdToken(string appName, string displayName)
        {
            string issuer = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase.Value}/";

            // All parameters send to Azure AD B2C needs to be sent as claims
            IList<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
            claims.Add(new System.Security.Claims.Claim("appName", appName, System.Security.Claims.ClaimValueTypes.String, issuer));
            claims.Add(new System.Security.Claims.Claim("displayName", displayName, System.Security.Claims.ClaimValueTypes.String, issuer));

            // Note: This key phrase needs to be stored also in Azure B2C Keys for token validation
            var securityKey = Encoding.UTF8.GetBytes(AppSettings.ClientSigningKey);

            var signingKey = new SymmetricSecurityKey(securityKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Create the token
            JwtSecurityToken token = new JwtSecurityToken(
                    issuer,
                    this.AppSettings.B2CClientId,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddDays(7),
                    signingCredentials);

            // Get the representation of the signed token
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }

        private string BuildUrl(string token)
        {
            string nonce = Guid.NewGuid().ToString("n");

            return string.Format(this.AppSettings.B2CAuthorizationUrl,
                    this.AppSettings.B2CTenant,
                    this.AppSettings.B2CPolicy,
                    this.AppSettings.B2CClientId,
                    Uri.EscapeDataString(this.AppSettings.B2CRedirectUri),
                    nonce) + "&id_token_hint=" + token;
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
