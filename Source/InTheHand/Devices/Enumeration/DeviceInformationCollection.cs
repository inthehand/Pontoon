//-----------------------------------------------------------------------
// <copyright file="DeviceInformationCollection.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a picker flyout that contains a list of devices for the user to choose from.
    /// </summary>
    public sealed class DeviceInformationCollection : List<DeviceInformation>
    {
    }
}