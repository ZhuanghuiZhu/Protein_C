namespace Basic
{
    partial class BasicForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BtnPlay = new System.Windows.Forms.Button();
            this.BtnSettings = new System.Windows.Forms.Button();
            this.BtnSnapshot = new System.Windows.Forms.Button();
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.StateLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnPlay
            // 
            this.BtnPlay.Location = new System.Drawing.Point(18, 18);
            this.BtnPlay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(112, 34);
            this.BtnPlay.TabIndex = 0;
            this.BtnPlay.Text = "播放";
            this.BtnPlay.UseVisualStyleBackColor = true;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // BtnSettings
            // 
            this.BtnSettings.Location = new System.Drawing.Point(156, 18);
            this.BtnSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSettings.Name = "BtnSettings";
            this.BtnSettings.Size = new System.Drawing.Size(112, 34);
            this.BtnSettings.TabIndex = 0;
            this.BtnSettings.Text = "设置";
            this.BtnSettings.UseVisualStyleBackColor = true;
            this.BtnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // BtnSnapshot
            // 
            this.BtnSnapshot.Location = new System.Drawing.Point(292, 18);
            this.BtnSnapshot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSnapshot.Name = "BtnSnapshot";
            this.BtnSnapshot.Size = new System.Drawing.Size(112, 34);
            this.BtnSnapshot.TabIndex = 0;
            this.BtnSnapshot.Text = "抓拍";
            this.BtnSnapshot.UseVisualStyleBackColor = true;
            this.BtnSnapshot.Click += new System.EventHandler(this.BtnSnapshot_Click);
            // 
            // PreviewBox
            // 
            this.PreviewBox.Location = new System.Drawing.Point(18, 76);
            this.PreviewBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.Size = new System.Drawing.Size(1010, 693);
            this.PreviewBox.TabIndex = 1;
            this.PreviewBox.TabStop = false;
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(21, 784);
            this.StateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(62, 18);
            this.StateLabel.TabIndex = 2;
            this.StateLabel.Text = "状态栏";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 50;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // BasicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 819);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.PreviewBox);
            this.Controls.Add(this.BtnSnapshot);
            this.Controls.Add(this.BtnSettings);
            this.Controls.Add(this.BtnPlay);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BasicForm";
            this.Text = "Camera C# demo[Basic]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BasicForm_FormClosing);
            this.Load += new System.EventHandler(this.BasicForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnSettings;
        private System.Windows.Forms.Button BtnSnapshot;
        private System.Windows.Forms.PictureBox PreviewBox;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}

