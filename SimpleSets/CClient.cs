using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
//git remote add origin https://github.com/Shisui-Pho/Custom_Sets.git
namespace SimpleSets
{
    //This partial class will handle user inputs
    public partial class CClient
    {
        static void Main(string[] args)
        {
            //Set the window size
            SetWindow(1250, 700);
            DisplayWelcomeScreen();

            //Shrink the window size
            SetWindow(800, 600);
            SetCollection<CSet> coll = new SetCollection<CSet>();
            LoadData(coll);

            //Add set if there are currently nno sets
            if (coll.Count == 0)
                AddNewSet(coll);
            //DisplayWelcomeScreen();
            Menu(coll);

            //Save the data before exiting
            SaveData(coll);
        }//Main method
        private static void Menu(SetCollection<CSet> collection)
        {
            char cOption;
            do
            {
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine("MENU");
                Console.WriteLine("=====\n\n");
                DisplayCollection(collection);
                Console.WriteLine("\n\n\tPlease select an option below of your choice\n\n");
                Console.WriteLine("\t1. Perform set operations." +
                                "\n\t2. Check for subsets." +
                                "\n\t3. Check for Element." +
                                "\n\t4. Add new set of your choice." +
                                "\n\t5. Remove a set." +
                                "\n\t6. Clear all sets." +
                                "\n\t7. Learn more about sets." +
                                "\n\tX. Exit(Your current sets will be saved).");
                Console.Write("\n\n\tOption : ");
                string sOp = Console.ReadLine();
                sOp = (string.IsNullOrEmpty(sOp) || string.IsNullOrWhiteSpace(sOp)) ? "P" : sOp;
                cOption = sOp[0].ToString().ToUpper()[0];
                switch (cOption)
                {
                    case '1': PerFormSetOperations(collection);
                        break;
                    case '2': CheckForeSubSets(collection);
                        break;
                    case '3': CheckForElement(collection);
                        break;
                    case '4': AddNewSet(collection);
                        break;
                    case '5':
                        RemoveSet(collection);
                        break;
                    case '6':
                        CleaAllSets(collection);
                        break;
                    case '7':
                        LearnMoreAboutSets();
                        break;
                    case 'X':
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("\n\n\tYou have chosen an option to Exit the application" +
                                        "\n\n\tPress R if you wish to return to the main menu or anykey to contine with the exit...");
                        char c = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
                        if (c == 'R')
                            cOption = c;//Will return to the main menu.
                        break;
                    default:
                        DisplayError("Oops! You have chossen an incorrect option.");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("\n\nPress any key to return to the main menu...");
                        Console.ReadKey();
                        break;
                }//Switch
            } while (cOption != 'X');
        }//Menu

        private static void CleaAllSets(SetCollection<CSet> collection)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\tWARNING: You are about to Clear everything!!\n\tOnce cleared you cannot reverse it.");
            Console.ResetColor();
            Console.Write("\n\tAre you sure about this(Y= Yes| N = No)? : ");
            char opt = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
            if (opt == 'Y')
                collection.ClearAll();
        }//CleaAllSets

        private static void RemoveSet(SetCollection<CSet> collection)
        {
            DisplayCollection(collection);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\tWARNING: You are about to remove a set from the set collection!!\n\tOnce removed you cannot retrieve it.");
            Console.WriteLine("\n\tIf you wish to cancel press \"0\"");
            Console.ResetColor();

            Console.Write("\n\n\tSet to be removed(choose from the above options) : ");
            char sSet = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
            if (sSet == '0')
                return;
            int i = (int)sSet - 65;
            if (i < 0 || i >= collection.Count)
                return;
            collection.RemoveAt(i);
        }//RemoveSet

