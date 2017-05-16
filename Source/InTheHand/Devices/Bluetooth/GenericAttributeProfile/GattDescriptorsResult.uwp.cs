//-----------------------------------------------------------------------
// <copyright file="GattDescriptorsResult.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptorsResult
    {
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptorsResult _result;

        /// <summary>
        /// Gets a vector of the GATT descriptors.
        /// </summary>
        private IReadOnlyList<GattDescriptor> GetDescriptors()
        {
            return _result.Descriptors;
        }

        /// <summary>
        /// Gets the GATT protocol error.
        /// </summary>
        public byte? ProtocolError { get; }

        /// <summary>
        /// Gets the status of the operation.
        /// </summary>
        public GattCommunicationStatus Status { get; }
    }
}