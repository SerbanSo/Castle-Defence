using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atestat
{
    public partial class Form1 : Form
    {
        int ok = 1;

        public Form1()
        {
            InitializeComponent();
            label1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            Suprafata forma = new Suprafata();           
            forma.ShowDialog();
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(ok==1)
            {
                label1.Show();
                ok *= -1;
            }
            else
            {
                label1.Hide();
                ok *= -1;
            }
        }
    }
}
