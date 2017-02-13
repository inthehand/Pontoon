//-----------------------------------------------------------------------
// <copyright file="Radio.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using System.Collections.Generic;

namespace InTheHand.Devices.Radios
{
    public sealed partial class Radio
    {
        private CBCentralManager _manager;

        internal Radio(CBCentralManager manager)
        {
            _manager = manager;
        }

        private static void GetRadiosAsyncImpl(List<Radio> radios)
        {
            radios.Add(new Radio(new CBCentralManager()));
        }
        
        // only supporting Bluetooth radio
        private RadioKind GetKindImpl()
        {
            return RadioKind.Bluetooth;
        }

        // matching the UWP behaviour (although we could have used the radio name)
        private string GetNameImpl()
        {
            return "Bluetooth";
        }

        private RadioState GetStateImpl()
        {
            try
            {
                return CBCentalManagerStateToRadioState(_manager.State);
            }
            catch
            {
                return RadioState.Unknown;
            }
        }
        
        private static RadioState CBCentalManagerStateToRadioState(CBCentralManagerState state)
        {
            switch (state)
            {
                case CBCentralManagerState.PoweredOn:
                    return RadioState.On;

                case CBCentralManagerState.PoweredOff:
                    return RadioState.Off;

                case CBCentralManagerState.Unauthorized:
                    return RadioState.Disabled;

                default:
                    return RadioState.Unknown;
            }
        }
    }
}