using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoryMaker
{
    public partial class ProjectDialog : Form
    {
        public Project Project;
        public ProjectDialog(Project project)
        {
            InitializeComponent();
            txtTitle.Text = project.Name;
            DateTime date = project.CreationDate;
            dispDate.Text = $"{date.Day}/{date.Month}/{date.Year}  {date.TimeOfDay}";
            Project = project;
        }

        private void ProjectDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Project.Name = txtTitle.Text;
            this.Close();
        }
    }
}
