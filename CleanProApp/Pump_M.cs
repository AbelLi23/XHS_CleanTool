using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class Pump_M : Form
    {
        public int stepId = 0;
        public string PumpSet = string.Empty;
        public bool OnlyTime = false;
        bool useK = false;
        public Pump_M(int index)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[3];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            stepId = index; PumpSet = FormRoot.Printer.CleanProcess[index];
            FormRoot.ExplainAction(PumpSet);//Analyze single Command;
            useK = (PumpSet.Substring(PumpSet.IndexOf('n') + 1) == "0") ? true : false;

            if ("0" == PumpSet.Trim('@', ';').Substring(1, 1))
            {
                OnlyTime = true;

                trkBar_Intensity.Visible = false;
                trkBar_HoldTime.Maximum = AppCfg.Default.M_Ttt_Max; trkBar_HoldTime.Minimum = AppCfg.Default.M_Ttt_Min;
                trkBar_HoldTime.LargeChange = trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_WaitTime.Visible = trkBar_CycleNum.Visible = false;

                label_Intensity.Visible = label_WaitTime.Visible = label_CycleNum.Visible = false;
                label_HoldTime.Text = "抽废墨时间";

                trkBar_Intensity.Visible = trkBar_WaitTime.Visible = trkBar_CycleNum.Visible = false;

                trkBar_HoldTime.Value = (useK) ? AppCfg.Default.M_Ttt_K022 : FormRoot.Printer.M_OnlyWorkTime;

                label_IntensityV.Visible = label_WaitTimeV.Visible = label_CycleNumV.Visible = false;
                label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.M_Ttt_Rto).ToString());
            }
            else
            {
                OnlyTime = false;

                trkBar_Intensity.Maximum = AppCfg.Default.M_Pow_Max; trkBar_Intensity.Minimum = AppCfg.Default.M_Pow_Min;
                trkBar_Intensity.LargeChange = trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
                trkBar_HoldTime.Maximum = AppCfg.Default.M_WrT_Max; trkBar_HoldTime.Minimum = AppCfg.Default.M_WrT_Min;
                trkBar_HoldTime.LargeChange = trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_WaitTime.Maximum = AppCfg.Default.M_SpT_Max; trkBar_WaitTime.Minimum = AppCfg.Default.M_SpT_Min;
                trkBar_WaitTime.LargeChange = trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
                trkBar_CycleNum.Maximum = AppCfg.Default.M_Cyc_Max; trkBar_CycleNum.Minimum = AppCfg.Default.M_Cyc_Min;
                trkBar_CycleNum.LargeChange = trkBar_CycleNum.SmallChange = trkBar_CycleNum.TickFrequency = 1;

                label_Intensity.Text = "抽墨强度";
                label_HoldTime.Text = "抽墨时间";
                label_WaitTime.Text = "间隔时间";
                label_CycleNum.Text = "抽墨次数";

                trkBar_Intensity.Value = (useK) ? AppCfg.Default.M_Pow_K056 : FormRoot.Printer.M_Strength;
                trkBar_HoldTime.Value = (useK) ? AppCfg.Default.M_WrT_K029 : FormRoot.Printer.M_WorkTime;
                trkBar_WaitTime.Value = (useK) ? AppCfg.Default.M_SpT_K030 : FormRoot.Printer.M_HoldTime;
                trkBar_CycleNum.Value = (useK) ? AppCfg.Default.M_Cyc_K010 : FormRoot.Printer.M_CycleNum;

                label_IntensityV.Text = string.Format("{0} 级", FormRoot.Printer.OrderNum[(int)(trkBar_Intensity.Value * AppCfg.Default.M_Pow_Rto)]);
                label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.M_WrT_Rto).ToString("#0.0"));
                label_WaitTimeV.Text = string.Format("{0} 秒", (trkBar_WaitTime.Value * AppCfg.Default.M_SpT_Rto).ToString("#0.0"));
                label_CycleNumV.Text = string.Format("{0} 次", (trkBar_CycleNum.Value * AppCfg.Default.M_Cyc_Rto).ToString());
            }
        }

        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if ((Button)sender == btn_OK)
            {
                if (OnlyTime)
                {
                    FormRoot.Printer.M_OnlyWorkTime = trkBar_HoldTime.Value;
                    PumpSet = string.Format("@M0{0}", FormRoot.Printer.M_OnlyWorkTime);
                    FormRoot.Printer.CleanProcess[stepId] = PumpSet;
                }
                else
                {
                    FormRoot.Printer.M_Strength = trkBar_Intensity.Value;
                    FormRoot.Printer.M_WorkTime = trkBar_HoldTime.Value;
                    FormRoot.Printer.M_HoldTime = trkBar_WaitTime.Value;
                    FormRoot.Printer.M_CycleNum = trkBar_CycleNum.Value;
                    PumpSet = string.Format("@M1x{0}p{1}s{2}n{3}", FormRoot.Printer.M_Strength, FormRoot.Printer.M_WorkTime,
                        FormRoot.Printer.M_HoldTime, FormRoot.Printer.M_CycleNum);
                    FormRoot.Printer.CleanProcess[stepId] = PumpSet;
                }
            }
            this.Close();
        }
        private void TrackBarValued(object sender, System.EventArgs e)
        {
            if (OnlyTime)
            {
                label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.M_Ttt_Rto).ToString());
            }
            else
            {
                label_IntensityV.Text = string.Format("{0} 级", FormRoot.Printer.OrderNum[(int)(trkBar_Intensity.Value * AppCfg.Default.M_Pow_Rto)]);
                label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.M_WrT_Rto).ToString("#0.0"));
                label_WaitTimeV.Text = string.Format("{0} 秒", (trkBar_WaitTime.Value * AppCfg.Default.M_SpT_Rto).ToString("#0.0"));
                label_CycleNumV.Text = string.Format("{0} 次", (trkBar_CycleNum.Value * AppCfg.Default.M_Cyc_Rto).ToString());
            }
        }
    }
}
