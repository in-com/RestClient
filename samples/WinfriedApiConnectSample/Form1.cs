using Incom.Web.RestClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private RestClient _client;

        public Form1()
        {
            InitializeComponent();

            _client = new RestClient(options =>
            {
                options.RestApiType = RestApiType.Terminplaner;
                options.ServerAddress = "http://localhost.fiddler:21699/";
                options.Credentials = new ClientCredentials()
                {
                    ClientId = "7c21e0033caa482e99d4376d6c4f7695",
                    ClientSecret = "SqnEm/t9GH24pPpHXiZ4LPrr/6sh4FSEuKh28DV/zJHpjb+9CE3apn7ndDRTZrh2rPNVHlpUTCSSYRGnEBB9kQ=="
                };
                options.SigningKey = new X509Certificate2(@"C:\Users\rklar\Desktop\incom.cer");
                options.AuthenticationEvents = new AuthenticationEvents()
                {
                    OnAuthorized = async (token) =>
                    {
                        var date = new DateTimeOffset(2017, 1, 1, 0, 0, 0, TimeSpan.FromHours(2));
                        var jupp = await _client.Request()
                            .AppendPathSegment("events")
                            .AppendPathSegment("claims")
                            .GetAsync<object>();
                    },
                    OnAuthorizationFailed = (context) =>
                    {
                        context.Handled();
                        return Task.CompletedTask;
                    }
                };
                options.EndpointEvents = new EndpointEvents()
                {
                    OnRequestFailed = (context) =>
                    {
                        context.Handled();
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await _client.ConnectAsync();
        }
    }
}
