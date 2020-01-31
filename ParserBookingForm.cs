using Newtonsoft.Json;
using OtpSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParserBooking
{
    public partial class ParserBookingForm : Form
    {
        BrowserForm _bf;
        CancellationTokenSource _cts;
        Thread _parseThr;

        public ParserBookingForm()
        {
            InitializeComponent();
            _bf = new BrowserForm();
            _bf.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _cts = new CancellationTokenSource();
            _parseThr = new Thread(new ParameterizedThreadStart(Algorithm));
            _parseThr.Start(_cts.Token);
        }

        async void Algorithm(object obj)
        {
            BigWriter bigWriter = new BigWriter(textBox1.Text, textBox2.Text, Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));
            try
            {
                stop_btn.Invoke(new Action(() => stop_btn.Enabled = true));
                parse_btn.Invoke(new Action(() => parse_btn.Enabled = false));
                var cts = (CancellationToken)obj;
                await _bf.Algorithm(cts, textBox3.Text, textBox1.Text, textBox2.Text, bigWriter, checkBox1.Checked);
                parse_btn.Invoke(new Action(() => parse_btn.Enabled = true));
                stop_btn.Invoke(new Action(() => stop_btn.Enabled = false));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                bigWriter.Finish();
                Stop_btn_Click(null, null);
            }
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    _cts.Cancel();
                }
                _cts.Dispose();
                _parseThr.Abort();
                parse_btn.Enabled = true;
                stop_btn.Enabled = false;
            }
            catch
            {

            }
        }

        private void ParserBookingForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
