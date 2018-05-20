using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КГ3
{
    public partial class Form1 : Form
    {
        private RayTracing rayTracing;

        public Form1()
        {
            InitializeComponent();
            rayTracing = new RayTracing();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            rayTracing.Resize(glControl1.Width, glControl1.Height);
        }

        private void Application_Idle(object sender, PaintEventArgs e)
        {

            glControl1_Paint(sender, e);

        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            rayTracing.Update();
            glControl1.SwapBuffers();
            rayTracing.CloseProgram();
        }
    }
}
