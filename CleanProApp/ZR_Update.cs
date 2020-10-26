using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CleanProApp
{
    public partial class ZR_Update : Form
    {
        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);

        public string fileIn = "";
        public ZR_Update(bool pakFirst)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[6];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            this.Size = new Size(420, 300);
            pnl_update.Location = pnl_toDat.Location;
            txt_fileIn.ReadOnly = txt_pdat.ReadOnly = txt_ptxt.ReadOnly = true;
            if (pakFirst)
            {
                pnl_toDat.Visible = true; pnl_update.Visible = false; label_fileIn.Text = "拖入流程文件:";
                txt_ptxt.Text = "清洗流程文件";
            }
            else
            {
                pnl_toDat.Visible = false; pnl_update.Visible = true; label_fileIn.Text = "拖入数据包:";
                txt_pdat.Text = "清洗数据包文件";
            }
            this.textBox1_MouseDown(null, null);
        }
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Link : DragDropEffects.None;

        }
        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            fileIn = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            string ext = Path.GetExtension(fileIn);
            if (".txt" == ext && !string.IsNullOrEmpty(fileIn))
            {
                txt_ptxt.Text = txt_pdat.Text = Path.GetFileName(fileIn);
            }
            else if (".dat" == ext && !string.IsNullOrEmpty(fileIn))
            {
                txt_ptxt.Text = txt_pdat.Text = Path.GetFileName(fileIn);
            }
        }
        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //HideCaret(((TextBox)sender).Handle);
            HideCaret(txt_fileIn.Handle);
            HideCaret(txt_ptxt.Handle);
            HideCaret(txt_pdat.Handle);
        }
        private void txt_ptxt_TextChanged(object sender, EventArgs e)
        {
            string ext = Path.GetExtension(fileIn);
            if ((TextBox)sender == txt_ptxt)
            {
                if (txt_ptxt.Text != "" && ".txt" == ext) btn_toDat.Enabled = true;
                else btn_toDat.Enabled = false;
            }
            else if ((TextBox)sender == txt_pdat)
            {
                if (txt_pdat.Text != "" && ".dat" == ext) btn_update.Enabled = true;
                else btn_update.Enabled = false;
            }
        }
        private void btn_opentxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.InitialDirectory = FormRoot.Printer.F_defaultPath;
            openDlg.FileName = "";
            if ((Button)sender == btn_toDat)
            {
                openDlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            }
            else if ((Button)sender == btn_update)
            {
                openDlg.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            }
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                fileIn = openDlg.FileName;
            }
            else return;

            string ext = Path.GetExtension(fileIn);
            if (".txt" == ext && !string.IsNullOrEmpty(fileIn))
            {
                txt_ptxt.Text = Path.GetFileName(fileIn);
            }
            else if (".dat" == ext && !string.IsNullOrEmpty(fileIn))
            {
                txt_pdat.Text = Path.GetFileName(fileIn);
            }
        }
        private void ConvertOrUpdate(object sender, EventArgs e)
        {
            if ((Button)sender == btn_toDat)
            {
                string DATPath = Path.GetDirectoryName(fileIn);
                string DATname = Path.GetFileNameWithoutExtension(fileIn) + ".dat";
                string DATFile = DATPath + "\\" + DATname;
                string content = FormRoot.Printer.F_CleanTxtCVRT_Dat(fileIn);

                if (File.Exists(DATFile)) File.Delete(DATFile);

                FileStream fs = new FileStream(DATFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(content);
                sw.Close();
                fs.Close();

                if (MessageBox.Show("流程文件已经打包完成，点击确定以查看", "查看数据包", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start("Explorer", "/select," + DATPath + "\\" + DATname);
                }
            }
            else if ((Button)sender == btn_update)
            {
                string DATA = string.Empty;
                FileStream fs = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
                fs.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(fs);
                DATA = sr.ReadToEnd();
                bool SendSuccess = FormRoot.Printer.F_SendDatToPrt(DATA);
                MessageBox.Show(SendSuccess ? "数据包已成功上传" : "上传失败", "提示:", MessageBoxButtons.OK, SendSuccess ? MessageBoxIcon.Asterisk : MessageBoxIcon.Error);
            }
        }
    }
}
