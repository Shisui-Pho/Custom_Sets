using System;
using SetLibrary;
using SetLibrary.Generic;
using SetLibrary.Collections;
using SetLibrary.Operations;
using System.Collections.Generic;

namespace AdvancedSet
{
    public partial class Client
    {
        #region Main method and menu
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

        #endregion Main method and menu

        #region Adding new set
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
                if (setString == "")
                {
                    shouldExit = true;
                    should_add = false;
                }
                else
                {
                    try
                    {
                        //Here try to create the set, if braces are not matching or there's some sort of conversion error, catch it.
                        newset = new GenericSet<int>(setString, settings);
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
        #endregion Adding new set

        #region Removing a set
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
            if (setName == "-1")
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
        private static void RemoveSet(ISetCollection<int> sets, string setName)
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

        #endregion Removing a set

        #region Modify set
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
                DisplayError("Only integers allowed.");
                AnyKey("Press any key to try again.....");
                return default;
            }
        }//GetElement
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
                                AnyKey("Press any key to try again........");
                                continue;
                            }//end if tree does not contain
                            Console.WriteLine();
                            Console.Write("\tRemove the element (Y = Yes/ N = No) : ");
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
                                AnyKey("Press any key to try again....");
                                continue;
                            }//end if tree does not contain
                            Console.WriteLine();
                            Console.Write("\tRemove the element (Y = Yes/ N = No) : ");
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
        #endregion Modify set

        #region Check for subset
        private static void CheckForSubsets(ISetCollection<int> sets)
        {
            do
            {
                var outputsets = GetInputSetCollection(InputTypeFunction.subsets,sets,out Operation opr, DisplayCheckingForSubSetsInformation);
                
                if (opr == Operation.Continue)
                {
                    var setA = outputsets[0];
                    var setB = outputsets[1];
                    bool isSubset = setA.IsSubSetOf(setB, out SetType type);
                    DisplayIsSubsetOfOutCome(isSubset, setA, setB, type);
                }
                else
                    break;
                AnyKey("Press any key to continue....");
            } while (true);
            AnyKey();
        }//CheckForSubsets
        #endregion Check for subset

        #region Getting Set Inputs
        private static ICSet<int> GetAndValidateInputForSetName(string header, ISetCollection<int> sets, ref string name)
        {
            name = string.Empty;
            Console.Write($"\t{header} : ");
            name = Console.ReadLine();
            name = name.Replace(" ", "");//Remove spaces
            return sets.FindSetByName(name);
        }//GetAndValidateInput
        //private static List<ICSet<int>> GetInputSetCollection(InputTypeFunction type,ISetCollection<int> sets, out ICSet<int> setA, out ICSet<int> setB, out Operation operation, delDisplayInfo_Func info = null, int count_loop = 2)
        //{
        //    string setAName = "";
        //    string setBName = "";
             
        //    setA = setB = null;
        //    operation = Operation.Cancelled;
        //    do
        //    {
        //        Console.Clear();
        //        Console.WriteLine($"\tChecking for {type.ToString()}");
        //        Console.WriteLine("\t====================");

        //        //Display information
        //        info?.Invoke();

        //        //Display the sets

        //        DisplaySets(sets);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        switch (type)
        //        {
        //            case InputTypeFunction.subsets:
        //                Console.WriteLine($"\tWe will use setA to represent the set which is supposed to be a subset of setB.");
        //                break;
        //            case InputTypeFunction.elements:
        //                Console.WriteLine("\tWe will use setA as the container set and setB as the element set.");
        //                break;
        //            case InputTypeFunction.operations:
        //                Console.WriteLine("\tWe will use setA for the set on the left and setB for the set at the right");
        //                break;
        //            case InputTypeFunction.Properties | InputTypeFunction.set_laws:
        //                Console.WriteLine("\tWe will user setA \"A\" and setB for \"B\", just like in the explanation");
        //                break;
        //        }//end switch
        //        Console.WriteLine("\tChose name from the above set(s). The name has to match one of the name above");
        //        Console.ForegroundColor = ConsoleColor.Black;
        //        Console.WriteLine();
        //        Console.WriteLine("\tPress \"-1\" to exit.");
        //        Console.WriteLine();

        //        if (setAName != "")
        //        {
        //            //Display the name of setA
        //            Console.WriteLine("\t\tsetA = {0}", setAName);
        //            Console.WriteLine();
        //            setB = GetAndValidateInputForSetName("\tsetB", sets, ref setBName);
        //            if (setB == default && setBName != "-1")
        //                setBName = "";
        //            else
        //                break;//Here means that the name is accurate or the user decided to quit the operation.
        //        }
        //        else
        //        {
        //            setA = GetAndValidateInputForSetName("\tSetA", sets, ref setAName);
        //            if (setAName == "-1")
        //                break;//Here the user decided to end the operation
        //            if (setA == default && setAName != "-1")
        //                setAName = "";
        //        }//end if
        //    } while (true);
        //    if (setA != null && setB != null)
        //        operation = Operation.Continue;

        //    return default;
        //}//GettingInput
        private static List<ICSet<int>> GetInputSetCollection(InputTypeFunction type, ISetCollection<int> sets,out Operation operation, delDisplayInfo_Func info = null, int numberOfSets = 2)
        {
            string setName = "";

            //Parallel arrays for the sets
            List<string> setNames = new List<string>();
            List<ICSet<int>> output_sets = new List<ICSet<int>>();

            //Assume not cancelled
            operation = Operation.Continue;
            do
            {
                //Stop the execution, it means we have read all the sets we need
                if (setNames.Count >= numberOfSets)
                {
                    operation = Operation.Continue;
                    break;
                }

                Console.Clear();
                Console.WriteLine($"\tChecking for {type.ToString()}");
                Console.WriteLine("\t====================");

                //Display information
                info?.Invoke();

                //Display the sets
                DisplaySets(sets);

                #region Prompt
                //Display special information
                Console.ForegroundColor = ConsoleColor.Green;
                switch (type)
                {
                    case InputTypeFunction.subsets:
                        Console.WriteLine($"\tWe will use setA to represent the set which is supposed to be a subset of setB.");
                        break;
                    case InputTypeFunction.elements:
                        Console.WriteLine("\tWe will use setA as the container set and setB as the element set.");
                        break;
                    case InputTypeFunction.operations:
                        Console.WriteLine("\tWe will use setA for the set on the left and setB for the set at the right");
                        break;
                    case InputTypeFunction.Properties | InputTypeFunction.set_laws:
                        Console.WriteLine("\tWe will user setA \"A\", setB for \"B\" and setC for \"C\", just like in the explanation.\n\tIf a universal set is required, it will be represented by setC.");
                        break;
                }//end switch
                Console.WriteLine("\tChose name from the above set(s). The name has to match one of the name above");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.WriteLine("\tPress \"-1\" to exit.");
                Console.WriteLine();
                //Display the current set and get the read the next set
                string next_entry = ListEntriesAndGetNextEntry(setNames);

                #endregion Prompt

                Console.WriteLine();
                var set = GetAndValidateInputForSetName($"\t{next_entry}", sets, ref setName);
                if (setName == "-1")//End the operation
                    break;
                if (set == default)//We did not find the prescribed set
                    continue;

                //Here we are good to go
                setNames.Add(setName);
                output_sets.Add(set);
            } while (true);

            if (output_sets.Count != numberOfSets)
                operation = Operation.Cancelled;

            return output_sets;
        }//GettingInput
        private static string ListEntriesAndGetNextEntry(List<string> setNames)
        {
            char _currentSet = 'A';//Starting entry
            string entry = "set" + _currentSet;
            foreach (var name in setNames)
            {
                Console.WriteLine($"\t\t{entry} = {name}");

                //Increament the entry
                _currentSet++;
                entry = "set" + _currentSet;
            }//en dforeach

            return entry;
        }//ListSets
        #endregion Getting set inputs

        #region Checking elments
        private static void CheckForElement(ISetCollection<int> sets)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("\tChecking for elements.");
                Console.WriteLine("\t======================");
                Console.WriteLine();
                Console.WriteLine("\t1. Check if a set is an element of another set.");
                Console.WriteLine("\t2. Check if some element is an element in an existing set.");
                Console.WriteLine("\tX. Cancel");
                Console.WriteLine();
                Console.Write("\tOption : ");
                string option = Console.ReadLine().ToUpper().Replace(" ", "");
                if (option.Length > 0 && option == "X")
                    break;
                switch (option)
                {
                    case "1":
                        CheckWithOtherSets(sets);
                        break;
                    case "2":
                        CheckWithElements(sets);
                        break;
                    default:
                        break;
                }//end switch
            } while (true);
            AnyKey();
        }//CheckForElement
        private static void CheckWithOtherSets(ISetCollection<int> sets)
        {
            do
            {
                var output_sets = GetInputSetCollection(InputTypeFunction.elements, sets, out Operation operation);
                Console.Clear();
                if (operation == Operation.Cancelled)
                    break;
                else
                {
                    var setA = output_sets[0];
                    var setB = output_sets[1];

                    Console.WriteLine("\tResults");
                    Console.WriteLine("\t=======");
                    Console.WriteLine();
                    Console.WriteLine("\tSetA : {0}", setA.ElementString);
                    Console.WriteLine("\tSetB : {0}", setB.ElementString);
                    Console.WriteLine();
                    Console.WriteLine("\tOutcome:");
                    Console.WriteLine();
                    //Check for that relation
                    if (setB.IsElementOf(setA))
                    {
                        Console.WriteLine("\tsetB is an element of setA.");
                    }//end if
                    else
                        Console.WriteLine("\tsetB is not an element of setA");

                    AnyKey("Press any key to continue.....");
                }//end if
            } while (true);
            AnyKey();
        }//CheckWithOtherSets
        private static void CheckWithElements(ISetCollection<int> sets)
        {
            string name = "";
            string error = "";
            do
            {
                Console.Clear();
                Console.WriteLine("\tChecking with elements");
                Console.WriteLine("\t======================");

                //Display the sets
                DisplaySets(sets);
                Console.WriteLine("\tChoose the set.");
                Console.WriteLine("\tPress \"-1\" to cancel.");
                Console.WriteLine();
                if (error != "")
                    DisplayError(error);
                ICSet<int> set = GetAndValidateInputForSetName("Set", sets, ref name);

                if (name == "-1")
                    break;
                if (set == null)
                {
                    error = "Invalid set name";
                    continue;
                }//end if

                //Reset the error.
                error = "";
                //Else 
                Console.Write("\tElement to add : ");
                string element = Console.ReadLine();
                if (element == "X")
                    break;
                if (element.Contains("}") || element.Contains("}"))
                {
                    var tree = GetElement(element);
                    if (tree == null)
                        continue;
                    DisplayContainsResults(set.Contains(tree), set, tree);
                }//end if
                else
                {
                    bool success = int.TryParse(element, out int elem);
                    if (!success)
                    {
                        error = "Only use integers";
                        continue;
                    }                        
                    DisplayContainsResults(set.Contains(elem), set, elem);
                }
                AnyKey("Press any key to continue........");
            } while (true);

            AnyKey();
        }//CheckWithElements
        #endregion Checking elements

        #region Set operations
        private static void PerformSetOperations(ISetCollection<int> sets)
        {
            ICSet<int> setA = default, setB = default, outcome = default;
            SetOperator @operator = SetOperator.Default;
            Operation opr = Operation.Continue;
            do
            {
                if (opr == Operation.Cancelled)
                    break;
                Console.Clear();
                Console.WriteLine("\tSet operators");
                Console.WriteLine("\t=============");
                Console.WriteLine();
                Console.WriteLine("\tChoose which operator to do.");
                Console.WriteLine("\t1. setA U setB");//U+2229
                Console.WriteLine("\t2. setA \u2229 setB");
                Console.WriteLine("\t3. setA - setB");
                Console.WriteLine("\t4. Complement");
                Console.WriteLine("\tX. Exit");
                Console.WriteLine();
                Console.Write("\tOption : ");
                string option = Console.ReadLine().ToUpper().Replace(" ", "");
                if (option == "X")
                    break;

                switch (option)
                {
                    case "1":
                    case "2":
                    case "3":

                        //Get the iput sets
                        var output_sets = GetInputSetCollection(InputTypeFunction.operations, sets, out opr);

                        //Check if we the user has not cancelled
                        if (opr == Operation.Cancelled)
                            continue;

                        //Get the two sets
                        setA = output_sets[0];
                        setB = output_sets[1];

                        if (option == "1")
                        {
                            @operator = SetOperator.Union;
                            outcome = setA.Union(setA);
                        }
                        else if (option == "2")
                        {
                            @operator = SetOperator.Intersection;
                            outcome = setA.Intersection(setB);
                        }
                        else
                        {
                            @operator = SetOperator.Difference;
                            outcome = setA.Difference(setB);
                        }
                        break;
                    case "4":
                        @operator = SetOperator.Complement;
                        SetComplement(sets);
                        break;
                    default:
                        break;
                }//end switch

                if( @operator != SetOperator.Complement)
                    DisplaySetOperationsResults(outcome, setA, setB, @operator);

                AnyKey(KeyType.Continue);
            } while (true);
            AnyKey(KeyType.Continue);
        }//PerformSetOperations
        private static void SetComplement(ISetCollection<int> sets)
        {
            string universalName = "";
            ICSet<int> universal = default;
            var loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("\tSet complement.");
                Console.WriteLine("\t===============");
                Console.WriteLine();
                DisplaySets(sets);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tFor set Complement, setA must be a subset of the universal set.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.WriteLine("\tPress \"-1\" to cancel");
                Console.WriteLine();
                if(universalName == "")
                {
                    universal = GetAndValidateInputForSetName("Universal set", sets,ref universalName);
                    if (universalName == "-1")
                        break;
                    if (universal == null)
                    {
                        universalName = "";
                        continue;
                    }
                    loop = false;
                }//end if
                //here we have the universal set
                if (loop)
                    Console.WriteLine("\tUniversal set : {0}", universalName);
                string setAName = "";
                ICSet<int> setA = GetAndValidateInputForSetName("SetA", sets, ref setAName);
                if (setAName == "-1")
                    break;
                if (setA == null)
                {
                    loop = true;
                    continue;
                }//end if

                Console.WriteLine();
                Console.WriteLine();
                ICSet<int> complement = setA.Complement(universal, out bool isUniversal);
                if (!isUniversal)
                {
                    Console.WriteLine("\t Set {0} is not a universal set for set {1}.", universalName, setAName);
                    continue;
                }//end if not universal
                //Here it is universal
                Console.WriteLine("\tThe complement of setA is \n\t{0}", complement.Cardinality);
                Console.WriteLine();
                AnyKey("Press any key to continue........");
            } while (true);
        }//SetComplement
        #endregion Set operations

        #region Perform set laws
        private static void PerformSetLaws(ISetCollection<int> sets)
        {
            DisplaySetLaws();
            AnyKey(KeyType.Continue);
            do
            {
                Console.Clear();
                DisplaySetLawPropertieMenu();
                string option = Inputs.ReadLineString();

                switch (option)
                {
                    /*  Console.WriteLine("\t1. Cummutative law");
                        Console.WriteLine("\t2. Associative law");
                        Console.WriteLine("\t3. Distributive law");
                        Console.WriteLine("\t5. DeMorgan's law");
                        Console.WriteLine("\t4. Double complement");
                        Console.WriteLine("\t6. Identity");
                        Console.WriteLine("\t7. Idempotence");
                        Console.WriteLine("\t8. Dominance");
                        Console.WriteLine("\t9. Review laws and properties");
                        Console.WriteLine("\tX. Exit");
            */
                    case "1":
                        CummutativeLaw(sets);
                        break;                    
                    case "2":
                        Associative(sets);
                        break;                    
                    case "3":
                        Distributive(sets);
                        break;                    
                    case "4":

                        break;                    
                    case "5":

                        break;                    
                    case "6":

                        break;                    
                    case "7":

                        break;                    
                    case "8":

                        break;                    
                    case "9":
                        Console.Clear();
                        DisplaySetLaws();
                        AnyKey(KeyType.Continue);
                        break;
                    case "X":
                        return;
                    default:
                        break;
                }//end switch
            } while (true);
        }//PerformSetLaws
        private static void CummutativeLaw(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\t  Cummutative law:");
            Console.WriteLine("\t  ==================");
            Console.WriteLine();
            Console.WriteLine("\t1.  A U B = B U A");
            Console.WriteLine("\t2.  A \u2229 B = B \u2229 A");
            Console.WriteLine("\tX. Exit");
            Console.WriteLine();
            string option = Inputs.ReadLineString();
            if (option == "X" && option != "1" && option != "2")
                return;

            //Read the sets
            var output_sets = GetInputSetCollection(InputTypeFunction.Properties | InputTypeFunction.set_laws, sets, out Operation opr);
            if (opr == Operation.Cancelled)
                return;

            var setA = output_sets[0]; var setB = output_sets[1];
            ICSet<int> outcomeleft, outcomeright;
            bool isFirstOption = false;
            if(option == "1")
            {
                outcomeleft = setA.Union(setB);
                outcomeright = setB.Union(setA);
                isFirstOption = true;
            }
            else
            {
                outcomeleft = setA.Intersection(setB);
                outcomeright = setB.Intersection(setA);
            }
            DisplaySetlawPropertiesOutcome(TypeOfLawProperty.Cummutativity, outcomeleft, outcomeright, setA, setB, isfirst_option: isFirstOption);
            AnyKey(KeyType.Continue);
        }//CummutativeLaw
        private static void Distributive(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\t  Distributive law:");
            Console.WriteLine("\t  ================");
            Console.WriteLine();
            Console.WriteLine("\t1. A U ( B \u2229 C ) = ( A U B ) \u2229 ( A U C)");
            Console.WriteLine("\t2. A \u2229 ( B U C ) = ( A \u2229 B ) U ( A \u2229 C)");
            Console.WriteLine("\tX. Exit");
            Console.WriteLine();
            string option = Inputs.ReadLineString();

            if (option == "X" && option != "1" && option != "2")
                return;

            //Read the sets
            var output_sets = GetInputSetCollection(InputTypeFunction.Properties | InputTypeFunction.set_laws, sets, out Operation opr, numberOfSets:3);
            if (opr == Operation.Cancelled)
                return;

            var setA = output_sets[0]; var setB = output_sets[1]; var setC = output_sets[2];
            ICSet<int> outcomeleft, outcomeright;
            bool isFirstOption = false;
            if (option == "1")
            {
                outcomeleft = setA.Union(setB.Intersection(setC));
                outcomeright = (setA.Union(setB)).Intersection(setA.Union(setC));
                isFirstOption = true;
            }
            else
            {
                outcomeleft = setA.Intersection(setB.Union(setC));
                outcomeright = (setA.Intersection(setB)).Union(setA.Intersection(setC));
            }
            DisplaySetlawPropertiesOutcome(TypeOfLawProperty.Distributive, outcomeleft, outcomeright, setA, setB, setC, isfirst_option: isFirstOption);

            AnyKey(KeyType.Continue);
        }
        private static void Associative(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\t Associative law:");
            Console.WriteLine("\t =================");
            Console.WriteLine("\t1. A U ( B U C ) = ( A U B) U C");
            Console.WriteLine("\t2. A \u2229 ( B \u2229 C ) = (A \u2229 B) \u2229 C");
            Console.WriteLine("\tX. Exit");
            Console.WriteLine();
            string option = Inputs.ReadLineString();

            if (option == "X" && option != "1" && option != "2")
                return;

            //Read the sets
            var output_sets = GetInputSetCollection(InputTypeFunction.Properties | InputTypeFunction.set_laws, sets, out Operation opr, numberOfSets: 3);
            if (opr == Operation.Cancelled)
                return;

            var setA = output_sets[0]; var setB = output_sets[1]; var setC = output_sets[2];
            ICSet<int> outcomeleft, outcomeright;
            bool isFirstOption = false;
            if (option == "1")
            {
                outcomeleft = setA.Union(setB.Union(setC));
                outcomeright = (setA.Union(setB)).Union(setC);
                isFirstOption = true;
            }
            else
            {
                outcomeleft = setA.Intersection(setB.Intersection(setC));
                outcomeright = (setA.Intersection(setB)).Intersection(setC);
            }
            DisplaySetlawPropertiesOutcome(TypeOfLawProperty.Associative, outcomeleft, outcomeright, setA, setB, setC, isfirst_option: isFirstOption);
            AnyKey(KeyType.Continue);
        }
        private static void DeMorgan(ISetCollection<int> sets)
        {
            Console.Clear();
            Console.WriteLine("\t5. DeMorgan's Law:");
            Console.WriteLine("\t====================");
            Console.WriteLine("\tNote: The following law will hold true <=> both setA and setB are in a universal setC");
            Console.WriteLine();
            Console.WriteLine("\t1. ~( A U B ) = ~A \u2229 ~B");
            Console.WriteLine("\t2. ~( A \u2229 B ) = ~A U ~B");
            Console.WriteLine("\tX. Exit");
            string option = Inputs.ReadLineString();
            if (option == "X" && option != "1" && option != "2")
                return;

            //Read the sets
            var output_sets = GetInputSetCollection(InputTypeFunction.Properties | InputTypeFunction.set_laws, sets, out Operation opr);
            if (opr == Operation.Cancelled)
                return;

            var setA = output_sets[0]; var setB = output_sets[1];
            ICSet<int> outcomeleft, outcomeright;
            bool isFirstOption = false;
            if (option == "1")
            {
                outcomeleft = setA.Union(setB);
                outcomeright = setB.Union(setA);
                isFirstOption = true;
            }
            else
            {
                outcomeleft = setA.Intersection(setB);
                outcomeright = setB.Intersection(setA);
            }
            DisplaySetlawPropertiesOutcome(TypeOfLawProperty.Cummutativity, outcomeleft, outcomeright, setA, setB, isfirst_option: isFirstOption);
        }
        #endregion Set laws

        #region Clearing and reseting naming
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

            if (option == "Y")
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
        #endregion Clearing and reseting naming

        #region Learn more about sets
        private static void LearnMoreAboutSets()
        {
            Console.Clear();
            Console.WriteLine("\tLearning about sets.........");
            AnyKey();
        }//LearnMoreAboutSets
        #endregion Learn more about sets.

        private static void AnyKey(KeyType type)
        {
            string msg = $"Press any key to {type.ToString().ToLower()}";
            Console.WriteLine();
            Console.Write($"\t{msg}...");
            Console.ReadKey();
        }//AnyKey
    }//class

}//namespace
