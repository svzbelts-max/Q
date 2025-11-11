using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleYellowButtonApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Runtime text assignment
            this.buttonWithResource.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 1 was clicked!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 2 was clicked!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 3 was clicked!");
        }
    }
}
