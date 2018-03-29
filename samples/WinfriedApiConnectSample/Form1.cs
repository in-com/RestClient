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
                options.ServerAddress = "https://develop.in-com.de";
                options.Credentials = new ClientCredentials()
                {
                    ClientId = "c7815a8f862d44a7a43107813f284fd9",
                    ClientSecret = "bjRFTvfTTqNV4WuyK97Km6FwGoKZqZeZj2jaeIDVe74VN7Gy7+5YX3l+X9/VS6TV6F9JkHn1WSnEe45H3YTLmw=="
                };
                options.SigningKey = new X509Certificate2(@"C:\Users\René Klar\Desktop\incom.cer");
                options.AuthenticationEvents = new AuthenticationEvents()
                {
                    OnAuthorized = async (token) =>
                    {
                        var jupp = await _client.Request()
                            .AppendPathSegment("labels")
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
