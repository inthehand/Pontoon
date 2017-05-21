// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageVersion.WinRT.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.ApplicationModel
{
    partial struct PackageVersion
    {
        public static implicit operator Windows.ApplicationModel.PackageVersion(PackageVersion pv)
        {
            return new Windows.ApplicationModel.PackageVersion { Major = pv.Major, Minor = pv.Minor, Build = pv.Build, Revision = pv.Revision };
        }

        public static implicit operator PackageVersion(Windows.ApplicationModel.PackageVersion pv)
        {
            return new PackageVersion() { Major = pv.Major, Minor = pv.Minor, Build = pv.Build, Revision = pv.Revision };
        }
    }
}