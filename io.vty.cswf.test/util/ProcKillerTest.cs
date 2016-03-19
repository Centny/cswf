using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System.Diagnostics;
using System.Threading;

namespace io.vty.cswf.test.util
{
    [TestClass]
    public class ProcKillerTest
    {
        [TestMethod]
        public void TestKill()
        {
            var killer = new ProcKiller(500);
            killer.Names.Add("cmd");
            //killer.Names.Add("sleep");
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = "test\\do_w.bat";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            killer.Running.Add(proc.Id);
            String data;
            TaskPool.Queue(i =>
            {
                Exec.exec(out data, "test\\do_w.bat");
            }, 0);
            TaskPool.Queue(i =>
            {
                Exec.exec(out data, "test\\do_w.bat");
            }, 1);
            while (true)
            {
                Thread.Sleep(500);
                var ps=Process.GetProcessesByName("cmd");
                if (ps.Length==1)
                {
                    break;
                }
            }
            Assert.AreEqual(1, Process.GetProcessesByName("cmd").Length);
            proc.Kill();
            Assert.AreEqual(0, Process.GetProcessesByName("cmd").Length);
        }
    }
}
