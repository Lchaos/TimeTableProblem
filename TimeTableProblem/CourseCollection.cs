using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Immutable;
namespace TimeTableProblem
{
    public class CourseCollection:List<Course>
    {
        public List<Course> Subjects;
        public List<DeterminedCourse> DeterminedCourses;
        public CourseCollection(IEnumerable<Course> sub,IEnumerable<DeterminedCourse> DeterminedCourse):base()
        {
            this.AddRange(DeterminedCourse);
            this.AddRange(sub);
            DeterminedCourses = new List<DeterminedCourse>(DeterminedCourse);
            Subjects = new List<Course>(sub);
            for (int i = 0; i < this.Count; i++)
            {
                this[i].index = i;
            }
        }
    }

    public class CourseInfoCollection : List<CourseSetting>
    {
        internal CourseCollection courseInfos;
        internal int DeterminedCourseNum;
        public ViolationInfo Info = new ViolationInfo();
        public IImmutableList<int>[] lst_EmptyPlace;

        public static System.Random g_r = new Random();
        public int[] room;
        public CourseInfoCollection(CourseCollection info) : base()
        {
            //System.Collections.ArrayList.Synchronized
            courseInfos = info;
            DeterminedCourseNum = info.DeterminedCourses.Count;
            init();
        }


        public List<DeterminedCourse> getSerObjects()
        {
            List<DeterminedCourse> ll = new List<DeterminedCourse>();
            for (int i = 0; i < this.Count; i++)
            {
                DeterminedCourse tmp = new DeterminedCourse();
                tmp.From(courseInfos[i]);
                var tmp2 = this[i];
                tmp.semester = tmp2.semester;
                tmp.koma = tmp2.koma;
                tmp.day = tmp2.day;
                ll.Add(tmp);
            }
            return ll;
        }

        public string ToJson()
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString<List<DeterminedCourse>>(getSerObjects());
        }

        public string ToCSV()
        {
            return ServiceStack.Text.CsvSerializer.SerializeToString<List<DeterminedCourse>>(getSerObjects());
        }

        public void init()
        {
            lst_EmptyPlace = new IImmutableList<int>[5];
            this.Clear();
            foreach (var item in courseInfos)
            {
                this.Add(new CourseSetting());
            }
            var lsttmp = new List<int>();
            for (int j = 0; j < 5; j++)
            {
               
                for (int i = 0; i < 90; i++)
                {
                    lsttmp.Add(i);
                }
                var tmp = ImmutableList.Create<int>(lsttmp.ToArray());
                lst_EmptyPlace[j] = tmp;
            }
            int cnt = this.Count;
            for (int i = 0; i < cnt; i++)
            {
                this[i].index = i;
            }

            room = new int[3 * 5 * 6];
        }

        public void refreshEmptyPlace()
        {
            //lst_EmptyPlace.Clear();
            var lsttmp = new List<int>[5];
            for (int j = 0; j < 5; j++)
            {
                //lst_EmptyPlace[j].Clear();
                lsttmp[j] = new List<int>();
                for (int i = 0; i < 90; i++)
                {
                    lsttmp[j].Add(i);
                }
            }
            for (int i = 0; i < this.Count; i++)
            {
                lsttmp[courseInfos[i].PlaceID].Remove(this[i].ToId());

            }
            for (int j = 0; j < 5; j++)
            {
                var tmp = ImmutableList.Create<int>(lsttmp[j].ToArray());
                lst_EmptyPlace[j] = tmp;
            }

        }

        //public void SetSetting(int index, System.Action<CourseSetting> act)
        //{
        //    var setting = this[index];
        //    act(setting);
        //    getEmptyPlace(courseInfos[index]).Remove(setting.ToId());
        //}


        public void SetDeterminedCourse()
        {
            for (int i = 0; i < DeterminedCourseNum; i++)
            {
                var setting = this[i];
                var info = courseInfos.DeterminedCourses[i];
                setting.Set(info);
                //getEmptyPlace(info).Remove(setting.ToId());
                //if (info.length == 2)
                //{
                //    getEmptyPlace(info).Remove(setting.ToId() + 1);
                //}
            }
            refreshEmptyPlace();
        }

