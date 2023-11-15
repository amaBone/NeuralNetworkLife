using System.Drawing;

namespace GestionaleBeB
{
    namespace DotTimeLine
    {

        public class Drawable
        {
            public bool doneDraw = true;
            private DrawableList parent;

            public virtual SizeF ControlSize
            {
                get { return Parent.instantiator.Size; }
            }


            public DrawableList Parent
            {
                get
                {
                    return parent;
                }
                set { parent = value; }
            }
            private PointF mousePos;
            public PointF MousePos
            {
                get { return mousePos; }
                set { mousePos = value; }
            }

            private PointF posi;
            public virtual PointF Pos
            {
                get { return posi; }
                set { posi = value; }
            }

            private SolidBrush dotBrush;
            public SolidBrush DotBrush
            {
                get
                {
                    if (dotBrush == null)
                    {
                        dotBrush = new SolidBrush(Color.White);                       
                    }
                    return dotBrush;
                }
                set
                {
                    dotBrush = value;
                }
            }

            public virtual void Draw(Graphics e)
            {

            }
            public virtual bool asCollider()
            {
                return false;
            }
            public virtual void doPhys(){ }


        }

    }
}
