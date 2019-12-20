using HelloWorld.utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        static int psTaskID = -1; //检测是否有上次执行的task，如果值为-1，则执行，否则kill掉上次执行未完成任务
        private void button1_Click(object sender, EventArgs e)
        {

            richTextBox1.Text = "";
            listBox1.Items.Clear();
            ProcessCmdUtils pcm = null;
            Process ps = null;

            string cmd = "";


            if (psTaskID != -1)
            {
                ProcessCmdUtils procKill = new ProcessCmdUtils();
                bool killStatus = procKill.KillProcExec(psTaskID);
                //Console.WriteLine("try external "+psTaskID);
                if (killStatus)
                {
                    psTaskID = -1;
                }
            }

            try
            {
                string address = textBox1.Text;

                cmd = "TRACERT.exe " + address;

                pcm = new ProcessCmdUtils();
                ps = pcm.DoTask(cmd, OutputHandler);

                psTaskID = ps.Id;//将运行的process name 赋值给 paTaskName
            }
            catch
            {
                throw;
            }
            finally
            {
                ps.Close();
            }

        }


        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                string res = outLine.Data;
                string[] resSqlit = System.Text.RegularExpressions.Regex.Split(res.Trim(), "\\s+");

                StringBuilder sb = new StringBuilder(this.richTextBox1.Text);
                this.richTextBox1.Text = sb.AppendLine(res).ToString();
                this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
                this.richTextBox1.ScrollToCaret();

                RegexUtils reu = null;
                RequestHelp rh = null;

                char[] charParms = new char[] { '[', ']' };
                string resEnd = resSqlit[resSqlit.Length - 1];
                string ipaddr = resEnd.Trim(charParms);

                //Console.WriteLine(resEnd);

                try
                {
                    reu = new RegexUtils();
                    rh = new RequestHelp();
                    if (reu.IPCheck(ipaddr)){
                        string ipZone = rh.InquireIpInfo(ipaddr);

                        if (ipZone != "No Value!")
                        {
                            listBox1.Items.Add(ipaddr + " " + ipZone);

                        }
                        else
                        {
                            MessageBox.Show("无效值");
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        private void OutputNmapHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                string res = outLine.Data;
                string[] resSqlit = System.Text.RegularExpressions.Regex.Split(res.Trim(), "\\s+");
                if (resSqlit.Length== 3)
                {
                    StringBuilder sb = new StringBuilder(this.richTextBox1.Text);
                    this.richTextBox1.Text = sb.AppendLine(res).ToString();
                    this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
                    this.richTextBox1.ScrollToCaret();
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RequestHelp rh = null;
            try
            {
                rh = new RequestHelp();
                string ipRes = rh.InquireLocalIp();
                MessageBox.Show(ipRes);
            }
            catch
            {
                MessageBox.Show("获取本地地址信息，请求异常。");
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
            ProcessCmdUtils pcm = null;
            Process ps = null;
            RegexUtils rgu = new RegexUtils();
            string nmapPath =Environment.CurrentDirectory+"\\tools\\nmap\\nmap.exe"; //测试

            if (!File.Exists(nmapPath))
            {
                MessageBox.Show("No Nmap tool");
                return;
            }

            string ip = textBox1.Text;

            int port = 0;

            string cmd = "";

            try
            {
                port = Convert.ToInt32(textBox2.Text);
                if (rgu.IPCheck(ip) || port >= 0 && port <= 65535)
                {
                    cmd = nmapPath + " -sT -p " + port + " " + ip;
                }
                else
                {
                    MessageBox.Show("Please check whether the format of the IP address or port number you entered is correct.");
                    return;
                }

                pcm = new ProcessCmdUtils();
                ps = pcm.DoTask(cmd, OutputNmapHandler);
            }
            catch (Exception ex)
            {
                if(ex.GetType().ToString()== "System.FormatException")
                {
                    MessageBox.Show("请检测您输入的端口号，格式是否正确！");
                        return;
                }
            }
            finally
            {
                if (ps != null)
                {
                    ps.Close();
                }
            }

        }
    }
}
