
using System;
namespace TimeTableProblem
{
    public class Script
    {
        public void Run()
        {
            //ESTimeTableProblem es = new ESTimeTableProblem();
            PSOTimeTableProblem es = new PSOTimeTableProblem();
            //PSO parameter
            es.Size = 20;
            es.MaxLoop = 10000;
            es.c1 = 7;
            es.c2 = 1;
            es.w = 0.05;
            es.usenorm = true;
            es.init();
            es.DoJob();
            //ES parameter
           /* es.maxLoop = 1000;
            es.rSize=1000;
            es.uSize=500;
            es.urType = 0;
            es.init();
            es.DoJob();*/

            System.IO.File.WriteAllText("test1.json",es.gBest.Solution.ToJson());
        }
    }
}
