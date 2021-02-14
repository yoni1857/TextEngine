
namespace StoryMaker
{
    partial class ProjectEditDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.dispDate = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddPNG = new System.Windows.Forms.Button();
            this.btnRemovePNG = new System.Windows.Forms.Button();
            this.ImagePreviewBox = new System.Windows.Forms.PictureBox();
            this.PNGlist = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.chooseImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePreviewBox)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.DimGray;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(188, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Project Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.DimGray;
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTitle.ForeColor = System.Drawing.Color.White;
            this.txtTitle.Location = new System.Drawing.Point(230, 7);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(395, 38);
            this.txtTitle.TabIndex = 1;
            // 
            // dispDate
            // 
            this.dispDate.BackColor = System.Drawing.Color.DimGray;
            this.dispDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dispDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dispDate.ForeColor = System.Drawing.Color.White;
            this.dispDate.Location = new System.Drawing.Point(864, 7);
            this.dispDate.Name = "dispDate";
            this.dispDate.ReadOnly = true;
            this.dispDate.Size = new System.Drawing.Size(210, 34);
            this.dispDate.TabIndex = 3;
            this.dispDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.DimGray;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(631, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(212, 32);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Creation Date:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Black", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1080, 94);
            this.button1.TabIndex = 4;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 436);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(1086, 100);
            this.panel1.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1062, 367);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gray;
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.ForeColor = System.Drawing.Color.White;
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1054, 338);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PNGs";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gray;
            this.tabPage2.ForeColor = System.Drawing.Color.White;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1054, 338);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "WAVs";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ImagePreviewBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.PNGlist, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1048, 332);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnAddPNG
            // 
            this.btnAddPNG.BackColor = System.Drawing.Color.DimGray;
            this.btnAddPNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddPNG.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnAddPNG.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnAddPNG.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btnAddPNG.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddPNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPNG.ForeColor = System.Drawing.Color.LawnGreen;
            this.btnAddPNG.Location = new System.Drawing.Point(3, 3);
            this.btnAddPNG.Name = "btnAddPNG";
            this.btnAddPNG.Size = new System.Drawing.Size(253, 71);
            this.btnAddPNG.TabIndex = 0;
            this.btnAddPNG.Text = "+";
            this.btnAddPNG.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnAddPNG.UseVisualStyleBackColor = false;
            this.btnAddPNG.Click += new System.EventHandler(this.btnAddPNG_Click);
            // 
            // btnRemovePNG
            // 
            this.btnRemovePNG.BackColor = System.Drawing.Color.DimGray;
            this.btnRemovePNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemovePNG.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnRemovePNG.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnRemovePNG.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btnRemovePNG.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemovePNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemovePNG.ForeColor = System.Drawing.Color.Red;
            this.btnRemovePNG.Location = new System.Drawing.Point(262, 3);
            this.btnRemovePNG.Name = "btnRemovePNG";
            this.btnRemovePNG.Size = new System.Drawing.Size(253, 71);
            this.btnRemovePNG.TabIndex = 1;
            this.btnRemovePNG.Text = "-";
            this.btnRemovePNG.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnRemovePNG.UseVisualStyleBackColor = false;
            this.btnRemovePNG.Click += new System.EventHandler(this.btnRemovePNG_Click);
            // 
            // ImagePreviewBox
            // 
            this.ImagePreviewBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ImagePreviewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImagePreviewBox.Location = new System.Drawing.Point(527, 3);
            this.ImagePreviewBox.Name = "ImagePreviewBox";
            this.ImagePreviewBox.Size = new System.Drawing.Size(518, 243);
            this.ImagePreviewBox.TabIndex = 2;
            this.ImagePreviewBox.TabStop = false;
            // 
            // PNGlist
            // 
            this.PNGlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PNGlist.FormattingEnabled = true;
            this.PNGlist.ItemHeight = 16;
            this.PNGlist.Location = new System.Drawing.Point(3, 3);
            this.PNGlist.Name = "PNGlist";
            this.PNGlist.Size = new System.Drawing.Size(518, 243);
            this.PNGlist.TabIndex = 3;
            this.PNGlist.SelectedIndexChanged += new System.EventHandler(this.PNGlist_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnRemovePNG, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAddPNG, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 252);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(518, 77);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // Clock
            // 
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // chooseImageDialog
            // 
            this.chooseImageDialog.DefaultExt = "png";
            this.chooseImageDialog.FileName = "*.png";
            this.chooseImageDialog.Filter = "PNG Images|*.png";
            this.chooseImageDialog.Multiselect = true;
            this.chooseImageDialog.Title = "Choose Image";
            // 
            // ProjectEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1086, 536);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dispDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProjectEditDialog";
            this.Text = "Project";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ProjectDialog_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePreviewBox)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox dispDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox ImagePreviewBox;
        private System.Windows.Forms.ListBox PNGlist;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnRemovePNG;
        private System.Windows.Forms.Button btnAddPNG;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer Clock;
        private System.Windows.Forms.OpenFileDialog chooseImageDialog;
    }
}