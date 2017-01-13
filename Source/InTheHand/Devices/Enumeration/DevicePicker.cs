//-----------------------------------------------------------------------
// <copyright file="DevicePicker.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a picker flyout that contains a list of devices for the user to choose from.
    /// </summary>
    public sealed class DevicePicker
    {
#if WINDOWS_UWP
        private Windows.Devices.Enumeration.DevicePicker _picker = new Windows.Devices.Enumeration.DevicePicker();

        private void _picker_DeviceSelected(Windows.Devices.Enumeration.DevicePicker sender, Windows.Devices.Enumeration.DeviceSelectedEventArgs args)
        {
            OnDeviceSelected(args.SelectedDevice);
        }

        private void _picker_DevicePickerDismissed(Windows.Devices.Enumeration.DevicePicker sender, object args)
        {
            OnDevicePickerDismissed();
        }

#elif WINDOWS_PHONE_APP
        private DevicePickerDialog _dialog;
#elif WINDOWS_APP
        private Popup _popup;
#endif

#if !WINDOWS_UWP
        private DevicePickerAppearance _appearance = new DevicePickerAppearance();   
        private DevicePickerFilter _filter = new DevicePickerFilter();
        
#endif

        /// <summary>
        /// Creates a <see cref="DevicePicker"/> object.
        /// </summary>
        public DevicePicker()
        {
#if WINDOWS_UWP
            _picker.DevicePickerDismissed += _picker_DevicePickerDismissed;
            _picker.DeviceSelected += _picker_DeviceSelected;
#endif
        }




        /// <summary>
        /// Indicates that the device picker was light dismissed by the user.
        /// Light dismiss happens when the user clicks somewhere other than the picker UI and the picker UI disappears. 
        /// On Windows Phone this indicates the Back button was pressed.
        /// </summary>
        public event TypedEventHandler<DevicePicker, Object> DevicePickerDismissed;

        // raises the DevicePickerDismissed event
        internal void OnDevicePickerDismissed()
        {
            DevicePickerDismissed?.Invoke(this, null);
        }


        /// <summary>
        /// Indicates that the user selected a device in the picker.
        /// </summary>
        public event TypedEventHandler<DevicePicker, DeviceSelectedEventArgs> DeviceSelected;

        // Raises the DeviceSelected event
        internal void OnDeviceSelected(DeviceInformation device)
        {
            DeviceSelected?.Invoke(this, new DeviceSelectedEventArgs() { SelectedDevice = device });
        }

        
        /// <summary>
        /// Gets the colors of the picker.
        /// </summary>
        /// <value>The color of the picker.</value>
        public DevicePickerAppearance Appearance
        {
            get
            {
#if WINDOWS_UWP
                return _picker.Appearance;
#else
                return _appearance;
#endif
            }
        }

        /// <summary>
        /// Gets the filter used to choose what devices to show in the picker.
        /// </summary>
        public DevicePickerFilter Filter
        {
            get
            {
#if WINDOWS_UWP
                return _picker.Filter;
#else                
                return _filter;
#endif
            }
        }


        /// <summary>
        /// Hides the picker.
        /// </summary>
        public void Hide()
        {
#if WINDOWS_PHONE_APP
            if(_dialog == null)
            {
                throw new InvalidOperationException("Picker is not shown");
            }

            _dialog.Hide();
#else
#endif
        }

        /// <summary>
        /// Shows the picker UI and returns the selected device; does not require you to register for an event.
        /// </summary>
        /// <param name="selection">The rectangle from which you want the picker to fly out.
        /// Ignored on Windows Phone.</param>
        /// <returns></returns>
        public Task<DeviceInformation> PickSingleDeviceAsync(Rect selection)
        {
            return PickSingleDeviceAsync(selection, Placement.Default);
        }

        /// <summary>
        /// Shows the picker UI and returns the selected device; does not require you to register for an event.
        /// </summary>
        /// <param name="selection">The rectangle from which you want the picker to fly out.
        /// Ignored on Windows Phone.</param>
        /// <param name="placement">The edge of the rectangle from which you want the picker to fly out.
        /// Ignored on Windows Phone.</param>
        /// <returns></returns>
        public async Task<DeviceInformation> PickSingleDeviceAsync(Rect selection, Placement placement)
        {
#if WINDOWS_UWP
            return await _picker.PickSingleDeviceAsync(selection, (Windows.UI.Popups.Placement)((int)placement)).AsTask();
#elif WINDOWS_PHONE_APP
            _dialog = new DevicePickerDialog(this);
            Windows.UI.Xaml.Controls.ContentDialogResult result = await _dialog.ShowAsync();
            return _dialog.SelectedDevice;
#elif WINDOWS_APP
            _popup = new Popup();
            DevicePickerControl dpc = new DevicePickerControl(this, _popup);
            _popup.Child = dpc;
            _popup.HorizontalOffset = selection.Right;
            _popup.VerticalOffset = selection.Top;
            _popup.IsLightDismissEnabled = true;
            _popup.IsOpen = true;
            return await Task.Run<DeviceInformation>(() => { return dpc.WaitForSelection(); });
#endif
        }

        /// <summary>
        /// Shows the picker UI.
        /// </summary>
        /// <param name="selection">The rectangle from which you want the picker to fly out.
        /// Ignored on Windows Phone.</param>
        public void Show(Rect selection)
        {
            Show(selection, Placement.Default);
        }

        /// <summary>
        /// Shows the picker UI. 
        /// </summary>
        /// <param name="selection">The rectangle from which you want the picker to fly out.
        /// Ignored on Windows Phone.</param>
        /// <param name="placement">The edge of the rectangle from which you want the picker to fly out.
        /// Ignored on Windows Phone.</param>
        public void Show(Rect selection, Placement placement)
        {
#if WINDOWS_PHONE_APP
            _dialog = new DevicePickerDialog(this);
            _dialog.ShowAsync();
#elif WINDOWS_APP
            _popup = new Popup();
            _popup.Child = new DevicePickerControl(this, _popup);
            _popup.HorizontalOffset = selection.Right;
            _popup.VerticalOffset = selection.Top;
            _popup.IsOpen = true;
#endif
        }
    }
}