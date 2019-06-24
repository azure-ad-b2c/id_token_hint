# Asp.Net core token builder with symmetric key 
This sample ASP.NET web application generates ID tokens and hosts the necessary metadata endpoints required to use the `id_token_hint` query string parameter in Azure AD B2C. ID tokens are JSON Web Tokens (JWTs) and, in this application, are signed using symmetric key. 


## Sending Application Data

The key of sending data to Azure AD B2C custom policy is to package the data into a JWT token as claims (id_token_hint). In this case, we send the user user **display name** and the **application name** to Azure B2C. 


With **id_token_hint**, the web application signs the JWT token to prove the token request comes from your application, by using signing key. You need the signing key, later to store it B2C keys. Your policy uses that key to validate the incoming JWT token, issued by your web application. Use following PowerShell code to generate client secret.

```PowerShell
$bytes = New-Object Byte[] 32
$rand = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rand.GetBytes($bytes)
$rand.Dispose()
$newClientSecret = [System.Convert]::ToBase64String($bytes)
$newClientSecret
```

> Note: the PowerShell generates a secret string. But you can define and use any arbitrary string.

###  Add the signing key to your application
Your application needs to sign the outgoing ID token. Store the client secret in your application configuarion file:  
```JSON
  "AppSettings": {
    ...
    "ClientSigningKey": "VK62QTn0m1hMcn0DQ3RPYDAr6yIiSvYgdRwjZtU5QhI="
  }
```

###  Add the signing key to Azure AD B2C
Azure AD B2C needs the client secret to validate the incoming ID token. You need to store the client secret your application uses to sign in, in your Azure AD B2C tenant:  

1.  Go to your Azure AD B2C tenant, and select **B2C Settings** > **Identity Experience Framework**
2.  Select **Policy Keys** to view the keys available in your tenant.
3.  Click **+Add**.
4.  For **Options**, use **Manual**.
5.  For **Name**, use `ClientAssertionSigningKey`.  
    The prefix `B2C_1A_` might be added automatically.
6.  In the **Secret** box, enter your sign-in key you generated earlier
7.  For **Key usage**, use **Encryption**.
8.  Click **Create**
9.  Confirm that you've created the key `B2C_1A_ClientAssertionSigningKey`.


## Community Help and Support
Use [Stack Overflow](https://stackoverflow.com/questions/tagged/azure-ad-b2c) to get support from the community. Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before. Make sure that your questions or comments are tagged with [azure-ad-b2c].
If you find a bug in the sample, please raise the issue on [GitHub Issues](https://github.com/azure-ad-b2c/samples/issues).
To provide product feedback, visit the Azure Active Directory B2C [Feedback page](https://feedback.azure.com/forums/169401-azure-active-directory?category_id=160596).
