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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InTheHand.ApplicationModel.DataTransfer;
using InTheHand.UI.Popups;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(InTheHand.Environment.OSVersion.VersionString);

            MessageDialog md = new MessageDialog("Content goes here", "Title goes here");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }, "one"));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }, "two"));
            md.Commands.Add(new UICommand("Three", (c) => { System.Diagnostics.Debug.WriteLine("Three"); }, "three"));
            //md.DefaultCommandIndex = 2;
            //md.CancelCommandIndex = 0;
            var cmd = await md.ShowAsync();

            await InTheHand.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
            //InTheHand.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.ProgressValue = 50;
        }
    }
}
