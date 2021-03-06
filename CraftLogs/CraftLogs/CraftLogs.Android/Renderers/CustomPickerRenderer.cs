﻿/*
Copyright 2018 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using Android.Content;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Views;
using CraftLogs.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace CraftLogs.Droid.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {

        public CustomPickerRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Focusable = false;
                Control.Gravity = GravityFlags.CenterHorizontal;
                /* To remove underline.
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.Transparent);
                Control.SetBackgroundDrawable(gd);
                */

                if (e.OldElement == null)
                    Control.InputType = InputTypes.TextFlagNoSuggestions;
            }
        }
    }
}