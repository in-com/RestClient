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
                options.RestApiType = RestApiType.WinFriedSE;
                options.ServerAddress = "http://localhost:62203";
                options.Credentials = new ClientCredentials()
                {
                    ClientId = "f4b4f1878fbc47ba92964e31f2da6528",
                    ClientSecret = "foZva8tFWiMSEqVGmfkhdmVPLyKJGP4A1YuPUoS3uAtPkKvuyGercfGsmAOnUryTBih386zLjklLuV5iRkHA=="
                };
                //options.SigningKey = new X509Certificate2(@"C:\Users\rklar\Desktop\incom.cer");
                options.AuthenticationEvents = new AuthenticationEvents()
                {
                    OnAuthorized = async (token) =>
                    {
                        var jupp = await _client.Request()
                            .AppendPathSegment("hub")
                            .AppendPathSegment("clients")
                            .GetAsync<object>();

                        //var starttime = new DateTime(DateTime.Now.Ticks, DateTimeKind.Local);
                        //var endtime = new DateTime(starttime.AddMinutes(30).Ticks, DateTimeKind.Local);

                        //var appointment = new AppointmentPost()
                        //{
                        //    Starttime = new DateTimeOffset(starttime),
                        //    Endtime = new DateTimeOffset(endtime),
                        //    CalendarId = 1,
                        //    SubjectId = 1,
                        //    StatusId = 1
                        //};

                        //var block = new BlockPost()
                        //{
                        //    Starttime = new DateTimeOffset(starttime),
                        //    Endtime = new DateTimeOffset(endtime),
                        //    CalendarId = "1",
                        //    Description = "srhdthdtj"
                        //};

                        ////var newAppointment = await _client.Request()
                        ////    .AppendPathSegment("events")
                        ////    .AppendPathSegment("sync")
                        ////    .AppendPathSegment(0)
                        ////    .SetBody(appointment)
                        ////    .PostAsync<Appointment>();

                        //await _client.Request()
                        //    .AppendPathSegment("time")
                        //    .AppendPathSegment("lock")
                        //    .SetQueryParam("isSync", "true")
                        //    .SetBody(block)
                        //    .PostAsync();
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
