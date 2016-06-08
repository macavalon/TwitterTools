using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Diagnostics;

using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        // This method shows you how to create Application credentials. 
        // This type of credentials do not take a AccessKey or AccessSecret.
        private ITemporaryCredentials CreateApplicationCredentials(string consumerKey, string consumerSecret)
        {
            return CredentialsCreator.GenerateApplicationCredentials(consumerKey, consumerSecret);
        }

        // We need the user to go on the Twitter Website to authorize our application
        private void GoToTwitterCaptchaPage(ITemporaryCredentials applicationCredentials)
        {
            var url = CredentialsCreator.GetAuthorizationURL(applicationCredentials);
            Console.WriteLine("Please go on {0}, to accept the application and get the captcha.", url);
            Process.Start(url.ToString());
        }

        // On the Twitter website the user will get a captcha that he will need to enter in the application
        private void GenerateCredentialsAndLogin(string captcha, ITemporaryCredentials applicationCredentials)
        {
            var newCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(captcha, applicationCredentials);

            // And now the user is logged in!
            TwitterCredentials.SetCredentials(newCredentials);

            var loggedUser = User.GetLoggedUser();
            Console.WriteLine("You are logged as {0}", loggedUser.ScreenName);

            if (loggedUser.ScreenName != "")
            {
                CurrentStatusCount.Text = "Authorised";
                accesskey.Text = newCredentials.AccessToken;
                accesskeySecret.Text = newCredentials.AccessTokenSecret;
            }
            else
            {
                CurrentStatusCount.Text = "Not Authorised";
                accesskey.Text = "";
                accesskeySecret.Text = "";
            }
        }


        private void AuthoriseClick(object sender, RoutedEventArgs e)
        {
            GenerateCredentialsAndLogin(verifier.Text, applicationCredentials);

        }


        ITemporaryCredentials applicationCredentials;

        private void GetPinClick(object sender, RoutedEventArgs e)
        {
            applicationCredentials = CreateApplicationCredentials(consumerkey.Text, consumerkeySecret.Text);

            GoToTwitterCaptchaPage(applicationCredentials);

        }

    }
}
