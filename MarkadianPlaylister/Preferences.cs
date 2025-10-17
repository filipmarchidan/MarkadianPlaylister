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
            SettingsManager.SaveSettings(settings);
            this.Close();

        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            filePath = settings.filePath;
            bitRateSelector.Text = settings.bitRateSelector;
            pathDisplay.Text = "Current Path:" + settings.filePath;
            enableQueue.Checked = settings.enableQueue;
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
    }
}
