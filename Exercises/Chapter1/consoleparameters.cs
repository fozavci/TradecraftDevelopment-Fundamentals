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
            switch (args[0])
                {
                    case "write":
                        // we define a parameter and assign a value. 
                        string parameter1 = args[1];
                        // print the parameter in a context.
                        Console.WriteLine("This is the parameter to write: {0}.", parameter1);
                        break;                        
                    case "read":
                        // we cast the args[1] as string
                        Console.WriteLine("The TestFunction is calling with {0}.", args[1] as string);
                        // Call TestFunction and pass the args[1] as string
                        TestFunction(args[1] as string);
                        break;
                    default:
                        Console.WriteLine("That's ok, let's try again");
                        break;
                }
        }
        return;
    }
    public static void TestFunction(string lifeis)
    {
        Console.WriteLine("The function is called with {0} successfully", lifeis);
    }
}



