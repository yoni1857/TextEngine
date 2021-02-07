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
using System.IO.Compression;
using YDK;
using YDK.Misc;
using System.Text.RegularExpressions;
using NAudio;
using NAudio.Wave;
using System.Speech.Synthesis;
using System.Threading;

namespace TextEngine
{
    public partial class Form1 : Form
    {
        string MyPath = "."/*Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName*/;
        Data currentDialog;
        ZipArchive currentArchive;
        string currentArchiveName;
        private WaveOutEvent outputDevice;
        private WaveStream waveStream;
        private SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
        private string desc = "TestEngine Game.";
        private Color playerText = Color.GreenYellow;
        private Color otherText = Color.Blue;
        private ZipArchive resourceArchive;
        private Queue<(ZipArchive, string)> history;

        private void LoadDialog(ZipArchive storyArchive, string storyArchiveName, ZipArchive resourceArchive)
        {
            if (storyArchive != null)
            {
                
                foreach (ZipArchiveEntry file in storyArchive.Entries)
                {
                    if (file.Name == "dialog.ydkl")
                    {
                        currentArchive = storyArchive;
                        currentArchiveName = storyArchiveName;
                        if (history.Count == 3)
                        {
                            history.Dequeue();
                            history.Enqueue((currentArchive, currentArchiveName));
                        }
                        Data dialog = Data.FromStream(file.Open());
                        if (File.Exists(MyPath + "\\Config\\autosave.s"))
                            File.Delete(MyPath + "\\Config\\autosave.s");
                        Tools.Write(MyPath + "\\Config\\autosave.s", storyArchiveName);
                        currentDialog = dialog;
                        string wav = "\\wav\\" + dialog.GetValue(254);
                        if (resourceArchive.GetEntry(wav) != null)
                        {
                            if (outputDevice != null)
                            {
                                outputDevice.Stop();
                                outputDevice.Dispose();
                                outputDevice = null;
                                waveStream.Dispose();
                                waveStream = null;
                            }
                            if (outputDevice == null)
                            {
                                outputDevice = new WaveOutEvent();
                            }
                            if (waveStream == null)
                            {
                                WaveStream stream = (WaveStream)resourceArchive.GetEntry(wav).Open();
                                outputDevice.Init(stream);
                            }
                            outputDevice.Play();
                        }
                        richTextBox1.AppendText(dialog.GetValue(0) + "\n");
                        richTextBox1.Select(
                            richTextBox1.Text.Length-dialog.GetValue(0).Length-1,
                            dialog.GetValue(0).Length
                            );
                        richTextBox1.SelectionColor = otherText;
                        richTextBox1.DeselectAll();
                        if (storyTTSToolStripMenuItem.Checked)
                        {
                            SpeechSynthesizer.SpeakAsync(dialog.GetValue(0));
                        }
                        if (resourceArchive.GetEntry("\\sprites\\" + dialog.GetValue(255))!= null)
                            pictureBox1.Image = Bitmap.FromStream(resourceArchive.GetEntry("\\sprites\\" + dialog.GetValue(255)).Open());
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
            history = new Queue<(ZipArchive, string)>();
            string updatedir = MyPath + "\\Update";
            string configdir = MyPath + "\\Config";
            if(File.Exists(MyPath+"\\res64.pak"))
                resourceArchive = ZipFile.Open(MyPath + "\\res64.pak", ZipArchiveMode.Update);
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            if(Directory.Exists(updatedir))
                foreach(FileInfo file in new DirectoryInfo(updatedir).EnumerateFiles())
                {
                    if (file.Extension == ".ypac")
                    {
                        Package.LoadPackage(file.FullName);
                    }
                }
            timer1.Start();
            currentArchive = ZipFile.Open(MyPath + "\\Story\\story.pak", ZipArchiveMode.Update);
            currentArchiveName = "\\Story\\story.pak";
            if (resourceArchive.GetEntry("game.ico") != null)
                Icon = new System.Drawing.Icon(resourceArchive.GetEntry("game.ico").Open());
            if(resourceArchive.GetEntry("gametitle") != null)
            {
                Text = (string)Tools.Read(resourceArchive.GetEntry("gametitle").Open());
                openToolStripMenuItem.Visible = false;
            } else
                Text = "TextEngine";

            if (resourceArchive.GetEntry("gametdesc") != null)
            {
                desc = (string)Tools.Read(resourceArchive.GetEntry("gamedesc").Open());
            }

            if (File.Exists(configdir + "\\colorscheme.xml"))
            {
                Data colorscheme = Data.FromFile(configdir + "\\colorscheme.xml");
                menuStrip1.BackColor = Color.FromName(colorscheme.GetValue(0));
                menuStrip2.BackColor = Color.FromName(colorscheme.GetValue(1));
                pictureBox1.BackColor = Color.FromName(colorscheme.GetValue(2));
                richTextBox1.BackColor = Color.FromName(colorscheme.GetValue(3));
                listView1.BackColor = Color.FromName(colorscheme.GetValue(4));
                playerText = Color.FromName(colorscheme.GetValue(5));
                otherText = Color.FromName(colorscheme.GetValue(6));
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
                if (data.GetValue(2)!=null)
                {
                    bool StoryTTS = true;
                    bool.TryParse(data.GetValue(2), out StoryTTS);
                    storyTTSToolStripMenuItem.Checked = StoryTTS;
                }
                if (data.GetValue(2) != null)
                {
                    bool selectionTTS = true;
                    bool.TryParse(data.GetValue(3), out selectionTTS);
                    selectedOptionTTSToolStripMenuItem.Checked = selectionTTS;
                }
            }

            if (File.Exists(configdir + "\\autosave.s"))
            {
                var archiveName = (string)Tools.Read(MyPath + configdir + "\\autosave.s");
                LoadDialog(ZipFile.OpenRead(archiveName), archiveName ,resourceArchive);
            }
            else if (Directory.Exists(MyPath + "\\Story"))
            {
                LoadDialog(currentArchive, currentArchiveName,resourceArchive);
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
                LoadDialog(ZipFile.OpenRead(openFileDialog1.FileName), openFileDialog1.FileName, resourceArchive);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if(history.Count > 0)
            {
                backToolStripMenuItem.Enabled = true;
            } else
            {
                backToolStripMenuItem.Enabled = false;
            }
            splitContainer1.BackColor = pictureBox1.BackColor;

            listView1.Font = new Font(listView1.Font.FontFamily.Name,
                (richTextBox1.Font.Size / 2) + 2,
                FontStyle.Regular
                );
        }

        private void BackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
                waveStream.Dispose();
                waveStream = null;
            }
            richTextBox1.AppendText(">> Back \n");
            Queue<(ZipArchive, string)> actualHistory = (Queue<(ZipArchive, string)>)history.Reverse();
            var archive = actualHistory.Dequeue();
            LoadDialog(archive.Item1,archive.Item2 ,resourceArchive);
            history = (Queue<(ZipArchive, string)>)actualHistory.Reverse();
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string updatedir = MyPath + "\\Update";
            string configdir = MyPath + "\\Config";
            if (resourceArchive.GetEntry("\\wav\\selectionchanged.wav") != null)
            {
                SoundPlayer player = new SoundPlayer(resourceArchive.GetEntry("\\wav\\selectionchanged.wav").Open());
                player.Play();
                player.Dispose();
            }
            if (listView1.SelectedItems.Count > 0)
            {
                int id = currentDialog.FindValue(listView1.SelectedItems[0].Text);
                if (id != 0)
                {
                    if(selectedOptionTTSToolStripMenuItem.Checked && SpeechSynthesizer.State != SynthesizerState.Speaking) {
                        SpeechSynthesizer.Speak(listView1.SelectedItems[0].Text);
                    }
                }
            }
        }

        private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            float fontSize;
            float.TryParse(toolStripTextBox1.Text, out fontSize);
            toolStripTextBox1.Text = fontSize.ToString();
            if (!string.IsNullOrWhiteSpace(toolStripTextBox1.Text))
            {
                try
                {
                    richTextBox1.Font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Regular);
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
            if (listView1.SelectedItems.Count > 0 && SpeechSynthesizer.State != SynthesizerState.Speaking)
            {
                int id = currentDialog.FindValue(listView1.SelectedItems[0].Text);
                if (id != 0)
                {
                    if (currentDialog.GetValue(id)[0] == '>')
                    {
                        richTextBox1.AppendText(currentDialog.GetValue(id) + "\n");
                        richTextBox1.Select(
                                richTextBox1.Text.Length - currentDialog.GetValue(id).Length - 1,
                                currentDialog.GetValue(id).Length
                                );
                        richTextBox1.SelectionColor = playerText;
                        richTextBox1.DeselectAll();
                    }
                    else if (currentDialog.GetValue(id)[0] != '>')
                    {
                        richTextBox1.AppendText("You: " + currentDialog.GetValue(id) + "\n");
                        string str = "You: " + currentDialog.GetValue(id);
                        richTextBox1.Select(
                                richTextBox1.Text.Length - str.Length - 1,
                                str.Length
                                );
                        richTextBox1.SelectionColor = playerText;
                        richTextBox1.DeselectAll();
                    }
                    string nextArchive = Path.GetFileNameWithoutExtension(currentArchiveName) + id.ToString() + ".pak";
                    LoadDialog(ZipFile.OpenRead(nextArchive),nextArchive, resourceArchive);
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
            config.SetValue(2, storyTTSToolStripMenuItem.Checked.ToString());
            config.SetValue(3, selectedOptionTTSToolStripMenuItem.Checked.ToString());
            config.ToFile(MyPath + "\\Config\\config.xml");
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                Reply();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox(desc, Text);
            aboutBox.ShowDialog();
        }
    }
}
