//-----------------------------------------------------------------------
// <copyright file="DevicePickerFilter.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents the filter used to determine which devices to show in the device picker.
    /// The filter parameters are OR-ed together to build the resulting filter.
    /// </summary>
    public sealed class DevicePickerFilter
    {
        private List<string> _supportedDeviceSelectors = new List<string>();

        /// <summary>
        /// Gets a list of AQS filter strings.
        /// This defaults to empty list (no filter).
        /// You can add one or more AQS filter strings to this vector and filter the devices list to those that meet one or more of the provided filters.
        /// </summary>
        public IList<string> SupportedDeviceSelectors
        {
            get
            {
                return _supportedDeviceSelectors;
            }
        }
    }
}