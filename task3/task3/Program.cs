using System;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main()
    {  
        Console.Write("Введите строку: ");
        string input = Console.ReadLine() ?? "";

        // Проверка на недопустимые символы (все, что не входит в от 'a' до 'z')
        var invalidChars = input.Where(c => c < 'a' || c > 'z').Distinct().ToArray();

        if (invalidChars.Length > 0)
        {
            Console.WriteLine("Ошибка: Неподходящая строка. Строка содержит недопустимые символы:");
            Console.WriteLine(string.Join(", ", invalidChars)); // Выводим список недопустимых символов
        }
        else
        {    
            string result = ProcessString(input);
            Console.WriteLine($"Обработанная строка: {result}");

            // Подсчёт количества вхождений каждого символа
            Console.WriteLine("Частота символов:");
            var charCounts = CountCharacters(result);

            // Вывожу результат, отсортированный по символу
            foreach (var kvp in charCounts.OrderBy(c => c.Key))
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }

    static string ProcessString(string input)
    {
        int length = input.Length;

        if (length % 2 == 0) 
        {
            int half = length / 2;
            string part1 = Reverse(input.Substring(0, half));   
            string part2 = Reverse(input.Substring(half));      
            return part1 + part2; 
        }
        else 
        {
            return Reverse(input) + input; 
        }
    }

    static string Reverse(string s)
    {
        char[] arr = s.ToCharArray(); 
        Array.Reverse(arr);          
        return new string(arr);      
    }

    // Подсчёт количества каждого символа в строке
    static Dictionary<char, int> CountCharacters(string s)
    {
        Dictionary<char, int> counts = new(); // Словарь для хранения частоты

        foreach (char c in s)
        {
            if (counts.ContainsKey(c)) // Если символ есть, то счетчик увеличивается
                counts[c]++;
            else                        
                counts[c] = 1;
        }

        return counts; 
    }
}
