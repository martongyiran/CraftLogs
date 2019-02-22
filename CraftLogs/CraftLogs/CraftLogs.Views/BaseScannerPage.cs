/*
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

using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace CraftLogs.Views
{
    public partial class BaseScannerPage : ZXingScannerPage
    {


        #region ScanResultCommand

        public static readonly BindableProperty ScanResultCommandProperty =
            BindableProperty.Create(nameof(ScanResultCommand), typeof(ICommand), typeof(BaseScannerPage), null);

        public ICommand ScanResultCommand
        {
            get => (ICommand)GetValue(ScanResultCommandProperty);
            set => SetValue(ScanResultCommandProperty, value);
        }

        #endregion

        static readonly MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions
        {
            AutoRotate = false,
            UseFrontCameraIfAvailable = false,
            TryHarder = true,
            PossibleFormats = new List<ZXing.BarcodeFormat>
        {
           ZXing.BarcodeFormat.QR_CODE
        }
        };

        public BaseScannerPage()
            :base (options)
        {
        }
    }
}
