using EPAMTask7._1.LINQTOSQL;
using System.Collections.Generic;
using System.Linq;

namespace FileExtensions.InformationBuilders
{
    /// <summary>
    /// Builds reports information, gathers it form the db
    /// </summary>
    public class BuildInformation
    {
        /// <summary>
        /// Method for getting information about one group session results
        /// </summary>
        /// <param name="id">session id</param>
        /// <param name="db">db context</param>
        /// <param name="group_ordered">ordered groups</param>
        /// <param name="spec_marks">marks by specialies</param>
        /// <param name="examiner_marks">all examiners marks</param>
        public void GetSpecialitySessionResults(int id, UniversityClassesDataContext db, out Dictionary<string, float> group_ordered,
            out Dictionary<string, float> spec_marks, out Dictionary<string, float> examiner_marks, string TypeOfOrder = "OrderBy")
        {

            var grades = from grade in db.Grade.ToList()
                         where grade.SessionID == id
                         select grade;

            var students = from grd in grades.ToList()
                           select grd.StudentID;

            var groups = from student in db.Student.ToList()
                         where students.Contains(student.ID)
                         select student.GroupID;

            groups = groups.Distinct();

            var groupsInNeed = from grp in db.Group.ToList()
                         where groups.Contains(grp.ID)
                         select grp;

            Dictionary<string, float> group_contains = new Dictionary<string, float>();

            foreach (var t in groupsInNeed)
            {
                var student_count = from student in db.Student
                                    where student.GroupID == t.ID
                                    select student.ID;

                var marks = from grd in grades
                            where student_count.Contains(grd.StudentID.Value)
                            select grd.Mark;

                float mark = (marks.Sum().Value / marks.Count());

                group_contains.Add(t.Name, mark);   
            }

            SortingMethods sorting = new SortingMethods();

            group_ordered = sorting.OrderList(group_contains, TypeOfOrder);

            var Specializations = from group_spec in groupsInNeed
                                  select group_spec.SpecializationID;

            spec_marks = new Dictionary<string, float>();

            foreach (var spec in Specializations)
            {
                var specialization = db.Specialization.Where(special_elem => special_elem.ID == spec).SingleOrDefault();

                var special_group_name = from group_elem in groupsInNeed
                                         where (specialization as Specialization).ID == group_elem.SpecializationID
                                         select group_elem;

                float aver_mark = 0;

                foreach (var group_elem in special_group_name)
                {
                    var mark = from mark_elem in group_ordered
                               where mark_elem.Key == group_elem.Name
                               select mark_elem.Value;
                    aver_mark += mark.SingleOrDefault();
                }

                if (aver_mark != 0)
                {
                    aver_mark /= special_group_name.Count();
                }

                spec_marks.Add((specialization as Specialization).Name, aver_mark);
            }

            examiner_marks = new Dictionary<string, float>();

            foreach (var examiner in db.Examiner)
            {
                var examiner_exams = from exam in db.Exam
                                     where exam.ExaminerID == examiner.ID && exam.SessionID == id
                                     select exam;

                float aver_mark = 0;
                int grades_count = 0;

                foreach (var exam in examiner_exams)
                {
                    var grades_ = from grade in grades
                                  where grade.ExamID == exam.ID
                                  select grade.Mark.Value;

                    aver_mark += grades_.Sum();
                    grades_count += grades.Count();
                }

                if (aver_mark != 0)
                {
                    aver_mark /= grades_count;
                }


                examiner_marks.Add(examiner.FirstName + " " + examiner.LastName, aver_mark);
            }
        }

