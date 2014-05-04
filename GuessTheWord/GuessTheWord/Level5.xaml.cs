using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class Level5 : Page
    {
        private StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
        private string fileInPC = "tempFourPicOneWord.txt";
        private string fileToUpload = "FourPicOneWord.txt";

        private LiveConnectClient client;
        private LiveAuthClient authClient;
        public Level5()
        {
            this.InitializeComponent();
            createFile();

            Loaded += MainPage_Loaded;
            txtB1.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            txtB2.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            txtB3.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            txtB4.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;

            initialize();

        }

        private async void initialize()
        {
            try
            {
                authClient = new LiveAuthClient();
                LiveLoginResult authLogResult = await authClient.LoginAsync(new List<string>() { "wl.signin", "wl.basic", "wl.skydrive_update" });

                if (authLogResult.Status == LiveConnectSessionStatus.Connected)
                {
                    client = new Microsoft.Live.LiveConnectClient(authClient.Session);

                    LiveOperationResult opResult = await client.GetAsync("me");
                    dynamic result = opResult.Result;

                    //tblUserName.Text = result.name;
                }
            }
            catch (Exception e)
            {
                //tblUserName.Text = "Error";
            }
        }

        private async void UploadToSkydrive(object sender, RoutedEventArgs e)
        {

            try
            {
                var file = await storageFolder.GetFileAsync(fileInPC);
                var upfile = await storageFolder.GetFileAsync(fileToUpload);
                //StorageFile sampleFile = await storageFolder.GetFileAsync(fileInPC);
                string textFile = await Windows.Storage.FileIO.ReadTextAsync(file);
                string textUpFile = await Windows.Storage.FileIO.ReadTextAsync(upfile);
                //txtTemp.Text = " PC "+textFile;
                //txtSky.Text = "Sky "+textUpFile;
                try
                {
                    GlobalV.tempLevel = Convert.ToInt32(textFile);
                    GlobalV.Level = Convert.ToInt32(textUpFile);
                }
                catch (OverflowException)
                {

                }
                catch (FormatException)
                {

                }

                if (GlobalV.tempLevel > GlobalV.Level)
                {
                    GlobalV.Level = GlobalV.tempLevel;
                    StorageFile newFile = await storageFolder.CreateFileAsync(fileToUpload, CreationCollisionOption.ReplaceExisting);
                    await Windows.Storage.FileIO.WriteTextAsync(newFile, GlobalV.Level.ToString());
                }
                else
                {
                }

                string textFile1 = await Windows.Storage.FileIO.ReadTextAsync(file);
                string textUpFile1 = await Windows.Storage.FileIO.ReadTextAsync(upfile);
              
                await client.BackgroundUploadAsync("me/skydrive", fileToUpload, upfile, OverwriteOption.Overwrite);

                MessageDialog dlg = new MessageDialog("File uploaded success fully");
                dlg.ShowAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageDialog dlg = new MessageDialog("File did not upload success fully");
                dlg.ShowAsync();

            }
        }
        

        private async void createFile()
        {

            StorageFile newFile = await storageFolder.CreateFileAsync(fileInPC, CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(newFile, GlobalV.Level.ToString());
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Latter_Click(object sender, RoutedEventArgs e)
        {
            if (txtB1.Text == "")
                txtB1.Text = (String)((Button)sender).Content;
            else if ((txtB1.Text != "") && (txtB2.Text == ""))
                txtB2.Text = (String)((Button)sender).Content;
            else if ((txtB1.Text != "") && (txtB2.Text != "") && (txtB3.Text == ""))
                txtB3.Text = (String)((Button)sender).Content;
            else if ((txtB1.Text != "") && (txtB2.Text != "") && (txtB3.Text != "") && (txtB4.Text == ""))
                txtB4.Text = (String)((Button)sender).Content;

        }

        private void txtB1_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            txtB1.Text = "";
            txtB1.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtB2_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            txtB2.Text = "";
            txtB2.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtB3_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            txtB3.Text = "";
            txtB3.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtB4_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            txtB4.Text = "";
            txtB4.Foreground = new SolidColorBrush(Colors.Black);
        }

        private async void eSubmit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((txtB1.Text + txtB2.Text + txtB3.Text + txtB4.Text)).ToLower().Equals("jump"))
            {
                this.Frame.Navigate(typeof(Level6));

                GlobalV.Level = 6;

                try
                {
                    var file = await storageFolder.GetFileAsync(fileInPC);
                    if (file != null)
                    {
                        StorageFile deleteFile = await storageFolder.GetFileAsync(fileInPC);
                        await deleteFile.DeleteAsync(StorageDeleteOption.Default);


                        StorageFile newFile = await storageFolder.CreateFileAsync(fileInPC, CreationCollisionOption.ReplaceExisting);
                        await Windows.Storage.FileIO.WriteTextAsync(newFile, GlobalV.Level.ToString());
                        // await writer.WriteLineAsync(UserText.Text);

                    }
                    else
                    {
                        //StorageFile createFile = await storageFolder.CreateFileAsync(fileInPC);
                        StorageFile newFile = await storageFolder.CreateFileAsync(fileInPC, CreationCollisionOption.ReplaceExisting);
                        await Windows.Storage.FileIO.WriteTextAsync(newFile, GlobalV.Level.ToString());

                    }
                }
                catch (FileNotFoundException)
                {
                    // tblUserName.Text = "file might not downloaded properly";
                }
            }
            else
            {
                txtB1.Foreground = new SolidColorBrush(Colors.Red);
                txtB2.Foreground = new SolidColorBrush(Colors.Red);
                txtB3.Foreground = new SolidColorBrush(Colors.Red);
                txtB4.Foreground = new SolidColorBrush(Colors.Red);
           
            }
        }


        private void eSubmit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            eSubmit.Stroke = new SolidColorBrush(Colors.Red);
        }

        private void eSubmit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            eSubmit.Stroke = new SolidColorBrush(Colors.White);
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
                grdHeader.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdImages.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdLatters.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdWord.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                scrViewImg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                BottomAppBar1.Visibility = Visibility.Collapsed;
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                //this.Frame.Navigate(typeof(MainPage));
                grdHeader.Visibility = Windows.UI.Xaml.Visibility.Visible;
                grdImages.Visibility = Windows.UI.Xaml.Visibility.Visible;
                grdLatters.Visibility = Windows.UI.Xaml.Visibility.Visible;
                grdWord.Visibility = Windows.UI.Xaml.Visibility.Visible;
                scrViewImg.Visibility = Windows.UI.Xaml.Visibility.Visible;
                BottomAppBar1.Visibility = Visibility.Visible;
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                potratemoode.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                grdHeader.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdImages.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdLatters.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdWord.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                scrViewImg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                BottomAppBar1.Visibility = Visibility.Collapsed;
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                potratemoode.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

            MessageDialog dlg = new MessageDialog("Use the four images given to guess the correct word," + "\nTap on white space before select the latter." + "\nDouble click on latter to remove any latter which entered" + "\nUse the button next to the word to test the answer, if word is correct you will move into next level, if not latters will indicate in red colour.");
            //MessageBox.Show(help, "How To Play ", MessageBoxButton.OK);
            dlg.ShowAsync();
        }


    }
}
