using System;

namespace SimpleSets
{
    public class Element : IComparable
    {
        public string ElementId { get; private set; }
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