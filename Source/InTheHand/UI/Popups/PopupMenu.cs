//-----------------------------------------------------------------------
// <copyright file="PopupMenu.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.UI.Popups.PopupMenu))]
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
    public sealed class PopupMenu
    {
        private const int MaxCommands = 6;

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
                    if (commands.Count > i)
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
        /// Creates a new instance of the PopupMenu class.
        /// </summary>
        public PopupMenu()
        {
        }

        /// <summary>
        /// Shows the context menu at the specified client coordinates.
        /// </summary>
        /// <param name="invocationPoint">The coordinates (in DIPs), relative to the window, of the user's finger or mouse pointer when the oncontextmenu event fired.
        /// The menu is placed above and centered on this point.</param>
        /// <returns>An object that represents the asynchronous operation.
        /// For more on the async pattern, see Asynchronous programming in the Windows Runtime.</returns>
        public IAsyncOperation<IUICommand> ShowAsync(Point invocationPoint)
        {
            return ShowForSelectionAsync(new Rect(invocationPoint.X, invocationPoint.Y,0,0), Placement.Default);
        }

        /// <summary>
        /// Shows the context menu by the specified selection.
        /// </summary>
        /// <param name="selection">The coordinates (in DIPs) of the selected rectangle, relative to the window.</param>
        /// <returns></returns>
        public IAsyncOperation<IUICommand> ShowForSelectionAsync(Rect selection)
        {
            return ShowForSelectionAsync(selection, Placement.Default);
        }

        /// <summary>
        /// Shows the context menu in the preferred placement relative to the specified selection.
        /// </summary>
        /// <param name="selection">The coordinates (in DIPs) of the selected rectangle, relative to the window.</param>
        /// <param name="preferredPlacement">The preferred placement of the context menu relative to the selection rectangle.</param>
        /// <returns></returns>
        public IAsyncOperation<IUICommand> ShowForSelectionAsync(Rect selection, Placement preferredPlacement)
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
            uac = UIAlertController.Create(this.Title, this.Content, UIAlertControllerStyle.ActionSheet);
            if (Commands.Count == 0)
            {
                uac.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel | UIAlertActionStyle.Default, ActionClicked));
            }
            else
            {
                for (int i = 0; i < this.Commands.Count; i++)
                {
                    UIAlertAction action = UIAlertAction.Create(this.Commands[i].Label, UIAlertActionStyle.Default, ActionClicked);
                    uac.AddAction(action);
                }
            }

             

            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;

            // set layout requirements for iPad
            var popoverController = uac.PopoverPresentationController;
            if(popoverController != null)
            {
                popoverController.SourceView = currentController.View;
                popoverController.SourceRect = new CoreGraphics.CGRect(selection.X, selection.Y, selection.Width, selection.Height);
                popoverController.PermittedArrowDirections = InTheHand.UI.Popups.PlacementHelper.ToArrowDirection(preferredPlacement);
            }

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
                        this.DefaultCommandIndex, // can choose which button has the focus
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
#elif WINDOWS_EMBEDDED
            MessageDialogForm mdf = new MessageDialogForm(this);
            mdf.Show();
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(this.Content, this.Title);
            foreach(IUICommand command in this.Commands)
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand(command.Label, null, command.Id));
            }
            Task<Windows.UI.Popups.IUICommand> t = dialog.ShowAsync().AsTask<Windows.UI.Popups.IUICommand>();
            
            return Task.Run<IUICommand>(() => {
                t.Wait();
                Windows.UI.Popups.IUICommand command = t.Result;
               
                if (command != null)
                {
                    if (command.Invoked != null)
                    {
                        command.Invoked.Invoke(command);
                    }
                    return new UICommand(command.Label, null, command.Id);
                }
                return null;
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
        
        private List<IUICommand> commands = new List<IUICommand>();

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
                return this.commands;
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

        private int defaultCommandIndex;

        /// <summary>
        /// Gets or sets the index of the command you want to use as the default.
        /// This is the command that fires by default when users press the ENTER key instead of a specific command, for example.
        /// </summary>
        /// <remarks>Add the commands before you set the index.</remarks>
        /// <value>The index of the default command.</value>
        public int DefaultCommandIndex
        {
            get
            {
                return this.defaultCommandIndex;
            }

            set
            {
                if (value < 0 || value >= this.Commands.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.defaultCommandIndex = value;
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