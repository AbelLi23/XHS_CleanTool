namespace CleanProApp
{
    partial class T_Delay
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
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label_HoldTimeV = new System.Windows.Forms.Label();
            this.trkBar_HoldTime = new System.Windows.Forms.TrackBar();
            this.label_HoldTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_HoldTime)).BeginInit();
            this.SuspendLayout();
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
            // label_HoldTimeV
            // 
            this.label_HoldTimeV.AutoSize = true;
            this.label_HoldTimeV.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HoldTimeV.Location = new System.Drawing.Point(187, 75);
            this.label_HoldTimeV.Margin = new System.Windows.Forms.Padding(0);
            this.label_HoldTimeV.Name = "label_HoldTimeV";
            this.label_HoldTimeV.Size = new System.Drawing.Size(65, 20);
            this.label_HoldTimeV.TabIndex = 63;
            this.label_HoldTimeV.Text = "运转时长";
            this.label_HoldTimeV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trkBar_HoldTime
            // 
            this.trkBar_HoldTime.AutoSize = false;
            this.trkBar_HoldTime.Location = new System.Drawing.Point(49, 75);
            this.trkBar_HoldTime.Margin = new System.Windows.Forms.Padding(0);
            this.trkBar_HoldTime.Name = "trkBar_HoldTime";
            this.trkBar_HoldTime.Size = new System.Drawing.Size(134, 25);
            this.trkBar_HoldTime.TabIndex = 62;
            this.trkBar_HoldTime.ValueChanged += new System.EventHandler(this.TrackBarValued);
            // 
            // label_HoldTime
            // 
            this.label_HoldTime.AutoSize = true;
            this.label_HoldTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HoldTime.Location = new System.Drawing.Point(18, 55);
            this.label_HoldTime.Margin = new System.Windows.Forms.Padding(0);
            this.label_HoldTime.Name = "label_HoldTime";
            this.label_HoldTime.Size = new System.Drawing.Size(59, 17);
            this.label_HoldTime.TabIndex = 60;
            this.label_HoldTime.Text = "运转时间:";
            this.label_HoldTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // T_Delay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 238);
            this.Controls.Add(this.label_HoldTimeV);
            this.Controls.Add(this.trkBar_HoldTime);
            this.Controls.Add(this.label_HoldTime);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "T_Delay";
            this.Text = "延时编辑";
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_HoldTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label_HoldTimeV;
        private System.Windows.Forms.TrackBar trkBar_HoldTime;
        private System.Windows.Forms.Label label_HoldTime;
    }
}