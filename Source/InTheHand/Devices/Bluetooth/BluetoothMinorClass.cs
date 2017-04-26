//-----------------------------------------------------------------------
// <copyright file="BluetoothMinorClass.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Indicates the Minor Class code of the device, which is the general family of device with which the device is associated.
    /// </summary>
    public enum BluetoothMinorClass
    {
        /// <summary>
        /// Use when a Minor Class code has not been assigned.
        /// </summary>
        Uncategorized = 0,

        /// <summary>
        /// A desktop computer.
        /// </summary>
        ComputerDesktop = 1,
        /// <summary>
        /// A server computer.
        /// </summary>
        ComputerServer = 2,
        /// <summary>
        /// A laptop computer.
        /// </summary>
        ComputerLaptop = 3,
        /// <summary>
        /// A handheld PC/PDA.
        /// </summary>
        ComputerHandheld = 4,
        /// <summary>
        /// A palm-sized PC/PDA.
        /// </summary>
        ComputerPalmSize = 5,
        /// <summary>
        /// A wearable, watch-sized, computer.
        /// </summary>
        ComputerWearable = 6,
        /// <summary>
        /// A tablet computer.
        /// </summary>
        ComputerTablet = 7,

        /// <summary>
        /// A cell phone.
        /// </summary>
        PhoneCellular = 1,
        /// <summary>
        /// A cordless phone.
        /// </summary>
        PhoneCordless = 2,
        /// <summary>
        /// A smartphone.
        /// </summary>
        PhoneSmartPhone = 3,
        /// <summary>
        /// A wired modem or voice gateway.
        /// </summary>
        PhoneWired = 4,
        /// <summary>
        /// Common ISDN access.
        /// </summary>
        PhoneIsdn = 5,

        /// <summary>
        /// Fully available.
        /// </summary>
        NetworkFullyAvailable = 0,
        /// <summary>
        /// 1% to 17% utilized.
        /// </summary>
        NetworkUsed01To17Percent = 8,
        /// <summary>
        /// 17% to 33% utilized.
        /// </summary>
        NetworkUsed17To33Percent = 16,
        /// <summary>
        /// 33% to 50% utilized.
        /// </summary>
        NetworkUsed33To50Percent = 24,
        /// <summary>
        /// 50% to 67% utilized.
        /// </summary>
        NetworkUsed50To67Percent = 32,
        /// <summary>
        /// 67% to 83% utilized.
        /// </summary>
        NetworkUsed67To83Percent = 40,
        /// <summary>
        /// 83% to 99% utilized.
        /// </summary>
        NetworkUsed83To99Percent = 48,
        /// <summary>
        /// Network service is not available.
        /// </summary>
        NetworkUsedNoServiceAvailable = 56,

        
        AudioVideoWearableHeadset = 1,
        AudioVideoHandsFree = 2,
        AudioVideoMicrophone = 4,
        AudioVideoLoudspeaker = 5,
        AudioVideoHeadphones = 6,
        AudioVideoPortableAudio = 7,
        AudioVideoCarAudio = 8,
        AudioVideoSetTopBox = 9,
        AudioVideoHiFiAudioDevice = 10,
        AudioVideoVcr = 11,
        AudioVideoVideoCamera = 12,
        AudioVideoCamcorder = 13,
        AudioVideoVideoMonitor = 14,
        AudioVideoVideoDisplayAndLoudspeaker = 15,
        AudioVideoVideoConferencing = 16,
        AudioVideoGamingOrToy = 18,

   
        PeripheralJoystick = 1,
        PeripheralGamepad = 2,
        PeripheralRemoteControl = 3,
        PeripheralSensing = 4,
        PeripheralDigitizerTablet = 5,
        PeripheralCardReader = 6,
        PeripheralDigitalPen = 7,
        PeripheralHandheldScanner = 8,
        PeripheralHandheldGesture = 9,

        ImagingDisplay = 4,
        ImagingCamera = 8,
        ImagingScanner = 16,
        ImagingPrinter = 32,
        
        WearableWristwatch = 1,
        WearablePager = 2,
        WearableJacket = 3,
        WearableHelmet = 4,
        WearableGlasses = 5,
        
        ToyRobot = 1,
        ToyVehicle = 2,
        ToyDoll = 3,
        ToyController = 4,
        ToyGame = 5,

        HealthBloodPressureMonitor = 1,
        HealthThermometer = 2,
        HealthWeighingScale = 3,
        HealthGlucoseMeter = 4,
        HealthPulseOximeter = 5,
        HealthHeartRateMonitor = 6,
        HealthHealthDataDisplay = 7,
        HealthStepCounter = 8,
        HealthBodyCompositionAnalyzer = 9,
        HealthPeakFlowMonitor = 10,
        HealthMedicationMonitor = 11,
        HealthKneeProsthesis = 12,
        HealthAnkleProsthesis = 13,
        HealthGenericHealthManager = 14,
        HealthPersonalMobilityDevice = 15,

    }
}