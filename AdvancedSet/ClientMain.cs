using System;
using SetLibrary;
using SetLibrary.Generic;
using SetLibrary.Collections;
namespace AdvancedSet
{
    public partial class Client
    {
        //This client will be for handling user inputs.
        private static SetExtractionSettings<int> settings = new SetExtractionSettings<int>(",");

        static void Main(string[] args)
        {
            //Setting up the back ground
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            //Create the set collection
            ISetCollection<int> collection = new SetCollection<int>();
            AddTestData(collection);

            //Call the menu
            Menu(collection);
            //DisplaySets(collection);
            //AddNewSet(collection);
            //DisplaySets(collection);
        }//Main
        
        public static void Menu(ISetCollection<int> sets)
        {
            //Option
            string  option;
            do
            {
                //Display menu here
                Console.Clear();
                Console.WriteLine("\tSet Application");
                Console.WriteLine("\t===============");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\tCurrent sets : ");
                Console.WriteLine();
                DisplaySets(sets);
                //Display the current sets
                DisplayMenuOptions();
                Console.WriteLine();
                Console.Write("\tOption : ");
                option = Console.ReadLine().ToUpper();

                bool success = int.TryParse(option, out int menuOption);
                if (!success && option != "X")
                    continue;
                switch (menuOption)
                {
                    case 1: AddNewSet(sets); break; 
                    case 2:RemoveSet(sets); break; 
                    case 3: CheckForSubsets(sets); break;
                    case 4: CheckForElement(sets); break; 
                    case 5: PerformSetOperations(sets); break; 
                    case 6: PerformSetLaws(sets); break;
                    case 7: ClearAllSests(sets); break; 
                    case 8:ResetNaming(sets); break; 
                    case 9: LearnMoreAboutSets(); break;
                    default:break;
                }//end switch
            } while (option != "X");
        }//Menu

        private static void LearnMoreAboutSets()
        {
            Console.Clear();
            Console.WriteLine("\tLearning about sets.........");
            AnyKey();
        }//LearnMoreAboutSets

        private static void ResetNaming(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tResetting naming.........");
            AnyKey();
        }//ResetNaming

        private static void ClearAllSests(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tClearing All sets.........");
            AnyKey();
        }//ClearAllSests

        private static void PerformSetLaws(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tPerformaing set laws.........");
            AnyKey();
        }//PerformSetLaws

        private static void PerformSetOperations(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tPerformaing set operations.........");
            AnyKey();
        }//PerformSetOperations

        private static void CheckForElement(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tChecking for elements.........");
            AnyKey();
        }//CheckForElement

        private static void CheckForSubsets(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tChecking for subsets.........");
            AnyKey();
        }//CheckForSubsets

        private static void RemoveSet(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tRemoving sets ......");
            AnyKey();
        }//RemoveSet

        private static void AddNewSet(ISetCollection<int> sets)
        {
            //Variable for adding and looping
            bool shouldExit = false;
            bool should_add = false;

            //Variables for displaying
            string setString = "";
            string error = "";
            //Set variable
            ICSet<int> newset = default;
            do
            {
                //Display the new set header
                DisplayNewSetHeader(sets);
                //If the user entered an incorrect error
                if (error == "")
                    Console.WriteLine();
                else
                    DisplayError(error);

                //Read the set string by passing the old string for modification
                setString = ReadSetString("\tSet string : ", setString);
                if(setString == "")
                {
                    shouldExit = true;
                    should_add = false;
                }
                else
                {
                    try
                    {
                        //Here try to create the set, if braces are not matching or there's some sort of conversion error, catch it.
                        newset = new GenericSet<int>(setString,settings);
                        //Set the flag to exit
                        shouldExit = true;
                        should_add = true;
                    }
                    catch
                    {
                        //set the error flag
                        error = "Braces are not matching or you have used none-integer values!!\n\tIf your previous attempt does not appear, press the \"up\" key on the keyboard ";
                    }//end catch
                }
            } while (!shouldExit);

            if (should_add)
            {
                //sets.Add(newset);
                Console.Clear();
                Console.WriteLine("\tThe Follwowing set will be added : ");
                Console.WriteLine("\n\tOriginal expression : {0}", setString);
                Console.WriteLine("\tEvaluated expression : {0}", newset);

                Console.WriteLine();
                Console.Write("\tDo you wish to add(Y = Yes/N = No) : ");
                char option = Console.ReadLine().ToUpper()[0];
                if (option == 'Y')
                {
                    sets.Add(newset);
                    Console.WriteLine("\n\tSet was added successfully.");
                }
                else
                {
                    Console.WriteLine("\n\tSet was not added.");
                }
            }
            AnyKey();
        }//AddNewSet
        
    }//class
}//namespace
