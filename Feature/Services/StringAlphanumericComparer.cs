using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Feature.Services
{
    /// <summary>
    /// Класс-компаратор для сортировки строк с числами (типа "Квартира 01") в порядке возрастания чисел
    /// </summary>
    public class StringAlphanumericComparer : IComparer<string>
    {
        // Регулярное выражение вернет числа в строке
        private readonly Regex regex = new Regex(@"\d+");
        public int Compare(string x, string y)
        {
            Match m1 = regex.Match(x);
            Match m2 = regex.Match(y);
            string num1 = m1.Value;
            string num2 = m2.Value;
            if (num1.Length < num2.Length)
                return -1;
            if (num1.Length > num2.Length)
                return 1;
            return string.Compare(num1, num2);                    
        }
    }
}

