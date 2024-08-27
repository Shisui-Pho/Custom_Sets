using SetLibrary;
using SetLibrary.Collections;
using SetLibrary.Generic;
using SetLibrary.Operations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
namespace AdvancedSet
{
    //This client will be for displaying 
    public partial class Client
    {
        public delegate void delDisplayInfo_Func();

        private static readonly string info_file_pdf = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Sets - Information Document.pdf");
        private static readonly string info_file_docx = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Sets - Information Document.docx");
        private static readonly string _dataFile = Path.Combine(Directory.GetCurrentDirectory(), "Files", "data.txt");

        private static int max_set_length = 0;
        private static void Welcome()
        {
            Console.WriteLine("\tImprovements");
            Console.WriteLine("\t============");
            Console.WriteLine("\t1. Now you can nest elements as much as you want in any way(biggest improvement).");
            Console.WriteLine("\t2. You modify a given set by removing and adding elements.");
            Console.WriteLine("\t3. More advanced set operations have been added.");
            Console.WriteLine("\t3. More sets properties have been added.");
            Console.WriteLine("\t4. Improved element sorting accuracy.");
            Console.WriteLine("\t5. Improved set string evaluation.");
            Console.WriteLine("\t6. Improved braces evaluation check.");
            Console.WriteLine("\t7. Improved overall perfomance of the application.");

            Console.WriteLine();
            Console.WriteLine("\tHOPE YOU ENJOY USING THE APPLICATION");
            AnyKey(KeyType.Continue);
            ClearConsoleWindow();
        }//Welcome
        private static void DisplaySets(ISetCollection<int> collection, string header = "Current sets")
        {
            Console.WriteLine();
            Console.WriteLine($"\t{header} : ");
            Console.WriteLine();
            foreach (Set item in collection)
            {
                Console.WriteLine($"\t{item.Name.PadRight(5)} : {item.ElementString.PadRight(max_set_length + 10)}  |{item.Name}| = {item.Cardinality}");
            }//end foreach
            Console.WriteLine();
            Console.WriteLine();
        }//DisplaySets
        private static void AddTestData(ISetCollection<int> sets)
        {
            //Creating the sets
            string expression = "{{3,4,1,5,8,{6,2,5,{8,9},{9,6}}},5,6,{6,3},9,1,0}";
            ICSet<int> set = new GenericSet<int>(expression, settings);
            sets.Add(set);

            expression = "{2,5,1,3,4,5,6}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);

            expression = "{{{{{{}}}}}}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);

            expression = "{10,{2,2,1},{6},5,2,5,5,8}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);

            expression = "{{2,1,7,3},{1},{2,5,5},{8,5,1,4,5},{1,2,5},{2,3}}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);

            expression = "{5,6,1,1,1,2,4,5,{2,1,2,3},{3,2,1,1,2,3,2,3,1,2,3}}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);
            
            expression = "{5,6,{1,2,3}}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);

            expression = "{1,2,4,5,6}";
            set = new GenericSet<int>(expression, settings);
            sets.Add(set);

        }//AddTestData
        private static void Load(ISetCollection<int> sets)
        {
            if (File.Exists(_dataFile))
            {
                string[] data = File.ReadAllLines(_dataFile);
                bool isFirstTime = true;
                foreach (var item in data)
                {
                    try
                    {
                        ICSet<int> set = new GenericSet<int>(item, settings);
                        sets.Add(set);
                    }
                    catch
                    {
                        DisplayCouldNotLoadSetString(item, isFirstTime);
                        isFirstTime = false;
                    }//end catch
                }//end forach
            }//end if
            else
            {
                Console.WriteLine("\tThe file \"{0}\" was not found.", _dataFile);
                Console.WriteLine("\tTest data will be loaded instead.");
                AddTestData(sets);
            }
            Console.WriteLine();
            Console.WriteLine("\tLoaded {0} sets.", sets.Count);
            MaxLength(sets);
            AnyKey(KeyType.Continue);
        }//Load
        private static void Save(ISetCollection<int> collection)
        {
            using(StreamWriter wr = new StreamWriter(_dataFile, false))
            {
                for (int i = 0; i < collection.Count; i++)
                    wr.WriteLine(collection[i].OriginalString);
            }
        }//Save
        private static void DisplayCouldNotLoadSetString(string setString, bool isfirstTime = true)
        {
            if(isfirstTime) 
                Console.WriteLine("\tCould not load the following sets:\n");
            Console.WriteLine("\t{0}", setString);
        }//DisplayCouldNotLoadSetString
        private static void DisplayNewSetHeader(ISetCollection<int> collection)
        {
            Console.Clear();
            Console.WriteLine("\tAdding a new set");
            Console.WriteLine("\t=================");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tThings to note : ");
            Console.WriteLine("\t1. You are allowed to use integers only.");
            Console.WriteLine("\t2. Make sure braces are matching.");
            Console.WriteLine("\t3. Nest as much as you like, but make sure baraces are matching.");
            Console.WriteLine("\t4. Elements must be seperated by commas.");
            Console.WriteLine("\t    Example of a set could be : A = {1,6,8,{2,3},5,{2,6,{8}},5}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t***If you wish to cancel this operation clear everyting and press enter.***");
            Console.WriteLine();
        }//DisplayNewSetHeader
        private static void DisplayError(string error_message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\tError : {0}", error_message);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;
        }//DisplayError
        private static void AnyKey(string msg = "Press any key to return to the main menu....")
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("\t{0}", msg);
            Console.ReadKey();
        }//AnyKey
        private static string ReadSetString(string sPrompt, string attempt = "")
        {
            //First clean the string to prepare it to be passed keys
            string @new = "";
            foreach (char character in attempt)
            {
                if (character == '{')
                    @new += "{{}";
                else if (character == '}')
                    @new += "{}}";
                else
                    @new += character;
            }//end foreach

            Console.Write(sPrompt);
            Console.CursorVisible = true;
            try
            {
                //SendKeys.SendWait("This is phio");
                SendKeys.SendWait(@new);
            }//end try
            catch
            {
                SendKeys.SendWait("");
            }//end catch
            string setString = Console.ReadLine();
            return setString;
        }//Crashed
        private static void DisplayMenuOptions()
        {
            Console.WriteLine("\t1.  Add new set.");
            Console.WriteLine("\t2.  Remove a set.");
            Console.WriteLine("\t3.  Modify a set.");
            Console.WriteLine("\t4.  Check for subset.");
            Console.WriteLine("\t5.  Check for element.");
            Console.WriteLine("\t6.  Perform set operation.");
            Console.WriteLine("\t7.  Perform set laws.");
            Console.WriteLine("\t8.  Clear all sets.");
            Console.WriteLine("\t9.  Reset set naming.");
            Console.WriteLine("\t10. Learn more about sets.");
            Console.WriteLine("\t11. Developer's Information.");
            Console.WriteLine("\tX.  Exit.");
        }//DisplayMenuOptions
        private static void DisplaSetsWarning(SetWarting warning)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\tWarning :");
            string about_to = "";
            string operation_about_to = "";
            string when_ = "";
            switch (warning)
            {
                case SetWarting.Clear:
                    about_to = "clear all the current sets";
                    operation_about_to = "remove/delete all the sets that have";
                    when_ = "Cleared";
                    break;
                case SetWarting.Remove_Set:
                    about_to = "Remove a set in the current sets";
                    operation_about_to = "remove/delete the set that has";
                    when_ = "Set is removed";
                    break;
                case SetWarting.Remove_Element:
                    about_to = "Remove an element in a set";
                    operation_about_to = "remove/delete the element that has";
                    when_ = "element is removed";
                    break;
                case SetWarting.Reset_Naming:
                    about_to = "Reset the naming of the sets.";
                    operation_about_to = "reset the naming of the set collection that has";
                    when_ = "reset";
                    break;
                default:
                    break;
            }
            Console.WriteLine($"\tYou are about to {about_to}.");
            Console.WriteLine($"\tThis operation will {operation_about_to} been saved.");
            Console.WriteLine($"\tWhen {when_}, it cannot be undone!!.");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }//DisplaClearAllSetsWarning
        private static void DisplayCheckingForSubSetsInformation()
        {
            Console.WriteLine();
            Console.WriteLine("\tThe following results can be expected from checking for subsets(We will use setA and setB).");
            Console.WriteLine("\t1. If all elements in setA are in setB and |setA| = |setB|, then setA is a subset of setB and can also be setB.");
            Console.WriteLine("\t2. If all elements in setA are in setB and |setA| < |setB|, then setA is a proper subset of setB.");
            Console.WriteLine("\t3. If not all elemnts in setA are in setB, then setA is not a subset of setA.");
            Console.WriteLine("\t4. If |setA| > |setB| then setA cannot be a subset of setB.");
            Console.WriteLine();
        }//DisplayCheckingForSubSetsInformation
        private static void DisplayIsSubsetOfOutCome(bool isSubset, ICSet<int> setA, ICSet<int> setB, SetType type)
        {
            Console.Clear();
            Console.WriteLine("\tResults");
            Console.WriteLine("\t=======");
            Console.WriteLine();
            int padding = (setA.ElementString.Length > setB.ElementString.Length) ? setA.ElementString.Length : setB.ElementString.Length;
            padding += 10;
            Console.WriteLine($"\tsetA : {setA.ElementString.PadRight(padding)} |setA| = {setA.Cardinality}");
            Console.WriteLine($"\tsetB : {setB.ElementString.PadRight(padding)} |setB| = {setB.Cardinality}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            switch (type)
            {
                case SetType.SubSet & SetType.Same_Set:
                    Console.WriteLine("\tsetA is a subset of setB. But setA is also the same as setB and so we can assume that setA = setB #");
                    break;
                case SetType.NotASubSet:
                    var diff = setA.Without(setB);
                    Console.WriteLine("\tsetA is not a subset of setB. Not all elements in setA are in setB. The following element(s) are not in setB but are in setA:");
                    Console.WriteLine("\t\t{0}", diff.ElementString);
                    break;
                case SetType.ProperSet:
                    Console.WriteLine("\tsetA is a proper set of setB. If we look at all the elements of setA we can see that they are also in setB.");
                    Console.WriteLine("\tAnd we can also see that |setA| < |setB| which satisfies our proper set definition #");
                    break;
                default:
                    break;

            }//end switch
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }//DisplayIsSubsetOfOutCome
        private static void DisplayContainsResults<T>(bool contains, ICSet<int> set, T element)
        {
            Console.Clear();
            Console.WriteLine("\tResults");
            Console.WriteLine("\t=======");
            Console.WriteLine($"\tThe set {set} {(contains ? "contains" : "does not contain")} the element {element.ToString()}");
            Console.WriteLine();
        }//DisplayContainsResults
        private static void DisplaySetOperationsResults(ICSet<int> outcome, ICSet<int> setA, ICSet<int> setB, SetOperator operation)
        {
            Console.Clear();
            Console.WriteLine("\tOutcome");
            Console.WriteLine("\t=======");
            Console.WriteLine();
            Console.WriteLine("\tsetA : {0}", setA);
            Console.WriteLine("\tsetB : {0}", setB);
            Console.WriteLine();
            switch (operation)
            {
                case SetOperator.Intersection:
                    Console.WriteLine("\tsetA \u2229 setB = {0}", outcome);
                    break;
                case SetOperator.Union:
                    Console.WriteLine("\tsetA U setB = {0}", outcome);
                    break;
                case SetOperator.Difference:
                    Console.WriteLine("\tsetA - setB = {0}", outcome);
                    break;
            }//end switch
            Console.WriteLine();
        }//DisplaySetOperationsResults
        private static void DisplaySetLaws()
        {
            Console.WriteLine();
            Console.WriteLine("\tSET LAWS AND PROPERTIES");
            Console.WriteLine("\t=======================");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t1. Cummutative law:");
            //Console.WriteLine("\t==================");
            Console.WriteLine("\t   A U B = B U A");
            Console.WriteLine("\t   A \u2229 B = B \u2229 A");
            Console.WriteLine();
            Console.WriteLine("\t2. Associative law:");
            //Console.WriteLine("\t==================");
            Console.WriteLine("\t   A U ( B U C ) = ( A U B) U C");
            Console.WriteLine("\t   A \u2229 ( B \u2229 C ) = (A \u2229 B) \u2229 C");
            Console.WriteLine();
            Console.WriteLine("\t3. Distributive law:");
            //Console.WriteLine("\t===================");
            Console.WriteLine("\t   A U ( B \u2229 C ) = ( A U B ) \u2229 ( A U C)");
            Console.WriteLine("\t   A \u2229 ( B U C ) = ( A \u2229 B ) U ( A \u2229 C)");
            Console.WriteLine();
            Console.WriteLine("\t4. Double complement:");
            //Console.WriteLine("\t=====================");
            Console.WriteLine("\t   ~(~A) = A");
            Console.WriteLine();
            Console.WriteLine("\t5. DeMorgan's Law:");
            //Console.WriteLine("\t====================");
            Console.WriteLine("\t   ~( A U B ) = ~A \u2229 ~B");
            Console.WriteLine("\t   ~( A \u2229 B ) = ~A U ~B");
            Console.WriteLine();
            Console.WriteLine("\t6. Identity:");
            //Console.WriteLine("\t===========");
            Console.WriteLine("\t   \u2205 U A = A");
            Console.WriteLine("\t   Universal \u2229 A = A");
            Console.WriteLine();
            Console.WriteLine("\t7. Idempotence:");
            //Console.WriteLine("\t==============");
            Console.WriteLine("\t   A U A = A");
            Console.WriteLine("\t   A \u2229 A = A");
            Console.WriteLine();
            Console.WriteLine("\t8. Dominance: ");
            //Console.WriteLine("\t============");
            Console.WriteLine("\t   A U Univeral = Univeral");
            Console.WriteLine("\t   A \u2229 \u2205 = \u2205");
            Console.WriteLine();

        }//DisplaySetLaws
        private static void DisplaySetLawPropertieMenu()
        {
            Console.WriteLine("\tSet laws and properties");
            Console.WriteLine("\t=======================");
            Console.WriteLine();
            Console.WriteLine("\t1. Cummutative law");
            Console.WriteLine("\t2. Associative law");
            Console.WriteLine("\t3. Distributive law");
            Console.WriteLine("\t4. Double complement");
            Console.WriteLine("\t5. DeMorgan's law");
            Console.WriteLine("\t6. Identity");
            Console.WriteLine("\t7. Idempotence");
            Console.WriteLine("\t8. Dominance");
            Console.WriteLine("\t9. Review laws and properties");
            Console.WriteLine("\tX. Exit");

            Console.WriteLine();
        }//DisplaySetLawPropertieMenu
        private static void DisplaySetlawPropertiesOutcome(TypeOfLawProperty law, ICSet<int> outcomeleft, ICSet<int> outcomeright, ICSet<int> A, ICSet<int> B = default, ICSet<int> C = default, bool isfirst_option = true)
        {
            Console.Clear();
            Console.WriteLine("\tSet law and properties outcome");
            Console.WriteLine("\t==============================");
            Console.WriteLine();
            string leftoutcome = outcomeleft.ToString();
            string rightoutcome = outcomeright.ToString();
            string setA = A.ToString();
            string setB, setC;
            string option_law = "";
            string left_option = "";
            string right_option = "";
            string sets = "";
            int max_length = 0;

            switch (law)
            {
                case TypeOfLawProperty.Cummutativity:
                    setB = B.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length, setB.Length);
                    if(isfirst_option)
                    {
                        option_law = "A U B = B U A";
                        left_option = "A U B";
                        right_option = "B U A";
                    }
                    else
                    {
                        option_law = "A \u2229 B = B \u2229 A";
                        left_option = "A \u2229 B";
                        right_option = "B \u2229 A";
                    }
                    sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                           $"\t B = {setB.PadRight(max_length)} |B| = {B.Cardinality}";
                    break;

                case TypeOfLawProperty.Associative:
                    setB = B.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length, setB.Length);
                    if (isfirst_option)
                    {
                        option_law = "A U ( B U C ) = ( A U B) U C";
                        left_option = "A U ( B U C )";
                        right_option = "( A U B ) U C";
                    }
                    else
                    {
                        option_law = "A \u2229 ( B \u2229 C ) = (A \u2229 B) \u2229 C";
                        left_option = "A \u2229 ( B \u2229 C )";
                        right_option = "( A \u2229 B ) \u2229 C";
                    }
                    sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                           $"\t B = {setB.PadRight(max_length)} |B| = {B.Cardinality}";
                    break;
                case TypeOfLawProperty.Distributive:
                    setB = B.ToString();
                    setC = C.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length, setB.Length, setC.Length);
                    if (isfirst_option)
                    {
                        option_law = "A U ( B \u2229 C ) = ( A U B ) \u2229 ( A U C )";
                        left_option = "A U ( B \u2229 C )";
                        right_option = "( A U B ) \u2229 ( A U C )";
                    }
                    else
                    {
                        option_law = "A \u2229 ( B U C ) = ( A \u2229 B ) U ( A \u2229 C)";
                        left_option = "A \u2229 ( B U C )";
                        right_option = "( A \u2229 B ) U ( A \u2229 C)";
                    }
                    sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                           $"\t B = {setB.PadRight(max_length)} |B| = {B.Cardinality}\n" +
                           $"\t C = {setC.PadRight(max_length)} |C| = {C.Cardinality}";
                    break;
                case TypeOfLawProperty.DoubleComplement:
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length);
                    option_law = "~ ( ~ A ) = A";
                    left_option = "~ ( ~ A )";
                    right_option = "A";
                    sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}";
                    break;
                case TypeOfLawProperty.Identity:
                    setB = B.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length, setB.Length);
                    if (isfirst_option)
                    {
                        option_law = "\u2205 U A = A";
                        left_option = "\u2205 U A";
                        right_option = "A";
                        sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                               $"\t \u2205 = {setB.PadRight(max_length)} |\u2205| = {B.Cardinality}";
                    }
                    else
                    {
                        option_law = "Universal \u2229 A = A";
                        left_option = "Universal \u2229 A";
                        right_option = "A";
                        sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                               $"\t Universal = {setB.PadRight(max_length)} |Universal| = {B.Cardinality}";
                    }

                    break;

                case TypeOfLawProperty.Idemptotence:
                    setA = A.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length);
                    if (isfirst_option)
                    {
                        option_law = "A U A = A";
                        left_option = "A U A";
                        right_option = "A";
                    }
                    else
                    {
                        option_law = "A \u2229 A = A";
                        left_option = "A \u2229 A";
                        right_option = "A";
                    }

                    sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n";
                    break;

                case TypeOfLawProperty.DeMorgan:
                    setB = B.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length, setB.Length);
                    if (isfirst_option)
                    {
                        option_law = "~ ( A U B ) = ~ A \u2229 ~ B";
                        left_option = "~ ( A U B )";
                        right_option = "~ A \u2229 ~ B";
                    }
                    else
                    {
                        option_law = "~ ( A \u2229 B ) = ~ A U ~ B";
                        left_option = "~ ( A \u2229 B )";
                        right_option = "~ A U ~ B";
                    }
                    sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                           $"\t B = {setB.PadRight(max_length)} |B| = {B.Cardinality}";
                    break;

                case TypeOfLawProperty.Dominance:

                    setB = B.ToString();
                    max_length = MaxLength(leftoutcome.Length, rightoutcome.Length, setA.Length, setB.Length);
                    if (isfirst_option)
                    {
                        option_law = "A U Univeral = Univeral";
                        left_option = "A U Univeral";
                        right_option = "Univeral";
                        sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                               $"\t Univeral = {setB.PadRight(max_length)} |Univeral| = {B.Cardinality}";
                    }
                    else
                    {
                        option_law = "A \u2229 \u2205 = \u2205";
                        left_option = "A \u2229 \u2205";
                        right_option = "\u2205";
                        sets = $"\t A = {setA.PadRight(max_length)} |A| = {A.Cardinality}\n" +
                               $"\t \u2205 = {setB.PadRight(max_length)} |\u2205| = {B.Cardinality}";
                    }
                    break;
                default:
                    break;
            }//end switch

            //Gets the max length between the two options
            int max_pad = MaxLength(left_option.Length, right_option.Length) + 4;
            max_length += 5;

            string left_card = $"|{left_option}|";
            string right_card = $"|{right_option}|";
            //Displaying
            Console.WriteLine(sets);
            Console.WriteLine();
            Console.WriteLine("\t{0} :  {1}", law.ToString(), option_law);
            Console.WriteLine();
            Console.Write($"  left side   : {left_option.PadRight(max_pad)} = ");
            Console.Write($"  {leftoutcome.PadRight(max_length)} {left_card.PadRight(max_pad)} = {outcomeleft.Cardinality}");
            Console.WriteLine();
            Console.Write($"  right side  : {right_option.PadRight(max_pad)} = ");
            Console.Write($"  {rightoutcome.PadRight(max_length)} {right_card.PadRight(max_pad)} = {outcomeright.Cardinality}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }//DisplaySetlawPropertiesOutcome
        private static int MaxLength(params int[] lengths)
        {
            int max = lengths[0];
            foreach (var item in lengths)
            {
                if (item > max)
                    max = item;
            }
            return max;
        }
        private static int MaxLength(ISetCollection<int> sets)
        {
            max_set_length = int.MinValue;
            foreach (Set item in sets)
            {
                int newLength = item.ElementString.Length;
                if (newLength > max_set_length)
                    max_set_length = newLength;
            }
            return max_set_length;
        }
        private static void DisplayNotUniversalError(List<ICSet<int>> sets)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tThe operation cannot be perfomed because one or more of the sets: ");
            for (int i = 0; i < sets.Count - 1; i++)
            {
                Console.WriteLine("\t{0}", sets[0]);
            }
            Console.WriteLine();
            Console.WriteLine("\t is/re not part of the universal set: ");
            Console.WriteLine("\t{0}", sets[sets.Count - 1]);
            Console.WriteLine();
            AnyKey(KeyType.Retry);
        }//NotUniversalError
    }//class
}//namespace
