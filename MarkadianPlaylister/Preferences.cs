using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkadianPlaylister
{
    public partial class Preferences : Form
    {
        MarkadianSettings settings;
        public String filePath;
        public String resourceDirectory;
        public Preferences(MarkadianSettings markadianSettings)
        {
            InitializeComponent();
            settings = markadianSettings;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            settings.bitRateSelector = bitRateSelector.Text;
            settings.enableQueue = enableQueue.Checked;
            settings.filePath = filePath;
            settings.resourceDirectory = resourceDirectory;
            settings.searchCount = countNumber.Value.ToString();
            settings.videoQuality = videoQualityBox.Text;
            settings.fileType = fileTypeBox.Text;
            SettingsManager.SaveSettings(settings);
            this.Close();

        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            ThemeManager.SetTheme(settings.theme == "Dark" ? AppTheme.Dark : AppTheme.Light);
            ThemeManager.ApplyTheme(this);
            filePath = settings.filePath;
            resourceDirectory = settings.resourceDirectory;
            bitRateSelector.Text = settings.bitRateSelector;
            pathDisplay.Text = "Current Path:" + settings.filePath;
            enableQueue.Checked = settings.enableQueue;
            countNumber.Value = int.Parse(settings.searchCount);
            resourceDirectoryPath.Text = "Resource Directory: " + settings.resourceDirectory;
            videoQualityBox.Text = settings.videoQuality ?? "best";
            fileTypeBox.Text = settings.fileType ?? "mp3";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select a new location for your music";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = folderBrowserDialog1.SelectedPath;
                pathDisplay.Text = filePath.ToString();
                settings.filePath = filePath;
            }
            else { MessageBox.Show("Not a valid path"); }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select a new location for your resources";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                resourceDirectory = folderBrowserDialog1.SelectedPath;
                resourceDirectoryPath.Text = "Resource Directory: " + resourceDirectory;
                settings.resourceDirectory = resourceDirectory;
            }
            else { MessageBox.Show("Not a valid path"); }
        }
    }
}
