using EPAMTask7._1.LINQTOSQL;
using FileExtensions.InformationBuilders;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensions.XLXSGenerators
{
    /// <summary>
    /// Reports generator and writers methods 
    /// </summary>
    public class GenerateXLXS
    {
        /// <summary>
        /// Connection string
        /// </summary>
        private readonly string connectString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Task7UB;Integrated Security=True";

        /// <summary>
        /// Generates report about speciality results in one session in XLXS file
        /// </summary>
        /// <param name="id">Session ID</param>
        /// <param name="filepath">filepath to save</param>
        public void WriteSpecialityGrade(int id, string filepath, string TypeOfOrder)
        {
            using(UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {
                BuildInformation buildInformation = new BuildInformation();

                Dictionary<string, float> examiner_marks;
                Dictionary<string, float> group_ordered;
                Dictionary<string, float> spec_marks;

                buildInformation.GetSpecialitySessionResults(id, db, out group_ordered, out spec_marks, out examiner_marks, TypeOfOrder);

                Application excelApp = new Application();
                Workbook workBook = excelApp.Workbooks.Add();
                Worksheet workSheet = workBook.ActiveSheet;

                Session session = db.Session.Where(p => p.ID == id).FirstOrDefault();

                workSheet.Cells[1, "A"] = "Session";
                workSheet.Cells[1, "B"] = "Group";
                workSheet.Cells[1, "C"] = "Average Mark";
                workSheet.Cells[2, "A"] = $"{session.StartDate} - {session.EndDate}";


                int i = 2;

                foreach (var group in group_ordered)
                {
                    workSheet.Cells[i, "B"] = group.Key;
                    workSheet.Cells[i, "C"] = group.Value;
                    i++;
                }

                workSheet.Cells[i, "A"] = "Specialization mark";

                foreach (var spec in spec_marks)
                {
                    workSheet.Cells[i, "B"] = spec.Key;
                    workSheet.Cells[i, "C"] = spec.Value;
                    i++;
                }

                workSheet.Cells[i, "A"] = "Examiner mark";

                foreach (var examiner in examiner_marks)
                {
                    workSheet.Cells[i, "B"] = examiner.Key;
                    workSheet.Cells[i, "C"] = examiner.Value;
                    i++;
                }

                workBook.Close(true, filepath);
                excelApp.Quit();
            }
        }

        /// <summary>
        /// Generates report about all session's progress in XLXS file
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="TypeOfOrder"></param>
        public void WriteAllSessionProgress(string filepath, string TypeOfOrder)
        {
            using(UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {
                Dictionary<string, List<float>> subject_mark;
                Dictionary<Session, Dictionary<string, List<float>>> results_ordered;
                List<Session> ordered_sessions;

                BuildInformation build = new BuildInformation();
                build.GetAllSessionsResult(db, out results_ordered,
                    out ordered_sessions, out subject_mark, TypeOfOrder);

                Application excelApp = new Application();
                Workbook workBook = excelApp.Workbooks.Add();
                Worksheet workSheet = workBook.ActiveSheet;
                workSheet.Cells[1, "A"] = "Session";
                workSheet.Cells[1, "B"] = "Group";
                workSheet.Cells[1, "C"] = "Max Mark";
                workSheet.Cells[1, "D"] = "Average mark";
                workSheet.Cells[1, "E"] = "Min Mark";

                int i = 2;
                foreach (var session_elem in results_ordered)
                {
                    workSheet.Cells[i, "A"] = $"{session_elem.Key.StartDate} - {session_elem.Key.EndDate}";
                    foreach (var group in session_elem.Value)
                    {
                        workSheet.Cells[i, "B"] = group.Key;
                        workSheet.Cells[i, "C"] = group.Value[0];
                        workSheet.Cells[i, "D"] = group.Value[1];
                        workSheet.Cells[i, "E"] = group.Value[2];
                        i++;
                    }
                }

                workSheet.Cells[i, "A"] = "Subject";
                int j = 2;

                foreach (Session session in ordered_sessions)
                {
                    workSheet.Cells[i, j] = $"{session.StartDate} - {session.EndDate}";
                    j++;
                }

                i += 1;

                foreach (var subject in subject_mark)
                {
                    j = 1;
                    workSheet.Cells[i, j] = subject.Key;
                    j++;
                    foreach (var mark in subject.Value)
                    {
                        workSheet.Cells[i, j] = mark;
                        j++;
                    }
                    i++;
                }

                workBook.Close(true, filepath);
                excelApp.Quit();
            }
        }

        /// <summary>
        /// Method for generating all D-students names from all of the groups
        /// </summary>
        /// <param name="university">DBContext</param>
        /// <param name="filepath">Where to save</param>
        /// <param name="TypeOfOrder">Sorting type</param>
        public void WriteAllDStudents(string filepath, string TypeOfOrder = "OrderBy")
        {
            using (UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {
                BuildInformation building = new BuildInformation();
                Dictionary<string, List<string>> ordered_group = building.GetAllDStudents(db, TypeOfOrder);

                Application excelApp = new Application();
                Workbook workBook = excelApp.Workbooks.Add();
                Worksheet workSheet = workBook.ActiveSheet;
                workSheet.Cells[1, "A"] = "Group";
                workSheet.Cells[1, "B"] = "Allocated students";
                int i = 2;
                foreach (var group in ordered_group)
                {
                    workSheet.Cells[i, "A"] = $"{group.Key}";
                    foreach (var student in group.Value)
                    {
                        workSheet.Cells[i, "B"] = student;
                        i++;
                    }
                }
                workBook.Close(true, filepath);
                excelApp.Quit();
            }
        }
    }
}
