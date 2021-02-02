using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace hamcheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Initializing...";
            Checker.Init();

            toolStripStatusLabel1.Text = "Querying...";
            Checker.GetResults(textBoxNames.Text, textBoxCities.Text);

            toolStripStatusLabel1.Text = "Updating...";
            dataGridViewResults.AutoGenerateColumns = true;
            if (Checker.ds != null && Checker.ds.Tables.Count > 0)
            {
                dataGridViewResults.DataSource = Checker.ds.Tables[0];
                dataGridViewResults.Update();
            }
            toolStripStatusLabel1.Text = "";
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

    }
}
