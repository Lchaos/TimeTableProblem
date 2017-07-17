using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
    public class CSTimeTableProblem:TimeTableProblemBase
    {

        //public Random g_r = new Random();
        //public int solutionSize;
        public LevyFilght lv;
        public bool withCompared = false;
        public FindEggChangeMode chaningMode = FindEggChangeMode.LevyFilght;
        public int settingnums=2;
        public double findEggsRate=0.001;
        public int size=1000;
        public List<Nest> Nests = new List<Nest>();
        public int maxLoop = 10000;
        public double alpha =1;
        public double beta =1.9;
        //public long evaltimes;
        //public double minConf;
        //public Func<byte[], bool> calfFitness;
        public Func<byte[], int> calConf;
        //public Func<byte[],int, int> calPointConf;
        public Action<Nest, int> SetNewPoints;

        public override void init()
        {
            //base.init(conf);
            //InitGraph(conf.Nodes, conf.IM);

            lv = new LevyFilght(alpha, beta);
            for (int i = 0; i < size; i++)
            {
                Nests.Add(new Nest(this));
            }
            //maxLoop = conf.maxLoop;
        }


        public class Report
        {
            public int iteration = 0;
            public int minConf = 0;
            public int maxConf = 0;
            public double avrConf = 0;
            public override string ToString()
            {
                return string.Format("{0},{1},{2},{3}", iteration, minConf, maxConf, avrConf);
            }
        }

        public List<dynamic> m_lstReport = new List<dynamic>();
        public override void DoJob()
        {
            //base.DoJob();
            for (int i = 0; i < maxLoop; i++)
            {
                Parallel.ForEach<Nest>(Nests, (item,state) => //foreach (var item in Nests)
                {
                    item.sendEgg();
                    //if (item.solution.Info.Violation == 0)
                    //{
                    //    state.Break();
                    //    //break;
                    //}
                    evaltimes++;
                });
                minConf = Nests.Max(x => x.solution.Info.Violation);
                if (minConf == 0)
                {
                    break;
                }
                foreach (var item in Nests)
                {
                    if (g_r.NextDouble() < findEggsRate)
                    {
                        item.findEgg();
                        evaltimes++;
                        if (item.solution.Info.Violation == 0)
                        {
                            break;
                        }
                    }
                }
                minConf = Nests.Min(x => x.solution.Info.Violation);
                if (minConf == 0)
                {
                    break;
                }
                Report rep = new Report() { iteration = i, avrConf = Nests.Average(o => o.solution.Info.Violation), maxConf = Nests.Max(o => o.solution.Info.Violation), minConf = Nests.Min(o => o.solution.Info.Violation) };
                m_lstReport.Add(rep);
                Console.WriteLine(rep.ToString());
                Console.WriteLine(ServiceStack.Text.JsonSerializer.SerializeToString( Nests.First(kk=> kk.solution.Info.Violation==minConf).solution.Info));
            }
            Console.WriteLine(this.evaltimes + "," + minConf);
        }

        public class Nest
        {
            //int solutionSize;
            public Random g_r;
            public Func<byte[], int> calConf;
            //public Func<byte[], int, int> calPointConf;

            public bool WithCompared;
            public FindEggChangeMode chaningMode;
            public int settingnums;
            public int denum;
            public int subnum;
            public Nest(CSTimeTableProblem csParent)
            {
                //solutionSize = csParent.solutionSize;
                WithCompared = csParent.withCompared;
                chaningMode = csParent.chaningMode;
                //this.csParent = csParent;
                calConf = csParent.calConf;
                solution = new CourseInfoCollection(csParent.infos);
                orgsolution = new CourseInfoCollection(csParent.infos);

                //calPointConf = csParent.calPointConf;
                g_r = csParent.g_r;
                //this.SetNewColor = csParent.SetNewPoints;
                HardVioliationAdapter.HardVioliationSetting(solution);
                
                CountConf();
                orgsolution.CopyFrom(solution);
                lv = csParent.lv;

                denum = csParent.decnum;
                subnum = csParent.subnum;

                unselectedIDS = new List<int>(subnum);
                selectedIDS = new List<int>(subnum);
                for (int i = 0; i < subnum; i++)
                {
                    unselectedIDS.Add(i);
                }
            }

            public CourseInfoCollection solution;
            public CourseInfoCollection orgsolution;

            //public int mconf;
            //public int orgconf;
            public double fitness;

            private LevyFilght lv;
            //private int numNodes;
            private List<int> unselectedIDS;
            private List<int> selectedIDS;



            public void Backup()
            {
                orgsolution.CopyFrom(solution);
                //Array.Copy(solution, orgsolution, solutionSize);
                //orgconf = mconf;
            }


            public void CountConf()
            {
                solution.CountVioInfo();
                //mconf = solution.Info.Violation;
            }


            public void rollBack()
            {
                //Array.Copy(orgsolution, solution, solutionSize);
                solution.CopyFrom(orgsolution);
                //mconf = orgconf;
            }

            public void sendEgg()
            {
                long numChangedNodes = lv.next(1, solution.courseInfos.Subjects.Count);
                Backup();
                for (int i = 0; i < numChangedNodes; i++)
                {
                    int id = getRandomID();
                    SetNewColor(id);

                }
                this.CountConf();
                if (orgsolution.Info.Violation < solution.Info.Violation)
                {
                    rollBack();
                }
                resetSelectionIDS();
            }


            public void SetNewColor(int id)
            {
                solution.SetPositionRandom(id);
            }
            //public void SetNewColor(int id)
            //{
            //    byte orgcolor = solution[id];
            //    byte flag = (byte)g_r.Next(0, 2);

            //    int orgconf =  calPointConf( solution, id);


            //    switch (orgcolor)
            //    {
            //        case 0: solution[id] = ++flag; break;
            //        case 1: solution[id] = flag == 0 ? (byte)0 : (byte)2; break;
            //        case 2: solution[id] = flag; break;
            //        default:
            //            break;
            //    }

            //    int destconf = calPointConf(solution, id);

            //    mconf += destconf - orgconf;
            //}

            public int getRandomID()
            {
                int id;
                lock (g_r)
                {
                    id = g_r.Next(0, unselectedIDS.Count);
                }
                int ret = unselectedIDS[id];
                lock (unselectedIDS)
                {
                    unselectedIDS.RemoveAt(id);
                    selectedIDS.Add(ret);
                }

                return ret;
            }

            private void resetSelectionIDS()
            {
                lock (unselectedIDS)
                {
                    foreach (var item in selectedIDS)
                    {
                        unselectedIDS.Add(item);
                    }
                    selectedIDS.Clear();
                }
            }

            public void findEgg()
            {
                if (WithCompared)
                {
                    Backup();

                }
                int numChangedNodes = 0;

                lock (g_r)
                {
                    switch (chaningMode)
                    {
                        case FindEggChangeMode.AverageRandom:
                            numChangedNodes = g_r.Next(0, subnum);
                            break;
                        case FindEggChangeMode.LevyFilght:
                            numChangedNodes = (int)lv.next(1, subnum);
                            break;
                        case FindEggChangeMode.Setting:
                            numChangedNodes = settingnums;
                            break;
                        default:
                            break;
                    }
                }


                for (int i = 0; i < numChangedNodes; i++)
                {
                    int id = getRandomID();
                    SetNewColor( id);
                }

                //this.CountConf();

                if (WithCompared && orgsolution.Info.Violation < solution.Info.Violation) rollBack();

                resetSelectionIDS();
            }
        }
    }
    public enum FindEggChangeMode
    {
        AverageRandom,
        LevyFilght,
        Setting
    }
}
