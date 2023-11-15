using GestionaleBeB.DotTimeLine;
using NN_try_1.drawable.Collider;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN_try_1
{
    class NN
    {
    }

    namespace baseNN
    {
        public class brain { // predisposta base per algoritmo genetico
            public List<node> input = new List<node>();
            public List<node> output = new List<node>();
            public List<node> hidden1 = new List<node>();
            public List<node> hidden2 = new List<node>();
            enum neuronType { input, output, neuron};
            private brain(bool __internal){


             }
            public brain()
            {
                prepareNeuronLayer(1,ref input,neuronType.input);
                prepareNeuronLayer(1, ref hidden1);
                prepareNeuronLayer(1,ref  hidden2);
                prepareNeuronLayer(1,ref  output, neuronType.output);

                linkLayer(ref input, ref hidden1);
                linkLayer(ref hidden1, ref hidden2);
                linkLayer(ref hidden2, ref output);

                input[0].signaling();
            }
            public List<OutputResponse> resp;
            public brain(int input, int hidden1, int hidden2, int output, List<OutputResponse> resp)
            {
                prepareNeuronLayer(input, ref this.input, neuronType.input);
                prepareNeuronLayer(hidden1, ref this.hidden1);
                prepareNeuronLayer(hidden2, ref this.hidden2);
                prepareNeuronLayer(output, ref this.output, neuronType.output);

                linkLayer(ref this.input, ref this.hidden1);
                linkLayer(ref this.hidden1, ref this.hidden2);
                linkLayer(ref this.hidden2, ref this.output);
                this.resp = resp;
                output_linkDelegate(resp);
            }
            public void output_linkDelegate(List<OutputResponse> del)
            {
                //if (del == null) return;
                for (int i = 0; i < del.Count; i++)
                {
                    ((output)this.output[i]).delega = del[i];
                }
            }
            public void SignalSend(List<float> inputs) 
            {
                for(int i = 0; i< input.Count; i++)
                {
                    var val = (input.Count > inputs.Count) ? inputs[i] : 0.0f;
                    input[i].activate(val, i);
                }
            }
            private void linkLayer(ref List<node> layerA, ref List<node> layerB, bool doWeight = true)
            {
                foreach(node n in layerA)
                {
                    n.sons = layerB;
                    //n.weight = new float[n.sons.Count];
                    if (doWeight)
                    {
                        n.setWeights();
                    }
                }
                int count = layerA.Count;
                foreach(node n in layerB)
                {
                    n.papano = count;
                }
            }
            private void prepareNeuronLayer(int no, ref List<node> _internal, neuronType t = neuronType.neuron)
            {
                
                    //List<node> _internal = neuronLayer;
                    var r = new Random();
                    for (int i = 0; i < no; i++)
                    {
                        switch (t)
                        {
                            case neuronType.input:
                                _internal.Add(new node());
                               // Console
                                    //.WriteLine("gothere into prepare input 00");
                                break;
                            case neuronType.output:
                                _internal.Add(new output());
                                //Console
                                    //.WriteLine("gothere into prepare input 11");
                                break;
                            default:
                                _internal.Add(new neuron());
                                //Console
                                    //.WriteLine("gothere into prepare input 22");
                                break;
                        }
                        //Console
                                    //.WriteLine("gothere into prepare input exited");
                        _internal[i].bias = (((float)r.NextDouble()) * 2.223f -1) * 5;
                        
                    }
                
            }
            
            public void Mutate(float percent ,ref List<node> _internal)
            {
                //List<node> _internal = list;
                int max_indexno = (int)Math.Ceiling(_internal.Count * percent);
                int[] _checked = new int[max_indexno];
                int toCheck = -1;
                Random r = new Random();
                for (int i = 0; i < max_indexno; i++)
                {
                    
                   // do
                    //{
                        toCheck = r.Next(0, _internal.Count-1);
                    //} while (__exist(toCheck,ref _checked));
                    _checked[i] = toCheck;
                    
                    _internal[toCheck].bias = (((float)r.NextDouble()) * 2.223f - 1) * 5f;
                    _internal[toCheck].mutate(percent);
                }
            }
            public static brain operator +(brain a, brain b)
            {
                var r = new Random();
                var bb = new brain(true);
                bb.input = new List<node>();
                bb.hidden1 = new List<node>();
                bb.hidden2 = new List<node>();
                bb.output = new List<node>();
                for (int i =0;i< a.input.Count; i++)
                {
                    //bb.input.Add((r.Next(-10, 2) > 1) ? a.input[i] : b.input[i]);
                    bb.input.Add(new node());
                   

                        bb.input[i].bias =
                    ((r.Next(0, 2) > 1) ? a.input[i].bias : b.input[i].bias);
                    bb.input[i].weight =
                    ((r.Next(0, 2) > 1) ? (float[])a.input[i].weight.Clone() : (float[])b.input[i].weight.Clone());
                }
                for (int i = 0; i < a.hidden1.Count; i++)
                {
                    bb.hidden1.Add(new neuron());
                    bb.hidden1[i].bias=
                    ((r.Next(-0, 2) > 1) ? a.hidden1[i].bias : b.hidden1[i].bias);
                    bb.hidden1[i].weight =
                    ((r.Next(-0, 2) > 1) ?(float[]) a.hidden1[i].weight.Clone() : (float[])b.hidden1[i].weight.Clone());
                }
                for (int i = 0; i < a.hidden2.Count; i++)
                {
                    bb.hidden2.Add(new neuron());
                    bb.hidden2[i].bias =
                    ((r.Next(-0, 2) > 1) ? a.hidden2[i].bias : b.hidden2[i].bias);
                    bb.hidden2[i].weight =
                    ((r.Next(-0, 2) > 1) ? (float[])a.hidden2[i].weight.Clone() : (float[])b.hidden2[i].weight.Clone());
                    
                }
                for (int i = 0; i < a.output.Count; i++)
                {
                    //bb.output.Add((r.Next(-10, 2) > 1) ? a.output[i] : b.output[i]);
                    bb.output.Add(new output());
                    bb.output[i].bias =
                    ((r.Next(-0, 2) > 1) ? a.output[i].bias : b.output[i].bias);
                    bb.output[i].weight =
                    ((r.Next(-0, 2) > 1) ? (float[])a.output[i].weight.Clone() : (float[])b.output[i].weight.Clone());
                    
                }
                bb.linkLayer(ref bb.input, ref bb.hidden1,false) ;
                bb.linkLayer(ref bb.hidden1, ref bb.hidden2,false);
                bb.linkLayer(ref bb.hidden2, ref bb.output,false);
                

                //bb.resp = new List<OutputResponse>(a.resp);
                //bb.output_linkDelegate(bb.resp);

                return bb;
            }
            private bool __exist(int index, ref int[] array)
            {
                foreach(int i in array)
                {
                    if (i == index)
                    {
                        return true;
                    }

                }
                return false;
            }

        }
        public class node {
            public int papano;
            public int activationCount;
            public float value;
            public float bias = 0.5f;//questo serve allingresso

            public float[] weight = new float[0]; //questo serve alluscita
            public void setWeights()
            {
                weight = new float[sons.Count];
                var r = new Random();
                for(int y = 0; y<weight.Length;y++)
                {
                    weight[y] = (float)r.NextDouble() * 2 - 1;
                }
            }
            public void mutate(float perc)
            {
                int max_indexno = weight.Length * (int)perc;
                int toCheck = -1;
                Random r = new Random();
                for (int i = 0; i < max_indexno; i++)
                {

                    // do
                    //{
                    toCheck = r.Next(0, weight.Length - 1);
                    //} while (__exist(toCheck,ref _checked));
                    

                   
                    weight[toCheck]=(((float)r.NextDouble()) * 2.223f - 1) * 1f;
                }
            }
            public List<node> sons;

            public void activate(float v, int index)
            {
                value += v;
                activationCount++;
                if (activationCount >= papano) {
                    value += bias;
                    signaling();
                   
                }
            }
            public virtual void signaling()
            {
                

                for (int i =0; i< sons.Count; i++)
                {
                    var tosend = value * weight[i];
                    sons[i].activate(tosend,i);
                }

               
                value = 0.0f;
                activationCount = 0;
            }




        }
        public class neuron:node {
            public override void signaling()
            {
                //Console.WriteLine("olala we got here");
                this.value =(float) ReLu(this.value);
                base.signaling();
                value = 0;
                activationCount = 0;
            }
            public double ReLu(double x)
            {
                return (x > 0.0) ? x : 0.0;
            }
            
        }
        public class output: neuron
        {
            public OutputResponse delega = __del;
            public override void signaling()
            {
                this.value = (float)tanh(this.value);
                

                //Console.WriteLine("whath the fuck " + value);
                delega(value);
                value = 0;
                activationCount = 0;
            }
            public double tanh(double x)
            {
                return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x));
            }
            public double LogSigmoid(double x)
            {
                if (x < -45.0) return 0.0;
                else if (x > 45.0) return 1.0;
                else return 1.0 / (1.0 + Math.Exp(-x));
            }

            private static void __del(float response)
            {
                //Console.WriteLine("whath the fuck response " + response);
            }
        }
       
        public class agent
        {
            public brain mind;
            public List<OutputResponse> opr;
            public agent() {


                LookAt = new Vector2(450, 0); // view distance
                rebrain();

                resetPos();
                resetFood();
                foodDistance = (Position - food.position).MagnitudeSqrd;
                oldPosition = Position;
                oldAngleDiff = LookAt.AngleDiffer((Position - food.position).Normalized);
                //parameter initialization
                Radius = 3;
                food.radius = 1.5f;

            }
            float foodDistance;
            public void resetPos()
            {
                LookAt = new Vector2(450, 0);
                var r = new Random();
                //Position = new Vector2(r.NextDouble() * 350, r.NextDouble() * 350);
                Position = new Vector2(320/2,350/2);
            }
            public void MutateBrain(float perc)
            {
                mind.Mutate(perc, ref mind.input);
                mind.Mutate(perc, ref mind.hidden1);
                mind.Mutate(perc, ref mind.hidden2);
                mind.Mutate(perc, ref mind.output);
            }
            public void rebrain()
            {
                OutputLinkage();
                mind = new brain(7, 6, 6, 2, opr);
            }
            public void OutputLinkage()
            {
                opr = new List<OutputResponse>();
                opr.Add(AiForward);
                opr.Add(AiTurn);
                
            }
            public void OutputLinkageExternal()// to use on reproduction
            {
                OutputLinkage();
                mind.output_linkDelegate(opr);
            }

            public int state = 0;
            public void Draw(Graphics e) {



                var f = new DrawableDot();
                f.Pos = food.position;
                f.Radius =(float) food.radius;
                f.Draw(e);

                var m = new DrawableDot();
                switch (state){
                    case 0:
                        m.DotBrush = (SolidBrush)new SolidBrush(Color.FromArgb(255,255,255,255));
                        break;
                    case 1:
                        m.DotBrush = (SolidBrush)new SolidBrush(Color.Magenta);
                        break;
                    case 2:
                        m.DotBrush = (SolidBrush)new SolidBrush(Color.DarkCyan);
                        break;
                    case 3:
                        m.DotBrush = (SolidBrush)new SolidBrush(Color.YellowGreen);
                        break;
                    default:
                        m.DotBrush = (SolidBrush)new SolidBrush(Color.Red);
                        break;
                }
                m.Pos = Position;
                m.Radius = Radius;
                //m.DotBrush = (SolidBrush)new SolidBrush(mecolor);
                m.Draw(e);
                m.DotBrush.Dispose();

                var l = new DrawableLine();
                l.Pos = Position;
                l.Pos2 = Position + LookAt;
                l.Draw(e);

                var l2 = new DrawableLine();
                l2.Pos = food.position;
                l2.Pos2 = Position;
                l2.LinePen = lineColl.CircelIntersect(food) > 0 ? new Pen(Color.FromArgb(25, 22, 255, 22)) : new Pen(Color.FromArgb(25,255,255,255));
                l2.Draw(e);
                l2.LinePen.Dispose();
            }
            private double oldAngleDiff;
            private CircleCollider food = new CircleCollider();
            
            private CircleCollider me = new CircleCollider();
            private LineCollider lineColl = new LineCollider();
            public Vector2 Position
            {
                get { return me.position; }
                set { me.position = value;
                    lineColl.pos = value;
                }
            }
            public float Radius
            {
                get { return (float)me.radius; }
                set { me.radius = value; }
            }
            
            public Vector2 LookAt
            {
                get { return lineColl.dir; }
                set { lineColl.dir = value; }
            }

            private float speed = 3f;
            private float turnRate = 3f;
            private float timescale = 1f;
            public void AiForward(float value) {
                Position += (LookAt.Normalized * speed * timescale) * (value);
            }
            
            public void AiTurn(float value) {
                LookAt.Rotate((turnRate * (value)) * timescale);
            }
            
            public void resetFood() {
                var r = new Random(RandCountSeed);

                food.position = new Vector2(r.NextDouble() * 350, r.NextDouble() * 350);
                //food.position = new Vector2(350, 350);
            }

            
            
            public void AiIng() {
                
                List<float> input = new List<float>();

                input.Add(lineColl.CircelIntersect(food)>0?1:0);//visione del cibo

                var toInput = (Position - food.position).Normalized;

                input.Add(((float)toInput.X < 0 ? (float)toInput.X : 0));
                input.Add(((float)toInput.X > 0 ?  (float)toInput.X : 0));
                input.Add(((float)toInput.Y < 0 ? /*c'era un meno*/(float)toInput.Y : 0));
                input.Add(((float)toInput.Y > 0 ?  (float)toInput.Y : 0));
                Vector2 d = LookAt.Normalized;
                input.Add((float)d.X);
                input.Add((float)d.Y);

                mind.SignalSend(input);
            }
            public float fitness;
            public float eatCount;
            public Vector2 oldPosition;
            public Vector2 oldDir = new Vector2(1, 0);
            public int RandCountSeed = 42;
            public void Tick() {
                //if (fitness < -31000) return;
                if (lineColl.CircelIntersect(food) > 0) fitness += 1f;
                var AngleDiff = LookAt.AngleDiffer((Position - food.position).Normalized);
                if (oldAngleDiff > ((AngleDiff<0)?-AngleDiff:AngleDiff))
                {
                    fitness -= 20f;
                    
                }
                else
                {
                      fitness += 10f;

                    

                }
                oldAngleDiff = ((AngleDiff < 0)? -AngleDiff:AngleDiff);


                LookAt.Magnitude = 450;
                AiIng();
                if (me.CircleIntersect(food))
                {
                    // mangiato;
                    eatCount++;
                    RandCountSeed++;
                    fitness += 300f;
                    resetFood();
                }
                if((Position - food.position).MagnitudeSqrd-2f < foodDistance && foodDistance < (Position - food.position).MagnitudeSqrd + 2f)
                {
                    //fitness -= 10;
                    //Console.WriteLine("\n got ere: food " + foodDistance);
                }
                if((Position - food.position).MagnitudeSqrd > foodDistance)
                {
                    fitness -= 0.8f* ((Position - food.position).MagnitudeSqrd - foodDistance);

                    
                }
                else
                {
                    fitness += 0.02f * ((Position - food.position).MagnitudeSqrd - foodDistance);//good

                }
                if (LookAt.AngleDiffer(oldDir) < 0.1f)
                {
                    //fitness = -0.5f;
                }

                if (lineColl.CircelIntersect(food) > 0)
                    {
                        fitness += 300.5f;
                }
                else {
                    fitness -= 2.5f;
                }
                foodDistance = (Position - food.position).MagnitudeSqrd;
                
                
                fitness-= 10f;

                if (Position.X == oldPosition.X && Position.Y == oldPosition.Y)
                {
                    //Console.WriteLine("brainded");
                    //rebrain();
                    //fitness -= 2.7428768f;
                   // eatCount -= 0;
                } //cazzo fa
                else
                {
                    //fitness += 1f;
                }


                oldPosition = Position;
            }

        }
    }

    public delegate void OutputResponse(float response);
}
