using System;
using System.Linq;

namespace CompileParser.Helper
{
    public static class Helper
    {
    public static String letters = "اأبتثجحخدذرزسشصضطظعغفقكلمنهویيىة";
    public static  String digits = "0123456789٠١٢٣٤٥٦٧٨٩";


    
    public static bool isAlpha(char ch)
        {
            if (ch == '\0') return false;

            bool isLetter = false;
            
            if (letters.FirstOrDefault(val => val == ch)==ch)
            {
                isLetter = true;
            }
            return isLetter;
        }

        
        public static bool isDigit(char ch)
        {
            if (ch == '\0') return false;

            bool isDigit = false;

            if (digits.FirstOrDefault(val => val == ch) == ch)
            {
                isDigit = true;
            }
           
            return isDigit;
        }
        public static bool isID(string input)
        {
            if (input == "\0") return false;

            if (isAlpha(input[0]))
            {
                foreach (var ch in input)
                {
                    if (isAlpha(ch) || isDigit(ch)) continue;
                    else return false;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
