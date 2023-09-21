using System;
namespace SimpleSets
{
    public class CSet
    {
        private Element[] elements;

        public int Cardinality { get; private set; }
        
        public string ElementString
        {
            get 
            {
                //TODO::
                string s = "{";
                for (int i = 0; i < Cardinality; i++)
                {
                    s += elements[i].ElementId + ",";
                }//end for
                s = s.Remove(s.Length - 1);
                s = s + "}";
                return s;
            }//get the element string
        }//Property for elementstring

        #region Indexers
        public Element this[string elementId]
        {
            get 
            {
                int i = IndexOf(elementId);
                if (i >= 0)
                    return null;
                return elements[i];
            }//getter
        }//indexer
        public Element this[Element element]
        {
            get
            {
                int i = IndexOf(element);
                if (i >= 0)
                    return null;
                return elements[i];
            }//getter
        }//indexer
        public Element this[int index]
        {
            get
            {
                if (index >= elements.Length)
                    throw new IndexOutOfRangeException();
                return elements[index];
            }//getter
        }//iundexer
        #endregion
        #region Constructors
        public CSet()
        {
            elements = new Element[0];
        }//ctor default

        public CSet(string elementString)
        {
            Element[] elements =  BreakDownString(elementString);
            CheckIfElementIsUniqueAndAdd(elements);
            Cardinality = this.elements.Length;//Set the cardinality
        }//ctor 2 for element string
        public CSet(Element[] elements)
        {
            CheckIfElementIsUniqueAndAdd(elements);
            Cardinality = this.elements.Length;//Set the cardinality
        }//ctor 2 
        #endregion
        #region BreakingDownStrings and Elements
        private Element[] BreakDownString(string elementString)
        {
            //Chheck for braces
            if (elementString[0] != '{' || elementString[elementString.Length - 1] != '}')
                throw new ArgumentException("The set string is not in the right format");

            elementString = elementString.Remove(0, 1);//Remove the firstBrace
            elementString = elementString.Remove(elementString.Length - 1);//Remove the las brace

            return IdentifyAndGetSetsAsElements(elementString);
        }//BreakDownString
        private Element[] IdentifyAndGetSetsAsElements(string elementString)
        {
            int[] oppeningbraces = new int[0];
            int[] clossingbraces = new int[0];
            for (int i = 0; i < elementString.Length; i++)
            {
                if(elementString[i] == '{')
                {
                    Array.Resize(ref oppeningbraces, oppeningbraces.Length + 1);
                    oppeningbraces[oppeningbraces.Length-1] = i;
                }//for oppening braces
                if(elementString[i] == '}')
                {
                    Array.Resize(ref clossingbraces, clossingbraces.Length + 1);
                    clossingbraces[clossingbraces.Length - 1] = i;
                }//for clossing braces
            }//end for

            if (oppeningbraces.Length > clossingbraces.Length)
                throw new ArgumentException("Missing a closing brace");
            if (clossingbraces.Length > oppeningbraces.Length)
                throw new ArgumentException("Missing an oppening brace");
            if (oppeningbraces.Length == 0 && clossingbraces.Length == 0)
                return GetNormalElements(elementString);

            string sStringElement = elementString;
            Element[] elements = new Element[oppeningbraces.Length];//There will be "n = number of oppening/clossing braces" elements as set
            for (int i = 0; i < oppeningbraces.Length; i++)
            {
                string el = elementString.Substring(oppeningbraces[i], clossingbraces[i] - oppeningbraces[i] + 1); // Get the set element
                elements[i] = new Element(el);//Insert the lement in the elements[]
                sStringElement = sStringElement.Replace(el, "");//Removing the element from the rest of the string
            }//end for

            string[] _restOfThelements = sStringElement.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Resize(ref elements, elements.Length + _restOfThelements.Length);
            int iStart = oppeningbraces.Length;

            for (int i = 0; i < _restOfThelements.Length; i++)
            {
                elements[iStart] = new Element(_restOfThelements[i]);
                iStart++;
            }//end for
            Array.Resize(ref elements, iStart);
            return elements;
        }//IdentifyAndGetSetsAsElements
        private Element[] GetNormalElements (string s)
        {
            string[] el = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Element[] elements = new Element[el.Length];
            for (int i = 0; i < el.Length; i++)
            {
                elements[i] = new Element(el[i]);
            }
            return elements;
        }//GetNormalElements
        private int currentLength = 0;
        private void CheckIfElementIsUniqueAndAdd(Element[] elements)
        {
            //Array.Sort(elements);
            Array.Resize(ref this.elements, elements.Length);
            int iCount = 0;
            this.elements[0] = elements[0];
            //currentLength = 1;
            for (int i = 0; i < elements.Length; i++)
            {
                if(IndexOf(elements[i].ElementId) == -1)
                {
                    this.elements[iCount] = elements[i];
                    iCount++;
                    currentLength++;
                }//check for unique elements
            }//end for
            Array.Resize(ref this.elements, iCount);
            Array.Sort(this.elements);
            //Array.Reverse(this.elements);
        }//CheckIfElementIsUnique
        #endregion
        #region IndexOf
        public int IndexOf(Element element)
        {
            for (int i = 0; i < currentLength; i++)
            {
                if (elements[i].ToString() == element.ToString())
                    return i;
            }
            return -1;
        }//IndexOf
        public int IndexOf(string element)
        {
            for (int i = 0; i < currentLength; i++)
            {
                if (elements[i].ToString() == element)
                    return i;
            }
            return -1;
        }//IndexOf
        public SetType IsSubsetOf(CSet setB)
        {
            int iMatch = 0;
            if (this.Cardinality > setB.Cardinality)
                return SetType.IsNotSubSetGreater;
            if(Cardinality == setB.Cardinality) //They are equal and Sorted
            {
                for (int i = 0; i < Cardinality; i++)
                {
                    int iVar = Array.BinarySearch(setB.GetElements(), this[i]);
                    if (iVar >= 0)
                        iMatch++;
                }//end for
                if (iMatch == Cardinality || iMatch == setB.Cardinality)
                    return SetType.SameSet;
            }//end if
            iMatch = 0;
            if(Cardinality < setB.Cardinality)
            {
                for(int i = 0; i< this.Cardinality; i++)
                {
                    if (setB.IndexOf(this.elements[i]) >= 0)
                        iMatch++;
                }
                if (iMatch == Cardinality)//If all elements in A are in B, then A is a proper set of B
                    return SetType.ProperSet;
            }//IsSubsetOf

            return SetType.IsNotSubSet; //For this to be a subset, the cardinbality must equal or less than the cardinality of B
        }//IsSubsetOf
        #endregion
        #region Set Operations using Operator overloading

