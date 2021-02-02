
namespace hamcheck
{
    partial class Form1
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
            this.textBoxNames = new System.Windows.Forms.TextBox();
            this.textBoxCities = new System.Windows.Forms.TextBox();
            this.labelCities = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.labelResults = new System.Windows.Forms.Label();
            this.dataSet1 = new System.Data.DataSet();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.labelNames = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxNames
            // 
            this.textBoxNames.Location = new System.Drawing.Point(16, 55);
            this.textBoxNames.MaxLength = 0;
            this.textBoxNames.Multiline = true;
            this.textBoxNames.Name = "textBoxNames";
            this.textBoxNames.Size = new System.Drawing.Size(303, 164);
            this.textBoxNames.TabIndex = 0;
            // 
            // textBoxCities
            // 
            this.textBoxCities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCities.Location = new System.Drawing.Point(351, 55);
            this.textBoxCities.Multiline = true;
            this.textBoxCities.Name = "textBoxCities";
            this.textBoxCities.Size = new System.Drawing.Size(289, 164);
            this.textBoxCities.TabIndex = 4;
            // 
            // labelCities
            // 
            this.labelCities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCities.AutoSize = true;
            this.labelCities.Location = new System.Drawing.Point(348, 40);
            this.labelCities.Name = "labelCities";
            this.labelCities.Size = new System.Drawing.Size(35, 13);
            this.labelCities.TabIndex = 5;
            this.labelCities.Text = "Cities:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(579, 231);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(60, 28);
            this.buttonSearch.TabIndex = 6;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // labelResults
            // 
            this.labelResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResults.AutoSize = true;
            this.labelResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResults.Location = new System.Drawing.Point(14, 259);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(45, 13);
            this.labelResults.TabIndex = 10;
            this.labelResults.Text = "Results:";
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(15, 275);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            this.dataGridViewResults.RowHeadersWidth = 62;
            this.dataGridViewResults.Size = new System.Drawing.Size(623, 285);
            this.dataGridViewResults.TabIndex = 12;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 562);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(651, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(651, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(52, 22);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // labelNames
            // 
            this.labelNames.AutoSize = true;
            this.labelNames.Location = new System.Drawing.Point(14, 39);
            this.labelNames.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNames.Name = "labelNames";
            this.labelNames.Size = new System.Drawing.Size(43, 13);
            this.labelNames.TabIndex = 15;
            this.labelNames.Text = "Names:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 584);
            this.Controls.Add(this.labelNames);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.labelResults);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.labelCities);
            this.Controls.Add(this.textBoxCities);
            this.Controls.Add(this.textBoxNames);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "HamCheck";
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNames;
        private System.Windows.Forms.TextBox textBoxCities;
        private System.Windows.Forms.Label labelCities;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelResults;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.Label labelNames;
    }
}

