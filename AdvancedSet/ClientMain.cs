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

                //Display the current sets
                DisplaySets(sets);

                //Display menu options
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
                    case 3: ModifySet(sets); break;
                    case 4: CheckForSubsets(sets); break;
                    case 5: CheckForElement(sets); break; 
                    case 6: PerformSetOperations(sets); break; 
                    case 7: PerformSetLaws(sets); break;
                    case 8: ClearAllSests(sets); break; 
                    case 9:ResetNaming(sets); break; 
                    case 10: LearnMoreAboutSets(); break;
                    default:break;
                }//end switch
            } while (option != "X");
        }//Menu

        private static void ModifySet(ISetCollection<int> sets)
        {
            ICSet<int> set;
            string name = "";
            string error = "";
            do
            {
                Console.Clear();
                Console.WriteLine("\tModifying a set");
                Console.WriteLine("\t================");
                Console.WriteLine();
                DisplaySets(sets);
                Console.WriteLine("\tPress \"-1\" to exit.");
                Console.WriteLine();
                if (error != "")
                    DisplayError(error);
                set = GetAndValidateInputForSetName("Set to modify", sets,ref name);
                if (name == "-1")
                    break;
                if (set == default)
                    error = "Invalid set name";
                else
                {
                    //Modify the set.
                    ModifySet(set, name);
                }//end else
            } while (true);
            AnyKey();
        }//ModifySet

        private static void ModifySet(ICSet<int> set,string name)
        {
            bool success;
            int elem;
            do
            {
                Console.Clear();
                Console.WriteLine($"\tModifying the set \"{name}\"");
                Console.WriteLine();
                int count = set.ElementString.Length;
                Console.WriteLine($"\t{set.ElementString.PadRight(count + 8)} |{set.Cardinality}|");
                Console.WriteLine();
                Console.WriteLine("\t1. Add an element.");
                Console.WriteLine("\t2. Remove an element.");
                Console.WriteLine("\tX. Cancel");
                Console.WriteLine();
                Console.Write("\tOption : ");
                string input = Console.ReadLine().ToUpper();
                Console.WriteLine();
                Console.WriteLine();
                if (input == "X")
                    break;
                switch (input)
                {
                    case "2":
                        Console.Write("\tElement to remove : ");
                        string element = Console.ReadLine();
                        if (element == "X")
                            break;
                        if(element.Contains("{") || element.Contains("}"))
                        {
                            var tree = GetElement(element);
                            if (!set.Contains(tree) || tree == default)
                            {
                                Console.WriteLine($"\tThe element \"{((tree == null) ? element : tree.ToString())}\" was not found in the set.");
                                Console.WriteLine("\tPress any key to try again....");
                                Console.ReadKey();
                                continue;
                            }//end if tree does not contain
                            Console.WriteLine();
                            Console.Write("\t Remove the element (Y = Yes/ N = No) : ");
                            string confirm = Console.ReadKey().KeyChar.ToString().ToUpper();
                            if (confirm == "Y")
                                set.RemoveElement(tree);//Remove the tree
                        }
                        else
                        {
                            success = int.TryParse(element, out elem);
                            if (!set.Contains(elem) || !success)
                            {
                                Console.WriteLine($"\tThe element \"{element}\" was not found in the set.");
                                Console.WriteLine("\tPress any key to try again....");
                                Console.ReadKey();
                                continue;
                            }//end if tree does not contain
                            Console.WriteLine();
                            Console.Write("\t Remove the element (Y = Yes/ N = No) : ");
                            string confirm = Console.ReadKey().KeyChar.ToString().ToUpper();
                            if (confirm == "Y")
                                set.RemoveElement(elem);//Remove the tree
                        }
                        break;
                    case "1":
                        Console.Write("\tElement to add : ");
                        element = Console.ReadLine();
                        if (element == "X")
                            break;
                        if (element.Contains("}")|| element.Contains("}"))
                        {
                            var tree = GetElement(element);
                            if(tree != null)
                                set.AddElement(tree);
                            continue;
                        }

                        success = int.TryParse(element, out elem);
                        if (success)
                            set.AddElement(elem);
                        break;
                    default:
                        break;
                }
            } while (true);
        }//EditingSet
        private static ISetTree<int> GetElement(string element)
        {
            if (!BracesEvaluation.AreBracesCorrect(element))
            {
                DisplayError("Braces are not matching.");
                Console.Write("\tPress any key to try again...");
                Console.ReadKey();
                return default;
            }//Extract the tree
            try
            {
                return SetExtraction.Extract(element, settings);
            }
            catch
            {
                DisplayError("Braces are not matching.");
                Console.Write("\tPress any key to try again...");
                Console.ReadKey();
                return default;
            }
        }//GetElement
        private static void LearnMoreAboutSets()
        {
            Console.Clear();
            Console.WriteLine("\tLearning about sets.........");
            AnyKey();
        }//LearnMoreAboutSets

        private static void ResetNaming(ISetCollection<int> sets)
        {
            bool success;
            do
            {
                Console.Clear();
                Console.WriteLine("\tResetting naming");
                Console.WriteLine("\t================");
                //DisplayResetNamingInfo();
                DisplaSetsWarning(SetWarting.Reset_Naming);
                //Display the current sets
                DisplaySets(sets);

                //Display menu options
                Console.WriteLine("\tY. Reset naming.");
                Console.WriteLine("\tX. Cancel");
                Console.WriteLine();
                Console.Write("\tOption : ");
                string sChoice = Console.ReadLine().ToUpper();
                sChoice = sChoice.Replace(" ", "");
                if (sChoice.Length <= 0)
                    sChoice = "D";
                success = sChoice[0] == 'Y';
                if (success || sChoice == "X")
                    break;
            } while (true);
            if (success)
            {
                Console.Clear();
                Console.WriteLine("\tNaming has been reset:");
                Console.WriteLine();
                //Display the old set with old naming
                DisplaySets(sets, "Old sets naming");

                //Reset the naming
                sets.Reset();

                //Display the new set with new naming
                DisplaySets(sets, "New sets naming");
            }//end if
            else
            {
                Console.WriteLine("\tNaming has not been reset.");
            }//end else
            AnyKey();
        }//ResetNaming

        private static void ClearAllSests(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tClearing all sets");
            Console.WriteLine("\t=================");
            //Display warning
            DisplaSetsWarning(SetWarting.Clear);
            Console.WriteLine("\tY. To continue");
            Console.WriteLine("\tX. To Cancel");
            string option = Console.ReadLine().ToUpper();
            if (option.Length > 1)//Get the first character
                option = option[0].ToString();

            if(option == "Y")
            {
                Console.WriteLine();
                Console.WriteLine("\tSet has been cleared.");
                sets.Clear();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tOperation has been cancelled.");
            }
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
            do
            {
                GetInputSetCollection(sets, out ICSet<int> setA, out ICSet<int> setB, out Operation opr);
                if (opr == Operation.Continue)
                {
                    bool isSubset = setA.IsSubSetOf(setB, out SetType type);
                    DisplayIsSubsetOfOutCome(isSubset, setA, setB, type);
                }
                else
                    break;
                Console.WriteLine();
                Console.Write("\tPress 1 to continue......");
                _ = int.TryParse(Console.ReadLine(), out int val);
                if (val != 1)
                    break;
            } while (true);
            AnyKey();
        }//CheckForSubsets
        private static ICSet<int> GetAndValidateInputForSetName(string header, ISetCollection<int> sets, ref string name)
        {
            Console.Write($"\t{header} : ");
            name = Console.ReadLine();
            name = name.Replace(" ", "");//Remove spaces
            return sets.FindSetByName(name);
        }//GetAndValidateInput
        private static void GetInputSetCollection(ISetCollection<int> sets, out ICSet<int> setA, out ICSet<int> setB, out Operation operation)
        {
            string setAName = "";
            string setBName = "";
            setA = setB = null;
            operation = Operation.Cancelled;
            do
            {
                Console.Clear();
                Console.WriteLine("\tChecking for subsets");
                Console.WriteLine("\t====================");

                //Display information
                DisplayCheckingForSubSetsInformation();

                //Display the sets

                DisplaySets(sets);

                Console.WriteLine("\tWe will use setA to represent the set which is supposed to be a subset of setB.");
                Console.WriteLine("\tChose name from the above set(s). The name has to match one of the name above");
                Console.WriteLine("\tPress \"-1\" to exit.");
                Console.WriteLine();

                if (setAName != "")
                {
                    //Display the name of setA
                    Console.WriteLine("\t\tsetA = {0}", setAName);
                    Console.WriteLine();
                    setB = GetAndValidateInputForSetName("\tsetB", sets, ref setBName);
                    if (setB == default && setBName != "-1")
                        setBName = "";
                    else
                        break;//Here means that the name is accurate or the user decided to quit the operation.
                }
                else
                {
                    setA = GetAndValidateInputForSetName("\tSetA", sets, ref setAName);
                    if (setAName == "-1")
                        break;//Here the user decided to end the operation
                    if (setA == default && setAName != "-1")
                        setAName = "";
                }//end if
            } while (true);
            if(setA != null && setB != null)
                operation = Operation.Continue;

        }//GettingInput
        private static void RemoveSet(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\tRemoving sets");
            Console.WriteLine("\t=============");
            DisplaSetsWarning(SetWarting.Remove_Set);
            DisplaySets(sets);
            Console.WriteLine("\tEnter \"-1\" to cancel");
            Console.Write("\tName of the set to remove : ");
            string setName = Console.ReadLine();
            //Remove spaces in the string
            setName = setName.Replace(" ", "");

            bool can_remove = false;
            Console.WriteLine();
            Console.WriteLine();
            if (setName.Length > 0 && setName != "-1")
                can_remove = sets.Contains(setName);
            if(setName == "-1")
            {
                Console.WriteLine("\tYou have chosen to cancel operation.");
            }//if cancelled
            else if (!can_remove)
            {
                Console.WriteLine($"\tThe name of the set \"{setName}\" does not exist in the current collection.");
                Console.WriteLine($"\tChose from the set names and makes sure the name is \"upper-case\" as part of convention.");
                Console.WriteLine();
            }//end else did not find the set.
            else
            {
                //Remove the set
                RemoveSet(sets, setName);
            }//end else we remove
            AnyKey();
        }//RemoveSet
        private static void RemoveSet(ISetCollection<int> sets,string setName)
        {
            Console.Clear();
            Console.WriteLine("\tCONFIRMATION");
            Console.WriteLine("\t============");
            Console.WriteLine();
            ICSet<int> set = sets.FindSetByName(setName);
            Console.WriteLine($"\tThe set \"{setName}\" will be removed.");
            Console.WriteLine($"\tSet : {set.ElementString}");
            Console.WriteLine();
            Console.WriteLine("\tY. To remove");
            Console.WriteLine("\tX. To cancel");
            Console.WriteLine();
            Console.Write("\tOption : ");
            string option = Console.ReadLine().ToUpper();
            if (option.Length > 0 && option[0] == 'Y')
            {
                sets.Remove(set);//Remove the set from the collection
                Console.WriteLine();
                Console.WriteLine("\tSet \"{0}\" has been removed.", setName);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tOperation cancelled.");
            }
        }//Remove set
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
