using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CleanProApp
{
    public partial class ZS_SetNet : Form
    {
        [DllImport("printer_interface", EntryPoint = "InitInterface")]
        private static extern int InitInterface(IntPtr handle, uint Msg);

        public List<NetworkInterface> AvailabelAdapters = new List<NetworkInterface>();
        string CurAdapterID = string.Empty;
        //string CurPrtIP = "192.168.1.10";
        IPAddress CurIPAddress = null;
        //ManagementObject SelectedAdapter = null;
        public static string INIFile = Environment.CurrentDirectory + "\\Portconfig.ini";
        public bool RealExit = false;
        public ZS_SetNet()
        {
            this.MaximizeBox = false;
            this.Icon = FormRoot.Printer.IconRes[7];
            this.ImeMode = ImeMode.Alpha;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            //Loc IP
            GetLocalAdapters();
            txtBox_LocIP1.Enabled = txtBox_LocIP2.Enabled = txtBox_LocIP3.Enabled = txtBox_LocIP4.Enabled = false;
            txtBox_LocIP1.Text = txtBox_LocIP2.Text = txtBox_LocIP3.Text = txtBox_LocIP4.Text = string.Empty;
            //Prt IP
            string EntileIP = FormRoot.Printer.F_ReadINI("Channel0Param", "DestIp", "", INIFile);
            string[] IPv4 = EntileIP.Split('.');
            txtBox_PrtIP1.Text = string.IsNullOrEmpty(EntileIP) ? "" : IPv4[0];
            txtBox_PrtIP2.Text = string.IsNullOrEmpty(EntileIP) ? "" : IPv4[1];
            txtBox_PrtIP3.Text = string.IsNullOrEmpty(EntileIP) ? "" : IPv4[2];
            txtBox_PrtIP4.Text = string.IsNullOrEmpty(EntileIP) ? "" : IPv4[3];
        }
        public void GetLocalAdapters()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet || adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    if (adapter.Speed != 0)
                        if (!adapter.Name.Contains("VMware"))
                        {
                            AvailabelAdapters.Add(adapter);
                            comBox_NetAdapter.Items.Add(adapter.Name);
                        }
            }
            comBox_NetAdapter.SelectedIndex = -1;
        }
        private void comBox_NetAdapter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string IP = string.Empty;
            if (comBox_NetAdapter.SelectedIndex != -1)
            {
                CurAdapterID = AvailabelAdapters[comBox_NetAdapter.SelectedIndex].Id;
                IPInterfaceProperties ipProp = AvailabelAdapters[comBox_NetAdapter.SelectedIndex].GetIPProperties();
                UnicastIPAddressInformationCollection ipList = ipProp.UnicastAddresses;
                foreach (UnicastIPAddressInformation ip in ipList)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IP = ip.Address.ToString();
                        CurIPAddress = ip.Address;
                    }
                }
                txtBox_LocIP1.Enabled = txtBox_LocIP2.Enabled = txtBox_LocIP3.Enabled = txtBox_LocIP4.Enabled = true;
            }
            else
            {
                txtBox_LocIP1.Enabled = txtBox_LocIP2.Enabled = txtBox_LocIP3.Enabled = txtBox_LocIP4.Enabled = false;
            }
            string[] IPparts = IP.Split('.');
            txtBox_LocIP1.Text = (string.IsNullOrEmpty(IP)) ? "" : IPparts[0];
            txtBox_LocIP2.Text = (string.IsNullOrEmpty(IP)) ? "" : IPparts[1];
            txtBox_LocIP3.Text = (string.IsNullOrEmpty(IP)) ? "" : IPparts[2];
            txtBox_LocIP4.Text = (string.IsNullOrEmpty(IP)) ? "" : IPparts[3];
        }
        private void btn_ReCnct_Click(object sender, System.EventArgs e)
        {
            if ((Button)sender == btn_ReCnct)
            {
                if (SetIPConfig())
                {
                    FormRoot.Printer.OfflineUse = false;
                    RealExit = true;
                }
                else
                    RealExit = false;
            }
            else
            {
                FormRoot.Printer.OfflineUse = true;
                RealExit = true;
            }
            this.Close();
        }
        public bool SetIPConfig()
        {
            //Set Local IP
            string SetLocIP = txtBox_LocIP1.Text + '.' + txtBox_LocIP2.Text + '.' + txtBox_LocIP3.Text + '.' + txtBox_LocIP4.Text;
            //string[] arrLocIP = new string[] { txtBox_LocIP1.Text, txtBox_LocIP2.Text, txtBox_LocIP3.Text, txtBox_LocIP4.Text };
            var VSL = ValidateIPAddress(SetLocIP);
            //Modify Prt IP
            string SetPrtIP = txtBox_PrtIP1.Text + '.' + txtBox_PrtIP2.Text + '.' + txtBox_PrtIP3.Text + '.' + txtBox_PrtIP4.Text;
            var VSP = ValidateIPAddress(SetPrtIP);
            if (!VSL || !VSP)
            {
                MessageBox.Show("请输入有效的IP地址", "验证", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            FormRoot.Printer.F_WriteINI("Channel0Param", "DestIp", SetPrtIP, INIFile);
            FormRoot.Printer.F_WriteINI("Channel0Param", "ConnectionName", comBox_NetAdapter.SelectedItem.ToString(), INIFile);
            FormRoot.Printer.F_WriteINI("Channel0Param", "LocalIp", SetLocIP, INIFile);
            string gateway = txtBox_LocIP1.Text + '.' + txtBox_LocIP2.Text + '.' + txtBox_LocIP3.Text + '.' + "1";

            SetLocalAdapter(CurIPAddress.ToString(), SetLocIP, "255.255.255.0", "");
            //if (!ReStartAdapter(SelectedAdapter))
            //{
            //    MessageBox.Show("请重新开启软件使IP设置生效", "操作指示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);//即将
            //    Environment.Exit(1);
            //}
            Thread.Sleep(100);
            //var stat = InitInterface(this.Handle, 0);
            var stat = FormRoot.Printer.Net_PingIsOK(SetPrtIP, 0);//, 2000
            Thread.Sleep(1000);
            if (!stat)
            {
                timer1.Enabled = true;
                label_err.Visible = true;

                if (DialogResult.OK == MessageBox.Show("IP已设置完成,程序即将重启", "操作指示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk))
                {
                    Thread.Sleep(1000);
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                return false;
            }

            return true;
        }
        private bool ReStartAdapter(ManagementObject SelectedAdpt)
        {
            try
            {
                SelectedAdpt.InvokeMethod("Disable", null);

            }
            catch (ManagementException)//me
            {
                return false;
            }
            //Thread.Sleep(100);
            try
            {
                SelectedAdpt.InvokeMethod("Enable", null);

            }
            catch (ManagementException)//me
            {
                return false;
            }
            return true;
        }
        public static bool ValidateIPAddress(string ipAddress)
        {
            Regex validipregex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            return (ipAddress != "" && validipregex.IsMatch(ipAddress.Trim())) ? true : false;
        }
        public bool SetLocalAdapter(string CurIP, string NewIP, string submask, string getway)
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            string str = string.Empty;

            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                if (CurIP == ((string[])mo["IPAddress"])[0])
                {
                    //string netshStr = string.Format("netsh interface ipv4 set address name = {0} source = static address = {1} mask = {2}"
                    //    , comBox_NetAdapter.SelectedItem.ToString(), NewIP, submask);
                    //string netshStr = string.Format("netsh interface ip add address {0} addr = {1} mask = {2} gateway = {3}"
                    //    , comBox_NetAdapter.SelectedItem.ToString(), NewIP, submask, getway);
                    //Process netPro = new Process();
                    //netPro.StartInfo.FileName = "cmd.exe";// "netsh.exe";
                    //netPro.StartInfo.UseShellExecute = false;
                    //netPro.StartInfo.RedirectStandardInput = true;
                    //netPro.StartInfo.RedirectStandardOutput = true;
                    //netPro.StartInfo.RedirectStandardError = true;
                    //netPro.Start();
                    //netPro.StandardInput.AutoFlush = true;
                    //netPro.StandardInput.WriteLine(netshStr);
                    //netPro.StandardInput.WriteLine("exit");
                    //MessageBox.Show(netPro.StandardOutput.ReadToEnd().ToString());
                    //netPro.WaitForExit();
                    //netPro.Close();

                    if (NewIP != null && submask != null)
                    {
                        //string caption = mo["Caption"].ToString();
                        inPar = mo.GetMethodParameters("EnableStatic");
                        //Set New IP
                        inPar["IPAddress"] = new string[] { NewIP };//"10.22.21.111",
                        inPar["SubnetMask"] = new string[] { submask };//submask, 
                        outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                    }
                    if (getway.Equals(string.Empty))
                    {
                        mo.InvokeMethod("SetGateways", null);
                    }
                    else
                    {
                        inPar = mo.GetMethodParameters("SetGateways");
                        inPar["DefaultIPGateway"] = new string[] { getway };//"10.22.21.1", 
                        outPar = mo.InvokeMethod("SetGateways", inPar, null);
                    }
                    //str = outPar["returnvalue"].ToString();
                    //return (str == "0" || str == "1") ? true : false;
                    ReStartAdapter(mo);

                    return true;
                }
            }
            return false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_err.Visible = false;
            timer1.Enabled = false;
        }
        private void ZS_SetNet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RealExit) e.Cancel = false;
            else
                Environment.Exit(1);
        }

    }
}
