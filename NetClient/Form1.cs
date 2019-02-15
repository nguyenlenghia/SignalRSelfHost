using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;

namespace NetClient
{
    public partial class Form1 : Form
    {

        private IHubProxy stockTickerHubProxy;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartSignalRAsync();
        }

        private async void StartSignalRAsync()
        {
            var hubConnection = new HubConnection("http://localhost:8080/signalr", useDefaultUrl: false);
            stockTickerHubProxy = hubConnection.CreateHubProxy("myHub");
            stockTickerHubProxy.On<string, string>("AddMessage", (name, message) => {

                this.InvokeIfRequired(c =>
                {
                    txtMessages.AppendText($"{name} {message}\n");
                });
            });
            await hubConnection.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var name = txtDisplayName.Text;
            var message = txtSendingMessage.Text;

            stockTickerHubProxy.Invoke("Send", name, message);
        }
    }

    public static class Extendsion
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new Action(() => action(c)));
            }
            else
            {
                action(c);
            }
        }
    }

    class SendModel
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
    
}
