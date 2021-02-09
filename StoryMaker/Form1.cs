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

namespace StoryMaker
{
    public partial class Form1 : Form
    {
        Dictionary<TreeNode, Data> nodes;
        Project currentProject;
        string currentPath;
        public Form1()
        {
            InitializeComponent();
            nodes = new Dictionary<TreeNode, Data>();
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
                listBox1.SelectedIndex = 0;
                richTextBox1.Text = nodes[newNode].GetValue(listBox1.SelectedIndex);
            } else
            {
                MessageBox.Show("Select a dialog first!");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nodes.Add(treeView1.Nodes[0], new Data());
            treeView1.SelectedNode = treeView1.Nodes[0];
            listBox1.SelectedIndex = 0;
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
            if(treeView1.SelectedNode != null)
                toolStripStatusLabel1.Text = treeView1.SelectedNode.Text + ":" + listBox1.SelectedIndex.ToString();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && !String.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                nodes[treeView1.SelectedNode].SetValue(listBox1.SelectedIndex, richTextBox1.Text);
            } else if (String.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                nodes[treeView1.SelectedNode].SetValue(listBox1.SelectedIndex, null);
            }
            else if(treeView1.SelectedNode == null)
            {
                MessageBox.Show("Select a dialog first!");
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            richTextBox1.Text = nodes[treeView1.SelectedNode].GetValue(listBox1.SelectedIndex);
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && listBox1.SelectedItem != null)
            {
                richTextBox1.Text = nodes[treeView1.SelectedNode].GetValue(listBox1.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Select a dialog first!");
            }
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
            richTextBox1.Text = nodes[treeView1.SelectedNode].GetValue(listBox1.SelectedIndex);
            listBox1.SelectedIndex = 0;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(currentProject != null)
            {
                ProjectDialog projectDialog = new ProjectDialog(currentProject);
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
            if (listBox1.SelectedItem != null)
            {
                richTextBox1.Text = nodes[treeView1.SelectedNode].GetValue(listBox1.SelectedIndex);
            }
        }

        private void toolStripStatusLabel1_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                richTextBox1.Text = nodes[treeView1.SelectedNode].GetValue(listBox1.SelectedIndex);
            }
        }
    }
}
