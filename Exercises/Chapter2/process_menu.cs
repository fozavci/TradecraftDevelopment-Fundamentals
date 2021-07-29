using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("# ");
            string userinput = Console.ReadLine();
            if (userinput == null || userinput == "") { 
                Console.WriteLine("Process to run?"); 
                continue; 
            }
            try
            {
                string process_name = userinput.Split( ' ' )[0];
                if (process_name == "exit") { break; }                
                string process_arguments = userinput.Substring(process_name.Length+1);
                Console.WriteLine("Process Name\t\t: {0}", process_name);  
                Console.WriteLine("Process Arguments\t: {0}", process_arguments);
                ExecProcess(process_name,process_arguments);                  
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh snap! " + e.Message);
            }
        }
    }

    static void ExecProcess(string process_name, string process_arguments)
        {
            string output = "";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = process_name;
            startInfo.Arguments = process_arguments;                
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true; 
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine("Process started.\nOutput:");                
            output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(output); 
            if (err != "") {
                Console.WriteLine("Error: {0}",err);
            }
            process.WaitForExit();
        }
}
