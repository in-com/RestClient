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
                options.ServerAddress = "http://localhost:59027";
                options.Credentials = new ClientCredentials()
                {
                    ClientId = "0f3348f7cdf64ceaba6a74965804d2e9",
                    ClientSecret = "EsnMT5J+7bJQMwWpNE8GfZYLDqEKAJQ9jgZIVSGBi8hLo+LbOsqZ8dEYy3JxZy1HrZB8gWLiM55CV6SAk7sfXw=="
                };
                options.SigningKey = new X509Certificate2(@"C:\Users\rklar\Desktop\incom.cer");
                options.AuthenticationEvents = new AuthenticationEvents()
                {
                    OnAuthorized = async (token) =>
                    {
                        var jupp = await _client.Request()
                            .AppendPathSegment("events")
                            .AppendPathSegment(4576)
                            .GetAsync<Appointment>();

                        var starttime = new DateTime(DateTime.Now.Ticks, DateTimeKind.Local);
                        var endtime = new DateTime(starttime.AddMinutes(30).Ticks, DateTimeKind.Local);

                        var appointment = new AppointmentPost()
                        {
                            Starttime = new DateTimeOffset(starttime),
                            Endtime = new DateTimeOffset(endtime),
                            CalendarId = 1,
                            SubjectId = 1,
                            StatusId = 1
                        };

                        var block = new BlockPost()
                        {
                            Starttime = new DateTimeOffset(starttime),
                            Endtime = new DateTimeOffset(endtime),
                            CalendarId = "1",
                            Description = "srhdthdtj"
                        };

                        //var newAppointment = await _client.Request()
                        //    .AppendPathSegment("events")
                        //    .AppendPathSegment("sync")
                        //    .AppendPathSegment(0)
                        //    .SetBody(appointment)
                        //    .PostAsync<Appointment>();

                        await _client.Request()
                            .AppendPathSegment("time")
                            .AppendPathSegment("lock")
                            .SetQueryParam("isSync", "true")
                            .SetBody(block)
                            .PostAsync();
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
