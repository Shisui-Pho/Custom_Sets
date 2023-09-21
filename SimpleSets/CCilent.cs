using System;
using System.Runtime.InteropServices;
using System.IO;
namespace SimpleSets
{
    //This Partial class will handle the UI displays and loading test data
    public partial class CClient
    {
        private static readonly string SetsInfo_FileName1 = @"Files/Sets Application Extensions.docx";
        private static readonly string SetsInfo_FileName2 = @"Files/Sets Application Extensions.pdf";
        private static readonly string SetsData_FileName = @"Files/SetCollection.txt";
        //Window setUp
        private static void TestData(SetCollection<CSet> coll)
        {
            string elementString = "{5,8,1,2,{5,8,9,6,33},7,8,14}";
            CSet set = new CSet(elementString);
            coll.Add(set);

            elementString = "{1,5,0,2,5}";
            set = new CSet(elementString);
            coll.Add(set);

            elementString = "{5,2,{5,6},5}";
            set = new CSet(elementString);
            coll.Add(set);
        }//TestData
        private static void SaveData(SetCollection<CSet> collection)
        {
            using(StreamWriter writer = new StreamWriter(SetsData_FileName, false))
            {
                for (int i = 0; i < collection.Count; i++)
                    writer.WriteLine(collection[i].ElementString);
            }//end using
        }//SaveData
        private static void LoadData(SetCollection<CSet> collection)
        {
            if (File.Exists(SetsData_FileName))
            {
                string[] sets = File.ReadAllText(SetsData_FileName).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sets.Length; i++)
                    collection.Add(new CSet(sets[i]));
                if (collection.Count <= 0)
                    TestData(collection);
                return;
            }//if the file exist


            TestData(collection);
        }//LoadData
        private static void DisplayWelcomeScreen()
        {
            Console.WriteLine("\tSIMPLE SET APPLICATION FOR QUICK ANSWERS");
            Console.WriteLine("\t==========================================\n");
            Console.WriteLine("\n\tAre you feeling lazy to work through sets problems.?");
            Console.WriteLine("\tIf so then you are in luck because this application will be handy for you.");
            Console.WriteLine("\n\tThis application is going to make your life easier by doing some of the basic set operations that can take some time to complete in seconds.");
            Console.WriteLine("\tRecieve your assignment now, and submit it in the next 5min with the help of this app(LOL).");

            Console.WriteLine("\n\n\tWhat to expect.");
            Console.WriteLine("\t===============\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t1. Saves your sets to prevent set entry everytime(Very handy).");
            Console.WriteLine("\n\t2. Be able to perform set operations for set X and set Y such as:");
            Console.WriteLine("\t\t: X - Y");
            Console.WriteLine("\t\t: Y - X");
            Console.WriteLine("\t\t: X OR Y");
            Console.WriteLine("\t\t: X AND Y");
            Console.WriteLine("\n\t3. Be able to also check if set X is a subset of Y or the other way around.");
            Console.WriteLine("\n\t4. Be able to also check if set X is an element of Y or the other way around.");
            Console.WriteLine("\n\t5. Gives you a very detailed information of the results above.");
            

            Console.ResetColor();
            Console.WriteLine("\n\n\tHAVE FUN!!");
            Console.WriteLine("\n\nPress any key to continue......");
            Console.ReadKey();
        }//WelcomeScreen
        private static void AddingNewSetRules()
        {
            Console.Clear();
            Console.WriteLine("CREATING A NEW SET INSTRUCTIONS");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============================");
            Console.WriteLine("1. First things first you need to start with an opening brace(\"{\")\n");
            Console.WriteLine("2. The list your elements seperated by commas. E.g| {5,6,8,6,....\n");
            Console.WriteLine("3. Should you wish to add an element as a set then return to step 1 and follow the steps again.");
            Console.WriteLine("4. The last crusial step is to end with a clossing brace(\"}\")\n");
            Console.WriteLine("After this for steps you have now created your new set, congratulations!!.\n");
            Console.WriteLine("\n\n\tPLEASE DO NOT NEST SETS INSIDE AN ELEMEN. E.g. {5,6,8,5,{5,3,{5,8}}}\n\t\t\tThis is acceptable {5,6,8,{5,6,8},{2,5,6}} \n");
            Console.ResetColor();
        }//AddingNewSetRules
        private static void DisplayCollection(SetCollection<CSet> collection)
        {
            Console.Clear();
            Console.WriteLine("\tCURRENTLY SAVED SETS");
            Console.WriteLine("\t====================");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            char A = 'A';
            foreach (CSet item in collection)
            {
                Console.WriteLine($"\t{A.ToString()} : {item.ElementString.PadRight(30)} |{A}| = {item.Cardinality}");
                A++;
            }//end for each
            Console.ResetColor();
        }//Display

