using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace io.vty.cswf.util
{
    public class Exec
    {
        public static int exec(out string output, string exe, params string[] args)
        {
            StringBuilder sb = new StringBuilder();
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = exe;
            proc.StartInfo.Arguments = string.Join(" ", args);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.OutputDataReceived += (sender, e) =>
            {
                lock (sb)
                {
                    sb.Append(e.Data + "\n");
                }
            };
            proc.ErrorDataReceived += (sender, e) =>
            {
                lock (sb)
                {
                    sb.Append(e.Data + "\n");
                }
            };
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
            output = sb.ToString();
            return proc.ExitCode;
        }
    }
}
