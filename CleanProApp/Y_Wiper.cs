using System.Collections.Generic;
using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class Y_Wiper : Form
    {
        public int lstId = 0;
        public bool IsInsert = false;
        public string WiperSet = string.Empty;
        public Y_Wiper(int index, bool IsNew)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[1];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            List<string> items = new List<string>();
            //Printer.W_YPos = new List<int>(items.Count); 刮片只考虑一个位置
            for (int wn = 0; wn < FormRoot.Printer.P_Counts; wn++)
            {
                items.Add((wn + 1).ToString() + "号喷头刮墨");
            }
            comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;

            lstId = index; IsInsert = IsNew;
            if (!IsNew)
            {
                WiperSet = FormRoot.Printer.CleanProcess[index];
                if (WiperSet.Trim('@', ';').Substring(0, 1) == "H")
                {
                    comBox_Position.SelectedIndex = int.Parse(WiperSet.Trim('@', ';').Substring(1, 1)) - 1;
                }
                else
                {
                    chkBox_Zero.Checked = true; // Here only used "Wn0";
                }
            }
        }
        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if (btn_OK == (Button)sender)
            {
                if (chkBox_Zero.Checked)
                {
                    if (IsInsert) FormRoot.Printer.CleanProcess.Insert(lstId + 1, string.Format("@Wn0;"));
                    else
                        FormRoot.Printer.CleanProcess[lstId] = string.Format("@Wn0;");
                }
                else
                {
                    if (comBox_Position.SelectedIndex == -1)
                    {
                        MessageBox.Show("必须选择有效的刮片位置", "提示:", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    if (IsInsert)
                    {
                        bool rst = false; int tmpVal = 0; int Pos = 0;
                        rst = int.TryParse(txtBox_Position.Text, out tmpVal);
                        if (tmpVal < AppCfg.Default.W_Pos_Min || tmpVal > AppCfg.Default.W_Pos_Max || !rst)
                        {
                            MessageBox.Show("刮片位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        Pos = FormRoot.Printer.W_YPos[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
                        FormRoot.Printer.CleanProcess.Insert(lstId + 1, string.Format("@H{0}{1};",
                                comBox_Position.SelectedIndex + 1, Pos));
                    }
                    else
                    {
                        bool rst = false; int tmpVal = 0; int Pos = 0;
                        rst = int.TryParse(txtBox_Position.Text, out tmpVal);
                        if (tmpVal < AppCfg.Default.W_Pos_Min || tmpVal > AppCfg.Default.W_Pos_Max || !rst)
                        {
                            MessageBox.Show("刮片位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        Pos = FormRoot.Printer.W_YPos[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
                        FormRoot.Printer.CleanProcess[lstId] = string.Format("@H{0}{1};",
                                comBox_Position.SelectedIndex + 1, Pos);
                    }

                }

                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void chkBox_Zero_CheckedChanged(object sender, System.EventArgs e)
        {
            comBox_Position.Enabled = txtBox_Position.Enabled = !chkBox_Zero.Checked;
        }

        private void comBox_Position_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comBox_Position.SelectedIndex == -1) return;

            txtBox_Position.Text = FormRoot.Printer.W_YPos[comBox_Position.SelectedIndex].ToString();
        }
    }
}
