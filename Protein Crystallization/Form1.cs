﻿using System;
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
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Protein_Crystallization
{
    public partial class Detector : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        BasicForm picture;
        private List<int> x = new List<int>();
        private List<int> y = new List<int>();
        public Detector()
        {
            InitializeComponent();
            picture = new BasicForm();
            picture.parent_window = this;
            picture.Visible = false;
            dataGridView1.Rows.Add(23);
            for (int i = 0; i < 24; i++) 
                this.dataGridView1[0, i].Value = Convert.ToString(i + 1);
            for (int i = 0; i < 24; i++)
                this.dataGridView1[1, i].Value = "";
            for (int i = 0; i < 24; i++)
                this.dataGridView1[2, i].Value = "";
            for (int i = 0; i < 24; i++)
                this.dataGridView1[3, i].Value = "";
            for (int i = 0; i < 24; i++)
                this.dataGridView1[4, i].Value = "";
            for (int i = 0; i < 24; i++)
                this.dataGridView1[6, i].Value = "";
            for (int i = 0; i < 24; i++)
            {
                this.dataGridView1.Rows[i].Cells[5].Value = "注射";
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
        public class setting
        {
            [XmlElementAttribute("Target_temperature")]
            public float temperature;
            [XmlElementAttribute("Target_moisture")]
            public float moisture;

            [XmlElementAttribute("Samples")]
            public uint samples;
            [XmlElementAttribute("Sample_id")]
            public uint sampleid;
            [XmlElementAttribute("Sample_radius")]
            public float s_radius;
            [XmlElementAttribute("Sample_angle_offset")]
            public float s_angle;

            [XmlElementAttribute("Hole_id")]
            public uint holeid;
            [XmlElementAttribute("Hole_radius")]
            public float h_radius;
            [XmlElementAttribute("Hole_angle_offset")]
            public float h_angle;
            [XmlElementAttribute("Hole_distance_offset")]
            public float h_off;

            [XmlElementAttribute("LED_light")]
            public uint LED;
            [XmlElementAttribute("Liquid_uL")]
            public uint uL;

            [XmlElementAttribute("Testing_time")]
            public int test_time;
            [XmlElementAttribute("Testing_time_unit")]
            public int test_time_unit;

            [XmlElementAttribute("Interval_time")]
            public int time;
            [XmlElementAttribute("Interval_time_unit")]
            public int time_unit;

            [XmlElementAttribute("grid_size")]
            public int grid_size;

            [XmlElementAttribute("grid_name")]
            public List<string> grid_name;

            [XmlElementAttribute("grid_resualt")]
            public List<string> grid_resualt;

            [XmlElementAttribute("grid_ diameter")]
            public List<string> grid_diameter;

            [XmlElementAttribute("grid_uL")]
            public List<string> grid_uL;

            [XmlElementAttribute("grid_other")]
            public List<string> grid_other;

            public setting()
            {
                grid_name     = new List<string>();
                grid_resualt  = new List<string>();
                grid_diameter = new List<string>();
                grid_uL       = new List<string>();
                grid_other    = new List<string>();
            }
        }
        private void LoadSetting_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//Load setting file
            {
                int i;
                setting save = new setting();
                XmlSerializer serializer = new XmlSerializer(save.GetType());
                TextReader read = new StreamReader(openFileDialog1.FileName);
                save = (setting)serializer.Deserialize(read);
                read.Close();
                targettemp.Text    = save.temperature.ToString();
                targetmoist.Text   = save.moisture.ToString();

                sample_radius.Text = save.s_radius.ToString();
                Sample.Text        = save.samples.ToString();
                sampleid.Text      = save.sampleid.ToString();
                angle.Text         = save.s_angle.ToString();

                radius.Text        = save.h_radius.ToString();
                holeid.Text        = save.holeid.ToString();
                hole_off.Text      = save.h_off.ToString();
                angle.Text         = save.h_angle.ToString();

                LED_light.Value = new decimal(save.LED);
                uL.Text         = save.uL.ToString();

                textBox7.Text   = save.test_time.ToString();
                comboBox1.SelectedIndex = save.test_time_unit;
                textBox1.Text = save.time.ToString();
                comboBox2.SelectedIndex = save.time_unit; 

                i = 0;
                foreach(string s in save.grid_name)
                {
                    dataGridView1[1, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_resualt)
                {
                    dataGridView1[2, i].Value = s;
                    i++;
                }
                i = 0;
                foreach(string s in save.grid_diameter)
                {
                    dataGridView1[3, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_uL)
                {
                    dataGridView1[4, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_other)
                {
                    dataGridView1[6, i].Value = s;
                    i++;
                }
            }
        }
        private void SaveSetting_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)//Save setting file
            {
                setting save = new setting();
                save.temperature = float.Parse(targettemp.Text);
                save.moisture    = float.Parse(targetmoist.Text);

                save.s_radius    = float.Parse(sample_radius.Text);
                save.samples     = uint.Parse(Sample.Text);
                save.sampleid    = uint.Parse(sampleid.Text);
                save.s_angle     = float.Parse(angle.Text);

                save.h_radius = float.Parse(radius.Text);
                save.holeid   = uint.Parse(holeid.Text);
                save.h_off    = float.Parse(hole_off.Text);
                save.h_angle  = float.Parse(angle.Text);

                save.LED         = decimal.ToUInt32(LED_light.Value);
                save.uL          = uint.Parse(uL.Text);

                save.test_time   = int.Parse(textBox7.Text);
                save.test_time_unit = comboBox1.SelectedIndex;
                save.time        = int.Parse(textBox1.Text);
                save.time_unit   = comboBox2.SelectedIndex;

                save.grid_size = dataGridView1.RowCount;
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_name.Add(dataGridView1[1, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_resualt.Add(dataGridView1[2, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_diameter.Add(dataGridView1[3, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_uL.Add(dataGridView1[4, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_other.Add(dataGridView1[6, i].Value.ToString());
                }
                XmlSerializer serializer = new XmlSerializer(save.GetType());
                TextWriter writer = new StreamWriter(saveFileDialog1.FileName);
                serializer.Serialize(writer, save);
                writer.Close();
            }
        }

        private void LoadCoodinate_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)//Load the coordinate file
            {
                StreamReader file = new StreamReader(openFileDialog2.FileName);
                while (!file.EndOfStream)
                {
                    string strReadLine = file.ReadLine(); //读取每行数据
                    string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
                    MatchCollection mc = Regex.Matches(strReadLine, regexStr);
                    if (mc.Count < 2)
                        return;
                    x.Add(int.Parse(mc[0].Value));
                    y.Add(int.Parse(mc[1].Value));
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            int i = int.Parse(textBox2.Text);
            if (i == 0) {
                PCAS.microscopexy(0, 0);
                return;
            }
            i = i - 1;
            if (i > x.Count)
                return;
            PCAS.microscopexy(x[i], y[i]);
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
            logtext.Select(logtext.TextLength, 0);//光标定位到文本最后
            logtext.ScrollToCaret();//滚动到光标处
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
            uint i = uint.Parse(sampleid.Text);
            uint sample = uint.Parse(Sample.Text);
            float d = float.Parse(sample_radius.Text);
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
            uint i = uint.Parse(holeid.Text);
            uint sample = uint.Parse(Sample.Text);
            float d = float.Parse(radius.Text);
            float a = float.Parse(holeangle.Text);
            float h_d = float.Parse(hole_off.Text);
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
            if (this.sampleid.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(set_sampleid);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.sampleid.Text = text;
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
        StreamReader reportfile = null;
        bool cycle = true;
        private void sample_exam_thread()
        {
            
            string strReadLine;
            string regexStr = @"[-+]?\b(?:[0-9]*\.)*[0-9]+\b";
            MatchCollection mc;
            uint i = 1;
            PCAS.set_radius(d);
            PCAS.set_angle(a);
            PCAS.set_sample(sample);
            while (i < sample)
            {
                if (cycle == true)
                    PCAS.move_to_sample(i);
                else
                {
                    if (i > x.Count - 1)
                        break;
                    PCAS.microscopexy(x[(int)i], y[(int)i]);
                }
                this.set_sampleid(i.ToString());
                Thread.Sleep(100);
                picture.Record_the_picture(i);
                Thread.Sleep(100);
                if (reportfile != null)
                {
                    strReadLine = reportfile.ReadLine(); //读取每行数据
                    //while (!reportfile.EndOfStream)
                    //    strReadLine = reportfile.ReadLine(); //读取每行数据
                    if (strReadLine != null)
                    {
                        mc = Regex.Matches(strReadLine, regexStr);
                        if (mc.Count >= 9)
                        {
                            dataGridView1[3, (int)i - 1].Value = mc[7].Value;
                        }
                    }
                }
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
            cycle = comboBox3.SelectedIndex == 0;
            sample = uint.Parse(Sample.Text);
            d = float.Parse(sample_radius.Text);
            a = float.Parse(angle.Text);
            if (comboBox1.SelectedIndex == 0)
                time = (int)uint.Parse(textBox7.Text) * 1000;
            else
                time = (int)uint.Parse(textBox7.Text) * 1000 * 60;
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
                picture.Visible = false;
            }
            picture.Left = this.Left + this.Size.Width;
            picture.Top  = this.Top;
            ;
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
            if (e.RowIndex < 0 || e.ColumnIndex < 1)
                return;
            if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value != null)
            {
                uL = uint.Parse(this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString());
                sampleid = (uint)e.RowIndex + 1;
                PCAS.set_hole_uL(uL);
                PCAS.move_to_hole(sampleid);                
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
                uint time;
                if(comboBox2.SelectedIndex ==0)
                    time = uint.Parse(textBox1.Text) * 1000 * 60;
                else
                    time = uint.Parse(textBox1.Text) * 1000 * 60 * 60;
                uint sample = uint.Parse(Sample.Text);
                uint timeinterval = uint.Parse(textBox7.Text) * 1000;
                if (time < sample * timeinterval)
                {
                    MessageBox.Show("时间间隔太短");
                }
                autotesttime.Interval = (int)time;
                autotesttime.Start();
                auto_test = true;
                time_test.Text = "终止";
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

        bool cycle_start = false;
        bool cycle_pause = false;
        int r = 0;
        private void cycle_thread()
        {
            int angle = 0;
            PCAS.micoscope_x(-r);
            while(angle < 360) {
                if (cycle_start == false)
                    return;
                if (cycle_pause == true)
                {
                    Thread.Sleep(1);
                    continue;
                }
                PCAS.micoscope_x((int)(r - r * Math.Cos(Math.PI / 180 * angle)));
                PCAS.micoscope_y((int)(r * Math.Sin(Math.PI/180 * angle)));
                angle++;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (cycle_start == false)
            {
                r = int.Parse(textBox9.Text);
                Thread cycle = new Thread(cycle_thread);
                cycle.Start();
                button14.Text = "停止";
                button15.Visible = true;
                cycle_start = true;
            }
            else
            {
                button14.Text = "启动";
                cycle_start = false;
                cycle_pause = false;
                button15.Visible = false;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (cycle_pause == false)
            {
                button15.Text = "恢复";
                cycle_pause = true;
            }
            else
            {
                button15.Text = "暂停";
                cycle_pause = false;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            PCAS.set_view_z();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            PCAS.set_hole_z();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//Load setting file
            {
                reportfile = new StreamReader(openFileDialog1.FileName);
            }
        }
    }
}
