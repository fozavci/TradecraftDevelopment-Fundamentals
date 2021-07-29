using System;
public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("# ");
            string userinput = Console.ReadLine();
            try
            {
                switch (userinput.Substring(0, Math.Min(4, userinput.Length)))
                    {
                        case "echo":
                            // let's echo what's asked 
                            Console.WriteLine(userinput.Substring(5,userinput.Length-5));
                            break;                        
                        case "stop":
                            // we exit
                            System.Environment.Exit(1);
                            break;
                        case "run":
                            Console.WriteLine("I don't Know");
                            break;
                        default:
                            // If there is no argument, say something nice. 
                            Console.WriteLine("Dude? You should write \"echo 12\" or something.");
                            Console.WriteLine("Instead you wrote: {0}", userinput);       
                            Console.WriteLine("If you didn't like, write \"stop\"");       
                            break;
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh snap! " + e.Message);
            }
        }
        
    }
}
