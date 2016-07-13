//-----------------------------------------------------------------------
// <copyright file="ApplicationDataLocality.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Storage
{
    /// <summary>
    /// Provides access to the application data store.
    /// Application data consists of settings that are local.
    /// </summary>
    public enum ApplicationDataLocality
    {
       Local,
       LocalCache,
       Roaming,
       Temporary,
    }
}
