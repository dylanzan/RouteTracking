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
            Process ps = null;

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
                //rh = new RequestHelp();

                string address = textBox1.Text;

                string cmd = "TRACERT.exe " + address;

                ps = ProcessCmdUtils.ExecCmd();

                ps.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);

                ps.Start();

                psTaskID = ps.Id;//将运行的process name 赋值给 paTaskName

                ps.StandardInput.WriteLine(cmd + "&exit");
                ps.StandardInput.AutoFlush = true;

                ps.BeginOutputReadLine();
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

                RegexUtils reu = new RegexUtils();
                RequestHelp rh = null;
                JsonParseUtils jsu = null;

                char[] charParms = new char[] { '[', ']' };
                string resEnd = resSqlit[resSqlit.Length - 1];
                string ipaddr = resEnd.Trim(charParms);

                //判断ip 类型
                switch (reu.IPCheckForS(ipaddr))
                {
                    case "ipv4":
                        if (reu.IPCheck(ipaddr))
                        {
                            jsu = new JsonParseUtils();
                            rh = new RequestHelp();
                            string ipv4JsonResponse = rh.GetAsync("http://39.96.177.233/" + ipaddr);
                            //string ipv4JsonResponse = rh.GetAsync("http://127.0.0.1:8081/" + ipaddr);
                            string ipv4Zone = jsu.JsonParse(ipv4JsonResponse);
                            listBox1.Items.Add(ipaddr + " " + ipv4Zone);
                        }
                        break;
                    case "ipv6":
                        jsu = new JsonParseUtils();
                        rh = new RequestHelp();
                        string ipv6Zone = rh.GetAsync("http://freeapi.ipip.net/" + ipaddr);
                        listBox1.Items.Add(ipaddr + " " + ipv6Zone);
                        break;
                    case "nothing":
                        break;
                    default:
                        MessageBox.Show("无效值");
                        break;
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
            RequestHelp rh = new RequestHelp();
            string ipRes = rh.GetAsync("http://39.96.177.233");
            //string ipRes = rh.GetAsync("http://127.0.0.1:8081");
            MessageBox.Show(ipRes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
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
                ps = ProcessCmdUtils.ExecCmd();
                ps.OutputDataReceived += new DataReceivedEventHandler(OutputNmapHandler);

                ps.Start();

                psTaskID = ps.Id;//将运行的process name 赋值给 paTaskName

                ps.StandardInput.WriteLine(cmd + "&exit");
                ps.StandardInput.AutoFlush = true;

                ps.BeginOutputReadLine();
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
