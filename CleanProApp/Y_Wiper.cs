using System.Collections.Generic;
using System.Windows.Forms;

namespace CleanProApp
{
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
                if (0 == int.Parse(WiperSet.Trim('@', ';').Substring(2))) chkBox_Zero.Checked = true;
                else
                {
                    comBox_Position.SelectedIndex = int.Parse(WiperSet.Trim('@', ';').Substring(2)) - 1;
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
                    if (IsInsert) FormRoot.Printer.CleanProcess.Insert(lstId + 1, string.Format("@Wn{0};", comBox_Position.SelectedIndex + 1));
                    else
                        FormRoot.Printer.CleanProcess[lstId] = string.Format("@Wn{0};", comBox_Position.SelectedIndex + 1);
                }

                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void chkBox_Zero_CheckedChanged(object sender, System.EventArgs e)
        {
            comBox_Position.Enabled = !chkBox_Zero.Checked;
        }
    }
}
