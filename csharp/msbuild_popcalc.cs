using System;
using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuildDLL
{
    public class ClassExample : Task, ITask
    {
        public override bool Execute()
        {
            Process.Start("calc.exe");
            return true;
        }
    }
}
