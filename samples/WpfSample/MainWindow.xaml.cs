using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using Incom.Web;
using Incom.Web.HubClient;
using Incom.Web.HubClient.Models;
using Incom.Web.Models;
using Incom.Web.RestClient;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace WpfSample
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        /// <summary>
        /// Enthält eine Instanz des <see cref="RestClient"/>.
        /// </summary>
        private RestClient _client;

        /// <summary>
        /// Enthält eine Instanz des <see cref="HubClient"/>.
        /// </summary>
        private HubClient _hubClient;

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisert eine neue Instanz der <see cref="MainWindow"/> Klasse.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitWfse();
            InitHub();
        }

        #endregion

        #region Events (RestClient)

        /// <summary>
        /// Stellt eine Verbindung zu API her.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WfseConnectButton_Click(object sender, RoutedEventArgs e)
        {
            _client.SetServerAddress(WfseApiServerTextBox.Text);
            _client.SetClientCredentials(WfseClientIdTextBox.Text, WfseClientSecretTextBox.Text);
            await _client.ConnectAsync();
        }

        /// <summary>
        /// Wird ausgeführt, sobald eine Verbindung zur API erfolgreich hergestellt wurde.
        /// </summary>
        /// <param name="token">Das angeforderte Token.</param>
        /// <returns></returns>
        private Task OnAuthorized(AuthenticationContext context)
        {
            WfseConnectionInfoTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            WfseConnectionInfoTextBlock.Text = "Verbunden";
            WfseApiServerTextBox.IsEnabled = false;
            WfseClientIdTextBox.IsEnabled = false;
            WfseClientSecretTextBox.IsEnabled = false;
            WfseConnectButton.IsEnabled = false;
            WfseEndpointTextBox.IsEnabled = true;
            WfseEndpointVersionCombox.IsEnabled = true;
            WfseRequestButton.IsEnabled = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wird ausgeführt, wenn die Verbindung zur API fehlgeschlagen ist.
        /// </summary>
        /// <param name="context">Ein <see cref="AuthenticationFailedContext"/> mit weiteren Informationen.</param>
        /// <returns></returns>
        private Task OnAuthorizationFailed(AuthenticationFailedContext context)
        {
            MessageBox.Show(context.Error.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            context.Handled();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wird ausgeführt, wenn ein Anfrage an den Server fehlschlägt.
        /// </summary>
        /// <param name="context">Ein <see cref="EndpointFailedContext"/> mit weiteren Informationen.</param>
        /// <returns></returns>
        private Task OnRequestFailed(EndpointFailedContext context)
        {
            MessageBox.Show(context.Error.Message, "Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
            context.Handled();
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Sendet eine Anfrage an den API Server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WfseRequestButton_Click(object sender, RoutedEventArgs e)
        {
            ApiEndpointVersion endpointVersion;

            switch (WfseEndpointVersionCombox.SelectedValue)
            {
                case 1:
                    endpointVersion = ApiEndpointVersion.Version1;
                    break;
                default:
                    endpointVersion = ApiEndpointVersion.Version2;
                    break;
            }

            var content = await _client.Request(endpointVersion)
                .AppendPathSegment(WfseEndpointTextBox.Text)
                .GetAsync<object>();

            if (content != null)
            {
                WfseTextEditor.Text = JsonConvert.SerializeObject(content, new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
            }
        }

        #endregion

        #region Events (HubClient)

        /// <summary>
        /// Stellt eine Verbindung zu API her.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HubConnectButton_Click(object sender, RoutedEventArgs e)
        {
            _hubClient.SetServerAddress(HubApiServerTextBox.Text);
            _hubClient.SetClientCredentials(HubClientIdTextBox.Text, HubClientSecretTextBox.Text);
            await _hubClient.ConnectAsync();
        }

        /// <summary>
        /// Trennt die Verbindung zum Hub.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HubDisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            _hubClient.Disconnect();
        }

        /// <summary>
        /// Sendet eine Anfrage an den API Server um seine Versionsnummer zu ermitteln.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HubGetApiVersionButton_Click(object sender, RoutedEventArgs e)
        {
            await _hubClient.GetApiVersion();
        }

        /// <summary>
        /// Wird ausgeführt, sobald eine Verbindung zum Hub erfolgreich hergestellt wurde.
        /// </summary>
        /// <param name="token">Das angeforderte Token.</param>
        /// <returns></returns>
        private Task OnHubAuthorized(AuthenticationContext context)
        {
            SetHubControls(true, false);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wird ausgeführt, wenn die Verbindung zur API fehlgeschlagen ist.
        /// </summary>
        /// <param name="context">Ein <see cref="AuthenticationFailedContext"/> mit weiteren Informationen.</param>
        /// <returns></returns>
        private Task OnHubAuthorizationFailed(AuthenticationFailedContext context)
        {
            MessageBox.Show(context.Error.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            SetHubControls(false, false);
            context.Handled();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wird ausgeführt, sobald eine Verbindung hergestellt wurde.
        /// </summary>
        /// <returns></returns>
        private async Task OnConnected()
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                SetHubControls(true, true);
            }));
        }

        /// <summary>
        /// Wird ausgeführt, sobald die Verbindung getrennt wurde.
        /// </summary>
        /// <returns></returns>
        private async Task OnDisconnected()
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                SetHubControls(false, false);
            }));
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Server seine Versionsnummer sendet.
        /// </summary>
        /// <param name="version">Die Versionsnummer des Servers.</param>
        /// <returns></returns>
        private async Task OnApiVersion(string version)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                HubTextEditor.Document.Insert(HubTextEditor.Document.TextLength, version);
                HubTextEditor.Document.Insert(HubTextEditor.Document.TextLength, "\n");
            }));
        }

        #endregion

        #region Events

        /// <summary>
        /// Leert den sichtbaren TextEditor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearEditorButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Tabs.SelectedIndex)
            {
                case 0:
                    WfseTextEditor.Clear();
                    break;
                case 1:
                    HubTextEditor.Clear();
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Richtet das Tab "WinFriedSE" ein.
        /// </summary>
        private void InitWfse()
        {
            // ComboBox mit den Endpunkt Versionen füllen.
            SetEndpointVersions();

            // JSON Syntax Highlighter für den TextEditor laden.
            LoadJsonSyntaxHighlighter();

            // Test API Zugangsdaten.
            WfseApiServerTextBox.Text = "https://api.in-com.de";
            WfseClientIdTextBox.Text = "Yzk5N2E2YzkwODJmNGMyZmFjNWFiYzg4Yzg5ZGE0Mzc";
            WfseClientSecretTextBox.Text = "QWVZenZhMTJyaUJhNEg3VGU5Z0xMUHMxQWZBRzU5VEt3RUJsSVR1QlRCZ0FiTnJoajZTU2pwZVkrTzZMVFVUK3BiTUxDa3kwb0Z2SXF5TTlnWW9PV0E9PQ";
            WfseEndpointTextBox.Text = "graeber?friedhof=07";

            // RestClient initialisieren.
            _client = new RestClient(options =>
            {
                options.ServerAddress = WfseApiServerTextBox.Text;
                options.Credentials = new ClientCredentials()
                {
                    ClientId = WfseClientIdTextBox.Text,
                    ClientSecret = WfseClientSecretTextBox.Text
                };
                options.AuthenticationEvents = new AuthenticationEvents()
                {
                    OnAuthorized = OnAuthorized,
                    OnAuthorizationFailed = OnAuthorizationFailed
                };
                options.EndpointEvents = new EndpointEvents()
                {
                    OnRequestFailed = OnRequestFailed
                };
            });
        }

        /// <summary>
        /// Richtet das Tab "WinFriedSE" ein.
        /// </summary>
        private void InitHub()
        {
            // Test API Zugangsdaten.
            HubApiServerTextBox.Text = "https://api.in-com.de";
            HubClientIdTextBox.Text = "Zjg0OWI2ZWYyMmQwNDRiNWJhNzQ1YzEzNjUxZDE1NzE";
            HubClientSecretTextBox.Text = "LzVFS1duZ08rNEFvRjhnWXN4UGttZmlTMDNMK0NPa3g2eDBGWE9haHJxTmlqT1AzdEQya0JMYjJxSGlpczlialpZNzlkN0IvSW9qVUZlbkJuWnlZZGc9PQ";

            // RestClient initialisieren.
            _hubClient = new HubClient(options =>
            {
                options.Name = "IncomTestTool";
                options.ServerAddress = HubApiServerTextBox.Text;
                options.Credentials = new ClientCredentials()
                {
                    ClientId = HubClientIdTextBox.Text,
                    ClientSecret = HubClientSecretTextBox.Text
                };
                options.AuthenticationEvents = new AuthenticationEvents()
                {
                    OnAuthorized = OnHubAuthorized,
                    OnAuthorizationFailed = OnHubAuthorizationFailed
                };
                options.HubEvents = new HubEvents()
                {
                    OnConnected = OnConnected,
                    OnDisconnected = OnDisconnected,
                    OnApiVersion = OnApiVersion
                };
                options.TraceLevel = TraceLevels.All;
                options.TraceWriter = new Tracing.TextEditorWriter(HubTextEditor);
            });
        }

        /// <summary>
        /// Richtet die Controls der TabPage "Hub" ein.
        /// </summary>
        /// <param name="isAuthorized"></param>
        /// <param name="isConnected"></param>
        private void SetHubControls(bool isAuthorized, bool isConnected)
        {
            HubConnectionInfoTextBlock.Foreground = new SolidColorBrush(isAuthorized ? Colors.Green : Colors.Red);
            HubConnectionInfoTextBlock.Text = isAuthorized ? "Verbunden" : "Nicht verbunden!";
            HubApiServerTextBox.IsEnabled = !isAuthorized;
            HubClientIdTextBox.IsEnabled = !isAuthorized;
            HubClientSecretTextBox.IsEnabled = !isAuthorized;
            HubConnectButton.Visibility = isConnected ? Visibility.Collapsed : Visibility.Visible;
            HubDisconnectButton.Visibility = isConnected ? Visibility.Visible : Visibility.Collapsed;
            HubGetApiVersionButton.IsEnabled = isConnected;
        }

        /// <summary>
        /// Lädt die Syntax Highlighter Einstellungen des TextEditors.
        /// </summary>
        private void LoadJsonSyntaxHighlighter()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WpfSample.Resources.json_syntax.xshd"))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    WfseTextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    SearchPanel.Install(WfseTextEditor);
                }
            }
        }

        /// <summary>
        /// Setzt die möglichen Endpunkt Versionen in die ComboBox.
        /// </summary>
        private void SetEndpointVersions()
        {
            WfseEndpointVersionCombox.SelectedValuePath = "Key";
            WfseEndpointVersionCombox.DisplayMemberPath = "Value";
            WfseEndpointVersionCombox.Items.Add(new KeyValuePair<int, string>(1, "Version 1"));
            WfseEndpointVersionCombox.Items.Add(new KeyValuePair<int, string>(2, "Version 2"));
        }

        #endregion
    }
}
