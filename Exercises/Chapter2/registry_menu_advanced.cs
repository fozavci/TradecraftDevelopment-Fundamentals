using System;
using System.Text;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;

public class Program
{
    public static Dictionary<string, dynamic> registryBases= new Dictionary<string, object>();
    public static void Main()
    {
	    
	    // Put the registry bases to the dictionary
        registryBases.Add("HKEY_CURRENT_USER",Registry.CurrentUser);
	    registryBases.Add("HKEY_CLASSES_ROOT",Registry.ClassesRoot);
	    registryBases.Add("HKEY_CURRENT_CONFIG",Registry.CurrentConfig);
	    registryBases.Add("HKEY_LOCAL_MACHINE",Registry.LocalMachine);
	    registryBases.Add("HKEY_USERS",Registry.Users);

        while (true)
        {
            // Starting a menu loop
            Console.Write("# ");
            string userinput = Console.ReadLine();
            if (userinput == null || userinput == "") { 
                Console.WriteLine("Use readkey, createkey, deletekey, setvalue or deletevalue."); 
                continue; 
            }
            
            string key ;

            switch (userinput)
            {
                case "readkey":
                    // Get the key from the user
                    key = GetKeyInput("read");                
                    // Call read registry key
                    ReadKey(key);
                    break;                        
                case "createkey":                
                    // Get the key from the user
                    key = GetKeyInput("create");                
                    // Call create registry key
                    CreateKey(key);
                    break;              
                case "deletekey":
                    // Get the key from the user
                    key = GetKeyInput("delete");                  
                    // Call create registry key
                    DeleteKey(key);
                    break;                      
                case "setvalue":
                    // Get the key from the user
                    key = GetKeyInput("update");                
                    // Call create registry key
                    SetValue(key);
                    break;    
                case "deletevalue":
                    // Get the key from the user
                    key = GetKeyInput("update");                
                    // Call create registry key
                    DeleteValue(key);
                    break;    
                case "exit":
                    return;                      
                default:
                Console.WriteLine("Use readkey, createkey, deletekey, setvalue or deletevalue."); 
                    break;
            }
        }
    }

    public static string GetKeyInput(string action)
    {
        string key;
        // Print some help
        Console.WriteLine("Give a key in the following formats:");
        Console.WriteLine(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        Console.WriteLine(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        Console.WriteLine(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");                    
        Console.WriteLine("\nRegistry key to {0}?",action); 
        key = Console.ReadLine(); 
        return key;    
    }
    public static void ReadKey(string userinput)
    {
        // Checking the registry base requested
        string basename = userinput.Split('\\')[0];
        if (! Program.registryBases.ContainsKey(basename)) { 
            Console.WriteLine("Registry base is not found."); 
            return; 
        }
        string keyname = userinput.Substring(basename.Length+1,userinput.Length-basename.Length-2);

        // Print what's requested
        Console.WriteLine("Registry base is {0}",basename);
        Console.WriteLine("Registry key is {0}",keyname);


        // Open the key for a "scope"
        using(RegistryKey key = registryBases[basename].OpenSubKey(keyname))
        {
            // Print the Sub Keys in a loop
            foreach(String subkeyName in key.GetSubKeyNames()) {
                Console.WriteLine(key.OpenSubKey(subkeyName).GetValue("DisplayName"));
            }
            // Print the Names and Values in a loop
            foreach (string valuename in key.GetValueNames()) {
                Console.WriteLine("Name: {0},\t Value: {1}", valuename,key.GetValue(valuename));  
            }

            key.Close();  
        }
    }

    public static void CreateKey(string userinput)
    {
        // Checking the registry base requested
        string basename = userinput.Split('\\')[0];
        if (! Program.registryBases.ContainsKey(basename)) { 
            Console.WriteLine("Registry base is not found."); 
            return; 
        }
        string keyname = userinput.Substring(basename.Length+1,userinput.Length-basename.Length-1);

        // Print what's requested
        Console.WriteLine("Registry base is {0}",basename);
        Console.WriteLine("Registry key is {0}",keyname);

        // Create the key or fail
        try {
            RegistryKey key = registryBases[basename].CreateSubKey(keyname); 
            Console.WriteLine("Created successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Couldn't create it: {0}", e);
        }
    }

    public static void DeleteKey(string userinput)
    {
        // Checking the registry base requested
        string basename = userinput.Split('\\')[0];
        if (! Program.registryBases.ContainsKey(basename)) { 
            Console.WriteLine("Registry base is not found."); 
            return; 
        }
        string keyname = userinput.Substring(basename.Length+1,userinput.Length-basename.Length-1);

        // Print what's requested
        Console.WriteLine("Registry base is {0}",basename);
        Console.WriteLine("Registry base key is {0}",keyname);

        // Create the key or fail
        try {
            registryBases[basename].DeleteSubKey(keyname);
            registryBases[basename].Close();
            Console.WriteLine("Deleted successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Couldn't delete it: {0}", e);
        }
    }

    public static void SetValue(string userinput)
    {
        // Checking the registry base requested
        string basename = userinput.Split('\\')[0];
        if (! Program.registryBases.ContainsKey(basename)) { 
            Console.WriteLine("Registry base is not found."); 
            return; 
        }
        string keyname = userinput.Substring(basename.Length+1,userinput.Length-basename.Length-1);

        // Print what's requested
        Console.WriteLine("Registry base is {0}",basename);
        Console.WriteLine("Registry key is {0}",keyname);

        Console.WriteLine("\nRegistry Value Name?"); 
        string vname = Console.ReadLine(); 
        Console.WriteLine("\nRegistry Value?"); 
        string value = Console.ReadLine(); 
         
        // Set the value or fail
        try {          
            // Open the key for a "scope"
            using(RegistryKey key = registryBases[basename].OpenSubKey(keyname,true)) // true makes it writable
            {
                key.SetValue(vname,value,RegistryValueKind.String);  
                key.Close();  // close it when you're finished
                Console.WriteLine("The registry value added successfully.");
            }            
        }
        catch (Exception e)
        {
            Console.WriteLine("Couldn't add the value: {0}", e);
        }
    }

    public static void DeleteValue(string userinput)
    {
        // Checking the registry base requested
        string basename = userinput.Split('\\')[0];
        if (! Program.registryBases.ContainsKey(basename)) { 
            Console.WriteLine("Registry base is not found."); 
            return; 
        }
        string keyname = userinput.Substring(basename.Length+1,userinput.Length-basename.Length-1);

        // Print what's requested
        Console.WriteLine("Registry base is {0}",basename);
        Console.WriteLine("Registry key is {0}",keyname);

        Console.WriteLine("\nRegistry Value Name?"); 
        string vname = Console.ReadLine(); 
        
        // Delete the value or fail
        try {          
            // Open the key for a "scope"
            using(RegistryKey key = registryBases[basename].OpenSubKey(keyname,true)) // true makes it writable
            {
                key.DeleteValue(vname);  
                key.Close();  // close it when you're finished
                Console.WriteLine("The registry value deleted successfully.");
            }            
        }
        catch (Exception e)
        {
            Console.WriteLine("Couldn't delete the value: {0}", e);
        }
    }

}
