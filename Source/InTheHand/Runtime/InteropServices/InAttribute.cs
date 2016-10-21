// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if __ANDROID__ || __IOS__ || WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE || WIN32
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(System.Runtime.InteropServices.InAttribute))]
#else

using System;

namespace System.Runtime.InteropServices
{
    public sealed class InAttribute : Attribute
    {
        public InAttribute()
        {
        }
    }
}
#endif