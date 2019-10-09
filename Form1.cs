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
        // static Queue<string> q = new Queue<string>(); //存储返回列表队列

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
                Console.WriteLine(res.Trim());

                Console.WriteLine(resSqlit.Length);

                StringBuilder sb = new StringBuilder(this.richTextBox1.Text);
                this.richTextBox1.Text = sb.AppendLine(res).ToString();
                this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
                this.richTextBox1.ScrollToCaret();

                RegexUtils reu = new RegexUtils();

                char[] charParms = new char[] { '[', ']' };
                string resEnd = resSqlit[resSqlit.Length - 1];
                string ipaddr = resEnd.Trim(charParms);

                if (reu.IPCheck(ipaddr))
                {
                    RequestHelp rh = null;
                    try
                    {
                        rh = new RequestHelp();
                        string zone = rh.GetAsync("http://freeapi.ipip.net/" + ipaddr);
                        listBox1.Items.Add(ipaddr + " " + zone);
                    }
                    catch
                    {
                        throw;
                    }
                }

            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RequestHelp rh = new RequestHelp();
            string ipRes=rh.GetAsync("http://myip.ipip.net");
            MessageBox.Show(ipRes);
        }
    }
}
