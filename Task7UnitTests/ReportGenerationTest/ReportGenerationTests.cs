using FileExtensions.XLXSGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Task7UnitTests.ReportGenerationTest
{
    /// <summary>
    /// Reports generation test classes
    /// </summary>
    [TestClass]
    public class ReportGenerationTests
    {
        /// <summary>
        /// Writes file with session results by session id
        /// </summary>
        [TestMethod]
        public void Report1GenerationTest()
        {
            var filepath = Environment.CurrentDirectory + @"\a.xlsx";
            var TypeOfOrder = "OrderBy";
            GenerateXLXS writer = new GenerateXLXS();

            writer.WriteSpecialityGrade(1, filepath, TypeOfOrder);

            long strs = 0;
            using (var reader = new FileStream(filepath, FileMode.Open))
            {
                strs = reader.Length;
            }

            Assert.IsTrue(strs > 0);
        }

        /// <summary>
        /// Writes file with all session results
        /// </summary>
        [TestMethod]
        public void Report2GenerationTest()
        {
            var filepath = Environment.CurrentDirectory + @"\b.xlsx";
            var TypeOfOrder = "OrderBy";

            GenerateXLXS writer = new GenerateXLXS();

            writer.WriteAllSessionProgress(filepath, TypeOfOrder);

            long strs = 0;
            using (var reader = new FileStream(filepath, FileMode.Open))
            {
                strs = reader.Length;
            }

            Assert.IsTrue(strs > 0);
        }

        /// <summary>
        /// Writes file with all D-Students (4 or less marks) by groups
        /// </summary>
        [TestMethod]
        public void Report3GenerationTest()
        {
            var filepath = Environment.CurrentDirectory + @"\c.xlsx";

            GenerateXLXS writer = new GenerateXLXS();

            writer.WriteAllDStudents(filepath, "OrderBy");

            long strs = 0;
            using (var reader = new FileStream(filepath, FileMode.Open))
            {
                strs = reader.Length;
            }

            Assert.IsTrue(strs > 0);
        }

        /// <summary>
        /// Creates exeption by trying to parse wrong type of order
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WriteReport4()
        {
            var filepath = Environment.CurrentDirectory + @"\d.xlsx";
            GenerateXLXS writer = new GenerateXLXS();

            writer.WriteAllDStudents(filepath, "OrderBuuuy");
        }
    }
}
