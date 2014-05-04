using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    public sealed partial class MainPage : Page
    {
        private StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
        private string theFileToGet = "FourPicOneWord.txt";
        private string tempFile = "tempFourPicOneWord.txt";
       

        private LiveConnectClient client;
        private LiveAuthClient authClient;

        //static int level;
        public MainPage()
        {
            
            this.InitializeComponent();
            
            //createFile();
            Loaded += MainPage_Loaded;
            initialize();
            createFile();
            readTempFile();
            readFileFromSkydrive();

          
        }


        public async void readTempFile()
        {
            try
            {
                var file = await storageFolder.GetFileAsync(tempFile);
                if (file != null)
                {
                    string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                    try
                    {
                        GlobalV.Level = Convert.ToInt32(text);
                    }
                    catch (OverflowException)
                    {

                    }
                    catch (FormatException)
                    {

                    }
                }
            }//end try
            catch (FileNotFoundException ex)
            {

            }

        }//end  readTempFile()

        public async void readFileFromSkydrive()
        {
            try
            {
                var file = await storageFolder.GetFileAsync(theFileToGet);
                if (file != null)
                {
                    string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                    try
                    {
                        GlobalV.tempLevel = Convert.ToInt32(text);
                    }
                    catch (OverflowException)
                    {

                    }
                    catch (FormatException)
                    {

                    }
                }
            }//end try
            catch (FileNotFoundException)
            {
            }
        }

        public void checkLevels()
        {
            if (GlobalV.tempLevel > GlobalV.Level)
            {
                GlobalV.Level = GlobalV.tempLevel;
            }
            else
            {
            }
        }

        public async void createFile()
        {
            StorageFile newFile = await storageFolder.CreateFileAsync(theFileToGet, CreationCollisionOption.ReplaceExisting);
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

       
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {


            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                potratemoode.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                snappedState.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                potratemoode.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void Ellipse_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            checkLevels();
            if (GlobalV.Level == 1)
                this.Frame.Navigate(typeof(Level1));
            else if (GlobalV.Level == 2)
                this.Frame.Navigate(typeof(Level2));
            else if (GlobalV.Level == 3)
                this.Frame.Navigate(typeof(Level3));
            else if (GlobalV.Level == 4)
                this.Frame.Navigate(typeof(Level4));
            else if (GlobalV.Level == 5)
                this.Frame.Navigate(typeof(Level5));
            else if (GlobalV.Level == 6)
                this.Frame.Navigate(typeof(Level6));
            else if (GlobalV.Level == 7)
                this.Frame.Navigate(typeof(Level7));
            else if (GlobalV.Level == 8)
                this.Frame.Navigate(typeof(Level8));
            else if (GlobalV.Level == 9)
                this.Frame.Navigate(typeof(Level9));
            else if (GlobalV.Level == 10)
                this.Frame.Navigate(typeof(Level10));
            else if(GlobalV.Level == 11)
                this.Frame.Navigate(typeof(FinalPage));
        }

        private void Ellipse_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            ePlay.Stroke = new SolidColorBrush(Colors.Red);
        }

        private void Ellipse_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            ePlay.Stroke = new SolidColorBrush(Colors.White);
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

                    tblUserName.Text = "Welcome : "+result.name;
                }
            }
            catch (Exception e)
            {
                tblUserName.Text = "Error";
            }
        }

        private async Task<bool> DownloadFile(string fileID, string whereToSave)
        {
            bool fileDownloadResult = false;
            string skydriveFileId = fileID;

            try
            {
                LiveConnectClient client = new Microsoft.Live.LiveConnectClient(authClient.Session);

                //create a target file with the correct name and extension
                StorageFile downloadedFile = await storageFolder.CreateFileAsync(whereToSave, CreationCollisionOption.ReplaceExisting);

                //pass in the file id (path) and the StorageFile you created and it's filled with the downloaded files data
                await client.BackgroundDownloadAsync(skydriveFileId + "/content", downloadedFile);
                fileDownloadResult = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message.ToString());
                fileDownloadResult = false;
            }

            return fileDownloadResult;
        }


        private async void SkydriveDownload_Click(object sender, RoutedEventArgs e)
        {

            string id = string.Empty;
            try
            {
                LiveConnectClient client = new Microsoft.Live.LiveConnectClient(authClient.Session);
                LiveOperationResult opResult = await client.GetAsync("me/skydrive/files");

                dynamic result = opResult.Result;

                List<object> items = opResult.Result["data"] as List<object>;

                foreach (object item in items)
                {
                    IDictionary<string, object> file = item as IDictionary<string, object>;

                    if (file["name"].ToString() == theFileToGet)
                    {
                        id = file["id"].ToString();
                        tblUserName.Text = file["id"].ToString();
                        //file found
                        //terminate loop
                        break;
                    }
                }

                if (id != "")
                {
                    bool downloadOperationResult = await DownloadFile(id, theFileToGet);

                    if (!downloadOperationResult)
                    {
                        tblUserName.Text = "File Downloading Failed";
                    }
                    else
                    {
                        tblUserName.Text = "File Downloading Successful";
                        var file = await storageFolder.GetFileAsync(theFileToGet);
                        string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                        try
                        {
                            GlobalV.tempLevel = Convert.ToInt32(text);
                        }
                        catch (OverflowException of)
                        {

                        }
                        catch (FormatException fx)
                        {

                        }

                    }
                }//end if (id != "")
                else
                {
                    tblUserName.Text = "file did not downloaded continue play";
                }


            }
            catch (Exception ex)
            {
                MessageDialog dlg = new MessageDialog("File did not downloaded from skydrive. please continue play");
                dlg.ShowAsync();
            }
            

        }//end download method


    }
}
