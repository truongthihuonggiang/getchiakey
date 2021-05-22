using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace getkey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender1, EventArgs e1)
        {


            List<string> cmds = new List<string>();
            cmds.Add("cd $env:userprofile\\AppData\\Local\\Chia-Blockchain\\app-*\\resources\\app.asar.unpacked\\daemon\\");
            cmds.Add("./chia keys show");
            //  lbFarmkey.Text = RunCommands(cmds);
            String workingDirectory = "";
            var process = new Process();
            var psi = new ProcessStartInfo();
            // startInfo.FileName = @"powershell.exe";
            psi.FileName = "powershell.exe";
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = workingDirectory;
            process.StartInfo = psi;
            process.Start();
            String s = "";
            String er = "";
            process.OutputDataReceived += (sender, e) => { 
                Console.WriteLine(e.Data); 
                s += e.Data; 
            };
            process.ErrorDataReceived += (sender, e) => {
                 Console.WriteLine(e.Data);  
                er += e.Data;
            };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            using (StreamWriter sw = process.StandardInput)
            {
                foreach (var cmd in cmds)
                {
                    sw.WriteLine(cmd);
                }
            }
            process.WaitForExit();
            if(er.Length > 0)
            {
                txtMasterkey.Text = "not found";
                txtFarmkey.Text = "not found";
                txtPoolkey.Text = "not found";
            }
            if (s.Length > 0)
            {
                txtMasterkey.Text = getpubmasterkey(s);
                txtFarmkey.Text = getpubfarmkey(s);
                txtPoolkey.Text = getpubpoolkey(s);
            }

        }
        String getpubmasterkey(String s)
        {
            String kq = "";
            int pos = s.LastIndexOf("Master public key (m):");
            if(pos > 0)
            {
                pos += 23;
                kq = s.Substring(pos, 96);
            }
           
            return kq;
        }
        String getpubpoolkey(String s)
        {
            String kq = "";
            int pos = s.LastIndexOf("Pool public key (m/12381/8444/1/0): ");
            if (pos > 0)
            {
                pos += 36;
                kq = s.Substring(pos, 96);
            }

            return kq;
        }
        String getpubfarmkey(String s)
        {
            String kq = "";
            int pos = s.LastIndexOf("Farmer public key (m/12381/8444/0/0): ");
            if (pos > 0)
            {
                pos += 38;
                kq = s.Substring(pos, 96);
            }

            return kq;
        }
        String RunCommands(List<string> cmds, string workingDirectory = "")
        {
            var process = new Process();
            var psi = new ProcessStartInfo();
           // startInfo.FileName = @"powershell.exe";
            psi.FileName = "powershell.exe";
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = workingDirectory;
            process.StartInfo = psi;
            process.Start();
            process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            using (StreamWriter sw = process.StandardInput)
            {
                foreach (var cmd in cmds)
                {
                    sw.WriteLine(cmd);
                }
            }
            process.WaitForExit();
            String s = process.StandardOutput.ReadLine();
            return "";
        }
    }
}
