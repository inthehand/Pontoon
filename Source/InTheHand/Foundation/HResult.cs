// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly:global::System.Runtime.CompilerServices.TypeForwardedTo(typeof(Windows.Foundation.HResult))]
#else

using Windows.Foundation.Metadata;

namespace Windows.Foundation
{
    [ContractVersion(typeof(FoundationContract), 65536u)]
	public struct HResult
	{
		public int Value;
	}
}
#endif