        private static void LearnMoreAboutSets()
        {
            if (File.Exists(SetsInfo_FileName1))
            {
                Process.Start(Path.Combine(Directory.GetCurrentDirectory(), SetsInfo_FileName1));
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("A word document will open up shortly.");
            }
            else if (File.Exists(SetsInfo_FileName2))
            {
                Process.Start(Path.Combine(Directory.GetCurrentDirectory(), SetsInfo_FileName2));
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("A pdf document will open up shortly.");
            }
            else
            {
                DisplayError($"The file \"{SetsInfo_FileName1}\" was removed from the root destination.");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\n\nPress any key to return to the main menu...");
            Console.ResetColor();
            Console.ReadKey();
        }//LearnMoreAboutSets
        private static void AddNewSet(SetCollection<CSet> collection)
        {
            DisplayNewSetRules();
            CSet set = Crashed("\n\n\tElement string : ");
            collection.Add(set);


            Console.WriteLine("\n\n\tPress R to add another set or anykey to return to the main menu...");
            char s = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
            if (s == 'R')
                AddNewSet(collection);
        }//AddNewSet
        private static CSet Crashed(string sPrompt, string sTryed = "")
        {
            Console.Write(sPrompt);
            try { SendKeys.SendWait(sTryed); }
            catch { SendKeys.SendWait(""); }
            string elementString = Console.ReadLine();
            try
            {
                CSet set = new CSet(elementString);
                return set;
            }
            catch(Exception ex)
            {
                if(ex is ArgumentException)
                {
                    DisplayNewSetRules();
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (ex.Message == "Did not follow steps")
                        Console.WriteLine("\n\n\tPlease follow the above steps to create a set!!!");
                    else
                        Console.WriteLine("\n\n"+ex.Message);
                    Console.ResetColor();
                    string d = "";
                    //For the sendKeys
                    foreach (char item in elementString)
                    {
                        if (item == '{')
                            d += "{{}";
                        else if (item == '}')
                            d += "{}}";
                        else
                            d += item;
                    }//end foreach
                    return Crashed(sPrompt, d);
                }//end try
                else
                {
                    DisplayError("Please follow the guideline!!!");
                    Console.WriteLine("\n\n\nPress any key to retry");
                    Console.ReadKey();
                    DisplayNewSetRules();
                    return Crashed(sPrompt);
                }//end else
            }//exception
        }//Crashed
        private static void CheckForElement(SetCollection<CSet> collection)
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("\tCHECKING FOR ELEMENT");
            Console.WriteLine("\t===================");
            DisplayCollection(collection);
            Console.WriteLine();
            Console.WriteLine("\tPlease chose the operation below ");
            Console.WriteLine("\n\t1. X is element Of Y.\n\t2. Y is element of X.");
            Console.WriteLine();
            Console.Write("\tOption : ");
            string sRead = Console.ReadLine();

            if(sRead == "" || string.IsNullOrEmpty(sRead) || string.IsNullOrWhiteSpace(sRead))
            {
                DisplayError("Please choose an option.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n\nPress any key to try again...");
                Console.ReadKey();
                CheckForElement(collection);
            }
            char cOption = sRead[0];

            if (!(cOption == '1' || cOption == '2'))
            {
                DisplayError("Oops! You have chossen an incorrect option.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n\nPress any key to try again...");
                Console.ReadKey();
                CheckForElement(collection);
            }//the option is invalid

            if (cOption == '1')
            {
                GetInputOfChoice("\tChecking if X is an element of Y. (chose X and Y from the above options)", collection, out int iX, out int iY);
                Console.Clear();
                bool isElement = collection[iX].IsElementOf(collection[iY]);
                Console.WriteLine("\n\n");
                DisplayIsElementOf(collection[iX], collection[iY], isElement);
            }
            else
            {
                GetInputOfChoice("\tChecking if Y is an element of X. (chose X and Y from the above options)", collection, out int iX, out int iY);
                Console.Clear();
                bool isElement = collection[iY].IsElementOf(collection[iX]);
                Console.WriteLine("\n\n");
                DisplayIsElementOf(collection[iY], collection[iX], isElement);
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\n\n\tPress R to repeat set operation or anykey to return to the main menu...");
            char s = Console.ReadKey().KeyChar.ToString().ToUpper()[0];

            if (s == 'R')
                CheckForElement(collection);

        }//CheckForeElement
        private static void CheckForeSubSets(SetCollection<CSet> collection)
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("\tCHECKING FOR SUBSET");
            Console.WriteLine("===================");
            DisplayCollection(collection);
            Console.WriteLine();
            Console.WriteLine("\tPlease chose the operation below ");
            Console.WriteLine("\n\t1. X Subset Of Y.\n\t2. Y Subset of X.");
            Console.WriteLine();
            Console.Write("\tOption : ");
            string sRead = Console.ReadLine();
            char cOption = sRead[0];

            if (sRead == "" || string.IsNullOrEmpty(sRead) || string.IsNullOrWhiteSpace(sRead))
            {
                DisplayError("Please make selection.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n\nPress any key to try again....");
                Console.ReadKey();
                CheckForeSubSets(collection);
            }

            if (!(cOption == '1' || cOption == '2'))
            {
                DisplayError("Oops! You have chossen an incorrect option.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n\nPress any key to try again...");
                Console.ReadKey();
                CheckForeSubSets(collection);
            }//the option is invalid

            if(cOption == '1')
            {
                GetInputOfChoice("\tChecking if X is a Subset of Y. (chose X and Y from the above options)", collection, out int iX, out int iY);
                Console.Clear();
                SetType type = collection[iX].IsSubsetOf(collection[iY]);
                DisplayIsSubset(collection[iX], collection[iY], type);
            }
            else
            {
                GetInputOfChoice("\tChecking if Y is a Subset of X. (chose X and Y from the above options)", collection, out int iX, out int iY);
                SetType type = collection[iY].IsSubsetOf(collection[iX]);
                Console.Clear();
                DisplayIsSubset(collection[iY], collection[iX], type);
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\n\n\tPress R to repeat set operation or anykey to return to the main menu.....");
            char s = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
            if (s == 'R')
                CheckForeSubSets(collection);
        }//CheckForeSubSets
        private static void PerFormSetOperations(SetCollection<CSet> collection)
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("\tPERFORMING SET OPERATION");
            Console.WriteLine("\t========================");
            DisplayCollection(collection);
            Console.WriteLine();
            Console.WriteLine("\tPlease chose the operation to be performed: ");
            Console.WriteLine("\n\t1. X-Y.\n\t2. Y-X.\n\t3. X OR Y\n\t4. X AND Y");
            Console.WriteLine();
            Console.Write("\tOption : ");
            string sRead = Console.ReadLine();

            if(string.IsNullOrEmpty(sRead) || string.IsNullOrWhiteSpace(sRead))
            {
                DisplayError("Oops! You have chossen an incorrect option.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n\nPress any key to try again...");
                Console.ReadKey();
                PerFormSetOperations(collection);
            }
            char cOption = sRead[0];

            if(!(cOption == '1' || cOption == '2' || cOption =='3' || cOption == '4'))
            {
                DisplayError("Oops! You have chossen an incorrect option.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n\nPress any key to try again...");
                Console.ReadKey();
                PerFormSetOperations(collection);
            }//the option is invalid
            CSet outPut;
            switch (cOption)
            {
                case '1':
                    GetInputOfChoice("\tPlease select the sets for X - Y (chose from letters above)", collection, out int iX, out int iY);
                    outPut = collection[iX] - collection[iY];
                    Console.Clear();
                    DisplayOperation(collection[iX], collection[iY], outPut, OperationType.X_Minus_Y);
                    break;
                case '2':
                    GetInputOfChoice("\tPlease select the sets for Y - X (chose from letters above)", collection, out iX, out iY);
                    outPut = collection[iY] - collection[iX];
                    Console.Clear();
                    DisplayOperation(collection[iX], collection[iY], outPut, OperationType.Y_Minus_X);
                    break;
                case '3':
                    GetInputOfChoice("\tPlease select the sets for X OR Y (chose from letters above)", collection, out iX, out iY);
                    outPut = collection[iY] | collection[iX];
                    Console.Clear();
                    DisplayOperation(collection[iX], collection[iY], outPut, OperationType.X_OR_Y);
                    break;
                case '4':
                    GetInputOfChoice("\tPlease select the sets for X AND Y (chose from letters above)", collection, out iX, out iY);
                    outPut = collection[iY] & collection[iX];
                    Console.Clear();
                    DisplayOperation(collection[iX], collection[iY], outPut, OperationType.X_And_Y);
                    break;
            }//end switch

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\n\n\tPress R to repeat set operation or anykey to return to the main menu...");
            char s = Console.ReadKey().KeyChar.ToString().ToUpper()[0];

            if (s == 'R')
                 PerFormSetOperations(collection);

        }//PerFormSetOperations
        private static void GetInputOfChoice(string prompt, SetCollection<CSet> collection, out int _iX, out int _iY)
        {
            _iX = ReadLineOption(prompt, collection, "Set X");
            _iY = ReadLineOption(prompt, collection, "Set Y");
        }//OperationInput
        private static int ReadLineOption(string prompt, SetCollection<CSet> collection, string toGet)
        {
            do
            {
                DisplayCollection(collection);
                Console.WriteLine("\n\n\n");
                Console.WriteLine(prompt);
                Console.Write($"\n\t{toGet} : ");
                string X = Console.ReadLine();
                X = (string.IsNullOrEmpty(X) || string.IsNullOrWhiteSpace(X)) ? "4" : X;
                int iX = (int)X[0] - 65;
                //a --- 32(97)
                //z --- 57(122)
                if(iX >= 32 )
                {
                    DisplayError("Oops you have inserted the lowercase letter (Sets are always denoted by uppercase letters).");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\n\nPress any key to try again....");
                    Console.ReadKey();
                    
                }
                else if (iX < 0 || iX >= collection.Count)
                {
                    DisplayError("Oops you have chosen an incorrect option.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\n\nPress any key to try again...");
                    Console.ReadKey();
                }else
                {
                    return iX;
                }
            } while (true);
        }//ReadLineOption
    }//Partial CClaient Class
}//namespace