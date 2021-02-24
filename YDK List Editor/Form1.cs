using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YDK;


namespace YDK_Data_Editor
{
    public partial class Form1 : Form
    {
        string currentFile = null;
        Data currentData;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "YDKL File|*.ydkl|All files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentData = Data.FromFile(openFileDialog1.FileName);
                currentFile = openFileDialog1.FileName;
                listBox1.SelectedIndex = 0;
                this.Text = "YDK Data Editor - "+openFileDialog1.FileName;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(currentData != null)
                richTextBox1.Text = currentData.GetValue(listBox1.SelectedIndex);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (currentData != null && !string.IsNullOrWhiteSpace(richTextBox1.Text))
                currentData.SetValue(listBox1.SelectedIndex, richTextBox1.Text);
            else if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                currentData.SetValue(listBox1.SelectedIndex, null);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(currentFile) == ".xml")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    currentData.ToFile(saveFileDialog1.FileName);
            }
            else if (currentFile != null)
            {
                currentData.ToFile(currentFile);
            } 
            else
            {
                MessageBox.Show("Error: no DATA file open.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentData = new Data();
                currentFile = saveFileDialog1.FileName;
                this.Text = "YDK Data Editor - " + saveFileDialog1.FileName;
            }
            
        }

        private void openXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML File |*.xml| All files |*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentData = Data.FromXMLFile(openFileDialog1.FileName);
                currentFile = openFileDialog1.FileName;
                listBox1.SelectedIndex = 0;
            }
        }
    }
}
