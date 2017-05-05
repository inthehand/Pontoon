//-----------------------------------------------------------------------
// <copyright file="DeviceUnpairingResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Contains information about the result of attempting to unpair a device.
    /// </summary>
    public sealed class DeviceUnpairingResult
    {
#if __ANDROID__
        private DeviceUnpairingResultStatus _status;

        internal DeviceUnpairingResult()
        {
            _status = DeviceUnpairingResultStatus.Failed;
        }

#elif WINDOWS_UWP
        private Windows.Devices.Enumeration.DeviceUnpairingResult _result;

        private DeviceUnpairingResult(Windows.Devices.Enumeration.DeviceUnpairingResult result)
        {
            _result = result;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceUnpairingResult(DeviceUnpairingResult result)
        {
            return result._result;
        }

        public static implicit operator DeviceUnpairingResult(Windows.Devices.Enumeration.DeviceUnpairingResult result)
        {
            return new DeviceUnpairingResult(result);
        }

#elif WIN32
        private DeviceUnpairingResultStatus _status;

        internal DeviceUnpairingResult(int error)
        {
            switch(error)
            {
                case 0:
                    _status = DeviceUnpairingResultStatus.Unpaired;
                    break;

                default:
                    _status = DeviceUnpairingResultStatus.Failed;
                    break;
            }
        }
#endif

            /// <summary>
            /// Gets the paired status of the device after the unpairing action completed.
            /// </summary>
            /// <value>The paired status of the device.</value>
        public DeviceUnpairingResultStatus Status
        {
            get
            {
#if WINDOWS_UWP
                return (DeviceUnpairingResultStatus)((int)_result.Status);
#elif WIN32
                return _status;
#else
                return DeviceUnpairingResultStatus.Failed;
#endif
            }
        }


    }

    /// <summary>
    /// The result of the unpairing action.
    /// </summary>
    public enum DeviceUnpairingResultStatus
    {
        /// <summary>
        /// The device object is successfully unpaired.
        /// </summary>
        Unpaired = 0,

        /// <summary>
        /// The device object was already unpaired.
        /// </summary>
        AlreadyUnpaired = 1,

        /// <summary>
        /// The device object is currently in the middle of either a pairing or unpairing action.
        /// </summary>
        OperationAlreadyInProgress = 2,

        /// <summary>
        /// The caller does not have sufficient permissions to unpair the device.
        /// </summary>
        AccessDenied = 3,

        /// <summary>
        /// An unknown failure occurred.
        /// </summary>
        Failed = 4,
    }
}