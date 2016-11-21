using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        JsonObject userdata;
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var data = e.Parameter.ToString();
            userdata = JsonObject.Parse(data);
            nametitle.Text = userdata.GetNamedString("first_name") + " " + userdata.GetNamedString("last_name") + "(" + userdata.GetNamedString("username") + ")";
            var interests = userdata.GetNamedArray("interests");
            if (interests.Count > 0)
            {
                foreach (JsonValue x in interests)
                {
                    JsonObject y = x.GetObject();
                    ListViewItem item = new ListViewItem();
                    item.FontSize = 20;
                    item.Content = y.GetNamedObject("fields").GetNamedString("name");
                    // item.Content = "";
                    // item.Name = "" + y.GetNamedNumber("pk");
                    interestList.Items.Add(item);
                }
            }
            else
            {
                
            }
        }

        private void hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            mysplitview1.IsPaneOpen = !mysplitview1.IsPaneOpen;
        }

        private async void interestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            ListViewItem li = (ListViewItem) lb.SelectedItem;
            //int pk = int.Parse(li.Name);
            int pk = 1;
            // get people corresponding to sport pk
            try
            {
                Uri baseUri = new Uri("http://10.196.13.214:8000");
                // Uri uri = new Uri("http://10.196.13.214:8000/api/matches/" + pk + "/");
                var cookieContainer = new CookieContainer();
                string response = "";
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = baseUri })
                {
                    cookieContainer.Add(baseUri, new Cookie("Cookie Name", "Cookie Value"));
                    var httpResponse = await client.GetAsync("/api/matches/" + pk + "/");
                    response = await httpResponse.Content.ReadAsStringAsync();
                }
                var jsonArray = JsonArray.Parse(response);
                
                peopleList.Visibility = 0;
                peopleList.Items.Clear();
                // cleared and now add people list
                if (jsonArray.Count > 0)
                {
                    foreach (var value in jsonArray)
                    {
                        var jsonObj = value.GetObject();
                    }
                }
                else
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = "No people found near you";
                    peopleList.Items.Add(lbi);
                }
            }
            catch(Exception ex)
            {
                var err = ex.Message.ToString();
            }
        }
    }
}