        public bool Exchange(int index1, int index2)
        {
            var setting1 = this[index1];
            var setting2 = this[index2];
            var info1 = courseInfos[index1];
            var info2 = courseInfos[index2];
            if (info1.PlaceID != info2.PlaceID) return false;

            if (info2.length == info1.length)
            {
                if (info2.length == 1)
                {
                    setting1.Exchange(setting2);
                }
                else
                {
                    setting1.Exchange(setting2);
                    this[info1.nextID].Exchange(this[info2.nextID]);
                }

                return true;
            }
            else
            {
                if (info2.length > info1.length)
                {
                    var tmp1 = info1;
                    var tmp2 = setting1;
                    info1 = info2;
                    setting1 = setting2;
                    info2 = tmp1;
                    setting2 = tmp2;
                }
                var place = getEmptyPlace(info1);
                if (setting2.koma < 6 && place.Contains(setting2.ToId() + 1))
                {
                    setting1.Exchange(setting2); 
                    removeEmptyPlace(info1,setting1.ToId() + 1);
                    addEmptyPlace(info2,this[info1.nextID].ToId());
                    this[info1.nextID].FromId(setting1.ToId() + 1);
                }
                else if (setting2.koma > 1 && place.Contains(setting2.ToId() - 1))
                {
                    setting1.Exchange(setting2);
                    addEmptyPlace(info2,this[info1.nextID].ToId());
                    removeEmptyPlace(info1,setting1.ToId() - 1);
                    this[info1.nextID].FromId(setting1.ToId() - 1);
                };
            }
            //else if (info1.length == 1 && info2.length == 2)
            //{
            //    var place = getEmptyPlace(info2);
            //    if (setting2.koma < 6) 
            //    return place.Contains(setting2.ToId() + 1);

            //    return setting2.koma >1 && place.Contains(setting2.ToId() - 1);
            //}
            return false;
        }

        public IImmutableList<int> getEmptyPlace(Course info)
        {
                return lst_EmptyPlace[info.PlaceID];
        }

        public void removeEmptyPlace(Course info,int id)
        {
            lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Remove(id);
        }
        public void addEmptyPlace(Course info, int id)
        {
            lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Add(id);
        }

        public bool Exchange(CourseSetting index1, CourseSetting index2)
        {
            return Exchange(index1.index, index2.index);
        }

        //public void Exchange(int index1, int index2)
        //{
        //    if (CanExchange(index1,index2))
        //    {
        //        __Exchange(index1, index2);
        //    }
        //}

        internal void __Exchange(int index1, int index2)
        {
            var setting1 = this[index1];
            var setting2 = this[index2];
            var info1 = courseInfos[index1];
            var info2 = courseInfos[index2];

            if (info2.length == info1.length)
            {
                setting1.Exchange(setting2);
            }
            else if (info2.length == 2 && info1.length == 1)
            {
                addEmptyPlace(info1,setting1.ToId() + 1);
                removeEmptyPlace(info2,setting2.ToId() + 1);
            }
            else if (info1.length == 1 && info2.length == 2)
            {
                addEmptyPlace(info2,setting2.ToId() + 1);
                removeEmptyPlace(info1,setting1.ToId() + 1);
            }
        }

