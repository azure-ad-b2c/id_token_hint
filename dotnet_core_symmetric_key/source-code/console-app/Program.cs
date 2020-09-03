using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdTokenBuilder
{
    class Program
    {
        static readonly string _signingKey = "VK62QTn0m1hMcn0DQ3RPYDAr6yIiSvYgdRwjZtU5QhI=";
        static readonly string _issuer = $"https://localhost";
        static readonly string _audience = "a489fc44-3cc0-4a78-92f6-e413cd853eae";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string JWT = BuildIdToken("john.s@contoso.com", " John Smith");
            Console.WriteLine(JWT);
        }

        private static string BuildIdToken(string userId, string displayName)
        {


            // All parameters send to Azure AD B2C needs to be sent as claims
            IList<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
            claims.Add(new System.Security.Claims.Claim("displayName", displayName, System.Security.Claims.ClaimValueTypes.String, _issuer));
            claims.Add(new System.Security.Claims.Claim("userId", userId, System.Security.Claims.ClaimValueTypes.String, _issuer));

            // Note: This key phrase needs to be stored also in Azure B2C Keys for token validation
            var securityKey = Encoding.UTF8.GetBytes(_signingKey);

            var signingKey = new SymmetricSecurityKey(securityKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Create the token
            JwtSecurityToken token = new JwtSecurityToken(
                    _issuer,
                    _audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddDays(7),
                    signingCredentials);

            // Get the representation of the signed token
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }
    }
}
