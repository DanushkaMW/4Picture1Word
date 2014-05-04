using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GuessTheWord
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinalPage : Page
    {
        public FinalPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }


        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {


            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                //this.Frame.Navigate(typeof(SnapMainpage));
                //LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                //grdHeader.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //grdImages.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //grdLatters.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //grdWord.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //scrViewImg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //BottomAppBar1.Visibility = Visibility.Collapsed;
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                //this.Frame.Navigate(typeof(MainPage));
                //grdHeader.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //grdImages.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //grdLatters.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //grdWord.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //scrViewImg.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //BottomAppBar1.Visibility = Visibility.Visible;
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                potratemoode.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                //grdHeader.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //grdImages.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //grdLatters.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //grdWord.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //scrViewImg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //BottomAppBar1.Visibility = Visibility.Collapsed;
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                potratemoode.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void Ellipse_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Level1));
        }

    }
}
