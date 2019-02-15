using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;

namespace SignalRSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(); // auto using all class that implemented class Hub

        }
    }
    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.AddMessage(name, message);
            //Clients.All.AddMessage(new SendModelServer() { Name = name, Message = message });

            System.Diagnostics.Trace.WriteLine($"{name} {message}");
        }
    }

    class SendModelServer
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}