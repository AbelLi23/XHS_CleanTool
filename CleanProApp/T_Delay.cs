using System.Windows.Forms;

namespace CleanProApp
{
    using AppCfg = CleanProApp.Properties.Settings;
    public partial class T_Delay : Form
    {
        public int stepId = 0;
        public string DelaySet = string.Empty;
        bool useK = false;
        public T_Delay(int index)
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[5];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            stepId = index; DelaySet = FormRoot.Printer.CleanProcess[index];
            FormRoot.ExplainAction(DelaySet);
            useK = (DelaySet.Substring(3, 1) == "0") ? true : false;

            trkBar_HoldTime.Maximum = AppCfg.Default.N_Dly_Max; trkBar_HoldTime.Minimum = AppCfg.Default.N_Dly_Min;
            trkBar_HoldTime.LargeChange = trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;

            label_HoldTime.Text = "延时时间";
            trkBar_HoldTime.Value = (useK) ? AppCfg.Default.N_Dly_K006 : FormRoot.Printer.N_WaitTime;
            label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.N_Dly_Rto).ToString());
        }

        private void QuitAdjustFrm(object sender, System.EventArgs e)
        {
            if ((Button)sender == btn_OK)
            {
                FormRoot.Printer.N_WaitTime = trkBar_HoldTime.Value;
                FormRoot.Printer.CleanProcess[stepId] = string.Format("@Nr{0}", FormRoot.Printer.N_WaitTime);
            }
            this.Close();
        }

        private void TrackBarValued(object sender, System.EventArgs e)
        {
            label_HoldTimeV.Text = string.Format("{0} 秒", (trkBar_HoldTime.Value * AppCfg.Default.N_Dly_Rto).ToString());
        }
    }
}
