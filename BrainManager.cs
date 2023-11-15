using GestionaleBeB.DotTimeLine;
using NN_try_1.baseNN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NN_try_1
{
    class BrainManager
    {
        List<DrawableIaDot> list;
        private int __actors;
        private Timer timer;
        public int lifespan = 1000;
        private int eracicle = 300;
        private int _eracicle = 300;
        public BrainManager(int actors) 
        {
            timer = new Timer();
            timer.Interval = lifespan;
            list = new List<DrawableIaDot>();
            __actors = actors;
            for(int i = 0; i < actors; i++)
            {
                list.Add(new DrawableIaDot());
                list[i].Radius = 8;
            }
            timer.Tick += new EventHandler(restart);
            //timer.Enabled = true;
            //timer.Start();

        }
        public void restart(object s, EventArgs p) {
            //timer.Stop();
            MessageBox.Show("wtf!");
            next_generation();
            //timer.Start();
            return;
        }
        bool end = false;
        public void tick(System.Drawing.Graphics e)
        {
            eracicle--;
            //Console.WriteLine(eracicle);
                if (eracicle < 0)
            {
                //MessageBox.Show("ordinario");
                next_generation();
                eracicle = _eracicle;
            }
            if (end) return;
            for (int i = 0; i < __actors; i++)
            {
                list[i].Draw(e);
            }
        }
        public void next_generation()
        {
            
            var list22 = from iaAgent in list
                    orderby iaAgent.fitness descending
                    orderby iaAgent.eatCount descending
                    select iaAgent;
            var list2 = list22.ToList();

            Console.WriteLine("fitness nivel");
            int __count = 0;
            foreach (DrawableIaDot i in list2)
            {
                
                    var r = new Random();
                i.food_collider.position = new Vector2(r.Next(0, 350*2), r.Next(0, 350*2));
                i.obstacle.Pos = i.food_collider.position;
                
               
                if ((__count) >= 6)
                {
                    
                    continue;
                }
                else { __count++; Console.WriteLine(i.fitness + " | " + i.eatCount); }
                i.fitness = 500f;
                i.eatCount = 0;
            }
            

            for(int i=0; i < (int)(list2.Count / 4); i += 1)
            {//accoppiamento
                
                list2[(int)(list2.Count / 4)+i].mine = list2[i].mine + list2[i + 1].mine;
                
                list2[(int)(list2.Count / 4) + i].mine.resp = new List<OutputResponse>();
                list2[(int)(list2.Count / 4) + i].mine.resp.Add(
                    list2[(int)(list2.Count / 4) + i].AiForward);
                list2[(int)(list2.Count / 4) + i].mine.resp.Add(
                list2[(int)(list2.Count / 4) + i].AiTurn);

                list2[(int)(list2.Count / 4) + i].mine.output_linkDelegate(list2[(int)(list2.Count / 4) + i].mine.resp);
                list2[(int)(list2.Count / 4) + i].DotBrush = (System.Drawing.SolidBrush)System.Drawing.Brushes.Cyan;
                ///section///

                list2[(int)(list2.Count / 4)*2 + i].mine = list2[i].mine + list2[i + 1].mine;

                list2[(int)(list2.Count / 4)*2 + i].mine.resp = new List<OutputResponse>();
                list2[(int)(list2.Count / 4)*2 + i].mine.resp.Add(
                    list2[(int)(list2.Count / 4)*2 + i].AiForward);
                list2[(int)(list2.Count / 4)*2 + i].mine.resp.Add(
                list2[(int)(list2.Count / 4)*2 + i].AiTurn);
                list2[(int)(list2.Count / 4)*2 + i].mine.output_linkDelegate(list2[(int)(list2.Count / 4)*2 + i].mine.resp);
                list2[(int)(list2.Count / 4)*2 + i].DotBrush = (System.Drawing.SolidBrush)System.Drawing.Brushes.Brown;

                list2[(int)(list2.Count / 4) * 2 + i].mine.Mutate(0.6f,ref list2[(int)(list2.Count / 4) * 2 + i].mine.input);
                list2[(int)(list2.Count / 4) * 2 + i].mine.Mutate(0.6f,ref list2[(int)(list2.Count / 4) * 2 + i].mine.hidden1);
                list2[(int)(list2.Count / 4) * 2 + i].mine.Mutate(0.6f,ref list2[(int)(list2.Count / 4) * 2 + i].mine.hidden2);
                list2[(int)(list2.Count / 4) * 2 + i].mine.Mutate(0.6f,ref list2[(int)(list2.Count / 4) * 2 + i].mine.output);
                ///section///


                list2[(int)(list2.Count / 4)*3 + i].mine = list2[i].mine + list2[i + 1].mine;

                list2[(int)(list2.Count / 4)*3 + i].mine.resp = new List<OutputResponse>();
                list2[(int)(list2.Count / 4)*3 + i].mine.resp.Add(
                    list2[(int)(list2.Count / 4)*3 + i].AiForward);
                list2[(int)(list2.Count / 4)*3 + i].mine.resp.Add(
                list2[(int)(list2.Count / 4)*3 + i].AiTurn);


                list2[(int)(list2.Count / 4)*3 + i].mine.output_linkDelegate(list2[(int)(list2.Count / 4)*3 + i].mine.resp);
                list2[(int)(list2.Count / 4)*3 + i].DotBrush = (System.Drawing.SolidBrush)System.Drawing.Brushes.YellowGreen;

                list2[(int)(list2.Count / 4) * 3 + i].mine.Mutate(1f,ref list2[(int)(list2.Count / 4) * 3 + i].mine.input);
                list2[(int)(list2.Count / 4) * 3 + i].mine.Mutate(1f,ref list2[(int)(list2.Count / 4) * 3 + i].mine.hidden1);
                list2[(int)(list2.Count / 4) * 3 + i].mine.Mutate(1f,ref list2[(int)(list2.Count / 4) * 3 + i].mine.hidden2);
                list2[(int)(list2.Count / 4) * 3 + i].mine.Mutate(1f,ref list2[(int)(list2.Count / 4) * 3 + i].mine.output);


            }

            list = list2;
        }
    }
    class BrainManage2
    {
        int actorCount = 0;
        public BrainManage2(int count)
        {
            actorCount = count;
            initialize();
        }
        List<agent> listAgent = new List<agent>();
        private void initialize()
        {
            for (int i = 0; i < actorCount; i++)
            {
                listAgent.Add(new agent());
            }
        }
        private void letTick()
        {
            foreach(agent a in listAgent)
            {
                a.Tick();
            }
        }
        public void taskTickThenDraw(int count, Graphics e)
        {
            if (!(listAgent.Count >= 4)) throw new ArgumentException("Index aren't enough", nameof(listAgent));

            var t = new Task( () => batchWork(0));
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
            Draw(count, e);
            era++;
            if (era > eracount)
            {
                eraEnd();
            }
        }
        private void batchWork(int count)
        {
            int _lcountl = listAgent.Count/4;
            for (int i = _lcountl * count; i< _lcountl * (count+1); i++)
            {
                listAgent[i].Tick();
            }
        }
        public void DrawTick(int count, Graphics e)
        {

            int __count = 0;
            foreach (agent a in listAgent)
            {
                a.Tick();
                
                if (__count++ < count) a.Draw(e); 
            }
            era++;
            if (era> eracount)
            {
                eraEnd();
            }
        }
        private void Draw(int count, Graphics e)
        {
            int __count = 0;
            foreach (agent a in listAgent)
            {
                if (++__count > count) break;
                a.Draw(e);
            }
            var f = new Font("Calibri", 16);
            Brush ef = new SolidBrush(Color.DarkCyan);
            e.DrawString(era.ToString(), f, ef, 10, 10);
            f.Dispose();
            ef.Dispose();
        }

        int eracount = 500;
        int era = 0;
        
        private void eraEnd()
        {
            era = 0;
            eracount+=0;
            var list22 = from iaAgent in listAgent
                         orderby iaAgent.fitness descending
                         orderby iaAgent.eatCount descending
                         select iaAgent;
            listAgent = list22.ToList();

            var __PrintCount = 6;
            var _PrintCount = 0;
            Console.Write("\n");
            foreach(var elem in listAgent)
            {
                
                if(_PrintCount++ < __PrintCount)
                {
                    Console.WriteLine("Fitness: " + elem.fitness + " Eated: " + elem.eatCount);
                }
                elem.RandCountSeed = 42;
                elem.resetFood();
                elem.resetPos();
                elem.fitness = 0;
                elem.eatCount = 0;
            }

            for (int i = 0; i < (int)(listAgent.Count / 4); i++)
            {
                listAgent[i].state += 1;
                int count = listAgent.Count;
                listAgent[(int)(count / 4) + i].mind = listAgent[i].mind + listAgent[i + 1].mind;
                listAgent[(int)(count / 4) + i].OutputLinkageExternal();
                listAgent[(int)(count / 4) + i * 1].MutateBrain(0.2f);
                listAgent[(int)(count / 4) + i].state = 1;

                listAgent[(int)(count / 4) + i *2].mind = listAgent[i].mind + listAgent[i + 1].mind;
                listAgent[(int)(count / 4) + i*2].OutputLinkageExternal();
                listAgent[(int)(count / 4) + i * 2].MutateBrain(0.8f);
                listAgent[(int)(count / 4) + i*2].state = 2;

                /*listAgent[(int)(count / 4) + i*3].mind = listAgent[i].mind + listAgent[i + 1].mind;
                listAgent[(int)(count / 4) + i*3].OutputLinkageExternal();
                listAgent[(int)(count / 4) + i * 3].MutateBrain(50f);*/
                listAgent[(int)(count / 4) + i * 3].rebrain();
                listAgent[(int)(count / 4) + i * 3].state = 3;
                listAgent[(int)(count / 4) + i * 3].MutateBrain(1.2f);

            }
        }
    }
}
