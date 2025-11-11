using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleYellowButtonApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            // More runtime assignments
            this.okButton.Text = "";
            this.cancelButton.Text = "";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OK clicked!");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
