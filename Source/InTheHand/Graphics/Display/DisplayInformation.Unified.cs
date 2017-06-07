//-----------------------------------------------------------------------
// <copyright file="DisplayInformation.Unified.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using UIKit;

namespace InTheHand.Graphics.Display
{
    partial class DisplayInformation
    {
        private UIScreen _screen;
        
        public static implicit operator UIScreen(DisplayInformation d)
        {
            return d._screen;
        }

        private DisplayInformation()
        {
            _screen = UIApplication.SharedApplication.KeyWindow.Screen;
        }

        private DisplayOrientations GetCurrentOrientation()
        {
            return _screen.Bounds.Width > _screen.Bounds.Height ? DisplayOrientations.Landscape : DisplayOrientations.Portrait;
        }

        private void GetRawDpi()
        {
            rawDpiX = (float?)_screen.NativeScale;
        }

        private void GetRawPixelsPerViewPixel()
        {
            _rawPixelsPerViewPixel = (float?)_screen.Scale;
        }
    }
}