using System;
using System.Collections.Generic;
using System.Text;

namespace hamcheck
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
    }
}
