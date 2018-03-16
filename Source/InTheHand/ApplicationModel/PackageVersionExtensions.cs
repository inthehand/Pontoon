// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-18 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Windows.ApplicationModel;

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Provides interoperability between Windows Runtime <see cref="PackageVersion"/> and .NET <see cref="Version"/> types.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public static class PackageVersionExtensions
    {
        /// <summary>
        /// Converts a <see cref="Version"/> to a PackageVersion.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static PackageVersion ToPackageVersion(this Version version)
        {
            PackageVersion packageVersion = new PackageVersion();
            packageVersion.Major = (ushort)version.Major;
            packageVersion.Minor = (ushort)version.Minor;

            if (version.Build != -1)
            {
                packageVersion.Build = (ushort)version.Build;
            }

            if (version.Revision != -1)
            {
                packageVersion.Revision = (ushort)version.Revision;
            }

            return packageVersion;
        }

        /// <summary>
        /// Converts the value to a <see cref="Version"/>.
        /// </summary>
        /// <param name="packageVersion"></param>
        /// <returns></returns>
        public static Version ToVersion(this PackageVersion packageVersion)
        {
            if (packageVersion.Revision == ushort.MaxValue)
            {
                if(packageVersion.Build == ushort.MaxValue)
                {
                    return new Version(packageVersion.Major, packageVersion.Minor);
                }
                else
                {
                    return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build);
                }
            }
            else
            {
                return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
            }

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
