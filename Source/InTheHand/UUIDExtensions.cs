// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UUIDExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Java.Util;

namespace InTheHand
{
    /// <summary>
    /// Helper class for <see cref="Java.Util.UUID"/>.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item></list>
    /// </remarks>
    public static class UUIDExtensions
    {
        public static Guid ToGuid(this UUID uuid)
        {
            return new Guid(uuid.ToString());
        }
    }
}