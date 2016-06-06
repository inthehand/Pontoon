
//-----------------------------------------------------------------------
// <copyright file="DevicePickerDialog.xaml.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace InTheHand.Devices.Enumeration
{
    internal sealed partial class DevicePickerDialog : ContentDialog
    {
        private DevicePicker _owner;
        public DevicePickerDialog(DevicePicker owner)
        {
            this._owner = owner;
            this.Background = new SolidColorBrush(this._owner.Appearance.BackgroundColor);
            this.Foreground = new SolidColorBrush(this._owner.Appearance.ForegroundColor);
            this.Resources.Add("AccentBrush", new SolidColorBrush(this._owner.Appearance.AccentColor));
            this.Resources.Add("ForegroundBrush", new SolidColorBrush(this._owner.Appearance.ForegroundColor));
            this.Resources.Add("SelectedAccentBrush", new SolidColorBrush(this._owner.Appearance.SelectedAccentColor));
            this.Resources.Add("SelectedBackgroundBrush", new SolidColorBrush(this._owner.Appearance.SelectedBackgroundColor));
            this.Resources.Add("SelectedForegroundBrush", new SolidColorBrush(this._owner.Appearance.SelectedForegroundColor));
            
            this.InitializeComponent();

            this.Loaded += DevicePickerDialog_Loaded;
            this.Closed += DevicePickerDialog_Closed;
        }

        void DevicePickerDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (SelectedDevice == null)
            {
                _owner.OnDevicePickerDismissed();
            }
        }

        async void DevicePickerDialog_Loaded(object sender, RoutedEventArgs e)
        {
            List<DeviceViewModel> devices = new List<DeviceViewModel>();

            if (_owner.Filter.SupportedDeviceSelectors.Count > 0)
            {
                foreach (string selector in _owner.Filter.SupportedDeviceSelectors)
                {
                    IReadOnlyCollection<Windows.Devices.Enumeration.DeviceInformation> filteredDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(selector);
                    foreach(Windows.Devices.Enumeration.DeviceInformation info in filteredDevices)
                    {
                        devices.Add(new DeviceViewModel(info));
                    }
                }
            }
            else
            {
                foreach(Windows.Devices.Enumeration.DeviceInformation info in await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync())
                {
                    devices.Add(new DeviceViewModel(info));
                }
            }
            
            DeviceList.ItemsSource = devices;

            if(devices.Count == 0)
            {
                EmptyMessage.Visibility = Visibility.Visible;
            }
        }

        private Windows.Devices.Enumeration.DeviceInformation _selectedDevice;
        public Windows.Devices.Enumeration.DeviceInformation SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
        }

        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DeviceList.SelectedIndex > -1)
            {
                _selectedDevice = (DeviceList.SelectedItem as DeviceViewModel).Information;
                _owner.OnDeviceSelected(_selectedDevice);

                this.Hide();
            }
        }
    }
}