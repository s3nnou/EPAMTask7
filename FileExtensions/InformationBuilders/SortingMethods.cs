using EPAMTask7._1.LINQTOSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensions.InformationBuilders
{
    /// <summary>
    /// Sorting methods for generated data
    /// </summary>
    public class SortingMethods
    {
        /// <summary>
        /// Method that order data that should be written
        /// </summary>
        /// <param name="dict"> Dictionary of data that should be ordered </param>
        /// <param name="TypeOfOrder"> Type of order </param>
        /// <returns> Ordered dictionary of data </returns>
        public  Dictionary<string, float> OrderList(Dictionary<string, float> dict, string TypeOfOrder)
        {
            switch (TypeOfOrder)
            {
                case "OrderBy":
                    {
                        var group_ordered = from group_elem in dict
                                            orderby group_elem.Key
                                            select group_elem;

                        Dictionary<string, float> valuePairs = new Dictionary<string, float>();

                        foreach (var elem in group_ordered)
                        {
                            valuePairs.Add(elem.Key, elem.Value);
                        }

                        return valuePairs;
                    }
                case "Descending":
                    {
                        var group_ordered = dict.OrderByDescending(t => t.Key);

                        Dictionary<string, float> valuePairs = new Dictionary<string, float>();

                        foreach (var elem in group_ordered)
                        {
                            valuePairs.Add(elem.Key, elem.Value);
                        }

                        return valuePairs;
                    }
                case "Reverse":
                    {
                        var group_ordered = dict.Reverse();

                        Dictionary<string, float> valuePairs = new Dictionary<string, float>();

                        foreach (var elem in group_ordered)
                        {
                            valuePairs.Add(elem.Key, elem.Value);
                        }

                        return valuePairs;
                    }
                default:
                    {
                        throw new Exception("No such type of ordering");
                    }
            }
        }
        /// <summary>
        /// Method that order data that should be written
        /// </summary>
        /// <param name="dict"> Dictionary of data that should be ordered </param>
        /// <param name="TypeOfOrder"> Type of order </param>
        /// <returns> Ordered dictionary of data </returns>
        public Dictionary<Session, Dictionary<string, List<float>>> OrderList(Dictionary<Session, Dictionary<string, List<float>>> dict, string TypeOfOrder)
        {
            Dictionary<Session, Dictionary<string, List<float>>> ordered_list = new Dictionary<Session, Dictionary<string, List<float>>>();
            switch (TypeOfOrder)
            {
                case "OrderBy":
                    {
                        var group_ordered = from group_elem in dict
                                            orderby group_elem.Key.StartDate
                                            select group_elem;

                        foreach (var group_elem in group_ordered)
                        {
                            var names = from student in group_elem.Value
                                        orderby student.Key
                                        select student;

                            Dictionary<string, List<float>> valuePairs = new Dictionary<string, List<float>>();

                            foreach (var name in names)
                            {
                                valuePairs.Add(name.Key, name.Value);
                            }

                            ordered_list.Add(group_elem.Key, valuePairs);
                        }

                        return ordered_list;
                    }
                case "Descending":
                    {
                        var group_ordered = dict.OrderByDescending(t => t.Key.StartDate);

                        foreach (var group_elem in group_ordered)
                        {
                            var names = group_elem.Value.OrderByDescending(t => t.Key);

                            Dictionary<string, List<float>> valuePairs = new Dictionary<string, List<float>>();

                            foreach (var name in names)
                            {
                                valuePairs.Add(name.Key, name.Value);
                            }

                            ordered_list.Add(group_elem.Key, valuePairs);
                        }

                        return ordered_list;
                    }
                case "Reverse":
                    {
                        var group_ordered = dict.Reverse();

                        Dictionary<Session, Dictionary<string, List<float>>> valuePairs = new Dictionary<Session, Dictionary<string, List<float>>>();

                        foreach (var groups in group_ordered)
                        {
                            valuePairs.Add(groups.Key, groups.Value);
                        }

                        return valuePairs;
                    }
                default:
                    {
                        throw new Exception("No such type of ordering");
                    }
            }
        }

        /// <summary>
        /// Method that order data that should be written
        /// </summary>
        /// <param name="dict"> Dictionary of data that should be ordered </param>
        /// <param name="TypeOfOrder"> Type of order </param>
        /// <returns> Ordered dictionary of data </returns>
        public Dictionary<string, List<string>> OrderList(Dictionary<string, List<string>> dict, string TypeOfOrder)
        {
            Dictionary<string, List<string>> ordered_list = new Dictionary<string, List<string>>();
            switch (TypeOfOrder)
            {
                case "OrderBy":
                    {
                        var group_ordered = from group_elem in dict
                                            orderby group_elem.Key
                                            select group_elem;

                        foreach (var group_elem in group_ordered)
                        {
                            var names = from student in group_elem.Value
                                        orderby student
                                        select student;

                            List<string> valuePairs = new List<string>();

                            foreach (var name in names)
                            {
                                valuePairs.Add(name);
                            }

                            ordered_list.Add(group_elem.Key, valuePairs);
                        }

                        return ordered_list;
                    }
                case "Descending":
                    {
                        var group_ordered = dict.OrderByDescending(t => t.Key);

                        foreach (var group_elem in group_ordered)
                        {
                            var names = group_elem.Value.OrderByDescending(t => t);

                            List<string> valuePairs = new List<string>();

                            foreach (var name in names)
                            {
                                valuePairs.Add(name);
                            }

                            ordered_list.Add(group_elem.Key, valuePairs);
                        }

                        return ordered_list;
                    }
                case "Reverse":
                    {
                        var group_ordered = dict.Reverse();

                        Dictionary<string, List<string>> valuePairs = new Dictionary<string, List<string>>();

                        foreach (var groups in group_ordered)
                        {
                            valuePairs.Add(groups.Key, groups.Value);
                        }

                        return valuePairs;
                    }
                default:
                    {
                        throw new Exception("No such type of ordering");
                    }
            }
        }
    }
}
