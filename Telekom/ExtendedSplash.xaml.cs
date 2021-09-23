using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Telekom
{
    partial class ExtendedSplash
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        private SplashScreen splash; // Variable to hold the splash screen object.
        private double ScaleFactor; //Variable to hold the device scale factor (use to determine phone screen resolution)

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            ScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();

            // Restore the saved session state if necessary
            Telekom();
        }

        async void statusText(string text)
        {
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

        async void hideStatusText()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                StatusBarProgressIndicator indicator = statusBar.ProgressIndicator;
                await indicator.HideAsync();
            }
        }

        async void Telekom()
        {
            statusText("Načítavam...");

            Debug.WriteLine("[tlkm_extendedsplash] ´500ms wait"); //vypadá to tak že bez neho to nepresmeruje na page
            await System.Threading.Tasks.Task.Delay(500);

            Debug.WriteLine("[tlkm_extendedsplash] deviceId: " + App.TLKM.deviceId.ToString());

            if (App.TLKM.localSettings.Values["hasAccount"] as string == "yes")
            {
                statusText("Prihlasujem...");

                Debug.WriteLine("[tlkm_extendedsplash] settings - hasAccount - retrieving information");

                Debug.WriteLine("[tlkm_extendedsplash] settings - deviceId: " + App.TLKM.localSettings.Values["deviceId"]);

                App.TLKM.accessToken = (string)App.TLKM.localSettings.Values["accessToken"];
                Debug.WriteLine("[tlkm_extendedsplash] settings - accessToken: " + App.TLKM.accessToken);

                App.TLKM.refreshToken = (string)App.TLKM.localSettings.Values["refreshToken"];
                Debug.WriteLine("[tlkm_extendedsplash] settings - refreshToken: " + App.TLKM.refreshToken);

                App.TLKM.serviceId = (long)App.TLKM.localSettings.Values["serviceId"];
                Debug.WriteLine("[tlkm_extendedsplash] settings - serviceId: " + App.TLKM.serviceId);

                bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.login());
                if (success)
                {
                    Debug.WriteLine("[tlkm_extendedsplash] logged in successfully!");
                    statusText("Prihlásenie úspešné...");
                    bool dash_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.dashboard());
                    if (!dash_success)
                    {
                        await App.TLKM.showError();
                    }
                    else
                    {
                        hideStatusText();
                        rootFrame.Navigate(typeof(dashboard));
                        Window.Current.Content = rootFrame;
                    }
                }
                else
                {
                    if (App.TLKM.lastCode == "hal.security.authentication.access_token.invalid")
                    {
                        statusText("Generujem nový token...");
                        bool regen_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.regen_token());
                        if (regen_success)
                            Debug.WriteLine("[tlkm_extendedsplash] token regenerated successfully!");
                        else
                            await App.TLKM.showError();

                        bool sec_login_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.login());
                        if (sec_login_success)
                        {
                            Debug.WriteLine("[tlkm_extendedsplash] login attempt 2 - logged in successfully!");
                            statusText("Prihlásenie úspešné...");
                            bool dash_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.dashboard());
                            if (!dash_success)
                            {
                                await App.TLKM.showError();
                            }
                            else
                            {
                                hideStatusText();
                                rootFrame.Navigate(typeof(dashboard));
                                Window.Current.Content = rootFrame;
                            }
                        }
                        else
                            await App.TLKM.showError();
                    }
                    else
                    {
                        await App.TLKM.showError();
                    }
                }


            }
            else
            {
                Debug.WriteLine("[tlkm_extendedsplash] settings - no account - proceeding to setup");

                hideStatusText();
                rootFrame.Navigate(typeof(setup_pin));
                Window.Current.Content = rootFrame;
            }
        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        void PositionImage()
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

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
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
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }

    }
}