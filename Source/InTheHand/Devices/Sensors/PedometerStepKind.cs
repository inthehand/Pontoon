// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PedometerStepKind.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// The type of step taken according to the pedometer.
    /// </summary>
    public enum PedometerStepKind
    {
        /// <summary>
        /// An unknown step type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A walking step.
        /// </summary>
        Walking = 1,

        /// <summary>
        /// A running step.
        /// </summary>
        Running = 2,
    }

#if TIZEN
    internal static class PedometerStepKindHelper
    {
        public static PedometerStepKind FromState(Tizen.Sensor.PedometerState s)
        {
            switch(s)
            {
                case Tizen.Sensor.PedometerState.Run:
                    return PedometerStepKind.Running;

                case Tizen.Sensor.PedometerState.Walk:
                    return PedometerStepKind.Walking;

                default:
                    return PedometerStepKind.Unknown;
            }
        }
    }
#endif
}
