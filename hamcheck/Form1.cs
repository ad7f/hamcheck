using HamCheckLib;
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

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Initializing...";
            HamCheckLib.HamCheckLib.Init();

            toolStripStatusLabel1.Text = "Querying...";
            HamCheckLib.HamCheckLib.GetResults(textBox1.Text, textBox3.Text);

            toolStripStatusLabel1.Text = "Updating...";
            dataGridView1.AutoGenerateColumns = true;
            if (HamCheckLib.HamCheckLib.ds != null && HamCheckLib.HamCheckLib.ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = HamCheckLib.HamCheckLib.ds.Tables[0];
                dataGridView1.Update();
            }
            //HamCheckLib.HamCheckLib.Close();

            toolStripStatusLabel1.Text = "";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
