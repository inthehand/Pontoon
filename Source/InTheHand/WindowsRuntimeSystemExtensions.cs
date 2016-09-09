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
    /// Provides extension methods for converting between tasks and Windows Runtime asynchronous actions and operations.
    /// </summary>
    [CLSCompliantAttribute(false)]
    public static class WindowsRuntimeSystemExtensions
    {
        /// <summary>
        /// Returns a Windows Runtime asynchronous action that represents a started task. 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IAsyncAction AsAsyncAction(this Task source)
        {
            return new RuntimeAction(source);
        }

        /// <summary>
        /// Returns a Windows Runtime asynchronous operation that represents a started task that returns a result. 
        /// </summary>
        /// <typeparam name="TResult">The type that returns the result.</typeparam>
        /// <param name="source">The started task. </param>
        /// <returns></returns>
        public static IAsyncOperation<TResult> AsAsyncOperation<TResult>(this Task<TResult> source)
        {
            return new RuntimeOperation<TResult>(source);
        }

        /// <summary>
        /// Returns a task that represents a Windows Runtime asynchronous action. 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Task AsTask(this IAsyncAction source)
        {
            RuntimeAction action = source as RuntimeAction;
            return action.task;
        }

        /// <summary>
        /// Returns a task that represents a Windows Runtime asynchronous operation returns a result.
        /// </summary>
        /// <typeparam name="TResult">The type of object that returns the result of the asynchronous operation.</typeparam>
        /// <param name="source">The asynchronous operation.</param>
        /// <returns></returns>
        public static Task<TResult> AsTask<TResult>(this IAsyncOperation<TResult> source)
        {
            RuntimeOperation<TResult> operation = source as RuntimeOperation<TResult>;
            return operation.task;
        }

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

        private AsyncOperationCompletedHandler<TResult> _completed;
        public AsyncOperationCompletedHandler<TResult> Completed
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

        public TResult GetResults()
        {
            return task.Result;
        }
    }
#endif
}