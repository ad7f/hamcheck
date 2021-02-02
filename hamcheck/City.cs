using System;
using System.Collections.Generic;
using System.Text;

namespace hamcheck
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
    }
}
