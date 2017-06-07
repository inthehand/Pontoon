//-----------------------------------------------------------------------
// <copyright file="DisplayInformation.macOS.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using AppKit;

namespace InTheHand.Graphics.Display
{
    partial class DisplayInformation
    {
        private NSScreen _screen;
        
        public static implicit operator NSScreen(DisplayInformation d)
        {
            return d._screen;
        }

        private static DisplayInformation DoGetForCurrentView()
        {
            DisplayInformation di = new DisplayInformation();
            di._screen = NSApplication.SharedApplication.MainWindow.Screen;

            return di;
        }

        private DisplayOrientations GetCurrentOrientation()
        {
            return _screen.Frame.Width > _screen.Frame.Height ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
        }
        
    }
}