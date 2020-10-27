using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class T_Delay : Form
    {
        public int stepId = 0;
        public string DelaySet = string.Empty;
        public T_Delay(int index)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[5];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            stepId = index; DelaySet = FormRoot.Printer.CleanProcess[index];

            trkBar_HoldTime.Maximum = AppCfg.Default.N_Dly_Max; trkBar_HoldTime.Minimum = AppCfg.Default.N_Dly_Min;
            trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;

            label_HoldTime.Text = "延时时间";
            trkBar_HoldTime.Value = AppCfg.Default.N_Dly_K06;
            label_HoldTimeV.Text = string.Format("{0}秒", (trkBar_HoldTime.Value * AppCfg.Default.N_Dly_Rto).ToString("#0.0"));
        }

        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if ((Button)sender == btn_OK)
            {
                AppCfg.Default.N_Dly_K06 = trkBar_HoldTime.Value;
            }
            this.Close();
        }

        private void TrackBarValued(object sender, System.EventArgs e)
        {
            label_HoldTimeV.Text = string.Format("{0}秒", (trkBar_HoldTime.Value * AppCfg.Default.N_Dly_Rto).ToString("#0.0"));
        }
    }
}
