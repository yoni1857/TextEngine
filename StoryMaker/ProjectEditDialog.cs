using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoryMaker
{
    public partial class ProjectEditDialog : Form
    {
        public Project Project;
        public ProjectEditDialog(Project project)
        {
            InitializeComponent();
            txtTitle.Text = project.Name;
            DateTime date = project.CreationDate;
            dispDate.Text = $"{date.Day}/{date.Month}/{date.Year}  {date.TimeOfDay}";
            Project = project;
        }

        private void ProjectDialog_Load(object sender, EventArgs e)
        {
            PNGlist.Items.AddRange(Project.Images.Keys.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Project.Name = txtTitle.Text;
            this.Close();
        }

        private void PNGlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImagePreviewBox.BackgroundImage = PNGlist.SelectedIndex > -1 ? Project.Images[PNGlist.SelectedItem.ToString()] : null;
        }

        private void btnRemovePNG_Click(object sender, EventArgs e)
        {
            try
            {
                Project.Images.Remove(PNGlist.SelectedItem.ToString());
                PNGlist.Items.Remove(PNGlist.SelectedItem);
            }
            catch { }
        }

        private void Clock_Tick(object sender, EventArgs e)
        {
            btnRemovePNG.Enabled = (PNGlist.SelectedIndex < 0);
            btnRemoveWAV.Enabled = (WAVlist.SelectedIndex < 0);
        }

        private void btnAddPNG_Click(object sender, EventArgs e)
        {
            try
            {
                if (chooseImageDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filename in chooseImageDialog.FileNames)
                    {
                        string internalname = "sprites\\" + Path.GetFileName(filename);
                        Project.Images.Add(internalname, Image.FromFile(filename));
                        PNGlist.Items.Add(internalname);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nAt: {ex.TargetSite} | Help Link: {ex.HelpLink}");
            }
        }

        private void btnRemoveWAV_Click(object sender, EventArgs e)
        {
            try {
                Project.WAVs.Remove(WAVlist.SelectedItem.ToString());
                WAVlist.Items.Remove(WAVlist.SelectedItem);
            }
            catch
            {
                return;
            }
        }

        private void btnAddWAV_Click(object sender, EventArgs e)
        {
            try
            {
                if(chooseWAVDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach(string filename in chooseWAVDialog.FileNames)
                    {
                        string internalname = "wav\\" + Path.GetFileName(filename);
                        Project.WAVs.Add(internalname, File.ReadAllBytes(filename));

                        WAVlist.Items.Add(internalname);
                    }
                }
            } catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nAt: {ex.TargetSite} | Help Link: {ex.HelpLink}");
            }
        }
    }
}
