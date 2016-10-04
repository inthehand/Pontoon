//-----------------------------------------------------------------------
// <copyright file="MessageDialog.cs" company="In The Hand Ltd">
//     Copyright © 2012-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Popups.MessageDialog))]
#else
namespace Windows.UI.Popups
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Windows.Foundation;
#if __ANDROID__
    using Android.App;
    using Android.Content;
    using Android.Content.Res;
#elif __IOS__
    using UIKit;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls; 
#endif
    /// <summary>
    /// Represents a dialog.
    /// </summary>
    /// <remarks>
    /// The dialog has a command bar that can support up to three commands.
    /// If you don't specify any commands, then a default command is added to close the dialog.
    /// <para>The dialog dims the screen behind it and blocks touch events from passing to the app's canvas until the user responds.</para>
    /// <para>Message dialogs should be used sparingly, and only for critical messages or simple questions that must block the user's flow.</para>
    /// </remarks>
    public sealed partial class MessageDialog
    {
#if WINDOWS_PHONE || WINDOWS_PHONE_APP
        private const int MaxCommands = 2;
#else
        private const int MaxCommands = 3;
#endif

#if __ANDROID__ || __IOS__
        EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        IUICommand _selectedCommand;
#endif

#if __IOS__
        UIAlertController uac = null;
       

        private void ActionClicked(UIAlertAction action)
        {
            // find the equivalent "metro" command and call the event handler
            for(int i = 0; i < uac.Actions.Length; i++)
            {
                if(uac.Actions[i] == action)
                {
                    if (Commands.Count > i)
                    {
                        _selectedCommand = Commands[i];

                        if (Commands[i].Invoked != null)
                        {
                            Commands[i].Invoked(Commands[i]);
                        }
                    }

                    break;
                }
            }

            handle.Set();
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDialog"/> class to display an untitled message dialog box that can be used to ask your user simple questions.
        /// </summary>
        /// <param name="content">The message you want displayed to the user.</param>
        public MessageDialog(string content) : this(content, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDialog"/> class to display a titled message dialog box that can be used to ask your user simple questions.
        /// </summary>
        /// <param name="content">The message you want displayed to the user.</param>
        /// <param name="title">The title you want displayed on the dialog box.</param>
        public MessageDialog(string content, string title)
        {
            this.Content = content;
            this.Title = title;
        }

        /// <summary>
        /// Begins an asynchronous operation showing a dialog.
        /// </summary>
        /// <returns>An object that represents the asynchronous operation.
        /// For more on the async pattern, see Asynchronous programming in the Windows Runtime.</returns>
        /// <remarks>In some cases, such as when the dialog is closed by the system out of your control, your result can be an empty command.
        /// <see cref="IAsyncOperation{TResult}.GetResults()"/> returns either the command selected which destroyed the dialog, or an empty command.
        /// For example, a dialog hosted in a charms window will return an empty command if the charms window has been dismissed.</remarks>
        public IAsyncOperation<IUICommand> ShowAsync()
        {
            if (this.Commands.Count > MaxCommands)
            {
                throw new InvalidOperationException();
            }

#if __ANDROID__
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity);
            Android.App.AlertDialog dialog = builder.Create();
            dialog.SetTitle(this.Title);
            dialog.SetMessage(this.Content);
            if (Commands.Count == 0)
            {
                dialog.SetButton(-1, Resources.System.GetString(Android.Resource.String.Cancel), new EventHandler<Android.Content.DialogClickEventArgs>(Clicked));
            }
            else
            {
                for (int i = 0; i < this.Commands.Count; i++)
                {
                    dialog.SetButton(-1 - i, this.Commands[i].Label, new EventHandler<Android.Content.DialogClickEventArgs>(Clicked));
                }
            }
            dialog.Show();

            return Task.Run<IUICommand>(() =>
            {
                handle.WaitOne();
                return _selectedCommand;
            }).AsAsyncOperation<IUICommand>();

#elif __IOS__
            uac = UIAlertController.Create(this.Title, this.Content, UIAlertControllerStyle.Alert);
            if (Commands.Count == 0)
            {
                uac.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel | UIAlertActionStyle.Default, ActionClicked));
            }
            else
            {
                for (int i = 0; i < this.Commands.Count; i++)
                {
                    UIAlertAction action = UIAlertAction.Create(this.Commands[i].Label, CancelCommandIndex == i ? UIAlertActionStyle.Cancel : UIAlertActionStyle.Default, ActionClicked);
                    uac.AddAction(action);
                }
            }
            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;
            
            currentController.PresentViewController(uac, true, null);

            return Task.Run<IUICommand>(() =>
            {
                handle.WaitOne();
                return _selectedCommand;
            }).AsAsyncOperation<IUICommand>();

#elif WINDOWS_PHONE
            List<string> buttons = new List<string>();
            foreach(IUICommand uic in this.Commands)
            {
                buttons.Add(uic.Label);
            }

            if (buttons.Count == 0)
            {
                buttons.Add("Close");
            }

            MessageDialogAsyncOperation asyncOperation = new MessageDialogAsyncOperation(this);

            string contentText = this.Content;

            // trim message body to 255 chars
            if (contentText.Length > 255)
            {
                contentText = contentText.Substring(0, 255);
            }
            
            while(Microsoft.Xna.Framework.GamerServices.Guide.IsVisible)
            {
                Thread.Sleep(250);
            }
            Microsoft.Xna.Framework.GamerServices.Guide.BeginShowMessageBox(
                        string.IsNullOrEmpty(this.Title) ? " " : this.Title,
                        contentText,
                        buttons,
                        (int)this.DefaultCommandIndex, // can choose which button has the focus
                        Microsoft.Xna.Framework.GamerServices.MessageBoxIcon.None, // can play sounds
                        result =>
                        {
                            int? returned = Microsoft.Xna.Framework.GamerServices.Guide.EndShowMessageBox(result);
                            
                            // process and fire the required handler
                            if (returned.HasValue)
                            {
                                if (Commands.Count > returned.Value)
                                {
                                    IUICommand theCommand = Commands[returned.Value];
                                    asyncOperation.SetResults(theCommand);
                                    if (theCommand.Invoked != null)
                                    {
                                        theCommand.Invoked(theCommand);
                                    }
                                }
                                else
                                {
                                    asyncOperation.SetResults(null);
                                }
                            }
                            else
                            {
                                asyncOperation.SetResults(null);
                            }
                        }, null);

            return asyncOperation.AsTask<IUICommand>().AsAsyncOperation<IUICommand>(); ;
#elif WINDOWS_UWP
            if (Commands.Count < 3 && Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.UI.ApplicationSettings.ApplicationsSettingsContract", 1))
            {
                Windows.UI.Xaml.Controls.ContentDialog cd = new Windows.UI.Xaml.Controls.ContentDialog();
                cd.Title = this.Title;
                cd.Content = this.Content;
                if(Commands.Count == 0)
                {
                    cd.PrimaryButtonText = "Close";
                }
                else
                {
                    cd.PrimaryButtonText = Commands[0].Label;
                    cd.PrimaryButtonClick += Cd_PrimaryButtonClick;
                    if(Commands.Count > 1)
                    {
                        cd.SecondaryButtonText = Commands[1].Label;
                        cd.SecondaryButtonClick += Cd_SecondaryButtonClick;
                    }
                }
                
                return Task.Run<IUICommand>(async () => 
                {
                    ManualResetEvent mre = new ManualResetEvent(false);
                    IUICommand command = null;

                    await cd.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        ContentDialogResult dr = await cd.ShowAsync();
                        if (Commands.Count > 0)
                        {
                            switch (dr)
                            {
                                case ContentDialogResult.Primary:
                                    command = Commands[0];
                                    if(Commands[0].Invoked != null)
                                    {
                                        Commands[0].Invoked.Invoke(Commands[0]);
                                    }
                                    break;

                                case ContentDialogResult.Secondary:
                                    command = commands[1];
                                    if (Commands[1].Invoked != null)
                                    {
                                        Commands[1].Invoked.Invoke(Commands[1]);
                                    }
                                    break;
                            }
                        }
                    });

                    mre.WaitOne();

                    return command;
                });
            }
            else
            {
                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(this.Content, this.Title);
                foreach (IUICommand command in this.Commands)
                {
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand(command.Label, (c)=> { command.Invoked(command); }, command.Id));
                }
                return Task.Run<IUICommand>(async () => {
                    Windows.UI.Popups.IUICommand command = await dialog.ShowAsync();
                    if (command != null)
                    {
                        int i = 0;
                        foreach(Windows.UI.Popups.IUICommand c in dialog.Commands)
                        {
                            if(command == c)
                            {
                                break;
                            }

                            i++;
                        }

                        return this.Commands[i];
                    }
                    return null;
                });
            }
