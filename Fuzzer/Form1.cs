using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fuzzer
{
    public partial class Form1 : Form
    {
        bool started = false;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            WebFuzzer fuzzer = new WebFuzzer(textBox1.Text, textBox2.Text);
            fuzzer.OnBruteforceStringLengthChanged += Fuzzer_OnBruteforceStringLengthChanged;
            fuzzer.OnError += Fuzzer_OnError;
            fuzzer.OnUrlExist += Fuzzer_OnUrlExist;
            fuzzer.OnEnded += Fuzzer_OnEnded;
            fuzzer.OnUrlComputed += Fuzzer_OnUrlComputed;
            fuzzer.StartAsync(3);
            started = true;
        }

        private void Fuzzer_OnUrlComputed(string obj)
        {
            Console.WriteLine(obj);
        }

        private void Fuzzer_OnEnded()
        {
            started = false;
            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText("L'opération est terminée.")));

        }

        private void Fuzzer_OnUrlExist(string obj)
        {
            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(obj + Environment.NewLine)));
        }

        private void Fuzzer_OnError(WebFuzzerError obj)
        {
            switch (obj)
            {
                case WebFuzzerError.Unknown:
                    break;
                case WebFuzzerError.InvalidUrl:
                    MessageBox.Show("L'adresse est invalide.");
                    break;
                case WebFuzzerError.OfflineHost:
                    MessageBox.Show("Impossible de contacter l'hôte distant.");
                    break;
            }
        }

        private void Fuzzer_OnBruteforceStringLengthChanged(int obj)
        {
            charNumber.Invoke(new Action(() => charNumber.Text = obj.ToString()));
        }
    }
}
