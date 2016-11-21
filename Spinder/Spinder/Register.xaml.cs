using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            String user = username.Text;
            String pass = password.Password;
            String firstname = first_name.Text;
            String lastname = last_name.Text;
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
