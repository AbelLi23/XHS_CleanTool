using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class Pump_S : Form
    {
        public int stepId = 0;
        public string PumpSet = string.Empty;
        bool useK = false;
        public Pump_S(int index)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[4];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            stepId = index; PumpSet = FormRoot.Printer.CleanProcess[index];
            FormRoot.ExplainAction(PumpSet);//Analyze single Command;
            useK = (PumpSet.Substring(PumpSet.IndexOf('n') + 1) == "0") ? true : false;

            trkBar_Intensity.Maximum = AppCfg.Default.V_Pow_Max; trkBar_Intensity.Minimum = AppCfg.Default.V_Pow_Min;
            trkBar_Intensity.LargeChange = trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
            trkBar_HoldTime.Maximum = AppCfg.Default.V_WrT_Max; trkBar_HoldTime.Minimum = AppCfg.Default.V_WrT_Min;
            trkBar_HoldTime.LargeChange = trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
            trkBar_WaitTime.Maximum = AppCfg.Default.V_SpT_Max; trkBar_WaitTime.Minimum = AppCfg.Default.V_SpT_Min;
            trkBar_WaitTime.LargeChange = trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
            trkBar_CycleNum.Maximum = AppCfg.Default.V_Cyc_Max; trkBar_CycleNum.Minimum = AppCfg.Default.V_Cyc_Min;
            trkBar_CycleNum.LargeChange = trkBar_CycleNum.SmallChange = trkBar_CycleNum.TickFrequency = 1;

            label_Intensity.Text = "闪喷强度";
            label_HoldTime.Text = "闪喷时间";
            label_WaitTime.Text = "间隔时间";
            label_CycleNum.Text = "闪喷次数";

            trkBar_Intensity.Value = (useK) ? AppCfg.Default.V_Pow_Kvvv : FormRoot.Printer.V_Strength;
            trkBar_HoldTime.Value = (useK) ? AppCfg.Default.V_WrT_Kvvv : FormRoot.Printer.V_WorkTime;
            trkBar_WaitTime.Value = (useK) ? AppCfg.Default.V_SpT_Kvvv : FormRoot.Printer.V_HoldTime;
            trkBar_CycleNum.Value = (useK) ? AppCfg.Default.V_Cyc_K007 : FormRoot.Printer.V_CycleNum;

            label_IntensityV.Text = string.Format("{0} 级", FormRoot.Printer.OrderNum[(int)(trkBar_Intensity.Value * AppCfg.Default.V_Pow_Rto)]);
            label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.V_WrT_Rto).ToString("#0.0"));
            label_WaitTimeV.Text = string.Format("{0} 秒", (trkBar_WaitTime.Value * AppCfg.Default.V_SpT_Rto).ToString("#0.0"));
            label_CycleNumV.Text = string.Format("{0} 次", (trkBar_CycleNum.Value * AppCfg.Default.V_Cyc_Rto).ToString());
        }

        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if ((Button)sender == btn_OK)
            {
                FormRoot.Printer.V_Strength = trkBar_Intensity.Value;
                FormRoot.Printer.V_WorkTime = trkBar_HoldTime.Value;
                FormRoot.Printer.V_HoldTime = trkBar_WaitTime.Value;
                FormRoot.Printer.V_CycleNum = trkBar_CycleNum.Value;

                PumpSet = string.Format("@V{0}p{1}s{2}n{3}", FormRoot.Printer.V_Strength, FormRoot.Printer.V_WorkTime,
                    FormRoot.Printer.V_HoldTime, FormRoot.Printer.V_CycleNum);
                FormRoot.Printer.CleanProcess[stepId] = PumpSet;
            }
            this.Close();
        }

        private void TrackBarValued(object sender, System.EventArgs e)
        {
            label_IntensityV.Text = string.Format("{0} 级", FormRoot.Printer.OrderNum[(int)(trkBar_Intensity.Value * AppCfg.Default.V_Pow_Rto)]);
            label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.V_WrT_Rto).ToString("#0.0"));
            label_WaitTimeV.Text = string.Format("{0} 秒", (trkBar_WaitTime.Value * AppCfg.Default.V_SpT_Rto).ToString("#0.0"));
            label_CycleNumV.Text = string.Format("{0} 次", (trkBar_CycleNum.Value * AppCfg.Default.V_Cyc_Rto).ToString());
        }
    }
}
