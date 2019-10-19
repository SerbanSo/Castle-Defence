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
    public partial class Test : Form
    {
        List<int> lista = new List<int>();
        public Test()
        {
            InitializeComponent();
            lista.Add(1);
            lista.Add(1);
            lista.Add(1);
            lista.Add(1);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < lista.Count; i++)
            {
                int x = 1, a=0, b=0;
                for (int j = 0; j < lista[i]; j++)
                {
                    x *= 2;
                }
                switch ((int)(i / 4f * 100))
                {
                    case 0:
                        a = 0;
                        b = 0;
                        break;
                    case 1:
                        a = 2;
                        b = 0;
                        break;
                    case 2:
                        a = 0;
                        b = 2;
                        break;
                    case 3:
                        a = 2;
                        b = 2;
                        break;
                }
                g.DrawRectangle(new Pen(Color.White), (float)Math.Pow(2,a), (float)Math.Pow(2, b), 360 / (float)Math.Pow(2,lista[i]), 360 / (float)Math.Pow(2,lista[i]));
            }

        }
    }
}
