//-----------------------------------------------------------------------
// <copyright file="DispatcherTimer.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Xaml.DispatcherTimer))]
#else

using System;
#if __ANDROID__ || __IOS__ || WIN32
using System.Timers;
#endif

#if __IOS__
using UIKit;
#endif

namespace Windows.UI.Xaml
{
    /// <summary>
    /// Provides a timer that is integrated into the Dispatcher queue, which is processed at a specified interval of time.
    /// </summary>
    public sealed class DispatcherTimer
    {
#if __ANDROID__ || __IOS__ || WIN32
        private Timer _timer;

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(Tick != null)
            {
#if __IOS__
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    Tick?.Invoke(this, null);
                });
#elif __ANDROID__
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
                {
                    Tick?.Invoke(this, null);
                });
#endif
            }
        }
#elif WINDOWS_PHONE
        private global::System.Windows.Threading.DispatcherTimer _dispatcherTimer;

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
           Tick?.Invoke(this, null);
        }
#endif
                /// <summary>
                /// Initializes a new instance of the <see cref="DispatcherTimer"/> class. 
                /// </summary>
        public DispatcherTimer()
        {
#if __ANDROID__ || __IOS__ || WIN32
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
#elif WINDOWS_PHONE
            _dispatcherTimer = new global::System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
#endif
        }

        



        /// <summary>
        /// Occurs when the timer interval has elapsed. 
        /// </summary>
        public event EventHandler<object> Tick;

        /// <summary>
        /// Gets or sets the amount of time between timer ticks. 
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
#if __ANDROID__ || __IOS__ || WIN32
                return TimeSpan.FromMilliseconds(_timer.Interval);
#elif WINDOWS_PHONE
                return _dispatcherTimer.Interval;
#else
                return TimeSpan.Zero;
#endif
            }

            set
            {
#if __ANDROID__ || __IOS__ || WIN32
                _timer.Interval = value.TotalMilliseconds;
#elif WINDOWS_PHONE
                _dispatcherTimer.Interval = value;
#endif
            }          
        }

        /// <summary>
        /// Gets a value that indicates whether the timer is running. 
        /// </summary>
        /// <value>true if the timer is enabled and running; otherwise, false.</value>
        public bool IsEnabled
        {
            get
            {
#if __ANDROID__ || __IOS__ || WIN32
                return _timer.Enabled;
#elif WINDOWS_PHONE
                return _dispatcherTimer.IsEnabled;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Starts the <see cref="DispatcherTimer"/>. 
        /// </summary>
        /// <remarks>If the timer has already started, then it is restarted.</remarks>
        public void Start()
        {
#if __ANDROID__ || __IOS__ || WIN32
            if(_timer.Enabled)
            {
                _timer.Stop();
            }

            _timer.Start();
#elif WINDOWS_PHONE
            _dispatcherTimer.Start();
#endif
        }

        /// <summary>
        /// Stops the <see cref="DispatcherTimer"/>. 
        /// </summary>
        public void Stop()
        {
#if __ANDROID__ || __IOS__ || WIN32
            _timer.Stop();
#elif WINDOWS_PHONE
            _dispatcherTimer.Stop();
#endif
        }
    }
}
#endif