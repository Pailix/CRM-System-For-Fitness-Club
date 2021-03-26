using System;
using System.Linq;
using Xceed.Wpf.Toolkit;

namespace CRM_System_For_Fitness_Club
{
    public static class InputChecker
    {
        public static bool IsTextStringCorrect(string stringToCheck)
        {
            if (stringToCheck.Length>0 && stringToCheck.All(char.IsLetter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsPhoneCorrect(MaskedTextBox phoneTxtBox)
        {
            return phoneTxtBox.IsMaskFull;
        }
        public static bool IsAdult(DateTime? birthDate)
        {
            if (birthDate!=null && DateTime.Now.Year - birthDate.Value.Year > 17)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDecimalCorrect(string decimalToCheck)
        {
            if (decimalToCheck.Length>0 && decimal.TryParse(decimalToCheck, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsTimePickedRight(DateTime timeToCheck)
        {
            if (timeToCheck.Hour > 7 && timeToCheck.Hour < 23)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDateInFuture(DateTime date)
        {
            if (date.Date >= DateTime.Now.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
