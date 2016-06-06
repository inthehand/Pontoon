//-----------------------------------------------------------------------
// <copyright file="DeviceSelectedEventArgs.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Windows.Devices.Enumeration;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Provides data for the <see cref="DeviceSelected"/> event on the <see cref="DevicePicker"/> object.
    /// </summary>
    public sealed class DeviceSelectedEventArgs
    {
        /// <summary>
        /// The device selected by the user in the picker.
        /// </summary>
        /// <value>The selected device.</value>
        public Windows.Devices.Enumeration.DeviceInformation SelectedDevice
        {
            get;
            internal set;
        }
    }
}