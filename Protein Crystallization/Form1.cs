using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Meteroi;
using System.Threading;

namespace Protein_Crystallization
{
    public partial class Detector : Form
    {
        public Detector()
        {
            InitializeComponent();

            dataGridView1.Rows.Add(23);
            for (int i = 0; i < 24; i++) 
                this.dataGridView1[0, i].Value = Convert.ToString(i + 1);
            //for (int i = 0; i < 12; i++)
            //{
            //    this.dataGridView1[0, i].Value = "A" + Convert.ToString(i + 1);
            //    this.dataGridView1[0, i + 12].Value = "B" + Convert.ToString(i + 1);
            //}
            for (int i = 0; i < 24; i++) 
                this.dataGridView1.Rows[i].Cells[5].Value = "注射";
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
                MessageBox.Show("连接失败");
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
            checkBox0.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false; 
            PCAS.set_ref(4);
        }
        private void RemoteClose_Click(object sender, EventArgs e)
        {
            string username = UserName.Text;
            string password = PassWords.Text;
            string ipaddress = IPAddress0.Text + '.' + IPAddress1.Text + '.' + IPAddress2.Text + '.' + IPAddress3.Text;
            DialogResult r;
            r = MessageBox.Show("确认关机", "远程关机", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Cancel)
                return;
            if (PCAS.disconnect(ipaddress, username, password) == true)
            {
                welcome.Visible = true;// Set the welcome page as invisible
                timer1.Enabled = false;
            } else
            {
                MessageBox.Show("关机失败");
            }
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
            PCAS.micoscope_y(100);
        }
        private void ym_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(1);
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
            PCAS.micoscope_y(-1);
        }
        private void ypp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(-100);
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
                temperature1.Text = t1.ToString("0.00");
            if (t2 != float.NaN)
                temperature0.Text = t2.ToString("0.00");
            if (m1 != float.NaN)
                moisture1.Text = m1.ToString("0.00");
            if (m2 != float.NaN && m2 > 0 && m2 < 100)
                moisture0.Text = m2.ToString("0.00");
            logtext.Text = PCAS.get_log();
        }
        private void LED_light_ValueChanged(object sender, EventArgs e)
        {
            uint i = decimal.ToUInt32(LED_light.Value);
            if (i > 100)
            {
                MessageBox.Show("请输入正确的亮度比列");
                return;
            }
            PCAS.set_led(i);
        }
        private void targettemp_TextChanged(object sender, EventArgs e)
        {

        }
        private void targetmoist_TextChanged(object sender, EventArgs e)
        {

        }
        private void radius_TextChanged(object sender, EventArgs e)
        {

        }
        private void syf_Click(object sender, EventArgs e)
        {
            PCAS.syringe_plus(25);
           
        }
        private void syb_Click(object sender, EventArgs e)
        {
            PCAS.syringe_minus(25);
        }    
        private void syff_Click(object sender, EventArgs e)
        {
            PCAS.syringe_plus(200);
        }
        private void sybb_Click(object sender, EventArgs e)
        {
            PCAS.syringe_minus(200);
        }
        private void set_Click(object sender, EventArgs e)
        {
            float target_temp = float.Parse(targettemp.Text);
            float target_moist = float.Parse(targetmoist.Text);
            if (target_temp > 30 || target_temp < 0)
            {
                MessageBox.Show("请输入正确的温度");
                return;
            }
            if (target_moist > 100 || target_moist < 0)
            {
                MessageBox.Show("请输入正确的湿度");
                return;
            }
            PCAS.set_target_temperature(target_temp);
            PCAS.set_target_moisture(target_moist);
        }
        
        private void exam_Click(object sender, EventArgs e)
        {
            uint i = uint.Parse(textBox3.Text);
            uint sample = uint.Parse(Sample.Text);
            float d = float.Parse(textBox4.Text);
            float a = float.Parse(angle.Text);
            if (i > sample)
            {
                MessageBox.Show("请输入正确的样本编号");
                return;
            }
            if (a > 360 || a < - 360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            PCAS.set_radius(d);
            PCAS.set_angle(a);
            PCAS.set_sample(sample);
            PCAS.move_to_sample(i);
        }

        private void addsample_Click(object sender, EventArgs e)
        {
            uint i = uint.Parse(textBox5.Text);
            uint sample = uint.Parse(Sample.Text);
            float d = float.Parse(radius.Text);
            float a = float.Parse(holeangle.Text);
            float h_d = float.Parse(hole_d.Text);
            float u = float.Parse(uL.Text);
            if (i > sample)
            {
                MessageBox.Show("请输入正确的样本编号");
                return;
            }
            if (a > 360 || a < -360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            if (h_d < 0 || h_d > 50)
            {
                MessageBox.Show("请输入正确的偏移");
                return;
            }
            PCAS.set_hole_radius(d);
            PCAS.set_hole_angle(a);
            PCAS.set_hole_sample(sample);
            PCAS.set_hole_delta(h_d);
            PCAS.set_hole_uL(u);
            PCAS.move_to_hole(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PCAS.pannel_in();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PCAS.pannel_out();
        }

        private uint sample = 0;
        private float d = 0;
        private float a = 0;
        private int time = 0;
        private bool exam_start = false;
        private void sample_exam_thread()
        {
            uint i = 1;
            PCAS.set_radius(d);
            PCAS.set_angle(a);
            PCAS.set_sample(sample);
            while (i < sample)
            {
                sampleid.Text = i.ToString();
                Thread.Sleep(time);
                PCAS.move_to_sample(i);
                if(exam_start == false)
                {
                    return;
                }
                i++;
            }                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread exam = new Thread(sample_exam_thread);
            sample = uint.Parse(Sample.Text);
            d = float.Parse(textBox4.Text);
            a = float.Parse(angle.Text);
            time = (int)uint.Parse(textBox7.Text) * 1000;
            if (a > 360 || a < -360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            if (exam_start == false)
            {
                exam_start = true;
                button3.Text = "中断";
                exam.Start();
            }
            else
            {
                exam_start = false;
                button3.Text = "检测";
            }
        }

        private void xp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xp.PerformClick();
            }
        }

        private void xpp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xpp.PerformClick();
            }
        }

        private void ym_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ym.PerformClick();
            }
        }

        private void ymm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ymm.PerformClick();
            }
        }

        private void xmm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xmm.PerformClick();
            }
        }

        private void xm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xm.PerformClick();
            }
        }

        private void yp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                yp.PerformClick();
            }
        }

        private void ypp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ypp.PerformClick();
            }
        }

        private void zm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zm.PerformClick();
            }
        }

        private void zmm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zmm.PerformClick();
            }
        }

        private void zp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zp.PerformClick();
            }
        }

    }
}
