// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.GuidAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [ContractVersion(typeof(FoundationContract), 65536U)]
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Interface)]
    public sealed class GuidAttribute : Attribute
    {
        public GuidAttribute(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        {
        }
    }
}
#endif