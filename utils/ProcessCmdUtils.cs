using System;
using System.Diagnostics;

namespace HelloWorld.utils
{
    class ProcessCmdUtils
    {
        private static Process ExecCmd()
        {
            //cmd = cmd.Trim().TrimEnd('&') + "&exit";


            Process p = null;
            try
            {
                p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            }
            catch (Exception e)
            {
                throw;
            }

            return p;
        }

        public bool KillProcExec(int procId)
        {
            string cmd = string.Format("taskkill /f /t /im {0}", procId);

            Process ps = null;
            try
            {
                ps = ExecCmd();
                ps.Start();
                ps.StandardInput.WriteLine(cmd + "&exit");
                return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                ps.Close();
            }


            return false;
        }

        public delegate void OutHandler(object sendingProcess, DataReceivedEventArgs outLine);

        public Process DoTask(string cmd, OutHandler oh)
        {
            Process ps = null;

            try
            {
                ps = ProcessCmdUtils.ExecCmd();
                ps.OutputDataReceived += new DataReceivedEventHandler(oh);

                ps.Start();

                ps.StandardInput.WriteLine(cmd + "&exit");
                ps.StandardInput.AutoFlush = true;

                ps.BeginOutputReadLine();

            }
            catch
            {
                throw;
            }

            return ps;
        }
    }
}
