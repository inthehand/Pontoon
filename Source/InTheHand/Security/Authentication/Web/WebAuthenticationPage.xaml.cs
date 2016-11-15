//-----------------------------------------------------------------------
// <copyright file="WebAuthenticationPage.xaml.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using InTheHand.Security.Authentication.Web;

namespace InTheHandSecurity.Authentication.Web
{
    internal partial class WebAuthenticationPage : PhoneApplicationPage
    {
        private string redirectUri;

        public WebAuthenticationPage()
        {
            InitializeComponent();

            Browser.Navigating += Browser_Navigating;
            Browser.Navigated += Browser_Navigated;
            Browser.NavigationFailed += Browser_NavigationFailed;
        }

        void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            Progress.IsIndeterminate = false;
            Progress.Visibility = Visibility.Collapsed;
            Icon.Visibility = Visibility.Visible;
            AppName.Visibility = Visibility.Visible;
            Browser.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Browser.Navigating -= this.Browser_Navigating;
            Browser.NavigationFailed -= this.Browser_NavigationFailed;

            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Icon.Source = new BitmapImage(InTheHand.ApplicationModel.Package.Current.Logo);
            AppName.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;

            redirectUri = NavigationContext.QueryString["returnUri"];
            Browser.Navigate(new Uri(this.NavigationContext.QueryString["uri"]));
            base.OnNavigatedTo(e);
        }        
        
        void Browser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Progress.IsIndeterminate = false;
            Progress.Visibility = Visibility.Collapsed;

            WebAuthenticationBroker.Result = new WebAuthenticationResult(e.Exception == null ? "" : e.Exception.Message, e.Exception == null ? 0 : (uint)e.Exception.HResult, WebAuthenticationStatus.ErrorHttp);

            NavigationService.GoBack();
            WebAuthenticationBroker.Handle.Set(); 
        }

        void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.OriginalString.Contains(redirectUri))
            {
                WebAuthenticationBroker.Result = new WebAuthenticationResult(e.Uri.OriginalString, 0, e.Uri.OriginalString.Contains("error=") ? WebAuthenticationStatus.UserCancel : WebAuthenticationStatus.Success);

                NavigationService.GoBack();
                WebAuthenticationBroker.Handle.Set(); 
            }
        }
    }
}