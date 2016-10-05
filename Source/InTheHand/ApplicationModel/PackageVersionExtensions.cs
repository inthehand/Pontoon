// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using Windows.ApplicationModel;

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Provides interoperability between Windows Runtime <see cref="PackageVersion"/> and .NET <see cref="Version"/> types.
    /// </summary>
    public static class PackageVersionExtensions
    {
        internal static PackageVersion ToPackageVersion(this Version version)
        {
            PackageVersion packageVersion = new PackageVersion();
            packageVersion.Major = (ushort)version.Major;
            packageVersion.Minor = (ushort)version.Minor;
            packageVersion.Build = (ushort)version.Build;
            packageVersion.Revision = (ushort)version.Revision;
            return packageVersion;
        }

        /// <summary>
        /// Converts the value to a <see cref="Version"/>.
        /// </summary>
        /// <param name="packageVersion"></param>
        /// <returns></returns>
        public static Version ToVersion(this PackageVersion packageVersion)
        {
            return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }

        /// <summary>
        /// Converts the value of the current Version object to its equivalent String representation.
        /// A specified count indicates the number of components to return.
        /// </summary>
        /// <param name="packageVersion"></param>
        /// <param name="fieldCount">The number of components to return. The fieldCount ranges from 0 to 4.</param>
        /// <returns>The String representation of the values of the major, minor, build, and revision components of the current Version object, each separated by a period character ('.').
        /// The fieldCount parameter determines how many components are returned.</returns>
        public static string ToString(this PackageVersion packageVersion, int fieldCount)
        {
            return packageVersion.ToVersion().ToString(fieldCount);
        }
    }
}
