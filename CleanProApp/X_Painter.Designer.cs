namespace CleanProApp
{
    partial class X_Painter
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
            this.label_VelocityV = new System.Windows.Forms.Label();
            this.trkBar_Velocity = new System.Windows.Forms.TrackBar();
            this.comBox_Position = new System.Windows.Forms.ComboBox();
            this.label_Position = new System.Windows.Forms.Label();
            this.label_Velocity = new System.Windows.Forms.Label();
            this.btn_AjVel = new System.Windows.Forms.Button();
            this.btn_AjPos = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.pnl_AjVel = new System.Windows.Forms.Panel();
            this.pnl_AjPos = new System.Windows.Forms.Panel();
            this.chkBox_Zero = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_Velocity)).BeginInit();
            this.pnl_AjVel.SuspendLayout();
            this.pnl_AjPos.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_VelocityV
            // 
            this.label_VelocityV.AutoSize = true;
            this.label_VelocityV.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_VelocityV.Location = new System.Drawing.Point(87, 3);
            this.label_VelocityV.Margin = new System.Windows.Forms.Padding(0);
            this.label_VelocityV.Name = "label_VelocityV";
            this.label_VelocityV.Size = new System.Drawing.Size(65, 20);
            this.label_VelocityV.TabIndex = 36;
            this.label_VelocityV.Text = "速度大小";
            this.label_VelocityV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trkBar_Velocity
            // 
            this.trkBar_Velocity.AutoSize = false;
            this.trkBar_Velocity.Location = new System.Drawing.Point(18, 30);
            this.trkBar_Velocity.Margin = new System.Windows.Forms.Padding(0);
            this.trkBar_Velocity.Name = "trkBar_Velocity";
            this.trkBar_Velocity.Size = new System.Drawing.Size(134, 25);
            this.trkBar_Velocity.TabIndex = 35;
            this.trkBar_Velocity.ValueChanged += new System.EventHandler(this.trkBar_Velocity_ValueChanged);
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
            this.comBox_Position.Location = new System.Drawing.Point(18, 30);
            this.comBox_Position.Margin = new System.Windows.Forms.Padding(0);
            this.comBox_Position.Name = "comBox_Position";
            this.comBox_Position.Size = new System.Drawing.Size(134, 25);
            this.comBox_Position.TabIndex = 34;
            this.comBox_Position.SelectedIndexChanged += new System.EventHandler(this.comBox_Position_SelectedIndexChanged);
            // 
            // label_Position
            // 
            this.label_Position.AutoSize = true;
            this.label_Position.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Position.Location = new System.Drawing.Point(18, 5);
            this.label_Position.Margin = new System.Windows.Forms.Padding(0);
            this.label_Position.Name = "label_Position";
            this.label_Position.Size = new System.Drawing.Size(59, 17);
            this.label_Position.TabIndex = 33;
            this.label_Position.Text = "小车位置:";
            this.label_Position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Velocity
            // 
            this.label_Velocity.AutoSize = true;
            this.label_Velocity.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Velocity.Location = new System.Drawing.Point(18, 5);
            this.label_Velocity.Margin = new System.Windows.Forms.Padding(0);
            this.label_Velocity.Name = "label_Velocity";
            this.label_Velocity.Size = new System.Drawing.Size(59, 17);
            this.label_Velocity.TabIndex = 32;
            this.label_Velocity.Text = "小车速度:";
            this.label_Velocity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_AjVel
            // 
            this.btn_AjVel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_AjVel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_AjVel.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AjVel.Image = global::CleanProApp.Properties.Resources.PainterVel;
            this.btn_AjVel.Location = new System.Drawing.Point(203, 75);
            this.btn_AjVel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AjVel.Name = "btn_AjVel";
            this.btn_AjVel.Size = new System.Drawing.Size(72, 66);
            this.btn_AjVel.TabIndex = 42;
            this.btn_AjVel.Text = "调整速度";
            this.btn_AjVel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_AjVel.UseVisualStyleBackColor = true;
            this.btn_AjVel.Visible = false;
            this.btn_AjVel.Click += new System.EventHandler(this.OptBeforeAjst);
            // 
            // btn_AjPos
            // 
            this.btn_AjPos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_AjPos.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_AjPos.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AjPos.Image = global::CleanProApp.Properties.Resources.PainterPos;
            this.btn_AjPos.Location = new System.Drawing.Point(203, 9);
            this.btn_AjPos.Margin = new System.Windows.Forms.Padding(0);
            this.btn_AjPos.Name = "btn_AjPos";
            this.btn_AjPos.Size = new System.Drawing.Size(72, 66);
            this.btn_AjPos.TabIndex = 43;
            this.btn_AjPos.Text = "调整位置";
            this.btn_AjPos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_AjPos.UseVisualStyleBackColor = true;
            this.btn_AjPos.Visible = false;
            this.btn_AjPos.Click += new System.EventHandler(this.OptBeforeAjst);
            // 
            // btn_OK
            // 
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Location = new System.Drawing.Point(30, 199);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(100, 30);
            this.btn_OK.TabIndex = 44;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.QuitAdjustFrm);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Location = new System.Drawing.Point(160, 199);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(100, 30);
            this.btn_Cancel.TabIndex = 45;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.QuitAdjustFrm);
            // 
            // pnl_AjVel
            // 
            this.pnl_AjVel.Controls.Add(this.trkBar_Velocity);
            this.pnl_AjVel.Controls.Add(this.label_Velocity);
            this.pnl_AjVel.Controls.Add(this.label_VelocityV);
            this.pnl_AjVel.Location = new System.Drawing.Point(9, 9);
            this.pnl_AjVel.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_AjVel.Name = "pnl_AjVel";
            this.pnl_AjVel.Size = new System.Drawing.Size(170, 66);
            this.pnl_AjVel.TabIndex = 46;
            // 
            // pnl_AjPos
            // 
            this.pnl_AjPos.Controls.Add(this.chkBox_Zero);
            this.pnl_AjPos.Controls.Add(this.label_Position);
            this.pnl_AjPos.Controls.Add(this.comBox_Position);
            this.pnl_AjPos.Location = new System.Drawing.Point(9, 85);
            this.pnl_AjPos.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_AjPos.Name = "pnl_AjPos";
            this.pnl_AjPos.Size = new System.Drawing.Size(170, 66);
            this.pnl_AjPos.TabIndex = 47;
            // 
            // chkBox_Zero
            // 
            this.chkBox_Zero.AutoSize = true;
            this.chkBox_Zero.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkBox_Zero.Location = new System.Drawing.Point(101, 3);
            this.chkBox_Zero.Margin = new System.Windows.Forms.Padding(0);
            this.chkBox_Zero.Name = "chkBox_Zero";
            this.chkBox_Zero.Size = new System.Drawing.Size(51, 21);
            this.chkBox_Zero.TabIndex = 48;
            this.chkBox_Zero.Text = "原点";
            this.chkBox_Zero.UseVisualStyleBackColor = true;
            this.chkBox_Zero.CheckedChanged += new System.EventHandler(this.chkBox_Zero_CheckedChanged);
            // 
            // X_Painter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 238);
            this.Controls.Add(this.pnl_AjPos);
            this.Controls.Add(this.pnl_AjVel);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_AjPos);
            this.Controls.Add(this.btn_AjVel);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "X_Painter";
            this.Text = "小车编辑";
            ((System.ComponentModel.ISupportInitialize)(this.trkBar_Velocity)).EndInit();
            this.pnl_AjVel.ResumeLayout(false);
            this.pnl_AjVel.PerformLayout();
            this.pnl_AjPos.ResumeLayout(false);
            this.pnl_AjPos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_VelocityV;
        private System.Windows.Forms.TrackBar trkBar_Velocity;
        private System.Windows.Forms.ComboBox comBox_Position;
        private System.Windows.Forms.Label label_Position;
        private System.Windows.Forms.Label label_Velocity;
        private System.Windows.Forms.Button btn_AjVel;
        private System.Windows.Forms.Button btn_AjPos;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Panel pnl_AjVel;
        private System.Windows.Forms.Panel pnl_AjPos;
        private System.Windows.Forms.CheckBox chkBox_Zero;
    }
}