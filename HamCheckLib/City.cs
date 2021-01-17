using System;
using System.Collections.Generic;
using System.Text;

namespace HamCheckLib
{
    public class City
    {
        public string cityName;
        public string stateName;

        public City(string city)
        {
            cityName = city;
            stateName = null;
        }

        public City(string city, string state)
        {
            cityName = city;
            stateName = state;
        }

        public string GetSqlWhere()
        {
            string sql = "";

            if (!string.IsNullOrWhiteSpace(cityName))
            {
                sql = " (city = '" + cityName.Replace("'", "''") + "' COLLATE NOCASE";
                if (stateName != null)
                {
                    sql = sql + " AND state='" + stateName.Replace("'", "''") + "' COLLATE NOCASE)";
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