        public void CountVioInfo()
        {
            Info.reset();
            var compilsoryCourse = courseInfos.DeterminedCourses.Where(c => c.compilsory == 1);
            var subCompilsoryCourse = courseInfos.Subjects.Where(c => c.compilsory == 1).ToList();
            foreach (var dec in compilsoryCourse)
            {
                var subCompilsoryCourseVio = subCompilsoryCourse.Where(c => dec.grade - c.grade == 1 && this[c.index].Equals(this[dec.index]));
                Info.grade += subCompilsoryCourseVio.Count();
            }

            var subContinueCourse = courseInfos.Subjects.Where(c => c.continue_semester == 1);
            foreach (var item in subContinueCourse)
            {
                int cnt = courseInfos.Count;
                for (int i = item.index + 1; i < cnt; i++)
                {
                    var setting = courseInfos[i];
                    if (setting.IsSameCourse(item))
                    {
                        var info1 = this[item.index];
                        var info2 = this[setting.index];
                        if (info1.semester == info2.semester || info1.day != info2.day || info1.koma != info2.koma)
                        {
                            Info.continueYear++;
                        }
                        if (item.length > 1)
                        {
                            if (info1.semester != info2.semester || info1.day != info2.day ||
    (info1.koma != info2.koma && Math.Abs(info1.koma - info2.koma) != 1)) Info.continued++;
                            else if (info1.koma == 2 && info2.koma == 3 || info1.koma == 3 && info2.koma == 2)
                            {
                                Info.crossLanchTime++;
                            }
                        }
                    }
                }
            }

            Info.sixthCourse = this.Where(c => c.koma == 6).Count();

            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (var item in subCompilsoryCourse)
            {
                var key = _CourseinfoKeyFunc(item);
                if (!dic.ContainsKey(key)) dic[key] = 1;
                else dic[key] += 1;
            }
            Info.compilsory = dic.Values.Where(i => i > 4).Count();

            dic.Clear();
            foreach (var item in courseInfos.Subjects.Where(i => i.grade == 3))
            {
                var key = _CourseinfoKeyFunc(item);
                if (!dic.ContainsKey(key)) dic[key] = 1;
                else dic[key]++;
            }
            Info.channel = dic.Values.Where(i => i > 2).Count();

            dic.Clear();
            foreach (var item in courseInfos)
            {
                var keyday = _CourseinfoKeyFunc(item);
                var key = keyday * 10 + this[item.index].koma;
                if (!dic.ContainsKey(key)) {
                    dic[key] = 1;
                    if (!dic.ContainsKey(keyday))
                    {
                        dic[keyday] = 1;
                    }
                    else dic[keyday]++;
                }
            }
            for (int i = 1; i < 4; i++)
            {
                int jstr = 1, jend = 4;
                if (i!=3)
                {
                    jstr = 0;
                    jend = 1;
                }
                for (int j = jstr; j < jend; j++)
                {
                    for (int l = 1; l < 4; l++)
                    {
                        for (int m = 1; m < 6; m++)
                        {
                            if (dic.ContainsKey(i * 1000 + j * 100 + l * 10 + m))
                            {
                                    var val = dic[i * 1000 + j * 100 + l * 10 + m];
                                    if (DayCountVioMap.ContainsKey(val))
                                    {
                                        Info.Violation += DayCountVioMap[val];
                                        Info.days++;
                                    }
                            }
                            else
                            {
                                Info.Violation += DayCountVioMap[0];
                                Info.days++;
                            }
                        }
                    }
                }
            }
            //foreach (var item in dic.Keys)
            //{
            //    var val = dic[item];
            //    if (DayCountVioMap.ContainsKey(val))
            //    {
            //        Info.Violation += DayCountVioMap[val];
            //        Info.days++;
            //    }
            //}

            Info.Count();
        }




        static CourseInfoCollection()
        {
            DayCountVioMap.Add(0, 15);
            DayCountVioMap.Add(1, 10);
            //DayCountVioMap.Add(2, 5);
            DayCountVioMap.Add(6, 5);
        }

        public static Dictionary<int, int> DayCountVioMap = new Dictionary<int, int>();

        int _CourseinfoKeyFunc(Course info) {
            var c = this[info.index];
            if (info.grade < 3)
            {
                return info.grade * 1000 + c.semester * 10 + c.day;
            }
            return info.grade * 1000 + info.profession * 100 + c.semester * 10 + c.day;
        }
        //public void SetRandomPositionInEmpty(int index)
        //{
        //    if (index < DeterminedCourseNum) return;
        //    var setting = this[index];
        //    var info = courseInfos[index];
        //    var emptyflag = setting.ToId();
        //    if (emptyflag != -1)
        //    {
        //        lst_EmptyPlace.Add(emptyflag);
        //    }
        //    int idIndex =  g_r.Next(0, lst_EmptyPlace.Count);
        //    int newposition=lst_EmptyPlace[idIndex];

        //    if (info.length == 2)
        //    {
        //        while (CourseSetting.getkoma(newposition) == 6 || !lst_EmptyPlace.Contains(newposition))
        //        {
        //            idIndex = g_r.Next(0, lst_EmptyPlace.Count);
        //            newposition = lst_EmptyPlace[idIndex];
        //        }
        //    }
        //    setting.FromId(newposition);
        //    lst_EmptyPlace.RemoveAt(idIndex);
        //    if (info.length == 2)
        //    {
        //        lst_EmptyPlace.Remove(newposition + 1);
        //    }
        //    setting.FromId(newposition);

        //}
        private bool _ContainsNearPosition(IImmutableList<int> emptyPlace, int id)
        {
            int koma = CourseSetting.getkoma(id);
            if (koma < 6 && emptyPlace.Contains(id + 1)) return true;
            if (koma > 1 && emptyPlace.Contains(id - 1)) return true;
            return false;
        }


