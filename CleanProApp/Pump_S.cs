using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class Pump_S : Form
    {
        public int stepId = 0;
        public string PumpSet = string.Empty;
        public Pump_S(int index)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[4];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            stepId = index; PumpSet = FormRoot.Printer.CleanProcess[index];

            trkBar_Intensity.Maximum = AppCfg.Default.V_Pow_Max; trkBar_Intensity.Minimum = AppCfg.Default.V_Pow_Min;
            trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
            trkBar_HoldTime.Maximum = AppCfg.Default.V_WrT_Max; trkBar_HoldTime.Minimum = AppCfg.Default.V_WrT_Min;
            trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
            trkBar_WaitTime.Maximum = AppCfg.Default.V_SpT_Max; trkBar_WaitTime.Minimum = AppCfg.Default.V_SpT_Min;
            trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
            trkBar_CycleNum.Maximum = AppCfg.Default.V_Cyc_Max; trkBar_CycleNum.Minimum = AppCfg.Default.V_Cyc_Min;
            trkBar_CycleNum.SmallChange = trkBar_CycleNum.TickFrequency = 1;

            label_Intensity.Text = "闪喷强度";
            label_HoldTime.Text = "闪喷时间";
            label_WaitTime.Text = "间隔时间";
            label_CycleNum.Text = "闪喷次数";

            trkBar_Intensity.Value = AppCfg.Default.V_Pow_Kvv;
            trkBar_HoldTime.Value = AppCfg.Default.V_WrT_Kvv;
            trkBar_WaitTime.Value = AppCfg.Default.V_SpT_Kvv;
            trkBar_CycleNum.Value = AppCfg.Default.V_Cyc_Kvv;

            label_IntensityV.Text = string.Format("{0}级", (trkBar_Intensity.Value * AppCfg.Default.V_Pow_Rto).ToString("#0.0"));
            label_HoldTimeV.Text = string.Format("{0}秒", (trkBar_HoldTime.Value * AppCfg.Default.V_WrT_Rto).ToString("#0.0"));
            label_WaitTimeV.Text = string.Format("{0}秒", (trkBar_WaitTime.Value * AppCfg.Default.V_SpT_Rto).ToString("#0.0"));
            label_CycleNumV.Text = string.Format("{0}次", (trkBar_CycleNum.Value * AppCfg.Default.V_Cyc_Rto).ToString("#0.0"));
        }

        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if ((Button)sender == btn_OK)
            {
                AppCfg.Default.V_Pow_Kvv = trkBar_Intensity.Value;
                AppCfg.Default.V_WrT_Kvv = trkBar_HoldTime.Value;
                AppCfg.Default.V_SpT_Kvv = trkBar_WaitTime.Value;
                AppCfg.Default.V_Cyc_Kvv = trkBar_CycleNum.Value;
                PumpSet = string.Format("@V{0}p{1}s{2}n{3}", AppCfg.Default.V_Pow_Kvv, AppCfg.Default.V_WrT_Kvv,
                    AppCfg.Default.V_SpT_Kvv, AppCfg.Default.V_Cyc_Kvv);
            }
            this.Close();
        }

        private void TrackBarValued(object sender, System.EventArgs e)
        {
            label_IntensityV.Text = string.Format("{0}级", (trkBar_Intensity.Value * AppCfg.Default.V_Pow_Rto).ToString("#0.0"));
            label_HoldTimeV.Text = string.Format("{0}秒", (trkBar_HoldTime.Value * AppCfg.Default.V_WrT_Rto).ToString("#0.0"));
            label_WaitTimeV.Text = string.Format("{0}秒", (trkBar_WaitTime.Value * AppCfg.Default.V_SpT_Rto).ToString("#0.0"));
            label_CycleNumV.Text = string.Format("{0}次", (trkBar_CycleNum.Value * AppCfg.Default.V_Cyc_Rto).ToString("#0.0"));
        }
    }
}
