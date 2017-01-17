//-----------------------------------------------------------------------
// <copyright file="GattCharacteristicProperties.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
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

#if __UNIFIED__
    internal static class GattCharacteristicPropertiesHelper
    {
        public static GattCharacteristicProperties ToGattCharacteristicProperties(this CoreBluetooth.CBCharacteristicProperties value)
        {
            GattCharacteristicProperties p = GattCharacteristicProperties.None;

            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.AuthenticatedSignedWrites))
            {
                p |= GattCharacteristicProperties.AuthenticatedSignedWrites;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Broadcast))
            {
                p |= GattCharacteristicProperties.Broadcast;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.ExtendedProperties))
            {
                p |= GattCharacteristicProperties.ExtendedProperties;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Indicate))
            {
                p |= GattCharacteristicProperties.Indicate;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Notify))
            {
                p |= GattCharacteristicProperties.Notify;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Read))
            {
                p |= GattCharacteristicProperties.Read;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Write))
            {
                p |= GattCharacteristicProperties.Write;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.WriteWithoutResponse))
            {
                p |= GattCharacteristicProperties.WriteWithoutResponse;
            }

            return p;
        }

        public static CoreBluetooth.CBCharacteristicProperties ToCBCharacteristicProperties(this GattCharacteristicProperties value)
        {
            CoreBluetooth.CBCharacteristicProperties p =  (CoreBluetooth.CBCharacteristicProperties)0;

            if(value.HasFlag(GattCharacteristicProperties.AuthenticatedSignedWrites))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.AuthenticatedSignedWrites;
            }
            if (value.HasFlag(GattCharacteristicProperties.Broadcast))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Broadcast;
            }
            if (value.HasFlag(GattCharacteristicProperties.ExtendedProperties))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.ExtendedProperties;
            }
            if (value.HasFlag(GattCharacteristicProperties.Indicate))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Indicate;
            }
            if (value.HasFlag(GattCharacteristicProperties.Notify))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Notify;
            }
            if (value.HasFlag(GattCharacteristicProperties.Read))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Read;
            }
            if (value.HasFlag(GattCharacteristicProperties.Write))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Write;
            }
            if (value.HasFlag(GattCharacteristicProperties.WriteWithoutResponse))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.WriteWithoutResponse;
            }

            return p;
        }
    }
#endif
}