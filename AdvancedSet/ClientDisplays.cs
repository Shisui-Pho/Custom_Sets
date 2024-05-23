using SetLibrary;
using SetLibrary.Collections;
using SetLibrary.Generic;
using System;
using System.Windows.Forms;

namespace AdvancedSet
{
    //This client will be for displaying 
    public partial class Client
    {
        private static void DisplaySets(ISetCollection<int> collection)
        {
            Console.WriteLine();
            foreach (Set item in collection)
            {
                Console.WriteLine($"\t{item.Name.PadRight(5)} : {item.ElementString.PadRight(60)}  |{item.Name}| = {item.Cardinality}");
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
        }//AddTestData
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
            Console.Write("\t {0}", msg);
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
            Console.WriteLine("\t1. Add new set.");
            Console.WriteLine("\t2. Remove a set.");
            Console.WriteLine("\t3. Check for subset.");
            Console.WriteLine("\t4. Check for element.");
            Console.WriteLine("\t5. Perform set operation.");
            Console.WriteLine("\t6. Perform set laws.");
            Console.WriteLine("\t7. Clear all sets.");
            Console.WriteLine("\t8. Reset set naming.");
            Console.WriteLine("\t9. Learn more about sets.");
            Console.WriteLine("\tX. Exit.");
        }//DisplayMenuOptions
    }//class
}//namespace
