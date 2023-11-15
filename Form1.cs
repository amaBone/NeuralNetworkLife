using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionaleBeB.DotTimeLine;
using NN_try_1.baseNN;

namespace NN_try_1
{
    public partial class Form1 : Form
    {
        private DrawableList drawableList;
        private DrawableDot me;
        private DrawableIaDot mi;
        //private BrainManager bm;
        private BrainManage2 bm;

        private Simulation sim;
        private ReplayGlance rp;
        public Form1()
        {
            

            drawableList = new DrawableList(this);
            InitializeComponent();
            me = new DrawableDot();
            drawableList.Add(me);
            me.Pos = new PointF(20, 20);
            me.Radius = 10;

            mi = new DrawableIaDot();
            mi.Pos = new PointF(50, 50);
            mi.Radius = 13;
            drawableList.Add(mi);
            //bm = new BrainManager(1024);
            //bm = new BrainManage2(1024);
            sim = new Simulation(512*2);
            sim.RunSimulation();
            rp = new ReplayGlance(sim);

        }
        private float x = 0;
        protected override void OnPaint(PaintEventArgs args)
        {
            Graphics g = args.Graphics;

            args.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            args.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            args.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //Get the Graphics object from PaintEventArgs

            

            //Fill ellipse
            //public Rectangle(Point location, Size size);
            x += 0.12f;
            var p = new PointF(x, 10);
            var s = new Size(10, 10);

            //var rect = new RectangleF(p, s);
            //g.FillEllipse(Brushes.Red,rect);

            //me.Pos = new PointF(30,x);
            //mi.Pos = new PointF(x / 8, 25);
            //mi.Rotate(0.25f);
            //drawableList.Draw(g);
            //bm.DrawTick(8,g);
            //bm.taskTickThenDraw(32, g);
            rp.Draw(g);
            Thread.Sleep(10);
            this.Invalidate();
        }
        

    }
}
