using System;
using System.Collections.Generic;
using EPAMTask7._1.LINQTOSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORM.CRUD;

namespace Task7UnitTests
{
    /// <summary>
    /// CRUD usage test cases
    /// </summary>
    [TestClass]
    public class SimpleCRUDTest
    {
        /// <summary>
        /// CRUD Read(Get) and Insert test
        /// </summary>
        [TestMethod]
        public void CRUD_Insertion_And_ReadingTest()
        {
            Specialization specialization = new Specialization() { ID = 99, Name = "Phisic" };
            CRUD<Specialization> crud = new CRUD<Specialization>();
            crud.Add(specialization, "ID");

            Specialization t_specialization = crud.Get(t => t.ID == specialization.ID);

            Assert.AreEqual(t_specialization.ID, specialization.ID);
            Assert.AreEqual(t_specialization.Name.Trim(), specialization.Name);

            crud.Delete(item => item.ID == t_specialization.ID);
        }

        /// <summary>
        /// CRUD Read(Get) and Insert Exeption test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CRUD_Insertion_And_ReadingTest_Exception()
        {
            Specialization specialization = new Specialization() { ID = 1, Name = "Phisic1" };
            CRUD<Specialization> crud = new CRUD<Specialization>();
            crud.Add(specialization, "ID");

            Specialization t_specialization = crud.Get(t => t.ID == specialization.ID);
        }

        /// <summary>
        /// CRUD Update test
        /// </summary>
        [TestMethod]
        public void CRUD_Update_Test()
        {
            Specialization specialization = new Specialization() { ID = 99, Name = "Phisic" };
            Specialization newSpecialization = new Specialization() { ID = 99, Name = "Chemical-engeneer" };

            CRUD<Specialization> crud = new CRUD<Specialization>();
            crud.Add(specialization, "ID");

            crud.Update(newSpecialization, e => e.ID == newSpecialization.ID);

            Specialization t_specialization = crud.Get(t => t.ID == specialization.ID);

            Assert.AreEqual(t_specialization.ID, newSpecialization.ID);
            Assert.AreEqual(t_specialization.Name.Trim(), newSpecialization.Name);

            crud.Delete(item => item.ID == t_specialization.ID);
        }

        /// <summary>
        /// CRUD Update test Exception test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void CRUD_Update_Test_Exception()
        {
            Specialization specialization = new Specialization() { ID = 99, Name = "Phisic" };
            Specialization newSpecialization = new Specialization() { ID = 99, Name = "Chemical-engeneer" };

            CRUD<Specialization> crud = new CRUD<Specialization>();
            crud.Update(newSpecialization, e => e.ID == newSpecialization.ID);
        }

        /// <summary>
        /// CRUD Delete test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void CRUD_Delete_Test()
        {
            Specialization specialization = new Specialization() { ID = 99, Name = "Phisic" };

            CRUD<Specialization> crud = new CRUD<Specialization>();
            crud.Add(specialization, "ID");
            crud.Delete(item => item.ID == specialization.ID);

            Specialization t_specialization = crud.Get(t => t.ID == specialization.ID);

            Assert.IsNull(t_specialization);
        }

        /// <summary>
        /// CRUD Update test Exception test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void CRUD_Delete_Test_Exception()
        {
            CRUD<Specialization> crud = new CRUD<Specialization>();
            crud.Delete(e => e.ID == 9999);
        }
    }
}
