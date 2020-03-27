using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Forms;
using System.IO;
using YDK;
using YDK.Misc;
using System.Text.RegularExpressions;
using NAudio;
using NAudio.Wave;

namespace TextEngine
{
    public partial class Form1 : Form
    {
        string MyPath = "."/*Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName*/;
        Data currentDialog;
        DirectoryInfo currentDir;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        private void LoadDialog(string dir)
        {
            string resourcedir = MyPath + "\\Resource";
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            if (directoryInfo.Exists)
            {
                foreach(FileInfo file in directoryInfo.EnumerateFiles())
                {
                    if (file.Name == "dialog.xml")
                    {
                        currentDir = directoryInfo;
                        Data dialog = Data.FromFile(file.FullName);
                        if (File.Exists(MyPath + "\\Config\\autosave.s"))
                            File.Delete(MyPath + "\\Config\\autosave.s");
                        Tools.Write(MyPath + "\\Config\\autosave.s", file.FullName);
                        currentDialog = dialog;
                        string wav = resourcedir + "\\wav\\" + dialog.GetValue(254);
                        if (File.Exists(wav))
                        {
                            if (outputDevice != null)
                            {
                                outputDevice.Stop();
                                outputDevice.Dispose();
                                outputDevice = null;
                                audioFile.Dispose();
                                audioFile = null;
                            }
                            if (outputDevice == null)
                            {
                                outputDevice = new WaveOutEvent();
                            }
                            if (audioFile == null)
                            {
                                audioFile = new AudioFileReader(wav);
                                outputDevice.Init(audioFile);
                            }
                            outputDevice.Play();
                        }
                        richTextBox1.AppendText(dialog.GetValue(0) + "\n");
                        if(File.Exists(resourcedir+"\\sprites\\" + dialog.GetValue(255)))
                            pictureBox1.Image = Bitmap.FromFile(resourcedir+"\\sprites\\" + dialog.GetValue(255));
                        listView1.Items.Clear();
                        for(int i = 1; i < dialog.values.Length; i++)
                        {
                            if(dialog.GetValue(i) != null && i < 250)
                            {
                                string text = dialog.GetValue(i);
                                listView1.Items.Add(text);
                            }
                        }
                    } else if (file.Name == "image.png")
                    {
                        //pictureBox1.Image = Bitmap.FromFile(file.FullName);
                    }
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void makeFore(Control control)
        {
            control.ForeColor = (control.BackColor.R + control.BackColor.G + control.BackColor.B) > 382 ? Color.Black : Color.White;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string updatedir = MyPath + "\\Update";
            string configdir = MyPath + "\\Config";
            string resourcedir = MyPath + "\\Resource";
            foreach(FileInfo file in new DirectoryInfo(updatedir).EnumerateFiles())
            {
                if (file.Extension == ".ypac")
                {
                    Package.LoadPackage(file.FullName);
                }
            }
            timer1.Start();
            currentDir = new DirectoryInfo(MyPath);
            if (File.Exists(resourcedir+"\\game.ico"))
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(resourcedir+"\\game.ico");
            if(File.Exists(configdir + "\\game.cfg"))
            {
                Text = Tools.Read(configdir + "\\game.cfg");
                openToolStripMenuItem.Visible = false;
            } else
                Text = "TextEngine";

            if (File.Exists(configdir + "\\colorscheme.xml"))
            {
                Data colorscheme = Data.FromFile(configdir + "\\colorscheme.xml");
                menuStrip1.BackColor = Color.FromName(colorscheme.GetValue(0));
                menuStrip2.BackColor = Color.FromName(colorscheme.GetValue(1));
                pictureBox1.BackColor = Color.FromName(colorscheme.GetValue(2));
                richTextBox1.BackColor = Color.FromName(colorscheme.GetValue(3));
                listView1.BackColor = Color.FromName(colorscheme.GetValue(4));
                makeFore(menuStrip1);
                makeFore(menuStrip2);
                makeFore(pictureBox1);
                makeFore(richTextBox1);
                makeFore(listView1);
                listView1.BorderStyle = BorderStyle.None;
                richTextBox1.BorderStyle = BorderStyle.None;
                pictureBox1.BorderStyle = BorderStyle.None;
                if(listView1.BackColor != Color.White)
                    listView1.GridLines = false;
            }



            if(File.Exists(configdir + "\\config.xml"))
            {
                Data data = Data.FromFile(configdir + "\\config.xml");
                if (!string.IsNullOrWhiteSpace(data.GetValue(0)))
                {
                    try
                    {
                        richTextBox1.Font = new Font(FontFamily.GenericSansSerif, (float)double.Parse(data.GetValue(0)), FontStyle.Regular);
                    }
                    catch
                    {

                    }
                }

                if(data.GetValue(1) != null)
                {
                    bool doubleClick = true;
                    bool.TryParse(data.GetValue(1), out doubleClick);
                    doubleClickToSelectToolStripMenuItem.Checked = doubleClick;
                }
            }

            if (File.Exists(configdir + "\\autosave.s"))
            {
                string text = Tools.Read(configdir + "\\autosave.s");
                if(!string.IsNullOrWhiteSpace(text) && File.Exists(text) && new FileInfo(text).Name == "dialog.xml")
                {
                    LoadDialog(Directory.GetParent(text).FullName); 
                }
            }
            else if (Directory.Exists(MyPath + "\\Story"))
            {
                LoadDialog(MyPath + "\\Story");
                openToolStripMenuItem.Visible = false;
            }
            toolStripTextBox1.Text = richTextBox1.Font.Size.ToString();
        }

        private void FilleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(doubleClickToSelectToolStripMenuItem.Checked)
            {
                Reply();
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                LoadDialog(Directory.GetParent(openFileDialog1.FileName).FullName);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            int isDialog = 0;
            foreach(FileInfo file in Directory.GetParent(currentDir.FullName).EnumerateFiles())
            {
                if(file.Name == "dialog.xml")
                {
                    isDialog++;
                }
            }
            if(isDialog > 0)
            {
                backToolStripMenuItem.Enabled = true;
            } else
            {
                backToolStripMenuItem.Enabled = false;
            }
            if (!listView1.Focused)
            {
                string updatedir = MyPath + "\\Update";
                string configdir = MyPath + "\\Config";
                string resourcedir = MyPath + "\\Resource";
                if (File.Exists(resourcedir + "\\sprites\\" + currentDialog.GetValue(255)))
                    pictureBox1.Image = Bitmap.FromFile(resourcedir + "\\sprites\\" + currentDialog.GetValue(255));
            }
            splitContainer1.BackColor = pictureBox1.BackColor;
        }

        private void BackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
                audioFile.Dispose();
                audioFile = null;
            }
            richTextBox1.AppendText(">> Back \n");
            LoadDialog(Directory.GetParent(currentDir.FullName).FullName);
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string updatedir = MyPath + "\\Update";
            string configdir = MyPath + "\\Config";
            string resourcedir = MyPath + "\\Resource";
            if (File.Exists(MyPath + "\\Sounds\\selectionchanged.wav"))
            {
                SoundPlayer player = new SoundPlayer(MyPath + "\\Sounds\\selectionchanged.wav");
                player.Play();
                player.Dispose();
            }
            if (listView1.SelectedItems.Count > 0)
            {
                int id = currentDialog.FindValue(listView1.SelectedItems[0].Text);
                if (id != 0)
                {
                    Data nextDialog = Data.FromFile(currentDir.FullName + "\\" + id.ToString() + "\\dialog.xml");
                    if (nextDialog.GetValue(255) != null && File.Exists(resourcedir + "\\sprites\\" + nextDialog.GetValue(255)))
                        pictureBox1.Image = Bitmap.FromFile(resourcedir + "\\sprites\\" + nextDialog.GetValue(255));
                }
            }
        }

        private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Regex pattern = new Regex("[\"':<>;,abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ{}_+=]");
            toolStripTextBox1.Text = pattern.Replace(toolStripTextBox1.Text, "").Replace("]", "").Replace("[", "").Replace("|", "").Replace("-", "");
            if (!string.IsNullOrWhiteSpace(toolStripTextBox1.Text))
            {
                try
                {
                    richTextBox1.Font = new Font(FontFamily.GenericSansSerif, (float)double.Parse(toolStripTextBox1.Text), FontStyle.Regular);
                }
                catch
                {

                }
            }
        }

        private void ListView1_MouseHover(object sender, EventArgs e)
        {
            listView1.Focus();
        }

        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!doubleClickToSelectToolStripMenuItem.Checked)
            {
                Reply();
            }
        }

        private void Reply()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int id = currentDialog.FindValue(listView1.SelectedItems[0].Text);
                if (id != 0)
                {
                    if(currentDialog.GetValue(id)[0] == '>')
                        richTextBox1.AppendText(currentDialog.GetValue(id) + "\n");
                    else if (currentDialog.GetValue(id)[0] != '>')
                        richTextBox1.AppendText("You: "+currentDialog.GetValue(id) + "\n");
                    LoadDialog(currentDir.FullName + "\\" + id.ToString());
                }
            }
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Data config = new Data();
            config.SetValue(0, toolStripTextBox1.Text);
            config.SetValue(1, doubleClickToSelectToolStripMenuItem.Checked.ToString());
            config.ToFile(MyPath + "\\Config\\config.xml");
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                Reply();
        }
    }
}