#elif WIN32
            return Task.Run<IUICommand>(() =>
            {
                IUICommand cmd = ShowTaskDialog();
                if (cmd != null)
                {
                    cmd.Invoked?.Invoke(cmd);
                }

                return cmd;
            }).AsAsyncOperation<IUICommand>();
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WINDOWS_UWP
        private void Cd_SecondaryButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, Windows.UI.Xaml.Controls.ContentDialogButtonClickEventArgs args)
        {
            Commands[1].Invoked.Invoke(Commands[1]);
        }

        private void Cd_PrimaryButtonClick(Windows.UI.Xaml.Controls.ContentDialog sender, Windows.UI.Xaml.Controls.ContentDialogButtonClickEventArgs args)
        {
            if(Commands.Count > 0)
            {
                Commands[0].Invoked.Invoke(Commands[0]);
            }
        }
#endif

#if __ANDROID__
        private void Clicked(object sender, Android.Content.DialogClickEventArgs e)
        {
            if (Commands.Count > 0)
            {
                _selectedCommand = Commands[-1 - e.Which];
                if (_selectedCommand.Invoked != null)
                {
                    _selectedCommand.Invoked(_selectedCommand);
                }
            }
            handle.Set();
        }
#endif
        
        private List<IUICommand> _commands = new List<IUICommand>();

        /// <summary>
        /// Gets the set of commands that appear in the command bar of the message dialog.
        /// </summary>
        /// <remarks>This is the array of commands that makes the dialog actionable.
        /// Get this array and add your dialog commands to it.</remarks>
        /// <value>The commands.</value>
        public IList<IUICommand> Commands
        {
            get
            {
                return _commands;
            }
        }

        /// <summary>
        /// Gets or sets the message to be displayed to the user.
        /// </summary>
        /// <value>The message to be displayed to the user.</value>
        /// <remarks>Use the content to convey the objective of the dialog.
        /// Present the message, error or blocking question as simply as possible without extraneous information.
        /// <para>When a title is used, use the content to present additional information helpful to understanding or using the dialog.
        /// You can use this area to provide more detail or define terminology.
        /// Don't repeat the title with slightly different wording.</para></remarks>
        public string Content { get; set; }

        private uint _cancelCommandIndex = uint.MaxValue;

        /// <summary>
        /// Gets or sets the index of the command you want to use as the cancel command.
        /// This is the command that fires when users press the ESC key.
        /// <para>Add the commands before you set the index.</para>
        /// </summary>
        public uint CancelCommandIndex
        {
            get
            {
                return _cancelCommandIndex;
            }
            set
            {
                if(value >= Commands.Count && value != uint.MaxValue)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _cancelCommandIndex = value;
            }
        }

        private uint _defaultCommandIndex = uint.MaxValue;

        /// <summary>
        /// Gets or sets the index of the command you want to use as the default.
        /// This is the command that fires by default when users press the ENTER key instead of a specific command, for example.
        /// </summary>
        /// <remarks>Add the commands before you set the index.</remarks>
        /// <value>The index of the default command.</value>
        public uint DefaultCommandIndex
        {
            get
            {
                return _defaultCommandIndex;
            }

            set
            {
                if (value >= this.Commands.Count && value != uint.MaxValue)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _defaultCommandIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the title to display on the dialog box, if any. 
        /// </summary>
        /// <value>The title you want to display on the dialog.
        /// If the title is not set, this will return an empty string.</value>
        /// <remarks>Use the title as a concise main instruction to convey the objective of the dialog.
        /// <para>Long titles do not wrap and will be truncated.</para>
        /// <para>If you're using the dialog to deliver a simple message, error or question, omit the title.
        /// Rely on the <see cref="Content"/> to deliver that core information.</para></remarks>
        public string Title { get; set; }
    }
}
#endif