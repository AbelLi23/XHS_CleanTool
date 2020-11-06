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
        #endregion

        //Painter
        #region Painter
        public string p_IndexStr;
        public string P_Type = " ";
        public int P_Counts = 1;
        public int P_Speed = 0;//(0-100)
        public List<int> P_XPos = new List<int>(9 * 9 * 2);//(0-1000000)
        public List<Label> P_Array = new List<Label>();
        public void P_AnalyzeArray()
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
        }
        #endregion

        //Wiper
        #region Wiper
        public string w_IndexStr;
        public bool W_Enable = true, WC_Enable = false;
        public int W_Speed = 0;
        public List<int> W_YPos = new List<int>(9 * 9 * 2);//(0-10)
        #endregion

        //Stage
        #region Stage
        public int C_Speed = 0;
        public string[] C_LevelMark = { "吸墨", "刮墨", "打印", "原点", "闪喷" };
        public List<int> C_Level = new List<int>(5);//(0-9)
        #endregion

        //PumpM
        #region PumpM
        public bool M_OnlyTime = false;
        public int M_OnlyWorkTime = AppCfg.Default.M_Ttt_Min;
        public int M_Strength = 1;
        public int M_WorkTime = AppCfg.Default.M_WrT_Min;//(0-1000, 999, 1000)
        public int M_HoldTime;
        public int M_CycleNum = 1;
        public byte M_OnOff = 0x04;
        #endregion

        //PumpS
        #region PumpS
        public int V_Strength = 1;
        public int V_WorkTime;
        public int V_HoldTime;
        public int V_CycleNum = 1;
        #endregion

        //Delay
        #region Delay
        public int N_WaitTime = 1;//(0, 1-1000)
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
        public bool F_SendDatToPrt(string fileName)
        {
            return false;
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
                    case "K":
                        var Kxxx = int.Parse(detail.Substring(2, 3));
                        switch (Kxxx)
                        {
                            case 6:
                                AppCfg.Default.N_Dly_K06 = int.Parse(detail.Substring(5));
                                break;
                            case 7:
                                AppCfg.Default.V_Cyc_Kvv = int.Parse(detail.Substring(5));
                                break;
                            case 8:
                                AppCfg.Default.C_Vel_K08 = int.Parse(detail.Substring(5));
                                break;
                            case 10:
                                AppCfg.Default.M_Cyc_K10 = int.Parse(detail.Substring(5));
                                break;
                            case 12:
                                AppCfg.Default.C_Pos2_K12 = int.Parse(detail.Substring(5));
                                break;
                            case 22:
                                AppCfg.Default.M_Ttt_K22 = int.Parse(detail.Substring(5));
                                break;
                            case 26:
                                AppCfg.Default.C_Pos1_K26 = int.Parse(detail.Substring(5));
                                break;
                            case 30:
                                AppCfg.Default.M_SpT_K30 = int.Parse(detail.Substring(5));
                                break;
                            case 55:
                                AppCfg.Default.W_Vel_K55 = int.Parse(detail.Substring(5));
                                break;
                            case 56:
                                AppCfg.Default.M_Pow_K56 = int.Parse(detail.Substring(5));
                                break;
                            case 59:
                                AppCfg.Default.C_Pos4_K59 = int.Parse(detail.Substring(5));
                                break;
                            case 74:
                                AppCfg.Default.V_WrT_Kvv = int.Parse(detail.Substring(5));
                                break;
                            case 0:
                                break;
                        }
                        break;
                    case "w":
                        P_Counts = detail.Trim('w', '0').Length;
                        w_IndexStr = act;
                        break;
                    case "x":
                        p_IndexStr = act;
                        break;
                    case "P":
                        if ("p" == detail.Substring(1, 1))
                        {
                            int.TryParse(detail.Substring(2), out P_Speed);
                            AppCfg.Default.P_Vel_Kpp = P_Speed;
                        }
                        else if ("0" == detail.Substring(1, 1))
                        {
                            ;
                        }
                        else
                        {
                            if (FormRoot.CleanMode.W0C_PP == CleanMode || FormRoot.CleanMode.W1_C_PP == CleanMode || FormRoot.CleanMode.W1C_PP == CleanMode)
                            {
                                var pn = int.Parse(detail.Substring(1, 1));
                                if (P_XPos[2 * pn - 2] == 0) P_XPos[2 * pn - 2] = int.Parse(detail.Substring(2));
                                else P_XPos[2 * pn - 1] = int.Parse(detail.Substring(2));
                            }
                            else
                            {
                                var pn = int.Parse(detail.Substring(1, 1));
                                P_XPos[pn - 1] = int.Parse(detail.Substring(2));
                            }
                        }
                        break;
                    case "W":
                        if ("n" == detail.Substring(1, 1))
                        {
                            ;
                        }
                        else
                        {
                            var wn = int.Parse(detail.Substring(1, 1));
                            W_YPos[wn - 1] = int.Parse(detail.Substring(2));
                        }
                        break;
                    case "C":
                        //从K指令中获取信息
                        break;
                    case "M":
                        //从K指令中获取信息
                        break;
                    case "V":
                        //从K指令中获取信息
                        break;
                    case "N":
                        //从K指令中获取信息
                        break;
                    case "E":
                        //无可用信息
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
        private static extern bool RequestUpgrade(int nTarget, ref byte[] szRequestRst, ref int nBuffLen);

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