        private int _NearEmptyPosition(IImmutableList<int> emptyPlace, int id)
        {
            int koma = CourseSetting.getkoma(id);
            if (koma == 1 && emptyPlace.Contains(id + 1)) return id + 1;
            if (koma == 6 && emptyPlace.Contains(id - 1)) return id - 1;
            if (koma > 1 && koma < 6)
            {
                var flag1 = emptyPlace.Contains(id + 1);
                var flag2 = emptyPlace.Contains(id - 1);
                lock (g_r)
                {
                    if (flag1 && flag2) return g_r.NextDouble() < 0.5 ? id + 1 : id - 1;
                    else if (flag1) return id + 1;
                    else if (flag2) return id - 1;
                }

            }

            return -1;
        }

        public bool InitRandomPositionInEmpty(int index, int semester = -1)
        {
            if (index < DeterminedCourseNum) return false;
            var setting = this[index];
            var info = courseInfos[index];
            var emptyflag = setting.ToId();
            var emptyPlace = getEmptyPlace(info);
            if (emptyflag >= 0)
            {
                return true;
                //emptyPlace.Add(emptyflag);
            }
            int idIndex = g_r.Next(0, emptyPlace.Count);
            int newposition = 0;
            var p = info.length == 2 ? emptyPlace.Where((r) => ((CourseSetting.getsemester(r) == semester) || semester == -1) &&
            _ContainsNearPosition(emptyPlace, r)).ToList() :
                emptyPlace.Where((r) => CourseSetting.getsemester(r) == semester || semester == -1).ToList();
            if (p.Count == 0)
            {
                return false;
            }
            idIndex = g_r.Next(0, p.Count);
            newposition = p[idIndex];
            setting.FromId(newposition);

            if (info.length == 2)
            {
                //emptyPlace.Remove(newposition + 1);
                for (int i = index + 1; i < this.Count; i++)
                {
                    if (this.courseInfos[i].IsSameCourse(info))
                    {
                        int id = _NearEmptyPosition(emptyPlace, newposition);
                        if (id == -1 && _ContainsNearPosition(emptyPlace, newposition))
                        {

                        }
                        this[i].FromId(id);
                        courseInfos[i].nextID = info.index;
                        info.nextID = courseInfos[i].index;
                        
                       lst_EmptyPlace[info.PlaceID] = emptyPlace.Remove(id);
                        lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Remove(newposition);
                        break;
                    }
                }
            }
            else
            {
                lst_EmptyPlace[info.PlaceID] = emptyPlace.Remove(newposition);
            }

            setting.FromId(newposition);


            return true;
        }

        public bool IsDeterminedPosition(int id,int placeid)
        {
            for (int i = 0; i < DeterminedCourseNum; i++)
            {
                if (courseInfos[i].PlaceID== placeid && this[i].ToId() == id)
                {
                    return true;
                }
            }
            return false;
        }


        public int PlacedIndex(int id, int PlaceID, int length)
        {
            var p = courseInfos.Where(r => r.PlaceID == PlaceID);
            var koma = CourseSetting.getkoma(id);
            var emptyPlace = lst_EmptyPlace[PlaceID];

            if (length == 1)
            {
                if (emptyPlace.Contains(id)) return -1;
                foreach (var item in p)
                {
                    var itemid = this[item.index].ToId();
                    if (itemid == id)
                    {
                        return item.index;
                    }
                }
                return -1;
            }
            else// if(length == 2)
            {
                if (emptyPlace.Contains(id) && _ContainsNearPosition(emptyPlace, id))
                {
                    return -1;
                }
                foreach (var item in p)
                {
                    var itemid = this[item.index].ToId();
                    if (itemid == id)
                    {
                        return item.index;
                    }
                    if (koma < 6 && id == itemid + 1)
                    {
                        return item.index;
                    }
                    else if (koma > 1 && id == itemid - 1)
                    {
                        return item.index;
                    }
                }
                return -1;

            }

        }

        public void SetPositionRandom(int index, bool exchangable = true)
        {
            int mC;
            do
            {
                lock (g_r)
                {
                    mC = g_r.Next(0, 30) + (this[index].semester - 1) * 30;
                }
                
            } while (IsDeterminedPosition(mC, courseInfos[index].PlaceID));
            SetPosition(mC, index, exchangable);
        }

