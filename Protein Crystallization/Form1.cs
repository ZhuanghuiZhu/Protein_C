using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Protein_Crystallization
{
    public partial class Detector : Form
    {
        public Detector()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            welcome.Visible = false;// Set the welcome page as invisible
        }

        private void LoadSetting_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//Load setting file
            { 

            }
        }

        private void SaveSetting_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)//Save setting file
            {

            }
        }

        private void LoadCoodinate_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)//Load the coordinate file
            {

            }
        }

        private void AlignmentMark0_Click(object sender, EventArgs e)
        {
            checkBox0.Checked = true;
        }

        private void AlignmentMark1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void AlignmentMark2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
        }

        private void AlignmentMark3_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
        }

        private void RemoteClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("确认关机", "远程关机", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
        }



    }
}
