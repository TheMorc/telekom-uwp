using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Diagnostics;
using Telekom.Views;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telekom
{
    internal partial class ExtendedSplash
    {
        internal Rect splashImageRect;
        internal bool dismissed = false;
        internal Frame rootFrame;
        internal SplashScreen splash;
        internal double ScaleFactor;

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();

            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            ScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            splash = splashscreen;

            if (splash != null)
            {
                splash.Dismissed += new TypedEventHandler<SplashScreen, object>(DismissedEventHandler);
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            rootFrame = new Frame();

            Telekom();
        }

        #region statusbar
        private async void StatusText(string text)
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 226, 0, 116);
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 226, 0, 116);

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                loadingLabel.Visibility = Visibility.Collapsed;
                progRing.Visibility = Visibility.Collapsed;

                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                statusBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 226, 0, 116);
                statusBar.BackgroundOpacity = 1.0;

                StatusBarProgressIndicator indicator = statusBar.ProgressIndicator;

                indicator.ProgressValue = null;
                indicator.Text = text;
                await indicator.ShowAsync();
            }
            else
            {
                loadingLabel.Text = text;
            }
        }

        private async void HideStatusText()
        {
            UISettings DefaultTheme = new Windows.UI.ViewManagement.UISettings();
            string uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (uiTheme == "#FF000000") //dark
            {
                titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 31, 31, 31);
                titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 31, 31, 31);
                titleBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            }
            else if (uiTheme == "#FFFFFFFF") //light
            {
                titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 230, 230, 230);
                titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 31, 31, 31);
                titleBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
            }

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();

                if (uiTheme == "#FF000000") //dark
                {
                    statusBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 31, 31, 31);
                    statusBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                }
                else if (uiTheme == "#FFFFFFFF") //light
                {
                    statusBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 230, 230, 230);
                    statusBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                }

                StatusBarProgressIndicator indicator = statusBar.ProgressIndicator;
                await indicator.HideAsync();
            }
        }
        #endregion

        private async void Telekom()
        {

            StatusText(App.resourceLoader.GetString("Loading"));

            Debug.WriteLine("[tlkm_extendedsplash] 10ms wait"); //seems like that something has issues getting loaded as soon as the page is loaded
            await System.Threading.Tasks.Task.Delay(10);

            Debug.WriteLine("[tlkm_extendedsplash] deviceId: " + App.TLKM.deviceId.ToString());

            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                App.TLKM.lastCode = "nointernet";
                OpenErrorPage();
                return;
            }

            if (App.TLKM.localSettings.Values["hasAccount"] as string == "yes")
            {
                StatusText(App.resourceLoader.GetString("Logging_in"));

                Debug.WriteLine("[tlkm_extendedsplash] settings - hasAccount - retrieving information");

                Debug.WriteLine("[tlkm_extendedsplash] settings - deviceId: " + App.TLKM.localSettings.Values["deviceId"]);

                App.TLKM.accessToken = (string)App.TLKM.localSettings.Values["accessToken"];
                Debug.WriteLine("[tlkm_extendedsplash] settings - accessToken: " + App.TLKM.accessToken);

                App.TLKM.refreshToken = (string)App.TLKM.localSettings.Values["refreshToken"];
                Debug.WriteLine("[tlkm_extendedsplash] settings - refreshToken: " + App.TLKM.refreshToken);

                App.TLKM.serviceId = (long)App.TLKM.localSettings.Values["serviceId"];
                Debug.WriteLine("[tlkm_extendedsplash] settings - serviceId: " + App.TLKM.serviceId);


                bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Login());
                if (success)
                {
                    Debug.WriteLine("[tlkm_extendedsplash] logged in successfully!");
                    StatusText(App.resourceLoader.GetString("Login_success"));
                }
                else
                {
                    if (App.TLKM.lastCode == "hal.security.authentication.access_token.invalid")
                    {
                        App.TLKM.lastCode = "";
                        StatusText(App.resourceLoader.GetString("Generating_new_token"));
                        bool regen_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Regen_token());
                        if (regen_success)
                        {
                            Debug.WriteLine("[tlkm_extendedsplash] token regenerated successfully!");
                        }
                        else
                        {
                            OpenErrorPage();
                            return;
                        }

                        success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Login());
                        if (success)
                        {
                            Debug.WriteLine("[tlkm_extendedsplash] login attempt 2 - logged in successfully!");
                            StatusText(App.resourceLoader.GetString("Login_success"));
                        }
                        else
                        {
                            OpenErrorPage();
                            return;
                        }
                    }
                    else if (App.TLKM.lastCode == "deadaccess")
                    {
                        Debug.WriteLine("[tlkm_extendedsplash] dead access token - setup reset");

                        HideStatusText();
                        rootFrame.Navigate(typeof(Setup_pin));
                        Window.Current.Content = rootFrame;
                        return;
                    }
                    else
                    {
                        OpenErrorPage();
                        return;
                    }
                }

                HideStatusText();
                rootFrame.Navigate(typeof(AppShell));
                Window.Current.Content = rootFrame;
            }
            else
            {
                Debug.WriteLine("[tlkm_extendedsplash] settings - no account - proceeding to setup");

                HideStatusText();
                rootFrame.Navigate(typeof(Setup_pin));
                Window.Current.Content = rootFrame;
            }
        }

        private void OpenErrorPage()
        {
            HideStatusText();
            rootFrame.Navigate(typeof(Error));
            Window.Current.Content = rootFrame;
        }

        #region extendedsplash logo
        // Position the extended splash screen image in the same location as the system splash screen image.
        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                extendedSplashImage.Height = splashImageRect.Height / ScaleFactor;
                extendedSplashImage.Width = splashImageRect.Width / ScaleFactor;
            }
            else
            {
                extendedSplashImage.Height = splashImageRect.Height;
                extendedSplashImage.Width = splashImageRect.Width;
            }
        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        private void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }
        #endregion
    }
}