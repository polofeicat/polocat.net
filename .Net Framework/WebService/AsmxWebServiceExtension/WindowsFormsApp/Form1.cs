﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string s1 = ws.GreetingHello("polocat");
                textBox1.Text = s1;
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
            }

        }


        private polocat_ws.WebService1 ws;

        private void Form1_Load(object sender, EventArgs e)
        {
            ws = new polocat_ws.WebService1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s1 = ws.GetUserNameById("tomcat");
            MessageBox.Show(s1);
        }
    }
}
