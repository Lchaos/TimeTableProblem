using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace TimeTableProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length!=0)
            {
                RunScript(args[0]);
            }
            else
            {
                PSOTimeTableProblem es = new PSOTimeTableProblem();
                //es.Size = 200;
                //es.MaxLoop = 5000;
                //es.c1 = 5;
                //es.c2 = 2;
                //es.w = 0.05;
                //es.usenorm = true;

                es.init();
                es.DoJob();
                //System.IO.File.WriteAllText("test4.json", es.Nests.OrderBy(r => r.solution.Info.Violation).First().solution.ToJson());
            }

        }


        public static void RunScript(string filename)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            CompilerParameters objCompilerParameters = new CompilerParameters();

            //添加需要引用的dll
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.ReferencedAssemblies.Add("TimeTableProblem.exe");
            objCompilerParameters.ReferencedAssemblies.Add("ServiceStack.Text.dll");

            //是否生成可执行文件
            objCompilerParameters.GenerateExecutable = false;

            //是否生成在内存中
            objCompilerParameters.GenerateInMemory = true;

            //编译代码
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, System.IO.File.ReadAllText(filename));

            if (cr.Errors.HasErrors)
            {
                var msg = string.Join(Environment.NewLine, cr.Errors.Cast<CompilerError>().Select(err => err.ErrorText));
                Console.WriteLine(msg);
            }
            else
            {
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("TimeTableProblem.Script");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("Run");
                objMI.Invoke(objHelloWorld, null);
            }
        }
        /// <summary>
        /// Passed
        /// </summary>
        public static void readtest()
        {
            var test1 =  ReadData.readCourse();

            var test2 = ReadData.readSubject();
            
            string str1 = CsvSerializer.SerializeToCsv(test1);
            Console.WriteLine("test1");
            Console.WriteLine(str1);
            string str = System.IO.File.ReadAllText("kotei.txt", Encoding.UTF8);
            Console.WriteLine(str1.Equals(str));
            string str2 = CsvSerializer.SerializeToCsv(test2);
            str = System.IO.File.ReadAllText("data10.txt", Encoding.UTF8);
            Console.WriteLine("test2");
            Console.WriteLine(str2);
            Console.WriteLine(str2.Equals(str));
        }
    }
}
