using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YDK;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System.Media;
using System.IO.Compression;
using static System.Windows.Forms.ListView;

namespace StoryMaker
{
    public partial class Form1 : Form
    {
        Dictionary<TreeNode, Data> nodes;
        Project currentProject;
        string currentPath;
        SoundPlayer soundPlayer;
        ListViewItemCollection prevItems;
        public Form1()
        {
            InitializeComponent();
            nodes = new Dictionary<TreeNode, Data>();
            prevItems = replyList.Items;
            timer1.Start();
        }

        private void LoadProject(Project project)
        {
            this.Text = "StoryMaker - " + project.Name;
            nodes = project.Nodes;
            treeView1.Nodes.Clear();
            foreach(TreeNode node in nodes.Keys.Reverse())
            {
                if (node.Parent != null)
                {
                    TreeNode parent = node.Parent;
                    node.Parent.Nodes.Remove(node);
                    parent.Nodes.Add(node);
                }
                else
                    treeView1.Nodes.Add(node);
            }

            wavPicker.Items.Clear();
            wavPicker.Items.Add("");
            wavPicker.Items.AddRange(project.WAVs.Keys.ToArray());
            dialogImagePicker.Items.Clear();
            dialogImagePicker.Items.Add("");
            dialogImagePicker.Items.AddRange(project.Images.Keys.ToArray());

            treeView1.EndUpdate();
            currentProject = project;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null)
            {
                TreeNode newNode = treeView1.SelectedNode.Nodes.Add((treeView1.SelectedNode.Nodes.Count + 1).ToString());
                nodes.Add(newNode, new Data());
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode = newNode;
                treeView1_AfterSelect(sender, null);
            } else
            {
                MessageBox.Show("Select a dialog first!");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nodes.Add(treeView1.Nodes[0], new Data());
            treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null && treeView1.SelectedNode.Level != 0)
            {
                nodes.Remove(treeView1.SelectedNode);
                treeView1.SelectedNode.Remove();
            }
            else if(treeView1.SelectedNode == null)
            {
                MessageBox.Show("Select a dialog first!");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
                toolStripStatusLabel1.Text = treeView1.SelectedNode.Parent != null ? treeView1.SelectedNode.Parent.Text + " --> " + treeView1.SelectedNode.Text : treeView1.SelectedNode.Text;
            if(prevItems != replyList.Items && treeView1.SelectedNode != null)
            {
                Data selectedData = nodes[treeView1.SelectedNode];
                for (int i = 1; i < 10; i++)
                {
                    if (replyList.Items.Count > i - 1)
                    {
                        selectedData.SetValue(i, replyList.Items[i - 1].Text);
                    }
                    else
                    {
                        selectedData.SetValue(i, null);
                    }
                }
            }

            prevItems = replyList.Items;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            promptBox_TextChanged(sender, e);
        }

