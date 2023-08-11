using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFirewallHelper;
using System.ServiceProcess;

namespace firewalld
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CloseFire_Click(object sender, EventArgs e)
        {
            //关闭防火墙
            Process proc = new Process();
            string top = "netsh.exe";
            proc.StartInfo.Arguments = "Advfirewall set allprofiles state off";
            proc.StartInfo.FileName = top;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            status.BackColor = Color.Red;
        }

        private void OpenFire_Click(object sender, EventArgs e)
        {
            //打开防火墙
            Process proc = new Process();
            string top = "netsh.exe";
            proc.StartInfo.Arguments = "Advfirewall set allprofiles state on";
            proc.StartInfo.FileName = top;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            status.BackColor = Color.Green;
        }

        private void status_Paint(object sender, PaintEventArgs e)
        {

        }
        private static NetFwTypeLib.INetFwMgr GetFirewallManager()
        {
            const string CLSID_FIREWALL_MANAGER = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
            Type objType = Type.GetTypeFromCLSID(new Guid(CLSID_FIREWALL_MANAGER));
            return Activator.CreateInstance(objType) as NetFwTypeLib.INetFwMgr;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var table = new DataTable();
            table.Columns.Add("描述",typeof(string));
            table.Columns.Add("类型", typeof(string));
            table.Columns.Add("Enabled", typeof(string));
            table.Columns.Add("本地路径", typeof(string));
            table.Columns.Add("本地地址", typeof(string));
            table.Columns.Add("本地端口", typeof(string));
            table.Columns.Add("远程地址", typeof(string));
            table.Columns.Add("远程端口", typeof(string));
            table.Columns.Add("Protocol", typeof(string));
            table.Columns.Add("Direction",typeof(string));
            List<string> rules = new List<string>();
            INetFwMgr netFwMgr = GetFirewallManager();
            if (netFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled == true)
            {
                status.BackColor = Color.Green;
            }
            else
            {
                status.BackColor = Color.Red;
            }
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(type);
            INetFwRules fwRules = fwPolicy2.Rules;
            foreach (INetFwRule rule in fwRules)
            {
                //rules.Add(rule.Name);
                table.Rows.Add(rule.Description,rule.Action,rule.Enabled,rule.ApplicationName,rule.LocalAddresses,rule.LocalPorts,rule.RemoteAddresses,rule.RemotePorts,rule.Protocol,rule.Direction);
            }
            dataGridView1.DataSource = table;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static void StopFirewallService()
        {
            ServiceController sc = new ServiceController("MpsSvc");
            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }
    }
}
