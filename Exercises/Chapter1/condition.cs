using System;
public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0) {
            // If there is no argument, say something nice. 
            Console.WriteLine("Dude?");             
        }
        else {                
            // we define a parameter and assign a value. 
            string parameter1 = String.Join(" ",args);
            // print the parameter in a context.
            Console.WriteLine("This is the parameter to write: {0}.", parameter1);
        }
        return;
    }
}


