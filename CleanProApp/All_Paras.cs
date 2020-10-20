using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CleanProApp
{
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
        }
        public List<Icon> IconRes;
        public List<Image> ImageList;
        public List<int> CleanSequence;
        public List<string> CleanProcess;
        public bool WipeInXdirect = true;
        public bool IsInModifying = false;
        public FormRoot.CleanMode CleanMode;
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
        public int M_OnlyWorkTime;
        public int M_Strength = 1;
        public int M_WorkTime;//(0-1000, 999, 1000)
        public int M_HoldTime;
        public int M_CycleNum = 1;
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
        #endregion
    }
}
