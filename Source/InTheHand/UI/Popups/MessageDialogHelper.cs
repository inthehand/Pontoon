//-----------------------------------------------------------------------
// <copyright file="MessageDialogHelper.cs" company="In The Hand Ltd">
//     Copyright © 2012-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace InTheHand.UI.Popups
{
    /// <summary>
    /// Provides helper methods for MessageDialog.
    /// </summary>
    public static class MessageDialogHelper
    {
        /// <summary>
        /// Shows a MessageDialog using a ContentDialog on desktop Windows where possible.
        /// </summary>
        /// <param name="md">The MessageDialog</param>
        /// <returns>The selected command.</returns>
        public static IAsyncOperation<IUICommand> ShowAsync2(this MessageDialog md)
        {
#if WINDOWS_UWP
            if (md.Commands.Count < 3 && Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
            {
                Windows.UI.Xaml.Controls.ContentDialog cd = new Windows.UI.Xaml.Controls.ContentDialog();
                cd.Title = md.Title;
                cd.Content = md.Content;
                if (md.Commands.Count == 0)
                {
                    cd.PrimaryButtonText = "Close";
                }
                else
                {
                    cd.PrimaryButtonText = md.Commands[0].Label;
                    cd.PrimaryButtonClick += (cnd, c)=>{
                        if (md.Commands.Count > 0)
                        {
                            md.Commands[0].Invoked?.Invoke(md.Commands[0]);
                        }
                    };
                    if (md.Commands.Count > 1)
                    {
                        cd.SecondaryButtonText = md.Commands[1].Label;
                        cd.SecondaryButtonClick += (cnd, c) => { md.Commands[1].Invoked?.Invoke(md.Commands[1]); };
                    }
                }

                return Task.Run<IUICommand>(async () =>
                {
                    ManualResetEvent mre = new ManualResetEvent(false);
                    IUICommand command = null;

                    await cd.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        ContentDialogResult dr = await cd.ShowAsync();
                        if (md.Commands.Count > 0)
                        {
                            switch (dr)
                            {
                                case ContentDialogResult.Primary:
                                    command = md.Commands[0];
                                    break;

                                case ContentDialogResult.Secondary:
                                    command = md.Commands[1];
                                    break;
                            }
                        }
                    });

                    mre.WaitOne();

                    return command;
                }).AsAsyncOperation<IUICommand>();
            }
            else
            {
                return md.ShowAsync();
            }
#else
            return md.ShowAsync();
#endif
        }
    }
}