        private static void DisplayOperation(CSet X, CSet Y, CSet Output, OperationType operation)
        {
            Console.WriteLine("\tTHE OUTPUT RESULTS OF THE OPERATION");
            Console.WriteLine("\t===================================\n");

            Console.ForegroundColor = ConsoleColor.Blue;
            if (operation == OperationType.X_And_Y || operation == OperationType.X_OR_Y || operation == OperationType.X_Minus_Y)
            {
                Console.WriteLine($"\tX : {X.ElementString.PadRight(30)} |X|={X.Cardinality}");
                Console.WriteLine($"\tY : {Y.ElementString.PadRight(30)} |Y|={Y.Cardinality}");
            }//if 
            else
            {
                Console.WriteLine($"\tY : {Y.ElementString.PadRight(30)} |Y|={Y.Cardinality}");
                Console.WriteLine($"\tX : {X.ElementString.PadRight(30)} |X|={X.Cardinality}");
            }//else

            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Green;
            string s = (operation == OperationType.X_And_Y) ? "X AND Y" :
                       (operation == OperationType.X_Minus_Y) ? "X - Y" :
                       (operation == OperationType.Y_Minus_X) ? "Y - X" : "X OR Y";
            Console.WriteLine($"\t{s} : {Output.ElementString.PadRight(30)} |{s}|= {Output.Cardinality }");
            Console.ResetColor();
        }//DisplayOperation
        private static void DisplayIsSubset(CSet X, CSet Y,SetType type)
        {
            Console.WriteLine("\tTHE OUTPUT RESULTS OF THE OPERATION");
            Console.WriteLine("\t===================================\n");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine($"\tX : {X.ElementString.PadRight(30)} |X|={X.Cardinality}");
            Console.WriteLine($"\tY : {Y.ElementString.PadRight(30)} |Y|={Y.Cardinality}");

            Console.WriteLine();
            if(type == SetType.IsNotSubSet)
            {
                CSet output = X - Y;
                string sElements = output.ElementString.Remove(0, 1);
                sElements = sElements.Remove(sElements.Length - 1);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\tSet X is not a subset of Set Y.\n" +
                                  $"\tThe element(s) {sElements} is an element of set X but not an element of set Y.");
            }
            if(type == SetType.IsSubSet)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tSet X is a subset of Set Y.\n\tAll elements in set X are also in set Y.");
            }
            if(type == SetType.ProperSet)
            {
                CSet output = Y - X;
                string sElements = output.ElementString.Remove(0, 1);
                sElements = sElements.Remove(sElements.Length - 1);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\tSet X is a PROPER SUBSET of Set Y.\n\tAll elements in the set X are also present is set Y." +
                                  $"\n\tBut not all elements in set Y are in set X, e.g. The following element(s) are not in X.\n\t{sElements}");
            }
            if(type == SetType.SameSet)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\tSet X is equal/Identical to Set Y");
            }
            if(type == SetType.IsNotSubSetGreater)
            {
                CSet output = X - Y;
                string sElements = output.ElementString.Remove(0, 1);
                sElements = sElements.Remove(sElements.Length - 1);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\tSet X has a cardinality that is greater than set Y, with this we know that set X cannot be a subset of Y.");
                Console.WriteLine($"\tA formal reasoning will be that the element(s) {sElements} are present in X but are not in Y.");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\tTry doing the other way around and see the results.");
            }
            Console.ResetColor();
        }//DisplayIsSubset
        private static void DisplayIsElementOf(CSet X, CSet Y, bool isElement)
        {
            Console.WriteLine("\tTHE OUTPUT RESULTS FOR CHECKING ELEMENT");
            Console.WriteLine("\t===================================\n");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine($"\tX : {X.ElementString.PadRight(30)} |X|={X.Cardinality}");
            Console.WriteLine($"\tY : {Y.ElementString.PadRight(30)} |Y|={Y.Cardinality}");
            Console.WriteLine();
            if (isElement)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tX is indeed an element in Y");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tX is not an element in Y");
            }
            Console.ResetColor();
        }//DisplayIsElementOf

        private static void DisplayError(string errorMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }//DisplayErro
    }////Partial CClaient Class
    #region Window layout
    //Partial class to assist with layout and colours of the console window
    public partial class CClient
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        public static void SetWindow(int w, int h)
        {
            Console.Clear();
            IntPtr ptr = GetConsoleWindow();
            MoveWindow(ptr, (1920 - w) / 2, (1080 - h) / 2, w, h, true);
        } //SetWindow
    } //class Client
    #endregion
}//namespace