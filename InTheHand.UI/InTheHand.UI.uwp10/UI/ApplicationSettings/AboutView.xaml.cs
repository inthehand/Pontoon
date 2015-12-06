using InTheHand.UI.ApplicationSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace InTheHandUI
{
    internal sealed partial class AboutView : Grid
    {
        public AboutView()
        {
            InitializeComponent();

            AppIcon.Source = new BitmapImage(InTheHand.ApplicationModel.Package.Current.Logo);
            AppNameText.Text = InTheHand.ApplicationModel.Package.Current.DisplayName;
            Version.Text = "Version " + InTheHand.ApplicationModel.Package.Current.Id.Version.ToString();
            if (SettingsPane.GetForCurrentView().showPublisher)
            {
                AuthorText.Text = string.Format("By {0}", InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);
            }
            else
            {
                AuthorText.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            Description.Text = InTheHand.ApplicationModel.Package.Current.Description;
        }
    }
}
