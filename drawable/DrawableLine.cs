using System.Drawing;

namespace GestionaleBeB
{
    namespace DotTimeLine
    {
        public class DrawableLine : Drawable
        {
            private Pen linePen;
            public Pen LinePen
            {
                get
                {
                    if(linePen == null)
                    {
                        linePen = new Pen(Color.Green, Stroke);
                    }
                    return linePen;
                }
                set
                {
                    linePen = value;
                }
            }

            private float stroke;
            public float Stroke
            {
                get { return stroke; }
                set { stroke = value; }
            }

            private PointF posi2;
            public PointF Pos2
            {
                
                get
                {
                   
                        return posi2;
                    

                }
                set { posi2 = value;}
            }

            public override PointF Pos
            {
                get
                {
                    
                        return base.Pos;
                    

                }
                set { base.Pos = value; }
            }


            private Center optCenter;
            private bool animate;
            public DrawableLine(Center optCenter = Center.None, bool animate = false)
            {
                this.optCenter = optCenter;
                this.animate = animate;
                base.doneDraw = false;

            }
            private PointF animpos2;
            private int callNo;
            private PointF animPos2
            {
                get {
                       var ret = new PointF(Pos.X + (++callNo), Pos.Y);
                    if (ret.X > Pos2.X)
                    {
                        ret.X = Pos2.X;
                        this.animate = false;
                        base.doneDraw = true;
                    }
                    return ret;
                    }
                
            }
            public override void Draw(Graphics e)
            {
               /*if (animate)
                {
                    e.DrawLine(LinePen, Pos, animPos2);
                    return;
                }*/

                e.DrawLine(LinePen, Pos, Pos2);
                //System.Console.WriteLine(Pos + " " + Pos2);
            }
            



        }
        public enum Center
        {
            X,
            Y,
            None
        }
    }

   
}
