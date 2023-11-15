using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GestionaleBeB
{
    namespace DotTimeLine
    {
        public class DrawableList
        {
            public Form instantiator;
            public Point MousePos => Cursor.Position;

            public Point cMousePos => instantiator.PointToClient(MousePos);

            public List<Drawable> toDraw;

            public bool IsEmpty() => toDraw.Count == 0;
            private Timer invalidateTimer;
            public DrawableList(Form inst)
            {
                instantiator = inst;
                toDraw = new List<Drawable>();
            }


            public void Add(Drawable obj)
            {
                obj.Parent = this;
                toDraw.Add(obj);
                startInvalidate();
            }

            private void startInvalidate()
            {
                if (invalidateTimer == null)
                {
                    invalidateTimer = new Timer();
                    invalidateTimer.Interval = (int)(1);
                    invalidateTimer.Enabled = true;
                    invalidateTimer.Tick += new EventHandler(InvalidateTimer_Tick);
                }
                else
                {
                    invalidateTimer.Stop();
                }


                
                invalidateTimer.Start();

                InvalidateTimer_Tick();
            }

            private void InvalidateTimer_Tick(object sender = null, System.EventArgs e = null)
            {
                if (DoINeedToInvalidate() == false) {

                    invalidateTimer.Stop();
                    return;
                    
                   }
                instantiator.Invalidate();
            }

            private bool DoINeedToInvalidate() => !toDraw.TrueForAll(DoneDraw);

            private bool DoneDraw(Drawable list)
            {
                return list.doneDraw;
            }

            public void Draw(Graphics ctx)
            {
                foreach(Drawable item in toDraw)
                {
                    item.Draw(ctx);
                }
            }

            public void MouseMove(ref PointF mousePos)
            {
                foreach(Drawable dot in toDraw)
                {
                    dot.MousePos = mousePos;
                }
            }

            public void ChangeSize(ref SizeF size)
            {
                
            }


        }

    }
}
