using HelloWorld.utils;
using System;
using System.Diagnostics;
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
        // static Queue<string> q = new Queue<string>(); //返回值队列

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            listBox1.Items.Clear();
            Process ps = null;
            //stringSplitUtils sp = new stringSplitUtils();
            try
            {
                //rh = new RequestHelp();

                string address = textBox1.Text;

                string cmd = "TRACERT.exe " + address;

                ps = new ProcessCmdUtils().ExecCmd();

                ps.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);

                ps.Start();

                ps.StandardInput.WriteLine(cmd + "&exit");
                ps.StandardInput.AutoFlush = true;

                ps.BeginOutputReadLine();
            }
            catch
            {
                throw;
            }

            ps.Close();
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
                        //MessageBox.Show("并不是ip有效类型。");
                        break;
                    default:
                        MessageBox.Show("无效值");
                        break;
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
            MessageBox.Show(ipRes);
        }
    }
}
