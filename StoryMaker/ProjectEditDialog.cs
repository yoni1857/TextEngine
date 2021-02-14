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
        private Dictionary<String, Image> Images;
        private List<String> WAVs;
        public ProjectEditDialog(Project project)
        {
            InitializeComponent();
            txtTitle.Text = project.Name;
            DateTime date = project.CreationDate;
            dispDate.Text = $"{date.Day}/{date.Month}/{date.Year}  {date.TimeOfDay}";
            Project = project;
            Images = new Dictionary<string, Image>();
            WAVs = new List<string>();
            foreach(ZipArchiveEntry entry in project.resArchive.Entries)
            {
                switch (Path.GetExtension(entry.Name.ToLower()))
                {
                    case ".png":
                        Images.Add(entry.Name, Image.FromStream(entry.Open()));
                        break;
                    case ".wav":
                        WAVs.Add(entry.Name);
                        break;
                }
            }
        }

        private void ProjectDialog_Load(object sender, EventArgs e)
        {
            PNGlist.Items.AddRange(Images.Keys.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Project.Name = txtTitle.Text;
            this.Close();
        }

        private void PNGlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ImagePreviewBox.Image = Images[PNGlist.SelectedItem.ToString()];
            } catch
            {
                
            }
        }

        private void btnRemovePNG_Click(object sender, EventArgs e)
        {
            try
            {
                Project.resArchive.GetEntry(PNGlist.SelectedItem.ToString()).Delete();
                Images.Remove(PNGlist.SelectedItem.ToString());
                PNGlist.Items.Remove(PNGlist.SelectedItem);
            }
            catch { }
        }

        private void Clock_Tick(object sender, EventArgs e)
        {
            btnRemovePNG.Enabled = (PNGlist.SelectedIndex < 0);
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
                        ZipArchiveEntry entry = Project.resArchive.CreateEntry(internalname);
                        using (StreamWriter writer = new StreamWriter(entry.Open()))
                        {
                            using (StreamReader reader = new StreamReader(File.OpenRead(filename)))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                        Images.Add(internalname, Image.FromFile(filename));
                        PNGlist.Items.Add(internalname);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nAt: {ex.TargetSite} | Help Link: {ex.HelpLink}");
            }
        }
    }
}
