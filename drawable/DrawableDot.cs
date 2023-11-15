using System.Drawing;

namespace GestionaleBeB
{
    namespace DotTimeLine
    {
        public class DrawableDot : Drawable
        {
            private SizeF controlSize;
            public SizeF ControlSize
            {
                get { return controlSize; }
                set
                {
                    controlSize = value;
                }
            }
            private RectangleF _radiusRected;
            private RectangleF radiusRected
            {
                get { return _radiusRected = new RectangleF(new PointF(Pos.X - Radius, Pos.Y-Radius), radiusSized); }
                set { _radiusRected = value; }
            }
            private SizeF radiusSized;
            private PointF anchorPos;
            public PointF AnchorPos
            {
                get { return new PointF(anchorPos.X - Radius, anchorPos.Y - Radius); }
                set { anchorPos = value; }
            }
            public float Radius
            {
                get { return radiusSized.Width / 2; }
                set
                { 
                    radiusSized = new SizeF(value * 2, value * 2);
                    radiusRected = new RectangleF(Pos, radiusSized);
                }
            }
            public override PointF Pos
            {
                get { return base.Pos; }
                set { base.Pos = value; }
            }

            private RectangleF MagnetedPos
            {
                get 
                {
                    var pt = new PointF(MousePos.X,MousePos.Y);
                    var toRet = new RectangleF(pt,radiusSized);
                    return toRet;
                }
            }
            public override void Draw(Graphics e)
            {
                e.FillEllipse(DotBrush, radiusRected);
            }
        }
    }
}
