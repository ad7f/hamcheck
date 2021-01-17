using System;
using System.Collections.Generic;
using System.Text;

namespace HamCheckLib
{
    public class Person
    {
        public string lastName;
        public string firstName;

        public Person(string last)
        {
            lastName = last;
            firstName = "";
        }

        public Person(string last, string first)
        {
            lastName = last;
            if (string.IsNullOrWhiteSpace(first))
            {
                firstName = "";
            }else
            {
                firstName = first;
            }            
        }

        public string GetSqlWhere()
        {
            string sql = "";

            if (lastName != null)
            {
                sql = " (last_name = '" + lastName.Replace("'","''") + "' ";
                if (firstName != null)
                {
                    sql = sql + " AND first_name='" + firstName.Replace("'", "''") + "' )";
                }
                else
                {
                    sql = sql + ") ";
                }
            }

            return sql; 
        }
    }
}
