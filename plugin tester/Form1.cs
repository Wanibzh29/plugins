﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nikse.SubtitleEdit.PluginLogic;

namespace plugin_tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _fileName = string.Empty;
        private void buttonWithFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _fileName = openFileDialog1.FileName;
                RunPlugin(File.ReadAllText(_fileName, Encoding.UTF8));
            }
        }

        private void RunPlugin(string content)
        {
            IPlugin hiColorer = new HIColorer();
            hiColorer.DoAction(this, content, 23.796, "<br />", Path.GetFileName(_fileName), null, content);
        }
    }
}
