// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsRuntimeSystemExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System
{
#if !WINDOWS_UWP && !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE
    /// <summary>
    /// 
    /// </summary>
    public static class WindowsRuntimeSystemExtensions
    {
        public static TaskAwaiter GetAwaiter(this IAsyncAction source)
        {
            if(source is RuntimeAction)
            {
                return ((RuntimeAction)source).task.GetAwaiter();
            }

            return new TaskAwaiter();
        }

        public static TaskAwaiter<TResult> GetAwaiter<TResult>(this IAsyncOperation<TResult> source)
        {
            if (source is RuntimeOperation<TResult>)
            {
                return ((RuntimeOperation<TResult>)source).task.GetAwaiter();
            }

            return new TaskAwaiter<TResult>();
        }
    }

    internal sealed class RuntimeAction : global::Windows.Foundation.IAsyncAction
    {
        internal Task task;

        public RuntimeAction(Task t)
        {
            task = t;
        }

        private AsyncActionCompletedHandler _completed;
        public AsyncActionCompletedHandler Completed
        {
            get
            {
                return _completed;
            }

            set
            {
                _completed = value;
            }
        }

        public void GetResults()
        { 
        }
    }

    internal sealed class RuntimeOperation<TResult> : global::Windows.Foundation.IAsyncOperation<TResult>
    {
        internal Task<TResult> task;

        public RuntimeOperation(Task<TResult> t)
        {
            task = t;
        }

        public AsyncOperationCompletedHandler<TResult> Completed
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public TResult GetResults()
        {
            return task.Result;
        }
    }
#endif
}