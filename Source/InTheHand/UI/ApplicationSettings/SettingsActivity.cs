//-----------------------------------------------------------------------
// <copyright file="SettingsActivity.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using Android.App;
using Android.Content.PM;
using Android.Views;
using InTheHand.ApplicationModel;

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// Exposes standard Android preferences screens
    /// </summary>
    //[CLSCompliant(false)]
    [Activity(Label ="Settings")]
    internal sealed class SettingsActivity : PreferenceActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            if(HasHeaders)
            {
                TextView tv = new TextView(this);
                tv.Text = "Version " + Windows.ApplicationModel.Package.Current.Id.Version.ToString(4);
                SetListFooter(tv);
            }
        }

        public override bool OnNavigateUp()
        {
            Finish();
            return true;
        }

        public override void OnBuildHeaders(IList<Header> target)
        {
            Bundle b = new Bundle();
            b.PutString("Title", "Test Fragment 1");
            target.Add(new Header() { Title = new Java.Lang.String("Test 1"), Summary= new Java.Lang.String("Summary 1")});

            Bundle b2 = new Bundle();
            b2.PutString("Title", "Test Fragment 2");
            target.Add(new Header() { Title = new Java.Lang.String("Test 2"), Summary = new Java.Lang.String("Summary 2")});
        }
    }
}
