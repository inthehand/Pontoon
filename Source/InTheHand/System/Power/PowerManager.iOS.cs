// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerManager.iOS.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Foundation;
using System;
using UIKit;

namespace InTheHand.System.Power
{
    partial class PowerManager
    {
        private static UIKit.UIDevice _device;
        private static bool _isSimulator;

        static PowerManager()
        {
            _device = UIDevice.CurrentDevice;
            if (_device.Model == "iPhone Simulator")
            {
                _isSimulator = true;
            }
            else
            {
                _device.BatteryMonitoringEnabled = true;
            }
        }

        private static BatteryStatus GetBatteryStatus()
        {
            switch (_device.BatteryState)
            {
                case UIKit.UIDeviceBatteryState.Charging:
                    return BatteryStatus.Charging;

                case UIKit.UIDeviceBatteryState.Unplugged:
                    return BatteryStatus.Discharging;

                case UIKit.UIDeviceBatteryState.Full:
                    return BatteryStatus.Idle;

                default:
                    return BatteryStatus.NotPresent;

            }
        }

        private static EnergySaverStatus GetEnergySaverStatus()
        {
            return NSProcessInfo.ProcessInfo.LowPowerModeEnabled ? EnergySaverStatus.On : EnergySaverStatus.Off;
        }

        private static PowerSupplyStatus GetPowerSupplyStatus()
        {
            return _device.BatteryState == UIKit.UIDeviceBatteryState.Charging ? PowerSupplyStatus.Adequate : PowerSupplyStatus.NotPresent;
        }

        private static int GetRemainingChargePercent()
        {
            if (_isSimulator)
            {
                return 100;
            }

            if (_device.BatteryLevel < 0.0f)
            {
                return 0;
            }

            double percent = _device.BatteryLevel;
            return Convert.ToInt32(percent * 100f);
        }

        private static void RemainingChargePercentChangedAdd()
        {
            if (!_isSimulator)
            {
                UIDevice.Notifications.ObserveBatteryLevelDidChange(BatteryLevelDidChangeHandler);
            }
        }

        private static void RemainingChargePercentChangedRemove()
        {
        }

        private static void BatteryLevelDidChangeHandler(object sender, NSNotificationEventArgs e)
        {
            _remainingChargePercentChanged?.Invoke(null, null);
        }
    }
}