using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Editor
{
    public partial class Form1 : Form
    {
        int i;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(Pens.Red, 10, 10, 100, 100);
            g.DrawEllipse(Pens.Red, 10, 10, 100, 100);
        }
    }
}
