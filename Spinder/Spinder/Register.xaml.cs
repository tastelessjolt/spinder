using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
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

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            String user = username.Text;
            String pass = password.Password;
            String firstname = first_name.Text;
            String lastname = last_name.Text;

            HttpClient http = new HttpClient();

            var headers = http.DefaultRequestHeaders;

            Uri requestUri = new Uri("http://10.196.13.214:8000/api/register/");

            HttpResponseMessage response = new HttpResponseMessage();
            string httpResponseBody = "";

            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("username", user);
            dict.Add("password", pass);
            dict.Add("first_name", firstname);
            dict.Add("last_name", lastname);
            try
            {
                IHttpContent formData = new HttpFormUrlEncodedContent(dict);
                response = await http.PostAsync(requestUri, formData).AsTask();
                httpResponseBody = await response.Content.ReadAsStringAsync();
                JsonObject jsonObj = JsonObject.Parse(httpResponseBody);
                if (jsonObj.GetNamedBoolean("success"))
                {
                    Frame rootFrame = Window.Current.Content as Frame;

                    if (rootFrame.CanGoBack)
                    {
                        rootFrame.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error : " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (password.Password == "Password")
            {
                status.Text = "'Password' is not allowed as a password.";
            }
            else
            {
                status.Text = string.Empty;
            }
        }


    }
}
