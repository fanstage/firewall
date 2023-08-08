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
        private bool IsFirewallEnabled()
        {
            Process process = new Process();
            process.StartInfo.FileName = "netsh.exe";
            process.StartInfo.Arguments = "advfirewall show currentprofile state";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Contains("ON");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsFirewallEnabled())
            {
                status.BackColor = Color.Green;
            }
            else
            {
                status.BackColor = Color.Red;
            }
        }
    }
}
