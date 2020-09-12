using EPAMTask7._1.LINQTOSQL;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ORM.CRUD
{
    /// <summary>
    /// CRUD implementation with a usage of reflection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CRUD<T> where T : class, new()                                   
    {
        private readonly string connectString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Task7UB;Integrated Security=True";

        /// <summary>
        /// Adds a new record to the DB
        /// </summary>
        /// <param name="entity">Current Object</param>
        /// <param name="IdPropertyName">Name of the property containing identity Column</param>
        /// <returns><see cref="System.Object"/> </returns>
        public void Add(T entity, string IdPropertyName)
        {
            using (UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {
                try
                {
                    db.GetTable<T>().InsertOnSubmit(entity);
                    db.SubmitChanges();
                }
                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();

                    for (int ind = 0; ind < ex.Errors.Count; ind++)
                    {
                        errorMessages.Append("Index #" + ind + "\n" +
                            "Message: " + ex.Errors[ind].Message + "\n" +
                            "Error Number: " + ex.Errors[ind].Number + "\n" +
                            "LineNumber: " + ex.Errors[ind].LineNumber + "\n" +
                            "Source: " + ex.Errors[ind].Source + "\n" +
                            "Procedure: " + ex.Errors[ind].Procedure + "\n");
                    }

                    throw new Exception(errorMessages.ToString());
                }

            }
        }

        /// <summary>
        /// Gets a one item from the table
        /// </summary>
        /// <param name="idSelector">Get query</param>
        /// <returns></returns>
        public T Get(Func<T, bool> idSelector)
        {
            using (UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {
                try
                {
                    db.DeferredLoadingEnabled = false;
                    return db.GetTable<T>().Single(idSelector); 
                }
                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();

                    for (int ind = 0; ind < ex.Errors.Count; ind++)
                    {
                        errorMessages.Append("Index #" + ind + "\n" +
                            "Message: " + ex.Errors[ind].Message + "\n" +
                            "Error Number: " + ex.Errors[ind].Number + "\n" +
                            "LineNumber: " + ex.Errors[ind].LineNumber + "\n" +
                            "Source: " + ex.Errors[ind].Source + "\n" +
                            "Procedure: " + ex.Errors[ind].Procedure + "\n");
                    }

                    throw new Exception(errorMessages.ToString());
                }
            }
        }

        /// <summary>
        /// Deletes the entity upon the defined query
        /// </summary>
        /// <param name="query">Delete Query</param>
        public void Delete(Expression<Func<T, bool>> query)
        {
            using (UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {

                try
                {
                    db.GetTable<T>().DeleteOnSubmit(db.GetTable<T>().Where(query).Single());
                    db.SubmitChanges();
                }
                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();

                    for (int ind = 0; ind < ex.Errors.Count; ind++)
                    {
                        errorMessages.Append("Index #" + ind + "\n" +
                            "Message: " + ex.Errors[ind].Message + "\n" +
                            "Error Number: " + ex.Errors[ind].Number + "\n" +
                            "LineNumber: " + ex.Errors[ind].LineNumber + "\n" +
                            "Source: " + ex.Errors[ind].Source + "\n" +
                            "Procedure: " + ex.Errors[ind].Procedure + "\n");
                    }

                    throw new Exception(errorMessages.ToString());
                }

                
            }
        }
        /// <summary>
        /// Updates Entity
        /// </summary>
        /// <param name="entity">Entity which hold the updated information</param>
        /// <param name="query">query to get the same entity from db and replace it</param>
       
        public virtual void Update(T entity, Expression<Func<T, bool>> query)
        {
            using (UniversityClassesDataContext db = new UniversityClassesDataContext(connectString))
            {
                object propertyValue = null;
                T entityFromDB = db.GetTable<T>().Where(query).SingleOrDefault();

                if (null == entityFromDB)
                    throw new NullReferenceException("Query Supplied to Get entity from DB is invalid, NULL value returned");

                PropertyInfo[] properties = entityFromDB.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    propertyValue = null;

                    if (null != property.GetSetMethod())
                    {
                        PropertyInfo entityProperty = entity.GetType().GetProperty(property.Name);

                        if (entityProperty.PropertyType.BaseType == Type.GetType("System.ValueType") || entityProperty.PropertyType == Type.GetType("System.String"))
                            propertyValue = entity.GetType().GetProperty(property.Name).GetValue(entity, null);

                        if (null != propertyValue)
                            property.SetValue(entityFromDB, propertyValue, null);
                    }
                }

                try
                {
                    db.SubmitChanges();
                }
                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();

                    for (int ind = 0; ind < ex.Errors.Count; ind++)
                    {
                        errorMessages.Append("Index #" + ind + "\n" +
                            "Message: " + ex.Errors[ind].Message + "\n" +
                            "Error Number: " + ex.Errors[ind].Number + "\n" +
                            "LineNumber: " + ex.Errors[ind].LineNumber + "\n" +
                            "Source: " + ex.Errors[ind].Source + "\n" +
                            "Procedure: " + ex.Errors[ind].Procedure + "\n");
                    }

                    throw new Exception(errorMessages.ToString());
                }
            }
        }
    }
}
