// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextManager.cs" company="In The Hand Ltd">
//   Copyright (c) 2015 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Manages Android Context reference
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InTheHand.Platform.Android
{
    public static class ContextManager
    {
        internal static Context _context;

        /// <summary>
        /// Sets a reference to the current android Context (usually the executing Activity)
        /// </summary>
        /// <param name="c">The current Activity Context.</param>
        /// <remarks>
        /// For Xamarin Forms you can set using Xamarin.Forms.Forms.Context.
        /// For Xamarin.Android call this method in your MainActivity.</remarks>
        public static void SetCurrentContext(Context c)
        {
            _context = c;
        }

        public static Context Context
        {
            get
            {
                if(_context == null)
                {
                    throw new PlatformNotSupportedException("You must call InTheHand.Platform.Android.ContextManager.SetCurrentContext before calling this method");
                }
                return _context;
            }
        }
    }
}