// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-15 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.ApplicationModel
{
    using System;
    using Windows.ApplicationModel;

    /// <summary>
    /// Provides interoperability between Windows Runtime <see cref="PackageVersion"/> and .NET <see cref="global::System.Version"/> types.
    /// </summary>
    public static class PackageVersionExtensions
    {
        internal static PackageVersion ToPackageVersion(this global::System.Version version)
        {
            PackageVersion packageVersion = new PackageVersion();
            packageVersion.Major = (ushort)version.Major;
            packageVersion.Minor = (ushort)version.Minor;
            packageVersion.Build = (ushort)version.Build;
            packageVersion.Revision = (ushort)version.Revision;
            return packageVersion;
        }

        internal static global::System.Version ToVersion(this PackageVersion packageVersion)
        {
            return new global::System.Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
    }
}
