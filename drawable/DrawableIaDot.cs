using NN_try_1;
using NN_try_1.baseNN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GestionaleBeB
{
    namespace DotTimeLine
    {
        public class Vector2
        {

            public double X=0;
            public double Y=0;
            public double this[int index]
            {
                get
                {
                    if (index==1)
                    {
                        return Y;
                    }
                    return X;
                }
            }
            public Vector2() { }
            public Vector2(PointF p) {
                X = p.X;
                Y = p.Y;
            }
            public Vector2(double x, double y)
            {
                X = x;
                Y = y;
            }
            public static Vector2 operator -(Vector2 a, Vector2 b)
            {
                return new Vector2(a.X - b.X, a.Y - b.Y);
            }
            public static Vector2 operator +(PointF p, Vector2 v)
            {
                return new Vector2(p.X + v.X, p.Y + v.Y);
            }
            public static Vector2 operator *(Vector2 v, double scalar)
            {
                return new Vector2(v.X * scalar, v.Y * scalar);
            }
            public static implicit operator PointF(Vector2 v)
            {
                return new PointF((float) v.X,(float) v.Y);
            }
            public static implicit operator Vector2(PointF p)
            {
                return new Vector2(p.X, p.Y);
            }
            

            private const double DegToRad = Math.PI / 180;

            public Vector2 Rotated(double degrees)
            {
                var t = new Vector2(this);
                t.Rotate(degrees);
                return t;
            }
            public void Rotate( double degrees)
            {
                RotateRadians(degrees * DegToRad);
            }

            public void RotateRadians(double radians)
            {
                var ca = System.Math.Cos(radians);
                var sa = System.Math.Sin(radians);
                X = (ca * X - sa * Y);
                Y = (sa * X + ca * Y);
               // return new Vector(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
            }
            public double AngleDiffer(Vector2 vector2)
            {
                return AngleBetween(this, vector2);
            }
            public static double AngleBetween(Vector2 vector1, Vector2 vector2)
            {
                double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
                double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

                return Math.Atan2(sin, cos) * (180 / Math.PI);
            }
            public float sqrdDistance(Vector2 dest)
            {
                float px, py;
                px = (float)dest.X - (float)X;
                py = (float)dest.Y - (float)Y;
                px *= px;
                py *= py;
                return px + py;
            }
            public static Vector2 Zero = new Vector2(0f, 0f);
            public float MagnitudeSqrd
            {
                get
                {
                    return (float) (X * X + Y * Y);
                }
            }
            public float Magnitude
            {
                get
                {
                    return (float) Math.Sqrt(X *X + Y*Y);
                }
                set
                {
                    var p = this.Normalized * value;
                    X = p.X;
                    Y = p.Y;
                    
                }
            }

            public Vector2 Normalized
            {
                get
                {
                    double distance = Math.Sqrt(X * X + Y * Y);
                    return new Vector2(X / distance, Y / distance);
                }
            }
        }
       
        public class DrawableIaDot : DrawableDot 
        {

           

        NN_try_1.drawable.Collider.LineCollider linecollider = new NN_try_1.drawable.Collider.LineCollider();
            DrawableLine line;
            public DrawableDot obstacle;
            Vector2 lookVector = new Vector2(100,0);
            DrawableDot tester;
            NN_try_1.drawable.Collider.
                CircleCollider my_collider= new NN_try_1.drawable.Collider.CircleCollider();

            public NN_try_1.drawable.Collider.
                CircleCollider food_collider = new NN_try_1.drawable.Collider.CircleCollider();

            public brain mine;

            private void AIing(bool view)
            {
                //prepare input
                var l = new List<float>();
                var t = ((Vector2)this.Pos - obstacle.Pos).MagnitudeSqrd;
                l.Add(last_move);
                l.Add(last_turn);
                l.Add(t);
                l.Add((view)?1.0f:0.0f);

                if (mine == null) return;
                mine.SignalSend(l);
            }

            //Ai section
            //punteggio
            //allontanarsi dal cibo abbassa la sopravvivenza
            //avvicinarsi al cibo aumenta la sopravvivenza
            //mangiare il cibo aumenta la sopravvivenza

            //input necessari//
            //2 magnitudine x y  posizione attore cibo
            //1 sensore visione raytrace con magnitudesqrd della collisione
            //2 input memoria

            //output necessari//
            //1 per avanti e indietro
            //1 per destra e sinistra
            //2 output memoria
            float pricePenalities = 0.2f;
            float price = 250.5f;
            public float fitness = 500f;
            public float eatCount = 0.0f;
            private void priceWin()
            {
                eatCount += price;
                fitness += 20f;
            }
            private void calculatePricePenalities(float val)
            {
                fitness += pricePenalities * val;

            }
            //utilizzare i seguenti durante il ciclo vitale
            float turn_costant = 0.32f;
            float Cicle_Movement_speed = 1.0f;
            public float timescale = 2.2f;
            float TurnConstant
            {
                get { return turn_costant * timescale; }
                set { turn_costant = value; }
            }
            float CicleMovementSpeed
            {
                get
                {
                    return Cicle_Movement_speed * timescale;
                }
                set
                {
                    Cicle_Movement_speed = value;
                }
            }
            float last_turn;
            int counted;
            float medium=0;
            public void AiTurn(float val)
            {
                last_turn = (last_turn*0.02f) + val*.2f;
                float tran = val * 2 - 0.5f;
                Rotate(tran * TurnConstant);
                medium += 1;
                fitness -= 0.2f;
                
            }
            float last_move;
            public void AiForward(float val)
            {
                last_move = (last_move * 0.02f) + val*0.02f;
                float tran = val * 2 - 0.5f;
                var l = lookVector.Normalized;
                l = l * (CicleMovementSpeed*tran);
                this.Pos = l + this.Pos;
                fitness -= 0.2f;
                //this.Pos
            }

            List<OutputResponse> opr;
            public DrawableIaDot()
            {
                this.Pos = new Vector2(100, 100);
                line = new DrawableLine();
                line.Pos = Pos;  
                
                line.Pos2 = line.Pos + lookVector;

                obstacle = new DrawableDot();
                obstacle.Pos = new PointF(150, 150);
                obstacle.Radius = 5;
                    obstacle.DotBrush=(SolidBrush)Brushes.DarkCyan;
                food_collider.position = obstacle.Pos;
                food_collider.radius = obstacle.Radius;

                tester = new DrawableDot();
                tester.Pos = new PointF(50, 50);
                tester.Radius = 1;
                opr = new List<OutputResponse>();
                opr.Add(AiForward);
                opr.Add(AiTurn);
                mine = new brain(4, 3, 3, 2, opr);
                fudjunction.LinePen = Pens.AliceBlue;
                knownpoint.LinePen = Pens.BlueViolet;
                oldMagnitude = ((Vector2)this.Pos - obstacle.Pos).MagnitudeSqrd;

                var r = new Random();
                food_collider.position = new Vector2(r.Next(305, 350 * 2), r.Next(305, 350 * 2));
                obstacle.Pos = food_collider.position;
            }
            
            
            public  void Rotate(double degrees)
            {
                lookVector.Rotate(degrees);
            }
            private float oldMagnitude;
            private DrawableLine fudjunction = new DrawableLine();
            private DrawableLine knownpoint = new DrawableLine();
            public override void Draw(Graphics e)
            {
                counted++;
                medium -= 0.2f;
                if (30 < counted)
                {
                    counted = 0;
                    if (medium < 0)
                    {
                        Console.WriteLine("brain ded whith fitness "+fitness);
                        this.mine = new brain(4, 3, 3, 2, opr);
                        fitness = 0;
                    }
                }
                fitness -= 5f;//prezzo per vivere
                base.Draw(e);
                lookVector.Magnitude = 800;
                line.Pos = this.Pos;
                line.Pos2 = line.Pos + lookVector;
                linecollider.pos = this.Pos;
                linecollider.dir = lookVector;
                my_collider.position = this.Pos;
                my_collider.radius = this.Radius;
                /*knownpoint.Pos = this.Pos;
                knownpoint.Pos2 = new Vector2(10, 10);
                knownpoint.Draw(e);*/
                if (((Vector2)this.Pos - obstacle.Pos).MagnitudeSqrd > oldMagnitude)
                {
                   
                    calculatePricePenalities(-.2f);
                }
                else
                {

                    calculatePricePenalities(1.2f);
                }
                



                if (obstacle != null)
                {
                    fudjunction.Pos = this.Pos;
                    fudjunction.Pos2 = food_collider.position;


                    fudjunction.Draw(e);

                    if (linecollider.CircelIntersect(obstacle.Pos, obstacle.Radius, e) > 0)
                    {
                        line.LinePen = Pens.Green;

                        AIing(true);
                    }
                    else
                    {
                         AIing(false);
                        line.LinePen = Pens.Red;
                    }
                }
                tester.Pos = line.Pos2;

                line.Draw(e);

                if (my_collider != null && obstacle != null)
                {
                    if (my_collider.CircleIntersect(food_collider))
                    {
                        var r = new Random();
                        food_collider.position = new Vector2(r.Next(305, 350 * 2), r.Next(305, 350 * 2));
                        obstacle.Pos = food_collider.position;

                        this.DotBrush = (SolidBrush)Brushes.Bisque;
                        //qui abbiamo mangiato
                        priceWin();

                    }
                }
                if (obstacle!=null)
                {

                    obstacle.Draw(e);

                }
                tester.Draw(e);

                
            }

            
           
        }
    }
}
