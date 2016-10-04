// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeOffsetHelper.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __IOS__
using Foundation;
#endif
using System;

namespace InTheHand
{
    /// <summary>
    /// Helper class for <see cref="DateTimeOffset"/>.
    /// </summary>
    public static class DateTimeOffsetHelper
    {
        private static DateTimeOffset dt = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// Converts a Unix time expressed as the number of seconds that have elapsed since 1970-01-01T00:00:00Z to a <see cref="DateTimeOffset"/> value. 
        /// </summary>
        /// <param name="seconds">A Unix time, expressed as the number of seconds that have elapsed since 1970-01-01T00:00:00Z (January 1, 1970, at 12:00 AM UTC).
        /// For Unix times before this date, its value is negative.</param>
        /// <returns>A date and time value that represents the same moment in time as the Unix time. </returns>
        public static DateTimeOffset FromUnixTimeSeconds(long seconds)
        {
#if __ANDROID__ || __IOS__
            return DateTimeOffset.FromUnixTimeSeconds(seconds);
#else
            return dt.AddSeconds(seconds);
#endif
        }

        /// <summary>
        /// Converts a Unix time expressed as the number of milliseconds that have elapsed since 1970-01-01T00:00:00Z to a <see cref="DateTimeOffset"/> value.
        /// </summary>
        /// <param name="milliseconds">A Unix time, expressed as the number of milliseconds that have elapsed since 1970-01-01T00:00:00Z (January 1, 1970, at 12:00 AM UTC).
        /// For Unix times before this date, its value is negative.</param>
        /// <returns>A date and time value that represents the same moment in time as the Unix time. </returns>
        public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds)
        {
#if __ANDROID__ || __IOS__
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
#else
            return dt.AddSeconds(milliseconds / 1000);
#endif
        }

        /// <summary>
        /// Returns the number of seconds that have elapsed since 1970-01-01T00:00:00Z. 
        /// </summary>
        /// <param name="date">The DateTimeOffset value.</param>
        /// <returns>The number of seconds that have elapsed since 1970-01-01T00:00:00Z. </returns>
        /// <remarks>Unix time represents the number of seconds that have elapsed since 1970-01-01T00:00:00Z (January 1, 1970, at 12:00 AM UTC).
        /// It does not take leap seconds into account.
        /// <para>This method first converts the current instance to UTC before returning its Unix time.
        /// For date and time values before 1970-01-01T00:00:00Z, this method returns a negative value.</para></remarks>
        public static long ToUnixTimeSeconds(this DateTimeOffset date)
        {
#if __ANDROID__ || __IOS__
            return date.ToUnixTimeSeconds();
#else
            return Convert.ToInt64(date.Subtract(dt).TotalSeconds);
#endif
        }

        /// <summary>
        /// Returns the number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z. 
        /// </summary>
        /// <param name="date">The DateTimeOffset value.</param>
        /// <returns>The number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z.</returns>
        /// <remarks>Unix time represents the number of seconds that have elapsed since 1970-01-01T00:00:00Z (January 1, 1970, at 12:00 AM UTC).
        /// It does not take leap seconds into account.
        /// This method returns the number of milliseconds in Unix time. 
        /// <para>This method first converts the current instance to UTC before returning the number of milliseconds in its Unix time.
        /// For date and time values before 1970-01-01T00:00:00Z, this method returns a negative value.</para></remarks>
        public static long ToUnixTimeMilliseconds(this DateTimeOffset date)
        {
#if __ANDROID__ || __IOS__ || WIN32
            return date.ToUnixTimeMilliseconds();
#else
            return Convert.ToInt64(date.Subtract(dt).TotalMilliseconds);
#endif
        }

#if __IOS__
        private static readonly DateTimeOffset NSDateReferenceDate = new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero);
        
        public static DateTimeOffset FromNSDate(NSDate date)
        {
            if (date == NSDate.DistantPast)
            {
                return DateTimeOffset.MinValue;
            }
            else if (date == NSDate.DistantFuture)
            {
                return DateTimeOffset.MaxValue;
            }

            return NSDateReferenceDate.AddSeconds(date.SecondsSinceReferenceDate);
        }
        
        public static NSDate ToNSDate(this DateTimeOffset value)
        {
            if (value == DateTimeOffset.MinValue)
            {
                return NSDate.DistantPast;
            }
            else if (value == DateTimeOffset.MaxValue)
            {
                return NSDate.DistantFuture;
            }

            return NSDate.FromTimeIntervalSinceReferenceDate(value.Subtract(NSDateReferenceDate).TotalSeconds);
        }
#endif
    }
}