        public void SetPosition(int id, int index, bool exchangable = true)
        {
            if (index < DeterminedCourseNum) {
                return;
            }

            if (IsDeterminedPosition(id, courseInfos[index].PlaceID))
            {
                return;
            }
            var setting = this[index];
            var info = courseInfos[index];
            var emptyflag = setting.ToId();
            var emptyPlace = getEmptyPlace(info);
            if (emptyflag < 0) {
                return;
            }
            var placedindx = PlacedIndex(id, info.PlaceID, info.length);
            if (placedindx!=-1)
            {
                if (placedindx < DeterminedCourseNum)
                {
                    return;
                }
                if (exchangable) Exchange(placedindx,index);
                return;
            }


            
            int newposition = id;
            
            if (info.length == 2)
            {
                var nearid = _NearEmptyPosition(emptyPlace, newposition);
                if (nearid!=-1)
                {
                    lst_EmptyPlace[info.PlaceID] = emptyPlace.Remove(nearid);
                    lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Remove(newposition);
                    lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Add(this[info.nextID].ToId());
                    lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Add(setting.ToId());
                    this[info.nextID].FromId(nearid);
                    setting.FromId(newposition);
                }


                //if (CourseSetting.getkoma(newposition) < 6 && emptyPlace.Contains(newposition + 1))
                //{
                //    emptyPlace.Add(emptyflag);
                //    setting.FromId(newposition);
                //    emptyPlace.Add(emptyflag + 1);
                //    emptyPlace.Remove(newposition);
                //    emptyPlace.Remove(newposition + 1);
                //}
                //else if (CourseSetting.getkoma(newposition) > 1 && emptyPlace.Contains(newposition - 1))
                //{
                //    emptyPlace.Add(emptyflag);
                //    setting.FromId(newposition - 1);
                //    emptyPlace.Add(emptyflag + 1);
                //    emptyPlace.Remove(newposition);
                //    emptyPlace.Remove(newposition - 1);
                //}
            }
            else
            {
                setting.FromId(newposition);
                lst_EmptyPlace[info.PlaceID] = emptyPlace.Remove(newposition);
                lst_EmptyPlace[info.PlaceID] = lst_EmptyPlace[info.PlaceID].Add(emptyflag);
            }
            //foreach (var iitem in this)
            //{
            //    if (iitem.ToId() < 0)
            //    {
            //        continue;
            //    }
            //    foreach (var iiitem in this)
            //    {
            //        if (iiitem.ToId() < 0)
            //        {
            //            continue;
            //        }
            //        if (iiitem.index != iitem.index)
            //        {
            //            if (courseInfos[iitem.index].PlaceID == courseInfos[iiitem.index].PlaceID && iiitem.ToId() == iitem.ToId())
            //            {
            //                throw new Exception();
            //            }
            //        }
            //    }
            //}
        }

        //public void SetPositionInEmpty(int id,int index)
        //{
        //    if (index < DeterminedCourseNum) return;
        //    var setting = this[index];
        //    var info = courseInfos[index];
        //    var emptyflag = setting.ToId();
        //    var emptyPlace = getEmptyPlace(info);
        //    if (emptyflag == -1) return;
        //    emptyPlace.Add(emptyflag);
        //    int newposition = id;

        //    if (info.length == 2)
        //    {
        //        if (CourseSetting.getkoma(newposition)< 6 && emptyPlace.Contains(newposition + 1))
        //        {
        //            setting.FromId(newposition);
        //            emptyPlace.Add(emptyflag+1);
        //            emptyPlace.Remove(newposition);
        //            emptyPlace.Remove(newposition + 1);
        //        }
        //        else if(CourseSetting.getkoma(newposition) > 1 && emptyPlace.Contains(newposition - 1))
        //        {
        //            setting.FromId(newposition-1);
        //            emptyPlace.Add(emptyflag + 1);
        //            emptyPlace.Remove(newposition);
        //            emptyPlace.Remove(newposition -1);
        //        }
        //    }
        //    else
        //    {
        //        setting.FromId(newposition);
        //        emptyPlace.Remove(newposition);
        //    }

        //}
        public void CopyFrom(CourseInfoCollection cc)
        {
            for (int j = 0; j < cc.Count; j++)
            {
                this[j].Set(cc[j]);
            }
            this.refreshEmptyPlace();
            this.Info.set(cc.Info);
        }


        public void ResetPosition()
        {
        }
    }
    
}