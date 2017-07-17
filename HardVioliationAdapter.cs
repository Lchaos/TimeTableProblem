using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
    public class HardVioliationAdapter
    {

        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
        public static void HardVioliationSetting(CourseInfoCollection settings)
        {
            bool flag = false;
            int cnt = 0;
            do
            {
                flag = false;
                CourseCollection info = settings.courseInfos;
                int index = info.DeterminedCourses.Count;

                settings.SetDeterminedCourse();
                settings.refreshEmptyPlace();
                var Group = from g in from r in info.Subjects
                                      select new { info = r, index = info.IndexOf(r) }
                            group g by g.info.@group.Substring(0, g.info.@group.Length - 1) + g.info.senkou + g.info.grade;
                foreach (var grp in Group)
                {
                    var tmpgrp = grp.ToList();
                    var cntGroupidcnt = tmpgrp.Select(r => Int32.Parse(ToDBC(r.info.group.Substring(r.info.group.Length - 1, 1)))).Distinct().Count();
                    if (cntGroupidcnt == 1)
                    {
                        for (int i = 0; i < tmpgrp.Count; i++)
                        {
                            if (!settings.InitRandomPositionInEmpty(tmpgrp[i].index))
                            {
                                settings.init();
                                flag = true;
                                break;
                                //HardVioliationSetting(settings);
                            };
                            settings.refreshEmptyPlace();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tmpgrp.Count; i++)
                        {
                            var tmp = tmpgrp[i].info.group;
                            if (!settings.InitRandomPositionInEmpty(tmpgrp[i].index, Int32.Parse(ToDBC(tmp.Substring(tmp.Length - 1, 1)))))
                            {
                                //Console.WriteLine(settings.ToCSV());
                                settings.init();
                                Console.WriteLine("Index:" + settings.courseInfos[ tmpgrp[i].index].group);
                                
                                flag = true;
                                break;
                                //HardVioliationSetting(settings);
                            };
                            settings.refreshEmptyPlace();
                        }
                    }
                    if (flag) break;
                }
                Console.WriteLine("LOOP:" +  cnt++);
            } while (flag);
            
        }
    }
}
