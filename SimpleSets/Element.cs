using System;

namespace SimpleSets
{
    public class Element : IComparable
    {
        //Data field for the element string
        private string element;

        public string ElementId
        {
            get { return element; }
            private set
            {
                if(value[0] == '{' && value[value.Length -1] == '}')
                {
                    string s = value;
                    s = s.Remove(0, 1);
                    s = s.Remove(s.Length - 1);
                    if (s.Contains("{") || s.Contains("}"))
                        throw new ArgumentException("Please do not nest a set inside an element!!");
                    element = value;
                }//for set as an element
                else if(value[0] == value[value.Length - 1])
                {
                    element = value;
                }//for a single character element
                else
                {
                    if (value.Contains("{") || value.Contains("}"))
                        throw new ArgumentException("Please do not nest a set inside an element!!");
                    element = value;
                }//for multiple characters element
            }//setter
        }//ElementId
        public Element(string elementID)
        {
            ElementId = elementID;
        }//CTOR 01
        public override string ToString()
        {
            return ElementId;
        }//ToString()

        public int CompareTo(object obj)
        {
            return string.Compare(obj.ToString(), ElementId);
        }//CompareTo
    }//class
}//namepscae