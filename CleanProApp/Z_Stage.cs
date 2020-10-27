using System.Collections.Generic;
using System.Windows.Forms;

namespace CleanProApp
{
    public partial class Z_Stage : Form
    {
        public int lstId = 0;
        public string StageSet = string.Empty;
        public bool IsInsert = false;
        public Z_Stage(int index, bool IsNew)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[2];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            lstId = index; IsInsert = IsNew;
            List<string> items = new List<string>(FormRoot.Printer.C_LevelMark);
            comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
            StageSet = FormRoot.Printer.CleanProcess[index];
            if (!IsNew)
            {
                comBox_Position.SelectedIndex = int.Parse(StageSet.Trim('@', ';').Substring(2));
            }
        }
        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if (btn_OK == (Button)sender)
            {
                if (comBox_Position.SelectedIndex == -1)
                {
                    MessageBox.Show("必须选择有效的墨栈位置", "提示:", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (IsInsert) FormRoot.Printer.CleanProcess.Insert(lstId + 1, string.Format("@C{0}{1}",
                    StageSet.Trim('@', ';').Substring(1, 1), comBox_Position.SelectedIndex));
                else
                    FormRoot.Printer.CleanProcess[lstId] = string.Format("@C{0}{1}",
                    StageSet.Trim('@', ';').Substring(1, 1), comBox_Position.SelectedIndex);

                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;

            this.Close();
        }
    }
}
