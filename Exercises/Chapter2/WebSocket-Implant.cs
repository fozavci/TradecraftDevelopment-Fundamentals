using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;


public class Program
{

    public static void Main(string[] args)
    {
        if (args.Length == 0) {
            Console.WriteLine("Usage: wsi.exe ws://localhost:5001");
            return;
        }

        string c2url = args[0];
        Console.WriteLine("Connecting to {0}", c2url);

        WebSocketClient wsc = new WebSocketClient();

        wsc.Connect(c2url).Wait();
        
    }    
}

public class WebSocketClient {
    public async Task Connect(string c2url) {
        
        // create a public websocket object
        ClientWebSocket webSocket = new ClientWebSocket();

        // connect to the websocket in configuration
        Console.WriteLine("Linking to the C2 via websocket... ");        
        webSocket.ConnectAsync(new Uri(c2url), CancellationToken.None).Wait();
        Console.WriteLine("The websocket connection is successfully established.");
        
        try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    Console.Error.WriteLine("Waiting for an instruction...");
                    // receiving the instruction from websocket
                    byte[] rbuffer = new byte[4000];
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(rbuffer), CancellationToken.None);
                    string instruction = Regex.Replace((string)Encoding.UTF8.GetString(rbuffer),"\0", string.Empty);
                    Console.Error.WriteLine("Got an instruction:\n{0}",instruction);

                    // processing the instruction
                    string data = InstructionProcess(instruction);

                    // get bytes of the data
                    byte[] buffer_bytes = Encoding.UTF8.GetBytes(data);

                    // send the data with buffer header
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer_bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.Error.WriteLine("Socket is closing...");
                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);                    
                    }
                }
            }
        catch (Exception e)
        {
            Console.Error.WriteLine("The service stopped responding: {0}", e);
            await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
        
    }

    public string InstructionProcess(string instruction) {
        try
        {
            string response = "";
            switch (Regex.Split(instruction, " ")[0])
            {
                case "help":
                    // let's help 
                    response = "Example commands:\n* echo 123\n* exit\n* run whoami";
                    break;     
                case "echo":
                    // let's echo what's asked 
                    response = Regex.Replace(instruction, "echo ", "");
                    break;                        
                case "exit":
                    // we exit                        
                    System.Environment.Exit(1);
                    break;
                case "run":
                    response = "I don't know how to run.";
                    break;
                default:
                    response = "Unknown Command!";
                    break;
            }
            return response+"\n# ";
        }
        catch (Exception e)
        {
            return "Oh snap! " + e.Message;
        }
    }
}
