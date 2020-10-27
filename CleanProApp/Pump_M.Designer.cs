namespace CleanProApp
{
    partial class Pump_M
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
            this.label_CycleNumV = new System.Windows.Forms.Label();
            this.label_WaitTimeV = new System.Windows.Forms.Label();
            this.label_HoldTimeV = new System.Windows.Forms.Label();
            this.label_IntensityV = new System.Windows.Forms.Label();
            this.trkBar_CycleNum = new System.Windows.Forms.TrackBar();
            this.trkBar_WaitTime = new System.Windows.Forms.TrackBar();
            this.trkBar_HoldTime = new System.Windows.Forms.TrackBar();
            this.trkBar_Intensity = new System.Windows.Forms.TrackBar();
            this.label_CycleNum = new System.Windows.Forms.Label();
            this.label_WaitTime = new System.Windows.Forms.Label();
            this.label_HoldTime = new System.Windows.Forms.Label();
            this.label_Intensity = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_CycleNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_WaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_HoldTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_Intensity)).BeginInit();
            this.SuspendLayout();
            // 
            // label_CycleNumV
            // 
            this.label_CycleNumV.AutoSize = true;
            this.label_CycleNumV.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CycleNumV.Location = new System.Drawing.Point(187, 158);
            this.label_CycleNumV.Margin = new System.Windows.Forms.Padding(0);
            this.label_CycleNumV.Name = "label_CycleNumV";
            this.label_CycleNumV.Size = new System.Drawing.Size(65, 20);
            this.label_CycleNumV.TabIndex = 42;
            this.label_CycleNumV.Text = "循环次数";
            this.label_CycleNumV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_WaitTimeV
            // 
            this.label_WaitTimeV.AutoSize = true;
            this.label_WaitTimeV.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_WaitTimeV.Location = new System.Drawing.Point(187, 113);
            this.label_WaitTimeV.Margin = new System.Windows.Forms.Padding(0);
            this.label_WaitTimeV.Name = "label_WaitTimeV";
            this.label_WaitTimeV.Size = new System.Drawing.Size(65, 20);
            this.label_WaitTimeV.TabIndex = 41;
            this.label_WaitTimeV.Text = "停止时长";
            this.label_WaitTimeV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_HoldTimeV
            // 
            this.label_HoldTimeV.AutoSize = true;
            this.label_HoldTimeV.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HoldTimeV.Location = new System.Drawing.Point(187, 68);
            this.label_HoldTimeV.Margin = new System.Windows.Forms.Padding(0);
            this.label_HoldTimeV.Name = "label_HoldTimeV";
            this.label_HoldTimeV.Size = new System.Drawing.Size(65, 20);
            this.label_HoldTimeV.TabIndex = 40;
            this.label_HoldTimeV.Text = "运转时长";
            this.label_HoldTimeV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_IntensityV
            // 
            this.label_IntensityV.AutoSize = true;
            this.label_IntensityV.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_IntensityV.Location = new System.Drawing.Point(187, 23);
            this.label_IntensityV.Margin = new System.Windows.Forms.Padding(0);
            this.label_IntensityV.Name = "label_IntensityV";
            this.label_IntensityV.Size = new System.Drawing.Size(65, 20);
            this.label_IntensityV.TabIndex = 39;
            this.label_IntensityV.Text = "强度大小";
            this.label_IntensityV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trkBar_CycleNum
            // 
            this.trkBar_CycleNum.AutoSize = false;
            this.trkBar_CycleNum.Location = new System.Drawing.Point(49, 154);
            this.trkBar_CycleNum.Margin = new System.Windows.Forms.Padding(0);
            this.trkBar_CycleNum.Name = "trkBar_CycleNum";
            this.trkBar_CycleNum.Size = new System.Drawing.Size(134, 25);
            this.trkBar_CycleNum.TabIndex = 38;
            this.trkBar_CycleNum.ValueChanged += new System.EventHandler(this.TrackBarValued);
            // 
            // trkBar_WaitTime
            // 
            this.trkBar_WaitTime.AutoSize = false;
            this.trkBar_WaitTime.Location = new System.Drawing.Point(49, 111);
            this.trkBar_WaitTime.Margin = new System.Windows.Forms.Padding(0);
            this.trkBar_WaitTime.Name = "trkBar_WaitTime";
            this.trkBar_WaitTime.Size = new System.Drawing.Size(134, 25);
            this.trkBar_WaitTime.TabIndex = 37;
            this.trkBar_WaitTime.ValueChanged += new System.EventHandler(this.TrackBarValued);
            // 
            // trkBar_HoldTime
            // 
            this.trkBar_HoldTime.AutoSize = false;
            this.trkBar_HoldTime.Location = new System.Drawing.Point(49, 68);
            this.trkBar_HoldTime.Margin = new System.Windows.Forms.Padding(0);
            this.trkBar_HoldTime.Name = "trkBar_HoldTime";
            this.trkBar_HoldTime.Size = new System.Drawing.Size(134, 25);
            this.trkBar_HoldTime.TabIndex = 36;
            this.trkBar_HoldTime.ValueChanged += new System.EventHandler(this.TrackBarValued);
            // 
            // trkBar_Intensity
            // 
            this.trkBar_Intensity.AutoSize = false;
            this.trkBar_Intensity.Location = new System.Drawing.Point(49, 25);
            this.trkBar_Intensity.Margin = new System.Windows.Forms.Padding(0);
            this.trkBar_Intensity.Name = "trkBar_Intensity";
            this.trkBar_Intensity.Size = new System.Drawing.Size(134, 25);
            this.trkBar_Intensity.TabIndex = 35;
            this.trkBar_Intensity.ValueChanged += new System.EventHandler(this.TrackBarValued);
            // 
            // label_CycleNum
            // 
            this.label_CycleNum.AutoSize = true;
            this.label_CycleNum.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CycleNum.Location = new System.Drawing.Point(18, 135);
            this.label_CycleNum.Margin = new System.Windows.Forms.Padding(0);
            this.label_CycleNum.Name = "label_CycleNum";
            this.label_CycleNum.Size = new System.Drawing.Size(59, 17);
            this.label_CycleNum.TabIndex = 34;
            this.label_CycleNum.Text = "循环次数:";
            this.label_CycleNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_WaitTime
            // 
            this.label_WaitTime.AutoSize = true;
            this.label_WaitTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_WaitTime.Location = new System.Drawing.Point(18, 93);
            this.label_WaitTime.Margin = new System.Windows.Forms.Padding(0);
            this.label_WaitTime.Name = "label_WaitTime";
            this.label_WaitTime.Size = new System.Drawing.Size(59, 17);
            this.label_WaitTime.TabIndex = 33;
            this.label_WaitTime.Text = "停止时间:";
            this.label_WaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_HoldTime
            // 
            this.label_HoldTime.AutoSize = true;
            this.label_HoldTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HoldTime.Location = new System.Drawing.Point(18, 51);
            this.label_HoldTime.Margin = new System.Windows.Forms.Padding(0);
            this.label_HoldTime.Name = "label_HoldTime";
            this.label_HoldTime.Size = new System.Drawing.Size(59, 17);
            this.label_HoldTime.TabIndex = 32;
            this.label_HoldTime.Text = "运转时间:";
            this.label_HoldTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Intensity
            // 
            this.label_Intensity.AutoSize = true;
            this.label_Intensity.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Intensity.Location = new System.Drawing.Point(18, 9);
            this.label_Intensity.Margin = new System.Windows.Forms.Padding(0);
            this.label_Intensity.Name = "label_Intensity";
            this.label_Intensity.Size = new System.Drawing.Size(59, 17);
            this.label_Intensity.TabIndex = 31;
            this.label_Intensity.Text = "运转强度:";
            this.label_Intensity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Location = new System.Drawing.Point(160, 199);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(100, 30);
            this.btn_Cancel.TabIndex = 49;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.QuitAdjustFrm);
            // 
            // btn_OK
            // 
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Location = new System.Drawing.Point(30, 199);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(100, 30);
            this.btn_OK.TabIndex = 48;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.QuitAdjustFrm);
            // 
            // Pump_M
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 238);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.label_CycleNumV);
            this.Controls.Add(this.label_WaitTimeV);
            this.Controls.Add(this.label_HoldTimeV);
            this.Controls.Add(this.label_IntensityV);
            this.Controls.Add(this.trkBar_CycleNum);
            this.Controls.Add(this.trkBar_WaitTime);
            this.Controls.Add(this.trkBar_HoldTime);
            this.Controls.Add(this.trkBar_Intensity);
            this.Controls.Add(this.label_CycleNum);
            this.Controls.Add(this.label_WaitTime);
            this.Controls.Add(this.label_HoldTime);
            this.Controls.Add(this.label_Intensity);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Pump_M";
            this.Text = "墨泵编辑";
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_CycleNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_WaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_HoldTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_Intensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_CycleNumV;
        private System.Windows.Forms.Label label_WaitTimeV;
        private System.Windows.Forms.Label label_HoldTimeV;
        private System.Windows.Forms.Label label_IntensityV;
        private System.Windows.Forms.TrackBar trkBar_CycleNum;
        private System.Windows.Forms.TrackBar trkBar_WaitTime;
        private System.Windows.Forms.TrackBar trkBar_HoldTime;
        private System.Windows.Forms.TrackBar trkBar_Intensity;
        private System.Windows.Forms.Label label_CycleNum;
        private System.Windows.Forms.Label label_WaitTime;
        private System.Windows.Forms.Label label_HoldTime;
        private System.Windows.Forms.Label label_Intensity;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;

    }
}