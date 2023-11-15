using GestionaleBeB.DotTimeLine;
using NN_try_1.drawable.Collider;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NN_try_1
{

    namespace baseNN
    {
        public class ReplayGlance
        {
            private List<AgentEx> _agents;
            public List<AgentEx> Agents
            {
                get { return _agents ??= new List<AgentEx>(); }
                set { _agents = value; }
            }

            private Simulation Instance;
            public ReplayGlance(Simulation inst)
            {
                Instance = inst;
            }
            public void getGlance()
            {
                Agents = new List<AgentEx>();
                List<BrainEx> l = new List<BrainEx>();
                lock (Instance.glanceLock)
                {
                    foreach(BrainEx b in Instance.glanceList)
                    {
                        l.Add(b.Clone());
                    }
                }
                foreach(var b in l)
                {
                    Agents.Add(new AgentEx(b));
                }
            }
            private int era = 700;
            private int eraCount = 0;
            public void Draw(Graphics e)
            {
                eraCount++;
                int drawCount = 0;
                int drawCountMax = 8;
                foreach (var b in Agents)
                {
                    drawCount++;
                    b.Tick();
                    b.Draw(e);
                    if (drawCount > drawCountMax) break;
                }
                
                var f = new Font("Calibri", 16);
                Brush ef = new SolidBrush(Color.DarkCyan);
                e.DrawString(eraCount.ToString(), f, ef, 10, 10);
                lock (Instance.generationLock)
                {
                    e.DrawString(Instance.generationCount.ToString(), f, ef, 10, 32);
                }
                f.Dispose();
                ef.Dispose();
                if (eraCount > era)
                {
                    eraCount = 0;
                    getGlance();
                    
                }

            }
        }

        public class Simulation
        {
            public readonly object glanceLock = new object();
            public readonly object generationLock = new object();
            public int generationCount;
            private List<BrainEx> __glanceList;
            public List<BrainEx> glanceList
            {
                get { return __glanceList ??= new List<BrainEx>(); }
                set { __glanceList = value; }
            }
            public void RunSimulation()
            {
                Thread t = new Thread(new ThreadStart(Simulatious));
                t.Start();
            }
            private void Simulatious()
            {
                while(true)
                {
                    lock (generationLock)
                    {
                        generationCount += 1;
                    }
                    Tick();
                }

            }

            private int era = 500;
            private int eraCount = 0;
            private List<AgentEx> _agents;
            private List<AgentEx> Agents
            {
                get { return _agents ??= new List<AgentEx>(); }
                set { _agents = value; }
            }

            public Simulation(int individue)
            {
                for(int i = 0; i < individue; i++)
                {
                    Agents.Add(new AgentEx());
                }
            }
            private void Tick()
            {
                eraCount++;
                int l = Agents.Count;//mettere multithread

                var t = new Task(() => batchWork(0));
                t.Start();
                var t2 = new Task(() => batchWork(1));
                t2.Start();

                var t3 = new Task(() => batchWork(2));
                t3.Start();
                var t4 = new Task(() => batchWork(3));
                t4.Start();
                t.Wait();
                t2.Wait();
                t3.Wait();
                t4.Wait();


              /*  for (int i = 0; i < l; i++)
                {
                    Agents[i].Tick();
                }*/
                if (eraCount > era) EndEra(l);
            }
            private void batchWork(int count)
            {
                int _lcountl = Agents.Count / 4;
                for (int i = _lcountl * count; i < _lcountl * (count + 1); i++)
                {
                    Agents[i].Tick();
                }
            }

            private void EndEra(int maxindex)
            {
                var quart = maxindex / 4;
                var list22 = from x in Agents
                             orderby x.fitness descending
                             select x;
                Agents = list22.ToList();
                Console.Write("\n");
                eraCount = 0;
                lock (glanceLock)
                {
                    glanceList = new List<BrainEx>();
                    for (int i =0; i < 8; i++)
                    {
                        Console.WriteLine("Fitness: " + Agents[i].fitness);
                        Console.WriteLine(Agents[i].Position.X + "  " + Agents[i].Position.Y);
                        glanceList.Add(Agents[i].mind.Clone());
                    }
                }
                for (int i = 0; i < quart; i++)
                {
                    Agents[i].reset();
                    Agents[quart + i].mind = Agents[i].mind + Agents[i + 1].mind;
                    Agents[quart + i].mind.mutate(0.30f);
                    Agents[quart + i].reset();


                    Agents[quart*2 + i].mind = Agents[i].mind + Agents[i + 1].mind;
                    Agents[quart + i].mind.mutate(0.60f);
                    Agents[quart*2 + i].reset();

                    Agents[quart * 3 + i] = new AgentEx(Agents[i].mind.Clone());
                    Agents[quart*3 + i].mind.mutate(0.90f);
                    Agents[quart*3 + i].reset();
                    
                }

            }
        }
        public class ActivationFunction
        {
            public static float ReLu(float x)
            {
                return (x > 0.0f) ? x : 0.0f;
            }
            public static float tanh(double x)
            {
                return (float)((Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x)));
            }
            public static float LogSigmoid(float x)
            {
                if (x < -45.0f) return 0.0f;
                else if (x > 45.0f) return 1.0f;
                else return 1.0f / (1.0f + (float)Math.Exp(-x));
            }
        }
        public class AgentEx{
            public BrainEx mind;
            int seedFood = 42;
            private Random randFoodPos = new Random(42);
            public AgentEx(BrainEx mind)
            { this.mind = mind;
                reset();
            }
                public AgentEx()
            {
                Position = new Vector2(50, 50);
                mind = new BrainEx(3, 3, 3, 2);
                ranFood();
                LookAt = new Vector2(450, 0);
            }
            public void reset()
            {
                fitness = 0;
                seedFood = 42;
                randFoodPos = new Random(42);
                Position = new Vector2(50, 50);
                ranFood();
                LookAt = new Vector2(450, 0);
            }
            private void ranFood()
            {
                food.position.X = randFoodPos.Next(0, 350);
                food.position.Y = randFoodPos.Next(0, 350);
            }
            public void Draw(Graphics e) {
                DrawableDot gme = new DrawableDot();
                gme.Pos = Position;
                gme.Radius = Radius;
                gme.Draw(e);
                

                DrawableLine gDir = new DrawableLine();
                gDir.Pos = Position;
                gDir.Pos2 = Position + LookAt;
                gDir.Draw(e);

                //sensor
                DrawableLine clgDir = new DrawableLine();
                clgDir.Pos = Position;
                clgDir.Pos2 = Position + sensorLeft.dir;
                clgDir.Draw(e);
                DrawableLine crgDir = new DrawableLine();
                crgDir.Pos = Position;
                crgDir.Pos2 = Position + sensorRight.dir;
                crgDir.Draw(e);

                DrawableDot gFood = new DrawableDot();
                gFood.Pos = food.position;
                gFood.Radius = (float)food.radius;
                gFood.Draw(e);
            }
            public void Tick()
            {
                LookAt.Magnitude = 350;//vecchio bug da sistemare probabile e dentro il calcolo della collisione

                var dir = (food.position - Position).Normalized;
                var c = lineColl.CircelIntersect(food) > 0 ? 1.0f : -1.0f;
                fitness += 2 * c;
                var cl = sensorLeft.CircelIntersect(food) > 0 ? 1.0f : -1.0f;
                var cr = sensorRight.CircelIntersect(food) > 0 ? 1.0f : -1.0f;
                var ab = LookAt.AngleDiffer(dir);
                var o = mind.tick(new float[3] {(float)cl, (float)cr, c });
                LookAt.Rotate(o[0] * turnRate * timescale);
                LookAt = LookAt * 1;
                var olddist = (Position-food.position).MagnitudeSqrd;
                Position += (LookAt.Normalized * speed * timescale) * (o[1]) ;

                //fitness -= 0.004f;
                if (((Position - food.position).MagnitudeSqrd) > olddist)
                {
                    //fitness -= 0.004f;
                }
                else { 
                    //fitness += 0.002f;
                       }

                if (me.CircleIntersect(food))
                {
                    fitness++;
                    randFoodPos = new Random(++seedFood);
                    ranFood();
                }
                
            }
            public float fitness=0;

            private CircleCollider food = new CircleCollider();
            private CircleCollider me = new CircleCollider();
            private LineCollider lineColl = new LineCollider();
            private LineCollider sensorLeft = new LineCollider();
            private LineCollider sensorRight = new LineCollider();
            public Vector2 Position
            {
                get { return me.position; }
                set
                {
                    me.position = value;
                    lineColl.pos = value;
                    sensorLeft.pos = value;
                    sensorRight.pos = value;
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
                set { 
                    lineColl.dir = value;
                    sensorLeft.dir = value.Rotated(-12f);
                    sensorRight.dir = value.Rotated(12f);
                }
            }

            private float speed = 3f;
            private float turnRate = 3f;
            private float timescale = 1f;

        }
        public class BrainEx {
            public BrainEx Clone()
            {
                var toret = new BrainEx();
                toret.hid1 = new layer(this.hid1);
                toret.hid2 = new layer(this.hid2);
                toret.output = new layer(this.output);
                return toret;
            }
            public BrainEx() { }
            public layer hid1;
            public layer hid2;
            public layer output;

            public BrainEx(int input, int hid1, int hid2, int output)
            {
                this.hid1 = new layer(input, hid1, NeuronActivationFunction.relu);
                this.hid2 = new layer(hid1, hid2, NeuronActivationFunction.relu);
                this.output = new layer(hid2, output, NeuronActivationFunction.tanh);
            }




            public BrainEx(BrainEx a, BrainEx b)
            {
                hid1 = new layer(a.hid1, b.hid1);
                hid2 = new layer(a.hid2, b.hid2);
                output = new layer(a.output, b.output);

            }
            public static BrainEx operator +(BrainEx a, BrainEx b)
            {
                return new BrainEx(a, b);
            }
           



            public float[] tick(float[] input)
            {
                hid1.doStress(ref input);
                hid2.doStress(ref hid1.output);
                output.doStress(ref hid2.output);
                return output.output;
            }
            public void mutate(float percNeuronWeight)
            {
                hid1.mutate(percNeuronWeight, percNeuronWeight);
                hid2.mutate(percNeuronWeight, percNeuronWeight);
                output.mutate(percNeuronWeight, percNeuronWeight);
            }

        }
        public enum NeuronActivationFunction
        {
            tanh, relu, sigmoid, none
        }

        public class layer {
            float[] input;// neuron in previus layer
            public float[] output;//available for the next layer
            public NeuronActivationFunction func;// what kind of operation to do on the input weighted and bias summed
             public float[,] inputWeight;// needed to multiply for every one 0...1 range
            public float[] bias;// summed just once after 
            public layer(int previousLayerCount,int thisLayerCount, NeuronActivationFunction f)
            {
                func = f;
                //initializeInput();
                initializeBias(thisLayerCount);
                initializeWeight(previousLayerCount, thisLayerCount);
                output = new float[thisLayerCount];
            }
            public layer(layer into)
            {
                inputWeight = (float[,])into.inputWeight.Clone();
                bias = (float[])into.bias.Clone();
                func = into.func;
                output = new float[into.output.Length];
            }
            public layer(layer a, layer b)
            {
                var v1 = a.inputWeight.GetLength(0);
                var v2 = a.inputWeight.GetLength(1);
                var vb = a.bias.Length;
                bias = new float[vb];
                inputWeight = new float[v1, v2];
                var r = new Random();
                for (int i = 0; i < v1; i++)
                {
                    for(int e = 0;e < v2; e++)
                    {
                        inputWeight[i, e] = (r.Next(0, 2) > 0) ? a.inputWeight[i, e] : b.inputWeight[i, e];
                    }
                }
                for (int i = 0; i < vb; i++)
                {
                    bias[i] = (r.Next(0, 2) > 0) ? a.bias[i] : b.bias[i];

                }
                func = (r.Next(0, 2) > 0) ? a.func:b.func;
                output = new float[a.output.Length];
            }
            public layer(float [,] inwe, float[] bi, NeuronActivationFunction f)
            {
                inputWeight = (float[,])inwe.Clone();
                bias = (float[])bi.Clone();
                func = f;
            }
            private void initializeBias(int thisLayerCount)
            {
                bias = new float[thisLayerCount];
                var r = new Random();
                for (int i = 0; i< thisLayerCount; i++)
                {
                    bias[i] = (float)r.NextDouble() * 12.0f;
                }
            }
            public void doStress(ref float[] incomingSignalValue /*incoming output*/)
            {
                for(int  i= 0; i < inputWeight.GetLength(0); i++)
                {
                    for(int o = 0; o < incomingSignalValue.Length; o++) {
                        output[i] += incomingSignalValue[o] * inputWeight[i,o];
                    }
                    output[i] += bias[i];
                }
                doFunc();
            }
            private void doFunc()
            {
                
                    switch (func)
                    {
                        case NeuronActivationFunction.relu:

                            for (int i = 0; i < output.Length; i++)
                            {
                                output[i] = ActivationFunction.ReLu(output[i]);
                            }
                            break;
                        case NeuronActivationFunction.tanh:

                            for (int i = 0; i < output.Length; i++)
                            {
                                output[i] = ActivationFunction.tanh(output[i]);
                            }
                            break;
                        case NeuronActivationFunction.sigmoid:
                            for (int i = 0; i < output.Length; i++)
                            {
                                output[i] = ActivationFunction.LogSigmoid(output[i]);
                            }
                            break;
                        default:
                            break;
                    }
                    
                
            }
            private void initializeWeight(int previousLayerCount, int thisLayerCount)
            {
                inputWeight = new float[thisLayerCount,previousLayerCount];

                var r = new Random();
                for (int i = 0; i < thisLayerCount; i++)
                {
                    for (int p = 0; p < previousLayerCount; p++)
                    {
                        inputWeight[i,p] = (float)r.NextDouble() * 2 - 1;

                    }
                }
            }

            public void mutate(float perconnection, float pernode)
            {
                var imax = Math.Round(inputWeight.GetLength(0) * pernode);
                var t = new Random();
                int indexA = 0;
                for (int u = 0; u < imax; u++)
                {

                    indexA = t.Next(0, inputWeight.GetLength(0) - 1);


                    var imaxB = Math.Round(inputWeight.GetLength(1) * perconnection);
                    
                    for (int i = 0; i < imaxB; i++)
                    {
                        int index = t.Next(0, inputWeight.GetLength(1) - 1);
                        inputWeight[indexA,index] = (float)t.NextDouble() * 2 - 1;
                    }

                }


                var imaxC = Math.Round(bias.Length * pernode);
                for (int i = 0; i < imaxC; i++)
                {
                    indexA = t.Next(0, bias.Length - 1);
                    bias[indexA] = (float)t.NextDouble() * 12.0f;
                }


            }
        }
    }
}
