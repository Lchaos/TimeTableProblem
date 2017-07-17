using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
    using SpecialFunctions = MathNet.Numerics.SpecialFunctions;
    public class LevyFilght
    {
       
        public LevyFilght(double a, double b)
        {
            r = CourseInfoCollection.g_r;
            this.a = a;
            this.b = b;
            this.sigma = SIGMA(b);
        }


        Random r;
        double a;
        double b;
        double sigma;
        public const double M_PI = Math.PI;
        private long _next()
        {
            return (long)nextDouble();
        }

        public double nextDouble()
        {

            //正規分布
            double x1 = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Sin(2.0 * M_PI * r.NextDouble());
            double x2 = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Sin(2.0 * M_PI * r.NextDouble());
            double x3 = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Sin(2.0 * M_PI * r.NextDouble());

            //レヴィ分布に使用
            double u = x1 * sigma;
            double v = x2;

            //レヴィ分布？（正）
            double step = Math.Abs(u / Math.Pow(Math.Abs(v), 1.0 / b));

            return a * step;
        }

        public long next(long start, long end)
        {

            double step = nextDouble();

            //while (step >= end)
            //{
            //    step = step / end - 1;
            //}

            long ret = (long)step + start;
            if (ret >= end)
            {
                long dis = (end - start);
                ret = ret % dis + start;
            }


            return ret;
        }
        double SIGMA(double beta)
        {
            //TODO
            //SpecialFunctions.Gamma()
            return Math.Pow(SpecialFunctions.Gamma(1.0 + beta) * Math.Sin(M_PI * beta / 2.0) / (SpecialFunctions.Gamma((1.0 + beta) / 2.0) * beta * Math.Pow(2.0, (beta - 1.0) / 2.0)), 1.0 / beta);
        }
    }
}
