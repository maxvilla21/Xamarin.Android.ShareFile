using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ShareFileApp
{
    [Activity(Label = "ShareFileApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button btnShareLocalFile;
        private Button btnShareRemoteFile;

        private ShareFile sharefile;
        string testFilePath;

        const string remoteFileUrl = "https://cupitcontent.blob.core.windows.net/images/cup-it.png";
        const string testFileName = "testfile.png";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Main);

            sharefile=new ShareFile();

            btnShareLocalFile = FindViewById<Button>(Resource.Id.btnShareLocalFile);
            btnShareRemoteFile = FindViewById<Button>(Resource.Id.btnShareRemoteFile);

            btnShareLocalFile.Click += BtnShareLocalFile_Click;
            btnShareRemoteFile.Click += BtnShareRemoteFile_Click;
            DownloadTestFile();
        }

        private async void BtnShareRemoteFile_Click(object sender, System.EventArgs e)
        {
          await  sharefile.ShareRemoteFile(remoteFileUrl, testFileName, "Share remote file");
        }

        private void BtnShareLocalFile_Click(object sender, System.EventArgs e)
        {
            sharefile.ShareLocalFile(testFilePath);




        }
        private async void DownloadTestFile()
        {
            using (var webClient = new WebClient())
            {
                var uri = new System.Uri(remoteFileUrl);
                var bytes = await webClient.DownloadDataTaskAsync(uri);
                testFilePath = WriteFile(testFileName, bytes);
            }

            btnShareLocalFile.Visibility = ViewStates.Visible;
        }
        private string WriteFile(string fileName, byte[] bytes)
        {
            var localFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string localPath = System.IO.Path.Combine(localFolder, fileName);
            File.WriteAllBytes(localPath, bytes); // write to local storage

            return string.Format("file://{0}/{1}", localFolder, fileName);
        }
    }
}

