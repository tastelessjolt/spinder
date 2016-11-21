using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private CancellationTokenSource cts;
        public LoginPage()
        {
            this.InitializeComponent();
            cts = new CancellationTokenSource();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                // TODO: Go back to the previous page
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame.CanGoBack)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            };
        }

        public Task Helpers { get; private set; }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            String user = usernameBox.Text;
            String pass = passwordBox.Password;
            HttpClient http = new HttpClient();

            var headers = http.DefaultRequestHeaders;

            Uri requestUri = new Uri("http://10.196.13.214:8000/api/login/");

            HttpResponseMessage response = new HttpResponseMessage();
            string httpResponseBody = "";

            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("username", user);
            dict.Add("password", pass);
            try
            {
                IHttpContent formData = new HttpFormUrlEncodedContent(dict);
                response = await http.PostAsync(requestUri, formData).AsTask();
                httpResponseBody = await response.Content.ReadAsStringAsync();
                var jsonObj = JsonObject.Parse(httpResponseBody);

                if (jsonObj.ContainsKey("success")){
                    httpResponseBody = "fail";
                    Logging.Text = "Wrong username or password";
                }
                else {
                    Logging.Text = jsonObj.GetNamedArray("interests").ToString();
                }

                if (httpResponseBody != "fail")
                {
                    this.Frame.Navigate(typeof(HomePage), httpResponseBody);
                }
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error : " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }

        private void field_Changed(object sender, RoutedEventArgs e)
        {
            Logging.Text = "";
        }
    }
}
