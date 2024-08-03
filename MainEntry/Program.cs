using static System.Console;
using System.Diagnostics;
namespace MainEntry
{
    class Program
    {
        static void Main(string[] args)
        {
            Process application = default;
            do
            {
                Clear();
                if (application != default && !application.HasExited)
                    application.Kill();//Close the application
                WriteLine("\tWELCOME");
                WriteLine("\t=======");
                WriteLine("\tQuick Links:");
                WriteLine("\t============");
                WriteLine();
                WriteLine("\tSIMPLE SET");
                WriteLine("\tSource code : https://github.com/Shisui-Pho/Custom_Sets/tree/master/SimpleSets");
                WriteLine("\tApplication : https://github.com/Shisui-Pho/Custom_Sets/tree/master/SimpleSets/App");
                WriteLine();
                WriteLine("\tADVANCED SET");
                WriteLine("\tSource code : https://github.com/Shisui-Pho/Custom_Sets/tree/master/AdvancedSet");
                WriteLine("\tApplication : https://github.com/Shisui-Pho/Custom_Sets/tree/master/AdvancedSet/App");
                WriteLine();
                WriteLine("\tSetLibrary(Logic behind the advanced set)");
                WriteLine("\tSource code : https://github.com/Shisui-Pho/Advance-Custom-Set/tree/master/SetLibrary");
                WriteLine("\tDll file    : https://github.com/Shisui-Pho/Advance-Custom-Set/tree/master/SetLibrary/Dll");
                WriteLine();
                WriteLine();
                WriteLine("\tChoose which application you with to open");
                WriteLine("\t=========================================");
                WriteLine();
                WriteLine("\t1. Simple set");
                WriteLine("\t2. Advanced set");
                WriteLine("\tX. Exit");

                Write("\n\tOption : ");
                string s = ReadLine().ToUpper();
                if (s.Length < 1)
                    continue;

                char option = s[0];

                if (option == '1')
                    application = Process.Start("SimpleSets");
                if(option == '2')
                    application = Process.Start("AdvancedSet.exe");
                if (option == 'X')
                    break;

                WriteLine();
                WriteLine();
                Write("\tAny key to continue......");
                ReadKey();
            } while (true);

            if(application != default &&  !application.HasExited)
                application.Kill();
        }//Main
    }//class
}//namespace