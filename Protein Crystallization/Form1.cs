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
using Basic;

namespace Protein_Crystallization
{
    public partial class Detector : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        BasicForm picture;

        public Detector()
        {
            InitializeComponent();
            picture = new BasicForm();
            picture.parent_window = this;
            picture.Visible = false;
            dataGridView1.Rows.Add(23);
            for (int i = 0; i < 24; i++) 
                this.dataGridView1[0, i].Value = Convert.ToString(i + 1);
            //for (int i = 0; i < 12; i++)
            //{
            //    this.dataGridView1[0, i].Value = "A" + Convert.ToString(i + 1);
            //    this.dataGridView1[0, i + 12].Value = "B" + Convert.ToString(i + 1);
            //}
            for (int i = 0; i < 24; i++)
            {
                this.dataGridView1.Rows[i].Cells[5].Value = "注射";
                //this.dataGridView1.Rows[i].Cells[5].
            }

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
                picture.Close();
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
        private void set_sampleid(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(set_sampleid);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox3.Text = text;
            }
        }
        private void set_exam(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.button3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(set_exam);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.button3.Text = text;
                this.savedir.Visible = true;
            }
        }
        private void sample_exam_thread()
        {
            uint i = 1;
            PCAS.set_radius(d);
            PCAS.set_angle(a);
            PCAS.set_sample(sample);
            while (i < sample)
            {
                PCAS.move_to_sample(i);
                this.set_sampleid(i.ToString());
                Thread.Sleep(100);
                picture.Record_the_picture(i);
                Thread.Sleep(100);
                if (exam_start == false)
                {
                    return;
                }
                Thread.Sleep(time);
                i++;
            }
            exam_start = false;
            set_exam("检测");
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
                button3.Text = "停止";
                savedir.Visible = false;
                exam.Start();
            }
            else
            {
                exam_start = false;
                savedir.Visible = true;
                button3.Text = "检测";
            }
        }

        Thread xp_t =null;
        Thread xpp_t =null;
        Thread yp_t =null;
        Thread ypp_t =null;
        Thread zp_t = null;
        Thread zpp_t =null;
        Thread xm_t = null;
        Thread xmm_t = null;
        Thread ym_t = null;
        Thread ymm_t = null;
        Thread zm_t = null;
        Thread zmm_t = null;

        bool xp_stop=false;
        bool xpp_stop = false;
        bool yp_stop = false;
        bool ypp_stop = false;
        bool zp_stop = false;
        bool zpp_stop = false;
        bool xm_stop = false;
        bool xmm_stop = false;
        bool ym_stop = false;
        bool ymm_stop = false;
        bool zm_stop = false;
        bool zmm_stop = false;

        private void xp_thread()
        {
            int i=0;
             while(true) {
                PCAS.micoscope_x(1);
                i++;
                Thread.Sleep(100);
                if(xp_stop == true)
                     return;
             }
        }

        private void xpp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(100);
                i++;
                Thread.Sleep(100);
                if (xpp_stop == true)
                    return;
            }
        }

        private void yp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(1);
                i++;
                Thread.Sleep(100);
                if (yp_stop == true)
                    return;
            }
        }

        private void ypp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(100);
                i++;
                Thread.Sleep(100);
                if (ypp_stop == true)
                    return;
            }
        }

        private void zp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(1);
                i++;
                Thread.Sleep(100);
                if (zp_stop == true)
                    return;
            }
        }

        private void zpp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(100);
                i++;
                Thread.Sleep(100);
                if (zpp_stop == true)
                    return;
            }
        }

        private void xm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(-1);
                i++;
                Thread.Sleep(100);
                if (xm_stop == true)
                    return;
            }
        }

        private void xmm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(-100);
                i++;
                Thread.Sleep(100);
                if (xmm_stop == true)
                    return;
            }
        }

        private void ym_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(-1);
                i++;
                Thread.Sleep(100);
                if (ym_stop == true)
                    return;
            }
        }

        private void ymm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(-100);
                i++;
                Thread.Sleep(100);
                if (ymm_stop == true)
                    return;
            }
        }

        private void zm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(-1);
                i++;
                Thread.Sleep(100);
                if (zm_stop == true)
                    return;
            }
        }

        private void zmm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(-100);
                i++;
                Thread.Sleep(100);
                if (zmm_stop == true)
                    return;
            }
        }

        private void xp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //xp_t = new Thread(xp_thread);
                //xp_stop = false;
                //xp_t.Start();
            }
        }

        private void xpp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //xpp_t = new Thread(xpp_thread);
                //xpp_stop = false;
                //xpp_t.Start();
            }
        }

        private void ym_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //ym_t = new Thread(ym_thread);
                //ym_stop = false;
                //ym_t.Start();
            }
        }

        private void ymm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //ymm_t = new Thread(ymm_thread);
                //ymm_stop = false;
                //ymm_t.Start();
            }
        }

        private void xmm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //xmm_t = new Thread(xmm_thread);
                //xmm_stop = false;
                //xmm_t.Start();
            }
        }

        private void xm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //xm_t = new Thread(xm_thread);
                //xm_stop = false;
                //xm_t.Start();
            }
        }

        private void yp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //yp_t = new Thread(yp_thread);
                //yp_stop = false;
                //yp_t.Start();
            }
        }

        private void ypp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //ypp_t = new Thread(ypp_thread);
                //ypp_stop = false;
                //ypp_t.Start();
            }
        }

        private void zm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //zm_t = new Thread(zm_thread);
                //zm_stop = false;
                //zm_t.Start();
            }
        }

        private void zmm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //zmm_t = new Thread(zmm_thread);
                //zmm_stop = false;
                //zmm_t.Start();
            }
        }

        private void zp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //zp_t = new Thread(zp_thread);
                //zp_stop = false;
                //zp_t.Start();
            }
        }

        private void zpp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //zpp_t = new Thread(zpp_thread);
                //zpp_stop = false;
                //zpp_t.Start();
            }
        }

        private void xp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xp_stop = true;
            }
        }

        private void xmm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xmm_stop = true;
            }
        }

        private void ymm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ymm_stop = true;
            }
        }

        private void ypp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ypp_stop = true;
            }
        }

        private void yp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                yp_stop = true;
            }
        }

        private void ym_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ym_stop = true;
            }
        }

        private void xpp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xpp_stop = true;
            }
        }

        private void xm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xm_stop = true;
            }
        }

        private void zpp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zpp_stop = true;
            }
        }

        private void zmm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zmm_stop = true;
            }
        }

        private void zm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zm_stop = true;
            }
        }

        public void set_show_picture()
        {
            button4.Text = "显示图像";
        }

        public void set_hide_picture()
        {
            button4.Text = "隐藏图像";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (picture.IsDisposed)
            {
                picture = new BasicForm();
                picture.parent_window = this;
            }
            picture.Left = this.Left + this.Size.Width;
            picture.Top  = this.Top;

            if (picture.Visible == false)
            {
                picture.Visible = true;
                set_hide_picture();
            }
            else
            {
                picture.Visible = false;
                set_show_picture();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            uint uL;
            uint sampleid;
            if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value != null)
            {
                uL = uint.Parse(this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString());
                sampleid = (uint)e.RowIndex + 1;
                PCAS.set_hole_uL(uL);
                PCAS.move_to_hole(sampleid);
                //MessageBox.Show("加样" + uL.ToString()+"uL"+" to sample"+sampleid);
            }
        }

        private void Detector_FormClosing(object sender, FormClosingEventArgs e)
        {
            picture.Close();
        }

        private void savedir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                picture.save_path = folderBrowserDialog1.SelectedPath;
            }
        }
        private bool auto_test = false;
        private void time_test_Click(object sender, EventArgs e)
        {
            if (auto_test == false)
            {
                uint time = uint.Parse(textBox1.Text) * 1000 * 60;
                uint sample = uint.Parse(Sample.Text);
                uint timeinterval = uint.Parse(textBox7.Text) * 1000;
                if (time < sample * timeinterval)
                {
                    MessageBox.Show("时间间隔太短");
                }
                autotesttime.Interval = (int)time;
                autotesttime.Start();
                auto_test = true;
                time_test.Text = "停止";
                if (exam_start == false)
                    button3.PerformClick();
            } else {
                auto_test = false;
                autotesttime.Stop();
                time_test.Text = "定时检查";
                if (exam_start == true)
                    button3.PerformClick();
            }
        }

        private void autotesttime_Tick(object sender, EventArgs e)
        {
            if (exam_start == false)
                button3.PerformClick();
        }

    }
}
