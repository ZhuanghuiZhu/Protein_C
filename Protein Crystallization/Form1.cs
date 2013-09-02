using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Meteroi;

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
            string username = UserName.Text;
            string password = PassWords.Text;
            string ipaddress = IPAddress0.Text + '.' + IPAddress1.Text + '.' + IPAddress2.Text + '.' + IPAddress3.Text;
            if (PCAS.connect(ipaddress, username, password))
            {
                float t1 = PCAS.get_box_temperature();
                float t2 = PCAS.get_chip_temperature();
                float m1 = PCAS.get_box_moisture();
                float m2 = PCAS.get_chip_moisture();
                welcome.Visible = false;// Set the welcome page as invisible
                timer1.Enabled = true;
                temperature1.Text = t1.ToString();
                temperature0.Text = t2.ToString();
                moisture0.Text = m1.ToString();
                moisture1.Text = m2.ToString();
            }
            else
            {
                MessageBox.Show("          连接失败");
            }
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
            PCAS.set_ref(0);
        }

        private void AlignmentMark1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            PCAS.set_ref(1);
        }

        private void AlignmentMark2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            PCAS.set_ref(2);
        }

        private void AlignmentMark3_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
            PCAS.set_ref(3);
        }
        private void original_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
            PCAS.set_ref(4);
        }
        private void RemoteClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("确认关机", "远程关机", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
        }

        private void xp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(1);
        }

        private void xpp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(100);
        }

        private void ymm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(-100);
        }

        private void ym_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(-1);
        }

        private void xm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(-1);
        }

        private void xmm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(-100);
        }

        private void yp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(1);
        }

        private void ypp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(100);
        }

        private void zmm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(-100);
        }

        private void zm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(-10);
        }

        private void zp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(10);
        }

        private void zpp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(100);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float t1 = PCAS.get_box_temperature();
            float t2 = PCAS.get_chip_temperature();
            float m1 = PCAS.get_box_moisture();
            float m2 = PCAS.get_chip_moisture();
            if (t1 != float.NaN)
                temperature1.Text = t1.ToString();
            if (t2 != float.NaN)
                temperature0.Text = t2.ToString();
            if (m1 != float.NaN)
                moisture1.Text = m1.ToString();
            if (m2 != float.NaN)
                moisture0.Text = m2.ToString();
            logtext.Text = PCAS.get_log();
        }

        private void LED_light_ValueChanged(object sender, EventArgs e)
        {
            uint i = decimal.ToUInt32(LED_light.Value);
            PCAS.set_led(i);
        }

        private void targettemp_TextChanged(object sender, EventArgs e)
        {
            float target_temp = float.Parse(targettemp.Text);
            PCAS.set_target_temperature(target_temp);
        }

        private void targetmoist_TextChanged(object sender, EventArgs e)
        {
            float target_moist = float.Parse(targetmoist.Text);
            PCAS.set_target_moisture(target_moist);
        }

        private void radius_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
