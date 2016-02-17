using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;

using Microsoft.Win32;
namespace OEM_Tweaker
{
    public partial class Form1 : Form
    {
        public static string re = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           {
               BitmapImage bit = new BitmapImage(new Uri(openFileDialog1.FileName));
               var h = bit.PixelHeight;
               var w = bit.PixelWidth;
               if (h == 96 && h == w)
               {
                   pictureBox1.ImageLocation = openFileDialog1.FileName;                    
               }else
               {
                   MessageBox.Show("bmp file not given in the required dimensions");
               }
           }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,"The logo should be strictly a bmp file with 96x96 pixels and keep in a safe location");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(this,"please make a back up of the original OEM before trying this tool,by clicking make backeup and can be latter restored by opening the backup and adding to revert the changes");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                if(File.Exists(openFileDialog1.FileName))
                {
                    BitmapImage bit = new BitmapImage(new Uri(openFileDialog1.FileName));
                    var h = bit.PixelHeight;
                    var w = bit.PixelWidth;
                    if (h == 96 && h == w)
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", true);
                        key.SetValue("Logo", openFileDialog1.FileName);
                        key.SetValue("Manufacturer", textBox1.Text);
                        key.SetValue("Model", textBox2.Text);
                        key.SetValue("SupportHours", textBox3.Text);
                        key.SetValue("SupportPhone", textBox4.Text);
                        key.SetValue("SupportURL", textBox5.Text);
                      
                    }
                    else
                    {
                        MessageBox.Show("bmp file not given in the required dimensions");
                    }
                }
                else
                {
                    MessageBox.Show("Logo file not found!");
                }
            }
            else
            {
                MessageBox.Show("Please fill all fields");
            }
        }

        public void ExportKey(string RegKey, string SavePath)
        {
            string path = "\"" + SavePath + "\"";
            string key = "\"" + RegKey + "\"";

            var proc = new Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;
                proc = Process.Start("regedit.exe", "/e " + path + " " + key + "");

                if (proc != null) proc.WaitForExit();
            }
            finally
            {
                if (proc != null) proc.Dispose();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExportKey(re, saveFileDialog1.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", true);
                try {
                    key.DeleteValue("Logo");
                    key.DeleteValue("Manufacturer");
                    key.DeleteValue("Model");
                    key.DeleteValue("SupportHours");
                    key.DeleteValue("SupportPhone");
                    key.DeleteValue("SupportURL");
                }
                catch(Exception h)
                {

                }
                var proc = new Process();
                try
                {
                    proc.StartInfo.FileName = "regedit.exe";
                    proc.StartInfo.UseShellExecute = false;
                    proc = Process.Start("regedit.exe", "/S " + openFileDialog2.FileName);

                    if (proc != null) proc.WaitForExit();
                }
                finally
                {
                    if (proc != null) proc.Dispose();
                }
            }

       }
    }
}