        private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = Color.White;
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = Color.Black;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null && currentPath != null)
            {
                currentProject.toFile(currentPath);
            } else if (currentPath == null && currentProject != null)
            {
                saveFileDialog1.FileName = currentProject.Name + ".smprj";
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentProject.toFile(saveFileDialog1.FileName);
                    currentPath = saveFileDialog1.FileName;
                }
            } else if (currentProject == null)
            {
                Project newProject = new Project("New Project", nodes);
                ProjectDialog dialog = new ProjectDialog(newProject);
                if (dialog.ShowDialog() == DialogResult.OK)
                { 
                    LoadProject(dialog.Project);
                    saveFileDialog1.FileName = currentProject.Name + ".smprj";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        currentProject.toFile(saveFileDialog1.FileName);
                        currentPath = saveFileDialog1.FileName;
                    }
                }
                
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project newProject = new Project("New Project");
            ProjectDialog dialog = new ProjectDialog(newProject);
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                LoadProject(dialog.Project);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
            {
                if(MessageBox.Show("Make sure you've saved your progress!", "Watch out!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK
                    && openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    LoadProject(Project.FromFile(openFileDialog1.FileName));
                    currentPath = openFileDialog1.FileName;
                }
            } else
            {
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    LoadProject(Project.FromFile(openFileDialog1.FileName));
                    currentPath = openFileDialog1.FileName;
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(currentProject != null)
            {
                if(exportFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentProject.ExportToStory(exportFileDialog1.SelectedPath);
                    foreach (FileInfo File in new DirectoryInfo(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName).EnumerateFiles())
                    {
                        if (File.Extension == ".pak")
                            File.Delete();
                    }
                    if(MessageBox.Show("Project Successfully Exported! Would you like to view your files?", "Huzzah!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start("explorer.exe", exportFileDialog1.SelectedPath);
                    }
                }
            } else
            {
                MessageBox.Show("You must create a project before exporting it!");
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            treeView1_AfterSelect(sender, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(currentProject != null)
            {
                ProjectEditDialog projectDialog = new ProjectEditDialog(currentProject);
                if(projectDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProject(projectDialog.Project);
                }
            } else
            {
                MessageBox.Show("You need a project before you edit its settings!");
            }
        }

        private void toolStripMenuItem1_DropDownClosed(object sender, EventArgs e)
        {
            toolStripMenuItem1.ForeColor = Color.White;
        }

        private void toolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItem1.ForeColor = Color.Black;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null && currentPath != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentProject.toFile(saveFileDialog1.FileName);
                    currentPath = saveFileDialog1.FileName;
                }
            }
            else if (currentPath == null && currentProject != null)
            {
                saveFileDialog1.FileName = currentProject.Name + ".smprj";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentProject.toFile(saveFileDialog1.FileName);
                    currentPath = saveFileDialog1.FileName;
                }
            }
            else if (currentProject == null)
            {
                Project newProject = new Project("New Project", nodes);
                ProjectDialog dialog = new ProjectDialog(newProject);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProject(dialog.Project);
                    saveFileDialog1.FileName = currentProject.Name + ".smprj";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        currentProject.toFile(saveFileDialog1.FileName);
                        currentPath = saveFileDialog1.FileName;
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Data selectedData = nodes[treeView1.SelectedNode];
            promptBox.Text = selectedData.GetValue(0);
            dialogImagePicker.SelectedItem = string.IsNullOrEmpty(selectedData.GetValue(255)) ? "" : selectedData.GetValue(255);
            wavPicker.SelectedItem = string.IsNullOrEmpty(selectedData.GetValue(254)) ? "" : selectedData.GetValue(254);
            replyList.Items.Clear();
            for (int i = 1; i < 10; i++)
            {
                if(selectedData.GetValue(i)!= null)
                    replyList.Items.Add(selectedData.GetValue(i));
            }
            replyList.EndUpdate();
            wavPicker_SelectedValueChanged(sender, e);
            dialogImagePicker_SelectedValueChanged(sender, e);
        }

        private void toolStripStatusLabel1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            promptBox_TextChanged(sender, e);
            if (replyList.Items.Count != 10)
                replyList.Items.Add("New Option");
            else
                MessageBox.Show("Reached max limit of options!");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            promptBox_TextChanged(sender, e);
            if (replyList.SelectedItems.Count > 0)
            {
                replyList.Items.Remove(replyList.SelectedItems[0]);
            } else
            {
                MessageBox.Show("Please select an item first!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (soundPlayer != null)
            {
                soundPlayer.Play();
            }
            else
            {
                if (wavPicker.SelectedItem != null)
                {
                    Stream stream = new MemoryStream(currentProject.WAVs[wavPicker.SelectedItem.ToString()]);

                    soundPlayer = new SoundPlayer(stream);

                    button3_Click(sender, e);
                }
                else MessageBox.Show("No sound to play!");
            }
            
        }

        private void wavPicker_SelectedValueChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                Data selectedData = nodes[treeView1.SelectedNode];
                selectedData.SetValue(254, (string)wavPicker.SelectedItem);
            }
            if (soundPlayer != null)
                soundPlayer.Stop();
            try
            {
                if (currentProject != null && wavPicker.SelectedItem != null)
                {
                    Stream stream = new MemoryStream(currentProject.WAVs[wavPicker.SelectedText]);
                    soundPlayer = new SoundPlayer(stream);
                }
            }
            catch { return;  }
        }

        private void dialogImagePicker_SelectedValueChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                Data selectedData = nodes[treeView1.SelectedNode];
                selectedData.SetValue(255, (string)dialogImagePicker.SelectedItem);
            }
            try
            {
                if(currentProject!=null && dialogImagePicker.SelectedItem != null)
                    dialogImageBox.BackgroundImage = string.IsNullOrWhiteSpace(dialogImagePicker.SelectedItem.ToString()) ? null : currentProject.Images[dialogImagePicker.SelectedItem.ToString()];
            }
            catch { return; }
        }

        private void promptBox_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                Data selectedData = nodes[treeView1.SelectedNode];
                selectedData.SetValue(0, promptBox.Text);
            }
            else MessageBox.Show("Please select a node first!");
        }

        private void wavPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void removeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            toolStripMenuItem4_Click(sender, e);
        }

        private void AddLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            addToolStripMenuItem_Click(sender, e);
        }
    }
}
