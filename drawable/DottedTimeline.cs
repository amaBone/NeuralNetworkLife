using GestionaleBeB.DotTimeLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionaleBeB
{
    public partial class DottedTimeline : UserControl
    {
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string title;
        public float strokeWidth = 3.0f;
        public float marginX = 12.0f;
        public float dotRadius = 6.0f;
        public  SizeF controlSize = new SizeF(0,0);
        public  PointF mousePos = new PointF(0, 0);

        private DrawableList drawableList;

        public DottedTimeline()
        {
            //drawableList = new DrawableList(this);
            InitializeComponent();
            
        }
        protected override void OnLoad(EventArgs e)
        {
            DrawableLine line = new DrawableLine(Center.X, true);
            line.Pos = new PointF(6, 6);
            line.Pos2 = new PointF(Width - 6, 6);
            drawableList.Add(line);
            base.OnLoad(e);
            
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            controlSize = new SizeF(this.Width, this.Height);

            if (!drawableList.IsEmpty())
            {
                drawableList.ChangeSize(ref this.controlSize);
            }
            this.Invalidate();

        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mousePos = e.Location;
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            var dot1 = new DrawableDot();
            float h = this.Height / 2;
            PointF pt1 = new PointF(e.X , h);
            dot1.Pos = pt1;
            dot1.Radius = 6.0f;
            drawableList.Add(dot1);

            this.Invalidate();
            this.Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var ctx = e.Graphics;


            if (!drawableList.IsEmpty())
            {
                
                drawableList.MouseMove(ref this.mousePos);
                drawableList.Draw(ctx);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

    }
    public class dotOnTimeLine
    {

    }

}
