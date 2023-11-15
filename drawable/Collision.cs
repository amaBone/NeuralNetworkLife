using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionaleBeB.DotTimeLine;
namespace NN_try_1.drawable
{
    class Collision
    {

    }
    namespace Collider
    {
        public class CircleCollider
        {
            public Vector2 position = new Vector2(0,0);
            public double radius = 1f;

            public bool CircleIntersect(Vector2 pos, double radius)
            {
                var v = position - pos;
                return 
                        v.MagnitudeSqrd
                               <=
                        (radius * radius) + (this.radius * this.radius)
                         ;

            }
            public bool CircleIntersect(CircleCollider c)
            {
                var v = position - c.position;
                return
                        v.MagnitudeSqrd
                               <=
                        (c.radius * c.radius) + (this.radius * this.radius)
                         ;
            }


        }
        public class LineCollider
        {
            public Vector2 pos = new Vector2(0,0);
            public Vector2 dir = new Vector2(0,0);
            public int CircelIntersect(CircleCollider c, Graphics e = null)
            {
                return CircelIntersect(c.position, c.radius, e);
            }
            public int CircelIntersect(PointF cP1, double radius,Graphics e = null)
            {
                PointF pos2 = pos + dir;
                float x, y, x1, y1;
                x = (float) ((pos.X < pos2.X) ? pos.X : pos2.X);
                y = (float)((pos.Y < pos2.Y) ? pos.Y : pos2.Y);
                x1 = (float)((pos.X > pos2.X) ? pos.X : pos2.X);
                y1 = (float)((pos.Y > pos2.Y) ? pos.Y : pos2.Y);
                float widht = x1 - x;
                float height = y1 - y;
                PointF p, p1;

                var f = new Vector2(pos);
                var end = new Vector2(pos2);
                var bitBehind = new Vector2(dir);
                bitBehind.Magnitude *= -0.05f;
                var m = new Vector2(pos + (bitBehind));
                if (m.sqrdDistance(cP1) < f.sqrdDistance(cP1))
                {
                    return 0;
                }

                int ret = math.FindLineCircleIntersections((float)cP1.X, (float)cP1.Y, (float)radius, pos, pos + dir, out p, out p1);
                
                float maxdist=dir.MagnitudeSqrd;
                

                if (e!=null)
                {
                    var dot = new DrawableDot();
                    dot.Pos = p1;
                    dot.Radius = 2;
                    dot.DotBrush = (SolidBrush)Brushes.Red;
                    dot.Draw(e);
                }
                //Console.WriteLine("f.sqrdDist: " + f.sqrdDistance(p1));
                if (f.sqrdDistance(p1) > maxdist)
                {
                    return 0;
                }
                return ret;
            }
            
        }


        class math
        {
            // Find the points of intersection.
            public static int FindLineCircleIntersections(
                float cx, float cy, float radius,
                PointF point1, PointF point2,
                out PointF intersection1, out PointF intersection2)
            {
                float dx, dy, A, B, C, det, t;

                dx = point2.X - point1.X;
                dy = point2.Y - point1.Y;

                A = dx * dx + dy * dy;
                B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
                C = (point1.X - cx) * (point1.X - cx) +
                    (point1.Y - cy) * (point1.Y - cy) -
                    radius * radius;

                det = B * B - 4 * A * C;
                if ((A <= 0.0000001) || (det < 0))
                {
                    // No real solutions.
                    intersection1 = new PointF(float.NaN, float.NaN);
                    intersection2 = new PointF(float.NaN, float.NaN);
                    return 0;
                }
                else if (det == 0)
                {
                    // One solution.
                    t = -B / (2 * A);
                    intersection1 =
                        new PointF(point1.X + t * dx, point1.Y + t * dy);
                    intersection2 = new PointF(float.NaN, float.NaN);
                    return 1;
                }
                else
                {
                    // Two solutions.
                    t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                    intersection1 =
                        new PointF(point1.X + t * dx, point1.Y + t * dy);
                    t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                    intersection2 =
                        new PointF(point1.X + t * dx, point1.Y + t * dy);
                    return 2;
                }
            }
        }
    }


}
