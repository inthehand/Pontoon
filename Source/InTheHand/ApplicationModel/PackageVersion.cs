// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.PackageVersion))]
#else
using System;

namespace Windows.ApplicationModel
{
    /// <summary>
    /// Represents the package version info.
    /// </summary>
    [CLSCompliant(false)]
    public struct PackageVersion
    {
        /// <summary>
        /// The major version number of the package.
        /// </summary>
        public ushort Major;
        /// <summary>
        /// The minor version number of the package.
        /// </summary>
        public ushort Minor;
        /// <summary>
        /// The build version number of the package.
        /// </summary>
        public ushort Build;
        /// <summary>
        /// The revision version number of the package.
        /// </summary>
        public ushort Revision;
    }
}
#endif