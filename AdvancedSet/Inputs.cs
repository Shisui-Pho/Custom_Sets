using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSet
{
    public static class Inputs
    {
        public static string ReadLineString(string prompt = "Option", bool uppercase = true)
        {
            Console.Write($"\t{prompt} :");
            string feedback = Console.ReadLine();
            if (uppercase)
                feedback = feedback.ToUpper();
            return feedback;
        }//end 
        public static T ReadLineValue<T>(string prompt)
        {
            Console.WriteLine();
            Console.Write($"\t{prompt} :");
            string feedback = Console.ReadLine();
            try
            {
                return (T)Convert.ChangeType(feedback, typeof(T));
            }
            catch
            {
                return ReadLineValue<T>(prompt);
            }
        }
        public static T ReadLineEnum<T>(string prompt)
            where T : Enum
        {
            Console.WriteLine();
            Console.Write($"\t{prompt} :");
            string feedback = Console.ReadLine();
            try
            {
                T val = (T)Enum.Parse(typeof(T), feedback);
                return (T)Convert.ChangeType(feedback, typeof(T));
            }
            catch
            {
                return ReadLineValue<T>(prompt);
            }
        }
    }
}
