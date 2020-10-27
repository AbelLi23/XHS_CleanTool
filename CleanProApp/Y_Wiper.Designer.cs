namespace CleanProApp
{
    partial class Y_Wiper
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
            this.comBox_Position = new System.Windows.Forms.ComboBox();
            this.label_Position = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.chkBox_Zero = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comBox_Position
            // 
            this.comBox_Position.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBox_Position.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comBox_Position.FormattingEnabled = true;
            this.comBox_Position.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.comBox_Position.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.comBox_Position.Location = new System.Drawing.Point(125, 75);
            this.comBox_Position.Margin = new System.Windows.Forms.Padding(0);
            this.comBox_Position.Name = "comBox_Position";
            this.comBox_Position.Size = new System.Drawing.Size(134, 25);
            this.comBox_Position.TabIndex = 16;
            // 
            // label_Position
            // 
            this.label_Position.AutoSize = true;
            this.label_Position.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Position.Location = new System.Drawing.Point(28, 78);
            this.label_Position.Margin = new System.Windows.Forms.Padding(0);
            this.label_Position.Name = "label_Position";
            this.label_Position.Size = new System.Drawing.Size(59, 17);
            this.label_Position.TabIndex = 15;
            this.label_Position.Text = "刮片位置:";
            this.label_Position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Location = new System.Drawing.Point(160, 199);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(100, 30);
            this.btn_Cancel.TabIndex = 47;
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
            this.btn_OK.TabIndex = 46;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.QuitAdjustFrm);
            // 
            // chkBox_Zero
            // 
            this.chkBox_Zero.AutoSize = true;
            this.chkBox_Zero.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkBox_Zero.Location = new System.Drawing.Point(208, 104);
            this.chkBox_Zero.Margin = new System.Windows.Forms.Padding(0);
            this.chkBox_Zero.Name = "chkBox_Zero";
            this.chkBox_Zero.Size = new System.Drawing.Size(51, 21);
            this.chkBox_Zero.TabIndex = 49;
            this.chkBox_Zero.Text = "原点";
            this.chkBox_Zero.UseVisualStyleBackColor = true;
            this.chkBox_Zero.CheckedChanged += new System.EventHandler(this.chkBox_Zero_CheckedChanged);
            // 
            // Y_Wiper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 238);
            this.Controls.Add(this.chkBox_Zero);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.comBox_Position);
            this.Controls.Add(this.label_Position);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Y_Wiper";
            this.Text = "刮片编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comBox_Position;
        private System.Windows.Forms.Label label_Position;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.CheckBox chkBox_Zero;
    }
}