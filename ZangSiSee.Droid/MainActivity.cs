﻿using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using System;
using Xamarin.Forms.Platform.Android;

namespace ZangSiSee.Droid
{
    [Activity(Label = "ZangSiSee", Icon = "@drawable/icon", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                try
                {
                    var ex = ((Exception)e.ExceptionObject).GetBaseException();
                    Console.WriteLine("**ZangSiSee MainActivity Unhandled Exception**\n\n" + ex);
                }
                catch
                {
                }
            };

            try
            {
                base.OnCreate(bundle);
                Window.SetSoftInputMode(SoftInput.AdjustPan);

                Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());

                ActionBar.SetIcon(new ColorDrawable(Color.Transparent));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}

