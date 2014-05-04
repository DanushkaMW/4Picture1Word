using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GuessTheWord
{
    class ReadWrite
    {
        public StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
        public string theFileToGet = "FourPicOneWord.txt";
        public string tempFile = "tempFourPicOneWord.txt";
        public string text;

        public async void readTempFile()
        {
            try
            {
                StorageFile file = await storageFolder.GetFileAsync(tempFile);
                if (file != null)
                {
                    text = await Windows.Storage.FileIO.ReadTextAsync(file);
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

    }


}
