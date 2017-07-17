using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
    public class ESTimeTableProblem:TimeTableProblemBase
    {
        internal List<CourseInfoCollection> m_uSolutions;
        private List<CourseInfoCollection> m_rSolutions;

        public int maxLoop = 1000;
        public int rSize=500;
        public int uSize=200;
        //internal int nSize;
        public int urType = 1;
        public CourseInfoCollection bestSol;

        public void init()
        {
            m_rSolutions = new List<CourseInfoCollection>();
            m_uSolutions = new List<CourseInfoCollection>();
            for (int i = 0; i < rSize; i++)
            {
                m_rSolutions.Add(new CourseInfoCollection(this.infos));
            }
            for (int i = 0; i < uSize; i++)
            {
                m_uSolutions.Add(new CourseInfoCollection(this.infos));
            }
            foreach (var item in m_uSolutions)
            {
                HardVioliationAdapter.HardVioliationSetting(item);
                item.CountVioInfo();
            }
        }

        public void DoJob()
        {
            int mC = 0;
            for (int z = 0; z < maxLoop; z++)
            {

                Parallel.For(0, rSize, (i) =>
                //for (int i = 0; i < rSize; i++)
                {
                    int uNum, mId;
                    lock (g_r)
                    {
                        uNum = g_r.Next(0, uSize);
                        mId = g_r.Next(0, this.subnum) + m_rSolutions[i].DeterminedCourseNum;
                    }

                    //for (int j = 0; j < m_uSolutions[uNum].Count; j++)
                    //{
                    //    m_rSolutions[i][j].Set(m_uSolutions[uNum][j]);
                    //}
                    m_rSolutions[i].CopyFrom(m_uSolutions[uNum]);
                    //m_rSolutions[i].refreshEmptyPlace();
                    m_rSolutions[i].SetPositionRandom(mId);
                    //m_rSolutions[i].SetPosition(mC, mId);
                    //m_rSolutions[i].refreshEmptyPlace();
                    m_rSolutions[i].CountVioInfo();
                    //while (mC == m_rSolutions[i][mId]) m_rSolutions[i][mId] = (byte)g_r.Next(0, 3);
                    lock (this)
                    {
                        evaltimes++;
                    }
                });

                var Group = (urType == 0 ? m_uSolutions.Union(m_rSolutions) : m_rSolutions);

                var nextSolutions = (from child in Group
                                     orderby child.Info.Violation
                                     select child).Take(uSize).ToList();

                for (int i = 0; i < nextSolutions.Count; i++)
                {
                    //CourseInfoCollection child = nextSolutions[i];
                    //CourseInfoCollection uNodes = m_uSolutions[i];
                    //for (int j = 0; j < nSize; j++) uNodes[j].Set(child[j]);
                    m_uSolutions[i] = nextSolutions[i];
                }
                for (int i = 0; i < m_rSolutions.Count; i++)
                {
                    m_rSolutions[i] = new CourseInfoCollection(infos);
                }
                //m_lstReport.Add(new Report() { iteration = z, avrConf = nextSolutions.Average(o => o.conf), maxConf = nextSolutions.Last().conf, minConf = nextSolutions[0].conf });
                minConf = nextSolutions.First().Info.Violation;
                Console.WriteLine(z);
                Console.WriteLine(ServiceStack.Text.JsonSerializer.SerializeToString(nextSolutions.First().Info));
                //foreach (var item in nextSolutions)
                //{
                //    foreach (var iitem in item)
                //    {
                //        if (iitem.ToId()<0)
                //        {
                //            throw new Exception();
                //        }
                //    }
                //}
                if (nextSolutions.First().Info.Violation == 0)
                {
                    bestSol = nextSolutions.First();
                    break;
                }
                bestSol = nextSolutions.First();
            }



            Console.WriteLine(m_uSolutions[0].Info.Violation);
        }
    }
}
