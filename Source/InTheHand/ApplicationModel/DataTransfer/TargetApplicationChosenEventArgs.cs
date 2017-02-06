//-----------------------------------------------------------------------
// <copyright file="TargetApplicationChosenEventArgs.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Contains information about the target app the user chose to share content with.
    /// To get this object, you must handle the TargetApplicationChosen event.
    /// </summary>
    public sealed class TargetApplicationChosenEventArgs
    {
        internal TargetApplicationChosenEventArgs(string applicationName)
        {
            ApplicationName = applicationName;
        }

        /// <summary>
        /// Contains the name of the app that the user chose to share content with.
        /// </summary>
        public string ApplicationName { get; private set; }
    }
}