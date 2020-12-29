using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CleanProApp
{
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;
    using AppCfg = CleanProApp.Properties.Settings;
    public class Xprinter
    {
        //Public
        #region Public
        public Xprinter()
        {
            var exePath = Environment.CurrentDirectory;
            var folderName = F_defaultPath.Substring(exePath.Length + 1, 8);
            string[] existings = Directory.GetFileSystemEntries(exePath);
            if (!existings.Contains(folderName))
            {
                DirectoryInfo info = new DirectoryInfo(exePath);
                info.CreateSubdirectory(folderName);
            }

            for (int p = 0; p < P_XPos.Capacity; p++) P_XPos.Add(0);
            for (int w = 0; w < W_YPos.Capacity; w++) W_YPos.Add(0);
            for (int c = 0; c < C_Level.Capacity; c++) C_Level.Add(0);

            //string Rst = ""; IntPtr RstLen = IntPtr.Zero;
            //bool isOK = RequestUpgrade(0, Rst, RstLen);
        }
        public List<Icon> IconRes;
        public List<Image> ImageList;
        public List<int> CleanSequence;
        public List<string> CleanProcess;
        public bool WipeInXdirect = true;
        public bool IsInModifying = false;
        public FormRoot.CleanMode CleanMode;
        public bool OfflineUse = false;
        public bool mustReStart = false;
        public string ProcessName = DateTime.Now.ToString("yyMMdd");
        public string[] ParaLabel = { "运转强度", "运转时间", "停止时间", "循环次数", "延时时长" };
        public string[] OrderNum = { "全功率运转", "Ⅰ", "Ⅱ", "Ⅲ", "Ⅳ", "Ⅴ", "Ⅵ", "Ⅶ", "Ⅷ", "Ⅸ" };
        #endregion

        //Painter
        #region Painter
        public string p_IndexStr;
        public string P_Type = "XHS";
        public int P_Counts = 1;
        public int P_Speed = AppCfg.Default.P_Vel_Min;//(0-100)
        public List<int> P_XPos = new List<int>(9 * 9 * 2);//(0-1000000)
        public List<Label> P_Array = new List<Label>();
        public void P_AnalyzeArray(ref bool UnValid)
        {
            //Extrapolated index string
            List<Label> lblarr = new List<Label>();
            foreach (Label lbl in P_Array)
            {
                if (!string.IsNullOrEmpty(lbl.Text))
                {
                    lblarr.Add(lbl);
                }
            }
            if (lblarr.Count <= 0)
            {
                //MessageBox.Show("当前没有喷头!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                UnValid = true; return;
            }
            else UnValid = false;
            List<ushort> txtNum = new List<ushort>();
            for (int i = 0; i < lblarr.Count; i++)
            {
                ushort tmp = 0;
                ushort.TryParse(lblarr[i].Text.Substring(0, 1), out tmp);
                txtNum.Add(tmp);
            }
            //var maxNum = txtNum.Max();
            UnValid = (txtNum.Count < txtNum.Max()) ? true : false;
            if (UnValid) return;

            P_Counts = (int)lblarr.Count;
            char[] ptarr = new char[lblarr.Count];
            char[] ptarr2 = new char[lblarr.Count];
            for (int i = 0; i < lblarr.Count; i++)
            {
                foreach (Label lbl in lblarr)
                {
                    if (int.Parse(lbl.Text.Substring(0, 1)) == (i + 1))
                    {
                        if (W_Enable || WC_Enable) ptarr[lblarr.Count - (1 + i)] = lbl.Name.Replace("label_P", "").Substring(0, 1)[0];
                        else ptarr[lblarr.Count - (1 + i)] = lbl.Name.Replace("label_P", "").Substring(1, 1)[0];
                    }
                    if (int.Parse(lbl.Text.Substring(0, 1)) == (i + 1))
                    {
                        if (W_Enable || WC_Enable) ptarr2[lblarr.Count - (1 + i)] = lbl.Name.Replace("label_P", "").Substring(1, 1)[0];
                        else ptarr2[lblarr.Count - (1 + i)] = lbl.Name.Replace("label_P", "").Substring(0, 1)[0];
                    }
                }
            }
            string str = new string(ptarr);
            if (str.Length < 9) w_IndexStr = @"@w" + str.PadLeft(9, '0') + @";";
            else w_IndexStr = @"@w" + str + @";";

            string str2 = new string(ptarr2);
            if (str2.Length < 9) p_IndexStr = @"@x" + str2.PadLeft(9, '0') + @";";
            else p_IndexStr = @"@x" + str + @";";

            //Estimated cleaning sequence
            List<int> sqc = new List<int>();
            for (int c = 0; c < FormRoot.P_ArrCol; c++)
            {
                for (int r = 0; r < FormRoot.P_ArrRow; r++)
                {
                    var id = c + r * FormRoot.P_ArrRow;
                    if ((id < FormRoot.P_ArrRow * FormRoot.P_ArrCol) && (!string.IsNullOrEmpty(P_Array[id].Text)))
                        sqc.Add(int.Parse(P_Array[id].Text.Trim('#')));
                }
            }
            CleanSequence = sqc;

            //Estimated PumpM enable
            byte ms_byte = 0x00; byte cl_byte = 0x01;
            for (int i = 0; i < M_PumpMs.Count; i++)
            {
                if (M_PumpMs[i].Visible == true && M_PumpMs[i].Checked == true)
                {
                    int tmpInt = cl_byte << i;
                    byte tmpByte = Convert.ToByte(tmpInt & 0xFF);
                    ms_byte = Convert.ToByte(ms_byte | tmpByte);
                }
            }
            M_OnOff = ms_byte;
        }
        #endregion

        //Wiper
        #region Wiper
        public string w_IndexStr;
        public bool W_Enable = true, WC_Enable = false;
        public int W_Speed = AppCfg.Default.W_Vel_Min;
        public List<int> W_YPos = new List<int>(9 * 9 * 2);//(0-10)
        #endregion

        //Stage
        #region Stage
        public int C_Speed = AppCfg.Default.C_Vel_Min;
        public string[] C_LevelMark = { "吸墨", "刮墨", "打印", "原点", "闪喷" };
        public List<int> C_Level = new List<int>(5);//(0-9)
        #endregion

        //PumpM
        #region PumpM
        public bool M_OnlyTime = false;
        public int M_OnlyWorkTime = AppCfg.Default.M_Ttt_Min;
        public int M_Strength = AppCfg.Default.M_Pow_Min;
        public int M_WorkTime = AppCfg.Default.M_WrT_Min;//(0-1000, 999, 1000)
        public int M_HoldTime = AppCfg.Default.M_SpT_Min;
        public int M_CycleNum = AppCfg.Default.M_Cyc_Min;
        public byte M_OnOff = 0x00;
        public List<CheckBox> M_PumpMs = new List<CheckBox>();
        #endregion

        //PumpS
        #region PumpS
        public int V_Strength = AppCfg.Default.V_Pow_Min;
        public int V_WorkTime = AppCfg.Default.V_WrT_Min;
        public int V_HoldTime = AppCfg.Default.V_SpT_Min;
        public int V_CycleNum = AppCfg.Default.V_Cyc_Min;
        #endregion

        //Delay
        #region Delay
        public int N_WaitTime = AppCfg.Default.N_Dly_Min;//(0, 1-1000)
        #endregion

        //Actual Process Document Info
        #region Document
        //public string F_defaultFolder = "喷头清洗流程文件";
        public string F_ProFile = "";
        public string F_defaultPath = Environment.CurrentDirectory + "\\喷头清洗流程文件";
        public void F_SaveProcess(List<string> actions, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < actions.Count; i++) sw.WriteLine(actions[i]);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        public List<string> F_ReadProcess(string fileName)
        {
            List<string> actions = new List<string>();
            if (string.IsNullOrEmpty(fileName)) return actions;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string tmp = sr.ReadLine();
            while (null != tmp)
            {
                actions.Add(tmp);
                tmp = sr.ReadLine();
            }
            sr.Close();
            fs.Close();

            return actions;
        }
        public string F_CleanTxtCVRT_Dat(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            List<string> txtFile = new List<string>();
            //DAT Head
            string CleanPrefix = "P0001".PadRight(8, '\0');
            string PrcName = Path.GetFileNameWithoutExtension(fileName).PadRight(64, '\0');
            int PrcLen = 0;
            txtFile = F_ReadProcess(fileName);
            for (int n = 0; n < txtFile.Count; n++)
            {
                txtFile[n] += "\r\n";
            }
            foreach (string step in txtFile)
            {
                PrcLen += step.Length;
            }
            string PrcLength = PrcLen.ToString().PadRight(8, '\0');
            string EndExt = "E1000".PadRight(80, '\0');
            string DATHead = (CleanPrefix + PrcName + PrcLength + EndExt).PadRight(1600, '\0');
            //DAT Body
            var remain = PrcLen % 32; int more = 0;
            if (remain != 0) { more = 32 - remain; }
            string DATBody = PrcName;
            foreach (string step in txtFile) DATBody += step;
            for (int i = 0; i < more; i++) DATBody += '\0';

            return DATHead + DATBody;
        }
        public bool F_SendDatToPrt(string fdata, ref bool BoardEx)
        {
            int datPort = int.Parse(F_ReadINI("Channel0Param", "DataPort", "", ZS_SetNet.INIFile));
            if (InitInterface(IntPtr.Zero, 0) == 0) return false;
            byte[] buff = new byte[6]; int bufflen = buff.Length;
            while ((buff[4] != 'U' || buff[5] != 'P'))
            {
                RequestUpgrade(1, buff, ref bufflen);
                Thread.Sleep(100);
                if ((buff[4] != 'U' && buff[5] != 'S'))
                { BoardEx = true; return false; }
            }
            //if (!RequestUpgrade(1, buff, ref bufflen)) return false;
            Thread.Sleep(100);
            if (!StartTransferData()) return false;
            Thread.Sleep(100);
            byte[] data = { 0xAA, 0xAA, 0xAA, 0xAA };

            int rst = SendData(data, data.Length, datPort);
            if (rst == 0) return false;
            for (int i = 0; i < 3; i++)
            {
                if (rst == 2) rst = SendData(data, data.Length, datPort);
                Thread.Sleep(100);
            }
            if (rst == 0) return false;

            byte[] fdatabyts = Encoding.ASCII.GetBytes(fdata);
            int frst = SendData(fdatabyts, fdatabyts.Length, datPort);
            if (frst == 0) return false;
            for (int i = 0; i < 3; i++)
            {
                if (frst == 2) rst = SendData(fdatabyts, fdatabyts.Length, datPort);
                Thread.Sleep(100);
            }
            if (frst == 0) return false;

            EndTransferData();
            CloseInterface();
            return true;
        }
        public bool F_AnalyzeProcess(List<string> actions)
        {
            bool valid1 = false;
            foreach (string act in actions)
            {
                if (act.Contains("@B")) valid1 = true;
            }
            bool valid2 = false;
            foreach (string act in actions)
            {
                if (act.Contains("@w")) valid2 = true;
            }
            bool valid3 = false;
            foreach (string act in actions)
            {
                if (act.Contains("@x")) valid3 = true;
            }
            if (!valid1 || !valid2 || !valid3) return false;

            //int pn = 0;
            foreach (string act in actions)
            {
                if (string.IsNullOrEmpty(act)) continue;
                string detail = act.Trim('@', ';');
                switch (detail.Substring(0, 1))
                {
                    case "B":
                        P_Type = detail.Substring(1, 1);
                        ProcessName = detail.Substring(2);
                        break;
                    case "x":
                        if (detail.Substring(1, 2) == "PV") AppCfg.Default.P_Vel_Kppp = int.Parse(detail.Substring(3));
                        else if (detail.Substring(1, 2) == "PL")
                        {
                            if (detail.Length < 4) continue;
                            string[] ploc = detail.Substring(4).Split(','); int ppos = 0;
                            for (int i = 0; i < ploc.Length; i++)
                            {
                                int.TryParse(ploc[i], out ppos);
                                P_XPos[i] = ppos;
                            }
                        }
                        else if (detail.Substring(1, 2) == "WL")
                        {
                            if (detail.Length < 4) continue;
                            string[] wloc = detail.Substring(4).Split(','); int wpos = 0;
                            for (int i = 0; i < wloc.Length; i++)
                            {
                                int.TryParse(wloc[i], out wpos);
                                W_YPos[i] = wpos;
                            }
                        }
                        else if (detail.Substring(1, 2) == "VP") AppCfg.Default.V_Pow_Kvvv = int.Parse(detail.Substring(3));
                        else if (detail.Substring(1, 2) == "VB") AppCfg.Default.V_WrT_Kvvv = int.Parse(detail.Substring(3));
                        else if (detail.Substring(1, 2) == "VE") AppCfg.Default.V_SpT_Kvvv = int.Parse(detail.Substring(3));
                        else if (detail.Substring(1, 2) == "VW") AppCfg.Default.V_Wav_Tvvv = int.Parse(detail.Substring(3));
                        else { p_IndexStr = act; }
                        break;
                    case "K":
                        var Kxxx = detail.Substring(2, 3);
                        if (FormRoot.NameOf(() => AppCfg.Default.W_Vel_K055).Substring(FormRoot.NameOf(() => AppCfg.Default.W_Vel_K055).Length - 3, 3) == Kxxx)
                            AppCfg.Default.W_Vel_K055 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.C_Vel_K008).Substring(FormRoot.NameOf(() => AppCfg.Default.C_Vel_K008).Length - 3, 3) == Kxxx)
                            AppCfg.Default.C_Vel_K008 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.C_Pos0_K999).Substring(FormRoot.NameOf(() => AppCfg.Default.C_Pos0_K999).Length - 3, 3) == Kxxx)
                            AppCfg.Default.C_Pos0_K999 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.C_Pos1_K026).Substring(FormRoot.NameOf(() => AppCfg.Default.C_Pos1_K026).Length - 3, 3) == Kxxx)
                            AppCfg.Default.C_Pos1_K026 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.C_Pos2_K012).Substring(FormRoot.NameOf(() => AppCfg.Default.C_Pos2_K012).Length - 3, 3) == Kxxx)
                            AppCfg.Default.C_Pos2_K012 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.C_Pos4_K059).Substring(FormRoot.NameOf(() => AppCfg.Default.C_Pos4_K059).Length - 3, 3) == Kxxx)
                            AppCfg.Default.C_Pos4_K059 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.M_Pow_K056).Substring(FormRoot.NameOf(() => AppCfg.Default.M_Pow_K056).Length - 3, 3) == Kxxx)
                            AppCfg.Default.M_Pow_K056 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.M_WrT_K029).Substring(FormRoot.NameOf(() => AppCfg.Default.M_WrT_K029).Length - 3, 3) == Kxxx)
                            AppCfg.Default.M_WrT_K029 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.M_SpT_K030).Substring(FormRoot.NameOf(() => AppCfg.Default.M_SpT_K030).Length - 3, 3) == Kxxx)
                            AppCfg.Default.M_SpT_K030 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.M_Cyc_K010).Substring(FormRoot.NameOf(() => AppCfg.Default.M_Cyc_K010).Length - 3, 3) == Kxxx)
                            AppCfg.Default.M_Cyc_K010 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.M_Ttt_K022).Substring(FormRoot.NameOf(() => AppCfg.Default.M_Ttt_K022).Length - 3, 3) == Kxxx)
                            AppCfg.Default.M_Ttt_K022 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.V_Cyc_K007).Substring(FormRoot.NameOf(() => AppCfg.Default.V_Cyc_K007).Length - 3, 3) == Kxxx)
                            AppCfg.Default.V_Cyc_K007 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.V_Frq_K074).Substring(FormRoot.NameOf(() => AppCfg.Default.V_Frq_K074).Length - 3, 3) == Kxxx)
                            AppCfg.Default.V_Frq_K074 = int.Parse(detail.Substring(6));
                        else if (FormRoot.NameOf(() => AppCfg.Default.N_Dly_K006).Substring(FormRoot.NameOf(() => AppCfg.Default.N_Dly_K006).Length - 3, 3) == Kxxx)
                            AppCfg.Default.N_Dly_K006 = int.Parse(detail.Substring(6));
                        else { ;}
                        break;
                    case "w":
                        P_Counts = detail.Trim('w', '0').Length;
                        w_IndexStr = act;
                        break;
                    case "P":
                    //上述分析已经包含
                    case "W":
                    //上述分析已经包含
                    case "C":
                    //上述分析已经包含
                    case "M":
                    //上述分析已经包含
                    case "V":
                    //上述分析已经包含
                    case "N":
                    //上述分析已经包含
                    case "E":
                    //无可用信息
                    default:
                        break;
                }
            }
            return true;
        }
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        public string F_ReadINI(string sec, string key, string def, string InINmame)
        {
            StringBuilder strbd = new StringBuilder(1024);
            GetPrivateProfileString(sec, key, def, strbd, 1024, InINmame);
            return strbd.ToString();
        }
        public int F_WriteINI(string sec, string key, string value, string InIName)
        {
            if (File.Exists(InIName))
            {
                return WritePrivateProfileString(sec, key, value, InIName);
            }
            return 0;
        }
        #endregion

        //NetWork and printer Interface
        #region NetWork Set & Printer Interface
        public bool Net_PingIsOK(string Ip, int time)//ms , int time
        {
            if (time == 0) return false;
            else
            {
                Ping send = new Ping();
                PingReply recv = send.Send(Ip, time);//, time
                //MessageBox.Show(recv.Status.ToString());
                bool rsv = (recv.Status == IPStatus.Success) ? true : false;
                send.Dispose();

                return rsv;
            }
        }
        public byte[] Net_CRC16(List<byte> bylist)
        {
            byte[] CRCsum = new byte[] { 0, 0 };
            if (bylist.Count > 0)
            {
                UInt16 wSum = 0;
                foreach (byte bt in bylist)
                {
                    wSum += bt;
                }
                CRCsum = new byte[] { (byte)(wSum & 0x00FF), (byte)((wSum & 0xFF00) >> 8) };
            }
            return CRCsum;
        }
        public string Net_ByteToString(byte[] arr, bool isReverse)
        {   //2bytes
            if (arr.Length == 1)
            {
                byte bt = arr[0];
                return Convert.ToString(bt).ToUpper().PadLeft(2, '0');
            }
            else
            {
                try
                {
                    byte hi = arr[0], lo = arr[1];
                    return Convert.ToString(isReverse ? hi + lo * 0x100 : hi * 0x100 + lo, 16).ToUpper().PadLeft(4, '0');
                }
                catch (Exception ex) { throw (ex); }
            }
        }
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
        private static extern bool RequestUpgrade(int nTarget, byte[] szRequestRst, ref int nBuffLen);

        public bool EnDBG = true;
        private bool SetDebugClean(bool enable)
        {
            return true;
        }
        private bool SendStepCmd(byte[] sglStep)
        {
            return true;
        }
        #endregion
    }
}
