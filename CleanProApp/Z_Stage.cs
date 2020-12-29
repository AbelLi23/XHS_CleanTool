using System.Collections.Generic;
using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
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
                if (IsInsert)
                {
                    bool rst = false; int tmpVal = 0; //int Pos = 0;
                    rst = int.TryParse(txtBox_Position.Text, out tmpVal);
                    if (tmpVal < AppCfg.Default.C_Pos_Min || tmpVal > AppCfg.Default.C_Pos_Max || !rst)
                    {
                        MessageBox.Show("墨栈位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[0])
                        AppCfg.Default.C_Pos0_K999 = rst ? tmpVal : AppCfg.Default.C_Pos0_K999;
                    else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[1])
                        AppCfg.Default.C_Pos1_K026 = rst ? tmpVal : AppCfg.Default.C_Pos1_K026;
                    else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[2])
                        AppCfg.Default.C_Pos2_K012 = rst ? tmpVal : AppCfg.Default.C_Pos2_K012;
                    else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[4])
                        AppCfg.Default.C_Pos4_K059 = rst ? tmpVal : AppCfg.Default.C_Pos4_K059;

                    FormRoot.Printer.CleanProcess.Insert(lstId + 1, string.Format("@C{0}{1}",
                    StageSet.Trim('@', ';').Substring(1, 1), comBox_Position.SelectedIndex));
                }
                else
                {
                    bool rst = false; int tmpVal = 0; //int Pos = 0;
                    rst = int.TryParse(txtBox_Position.Text, out tmpVal);
                    if (tmpVal < AppCfg.Default.C_Pos_Min || tmpVal > AppCfg.Default.C_Pos_Max || !rst)
                    {
                        MessageBox.Show("墨栈位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[0])
                        AppCfg.Default.C_Pos0_K999 = rst ? tmpVal : AppCfg.Default.C_Pos0_K999;
                    else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[1])
                        AppCfg.Default.C_Pos1_K026 = rst ? tmpVal : AppCfg.Default.C_Pos1_K026;
                    else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[2])
                        AppCfg.Default.C_Pos2_K012 = rst ? tmpVal : AppCfg.Default.C_Pos2_K012;
                    else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[4])
                        AppCfg.Default.C_Pos4_K059 = rst ? tmpVal : AppCfg.Default.C_Pos4_K059;

                    FormRoot.Printer.CleanProcess[lstId] = string.Format("@C{0}{1}",
                    StageSet.Trim('@', ';').Substring(1, 1), comBox_Position.SelectedIndex);
                }
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void comBox_Position_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comBox_Position.SelectedIndex == -1) return;

            if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[3])
            {
                txtBox_Position.Enabled = false;
                txtBox_Position.Text = "0";
            }
            else
            {
                txtBox_Position.Enabled = true;
                if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[0])
                    txtBox_Position.Text = AppCfg.Default.C_Pos0_K999.ToString();
                else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[1])
                    txtBox_Position.Text = AppCfg.Default.C_Pos1_K026.ToString();
                else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[2])
                    txtBox_Position.Text = AppCfg.Default.C_Pos2_K012.ToString();
                else if (comBox_Position.SelectedItem.ToString() == FormRoot.Printer.C_LevelMark[4])
                    txtBox_Position.Text = AppCfg.Default.C_Pos4_K059.ToString();
            }
        }
    }
}
