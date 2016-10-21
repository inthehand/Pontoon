//-----------------------------------------------------------------------
// <copyright file="PopupMenu.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP 
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.UI.Popups.PopupMenu))]
//#else

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InTheHand.Foundation;
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

namespace InTheHand.UI.Popups
{
    /// <summary>
    /// Represents a context menu.
    /// </summary>
    /// <remarks>
    /// Context menus can show a maximum of six commands.
    /// This limit helps to ensure that the context menu remains uncluttered, usable, and directly relevant to users. 
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
                    if (_commands.Count > i)
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
        public Task<IUICommand> ShowAsync(Point invocationPoint)
        {
            return ShowForSelectionAsync(new Rect(invocationPoint.X, invocationPoint.Y,0,0), Placement.Default);
        }

        /// <summary>
        /// Shows the context menu by the specified selection.
        /// </summary>
        /// <param name="selection">The coordinates (in DIPs) of the selected rectangle, relative to the window.</param>
        /// <returns></returns>
        public Task<IUICommand> ShowForSelectionAsync(Rect selection)
        {
            return ShowForSelectionAsync(selection, Placement.Default);
        }

        /// <summary>
        /// Shows the context menu in the preferred placement relative to the specified selection.
        /// </summary>
        /// <param name="selection">The coordinates (in DIPs) of the selected rectangle, relative to the window.</param>
        /// <param name="preferredPlacement">The preferred placement of the context menu relative to the selection rectangle.</param>
        /// <returns></returns>
        public Task<IUICommand> ShowForSelectionAsync(Rect selection, Placement preferredPlacement)
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
            });

#elif __IOS__
            uac = UIAlertController.Create("", "", UIAlertControllerStyle.ActionSheet);
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
            });
#else
            throw new PlatformNotSupportedException();
#endif
        }



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
        /// Gets the commands for the context menu.
        /// </summary>
        /// <value>The commands for the context menu.</value>
        public IList<IUICommand> Commands
        {
            get
            {
                return _commands;
            }
        }
    }
}
//#endif