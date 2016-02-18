using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace io.vty.cswf.util
{
    public class Exec
    {
        public static string exec(string exe,params string[] args)
        {
            Process proc=new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = exe;
            proc.StartInfo.Arguments = string.Join(" ", args);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            return proc.StandardOutput.ReadToEnd();
        }
    }
}
