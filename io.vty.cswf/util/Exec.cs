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
            proc.StartInfo.Arguments = Join(args);
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

        public static String Join(String[] args, int idx = 0, int len = -1)
        {
            if (len < 0)
            {
                len = args.Length - idx;
            }
            StringBuilder sb = new StringBuilder();
            for (var i=0;i< len;i++)
            {
                var arg = args[idx + i];
                if (arg.Contains(' '))
                {
                    sb.AppendFormat("'{0}'", arg);
                }
                else
                {
                    sb.Append(arg);
                }
                sb.Append(" ");
            }
            return sb.ToString();
        }
        public static int exec(out string output, string cmds)
        {
            if (String.IsNullOrWhiteSpace(cmds))
            {
                throw new Exception("cmds is null or empty");
            }
            var targs = ParseArgs(cmds);
            var args = new List<String>();
            for (var i = 1; i < targs.Length; i++)
            {
                args.Add(targs[i]);
            }
            return exec(out output, targs[0], args.ToArray<String>());
        }

        public delegate void Do();
        public static String[] ParseArgs(string args)
        {
            IList<String> ls = new List<String>();
            StringBuilder sb = new StringBuilder();
            char last = '\0';
            Do adda = () =>
            {
                if (sb.Length > 0)
                {
                    ls.Add(sb.ToString());
                    sb.Clear();
                }
            };
            for (var i = 0; i < args.Length; i++)
            {
                if (last == '\0')
                {
                    switch (args[i])
                    {
                        case '\t':
                            adda();
                            break;
                        case ' ':
                            adda();
                            break;
                        case '\'':
                            adda();
                            last = '\'';
                            break;
                        case '"':
                            adda();
                            last = '"';
                            break;
                        default:
                            sb.Append(args[i]);
                            break;
                    }

                }
                else
                {
                    if (last == args[i])
                    {
                        adda();
                        last = '\0';
                    }
                    else
                    {
                        sb.Append(args[i]);
                    }
                }
            }
            adda();
            return ls.ToArray<String>();
        }
    }
}
