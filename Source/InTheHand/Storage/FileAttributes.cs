//-----------------------------------------------------------------------
// <copyright file="FileAttributes.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Storage
{
    /// <summary>
    /// Describes the attributes of a file or folder.
    /// </summary>
    [Flags]
    public enum FileAttributes : int
    {
        /// <summary>
        /// The item is normal.
        /// That is, the item doesn't have any of the other values in the enumeration.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The item is read-only.
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// The item is a directory.
        /// </summary>
        Directory = 16,

        /// <summary>
        /// The item is archived.
        /// </summary>
        Archive = 32,

        /// <summary>
        /// The item is a temporary file.
        /// </summary>
        Temporary = 256,
    }
}