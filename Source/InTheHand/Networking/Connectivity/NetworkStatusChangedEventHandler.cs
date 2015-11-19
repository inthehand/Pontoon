//-----------------------------------------------------------------------
// <copyright file="NetworkInformation.cs" company="In The Hand Ltd">
//     Copyright © 2015 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Networking.Connectivity
{
    /// <summary>
    /// Represents the method that handles network status change notifications.
    /// This method is called when any properties exposed by the <see cref="NetworkInformation"/> object changes while the app is active.
    /// </summary>
    /// <param name="sender"></param>
    public delegate void NetworkStatusChangedEventHandler(object sender);
}
