//-----------------------------------------------------------------------
// <copyright file="GattCharacteristicProperties.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Specifies the values for the GATT characteristic properties as well as the GATT Extended Characteristic Properties Descriptor.
    /// </summary>
    [Flags]
    public enum GattCharacteristicProperties : uint
    { 
        None = 0,
        Broadcast = 1,
        Read = 2,
        WriteWithoutResponse = 4,
        Write = 8,
        Notify = 16,
        Indicate = 32,
        AuthenticatedSignedWrites = 64,
        ExtendedProperties = 128,
        ReliableWrites = 256,
        WriteableAuxiliaries = 512,
    }
}