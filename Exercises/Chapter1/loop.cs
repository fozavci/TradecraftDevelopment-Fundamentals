using System;
namespace Application 
{
    public static class Program
    {
        public static void Main()
        {
            // Loop for 0 to 10, and print loop count
            for(int i=0; i < 10; ++i)  
                Console.WriteLine("Loop: {0}",i+1);

            // Loop for 0 to 5, and print loop count                        
            int n = 0;
            while (n < 5)
            {
                Console.WriteLine("Loop: {0}",n+1);
                n++;
            }
        }
    }
}


