﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CleanProApp
{
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using AppCfg = CleanProApp.Properties.Settings;
    using AppRes = CleanProApp.Properties.Resources;
    public partial class FormRoot : Form
    {
        //Create a new global instance
        public static Xprinter Printer;
        public static Socket CtrlSite_CMD, CtrlSite_Data;
        public int UnitSize = 30, GirdSize = 2;
        public static int P_ArrRow = 4, P_ArrCol = 4;
        //public static DialogResult PnlMoveRst = DialogResult.None;
        public enum CleanMode { W1_C_PP, W1_C_P, W1C_PP, W1C_P, W0C_PP }//W1表示刮片能动, PP表示用小车来刮
        public enum UIPages { StartPage, FirstStep, SecondStep, ThirdStep, FourthStep }
        //UDP session
        Socket cmdSKT = null, datSKT = null;
        IPEndPoint cmdEnd = null, datEnd = null;
        byte[] datMSG = new byte[1024 * 1024];
        byte[] cmdMSG = new byte[1024 * 1024];
        const byte SOH = 0x01, EOT = 0x04, ESC = 0x1B;
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
            AppCfg.Default.Reset();// Unnecessary
            //Create a new global instance
            Printer = new Xprinter();
            Printer.IconRes = new List<Icon>(new Icon[] { AppRes.Painter1, AppRes.Wiper1, AppRes.Stage1, AppRes.PumpM1, AppRes.PumpS1, AppRes.Delay1, AppRes.Update, AppRes.NetSet });
            Printer.ImageList = new List<Image>(new Image[] { AppRes.Painter, AppRes.Wiper, AppRes.Stage, AppRes.PumpM, AppRes.PumpS, AppRes.DelayImg });
            //Check and Diagnose Net Connect
            ConnectAndDiagnose();
        }
        /// <summary>
        /// Replace NameOf() method in C# 6.0 ★★★★★
        /// </summary>
        public static string NameOf<T>(Expression<Func<T>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }
        private void FormRoot_Load(object sender, EventArgs e)
        {
            RenderUI(UIPages.StartPage);
            page_ShowStep(UIPages.StartPage);

            //Start Page
            btn_NewFile.Size = btn_Modify.Size = new Size(this.ClientSize.Width / 2, (this.ClientSize.Height - statusStrip.Height) / 2);
            btn_NewDat.Size = btn_SetDat.Size = btn_NewFile.Size;
            btn_NewFile.Location = new Point(0, 0);
            btn_Modify.Location = new Point(btn_NewFile.Location.X + btn_NewFile.Width, btn_NewFile.Location.Y);
            btn_NewDat.Location = new Point(btn_NewFile.Location.X, btn_NewFile.Location.Y + btn_NewFile.Height);
            btn_SetDat.Location = new Point(btn_Modify.Location.X, btn_NewDat.Location.Y);

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
            var dist = 60;
            checkBox1.Location = new Point(pnl_ptype.Location.X + pnl_ptype.Width - checkBox1.Width, checkBox1.Location.Y + dist); Printer.M_PumpMs.Add(checkBox1);
            checkBox2.Location = new Point(checkBox1.Location.X, checkBox2.Location.Y + dist); Printer.M_PumpMs.Add(checkBox2);
            checkBox3.Location = new Point(checkBox1.Location.X, checkBox3.Location.Y + dist); Printer.M_PumpMs.Add(checkBox3);
            checkBox4.Location = new Point(checkBox1.Location.X, checkBox4.Location.Y + dist); Printer.M_PumpMs.Add(checkBox4);
            checkBox5.Location = new Point(checkBox1.Location.X, checkBox5.Location.Y + dist); Printer.M_PumpMs.Add(checkBox5);
            checkBox6.Location = new Point(checkBox1.Location.X, checkBox6.Location.Y + dist); Printer.M_PumpMs.Add(checkBox6);
            checkBox7.Location = new Point(checkBox1.Location.X, checkBox7.Location.Y + dist); Printer.M_PumpMs.Add(checkBox7);
            checkBox8.Location = new Point(checkBox1.Location.X, checkBox8.Location.Y + dist); Printer.M_PumpMs.Add(checkBox8);
            btn_SecondBack.Location = btn_FirstBack.Location;
            btn_SecondNext.Location = btn_FirstNext.Location;

            //Third groupBox
            gBox_Third.Size = gBox_First.Size; gBox_Third.Location = gBox_First.Location;
            remainHeight = gBox_Third.Height - pnl_AllPara.Height - pnl_AllPara.Location.Y - btn_ThirdBack.Height;
            pnl_AllPara.Location = new Point(pnl_mode.Location.X, pnl_AllPara.Location.Y + remainHeight / 2);
            pnl_Para1.Location = new Point(btn_FirstBack.Location.X, pnl_AllPara.Location.Y);
            pnl_Para2.Location = new Point(pnl_Para1.Location.X, pnl_Para1.Location.Y + pnl_AllPara.Height / 2);
            chkBox_WaveFreq.Location = chkBox_HoldTime.Location;
            btn_SetPumpPara.Location = new Point(btn_SetPumpPara.Location.X, 5);
            btn_SetAxisPara.Location = new Point(btn_SetPumpPara.Location.X, btn_SetPumpPara.Location.Y + pnl_AllPara.Height / 2);
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
            btn_Update.Location = new Point(pnl_EditStep.Location.X + btn_EditStetp.Location.X, listView_CleanSteps.Location.Y + listView_CleanSteps.Height - btn_Update.Height);
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
                    btn_NewDat.Visible = btn_SetDat.Visible = true;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.FirstStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    btn_NewDat.Visible = btn_SetDat.Visible = false;
                    gBox_First.Visible = true;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.SecondStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    btn_NewDat.Visible = btn_SetDat.Visible = false;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = true;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.ThirdStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    btn_NewDat.Visible = btn_SetDat.Visible = false;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = true;
                    gBox_Fourth.Visible = false;
                    break;
                case UIPages.FourthStep:
                    btn_NewFile.Visible = btn_Modify.Visible = false;
                    btn_NewDat.Visible = btn_SetDat.Visible = false;
                    gBox_First.Visible = false;
                    gBox_Second.Visible = false;
                    gBox_Third.Visible = false;
                    gBox_Fourth.Visible = true;
                    btn_FourthBack.Enabled = Printer.IsInModifying ? false : true;
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
                    Printer.CleanProcess = Printer.F_ReadProcess(Printer.F_ProFile);
                    if (Printer.F_AnalyzeProcess(Printer.CleanProcess)) Printer.IsInModifying = true;
                    else
                    {
                        MessageBox.Show("不是有效的清洗流程文件!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                    }
                    if (!Printer.OfflineUse)
                    {
                        EnDBG = SetDebugClean(true);
                        if (!EnDBG)
                        {
                            MessageBox.Show("网络通讯异常,无法设定调试状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        toolStripStatusLabel2.Text = string.Format("打印机当前状态:【{0}】", (EnDBG) ? "调试" : "在线");
                        toolStripStatusLabel2.BackColor = (EnDBG) ? Color.SkyBlue : Color.LightGreen;
                    }
                    RenderUI(UIPages.FourthStep);
                    page_ShowStep(UIPages.FourthStep);
                }
                else return;
            }
            else
            {
                //Initialize UI parameters
                RenderUI(UIPages.FirstStep);
                page_ShowStep(UIPages.FirstStep);
            }
        }
        private void Pak_Or_Update(object sender, EventArgs e)
        {
            ZR_Update update = null;
            if ((Button)sender == btn_NewDat)
            {
                update = new ZR_Update(true);
                update.ShowDialog();
            }
            else if (!Printer.OfflineUse)
            {
                update = new ZR_Update(false);
                update.ShowDialog();
            }
            else
            {
                MessageBox.Show("打印机当前为离线状态,无法上传!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //ZR_Update update = (btn_NewDat == (Button)sender) ? (new ZR_Update(true)) : (new ZR_Update(false));
            //update.ShowDialog();
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
                    GetNewData(UIPages.SecondStep); if (Unvalid) return;
                    RenderUI(UIPages.FirstStep);
                    page_ShowStep(UIPages.FirstStep);
                    break;
                case "btn_ThirdBack":
                    GetNewData(UIPages.ThirdStep);
                    if (!Printer.OfflineUse)
                    {
                        EnDBG = SetDebugClean(false);
                        toolStripStatusLabel2.Text = string.Format("打印机当前状态:【{0}】", (EnDBG) ? "在线" : "未知");
                        toolStripStatusLabel2.BackColor = (EnDBG) ? Color.LightGreen : Color.OrangeRed;
                    }
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
                    if (Printer.P_Type == " ")
                    {
                        MessageBox.Show("请选择喷头类型!", "仔细点啦,兄dei...", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    GetNewData(UIPages.SecondStep); if (Unvalid) return;
                    RenderUI(UIPages.ThirdStep);
                    if (!Printer.OfflineUse)
                    {
                        EnDBG = SetDebugClean(true);
                        if (!EnDBG)
                        {
                            MessageBox.Show("网络通讯异常,无法设定调试状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        toolStripStatusLabel2.Text = string.Format("打印机当前状态:【{0}】", (EnDBG) ? "调试" : "在线");
                        toolStripStatusLabel2.BackColor = (EnDBG) ? Color.SkyBlue : Color.LightGreen;
                    }
                    page_ShowStep(UIPages.ThirdStep);
                    break;
                case "btn_ThirdNext":
                    GetNewData(UIPages.ThirdStep);
                    RenderUI(UIPages.FourthStep);
                    page_ShowStep(UIPages.FourthStep);
                    break;
                case "btn_SaveProFile":
                    SaveProcessFile();
                    if (!Printer.OfflineUse)
                    {
                        string DatStream = string.Empty;
                        if (!string.IsNullOrEmpty(saveProFileDialog.FileName))
                        {
                            DatStream = Printer.F_CleanTxtCVRT_Dat(saveProFileDialog.FileName);
                            bool otherEx = false;
                            bool TrasGood = Printer.F_SendDatToPrt(DatStream, ref otherEx);
                            string TipMsg = TrasGood ? string.Format("流程文件已经保存并成功上传至打印机.") :
                                otherEx ? string.Format("已经保存流程文件，当前主板状态不支持升级，请检查") :
                                string.Format("流程文件已经保存，上传至打印机失败!");
                            MessageBoxIcon TipIco = TrasGood ? MessageBoxIcon.Asterisk : MessageBoxIcon.Exclamation;
                            MessageBox.Show(TipMsg, "提示:", MessageBoxButtons.OK, TipIco);
                        }
                    }
                    else return;
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
        public int Num_Used = 1; public List<string> Num_NotUsed = new List<string>();
        //public List<int> Num_NotUsed = new List<int>();
        private void label_P_MouseClick(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (!string.IsNullOrEmpty(label.Text)) return;
                if (Num_NotUsed.Count > 0)
                {
                    label.Text = Num_NotUsed[Num_NotUsed.Count - 1];
                    Num_NotUsed.RemoveAt(Num_NotUsed.Count - 1);
                    Printer.M_PumpMs[int.Parse(label.Text.Trim('#')) - 1].Visible = true;
                    Printer.M_PumpMs[int.Parse(label.Text.Trim('#')) - 1].Checked = true;
                }
                else
                {
                    if (Num_Used > 8)//Max 9? 
                    {
                        MessageBox.Show("当前版本不支持该操作,请联系鑫海胜技术支持", "友情提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    label.Text = Num_Used.ToString() + '#';
                    Printer.M_PumpMs[Num_Used - 1].Visible = true;
                    Num_Used++;
                }
                label.BackColor = Color.LightSeaGreen;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (string.IsNullOrEmpty(label.Text)) return;
                Num_NotUsed.Add(label.Text);
                Printer.M_PumpMs[int.Parse(label.Text.Trim('#')) - 1].Visible = false;
                Printer.M_PumpMs[int.Parse(label.Text.Trim('#')) - 1].Checked = false;
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
            Num_Used = 1; Num_NotUsed.Clear();
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

                label_WaitTime.Visible = trkBar_WaitTime.Visible = label_WaitTimeV.Visible = true;
                label_CycleNum.Visible = trkBar_CycleNum.Visible = label_CycleNumV.Visible = true;
                if (chkBox_HoldTime.Checked)
                    this.chkBox_HoldTime_CheckedChanged(null, null);
                else
                {
                    trkBar_HoldTime.Maximum = AppCfg.Default.M_WrT_Max; trkBar_HoldTime.Minimum = AppCfg.Default.M_WrT_Min;
                    trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                    trkBar_HoldTime.Value = AppCfg.Default.M_WrT_K029;//Printer.M_WorkTime;
                    label_HoldTimeV.Text = (AppCfg.Default.M_WrT_Rto * trkBar_HoldTime.Value).ToString("#0.0") + @" 秒";
                }
                trkBar_Intensity.Maximum = AppCfg.Default.M_Pow_Max; trkBar_Intensity.Minimum = AppCfg.Default.M_Pow_Min;
                trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
                trkBar_WaitTime.Maximum = AppCfg.Default.M_SpT_Max; trkBar_WaitTime.Minimum = AppCfg.Default.M_SpT_Min;
                trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
                trkBar_CycleNum.Maximum = AppCfg.Default.M_Cyc_Max; trkBar_CycleNum.Minimum = AppCfg.Default.M_Cyc_Min;
                trkBar_CycleNum.SmallChange = trkBar_Intensity.TickFrequency = 1;

                trkBar_Intensity.Value = AppCfg.Default.M_Pow_K056;//Printer.M_Strength;
                trkBar_WaitTime.Value = AppCfg.Default.M_SpT_K030;//Printer.M_HoldTime;
                trkBar_CycleNum.Value = AppCfg.Default.M_Cyc_K010;//Printer.M_CycleNum;

                label_IntensityV.Text = (trkBar_Intensity.Value == 0) ? string.Format("全功率运转") :
                    string.Format("{0} 级", Printer.OrderNum[(int)(AppCfg.Default.M_Pow_Rto * trkBar_Intensity.Value)]);
                label_WaitTimeV.Text = (AppCfg.Default.M_SpT_Rto * trkBar_WaitTime.Value).ToString("#0.0") + @" 秒";
                label_CycleNumV.Text = (AppCfg.Default.M_Cyc_Rto * trkBar_CycleNum.Value).ToString() + @" 次";
            }
            else if (rBtn_V_Set == radioBtn)
            {
                chkBox_WaveFreq.Visible = (radioBtn.Checked) ? true : false;
                label_Intensity.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[0]) : Printer.ParaLabel[0];
                label_HoldTime.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[1]) : Printer.ParaLabel[1];
                label_WaitTime.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[2]) : Printer.ParaLabel[2];
                label_CycleNum.Text = (radioBtn.Checked) ? (btn_PumpS.Text + Printer.ParaLabel[3]) : Printer.ParaLabel[3];

                trkBar_Intensity.Enabled = true;
                trkBar_WaitTime.Enabled = true;
                trkBar_CycleNum.Enabled = true;
                this.chkBox_WaveFreq_CheckedChanged(null, null);

                trkBar_WaitTime.Maximum = AppCfg.Default.V_SpT_Max; trkBar_WaitTime.Minimum = AppCfg.Default.V_SpT_Min;
                trkBar_WaitTime.SmallChange = trkBar_WaitTime.TickFrequency = 1;
                trkBar_CycleNum.Maximum = AppCfg.Default.V_Cyc_Max; trkBar_CycleNum.Minimum = AppCfg.Default.V_Cyc_Min;
                trkBar_CycleNum.SmallChange = trkBar_Intensity.TickFrequency = 1;

                trkBar_WaitTime.Value = AppCfg.Default.V_SpT_Kvvv;//Printer.V_HoldTime;
                trkBar_CycleNum.Value = AppCfg.Default.V_Cyc_K007;//Printer.V_CycleNum;

                label_WaitTimeV.Text = (AppCfg.Default.V_SpT_Rto * trkBar_WaitTime.Value).ToString("#0.0") + @" 秒";
                label_CycleNumV.Text = (AppCfg.Default.V_Cyc_Rto * trkBar_CycleNum.Value).ToString() + @" 次";
            }
            else if (rBtn_N_Set == radioBtn)
            {
                label_Intensity.Visible = (radioBtn.Checked) ? false : true;
                label_HoldTime.Text = (radioBtn.Checked) ? Printer.ParaLabel[4] : Printer.ParaLabel[1];
                label_WaitTime.Visible = (radioBtn.Checked) ? false : true;
                label_CycleNum.Visible = (radioBtn.Checked) ? false : true;

                trkBar_Intensity.Visible = (radioBtn.Checked) ? false : true;
                trkBar_HoldTime.Maximum = AppCfg.Default.N_Dly_Max; trkBar_HoldTime.Minimum = AppCfg.Default.N_Dly_Min;
                trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_WaitTime.Visible = (radioBtn.Checked) ? false : true;
                trkBar_CycleNum.Visible = (radioBtn.Checked) ? false : true;

                trkBar_HoldTime.Value = AppCfg.Default.N_Dly_K006;//Printer.N_WaitTime;

                label_IntensityV.Visible = (radioBtn.Checked) ? false : true;
                label_HoldTimeV.Text = (AppCfg.Default.N_Dly_Rto * trkBar_HoldTime.Value).ToString() + @" 秒";
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
                trkBar_HoldTime.Maximum = AppCfg.Default.M_Ttt_Max; trkBar_HoldTime.Minimum = AppCfg.Default.M_Ttt_Min;
                trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_HoldTime.Value = AppCfg.Default.M_Ttt_K022;//Printer.M_OnlyWorkTime;
                label_HoldTimeV.Text = (AppCfg.Default.M_Ttt_Rto * trkBar_HoldTime.Value).ToString() + @" 秒";
            }
            else
            {
                Printer.M_OnlyTime = false;
                trkBar_HoldTime.Maximum = AppCfg.Default.M_WrT_Max; trkBar_HoldTime.Minimum = AppCfg.Default.M_WrT_Min;
                trkBar_HoldTime.SmallChange = trkBar_HoldTime.TickFrequency = 1;
                trkBar_HoldTime.Value = AppCfg.Default.M_WrT_K029;//Printer.M_WorkTime;
                label_HoldTimeV.Text = (AppCfg.Default.M_WrT_Rto * trkBar_HoldTime.Value).ToString("#0.0") + @" 秒";
            }
            trkBar_WaitTime.Enabled = (chkBox_HoldTime.Checked) ? false : true;
            trkBar_CycleNum.Enabled = (chkBox_HoldTime.Checked) ? false : true;
        }
        private void chkBox_WaveFreq_CheckedChanged(object sender, EventArgs e)
        {
            label_Intensity.Text = (chkBox_WaveFreq.Checked) ? string.Format("闪喷波形选择") : (btn_PumpS.Text + Printer.ParaLabel[0]);
            trkBar_Intensity.Maximum = (chkBox_WaveFreq.Checked) ? AppCfg.Default.V_Wav_Max : AppCfg.Default.V_Pow_Max;
            trkBar_Intensity.Minimum = (chkBox_WaveFreq.Checked) ? AppCfg.Default.V_Wav_Min : AppCfg.Default.V_Pow_Min;
            trkBar_Intensity.SmallChange = trkBar_Intensity.TickFrequency = 1;
            trkBar_Intensity.Value = (chkBox_WaveFreq.Checked) ? AppCfg.Default.V_Wav_Tvvv : AppCfg.Default.V_Pow_Kvvv;
            label_IntensityV.Text = (chkBox_WaveFreq.Checked) ? string.Format("{0} 波形", (char)('A' + (trkBar_Intensity.Value * AppCfg.Default.V_Wav_Rto))) :
                string.Format("{0} 级", Printer.OrderNum[(int)(AppCfg.Default.V_Pow_Rto * trkBar_Intensity.Value)]);

            label_HoldTime.Text = (chkBox_WaveFreq.Checked) ? string.Format("闪喷频率调节") : (btn_PumpS.Text + Printer.ParaLabel[1]);
            trkBar_HoldTime.Maximum = (chkBox_WaveFreq.Checked) ? AppCfg.Default.V_Frq_Max : AppCfg.Default.V_WrT_Max;
            trkBar_HoldTime.Minimum = (chkBox_WaveFreq.Checked) ? AppCfg.Default.V_Frq_Min : AppCfg.Default.V_WrT_Min;
            trkBar_HoldTime.SmallChange = trkBar_Intensity.TickFrequency = 1;
            trkBar_HoldTime.Value = (chkBox_WaveFreq.Checked) ? AppCfg.Default.V_Frq_K074 : AppCfg.Default.V_WrT_Kvvv;
            label_HoldTimeV.Text = (chkBox_WaveFreq.Checked) ? string.Format("{0} kHZ", (trkBar_HoldTime.Value * AppCfg.Default.V_Frq_Rto)) :
                (AppCfg.Default.V_WrT_Rto * trkBar_HoldTime.Value).ToString("#0.0") + @" 秒";

            label_WaitTime.Visible = trkBar_WaitTime.Visible = label_WaitTimeV.Visible = !chkBox_WaveFreq.Checked;
            label_CycleNum.Visible = trkBar_CycleNum.Visible = label_CycleNumV.Visible = !chkBox_WaveFreq.Checked;
        }
        private void trkBar_Para1_ValueChanged(object sender, EventArgs e)
        {
            if (rBtn_N_Set.Checked)
            {
                label_HoldTimeV.Text = (AppCfg.Default.N_Dly_Rto * trkBar_HoldTime.Value).ToString() + @" 秒";
            }
            else if (rBtn_M_Set.Checked && chkBox_HoldTime.Checked)
            {
                label_HoldTimeV.Text = (AppCfg.Default.M_Ttt_Rto * trkBar_HoldTime.Value).ToString() + @" 秒";
            }
            else if (rBtn_M_Set.Checked && !chkBox_HoldTime.Checked)
            {
                label_IntensityV.Text = (trkBar_Intensity.Value == 0) ? string.Format("全功率运转") :
                    string.Format("{0} 级", Printer.OrderNum[(int)(AppCfg.Default.M_Pow_Rto * trkBar_Intensity.Value)]);
                label_HoldTimeV.Text = (AppCfg.Default.M_WrT_Rto * trkBar_HoldTime.Value).ToString("#0.0") + @" 秒";
                label_WaitTimeV.Text = (AppCfg.Default.M_SpT_Rto * trkBar_WaitTime.Value).ToString("#0.0") + @" 秒";
                label_CycleNumV.Text = (AppCfg.Default.M_Cyc_Rto * trkBar_CycleNum.Value).ToString() + @" 次";
            }
            else
            {
                label_IntensityV.Text = (chkBox_WaveFreq.Checked) ? string.Format("{0} 波形", (char)('A' + (trkBar_Intensity.Value * AppCfg.Default.V_Wav_Rto))) :
                string.Format("{0} 级", Printer.OrderNum[(int)(AppCfg.Default.V_Pow_Rto * trkBar_Intensity.Value)]);
                label_HoldTimeV.Text = (chkBox_WaveFreq.Checked) ? string.Format("{0} kHZ", (trkBar_HoldTime.Value * AppCfg.Default.V_Frq_Rto)) :
                (AppCfg.Default.V_WrT_Rto * trkBar_HoldTime.Value).ToString("#0.0") + @" 秒";
                label_WaitTimeV.Text = (AppCfg.Default.V_SpT_Rto * trkBar_WaitTime.Value).ToString("#0.0") + @" 秒";
                label_CycleNumV.Text = (AppCfg.Default.V_Cyc_Rto * trkBar_CycleNum.Value).ToString() + @" 次";
            }
        }
        private void btn_SetPumpPara_Click(object sender, EventArgs e)
        {
            btn_SetPumpPara.Enabled = false;
            //更新数据的主体
            if (rBtn_M_Set.Checked)
            {
                if (chkBox_HoldTime.Checked)
                {
                    AppCfg.Default.M_Ttt_K022 = trkBar_HoldTime.Value;
                    if (!Printer.OfflineUse && EnDBG)//!Printer.OfflineUse && EnDBG
                    {
                        var vn = NameOf(() => AppCfg.Default.M_Ttt_K022);
                        int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.M_Ttt_K022);
                    }
                }
                else
                {
                    AppCfg.Default.M_Pow_K056 = trkBar_Intensity.Value;
                    AppCfg.Default.M_WrT_K029 = trkBar_HoldTime.Value;
                    AppCfg.Default.M_SpT_K030 = trkBar_WaitTime.Value;
                    AppCfg.Default.M_Cyc_K010 = trkBar_CycleNum.Value;
                    if (!Printer.OfflineUse && EnDBG)
                    {
                        var vn = NameOf(() => AppCfg.Default.M_Pow_K056);
                        int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.M_Pow_K056);

                        vn = NameOf(() => AppCfg.Default.M_WrT_K029);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.M_WrT_K029);

                        vn = NameOf(() => AppCfg.Default.M_SpT_K030);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.M_SpT_K030);

                        vn = NameOf(() => AppCfg.Default.M_Cyc_K010);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.M_Cyc_K010);
                    }
                }
            }
            else if (rBtn_V_Set.Checked)
            {
                if (chkBox_WaveFreq.Checked)
                {
                    AppCfg.Default.V_Wav_Tvvv = trkBar_Intensity.Value;
                    AppCfg.Default.V_Frq_K074 = trkBar_HoldTime.Value;
                    if (!Printer.OfflineUse && EnDBG)
                    {
                        SendStepCmd(string.Format("@T{0};", AppCfg.Default.V_Wav_Tvvv));
                        Thread.Sleep(100);
                        var vn = NameOf(() => AppCfg.Default.V_Frq_K074);
                        int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.V_Frq_K074);
                        btn_SetPumpPara.Enabled = true;
                    }
                }
                else
                {
                    AppCfg.Default.V_Pow_Kvvv = trkBar_Intensity.Value;
                    AppCfg.Default.V_WrT_Kvvv = trkBar_HoldTime.Value;
                    AppCfg.Default.V_SpT_Kvvv = trkBar_WaitTime.Value;
                    AppCfg.Default.V_Cyc_K007 = trkBar_CycleNum.Value;
                    if (!Printer.OfflineUse && EnDBG)
                    {
                        var vn = NameOf(() => AppCfg.Default.V_Cyc_K007);
                        int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SetSingleKVal(Knnn, AppCfg.Default.V_Cyc_K007);
                    }
                }
            }
            else if (rBtn_N_Set.Checked)
            {
                AppCfg.Default.N_Dly_K006 = trkBar_HoldTime.Value;
                if (!Printer.OfflineUse && EnDBG)
                {
                    var vn = NameOf(() => AppCfg.Default.N_Dly_K006);
                    int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                    SetSingleKVal(Knnn, AppCfg.Default.N_Dly_K006);
                }
            }
            btn_SetPumpPara.Enabled = true;
        }
        private void rBtn_Para2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            string Vel = "速度", Pos = "位置";
            if (rBtn_C_Set == radioBtn)
            {
                label_Velocity.Text = (radioBtn.Checked) ? (btn_Stage.Text + Vel) : Vel;
                label_Position.Text = (radioBtn.Checked) ? (btn_Stage.Text + Pos) : Pos;

                trkBar_Velocity.Maximum = AppCfg.Default.C_Vel_Max; trkBar_Velocity.Minimum = AppCfg.Default.C_Vel_Min;
                trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
                trkBar_Velocity.Value = AppCfg.Default.C_Vel_K008;//Printer.C_Speed;
                label_VelocityV.Text = (AppCfg.Default.C_Vel_Rto * trkBar_Velocity.Value).ToString() + " kHZ";

                List<string> items = new List<string>(Printer.C_LevelMark);
                //Printer.C_Level = new List<int>(items.Count);
                comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
                label_PosUnit.Text = label_MicUnit.Text = @"(0-255) * 1024 pulse";
            }
            else if (rBtn_P_Set == radioBtn)
            {
                label_Velocity.Text = (radioBtn.Checked) ? (btn_Painter.Text + Vel) : Vel;
                label_Position.Text = (radioBtn.Checked) ? (btn_Painter.Text + Pos) : Pos;

                trkBar_Velocity.Maximum = AppCfg.Default.P_Vel_Max; trkBar_Velocity.Minimum = AppCfg.Default.P_Vel_Min;
                trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
                trkBar_Velocity.Value = AppCfg.Default.P_Vel_Kppp;//Printer.P_Speed;
                label_VelocityV.Text = (trkBar_Velocity.Value == 0) ? string.Format("最慢") :
                    (AppCfg.Default.P_Vel_Rto * trkBar_Velocity.Value).ToString("#0.00") + " m/s";

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
                label_PosUnit.Text = "mm";//= label_MicUnit.Text
            }
            else if (rBtn_W_Set == radioBtn)
            {
                label_Velocity.Text = (radioBtn.Checked) ? (btn_Wiper.Text + Vel) : Vel;
                label_Position.Text = (radioBtn.Checked) ? (btn_Wiper.Text + Pos) : Pos;

                trkBar_Velocity.Maximum = AppCfg.Default.W_Vel_Max; trkBar_Velocity.Minimum = AppCfg.Default.W_Vel_Min;
                trkBar_Velocity.SmallChange = trkBar_Velocity.TickFrequency = 1;
                trkBar_Velocity.Value = AppCfg.Default.W_Vel_K055;//Printer.W_Speed;
                label_VelocityV.Text = (AppCfg.Default.W_Vel_Rto * trkBar_Velocity.Value).ToString() + " kHZ";

                List<string> items = new List<string>();
                //Printer.W_YPos = new List<int>(items.Count); 刮片只考虑一个位置
                for (int wn = 0; wn < Printer.P_Counts; wn++)
                {
                    items.Add((wn + 1).ToString() + "号喷头刮墨");
                }
                comBox_Position.DataSource = items; comBox_Position.SelectedIndex = -1;
                label_PosUnit.Text = "(0-255) * 256 pulse";
            }
            txtBox_Position.Text = "";
            List<string> items2 = new List<string>();
            for (int i = 1; i < 20 + 1; i++) items2.Add(i.ToString());
            comBox_microPos.DataSource = items2; comBox_microPos.SelectedIndex = -1;
        }
        private void trkBar_Para2_ValueChanged(object sender, EventArgs e)
        {
            if (rBtn_C_Set.Checked)
            {
                label_VelocityV.Text = (AppCfg.Default.C_Vel_Rto * trkBar_Velocity.Value).ToString() + " kHZ";
            }
            else if (rBtn_P_Set.Checked)
            {
                label_VelocityV.Text = (trkBar_Velocity.Value == 0) ? string.Format("最慢") :
                    (AppCfg.Default.P_Vel_Rto * trkBar_Velocity.Value).ToString("#0.00") + " m/s";
            }
            else if (rBtn_W_Set.Checked)
            {
                label_VelocityV.Text = (AppCfg.Default.W_Vel_Rto * trkBar_Velocity.Value).ToString() + " kHZ";
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
                    txtBox_Position.Enabled = (comBox_Position.SelectedItem.ToString() == Printer.C_LevelMark[3]) ? false : true;
                }
                else
                    txtBox_Position.Enabled = false;
            }
            else if (rBtn_P_Set.Checked)
            {
                if (comBox_Position.SelectedItem != null)
                {
                    if (Printer.P_XPos.Count != 0)
                        txtBox_Position.Text = Printer.P_XPos[comBox_Position.SelectedIndex].ToString();
                    txtBox_Position.Enabled = true;
                }
                else
                    txtBox_Position.Enabled = false;
            }
            else if (rBtn_W_Set.Checked)
            {
                if (comBox_Position.SelectedItem != null)
                {
                    if (Printer.W_YPos.Count != 0)
                        txtBox_Position.Text = Printer.W_YPos[comBox_Position.SelectedIndex].ToString();
                    txtBox_Position.Enabled = true;
                }
                else
                    txtBox_Position.Enabled = false;
            }
        }
        private void btn_SetAxisPara_Click(object sender, EventArgs e)
        {
            btn_SetAxisPara.Enabled = false;
            //更新数据的主体            
            bool rst = false; int tmpVal = 0;
            if (comBox_Position.SelectedItem != null) { rst = int.TryParse(txtBox_Position.Text, out tmpVal); }
            if (rBtn_C_Set.Checked)
            {
                AppCfg.Default.C_Vel_K008 = trkBar_Velocity.Value;
                if (!Printer.OfflineUse && EnDBG)
                {
                    var vn = NameOf(() => AppCfg.Default.C_Vel_K008);
                    int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                    SetSingleKVal(Knnn, AppCfg.Default.C_Vel_K008);
                }
                if (comBox_Position.SelectedIndex != -1)
                {
                    if (tmpVal < AppCfg.Default.C_Pos_Min || tmpVal > AppCfg.Default.C_Pos_Max || !rst)
                    {
                        MessageBox.Show("墨栈位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btn_SetAxisPara.Enabled = true;
                        return;
                    }
                    Printer.C_Level[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
                    if (comBox_Position.SelectedItem != null)
                    {
                        string vn = string.Empty; int Knnn = 0;
                        switch (comBox_Position.SelectedIndex)
                        {
                            case 0:
                                AppCfg.Default.C_Pos0_K999 = Printer.C_Level[comBox_Position.SelectedIndex];
                                vn = NameOf(() => AppCfg.Default.C_Pos0_K999);
                                Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                                if (!Printer.OfflineUse && EnDBG) SendStepCmd(string.Format("@K1{0}v{1};", Knnn, AppCfg.Default.C_Pos0_K999));
                                break;
                            case 1:
                                AppCfg.Default.C_Pos1_K026 = Printer.C_Level[comBox_Position.SelectedIndex];
                                vn = NameOf(() => AppCfg.Default.C_Pos1_K026);
                                Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                                if (!Printer.OfflineUse && EnDBG) SetSingleKVal(Knnn, AppCfg.Default.C_Pos1_K026);
                                break;
                            case 2:
                                AppCfg.Default.C_Pos2_K012 = Printer.C_Level[comBox_Position.SelectedIndex];
                                vn = NameOf(() => AppCfg.Default.C_Pos2_K012);
                                Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                                if (!Printer.OfflineUse && EnDBG) SetSingleKVal(Knnn, AppCfg.Default.C_Pos2_K012);
                                break;
                            case 3:
                                break;
                            case 4:
                                AppCfg.Default.C_Pos4_K059 = Printer.C_Level[comBox_Position.SelectedIndex];
                                vn = NameOf(() => AppCfg.Default.C_Pos4_K059);
                                Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                                if (!Printer.OfflineUse && EnDBG) SetSingleKVal(Knnn, AppCfg.Default.C_Pos4_K059);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (rBtn_P_Set.Checked)
            {
                AppCfg.Default.P_Vel_Kppp = trkBar_Velocity.Value;
                if (!Printer.OfflineUse && EnDBG)
                {
                    SendStepCmd(AddSingleStep("Pp"));
                }
                if (comBox_Position.SelectedIndex != -1)
                {
                    if (tmpVal < AppCfg.Default.P_Pos_Min || tmpVal > AppCfg.Default.P_Pos_Max || !rst)
                    {
                        MessageBox.Show("小车位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btn_SetAxisPara.Enabled = true;
                        return;
                    }
                    Printer.P_XPos[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
                    if (!Printer.OfflineUse && EnDBG && comBox_Position.SelectedItem != null)
                    {
                        //No K-Value stored in FPGA or CPU
                    }
                }
            }
            else if (rBtn_W_Set.Checked)
            {
                AppCfg.Default.W_Vel_K055 = trkBar_Velocity.Value;
                if (!Printer.OfflineUse && EnDBG)
                {
                    var vn = NameOf(() => AppCfg.Default.W_Vel_K055);
                    int Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                    SetSingleKVal(Knnn, AppCfg.Default.W_Vel_K055);
                }
                if (comBox_Position.SelectedIndex != -1)
                {
                    if (tmpVal < AppCfg.Default.W_Pos_Min || tmpVal > AppCfg.Default.W_Pos_Max || !rst)
                    {
                        MessageBox.Show("刮片位置值无效", "Warn!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btn_SetAxisPara.Enabled = true;
                        return;
                    }
                    Printer.W_YPos[comBox_Position.SelectedIndex] = rst ? tmpVal : 0;
                    if (!Printer.OfflineUse && EnDBG && comBox_Position.SelectedItem != null)
                    {
                        //No K-Value stored in FPGA or CPU
                    }
                }
            }
            btn_SetAxisPara.Enabled = true;
        }
        private void btn_TestPos_Click(object sender, EventArgs e)
        {
            btn_TestPos.Enabled = btn_microDec.Enabled = btn_microInc.Enabled = false;
            if (btn_TestPos == (Button)sender)
            {
                if (rBtn_C_Set.Checked)
                {
                    if ((comBox_Position.SelectedIndex != -1) && !Printer.OfflineUse)
                    {
                        this.btn_SetAxisPara_Click(null, null);
                        SendStepCmd(AddSingleStep(string.Format("Cn{0}", comBox_Position.SelectedIndex)));
                    }
                    else
                    {
                        btn_TestPos.Enabled = btn_microDec.Enabled = btn_microInc.Enabled = true;
                        return;
                    }
                }
                else if (rBtn_P_Set.Checked)
                {
                    if ((comBox_Position.SelectedIndex != -1) && !Printer.OfflineUse)
                    {
                        this.btn_SetAxisPara_Click(null, null);

                        string pn = string.Empty;
                        if (CleanMode.W1_C_PP == Printer.CleanMode || CleanMode.W0C_PP == Printer.CleanMode || CleanMode.W1C_PP == Printer.CleanMode)
                        {
                            if (comBox_Position.SelectedIndex % 2 == 0) pn = string.Format("P{0}", comBox_Position.SelectedIndex / 2 + 1);
                            else pn = string.Format("P{0}", (comBox_Position.SelectedIndex + 1) / 2);
                        }
                        else
                            pn = string.Format("P{0}", comBox_Position.SelectedIndex + 1);

                        //int dist = 0;
                        //if (txtBox_Position.Text != "") int.TryParse(txtBox_Position.Text, out dist);
                        //SendStepCmd(string.Format(@"@{0}{1};", pn, dist));

                        SendStepCmd(string.Format(@"@{0}{1};", pn, Printer.P_XPos[comBox_Position.SelectedIndex]));
                    }
                    else
                    {
                        btn_TestPos.Enabled = btn_microDec.Enabled = btn_microInc.Enabled = true;
                        return;
                    }
                }
                else if (rBtn_W_Set.Checked)
                {
                    if ((comBox_Position.SelectedIndex != -1) && !Printer.OfflineUse)
                    {
                        this.btn_SetAxisPara_Click(null, null);

                        string wn = string.Format("H{0}", comBox_Position.SelectedIndex + 1);

                        SendStepCmd(AddSingleStep(wn));
                    }
                    else
                    {
                        btn_TestPos.Enabled = btn_microDec.Enabled = btn_microInc.Enabled = true;
                        return;
                    }
                }
            }
            else if (btn_microDec == (Button)sender)
            {
                if (comBox_microPos.SelectedItem != null)
                {
                    int miniDec = int.Parse(comBox_microPos.SelectedItem.ToString());
                    int initDec = 0;
                    bool rst = int.TryParse(txtBox_Position.Text, out initDec);
                    txtBox_Position.Text = (rst) ? string.Format("{0}", (initDec - miniDec)) : txtBox_Position.Text;
                }
                this.btn_TestPos_Click(btn_TestPos, null);
            }
            else if (btn_microInc == (Button)sender)
            {
                if (comBox_microPos.SelectedItem != null)
                {
                    int miniInc = int.Parse(comBox_microPos.SelectedItem.ToString());
                    int initInc = 0;
                    bool rst = int.TryParse(txtBox_Position.Text, out initInc);
                    txtBox_Position.Text = (rst) ? string.Format("{0}", (initInc + miniInc)) : txtBox_Position.Text;
                }
                this.btn_TestPos_Click(btn_TestPos, null);
            }

            btn_TestPos.Enabled = btn_microDec.Enabled = btn_microInc.Enabled = true;
            return;
        }
        #endregion

        #region Fourth groupBox callBack (&function)
        private string AddSingleStep(string stepTag)
        {
            // Here no "K" & "T" command character !
            string Head = @"@", Tail = @";";
            string SingleStep = "";
            switch (stepTag.Substring(0, 1))
            {
                case "P":
                    if ("p" == stepTag.Substring(1, 1))
                    {
                        SingleStep = stepTag + AppCfg.Default.P_Vel_Kppp.ToString();
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
                case "W"://Now replaced by "H"!!!
                    SingleStep = stepTag;
                    break;
                case "H":
                    var hn = int.Parse(stepTag.Substring(1, 1));
                    SingleStep = stepTag + Printer.W_YPos[hn - 1].ToString();
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
                        SingleStep = stepTag + "x" + AppCfg.Default.M_Pow_K056.ToString() + "p" + AppCfg.Default.M_WrT_K029.ToString() + "s"
                            + AppCfg.Default.M_SpT_K030.ToString() + "n" + "0"; //AppCfg.Default.M_Cyc_K010.ToString();
                    }
                    break;
                case "V":
                    SingleStep = stepTag + AppCfg.Default.V_Pow_Kvvv.ToString() + "p" + AppCfg.Default.V_WrT_Kvvv.ToString() + "s"
                        + AppCfg.Default.V_SpT_Kvvv.ToString() + "n" + "0";//AppCfg.Default.V_Cyc_K007.ToString();
                    break;
                case "N":
                    SingleStep = stepTag; //"Nr0";
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
                        var hn = "H" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(hn));
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
                        var hn = "H" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(hn));
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
                        var hn = "H" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(hn));
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
                        var hn = "H" + Printer.CleanSequence[n].ToString();
                        ProcessTemplate.Add(AddSingleStep(hn));
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
                else if ("H" == act.Substring(1, 1))
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
        private void TestSingleStep(object sender, EventArgs e)
        {
            if (listView_CleanSteps.SelectedItems.Count == 0) return;
            var index = int.Parse(listView_CleanSteps.SelectedItems[0].Text) - 1;
            listView_CleanSteps.Focus();
            listView_CleanSteps.Items[index].Selected = true;
            if (Printer.OfflineUse || !EnDBG) return;

            btn_TestStetp.Enabled = false;
            if (Printer.CleanProcess[index].Substring(1, 1) == "C")
            {
                string vn = string.Empty; int Knnn = 0;
                switch (Printer.CleanProcess[index].Substring(3, 1))
                {
                    case "0":
                        vn = NameOf(() => AppCfg.Default.C_Pos0_K999);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SendStepCmd(string.Format("@K1{0}v{1};", Knnn, AppCfg.Default.C_Pos0_K999));
                        break;
                    case "1":
                        vn = NameOf(() => AppCfg.Default.C_Pos1_K026);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SendStepCmd(string.Format("@K1{0}v{1};", Knnn, AppCfg.Default.C_Pos1_K026));
                        break;
                    case "2":
                        vn = NameOf(() => AppCfg.Default.C_Pos2_K012);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SendStepCmd(string.Format("@K1{0}v{1};", Knnn, AppCfg.Default.C_Pos2_K012));
                        break;
                    case "3":
                        break;
                    case "4":
                        vn = NameOf(() => AppCfg.Default.C_Pos4_K059);
                        Knnn = int.Parse(vn.Substring(vn.Length - 3, 3));
                        SendStepCmd(string.Format("@K1{0}v{1};", Knnn, AppCfg.Default.C_Pos4_K059));
                        break;
                    default:
                        break;
                }
            }
            Thread.Sleep(100);
            SendStepCmd(Printer.CleanProcess[index]);
            btn_TestStetp.Enabled = true;
        }
        private void ManualAddSteps(object sender, EventArgs e)
        {
            if (listView_CleanSteps.SelectedItems.Count == 0) return;
            var index = int.Parse(listView_CleanSteps.SelectedItems[0].Text) - 1;
            switch (((Button)sender).Name)
            {
                case "btn_PumpM":
                    if (DialogResult.OK == MessageBox.Show("点击'确定'添加正常墨泵动作, 点击'取消'则添加抽废墨动作", "?",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                        Printer.CleanProcess.Insert(index + 1, AddSingleStep("M1"));
                    else
                        Printer.CleanProcess.Insert(index + 1, AddSingleStep("M0"));
                    break;
                case "btn_Delay":
                    Printer.CleanProcess.Insert(index + 1, AddSingleStep("Nr0"));
                    break;
                case "btn_Painter":
                    X_Painter xpt = new X_Painter(index, true);
                    DialogResult xptRst = xpt.ShowDialog();
                    if (xptRst == DialogResult.Cancel || xptRst == DialogResult.No || xptRst == DialogResult.None)
                    {
                        listView_CleanSteps.Focus();
                        listView_CleanSteps.Items[index].Selected = true;
                        return;
                    }
                    break;
                case "btn_Wiper":
                    Y_Wiper ywp = new Y_Wiper(index, true);
                    DialogResult ywpRst = ywp.ShowDialog();
                    if (ywpRst == DialogResult.Cancel || ywpRst == DialogResult.No || ywpRst == DialogResult.None)
                    {
                        listView_CleanSteps.Focus();
                        listView_CleanSteps.Items[index].Selected = true;
                        return;
                    }
                    break;
                case "btn_Stage":
                    Z_Stage zsg = new Z_Stage(index, true);
                    DialogResult zsgRst = zsg.ShowDialog();
                    if (zsgRst == DialogResult.Cancel || zsgRst == DialogResult.No || zsgRst == DialogResult.None)
                    {
                        listView_CleanSteps.Focus();
                        listView_CleanSteps.Items[index].Selected = true;
                        return;
                    }
                    break;
                case "btn_PumpS":
                    Printer.CleanProcess.Insert(index + 1, AddSingleStep("V"));
                    break;
            }
            FlushActionList(Printer.CleanProcess);
            listView_CleanSteps.Focus();
            listView_CleanSteps.Items[index + 1].Selected = true;
            listView_CleanSteps.EnsureVisible(index + 1);
        }
        private void EditCleanSteps(object sender, EventArgs e)
        {
            if (listView_CleanSteps.SelectedItems.Count == 0) return;
            var index = int.Parse(listView_CleanSteps.SelectedItems[0].Text) - 1;
            switch (((Button)sender).Name)
            {
                case "btn_EditStetp":
                    string CurStep = Printer.CleanProcess[index].Trim('@', ';');
                    if ("P" == CurStep.Substring(0, 1))
                    {
                        X_Painter xpt = new X_Painter(index, false);
                        xpt.ShowDialog();
                    }
                    else if ("W" == CurStep.Substring(0, 1))
                    {
                        Y_Wiper ywp = new Y_Wiper(index, false);
                        ywp.ShowDialog();
                    }
                    else if ("H" == CurStep.Substring(0, 1))
                    {
                        Y_Wiper ywp = new Y_Wiper(index, false);
                        ywp.ShowDialog();
                    }
                    else if ("C" == CurStep.Substring(0, 1))
                    {
                        Z_Stage zsg = new Z_Stage(index, false);
                        zsg.ShowDialog();
                    }
                    else if ("M" == CurStep.Substring(0, 1))
                    {
                        Pump_M ppm = new Pump_M(index);
                        ppm.ShowDialog();
                    }
                    else if ("V" == CurStep.Substring(0, 1))
                    {
                        Pump_S pps = new Pump_S(index);
                        pps.ShowDialog();
                    }
                    else if ("N" == CurStep.Substring(0, 1))
                    {
                        T_Delay tdy = new T_Delay(index);
                        tdy.ShowDialog();
                    }
                    FlushActionList(Printer.CleanProcess);
                    listView_CleanSteps.Focus();
                    listView_CleanSteps.Items[index].Selected = true;
                    listView_CleanSteps.EnsureVisible(index);
                    break;
                case "btn_UpStetp":
                    List<string> NewOrderUp = new List<string>();
                    if (index == 0) return;
                    else
                    {
                        string temp = Printer.CleanProcess[index - 1];
                        Printer.CleanProcess[index - 1] = Printer.CleanProcess[index];
                        Printer.CleanProcess[index] = temp;
                    }
                    FlushActionList(Printer.CleanProcess);
                    listView_CleanSteps.Focus();
                    listView_CleanSteps.Items[index - 1].Selected = true;
                    listView_CleanSteps.EnsureVisible(index - 1);
                    break;
                case "btn_DownStetp":
                    List<string> NewOrderDn = new List<string>();
                    if (index == Printer.CleanProcess.Count - 1) return;
                    else
                    {
                        string temp = Printer.CleanProcess[index + 1];
                        Printer.CleanProcess[index + 1] = Printer.CleanProcess[index];
                        Printer.CleanProcess[index] = temp;
                    }
                    FlushActionList(Printer.CleanProcess);
                    listView_CleanSteps.Focus();
                    listView_CleanSteps.Items[index + 1].Selected = true;
                    listView_CleanSteps.EnsureVisible(index + 1);
                    break;
                case "btn_DelStetp":
                    Printer.CleanProcess.RemoveAt(index);
                    FlushActionList(Printer.CleanProcess);
                    if (Printer.CleanProcess.Count != 0 && index < Printer.CleanProcess.Count)
                    {
                        listView_CleanSteps.Focus();
                        listView_CleanSteps.Items[index].Selected = true;
                        listView_CleanSteps.EnsureVisible(index);
                    }
                    break;
            }
        }
        private void listView_CleanSteps_DoubleClick(object sender, EventArgs e)
        {
            EditCleanSteps(btn_EditStetp, null);
        }
        public static string ExplainAction(string act)
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
                        else description = string.Format("小车速度设定为:【{0}】", (AppCfg.Default.P_Vel_Rto * Vel).ToString("#0.00") + @" m/s");
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
                    var wn = 0; int.TryParse(act.Substring(2, 1), out wn);
                    if (wn == 0) description = "刮片到达【原点位置】";
                    else description = string.Format("刮片到达【{0}{1}位置】", wn, "号喷头刮墨");//no longer used
                    break;
                case "H":
                    var hn = 0; int.TryParse(act.Substring(1, 1), out hn);
                    if (hn == 0) description = "刮片到达【原点位置】";
                    else description = string.Format("刮片到达【{0}{1}位置】", hn, "号喷头刮墨");
                    break;
                case "C":
                    var cn = 3; int.TryParse(act.Substring(2, 1), out cn);
                    description = string.Format("墨栈到达【{0}高度】", Printer.C_LevelMark[cn]);
                    break;
                case "M":
                    if ("0" == act.Substring(1, 1))
                    {
                        if ("M0999" == act)
                        {
                            description = string.Format("墨泵吸墨【{0} 秒】", (AppCfg.Default.M_Ttt_K022 * AppCfg.Default.M_Ttt_Rto).ToString());
                            Printer.M_OnlyWorkTime = AppCfg.Default.M_Ttt_K022;
                        }
                        else if ("M01000" == act) { description = string.Format("墨泵吸墨【常开,一直吸】"); Printer.M_OnlyWorkTime = 0x03E8; }
                        else
                        {
                            Printer.M_OnlyWorkTime = int.Parse(act.Substring(2));
                            description = string.Format("墨泵吸墨【{0} 秒】", Printer.M_OnlyWorkTime);
                        }
                    }
                    else
                    {
                        var MuseK = (act.Substring(act.IndexOf('n') + 1, 1) == "0") ? true : false;
                        if (MuseK)
                        {
                            description = string.Format("墨泵工作【{0} 级强度】【单次运转 {1} 秒】【单次停止 {2} 秒】【循环 {3} 次】",
                            Printer.OrderNum[(int)(AppCfg.Default.M_Pow_K056 * AppCfg.Default.M_Pow_Rto)],
                            (AppCfg.Default.M_WrT_K029 * AppCfg.Default.M_WrT_Rto).ToString("#0.0"),
                            (AppCfg.Default.M_SpT_K030 * AppCfg.Default.M_SpT_Rto).ToString("#0.0"),
                            (AppCfg.Default.M_Cyc_K010 * AppCfg.Default.M_Cyc_Rto).ToString());
                            // for Editor form get correct data
                            Printer.M_Strength = AppCfg.Default.M_Pow_K056;
                            Printer.M_WorkTime = AppCfg.Default.M_WrT_K029;
                            Printer.M_HoldTime = AppCfg.Default.M_SpT_K030;
                            Printer.M_CycleNum = AppCfg.Default.M_Cyc_K010;
                        }
                        else
                        {
                            string[] ValMattr = act.Substring(2).Split('x', 'p', 's', 'n');
                            int.TryParse(ValMattr[1], out Printer.M_Strength);
                            int.TryParse(ValMattr[2], out Printer.M_WorkTime);
                            int.TryParse(ValMattr[3], out Printer.M_HoldTime);
                            int.TryParse(ValMattr[4], out Printer.M_CycleNum);
                            description = string.Format("墨泵工作【{0} 级强度】【单次运转 {1} 秒】【单次停止 {2} 秒】【循环 {3} 次】",
                            Printer.OrderNum[(int)(Printer.M_Strength * AppCfg.Default.M_Pow_Rto)],
                            (Printer.M_WorkTime * AppCfg.Default.M_WrT_Rto).ToString("#0.0"),
                            (Printer.M_HoldTime * AppCfg.Default.M_SpT_Rto).ToString("#0.0"),
                            (Printer.M_CycleNum * AppCfg.Default.M_Cyc_Rto).ToString());
                        }
                    }
                    break;
                case "V":
                    var VuseK = (act.Substring(act.IndexOf('n') + 1, 1) == "0") ? true : false;
                    if (VuseK)
                    {
                        description = string.Format("闪喷工作【{0} 级强度】【单次闪喷 {1} 秒】【单次停止 {2} 秒】【循环 {3} 次】",
                        Printer.OrderNum[(int)(AppCfg.Default.V_Pow_Kvvv * AppCfg.Default.V_Pow_Rto)],
                        (AppCfg.Default.V_WrT_Kvvv * AppCfg.Default.V_WrT_Rto).ToString("#0.0"),
                        (AppCfg.Default.V_SpT_Kvvv * AppCfg.Default.V_SpT_Rto).ToString("#0.0"),
                        (AppCfg.Default.V_Cyc_K007 * AppCfg.Default.V_Cyc_Rto).ToString());
                        // for Editor form get correct data
                        Printer.V_Strength = AppCfg.Default.V_Pow_Kvvv;
                        Printer.V_WorkTime = AppCfg.Default.V_WrT_Kvvv;
                        Printer.V_HoldTime = AppCfg.Default.V_SpT_Kvvv;
                        Printer.V_CycleNum = AppCfg.Default.V_Cyc_K007;
                    }
                    else
                    {
                        string[] ValVattr = act.Substring(1).Split('p', 's', 'n');
                        int.TryParse(ValVattr[0], out Printer.V_Strength);
                        int.TryParse(ValVattr[1], out Printer.V_WorkTime);
                        int.TryParse(ValVattr[2], out Printer.V_HoldTime);
                        int.TryParse(ValVattr[3], out Printer.V_CycleNum);
                        description = string.Format("闪喷工作【{0} 级强度】【单次闪喷 {1} 秒】【单次停止 {2} 秒】【循环 {3} 次】",
                            Printer.OrderNum[(int)(Printer.V_Strength * AppCfg.Default.V_Pow_Rto)],
                            (Printer.V_WorkTime * AppCfg.Default.V_WrT_Rto).ToString("#0.0"),
                            (Printer.V_HoldTime * AppCfg.Default.V_SpT_Rto).ToString("#0.0"),
                            (Printer.V_CycleNum * AppCfg.Default.V_Cyc_Rto).ToString());
                    }
                    break;
                case "N":
                    var NuseK = (act.Substring(2, 1) == "0") ? true : false;
                    if (NuseK)
                    {
                        description = string.Format("延时【{0} 秒】", (AppCfg.Default.N_Dly_K006 * AppCfg.Default.N_Dly_Rto).ToString());
                        Printer.N_WaitTime = AppCfg.Default.N_Dly_K006;
                    }
                    else
                    {
                        int.TryParse(act.Substring(2), out Printer.N_WaitTime);
                        description = string.Format("延时【{0} 秒】", (Printer.N_WaitTime * AppCfg.Default.N_Dly_Rto).ToString());
                    }
                    break;
            }
            return description;
        }
        private void InsertPreKSet(List<string> finalProc)
        {
            string Xcmd = string.Empty; string Korder = string.Empty;

            finalProc.Insert(0, Printer.w_IndexStr);
            finalProc.Insert(0, Printer.p_IndexStr);

            #region//预先设定K值的部分--------------------------------Start
            //Delay
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.N_Dly_K006);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.N_Dly_K006.ToString()));
            //Pump_V
            finalProc.Insert(0, string.Format("@T{0};", AppCfg.Default.V_Wav_Tvvv));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.V_Frq_K074);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.V_Frq_K074.ToString()));
            finalProc.Insert(0, string.Format("@xVW{0};", AppCfg.Default.V_Wav_Tvvv));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.V_Cyc_K007);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.V_Cyc_K007.ToString()));
            finalProc.Insert(0, string.Format("@xVE{0};", AppCfg.Default.V_SpT_Kvvv));
            finalProc.Insert(0, string.Format("@xVB{0};", AppCfg.Default.V_WrT_Kvvv));
            finalProc.Insert(0, string.Format("@xVP{0};", AppCfg.Default.V_Pow_Kvvv));
            //Pump_M
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.M_Ttt_K022);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.M_Ttt_K022.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.M_Cyc_K010);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.M_Cyc_K010.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.M_SpT_K030);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.M_SpT_K030.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.M_WrT_K029);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.M_WrT_K029.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.M_Pow_K056);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.M_Pow_K056.ToString()));
            //Stage
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.C_Pos4_K059);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.C_Pos4_K059.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.C_Pos2_K012);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.C_Pos2_K012.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.C_Pos1_K026);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.C_Pos1_K026.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.C_Pos0_K999);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.C_Pos0_K999.ToString()));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.C_Vel_K008);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.C_Vel_K008.ToString()));
            //Wiper
            Xcmd = string.Empty; for (int l = 0; l < Printer.P_Counts; l++) { Xcmd += string.Format(",{0}", Printer.W_YPos[l]); }
            finalProc.Insert(0, string.Format("@xWL{0};", Xcmd));
            Korder = string.Empty; Korder = NameOf(() => AppCfg.Default.W_Vel_K055);
            finalProc.Insert(0, string.Format("@K1{0}v{1};", Korder.Substring(Korder.Length - 3, 3), AppCfg.Default.W_Vel_K055.ToString()));
            //Painter
            Xcmd = string.Empty; for (int l = 0; l < Printer.P_Counts * 2; l++) { Xcmd += string.Format(",{0}", Printer.P_XPos[l]); }
            finalProc.Insert(0, string.Format("@xPL{0};", Xcmd));
            finalProc.Insert(0, string.Format("@xPV{0};", AppCfg.Default.P_Vel_Kppp.ToString()));
            #endregion//预先设定K值的部分--------------------------------Final

            finalProc.Insert(0, string.Format(@"@B{0}{1};", Printer.P_Type, Printer.ProcessName));
        }
        private void SaveProcessFile()
        {
            saveProFileDialog.InitialDirectory = Printer.F_defaultPath;
            saveProFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveProFileDialog.DefaultExt = ".txt";
            saveProFileDialog.FileName = "";
            if (saveProFileDialog.ShowDialog() == DialogResult.OK)
            {
                //btn_Update.Visible = true;
                string ProFile = saveProFileDialog.FileName;
                List<string> allStep = new List<string>(Printer.CleanProcess);
                InsertPreKSet(allStep);//追加流程头
                allStep.Add(@"@En0;");//追加流程尾
                Printer.F_SaveProcess(allStep, ProFile);
            }
        }
        private void btn_Update_Click(object sender, EventArgs e)
        {
            ZR_Update update = new ZR_Update(false);
            update.ShowDialog();
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
                    toolStripStatusLabel2.Text = string.Format("打印机当前状态:【{0}】", (Printer.OfflineUse) ? "离线" : "在线");
                    toolStripStatusLabel2.BackColor = (Printer.OfflineUse) ? Color.LightYellow : Color.LightGreen;
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
                                        if (name == plabel.Name)
                                        {
                                            plabel.Text = string.Format("{0}#", n + 1); plabel.BackColor = Color.LightSeaGreen;
                                            Printer.M_PumpMs[n].Visible = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Printer.w_IndexStr) && !string.IsNullOrEmpty(Printer.p_IndexStr))
                                    {
                                        var rowinfo = Printer.w_IndexStr.Trim('@', 'w', '0', ';');
                                        var colinfo = Printer.p_IndexStr.Trim('@', 'x', '0', ';');
                                        var name = string.Format("label_P{0}{1}", colinfo.Substring(colinfo.Length - n - 1, 1), rowinfo.Substring(rowinfo.Length - n - 1, 1));
                                        if (name == plabel.Name)
                                        {
                                            plabel.Text = string.Format("{0}#", n + 1); plabel.BackColor = Color.LightSeaGreen;
                                            Printer.M_PumpMs[n].Visible = true;
                                        }
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
                    comBox_Position.Click += this.btn_SetAxisPara_Click;
                    btn_TestPos.Enabled = btn_microDec.Enabled = btn_microInc.Enabled = !Printer.OfflineUse;
                    break;
                case UIPages.FourthStep:
                    txtBox_SerialNum.Text = Printer.ProcessName;
                    txtBox_SerialNum.AutoCompleteCustomSource.Add(Printer.ProcessName);
                    if (Printer.CleanProcess[0].Contains("@B") || Printer.CleanProcess[Printer.CleanProcess.Count - 1].Contains("@En"))
                    {
                        var tailOfSet = Printer.CleanProcess.FindIndex(a => a.Contains("@w"));
                        Printer.CleanProcess.RemoveRange(0, tailOfSet + 1);//Now should be 25
                        Printer.CleanProcess.RemoveAt(Printer.CleanProcess.Count - 1);
                    }
                    FlushActionList(Printer.CleanProcess);
                    break;
            }
        }
        bool Unvalid = false;
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
                    Printer.P_AnalyzeArray(ref Unvalid);
                    if (Unvalid) { MessageBox.Show("喷头编号不连续或者缺失!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }
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

        #region Connect Printer and Diagnose
        private void ConnectAndDiagnose()
        {
            ZS_SetNet netSet = new ZS_SetNet();
            string PrtIP = Printer.F_ReadINI("Channel0Param", "DestIp", "", ZS_SetNet.INIFile);
            string LocIP = Printer.F_ReadINI("Channel0Param", "LocalIp", "", ZS_SetNet.INIFile);
            int cmdPort = int.Parse(Printer.F_ReadINI("Channel0Param", "CmdPort", "", ZS_SetNet.INIFile));
            int datPort = int.Parse(Printer.F_ReadINI("Channel0Param", "DataPort", "", ZS_SetNet.INIFile));

            //var stat = InitInterface(netSet.Handle, 0);
            int milsec = 5000; //TimeOut for Ping the Printer
            var stat = Printer.Net_PingIsOK(PrtIP, milsec);//, milsec
            if (!stat)
            {
                if (DialogResult.Yes == MessageBox.Show("连接打印机失败,选择【是】前往设置网络,选择【否】则脱机使用该工具", "连接失败", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    netSet.ShowDialog();
                else Printer.OfflineUse = true;
            }

            if (!Printer.OfflineUse)
            {
                //Create local socket
                try
                {
                    cmdSKT = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    cmdEnd = new IPEndPoint(IPAddress.Parse(LocIP), cmdPort); cmdSKT.Bind(cmdEnd);
                    Thread tc = new Thread(cmdMSGBack);
                    tc.IsBackground = true;
                    tc.Start();

                    datSKT = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    datEnd = new IPEndPoint(IPAddress.Parse(LocIP), datPort); datSKT.Bind(datEnd);
                    Thread td = new Thread(datMSGBack);
                    td.IsBackground = true;
                    td.Start();
                }
                catch (SocketException se)
                {
                    switch (se.ErrorCode)
                    {
                        case 0x2741:
                            if (DialogResult.Yes == MessageBox.Show("本地IP与配置文件设定值不一致,选择【是】重新设置本地IP,选择【否】则脱机使用该工具", "连接失败", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                                netSet.ShowDialog();
                            else
                            {
                                Printer.OfflineUse = true;
                            }
                            break;
                        case 0x2740:
                            if (DialogResult.Yes == MessageBox.Show("端口9000或9001被占用，选择【是】脱机使用该工具,选择【否】退出程序", "提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                                Printer.OfflineUse = true;
                            else
                            {
                                Environment.Exit(1);
                            }
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show("UnKnow NetWork Error");
                }
            }
        }
        #endregion

        //NetWork and printer Interface
        #region NetWork Set & Printer Interface
        //NetWork
        public bool PrtReady = true;
        //Printer API
        [DllImport("printer_interface", EntryPoint = "InitInterface")]
        private static extern int InitInterface(IntPtr handle, uint Msg);
        [DllImport("printer_interface", EntryPoint = "CloseInterface")]
        private static extern int CloseInterface();

        [DllImport("printer_interface", EntryPoint = "StartTransferData")]
        private static extern bool StartTransferData();
        [DllImport("printer_interface", EntryPoint = "SendData")]
        private static extern int SendData(byte[] pData, long nLength, int nPortIndex);
        [DllImport("printer_interface", EntryPoint = "EndTransferData")]
        private static extern bool EndTransferData();

        [DllImport("printer_interface", EntryPoint = "RequestUpgrade")]
        private static extern bool RequestUpgrade(int nTarget, ref byte[] szRequestRst, ref int nBuffLen);

        public bool EnDBG = true;
        private bool SetDebugClean(bool enable)
        {
            //byte[] CmdStr = new byte[1024];
            List<byte> CmdLst = new List<byte>();
            //Length
            byte[] Length = new byte[] { 0x06, 0x00 };
            CmdLst.AddRange(Length);
            //CmdInfo
            byte[] CmdInf = new byte[] { 0x32, 0x62 };
            CmdLst.AddRange(CmdInf);
            //Data
            byte onoff = (byte)(enable ? 0x01 : 0x00);
            CmdLst.Add(onoff);
            CmdLst.Add(Printer.M_OnOff);
            //CRC
            byte[] CRCrst = Printer.Net_CRC16(CmdLst);
            CmdLst.AddRange(CRCrst);
            //SOH EOT ESC
            CmdLst = EscapeSpecial(CmdLst); CmdLst.Insert(0, SOH); CmdLst.Add(EOT);
            CmdLst.InsertRange(0, new byte[] { 0xCD, 0, 0, 0 });

            //******Send & Receive
            cmdMSGSend(CmdLst.ToArray());
            Thread.Sleep(100);

            /*
            List<byte> NoFPGABack = new List<byte>();
            if ((cmdMSG[0] == 0xCD) && (cmdMSG[1] == 0xFF))
            {
                for (int i = cmdMSG.Length / 2; i < cmdMSG.Length; i++)
                {
                    NoFPGABack.Add(cmdMSG[i]);
                }
            }
            else
            {
                for (int i = 0; i < cmdMSG.Length; i++)
                {
                    NoFPGABack.Add(cmdMSG[i]);
                }
            }

            while (NoFPGABack.FindIndex(a => (a != 0)) < 0) Thread.Sleep(0);
            */

            int again = 0;
            while (again < 3)
            {
                if (new List<byte>(cmdMSG).FindIndex(a => (a != 0)) < 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("未收到设备回复,是否再次设定调试状态?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        cmdMSGSend(CmdLst.ToArray());
                        Thread.Sleep(100);
                    }
                    else
                        break;
                }
                again++;
            }
            if (new List<byte>(cmdMSG).FindIndex(a => (a != 0)) < 0) return false;

            //extract the data
            List<byte> rsv = new List<byte>(cmdMSG);
            //rsv.RemoveRange(0, 4);
            int index = rsv.FindIndex(a => (a == EOT));
            rsv.RemoveRange(index, rsv.Count - index);
            rsv.RemoveRange(0, 5);
            var databytes = DecodeSpecial(rsv);
            Array.Clear(cmdMSG, 0, cmdMSG.Length);

            //******Analyze data
            databytes.RemoveRange(0, 4);
            databytes.RemoveAt(databytes.Count - 1);
            databytes.RemoveAt(databytes.Count - 1);
            List<byte> dataBack = new List<byte>(databytes);

            //return true; //For Debug
            if (dataBack.Count != 1) return false;
            else
            {
                return (dataBack[0] == 0x01) ? true : false;
            }
        }
        private bool SendStepCmd(string sglStep)
        {
            // PreHandling for data
            byte[] OrgStep = Encoding.ASCII.GetBytes(sglStep);
            List<byte> CmdLst = new List<byte>();
            //Length
            byte LenL = Convert.ToByte(0xFF & OrgStep.Length); CmdLst.Add(LenL);
            byte LenH = Convert.ToByte(0xFF & (OrgStep.Length >> 8)); CmdLst.Add(LenH);
            //CmdInfo
            byte[] CmdInf = new byte[] { 0x32, 0x63 };
            CmdLst.AddRange(CmdInf);
            //Data
            CmdLst.AddRange(OrgStep);
            //CRC
            byte[] CRCrst = Printer.Net_CRC16(CmdLst);
            CmdLst.AddRange(CRCrst);
            //SOH EOT ESC
            CmdLst = EscapeSpecial(CmdLst); CmdLst.Insert(0, SOH); CmdLst.Add(EOT);
            CmdLst.InsertRange(0, new byte[] { 0xCD, 0, 0, 0 });

            //******Send & Receive
            cmdMSGSend(CmdLst.ToArray());
            Thread.Sleep(100);

            /*
            List<byte> NoFPGABack = new List<byte>();
            if ((cmdMSG[0] == 0xCD) && (cmdMSG[1] == 0xFF))
            {
                for (int i = cmdMSG.Length / 2; i < cmdMSG.Length; i++)
                {
                    NoFPGABack.Add(cmdMSG[i]);
                }
            }
            else
            {
                for (int i = 0; i < cmdMSG.Length; i++)
                {
                    NoFPGABack.Add(cmdMSG[i]);
                }
            }

            while (NoFPGABack.FindIndex(a => (a != 0)) < 0) Thread.Sleep(0);
            */

            int again = 0;
            while (again < 3)
            {
                if (new List<byte>(cmdMSG).FindIndex(a => (a != 0)) < 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("未收到设备回复,是否再次发送指令?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        cmdMSGSend(CmdLst.ToArray());
                        Thread.Sleep(100);
                    }
                    else
                        break;
                }
                again++;
            }
            if (new List<byte>(cmdMSG).FindIndex(a => (a != 0)) < 0) return false;

            //extract the data
            List<byte> rsv = new List<byte>(cmdMSG);
            int index = rsv.FindIndex(a => (a == EOT));
            rsv.RemoveRange(index, rsv.Count - index);
            rsv.RemoveRange(0, 5);
            var databytes = DecodeSpecial(rsv);
            Array.Clear(cmdMSG, 0, cmdMSG.Length);

            //******Analyze data
            databytes.RemoveRange(0, 4);
            databytes.RemoveAt(databytes.Count - 1);
            databytes.RemoveAt(databytes.Count - 1);
            List<byte> dataBack = new List<byte>(databytes);

            //return true; //For Debug
            if (dataBack.Count != 1) return false;
            else
            {
                return (dataBack[0] == 0x01) ? true : false;
            }
            //else
            //{
            //    if (dataBack[0] == 0x00) return false;
            //    else if (dataBack[0] == 0x01)
            //    {
            //        DateTime st = DateTime.Now;
            //        while (true)
            //        {
            //            DateTime et = DateTime.Now;
            //            TimeSpan ts = et - st;
            //            if (ts.Seconds <= 20)
            //            {
            //                if (cmdMSG.Length > 0)
            //                {
            //                    rsv = new List<byte>(cmdMSG);
            //                    index = rsv.FindIndex(a => (a == EOT));
            //                    rsv.RemoveRange(index, rsv.Count - index);
            //                    rsv.RemoveAt(0);
            //                    databytes = DecodeSpecial(rsv);
            //                    Array.Clear(cmdMSG, 0, cmdMSG.Length);

            //                    databytes.RemoveRange(0, 4);
            //                    databytes.RemoveAt(databytes.Count - 1);
            //                    databytes.RemoveAt(databytes.Count - 1);
            //                    dataBack = new List<byte>(databytes);

            //                    if (dataBack[0] == 0x02) return true;
            //                    else return false;
            //                }
            //                Thread.Sleep(100);
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //    }
            //    return false;
            //    //return (dataBack[0] == 0x01) ? true : false;
            //}
        }
        private bool SetSingleKVal(int Knum, int Kval)
        {
            List<byte> CmdLst = new List<byte>();
            //Length
            byte[] Length = new byte[] { 0x07, 0x00 };
            CmdLst.AddRange(Length);
            //CmdInfo
            byte[] CmdInf = new byte[] { 0x31, 0x34 };
            CmdLst.AddRange(CmdInf);
            //Data
            ushort knum = (ushort)(Knum & 0x00FF);
            CmdLst.Add((byte)(knum & 0xFF));
            CmdLst.Add((byte)((knum >> 8) & 0xFF));
            CmdLst.Add((byte)(Kval & 0xFF));
            //CRC
            byte[] CRCrst = Printer.Net_CRC16(CmdLst);
            CmdLst.AddRange(CRCrst);
            //SOH EOT ESC
            CmdLst = EscapeSpecial(CmdLst); CmdLst.Insert(0, SOH); CmdLst.Add(EOT);
            CmdLst.InsertRange(0, new byte[] { 0xCD, 0, 0, 0 });

            //******Send & Receive
            cmdMSGSend(CmdLst.ToArray());
            Thread.Sleep(100);

            int again = 0;
            while (again < 3)
            {
                if (new List<byte>(cmdMSG).FindIndex(a => (a != 0)) < 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("写入K值失败,是否再次发送指令?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        cmdMSGSend(CmdLst.ToArray());
                        Thread.Sleep(100);
                    }
                    else
                        break;
                }
                again++;
            }
            if (new List<byte>(cmdMSG).FindIndex(a => (a != 0)) < 0) return false;

            //extract the data
            List<byte> rsv = new List<byte>(cmdMSG);
            int index = rsv.FindIndex(a => (a == EOT));
            rsv.RemoveRange(index, rsv.Count - index);
            rsv.RemoveRange(0, 5);
            var databytes = DecodeSpecial(rsv);
            Array.Clear(cmdMSG, 0, cmdMSG.Length);

            //******Analyze data
            databytes.RemoveRange(0, 4);
            databytes.RemoveAt(databytes.Count - 1);
            databytes.RemoveAt(databytes.Count - 1);
            List<byte> dataBack = new List<byte>(databytes);

            if (dataBack.Count != 1) return false;
            else
            {
                return (dataBack[0] == 0x01) ? true : false;
            }
        }
        private List<byte> EscapeSpecial(List<byte> bytelist)
        {
            List<byte> afterConvert = new List<byte>(bytelist);
            int cnt = 0;
            foreach (byte bt in bytelist)
            {
                switch (bt)
                {
                    case SOH:
                        afterConvert[cnt] = 0x1B;
                        afterConvert.Insert(cnt + 1, 0x81);
                        cnt++;
                        break;
                    case EOT:
                        afterConvert[cnt] = 0x1B;
                        afterConvert.Insert(cnt + 1, 0x84);
                        cnt++;
                        break;
                    case ESC:
                        afterConvert[cnt] = 0x1B;
                        afterConvert.Insert(cnt + 1, 0x9B);
                        cnt++;
                        break;
                    default:
                        break;
                }
                cnt++;
            }
            return afterConvert;
        }
        private List<byte> DecodeSpecial(List<byte> bytelist)
        {
            //ushort soh = 0x1B81, eot = 0x1B84, esc = 0x1B9B;
            List<byte> rst = new List<byte>();

            for (int i = 0; i < bytelist.Count; i++)
            {
                if (bytelist[i] != ESC) rst.Add(bytelist[i]);
                else if ((i + 1) > bytelist.Count && bytelist[i + 1] != 0x81 && bytelist[i + 1] != 0x84 && bytelist[i + 1] != 0x9B) rst.Add(bytelist[i]);
                else
                {
                    if (bytelist[i + 1] == 0x81) { bytelist.RemoveAt(i + 1); rst.Add(SOH); }
                    else if (bytelist[i + 1] == 0x84) { bytelist.RemoveAt(i + 1); rst.Add(EOT); }
                    else { bytelist.RemoveAt(i + 1); rst.Add(ESC); }
                }
            }
            return rst;
        }
        #endregion

        #region Net UDP communication
        private void datMSGBack(object obj)
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                int length = datSKT.ReceiveFrom(datMSG, ref point);
            }
        }
        private void cmdMSGBack(object obj)
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                int length = cmdSKT.ReceiveFrom(cmdMSG, ref point);
            }
        }
        private void datMSGSend(byte[] dat)
        {
            string PrtIP = Printer.F_ReadINI("Channel0Param", "DestIp", "", ZS_SetNet.INIFile);
            int datPort = int.Parse(Printer.F_ReadINI("Channel0Param", "DataPort", "", ZS_SetNet.INIFile));
            EndPoint point = new IPEndPoint(IPAddress.Parse(PrtIP), datPort);

            datSKT.SendTo(dat, point);
        }
        private void cmdMSGSend(byte[] cmd)
        {
            string PrtIP = Printer.F_ReadINI("Channel0Param", "DestIp", "", ZS_SetNet.INIFile);
            int cmdPort = int.Parse(Printer.F_ReadINI("Channel0Param", "CmdPort", "", ZS_SetNet.INIFile));
            EndPoint point = new IPEndPoint(IPAddress.Parse(PrtIP), cmdPort);

            cmdSKT.SendTo(cmd, point);
        }
        #endregion

        private void FormRoot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("结束本次编辑?", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))//不玩了吗
            {
                if (EnDBG && !Printer.OfflineUse) SetDebugClean(false); // Exit Debug Mode
                AppCfg.Default.Save();
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

    }
}