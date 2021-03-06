﻿using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.Helper;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace YummyCookWindowsUniversal
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Allows tracking page views, exceptions and other telemetry through the Microsoft Application Insights service.
        /// </summary>
        public static Microsoft.ApplicationInsights.TelemetryClient TelemetryClient;

        public static Frame ContentFrame = null;
        public static Frame MainFrame = null;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            TelemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            ConfigHelper.ConfigAppSetting();

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if(!ConfigHelper.CheckUserCache())
                {
                    rootFrame.Navigate(typeof(StartPage), e.Arguments);
                }
                else
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
            }
            Window.Current.Activate();

        }

        public static void SetUpTitleBar(bool isGray=false)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (!isGray)
            {
               
                titleBar.BackgroundColor = (App.Current.Resources["CookThemeDark"] as SolidColorBrush).Color;
                titleBar.ForegroundColor = Colors.White;
                titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
                titleBar.InactiveForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = (App.Current.Resources["CookThemeDark"] as SolidColorBrush).Color;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveBackgroundColor = titleBar.BackgroundColor;
                titleBar.ButtonInactiveForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = (App.Current.Resources["CookTheme"] as SolidColorBrush).Color;
            }
            else
            {
                
                titleBar.BackgroundColor = Colors.LightGray;
                titleBar.ForegroundColor = Colors.Black;
                titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
                titleBar.InactiveForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.LightGray;
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonInactiveBackgroundColor = titleBar.BackgroundColor;
                titleBar.ButtonInactiveForegroundColor = Colors.Black;
                titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            
            base.OnActivated(args);
        }
    }
}
