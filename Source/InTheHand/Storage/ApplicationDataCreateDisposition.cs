//-----------------------------------------------------------------------
// <copyright file="ApplicationDataCreateDisposition.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Storage
{
    /// <summary>
    /// Specifies options for creating application data containers or returning existing containers.
    /// <para>This enumeration is used by the <see cref="ApplicationDataContainer.CreateContainer"/> method.</para>
    /// </summary>
    public enum ApplicationDataCreateDisposition
    {
        /// <summary>
        /// Always returns the specified container.
        /// Creates the container if it does not exist.
        /// </summary>
        Always = 0,

        /// <summary>
        /// Returns the specified container only if it already exists.
        /// Raises an exception of type System.Exception if the specified container does not exist.
        /// </summary>
        Existing = 1,
    }
}