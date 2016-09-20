// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsRuntimeSystemExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Runtime.CompilerServices;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly: TypeForwardedTo(typeof(global::System.WindowsRuntimeSystemExtensions))]
[assembly: TypeForwardedTo(typeof(global::System.IO.WindowsRuntimeStorageExtensions))]
#else
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace System
{
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

    namespace IO
    {
        public static class WindowsRuntimeStorageExtensions
        {
            /// <summary>
            /// Retrieves a stream for reading from a specified file.
            /// </summary>
            /// <param name="windowsRuntimeFile"></param>
            /// <returns></returns>
            [CLSCompliantAttribute(false)]
            public static Task<Stream> OpenStreamForReadAsync(this IStorageFile windowsRuntimeFile)
            {
                if (windowsRuntimeFile == null)
                {
                    throw new ArgumentNullException("windowsRuntimeFile");
                }

#if __ANDROID__ || __IOS__
                return Task.FromResult<Stream>(global::System.IO.File.OpenRead(windowsRuntimeFile.Path));
#else
                throw new PlatformNotSupportedException();
#endif
            }

            /// <summary>
            /// Retrieves a stream for reading from a file in the specified parent folder.
            /// </summary>
            /// <param name="rootDirectory">The Windows Runtime IStorageFolder object that contains the file to read from.</param>
            /// <param name="relativePath">The path, relative to the root folder, to the file to read from.</param>
            /// <returns></returns>
            [CLSCompliantAttribute(false)]
            public static Task<Stream> OpenStreamForReadAsync(this IStorageFolder rootDirectory, string relativePath)
            {
#if __ANDROID__ || __IOS__
                string newPath = Path.Combine(rootDirectory.Path, relativePath);
                return Task.FromResult<Stream>(global::System.IO.File.OpenRead(newPath));
#else
                throw new PlatformNotSupportedException();
#endif
            }

            /// <summary>
            /// Retrieves a stream for writing to a specified file.
            /// </summary>
            /// <param name="windowsRuntimeFile">The Windows Runtime IStorageFile object to write to.</param>
            /// <returns></returns>
            [CLSCompliantAttribute(false)]
            public static Task<Stream> OpenStreamForWriteAsync(this IStorageFile windowsRuntimeFile)
            {
                if(windowsRuntimeFile == null)
                {
                    throw new ArgumentNullException("windowsRuntimeFile");
                }

#if __ANDROID__ || __IOS__
                return Task.FromResult<Stream>(global::System.IO.File.OpenWrite(windowsRuntimeFile.Path));
#else
                throw new PlatformNotSupportedException();
#endif
            }

            /// <summary>
            /// Retrieves a stream for writing from a file in the specified parent folder.
            /// </summary>
            /// <param name="rootDirectory">The Windows Runtime IStorageFolder object that contains the file to write to.</param>
            /// <param name="relativePath">The path, relative to the root folder, to the file to write to.</param>
            /// <returns></returns>
            [CLSCompliantAttribute(false)]
            public static Task<Stream> OpenStreamForWriteAsync(this IStorageFolder rootDirectory, string relativePath)
            {
#if __ANDROID__ || __IOS__
                string newPath = Path.Combine(rootDirectory.Path, relativePath);
                return Task.FromResult<Stream>(global::System.IO.File.OpenWrite(newPath));
#else
                throw new PlatformNotSupportedException();
#endif
            }
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
}
#endif