        //TO DO Set Operation
        public static CSet operator &(CSet setA, CSet setB)
        {
            string elementstring = "{";
            for (int i = 0; i < setA.Cardinality; i++)
            {
                for (int j = 0; j < setB.Cardinality; j++)
                {
                    if (setA[i].ElementId == setB[j].ElementId)
                        elementstring += setA[i].ElementId + ",";
                }//for rows
            }//for columns
            elementstring = elementstring.Remove(elementstring.Length - 1);
            elementstring += "}";

            return new CSet(elementstring);//Return the Set
        }//Intersection operator
        
        public static CSet operator -(CSet setA, CSet setB)
        {
            string elementstring = "{";
            for (int i = 0; i < setA.Cardinality; i++)
            {
                if (setB.IndexOf(setA[i].ElementId) == -1)
                    elementstring += setA[i].ElementId + ",";
            }//end for
            elementstring = elementstring.Remove(elementstring.Length - 1);
            elementstring += "}";
            return new CSet(elementstring);//Return the set
        }//A without B operator

        public static CSet operator |(CSet setA, CSet setB)
        {
            //Extract the element string for set A
            string sSetA = setA.ElementString;
            sSetA = sSetA.Remove(sSetA.Length - 1) + ",";

            //Extract the element string for set B
            string sSetB = setB.ElementString;
            sSetB = sSetB.Remove(0, 1);

            return new CSet(sSetA + sSetB);
        }//A or B opeartor

        public static CSet operator *(CSet setA, CSet setB)
        {
            //TODO : Multiplication
            return default;
        }//A X B operator

        #endregion

        public bool IsElementOf(CSet setB) => setB.IndexOf(this.ElementString) >= 0;
        public Element[] GetElements() => this.elements;
    }//class
    public enum SetType
    {
        ProperSet, 
        SameSet, 
        IsSubSet,
        IsNotSubSet,
        IsNotSubSetGreater
    }//SetType
    public enum ElementType
    {
        IsElement, 
        IsNotElement
    }//ElementType
    public enum OperationType
    {
        X_Minus_Y,
        Y_Minus_X,
        X_And_Y,
        X_OR_Y
    }//OperationType
}//namepscae
