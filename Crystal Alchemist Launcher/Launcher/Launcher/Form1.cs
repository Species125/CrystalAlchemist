using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace Launcher
{
    public partial class Form1 : Form
    {
        private DirectoryInfo currentDir;
        private DirectoryInfo targetDir;

        private string uri = "http://www.gungnir-arts.com/Download/";
        private string versionFileName = "Version.txt";
        private string exeFileName = "Crystal Alchemist.exe";
        private string zipFilename = "CrystalAlchemist.zip";
        private string subFolder = "Game";

        private string content;
        private string gamePath;
        private string versionPath;

        public Form1()
        {
            InitializeComponent();

            this.currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());

            string target = this.currentDir.FullName + "\\"+this.subFolder;
            if (!Directory.Exists(target)) Directory.CreateDirectory(target);
            this.targetDir = new DirectoryInfo(target);

            this.versionPath = targetDir.FullName + "\\"+this.versionFileName;

            CanPlay();
            this.label1.Visible = false;
            this.progressBar1.Visible = false;

            bool isUpToDate = IsVersionUpToDate();
            if (!isUpToDate)
            {
                this.downloadButton.Visible = true;
            }
        }

        private bool IsVersionUpToDate()
        {
            try
            {
                string versionUri = this.uri + this.versionFileName;
                using (WebClient myWebClient = new WebClient())
                {
                    this.content = myWebClient.DownloadString(versionUri);

                    FileInfo file = new FileInfo(this.versionPath);

                    if (file.Exists)
                    {
                        this.downloadButton.Text = "UPDATE";
                        string text = File.ReadAllText(this.versionPath);

                        string on = "0." + content.Replace(".", "");
                        string lo = "0." + text.Replace(".", "");

                        double onlineVersion = double.Parse(on, CultureInfo.InvariantCulture);
                        double localVersion = double.Parse(lo, CultureInfo.InvariantCulture);

                        if (localVersion >= onlineVersion) return true;                        
                    }
                    else
                    {
                        this.downloadButton.Text = "DOWNLOAD";
                    }               
                }
            }
            catch { }

            return false;
        }

        private void DownloadNewVersion(string name)
        {
            try
            {
                this.label1.Visible = true;
                this.label1.Text = "Start Downloading";

                string gameUri = this.uri + name;
                string path = this.targetDir.FullName + @"\" + name;

                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    myWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    myWebClient.DownloadFileAsync(new Uri(gameUri), path);
                    this.label1.Text = "Downloading...";
                }
            }
            catch { Unzip(); }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.label1.Text = "Download completed!";
            Unzip();
        }

        private void Unzip()
        {
            this.label1.Text = "Unzip files...";
            if (!this.backgroundWorker1.IsBusy) this.backgroundWorker1.RunWorkerAsync();
        }

        private void CanPlay()
        {
            this.label1.Visible = false;
            FileInfo file = new FileInfo(this.targetDir.FullName+"\\"+this.exeFileName);
            if (file.Exists) this.gamePath = file.FullName;
            else this.gamePath = "";

            if (this.gamePath.Length <= 5)
            {
                this.PlayButton.Text = "NOT  INSTALLED";
                this.PlayButton.Enabled = false;
            }
            else
            {
                this.PlayButton.Text = "START";
                this.PlayButton.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(this.gamePath);
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.PlayButton.Enabled = false;
            this.PlayButton.Text = "PLEASE  WAIT";
            this.downloadButton.Visible = false;
            this.progressBar1.Visible = true;
            DownloadNewVersion(this.zipFilename);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                FileInfo[] file = this.targetDir.GetFiles(this.zipFilename, SearchOption.TopDirectoryOnly);

                if (file.Length > 0)
                {
                    FastZip fastZip = new FastZip();
                    //string fileFilter = "-Launcher.exe;-ICSharpCode.SharpZipLib.dll";
                    string fileFilter = "";
                    fastZip.ExtractZip(file[0].FullName, this.targetDir.FullName, fileFilter);

                    Thread.Sleep(500);

                    file[0].Delete();
                }
            }
            catch 
            { 
                this.backgroundWorker1.CancelAsync(); 
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.label1.Text = "Unzip completed!";
            this.PlayButton.Enabled = true;
            this.PlayButton.Text = "START";
            this.progressBar1.Visible = false;

            File.WriteAllText(this.versionPath, this.content);

            CanPlay();
        }
    }
}
