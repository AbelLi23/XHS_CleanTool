using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CleanProApp
{
    using AppRes = CleanProApp.Properties.Resources;
    public partial class FormRoot : Form
    {
        //Create a new global instance
        public static Xprinter Printer;
        public int UnitSize = 30, GirdSize = 2;
        public static int P_ArrRow = 4, P_ArrCol = 4;
        public enum UIPages { StartPage, FirstStep, SecondStep, ThirdStep, FourthStep }
        public enum CleanMode { W1_C_PP, W1_C_P, W1C_PP, W1C_P, W0C_PP }//W1表示刮片能动, PP表示用小车来刮

        public FormRoot()
        {
            this.Icon = AppRes.XHS;
            this.MaximizeBox = false;
            this.ImeMode = ImeMode.Alpha;
            //this.DoubleBuffered = true;
            //this.Font = new Font("微软雅黑", 9);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            //Create a new global instance
            Printer = new Xprinter();
            Printer.IconRes = new List<Icon>(new Icon[] { AppRes.Painter1, AppRes.Wiper1, AppRes.Stage1, AppRes.PumpM1, AppRes.PumpS1, AppRes.Delay1 });
            Printer.ImageList = new List<Image>(new Image[] { AppRes.Painter, AppRes.Wiper, AppRes.Stage, AppRes.PumpM, AppRes.PumpS, AppRes.DelayImg });
        }

        private void FormRoot_Load(object sender, EventArgs e)
        {
            RenderUI(UIPages.StartPage);
            page_ShowStep(UIPages.StartPage);

            //Start Page
            btn_NewFile.Size = btn_Modify.Size = new Size(this.ClientSize.Width / 2, this.ClientSize.Height - statusStrip.Height);
            btn_NewFile.Location = new Point(0, 0);
            btn_Modify.Location = new Point(btn_NewFile.Location.X + btn_NewFile.Width, btn_NewFile.Location.Y);

            //First groupBox
            gBox_First.Size = new Size(this.ClientSize.Width - 2 * gBox_First.Location.X, this.ClientSize.Height - 2 * gBox_First.Location.Y - statusStrip.Height);
            var remainWidth = gBox_First.Width - pnl_mode.Width;
            var remainHeight = gBox_First.Height - 2 * pnl_mode.Height - pnl_mode.Location.Y - btn_FirstBack.Height;
            pnl_mode.Location = new Point(remainWidth / 2, pnl_mode.Location.Y + remainHeight / 2);
            pnl_direct.Location = new Point(pnl_mode.Location.X, pnl_mode.Location.Y + pnl_mode.Height);
            btn_FirstBack.Location = new Point(gBox_First.Location.X, gBox_First.Height - btn_FirstBack.Height);
            btn_FirstNext.Location = new Point(gBox_First.Width - btn_FirstNext.Width - gBox_First.Location.X, btn_FirstBack.Location.Y);

            //Second groupBox
            gBox_Second.Size = gBox_First.Size; gBox_Second.Location = gBox_First.Location;
            pnl_ptype.Location = new Point(pnl_mode.Location.X, pnl_ptype.Location.Y);
            label_arrTip.Location = new Point(pnl_ptype.Location.X + (pnl_ptype.Width - label_arrTip.Width) / 2, pnl_ptype.Location.Y + pnl_ptype.Height);
            pnl_P_Array.Size = new Size(UnitSize * 11 + GirdSize * 10, UnitSize * 11 + GirdSize * 10);//MAX:9x9
            remainWidth = gBox_Second.Width - pnl_P_Array.Width;
            remainHeight = gBox_Second.Height - pnl_ptype.Height - pnl_ptype.Location.Y - label_arrTip.Height - pnl_P_Array.Height - btn_SecondBack.Height;
            pnl_P_Array.Location = new Point(remainWidth / 2, label_arrTip.Location.Y + remainHeight / 2);
            btn_SecondBack.Location = btn_FirstBack.Location;
            btn_SecondNext.Location = btn_FirstNext.Location;

            //Third groupBox
            gBox_Third.Size = gBox_First.Size; gBox_Third.Location = gBox_First.Location;
            remainHeight = gBox_Third.Height - pnl_AllPara.Height - pnl_AllPara.Location.Y - btn_ThirdBack.Height;
            pnl_AllPara.Location = new Point(pnl_mode.Location.X, pnl_AllPara.Location.Y + remainHeight / 2);
            //label_Split.Size = new Size(pnl_AllPara.Width, GirdSize);
            //label_Split.Text = ""; label_Split.BackColor = Color.Blue;
            //label_Split.Location = pnl_Para2.Location;
            //Graphics gg = gBox_Third.CreateGraphics();
            //gg.DrawLine(new Pen(Color.DarkOrange), new Point(gBox_Third.Location.X + pnl_AllPara.Location.X, gBox_Third.Location.Y + pnl_AllPara.Location.Y + pnl_Para2.Location.Y),
            //    new Point(gBox_Third.Location.X + pnl_AllPara.Location.X + pnl_AllPara.Width, gBox_Third.Location.Y + pnl_AllPara.Location.Y + pnl_Para2.Location.Y));
            btn_ThirdBack.Location = btn_FirstBack.Location;
            btn_ThirdNext.Location = btn_FirstNext.Location;

            //Fourth groupBox
            gBox_Fourth.Size = gBox_First.Size; gBox_Fourth.Location = gBox_First.Location;
            remainWidth = gBox_Fourth.Width - 2 * pnl_Actions.Width - 4 * pnl_Actions.Location.X;
            remainHeight = gBox_Fourth.Height - btn_FourthBack.Height - pnl_Actions.Location.Y - pnl_Actions.Location.X;
            listView_CleanSteps.Size = new Size(remainWidth, remainHeight);
            listView_CleanSteps.Location = new Point(pnl_Actions.Width + 2 * pnl_Actions.Location.X, pnl_Actions.Location.Y);
            pnl_EditStep.Location = new Point(listView_CleanSteps.Location.X + listView_CleanSteps.Width + pnl_Actions.Location.X, pnl_Actions.Location.Y);
            pnl_SerialNum.Location = new Point(btn_FirstNext.Location.X - pnl_SerialNum.Width, btn_FirstNext.Location.Y);
            btn_FourthBack.Location = btn_FirstBack.Location;
            btn_SaveProFile.Location = btn_FirstNext.Location;
        }

        #region Start Page callBack (&function)
        private void page_ShowStep(UIPages page)
        {
            switch (page)
            {
                case UIPages.StartPage:
                    btn_NewFile.Visible = btn_Modify.Visible = true;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.FirstStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    gBox_First.Visible = true;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.SecondStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = true;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.ThirdStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = true;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.FourthStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = true;
                    break;
            }
        }
        private void btn_StartPageClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (btn_Modify == button)
            {
                //Analyze an exiting file
                openProFileDialog.InitialDirectory = Printer.F_defaultPath;
                openProFileDialog.FileName = "";
                openProFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openProFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Printer.F_ProFile = openProFileDialog.FileName;
                    if (Printer.F_AnalyzeProcess(Printer.F_ReadProcess(Printer.F_ProFile))) Printer.IsInModifying = true;
                    else
                    {
                        MessageBox.Show("不是有效的清洗流程文件!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                    }
                }
                else return;
            }
            //Initialize UI parameters
            RenderUI(UIPages.FirstStep);
            page_ShowStep(UIPages.FirstStep);
        }
        #endregion

        #region First groupBox callBack (&function)
        private void rBtn_W_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            if (rBtn_W_Enable == radioBtn)
            {
                Printer.W_Enable = pnl_direct.Visible = radioBtn.Checked;
                Printer.WC_Enable = !radioBtn.Checked;
            }
            else if (rBtn_WC_Enable == radioBtn)
            {
                Printer.WC_Enable = pnl_direct.Visible = radioBtn.Checked;
                Printer.W_Enable = !radioBtn.Checked;
            }
            else if (rBtn_W_Disable == radioBtn)
            {
                Printer.W_Enable = Printer.WC_Enable = !radioBtn.Checked;
            }
            else if (rBtn_W_InPainter == radioBtn)
            {
                Printer.WipeInXdirect = radioBtn.Checked;
            }
            else if (rBtn_W_InWiper == radioBtn)
            {
                Printer.WipeInXdirect = !radioBtn.Checked;
            }

            if (!Printer.W_Enable && !Printer.WC_Enable) Printer.CleanMode = CleanMode.W0C_PP;
            else if (Printer.W_Enable && Printer.WipeInXdirect) Printer.CleanMode = CleanMode.W1_C_PP;
            else if (Printer.W_Enable && !Printer.WipeInXdirect) Printer.CleanMode = CleanMode.W1_C_P;
            else if (Printer.WC_Enable && Printer.WipeInXdirect) Printer.CleanMode = CleanMode.W1C_PP;
            else if (Printer.WC_Enable && !Printer.WipeInXdirect) Printer.CleanMode = CleanMode.W1C_P;
        }
        private void btn_FirstBack_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "btn_FirstBack":
                    GetNewData(UIPages.FirstStep);
                    RenderUI(UIPages.StartPage);
                    page_ShowStep(UIPages.StartPage);
                    break;
                case "btn_SecondBack":
                    GetNewData(UIPages.SecondStep);
                    RenderUI(UIPages.FirstStep);
                    page_ShowStep(UIPages.FirstStep);
                    break;
                case "btn_ThirdBack":
                    GetNewData(UIPages.ThirdStep);
                    RenderUI(UIPages.SecondStep);
                    page_ShowStep(UIPages.SecondStep);
                    break;
                case "btn_FourthBack":
                    if (DialogResult.Yes == MessageBox.Show("返回将不会保存当前所有修改动作,是否继续?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        GetNewData(UIPages.FourthStep);
                        RenderUI(UIPages.ThirdStep);
                        page_ShowStep(UIPages.ThirdStep);
                    }
                    else return;
                    break;
            }
        }
        private void btn_FirstNext_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "btn_FirstNext":
                    GetNewData(UIPages.FirstStep);
                    RenderUI(UIPages.SecondStep);
                    page_ShowStep(UIPages.SecondStep);
                    break;
                case "btn_SecondNext":
                    GetNewData(UIPages.SecondStep);
                    if (Printer.P_Type == " ")
                    {
                        MessageBox.Show("请选择喷头类型!", "仔细点啦,兄dei...", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    RenderUI(UIPages.ThirdStep);
                    page_ShowStep(UIPages.ThirdStep);
                    break;
                case "btn_ThirdNext":
                    GetNewData(UIPages.ThirdStep);
                    RenderUI(UIPages.FourthStep);
                    page_ShowStep(UIPages.FourthStep);
                    break;
                case "btn_SaveProFile":
                    SaveProcessFile();
                    break;
            }
        }
        #endregion

        #region Second groupBox callBack (&function)
        private void draw_GirdLine()
        {
            if (P_ArrRow != 0 && P_ArrCol != 0)
            {
                var XGirdSize = new Size(UnitSize * (P_ArrCol + 2) + GirdSize * (P_ArrCol + 1), GirdSize);
                var YGirdSize = new Size(GirdSize, UnitSize * (P_ArrRow + 2) + GirdSize * (P_ArrRow + 1));
                for (int r = 0; r < P_ArrRow + 1; r++)
                {
                    Label xGird = new Label();
                    xGird.BackColor = label_arrTip.ForeColor;
                    xGird.Size = XGirdSize;
                    xGird.Location = new Point(0, (r + 1) * UnitSize + r * GirdSize);
                    pnl_P_Array.Controls.Add(xGird);
                    if (r > 0)
                    {
                        Label xHead = new Label();
                        xHead.Text = (Printer.W_Enable || Printer.WC_Enable) ? ("[P" + r.ToString() + "]") : ("[0" + r.ToString() + "]");
                        xHead.Font = label_arrTip.Font;
                        xHead.TextAlign = ContentAlignment.MiddleCenter;
                        xHead.Size = new Size(UnitSize, UnitSize);
                        xHead.Location = new Point(0, r * (UnitSize + GirdSize));
                        pnl_P_Array.Controls.Add(xHead);
                    }
                }
                for (int c = 0; c < P_ArrCol + 1; c++)
                {
                    Label yGird = new Label();
                    yGird.BackColor = label_arrTip.ForeColor;
                    yGird.Size = YGirdSize;
                    yGird.Location = new Point((c + 1) * UnitSize + c * GirdSize, 0);
                    pnl_P_Array.Controls.Add(yGird);
                    if (c > 0)
                    {
                        Label yHead = new Label();
                        yHead.Text = (Printer.W_Enable || Printer.WC_Enable) ? ("[0" + c.ToString() + "]") : ("[" + (char)('A' + (c - 1)) + "]");
                        yHead.Font = label_arrTip.Font;
                        yHead.TextAlign = ContentAlignment.MiddleCenter;
                        yHead.Size = new Size(UnitSize, UnitSize);
                        yHead.Location = new Point(c * (UnitSize + GirdSize), 0);
                        pnl_P_Array.Controls.Add(yHead);
                    }
                }
            }
        }
        private void draw_PainterArr()
        {
            if (P_ArrRow != 0 && P_ArrCol != 0)
            {
                for (int r = 0; r < P_ArrRow; r++)
                {
                    for (int c = 0; c < P_ArrCol; c++)
                    {
                        Label label_P = new Label();
                        label_P.MouseClick += label_P_MouseClick;
                        pnl_P_Array.Controls.Add(label_P);
                        label_P.BackColor = Color.LightSteelBlue;
                        label_P.Font = label_arrTip.Font;
                        label_P.TextAlign = ContentAlignment.MiddleCenter;
                        label_P.Size = new Size(UnitSize, UnitSize);
                        label_P.Location = new Point((c + 1) * (UnitSize + GirdSize), (r + 1) * (UnitSize + GirdSize));
                        var realRow = (r + 1).ToString(); var realCol = (c + 1).ToString();
                        label_P.Name = "label_P" + realRow + realCol;
                        Printer.P_Array.Add(label_P);
                    }
                }
            }
        }
        public int Num_Used = 1; public string Num_NotUsed = "";
        //public List<int> Num_NotUsed = new List<int>();
        private void label_P_MouseClick(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (!string.IsNullOrEmpty(label.Text)) return;
                if (!string.IsNullOrEmpty(Num_NotUsed))
                {
                    label.Text = Num_NotUsed;
                    Num_NotUsed = "";
                }
                else
                {
                    label.Text = Num_Used.ToString() + '#';
                    Num_Used++;
                }
                label.BackColor = Color.LightSeaGreen;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (string.IsNullOrEmpty(label.Text)) return;
                if (!string.IsNullOrEmpty(Num_NotUsed)) return;
                Num_NotUsed = label.Text;
                label.Text = "";
                label.BackColor = Color.LightSteelBlue;
            }
            else
                return;
        }
        private void comBox_P_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Printer.P_Type = (comBox_P_Type.SelectedItem != null) ? comBox_P_Type.SelectedItem.ToString() : " ";
        }
        private void comBox_P_SizeCchanged(object sender, EventArgs e)
        {
            //第一次绘制pnl_P_Array是由选择行列的事件的自动触发完成;
            Num_Used = 1; Num_NotUsed = "";
            Printer.P_Array.Clear();
            pnl_P_Array.Controls.Clear();

            P_ArrRow = comBox_P_Row.SelectedIndex + 1;
            P_ArrCol = comBox_P_Col.SelectedIndex + 1;
            draw_GirdLine(); draw_PainterArr();
        }
        #endregion

        #region Third groupBox callBack (&function)
        private void rBtn_Para1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            if (rBtn_M_Set == radioBtn)
            {
                chkBox_HoldTime.Visible = (radioBtn.Checked) ? true : false;
                label_Intensity.Text = (radioBtn.Checked) ? (btn_PumpM.Text + Printer.ParaLabel[0]) : Printer.ParaLabel[0];
                label_HoldTime.Text = (radioBtn.Checked) ? (btn_PumpM.Text + Printer.ParaLabel[1]) : Printer.ParaLabel[1];
                label_WaitTime.Text = (radioBtn.Checked) ? (btn_PumpM.Text + Printer.ParaLabel[2]) : Printer.ParaLabel[2];
                label_CycleNum.Text = (radioBtn.Checked) ? (btn_PumpM.Text + Printer.ParaLabel[3]) : Printer.ParaLabel[3];

                if (chkBox_HoldTime.Checked)
                    this.chkBox_HoldTime_CheckedChanged(null, null);
                else
                {
                    trkBar_HoldTime.Maximum = 1000; trkBar_HoldTime.Minimum = 0; trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                    trkBar_HoldTime.Value = Printer.M_WorkTime;
                    label_HoldTimeV.Text = (0.1 * trkBar_HoldTime.Value).ToString("#0.0") + "秒";
                }
                trkBar_Intensity.Maximum = 7; trkBar_Intensity.Minimum = 1; trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
                //trkBar_HoldTime.Maximum = 1000; trkBar_HoldTime.Minimum = 0; trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_WaitTime.Maximum = 1000; trkBar_WaitTime.Minimum = 0; trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
                trkBar_CycleNum.Maximum = 100; trkBar_CycleNum.Minimum = 1; trkBar_CycleNum.SmallChange = trkBar_Intensity.TickFrequency = 1;

                trkBar_Intensity.Value = Printer.M_Strength;
                //trkBar_HoldTime.Value = Printer.M_WorkTime;
                trkBar_WaitTime.Value = Printer.M_HoldTime;
                trkBar_CycleNum.Value = Printer.M_CycleNum;

                label_IntensityV.Text = trkBar_Intensity.Value.ToString() + "级";
                //label_HoldTimeV.Text = (0.1 * trkBar_HoldTime.Value).ToString("#0.0") + "秒";
                label_WaitTimeV.Text = (0.1 * trkBar_WaitTime.Value).ToString("#0.0") + "秒";
                label_CycleNumV.Text = trkBar_CycleNum.Value.ToString() + "次";
            }
            else if (rBtn_V_Set == radioBtn)
            {
                label_Intensity.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[0]) : Printer.ParaLabel[0];
                label_HoldTime.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[1]) : Printer.ParaLabel[1];
                label_WaitTime.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[2]) : Printer.ParaLabel[2];
                label_CycleNum.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[3]) : Printer.ParaLabel[3];

                trkBar_Intensity.Enabled = true;
                trkBar_WaitTime.Enabled = true;
                trkBar_CycleNum.Enabled = true;

                trkBar_Intensity.Maximum = 9; trkBar_Intensity.Minimum = 1; trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
                trkBar_HoldTime.Maximum = 1000; trkBar_HoldTime.Minimum = 0; trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_WaitTime.Maximum = 1000; trkBar_WaitTime.Minimum = 0; trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
                trkBar_CycleNum.Maximum = 100; trkBar_CycleNum.Minimum = 1; trkBar_CycleNum.SmallChange = trkBar_Intensity.TickFrequency = 1;

                trkBar_Intensity.Value = Printer.V_Strength;
                trkBar_HoldTime.Value = Printer.V_WorkTime;
                trkBar_WaitTime.Value = Printer.V_HoldTime;
                trkBar_CycleNum.Value = Printer.V_CycleNum;

                label_IntensityV.Text = trkBar_Intensity.Value.ToString() + "级";
                label_HoldTimeV.Text = (0.1 * trkBar_HoldTime.Value).ToString("#0.0") + "秒";
                label_WaitTimeV.Text = (0.1 * trkBar_WaitTime.Value).ToString("#0.0") + "秒";
                label_CycleNumV.Text = trkBar_CycleNum.Value.ToString() + "次";
            }
            else if (rBtn_N_Set == radioBtn)
            {
                label_Intensity.Visible = (radioBtn.Checked) ? false : true;
                label_HoldTime.Text = (radioBtn.Checked) ? Printer.ParaLabel[4] : Printer.ParaLabel[1];
                label_WaitTime.Visible = (radioBtn.Checked) ? false : true;
                label_CycleNum.Visible = (radioBtn.Checked) ? false : true;

                trkBar_Intensity.Visible = (radioBtn.Checked) ? false : true;
                trkBar_HoldTime.Maximum = 1000; trkBar_HoldTime.Minimum = 1; trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_WaitTime.Visible = (radioBtn.Checked) ? false : true;
                trkBar_CycleNum.Visible = (radioBtn.Checked) ? false : true;

                trkBar_HoldTime.Value = Printer.N_WaitTime;

                label_IntensityV.Visible = (radioBtn.Checked) ? false : true;
                label_HoldTimeV.Text = trkBar_HoldTime.Value.ToString() + "秒";
                label_WaitTimeV.Visible = (radioBtn.Checked) ? false : true;
                label_CycleNumV.Visible = (radioBtn.Checked) ? false : true;
            }
        }
        private void chkBox_HoldTime_CheckedChanged(object sender, EventArgs e)
        {
            trkBar_Intensity.Enabled = (chkBox_HoldTime.Checked) ? false : true;
            if (chkBox_HoldTime.Checked)
            {
                Printer.M_OnlyTime = true;
                trkBar_HoldTime.Maximum = 1000; trkBar_HoldTime.Minimum = 0; trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_HoldTime.Value = Printer.M_OnlyWorkTime;
                label_HoldTimeV.Text = trkBar_HoldTime.Value.ToString() + "秒";
            }
            else
            {
                Printer.M_OnlyTime = false;
                trkBar_HoldTime.Maximum = 1000; trkBar_HoldTime.Minimum = 0; trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_HoldTime.Value = Printer.M_WorkTime;
                label_HoldTimeV.Text = (0.1 * trkBar_HoldTime.Value).ToString("#0.0") + "秒";
            }
            trkBar_WaitTime.Enabled = (chkBox_HoldTime.Checked) ? false : true;
            trkBar_CycleNum.Enabled = (chkBox_HoldTime.Checked) ? false : true;
        }
        private void trkBar_Para1_ValueChanged(object sender, EventArgs e)
        {
            if (rBtn_N_Set.Checked)
            {
                label_HoldTimeV.Text = trkBar_HoldTime.Value.ToString() + "秒";
            }
            else if (rBtn_M_Set.Checked && chkBox_HoldTime.Checked)
            {
                label_HoldTimeV.Text = trkBar_HoldTime.Value.ToString() + "秒";
            }
            else
            {
                label_IntensityV.Text = trkBar_Intensity.Value.ToString() + "级";
                label_HoldTimeV.Text = (0.1 * trkBar_HoldTime.Value).ToString("#0.0") + "秒";
                label_WaitTimeV.Text = (0.1 * trkBar_WaitTime.Value).ToString("#0.0") + "秒";
                label_CycleNumV.Text = trkBar_CycleNum.Value.ToString() + "次";
            }
        }
        private void btn_SetPumpPara_Click(object sender, EventArgs e)
        {
            //更新数据的主体
            if (rBtn_M_Set.Checked)
            {
                if (chkBox_HoldTime.Checked)
                {
                    Printer.M_OnlyWorkTime = trkBar_HoldTime.Value;
                }
                else
                {
                    Printer.M_Strength = trkBar_Intensity.Value;
                    Printer.M_WorkTime = trkBar_HoldTime.Value;
                    Printer.M_HoldTime = trkBar_WaitTime.Value;
                    Printer.M_CycleNum = trkBar_CycleNum.Value;
                }
            }
            else if (rBtn_V_Set.Checked)
            {
                Printer.V_Strength = trkBar_Intensity.Value;
                Printer.V_WorkTime = trkBar_HoldTime.Value;
                Printer.V_HoldTime = trkBar_WaitTime.Value;
                Printer.V_CycleNum = trkBar_CycleNum.Value;
            }
            else if (rBtn_N_Set.Checked)
            {
                Printer.N_WaitTime = trkBar_HoldTime.Value;
            }
        }
        private void rBtn_Para2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            string Vel = "速度", Pos = "位置";
            if (rBtn_C_Set == radioBtn)
            {
                label_Velocity.Text = (radioBtn.Checked) ? (btn_Stage.Text + Vel) : Vel;
                label_Position.Text = (radioBtn.Checked) ? (btn_Stage.Text + Pos) : Pos;

                trkBar_Velocity.Maximum = 100; trkBar_Velocity.Minimum = 0; trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
                trkBar_Velocity.Value = Printer.C_Speed;
                label_VelocityV.Text = (0.1 * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";

                List<string> items = new List<string>(Printer.C_LevelMark);
                //Printer.C_Level = new List<int>(items.Count);
                comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
            }
            else if (rBtn_P_Set == radioBtn)
            {
                label_Velocity.Text = (radioBtn.Checked) ? (btn_Painter.Text + Vel) : Vel;
                label_Position.Text = (radioBtn.Checked) ? (btn_Painter.Text + Pos) : Pos;

                trkBar_Velocity.Maximum = 100; trkBar_Velocity.Minimum = 0; trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
                trkBar_Velocity.Value = Printer.P_Speed;
                label_VelocityV.Text = (0.1 * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";

                List<string> items = new List<string>();
                //Printer.P_XPos = new List<int>(items.Count);
                if (CleanMode.W1_C_PP == Printer.CleanMode || CleanMode.W0C_PP == Printer.CleanMode || CleanMode.W1C_PP == Printer.CleanMode)
                {
                    for (int pn = 0; pn < Printer.P_Counts; pn++)
                    {
                        items.Add((pn + 1).ToString() + "号喷头刮墨前");
                        items.Add((pn + 1).ToString() + "号喷头刮墨后");
                    }
                }
                else
                {
                    for (int pn = 0; pn < Printer.P_Counts; pn++)
                    {
                        items.Add((pn + 1).ToString() + "号喷头刮墨");
                    }
                }
                comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
            }
            else if (rBtn_W_Set == radioBtn)
            {
                label_Velocity.Text = (radioBtn.Checked) ? (btn_Wiper.Text + Vel) : Vel;
                label_Position.Text = (radioBtn.Checked) ? (btn_Wiper.Text + Pos) : Pos;

                trkBar_Velocity.Maximum = 100; trkBar_Velocity.Minimum = 0; trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
                trkBar_Velocity.Value = Printer.W_Speed;
                label_VelocityV.Text = (0.1 * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";

                List<string> items = new List<string>();
                //Printer.W_YPos = new List<int>(items.Count); 刮片只考虑一个位置
                for (int wn = 0; wn < Printer.P_Counts; wn++)
                {
                    items.Add((wn + 1).ToString() + "号喷头刮墨");
                }
                comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
            }
            txtBox_Position.Text = "";
            List<string> items2 = new List<string>();
            for (int i = 1; i < 100 + 1; i++) items2.Add(i.ToString());
            comBox_microPos.DataSource = items2; comBox_microPos.SelectedIndex = -1;
        }
        private void trkBar_Para2_ValueChanged(object sender, EventArgs e)
        {
            if (rBtn_C_Set.Checked)
            {
                label_VelocityV.Text = (0.1 * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";
            }
            else if (rBtn_P_Set.Checked)
            {
                label_VelocityV.Text = (0.1 * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";
            }
            else if (rBtn_W_Set.Checked)
            {
                label_VelocityV.Text = (0.1 * trkBar_Velocity.Value).ToString("#0.0") + "(m/s)";
            }
        }
        private void comBox_Position_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBtn_C_Set.Checked)
            {
                if (comBox_Position.SelectedItem != null)
                {
                    if (Printer.C_Level.Count != 0)
                        txtBox_Position.Text = Printer.C_Level[comBox_Position.SelectedIndex].ToString();
                }

            }
            else if (rBtn_P_Set.Checked)
            {
                if (comBox_Position.SelectedItem != null)
                {
                    if (Printer.P_XPos.Count != 0)
                        txtBox_Position.Text = Printer.P_XPos[comBox_Position.SelectedIndex].ToString();
                }
            }
            else if (rBtn_W_Set.Checked)
            {
                if (comBox_Position.SelectedItem != null)
                {
                    if (Printer.W_YPos.Count != 0)
                        txtBox_Position.Text = Printer.W_YPos[comBox_Position.SelectedIndex].ToString();
                }
            }
        }
        private void btn_SetAxisPara_Click(object sender, EventArgs e)
        {
            //更新数据的主体
            if (rBtn_C_Set.Checked)
            {
                Printer.C_Speed = trkBar_Velocity.Value;
                bool rst = false; int tmpVal = 0;
                if (comBox_Position.SelectedItem != null) { rst = int.TryParse(txtBox_Position.Text, out tmpVal); }
                if (comBox_Position.SelectedIndex != -1) Printer.C_Level[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
            }
            else if (rBtn_P_Set.Checked)
            {
                Printer.P_Speed = trkBar_Velocity.Value;
                bool rst = false; int tmpVal = 0;
                if (comBox_Position.SelectedItem != null) { rst = int.TryParse(txtBox_Position.Text, out tmpVal); }
                if (comBox_Position.SelectedIndex != -1) Printer.P_XPos[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
            }
            else if (rBtn_W_Set.Checked)
            {
                Printer.W_Speed = trkBar_Velocity.Value;
                bool rst = false; int tmpVal = 0;
                if (comBox_Position.SelectedItem != null) { rst = int.TryParse(txtBox_Position.Text, out tmpVal); }
                if (comBox_Position.SelectedIndex != -1) Printer.W_YPos[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
            }
        }
        #endregion

        #region Fourth groupBox callBack (&function)
        private string AddSingleStep(string stepTag)
        {
            string Head = @"@", Tail = @";";
            string SingleStep = "";
            switch (stepTag.Substring(0, 1))
            {
                case "P":
                    if ("p" == stepTag.Substring(1, 1))
                    {
                        SingleStep = stepTag + Printer.P_Speed.ToString();
                    }
                    else if ("0" == stepTag.Substring(1, 1))
                    {
                        SingleStep = stepTag + "0";
                    }
                    else
                    {
                        var pn = int.Parse(stepTag.Substring(1, 1));
                        if (CleanMode.W1_C_PP == Printer.CleanMode || CleanMode.W0C_PP == Printer.CleanMode || CleanMode.W1C_PP == Printer.CleanMode)
                        {
                            if (1 == int.Parse(stepTag.Substring(2, 1)))
                            {
                                SingleStep = stepTag.Substring(0, 2) + Printer.P_XPos[2 * pn - 2].ToString();
                            }
                            else if (2 == int.Parse(stepTag.Substring(2, 1)))
                            {
                                SingleStep = stepTag.Substring(0, 2) + Printer.P_XPos[2 * pn - 1].ToString();
                            }
                        }
                        else
                        {
                            SingleStep = stepTag + Printer.P_XPos[pn - 1].ToString();
                        }
                    }
                    break;
                case "W":
                    SingleStep = stepTag;
                    break;
                case "C":
                    SingleStep = stepTag;
                    break;
                case "M":
                    if ("0" == stepTag.Substring(1, 1))
                    {
                        SingleStep = stepTag + "999";//Printer.M_OnlyWorkTime.ToString();
                    }
                    else if ("1" == stepTag.Substring(1, 1))
                    {
                        SingleStep = stepTag + "x" + Printer.M_Strength.ToString() + "p" + Printer.M_WorkTime.ToString() + "s"
                            + Printer.M_HoldTime.ToString() + "n" + "0";//Printer.M_CycleNum.ToString()
                    }
                    break;
                case "V":
                    SingleStep = stepTag + Printer.V_Strength.ToString() + "p" + Printer.V_WorkTime.ToString() + "s"
                        + Printer.V_HoldTime.ToString() + "n" + "0";
                    break;
                case "N":
                    SingleStep = stepTag;
                    break;
            }
            return (Head + SingleStep + Tail);
        }
        private List<string> CreateProcessTemplate(CleanMode mode)
        {
            List<string> ProcessTemplate = new List<string>();
            // ① 设定小车速度
            ProcessTemplate.Add(AddSingleStep("Pp"));
            // ② 小车回到原点
            ProcessTemplate.Add((AddSingleStep("P0")));
            // ③ 墨栈到吸墨位
            ProcessTemplate.Add(AddSingleStep("Cn0"));
            // ④ 墨泵开始吸墨
            ProcessTemplate.Add(AddSingleStep("M1"));
            // ⑤ 增加保压延时
            ProcessTemplate.Add(AddSingleStep("Nr0"));
            // ⑥ 墨栈回到原点
            ProcessTemplate.Add(AddSingleStep("Cn3"));
            // ⑦ 墨泵吸走废墨
            ProcessTemplate.Add(AddSingleStep("M0"));
            switch (Printer.CleanMode)
            {
                case CleanMode.W1_C_PP:
                    // ⑧ 开始刮墨流程
                    for (int n = 0; n < Printer.CleanSequence.Count; n++)
                    {
                        // 小车到喷头刮墨前位置
                        var tmpH = "P" + Printer.CleanSequence[n].ToString() + "1";
                        ProcessTemplate.Add(AddSingleStep(tmpH));
                        // 刮片到指定喷头刮墨位
                        var wn = "Wn" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(wn));
                        // 小车到喷头刮墨后位置
                        var tmpT = "P" + Printer.CleanSequence[n].ToString() + "2";
                        ProcessTemplate.Add(AddSingleStep(tmpT));
                        // 刮片再次回到原点位置
                        ProcessTemplate.Add(AddSingleStep("Wn0"));
                    }
                    break;
                case CleanMode.W0C_PP:
                    for (int n = 0; n < Printer.CleanSequence.Count; n++)
                    {
                        // 小车到喷头刮墨前位置
                        var tmpH = "P" + Printer.CleanSequence[n].ToString() + "1";
                        ProcessTemplate.Add(AddSingleStep(tmpH));
                        // 墨栈到刮墨位置
                        var cnU = string.Format("C{0}1", Printer.CleanSequence[n]);
                        ProcessTemplate.Add(AddSingleStep(cnU));
                        // 小车到喷头刮墨后位置
                        var tmpT = "P" + Printer.CleanSequence[n].ToString() + "2";
                        ProcessTemplate.Add(AddSingleStep(tmpT));
                        // 墨栈到原点位置
                        var cnD = string.Format("C{0}3", Printer.CleanSequence[n]);
                        ProcessTemplate.Add(AddSingleStep(cnD));
                    }
                    break;
                case CleanMode.W1_C_P:
                    for (int n = 0; n < Printer.CleanSequence.Count; n++)
                    {
                        // 刮片到达刮墨位置
                        var wn = "Wn" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(wn));
                        // 小车到达刮墨位置
                        var pn = "P" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(pn));
                        // 刮片回到原点
                        ProcessTemplate.Add(AddSingleStep("Wn0"));
                    }
                    break;
                case CleanMode.W1C_P:
                    for (int n = 0; n < Printer.CleanSequence.Count; n++)
                    {
                        // 刮片到达刮墨位置
                        var wn = "Wn" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(wn));
                        // 小车到达刮墨位置 
                        var pn = "P" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(pn));
                        // 墨栈到刮墨位置
                        var cnU = string.Format("C{0}1", Printer.CleanSequence[n]);
                        ProcessTemplate.Add(AddSingleStep(cnU));
                        // 刮片回到原点
                        ProcessTemplate.Add(AddSingleStep("Wn0"));
                        // 墨栈回到原点
                        var cnD = string.Format("C{0}3", Printer.CleanSequence[n]);
                        ProcessTemplate.Add(AddSingleStep(cnD));
                    }
                    break;
                case CleanMode.W1C_PP:
                    for (int n = 0; n < Printer.CleanSequence.Count; n++)
                    {
                        // 小车到喷头刮墨前位置
                        var tmpH = "P" + Printer.CleanSequence[n].ToString() + "1";
                        ProcessTemplate.Add(AddSingleStep(tmpH));
                        // 刮片到达刮墨位置
                        var wn = "Wn" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(wn));
                        // 墨栈到刮墨位置
                        var cnU = string.Format("C{0}1", Printer.CleanSequence[n]);
                        ProcessTemplate.Add(AddSingleStep(cnU));
                        // 小车到喷头刮墨后位置
                        var tmpT = "P" + Printer.CleanSequence[n].ToString() + "2";
                        ProcessTemplate.Add(AddSingleStep(tmpT));
                        // 墨栈回到原点
                        var cnD = string.Format("C{0}3", Printer.CleanSequence[n]);
                        ProcessTemplate.Add(AddSingleStep(cnD));
                        // 刮片回到原点
                        ProcessTemplate.Add(AddSingleStep("Wn0"));
                    }
                    break;
            }
            // ⑨ 小车回到原点
            ProcessTemplate.Add((AddSingleStep("P0")));
            // ⑩ 墨栈到闪喷位
            ProcessTemplate.Add((AddSingleStep("Cn4")));
            // ⑩ 闪喷开始工作
            ProcessTemplate.Add((AddSingleStep("V")));
            // ⑩ 墨栈再回原点
            ProcessTemplate.Add(AddSingleStep("Cn3"));
            // ⑩ 再次吸走废墨
            ProcessTemplate.Add(AddSingleStep("M0"));
            return ProcessTemplate;
        }

        private void CreateActionList()
        {
            listView_CleanSteps.Columns.Clear();//清除列头
            listView_CleanSteps.Columns.Add("顺序", listView_CleanSteps.Width / 10, HorizontalAlignment.Center);
            listView_CleanSteps.Columns.Add("动作部件", listView_CleanSteps.Width / 9, HorizontalAlignment.Center);
            listView_CleanSteps.Columns.Add("动作描述", listView_CleanSteps.Width, HorizontalAlignment.Left);
            //列宽控制？
        }
        private void FlushActionList(List<string> acts)
        {
            CreateActionList();
            listView_CleanSteps.Items.Clear();
            var Begin = 1;//清除行重新填写
            listView_CleanSteps.BeginUpdate();
            listView_CleanSteps.SmallImageList = imageInSteps;
            foreach (string act in acts)
            {
                ListViewItem singleRow = new ListViewItem();
                if ("P" == act.Substring(1, 1))
                {
                    singleRow.Text = Begin.ToString(); Begin++;
                    singleRow.SubItems.Add(btn_Painter.Text);
                    singleRow.SubItems.Add(ExplainAction(act));
                    singleRow.ImageIndex = 1;
                }
                else if ("W" == act.Substring(1, 1))
                {
                    singleRow.Text = Begin.ToString(); Begin++;
                    singleRow.SubItems.Add(btn_Wiper.Text);
                    singleRow.SubItems.Add(ExplainAction(act));
                    singleRow.ImageIndex = 2;
                }
                else if ("C" == act.Substring(1, 1))
                {
                    singleRow.Text = Begin.ToString(); Begin++;
                    singleRow.SubItems.Add(btn_Stage.Text);
                    singleRow.SubItems.Add(ExplainAction(act));
                    singleRow.ImageIndex = 3;
                }
                else if ("M" == act.Substring(1, 1))
                {
                    singleRow.Text = Begin.ToString(); Begin++;
                    singleRow.SubItems.Add(btn_PumpM.Text);
                    singleRow.SubItems.Add(ExplainAction(act));
                    singleRow.ImageIndex = 4;
                }
                else if ("V" == act.Substring(1, 1))
                {
                    singleRow.Text = Begin.ToString(); Begin++;
                    singleRow.SubItems.Add(btn_PumpS.Text);
                    singleRow.SubItems.Add(ExplainAction(act));
                    singleRow.ImageIndex = 5;
                }
                else if ("N" == act.Substring(1, 1))
                {
                    singleRow.Text = Begin.ToString(); Begin++;
                    singleRow.SubItems.Add(btn_Delay.Text);
                    singleRow.SubItems.Add(ExplainAction(act));
                    singleRow.ImageIndex = 6;
                }
                else continue;
                listView_CleanSteps.Items.Add(singleRow);
            }
            listView_CleanSteps.EndUpdate();
        }
        private string ExplainAction(string act)
        {
            string description = act;
            act = act.Trim('@', ';');
            switch (act.Substring(0, 1))
            {
                case "P":
                    if ("p" == act.Substring(1, 1))
                    {
                        var Vel = 0; int.TryParse(act.Substring(2), out Vel);
                        if (Vel == 0) description = "小车速度设定为:【最慢】";
                        else description = string.Format("小车速度设定为:【{0}】", (0.1 * Vel).ToString("#0.0") + "(m/s)");
                    }
                    else if ("0" == act.Substring(1, 1))
                    {
                        description = "小车到达【原点位置】";
                    }
                    else
                    {
                        var pn = 0; int.TryParse(act.Substring(1, 1), out pn);
                        if (CleanMode.W1_C_PP == Printer.CleanMode || CleanMode.W0C_PP == Printer.CleanMode || CleanMode.W1C_PP == Printer.CleanMode)
                        {
                            if (int.Parse(act.Substring(2)) == Printer.P_XPos[2 * pn - 2])
                                description = string.Format("小车到达【{0}{1}位置】", pn, "号喷头刮墨前");
                            else if (int.Parse(act.Substring(2)) == Printer.P_XPos[2 * pn - 1])
                                description = string.Format("小车到达【{0}{1}位置】", pn, "号喷头刮墨后");
                        }
                        else if (CleanMode.W1_C_P == Printer.CleanMode || CleanMode.W1C_P == Printer.CleanMode)
                        {
                            description = string.Format("小车到达【{0}{1}位置】", pn, "号喷头刮墨");
                        }
                    }
                    break;
                case "W":
                    var wn = 0; int.TryParse(act.Substring(2), out wn);
                    if (wn == 0) description = "刮片到达【原点位置】";
                    else description = string.Format("刮片到达【{0}{1}位置】", wn, "号喷头刮墨");
                    break;
                case "C":
                    var cn = 3; int.TryParse(act.Substring(2), out cn);
                    description = string.Format("墨栈到达【{0}高度】", Printer.C_LevelMark[cn]);
                    break;
                case "M":
                    if ("M0999" == act) description = string.Format("墨泵吸墨【{0}秒】", Printer.M_OnlyWorkTime);
                    else
                    {
                        description = string.Format("墨泵工作【{0}级强度】【单次运转{1}秒】【单次停止{2}秒】【循环{3}次】",
                            Printer.M_Strength, Printer.M_WorkTime, Printer.M_HoldTime, Printer.M_CycleNum);
                    }
                    break;
                case "V":
                    description = string.Format("闪喷工作【{0}级强度】【单次闪喷{1}秒】【单次停止{2}秒】【循环{3}次】",
                            Printer.V_Strength, Printer.V_WorkTime, Printer.V_HoldTime, Printer.V_CycleNum);
                    break;
                case "N":
                    description = string.Format("延时【{0}秒】", Printer.N_WaitTime);
                    break;
            }
            return description;
        }
        private void InsertPreKSet()
        {
            //预先设定K值的部分
            Printer.CleanProcess.Insert(0, Printer.p_IndexStr);
            Printer.CleanProcess.Insert(0, Printer.w_IndexStr);
            Printer.CleanProcess.Insert(0, string.Format(@"@B{0}{1};", Printer.P_Type, Printer.ProcessName));
        }
        private void SaveProcessFile()
        {
            saveProFileDialog.InitialDirectory = Printer.F_defaultPath;
            saveProFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveProFileDialog.DefaultExt = ".txt";
            saveProFileDialog.FileName = "";
            if (saveProFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ProFile = saveProFileDialog.FileName;
                InsertPreKSet();//追加流程头
                Printer.CleanProcess.Add(@"@En0;");//追加流程尾
                Printer.F_SaveProcess(Printer.CleanProcess, ProFile);
            }
        }
        #endregion

        #region Rendering the User interface OR getting the data function
        private void RenderUI(UIPages page)
        {
            switch (page)
            {
                case UIPages.StartPage:
                    var DateNow = DateTime.Now.ToString("yyyy-MM-dd");
                    var WeekDay = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
                    toolStripStatusLabel1.Text = string.Format("今天是 {0} [ {1} ]", DateNow, WeekDay);
                    toolStripStatusLabel1.Alignment = ToolStripItemAlignment.Right;
                    break;
                case UIPages.FirstStep:
                    pnl_direct.Visible = Printer.W_Enable || Printer.WC_Enable;
                    rBtn_W_Enable.Checked = Printer.W_Enable;
                    rBtn_WC_Enable.Checked = Printer.WC_Enable;
                    rBtn_W_Disable.Checked = !Printer.W_Enable && !Printer.WC_Enable;
                    rBtn_W_InPainter.Checked = Printer.WipeInXdirect;
                    rBtn_W_InWiper.Checked = !Printer.WipeInXdirect;
                    break;
                case UIPages.SecondStep:
                    if (Printer.P_Type != " ")
                    {
                        char[] ptype = Printer.P_Type.ToCharArray();
                        comBox_P_Type.SelectedIndex = ptype[0] - 'A';
                    }
                    else comBox_P_Type.SelectedIndex = -1;

                    if (P_ArrRow != 0) comBox_P_Row.SelectedIndex = P_ArrRow - 1;
                    else comBox_P_Row.SelectedIndex = 3;

                    if (P_ArrCol != 0) comBox_P_Col.SelectedIndex = P_ArrCol - 1;
                    else comBox_P_Col.SelectedIndex = 3;

                    if (Printer.IsInModifying)
                    {
                        for (int n = 0; n < Printer.P_Counts; n++)
                        {
                            foreach (Label plabel in pnl_P_Array.Controls)
                            {
                                if (Printer.W_Enable || Printer.WC_Enable)
                                {
                                    if (!string.IsNullOrEmpty(Printer.w_IndexStr) && !string.IsNullOrEmpty(Printer.p_IndexStr))
                                    {
                                        var rowinfo = Printer.w_IndexStr.Trim('@', 'w', '0', ';');
                                        var colinfo = Printer.p_IndexStr.Trim('@', 'x', '0', ';');
                                        var name = string.Format("label_P{0}{1}", rowinfo.Substring(rowinfo.Length - n - 1, 1), colinfo.Substring(colinfo.Length - n - 1, 1));
                                        if (name == plabel.Name) { plabel.Text = string.Format("{0}#", n + 1); plabel.BackColor = Color.LightSeaGreen; }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Printer.w_IndexStr) && !string.IsNullOrEmpty(Printer.p_IndexStr))
                                    {
                                        var rowinfo = Printer.w_IndexStr.Trim('@', 'w', '0', ';');
                                        var colinfo = Printer.p_IndexStr.Trim('@', 'x', '0', ';');
                                        var name = string.Format("label_P{0}{1}", colinfo.Substring(colinfo.Length - n - 1, 1), rowinfo.Substring(rowinfo.Length - n - 1, 1));
                                        if (name == plabel.Name) { plabel.Text = string.Format("{0}#", n + 1); plabel.BackColor = Color.LightSeaGreen; }
                                    }
                                }
                            }
                        }
                        Num_Used = Printer.P_Counts + 1; Printer.IsInModifying = false;
                    }
                    break;
                case UIPages.ThirdStep:
                    rBtn_M_Set.Checked = true; rBtn_C_Set.Checked = true;
                    this.rBtn_Para1_CheckedChanged(rBtn_M_Set, null);
                    this.rBtn_Para2_CheckedChanged(rBtn_C_Set, null);
                    break;
                case UIPages.FourthStep:
                    txtBox_SerialNum.Text = Printer.ProcessName;
                    txtBox_SerialNum.AutoCompleteCustomSource.Add(Printer.ProcessName);
                    FlushActionList(Printer.CleanProcess);
                    break;
            }
        }
        private void GetNewData(UIPages page)
        {
            switch (page)
            {
                case UIPages.StartPage:
                    break;
                case UIPages.FirstStep:
                    Printer.W_Enable = rBtn_W_Enable.Checked;
                    Printer.WC_Enable = rBtn_WC_Enable.Checked;
                    Printer.WipeInXdirect = rBtn_W_InPainter.Checked;
                    break;
                case UIPages.SecondStep:
                    this.comBox_P_Type_SelectedIndexChanged(comBox_P_Type, null);
                    P_ArrRow = (comBox_P_Row.SelectedItem != null) ? comBox_P_Row.SelectedIndex + 1 : 4;
                    P_ArrCol = (comBox_P_Col.SelectedItem != null) ? comBox_P_Col.SelectedIndex + 1 : 4;
                    Printer.P_AnalyzeArray();
                    break;
                case UIPages.ThirdStep:
                    List<string> steps = CreateProcessTemplate(Printer.CleanMode);
                    Printer.CleanProcess = steps;//Not completely
                    break;
                case UIPages.FourthStep:
                    Printer.ProcessName = txtBox_SerialNum.Text;
                    break;
            }
        }
        #endregion

        private void FormRoot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("不玩了吗?", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

    }
}