namespace CleanProApp
{
    partial class ZR_Update
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
            this.txt_fileIn = new System.Windows.Forms.TextBox();
            this.label_fileIn = new System.Windows.Forms.Label();
            this.txt_ptxt = new System.Windows.Forms.TextBox();
            this.txt_pdat = new System.Windows.Forms.TextBox();
            this.label_protxt = new System.Windows.Forms.Label();
            this.label_prodat = new System.Windows.Forms.Label();
            this.btn_toDat = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.pnl_toDat = new System.Windows.Forms.Panel();
            this.pnl_update = new System.Windows.Forms.Panel();
            this.pnl_FileIn = new System.Windows.Forms.Panel();
            this.btn_opentxt = new System.Windows.Forms.Button();
            this.btn_opendat = new System.Windows.Forms.Button();
            this.pnl_toDat.SuspendLayout();
            this.pnl_update.SuspendLayout();
            this.pnl_FileIn.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_fileIn
            // 
            this.txt_fileIn.AllowDrop = true;
            this.txt_fileIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_fileIn.Cursor = System.Windows.Forms.Cursors.Default;
            this.txt_fileIn.Font = new System.Drawing.Font("SimSun", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_fileIn.ForeColor = System.Drawing.SystemColors.Window;
            this.txt_fileIn.Location = new System.Drawing.Point(10, 25);
            this.txt_fileIn.Margin = new System.Windows.Forms.Padding(0);
            this.txt_fileIn.Name = "txt_fileIn";
            this.txt_fileIn.ReadOnly = true;
            this.txt_fileIn.Size = new System.Drawing.Size(131, 71);
            this.txt_fileIn.TabIndex = 0;
            this.txt_fileIn.TabStop = false;
            this.txt_fileIn.Text = "+";
            this.txt_fileIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fileIn.WordWrap = false;
            this.txt_fileIn.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.txt_fileIn.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            this.txt_fileIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            // 
            // label_fileIn
            // 
            this.label_fileIn.AutoSize = true;
            this.label_fileIn.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_fileIn.Location = new System.Drawing.Point(10, 7);
            this.label_fileIn.Margin = new System.Windows.Forms.Padding(0);
            this.label_fileIn.Name = "label_fileIn";
            this.label_fileIn.Size = new System.Drawing.Size(131, 17);
            this.label_fileIn.TabIndex = 0;
            this.label_fileIn.Text = "拖入流程文件或数据包:";
            // 
            // txt_ptxt
            // 
            this.txt_ptxt.Location = new System.Drawing.Point(10, 27);
            this.txt_ptxt.Margin = new System.Windows.Forms.Padding(0);
            this.txt_ptxt.Name = "txt_ptxt";
            this.txt_ptxt.ReadOnly = true;
            this.txt_ptxt.Size = new System.Drawing.Size(200, 21);
            this.txt_ptxt.TabIndex = 2;
            this.txt_ptxt.TextChanged += new System.EventHandler(this.txt_ptxt_TextChanged);
            this.txt_ptxt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            // 
            // txt_pdat
            // 
            this.txt_pdat.Location = new System.Drawing.Point(12, 27);
            this.txt_pdat.Margin = new System.Windows.Forms.Padding(0);
            this.txt_pdat.Name = "txt_pdat";
            this.txt_pdat.ReadOnly = true;
            this.txt_pdat.Size = new System.Drawing.Size(200, 21);
            this.txt_pdat.TabIndex = 3;
            this.txt_pdat.TextChanged += new System.EventHandler(this.txt_ptxt_TextChanged);
            this.txt_pdat.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            // 
            // label_protxt
            // 
            this.label_protxt.AutoSize = true;
            this.label_protxt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_protxt.Location = new System.Drawing.Point(10, 3);
            this.label_protxt.Margin = new System.Windows.Forms.Padding(0);
            this.label_protxt.Name = "label_protxt";
            this.label_protxt.Size = new System.Drawing.Size(83, 17);
            this.label_protxt.TabIndex = 0;
            this.label_protxt.Text = "清洗流程文件:";
            // 
            // label_prodat
            // 
            this.label_prodat.AutoSize = true;
            this.label_prodat.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_prodat.Location = new System.Drawing.Point(10, 3);
            this.label_prodat.Margin = new System.Windows.Forms.Padding(0);
            this.label_prodat.Name = "label_prodat";
            this.label_prodat.Size = new System.Drawing.Size(95, 17);
            this.label_prodat.TabIndex = 0;
            this.label_prodat.Text = "清洗流程数据包:";
            // 
            // btn_toDat
            // 
            this.btn_toDat.Enabled = false;
            this.btn_toDat.Image = global::CleanProApp.Properties.Resources.Pakdat;
            this.btn_toDat.Location = new System.Drawing.Point(218, 3);
            this.btn_toDat.Margin = new System.Windows.Forms.Padding(0);
            this.btn_toDat.Name = "btn_toDat";
            this.btn_toDat.Size = new System.Drawing.Size(60, 45);
            this.btn_toDat.TabIndex = 6;
            this.btn_toDat.Text = "打\r\n包";
            this.btn_toDat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_toDat.UseVisualStyleBackColor = true;
            this.btn_toDat.Click += new System.EventHandler(this.ConvertOrUpdate);
            // 
            // btn_update
            // 
            this.btn_update.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_update.Enabled = false;
            this.btn_update.Image = global::CleanProApp.Properties.Resources.Update1;
            this.btn_update.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_update.Location = new System.Drawing.Point(218, 3);
            this.btn_update.Margin = new System.Windows.Forms.Padding(0);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(60, 45);
            this.btn_update.TabIndex = 7;
            this.btn_update.Text = "上\r\n传";
            this.btn_update.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.ConvertOrUpdate);
            // 
            // pnl_toDat
            // 
            this.pnl_toDat.Controls.Add(this.btn_opentxt);
            this.pnl_toDat.Controls.Add(this.label_protxt);
            this.pnl_toDat.Controls.Add(this.txt_ptxt);
            this.pnl_toDat.Controls.Add(this.btn_toDat);
            this.pnl_toDat.Location = new System.Drawing.Point(53, 152);
            this.pnl_toDat.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_toDat.Name = "pnl_toDat";
            this.pnl_toDat.Size = new System.Drawing.Size(300, 52);
            this.pnl_toDat.TabIndex = 8;
            // 
            // pnl_update
            // 
            this.pnl_update.Controls.Add(this.btn_opendat);
            this.pnl_update.Controls.Add(this.label_prodat);
            this.pnl_update.Controls.Add(this.txt_pdat);
            this.pnl_update.Controls.Add(this.btn_update);
            this.pnl_update.Location = new System.Drawing.Point(53, 224);
            this.pnl_update.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_update.Name = "pnl_update";
            this.pnl_update.Size = new System.Drawing.Size(300, 52);
            this.pnl_update.TabIndex = 9;
            // 
            // pnl_FileIn
            // 
            this.pnl_FileIn.Controls.Add(this.label_fileIn);
            this.pnl_FileIn.Controls.Add(this.txt_fileIn);
            this.pnl_FileIn.Location = new System.Drawing.Point(113, 36);
            this.pnl_FileIn.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_FileIn.Name = "pnl_FileIn";
            this.pnl_FileIn.Size = new System.Drawing.Size(152, 102);
            this.pnl_FileIn.TabIndex = 9;
            // 
            // btn_opentxt
            // 
            this.btn_opentxt.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_opentxt.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_opentxt.Location = new System.Drawing.Point(175, 7);
            this.btn_opentxt.Margin = new System.Windows.Forms.Padding(0);
            this.btn_opentxt.Name = "btn_opentxt";
            this.btn_opentxt.Size = new System.Drawing.Size(35, 20);
            this.btn_opentxt.TabIndex = 1;
            this.btn_opentxt.Text = "...";
            this.btn_opentxt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_opentxt.UseVisualStyleBackColor = false;
            this.btn_opentxt.Click += new System.EventHandler(this.btn_opentxt_Click);
            // 
            // btn_opendat
            // 
            this.btn_opendat.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_opendat.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_opendat.Location = new System.Drawing.Point(175, 7);
            this.btn_opendat.Margin = new System.Windows.Forms.Padding(0);
            this.btn_opendat.Name = "btn_opendat";
            this.btn_opendat.Size = new System.Drawing.Size(35, 20);
            this.btn_opendat.TabIndex = 1;
            this.btn_opendat.Text = "...";
            this.btn_opendat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_opendat.UseVisualStyleBackColor = false;
            this.btn_opendat.Click += new System.EventHandler(this.btn_opentxt_Click);
            // 
            // ZR_Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 304);
            this.Controls.Add(this.pnl_FileIn);
            this.Controls.Add(this.pnl_update);
            this.Controls.Add(this.pnl_toDat);
            this.Name = "ZR_Update";
            this.Text = "更新清洗流程";
            this.pnl_toDat.ResumeLayout(false);
            this.pnl_toDat.PerformLayout();
            this.pnl_update.ResumeLayout(false);
            this.pnl_update.PerformLayout();
            this.pnl_FileIn.ResumeLayout(false);
            this.pnl_FileIn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_fileIn;
        private System.Windows.Forms.Label label_fileIn;
        private System.Windows.Forms.TextBox txt_ptxt;
        private System.Windows.Forms.TextBox txt_pdat;
        private System.Windows.Forms.Label label_protxt;
        private System.Windows.Forms.Label label_prodat;
        private System.Windows.Forms.Button btn_toDat;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Panel pnl_toDat;
        private System.Windows.Forms.Panel pnl_update;
        private System.Windows.Forms.Panel pnl_FileIn;
        private System.Windows.Forms.Button btn_opentxt;
        private System.Windows.Forms.Button btn_opendat;


    }
}