using System;
using System.Text;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {

 	    Dictionary<string, dynamic> registryBases= new Dictionary<string, object>();
	    registryBases.Add("HKEY_CURRENT_USER",Registry.CurrentUser);
	    registryBases.Add("HKEY_CLASSES_ROOT",Registry.ClassesRoot);
	    registryBases.Add("HKEY_CURRENT_CONFIG",Registry.CurrentConfig);
	    registryBases.Add("HKEY_LOCAL_MACHINE",Registry.LocalMachine);
	    registryBases.Add("HKEY_USERS",Registry.Users);

	    // Print some help
	    Console.WriteLine("Give a key in the following formats:");
	    Console.WriteLine(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
	    Console.WriteLine(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
	    Console.WriteLine(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

        while (true)
        {
            // Starting a menu loop
            Console.Write("# ");
            string userinput = Console.ReadLine();
            if (userinput == null || userinput == "") { 
                Console.WriteLine("Registry key to read?"); 
                continue; 
            }
            // Checking the registry base requested
	        string basename = userinput.Split('\\')[0];
            if (! registryBases.ContainsKey(basename)) { 
                Console.WriteLine("Registry base is not found."); 
                continue; 
            }
            string keyname = userinput.Substring(basename.Length+1,userinput.Length-basename.Length-1);

            // Print what's requested
            Console.WriteLine("Registry base is {0}",basename);
            //Console.WriteLine("Registry key is {0}",keyname);


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
            }

        }
    }

}
