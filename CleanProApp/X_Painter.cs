using System.Drawing;
using System.Windows.Forms;

namespace CleanProApp
{
    using System.Collections.Generic;
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class X_Painter : Form
    {
        public string PainterSet = string.Empty;
        public int Vel = AppCfg.Default.P_Vel_Min;
        public int Pos = 0;
        public int PN = 0;
        public int StepIndex = 0;
        public bool IsInsert = false;
        public X_Painter(int index, bool IsNew)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[0];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            //trkBar_Velocity
            trkBar_Velocity.Maximum = AppCfg.Default.P_Vel_Max; trkBar_Velocity.Minimum = AppCfg.Default.P_Vel_Min;
            trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
            trkBar_Velocity.Value = trkBar_Velocity.Minimum;

            //comBox_Position
            List<string> items = new List<string>();
            if (FormRoot.CleanMode.W1_C_PP == FormRoot.Printer.CleanMode || FormRoot.CleanMode.W0C_PP == FormRoot.Printer.CleanMode || FormRoot.CleanMode.W1C_PP == FormRoot.Printer.CleanMode)
            {
                for (int pn = 0; pn < FormRoot.Printer.P_Counts; pn++)
                {
                    items.Add((pn + 1).ToString() + "号喷头刮墨前");
                    items.Add((pn + 1).ToString() + "号喷头刮墨后");
                }
            }
            else
            {
                for (int pn = 0; pn < FormRoot.Printer.P_Counts; pn++)
                {
                    items.Add((pn + 1).ToString() + "号喷头刮墨");
                }
            }
            comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
            //Control UI arrange
            btn_AjPos.Location = new Point((this.ClientSize.Width - 2 * btn_AjPos.Width) / 2, (this.ClientSize.Height - btn_AjPos.Height) / 2);
            btn_AjVel.Location = new Point(btn_AjPos.Location.X + btn_AjPos.Width, btn_AjPos.Location.Y);
            pnl_AjPos.Location = new Point((this.ClientSize.Width - pnl_AjPos.Width) / 2, (this.ClientSize.Height - pnl_AjPos.Height - btn_OK.Height) / 2);
            pnl_AjVel.Location = pnl_AjPos.Location;
            //Step analyze
            StepIndex = index; IsInsert = IsNew;
            PainterSet = FormRoot.Printer.CleanProcess[index];
            if (IsNew)
            {
                btn_AjVel.Visible = btn_AjPos.Visible = true;
                pnl_AjPos.Visible = pnl_AjVel.Visible = false;
                btn_OK.Visible = btn_Cancel.Visible = false;
            }
            else if ("p" == PainterSet.Trim('@', ';').Substring(1, 1))
            {
                btn_AjVel.Visible = btn_AjPos.Visible = false;
                pnl_AjVel.Visible = true; pnl_AjPos.Visible = false;
                btn_OK.Visible = btn_Cancel.Visible = true;

                trkBar_Velocity.Value = int.Parse(PainterSet.Trim('@', ';').Substring(2));
                label_VelocityV.Text = (AppCfg.Default.P_Vel_Rto * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";
            }
            else
            {
                btn_AjVel.Visible = btn_AjPos.Visible = false;
                pnl_AjPos.Visible = true; pnl_AjVel.Visible = false;
                btn_OK.Visible = btn_Cancel.Visible = true;

                var pn = int.Parse(PainterSet.Trim('@', ';').Substring(1, 1));
                if (pn == 0) chkBox_Zero.Checked = true;
                else
                {
                    if (FormRoot.CleanMode.W0C_PP == FormRoot.Printer.CleanMode || FormRoot.CleanMode.W1_C_PP == FormRoot.Printer.CleanMode || FormRoot.CleanMode.W1C_PP == FormRoot.Printer.CleanMode)
                    {
                        comBox_Position.SelectedIndex = (FormRoot.Printer.P_XPos[2 * pn - 2] == int.Parse(PainterSet.Trim('@', ';').Substring(2))) ? (2 * pn - 2) : (2 * pn - 1);
                    }
                    else
                        comBox_Position.SelectedIndex = pn - 1;
                }
            }
        }
        private void OptBeforeAjst(object sender, System.EventArgs e)
        {
            if (btn_AjVel == (Button)sender)
            {
                btn_AjVel.Visible = btn_AjPos.Visible = false;
                pnl_AjVel.Visible = true; pnl_AjPos.Visible = false;
                btn_OK.Visible = btn_Cancel.Visible = true;
                trkBar_Velocity.Value = trkBar_Velocity.Minimum;
            }
            else
            {
                btn_AjVel.Visible = btn_AjPos.Visible = false;
                pnl_AjPos.Visible = true; pnl_AjVel.Visible = false;
                btn_OK.Visible = btn_Cancel.Visible = true;
            }
        }
        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if (btn_OK == (Button)sender)
            {
                if ("p" == PainterSet.Trim('@', ';').Substring(1, 1))
                {
                    PainterSet = string.Format("@Pp{0};", trkBar_Velocity.Value);
                }
                else
                {
                    PainterSet = (chkBox_Zero.Checked) ? string.Format("@P00;") : string.Format("@P{0}{1};", PN, Pos);
                }
                //FormRoot.Printer.CleanProcess.Insert(IsInsert ? StepIndex + 1 : StepIndex, PainterSet);
                if (IsInsert) FormRoot.Printer.CleanProcess.Insert(StepIndex + 1, PainterSet);
                else FormRoot.Printer.CleanProcess[StepIndex] = string.Format(PainterSet);
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

        private void trkBar_Velocity_ValueChanged(object sender, System.EventArgs e)
        {
            label_VelocityV.Text = (AppCfg.Default.P_Vel_Rto * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";
        }

        private void comBox_Position_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //No Necessary
            if (comBox_Position.SelectedIndex == -1) return;
            if (FormRoot.CleanMode.W0C_PP == FormRoot.Printer.CleanMode || FormRoot.CleanMode.W1_C_PP == FormRoot.Printer.CleanMode || FormRoot.CleanMode.W1C_PP == FormRoot.Printer.CleanMode)
            {
                if (comBox_Position.SelectedIndex % 2 == 0) PN = comBox_Position.SelectedIndex / 2 + 1;
                else PN = (comBox_Position.SelectedIndex + 1) / 2;
            }
            else
                PN = comBox_Position.SelectedIndex + 1;
            Pos = FormRoot.Printer.P_XPos[comBox_Position.SelectedIndex];
        }
    }
}