        /// <summary>
        /// Builds information about all sessions and it's dynamics
        /// </summary>
        /// <param name="db">db contex</param>
        /// <param name="results_ordered">ordered results</param>
        /// <param name="ordered_sessions">orderes sessions</param>
        /// <param name="subject_mark">all marks from subjects</param>
        public void GetAllSessionsResult(UniversityClassesDataContext db, out Dictionary<Session, Dictionary<string, List<float>>> results_ordered,
                    out List<Session> ordered_sessions, out Dictionary<string, List<float>> subject_mark, string TypeOfOrder = "OrderBy")
        {
            Dictionary<Session, Dictionary<string, List<float>>> results = new Dictionary<Session, Dictionary<string, List<float>>>();
            subject_mark = new Dictionary<string, List<float>>();

            foreach (Session session_elem in db.Session)
            {
                Dictionary<string, List<float>> group_contains = new Dictionary<string, List<float>>();


                var Grades = from grade in db.Grade.ToList()
                             where grade.SessionID == session_elem.ID
                             select grade;

                var students = from grd in Grades.ToList()
                               select grd.StudentID;

                var groups = from student in db.Student.ToList()
                             where students.Contains(student.ID)
                             select student.GroupID;

                groups = groups.Distinct();

                var Groups = from grp in db.Group.ToList()
                             where groups.Contains(grp.ID)
                             select grp;

                foreach (var t in Groups)
                {
                    var student_count = from student in db.Student.ToList()
                                        where student.GroupID == t.ID
                                        select student.ID;

                    var marks = from grd in Grades
                                where student_count.Contains(grd.StudentID.Value)
                                select grd.Mark;

                    float mark_Aver = (marks.Sum().Value / marks.Count());
                    float mark_max = marks.Max().Value;
                    float mark_min = marks.Min().Value;

                    group_contains.Add(t.Name, new List<float>() { mark_max, mark_Aver, mark_min });
                }

                results.Add(session_elem, group_contains);
            }
            SortingMethods sorting = new SortingMethods();

            results_ordered = sorting.OrderList(results, TypeOfOrder);

            var ordered_sessions_ = from session in db.Session
                                   orderby session.StartDate
                                   select session;

            ordered_sessions = ordered_sessions_.ToList();

            foreach (Subject subject in db.Subject)
            {
                List<float> marks = new List<float>();

                foreach (var session in ordered_sessions)
                {
                    float mark = 0;

                    var exxx = from ex in db.Exam.ToList()
                               where ex.SessionID == session.ID && ex.SubjectID == subject.ID
                               select ex.ID;

                    var grades = from grd in db.Grade.ToList()
                                 where exxx.Contains(grd.ExamID.Value)
                                 select grd.Mark;

                    if (grades.Count() != 0)
                    {
                        mark = grades.Sum().Value / grades.Count();
                    }

                    marks.Add(mark);
                }

                subject_mark.Add(subject.Name, marks);
            }
        }

        /// <summary>
        /// Method for getting information about all D-Students
        /// </summary>
        /// <param name="db">DBContext</param>
        /// <param name="filepath">path to the file</param>
        /// <param name="TypeOfOrder">how to sort</param>
        /// <returns>group name and students names</returns>
        public Dictionary<string, List<string>> GetAllDStudents(UniversityClassesDataContext db, string TypeOfOrder = "OrderBy")
        {
            Dictionary<string, List<string>> group_allocated = new Dictionary<string, List<string>>();


            var Grades = from grade in db.Grade.ToList()
                         where grade.Mark < 4
                         select grade;

            var students = from grd in Grades.ToList()
                           select grd.StudentID;


            var student_allocated = from student in db.Student.ToList()
                                    where students.Contains(student.ID)
                                    select student;

            student_allocated = student_allocated.Distinct();

            var groups = from student in student_allocated.ToList()
                         select student.GroupID;


            var Groups = from grp in db.Group.ToList()
                         where groups.Contains(grp.ID)
                         select grp;

            foreach (var t in Groups)
            {
                var student_count = from student in db.Student.ToList()
                                    where student.GroupID == t.ID
                                    select student.LastName;

                group_allocated.Add(t.Name, student_count.ToList());
            }

            SortingMethods methods = new SortingMethods();

            return methods.OrderList(group_allocated, TypeOfOrder);
        }
    }
}
