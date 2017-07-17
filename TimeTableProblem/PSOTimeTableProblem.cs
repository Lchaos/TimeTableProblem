using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
   public  class PSOTimeTableProblem :TimeTableProblemBase
    {
        public int Size = 200;
        public int MaxLoop = 5000;
        public Bird gBest;
        private Bird[] Birds;
        public double c1 = 5;
        public double c2 = 2;
        public double w = 0.05;

        public bool usenorm;

        public class Bird
        {
            public CourseInfoCollection Solution;
            //public int conf;
            public Bird pBest;
            public int preHammingDistance = 0;

            public double Fitness = 0;
        }

        public override void init()
        {
            Birds = new Bird[Size];
            for (int i = 0; i < Size; i++)
            {
                Birds[i] = new Bird();
                Bird tmp = Birds[i];
                tmp.pBest = new Bird();
                tmp.Solution = new CourseInfoCollection(infos);
                HardVioliationAdapter.HardVioliationSetting(tmp.Solution);
                tmp.Solution.CountVioInfo();
                tmp.pBest.Solution = new CourseInfoCollection(infos);
                tmp.pBest.Solution.CopyFrom(tmp.Solution);
            }
            int sumConf = Birds.Sum(i => i.Solution.Info.Violation);
            foreach (var item in Birds)
            {
                item.Fitness = 1.0 - (double)item.Solution.Info.Violation / (double)sumConf;
                item.pBest.Fitness = item.Fitness;
            }
            gBest = new Bird();
            gBest.Solution = new CourseInfoCollection(infos);
            gBest.Solution.Info.Violation= -1;
            RefreshGBEST();
            foreach (var item in Birds)
            {
                foreach (var iitem in item.Solution)
                {
                    if (iitem.ToId() < 0)
                    {
                        throw new Exception();
                    }
                    foreach (var iiitem in item.Solution)
                    {
                        if (iiitem.index != iitem.index)
                        {
                            if (infos[iitem.index].PlaceID == infos[iiitem.index].PlaceID && iiitem.ToId() == iitem.ToId())
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
            }

        }

        public int HammingDistance(CourseInfoCollection sol1, CourseInfoCollection sol2)
        {
            int ret = 0;
            for (int i = sol1.DeterminedCourseNum; i < sol1.Count; i++)
            {
                if ((sol1[i].Equals(sol2[i]))) continue;
                ret++;
            }
            return ret;
        }

        public double Similarity(CourseInfoCollection sol1, CourseInfoCollection sol2)
        {
            if(usenorm) return 1;
            int ham = HammingDistance(sol1, sol2);
            return 1.0 - (double)ham / (double)subnum;
        }

        public override void DoJob()
        {

            double[] rate = new double[3];
            CourseInfoCollection preBirdSolution = new CourseInfoCollection(infos);
            //var decnum = preBirdSolution.DeterminedCourseNum;
           // var subnum = preBirdSolution.courseInfos.Subjects.Count;
            for (int i = 0; i < MaxLoop; i++)
            {

                double r1 = g_r.NextDouble();
                double r2 = g_r.NextDouble();

                Parallel.ForEach<Bird>(Birds, (tmp) =>
                {
                    //try
                    //{
                        //Bird tmp = Birds[j];
                        double[] Pset = new double[3];
                        double sim1 = Similarity(tmp.Solution, tmp.pBest.Solution);
                        double sim2 = Similarity(tmp.Solution, gBest.Solution);
                        double sim3 = 1 - (double)tmp.preHammingDistance / (double)(subnum);
                        preBirdSolution.CopyFrom(tmp.Solution);// Array.Copy(tmp.Solution, , m_numNodes);

                        double baseNum = w * sim3 + c1 * sim1 * r1 + c2 * sim2 * r2;//w + c1 * r1 + c2 * r2; 
                        if (baseNum == 0) return;
                        Pset[0] = (w * sim3) / baseNum;//w / baseNum;
                        Pset[1] = (c1 * sim1 * r1) / baseNum; //c1 * r1 / baseNum;
                        Pset[2] = (c2 * sim2 * r2) / baseNum; //c2 * r2 / baseNum;
                        Pset[1] += Pset[0];
                        Pset[2] += Pset[1];
                        for (int g = 0; g < subnum; g++)
                        {



                        //double r1 = g_r.NextDouble();
                        //double r2 = g_r.NextDouble();

                        //double r1 = g_r.NextDouble();
                        //double r2 = g_r.NextDouble();
                        //double baseNum = w + c1 * ham1 + c1 * ham1;

                        //Pset[0] = g_r.NextDouble();
                        //Pset[1] = (1.0 - Pset[0]) * g_r.NextDouble();
                        //Pset[2] = 1.0 - Pset[0] - Pset[1];

                        //for (int k = 0; k < 3; k++)
                        //{
                        //    rate[k] = w * Pset[k] / baseNum;
                        //}
                        //rate[tmp.pBest.Solution[g]] += c1 * r1 / baseNum;
                        //rate[gBest.Solution[g]] += c2 * r2 / baseNum;
                        //tmp.Solution[g] = 2;
                        double r;
                        lock (g_r)
                        {
                            r = g_r.NextDouble();
                        }
                        for (byte k = 0; k < 3; k++)
                            {
                                if (r > Pset[k]) continue;
                                if (k == 2)
                                {
                                    tmp.Solution.SetPosition(gBest.Solution[g + decnum].ToId(), g + decnum);
                                    break;
                                }

                                switch (k)
                                {
                                    case 0: tmp.Solution.SetPositionRandom(g + decnum); break;
                                    case 1: tmp.Solution.SetPosition(tmp.pBest.Solution[g + decnum].ToId(), g + decnum); break;
                                    default: break;
                                }
                                break;
                            }
                        }
                        tmp.preHammingDistance =usenorm?  HammingDistance(tmp.Solution, preBirdSolution):0;
                        tmp.Solution.CountVioInfo();
                        lock (this)
                        {
                            evaltimes++;
                        }
                    //}
                    //catch (Exception ex)
                    //{

                    //    throw ex; 
                    //}
                  
                    
                });

                //for (int j = 0; j < Size; j++)
               
                int sumConf = Birds.Sum(o => o.Solution.Info.Violation);
                for (int g = 0; g < Size; g++)
                {
                    Bird item = Birds[g];
                    item.Fitness = 1.0 - (double)item.Solution.Info.Violation / (double)sumConf;
                    if (item.Solution.Info.Violation <= item.pBest.Solution.Info.Violation)
                    {
                       // Array.Copy(item.Solution, item.pBest.Solution);
                        item.pBest.Solution.CopyFrom(item.Solution);
                        item.pBest.Fitness = item.Fitness;
                    }
                }
                //foreach (var item in Birds)
                //{
                //    foreach (var iitem in item.Solution)
                //    {
                //        if (iitem.ToId() < 0)
                //        {
                //            throw new Exception();
                //        }
                //        foreach (var iiitem in item.Solution)
                //        {
                //            if (iiitem.index != iitem.index)
                //            {
                //                if (infos[iitem.index].PlaceID == infos[iiitem.index].PlaceID && iiitem.ToId() == iitem.ToId())
                //                {
                //                    throw new Exception();
                //                }
                //            }
                //        }
                //    }
                //}
                RefreshGBEST();
                if (gBest.Solution.Info.Violation == 0) break;
                Console.WriteLine(i);
                Console.WriteLine(ServiceStack.Text.JsonSerializer.SerializeToString(gBest.Solution.Info));
            }
            minConf = gBest.Solution.Info.Violation;
            Console.WriteLine(gBest.Solution.Info.Violation);
        }

        public void RefreshGBEST()
        {
            //Bird bGBest = Birds.OrderBy(i => i.Solution.Info.Violation).First();
            foreach (var item in Birds)
            {
                if (gBest.Solution.Info.Violation == -1 || gBest.Solution.Info.Violation >= item.Solution.Info.Violation)
                {
                    gBest.Solution.CopyFrom(item.Solution);
                }
            }

            //gBest.Solution.CopyFrom(bGBest.Solution);
        }
    }
}
