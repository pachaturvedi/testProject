using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Liftr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace TEsting
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World");
            try
            {
                var vaultEndpoint = "https://pc-kv-test.vault.azure.net/";
                var msiClientId = "1be76b84-329e-48ee-ad3f-c0a67968d406";
                var tokenProviderConnectionString = $"RunAs=App;AppId={msiClientId}";
                var tokenProvider = new AzureServiceTokenProvider(connectionString: tokenProviderConnectionString);
                var callback = new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback);
                var keyvaultClient = new KeyVaultClient(callback);

                var key = await keyvaultClient.GetKeyAsync(vaultEndpoint, "secretKey");

                Console.WriteLine(key);

                //var client = new SecretClient(new Uri(vaultEndpoint), new ManagedIdentityCredential(clientId: msiClientId));
                //var client = new SecretClient(new Uri(vaultEndpoint), new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = msiClientId }));

                //string secretName = "secretKey";
                //KeyVaultSecret secret = client.GetSecret(secretName);

                //Console.WriteLine("Your secret is '" + secret.Value + "'.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
