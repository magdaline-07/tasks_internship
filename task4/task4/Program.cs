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

            // Поиск подстроки с началом и концом на гласную
            string vowelSubstring = FindLongestVowelSubstring(result);
            Console.WriteLine("Наибольшая подстрока с началом и концом на гласную:");
            Console.WriteLine(vowelSubstring.Length > 0 ? vowelSubstring : "(не найдена)");
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

    static Dictionary<char, int> CountCharacters(string s)
    {
        Dictionary<char, int> counts = new();

        foreach (char c in s)
        {
            if (counts.ContainsKey(c))
                counts[c]++;
            else
                counts[c] = 1;
        }

        return counts;
    }

    // Поиск наибольшей подстроки, начинающейся и заканчивающейся на гласную
    static string FindLongestVowelSubstring(string s)
    {
        string vowels = "aeiouy";
        int maxLen = 0;
        string longest = "";

        for (int i = 0; i < s.Length; i++)
        {
            if (!vowels.Contains(s[i]))
                continue;

            for (int j = s.Length - 1; j > i; j--)
            {
                if (vowels.Contains(s[j]))
                {
                    string sub = s.Substring(i, j - i + 1);
                    if (sub.Length > maxLen)
                    {
                        maxLen = sub.Length;
                        longest = sub;
                    }
                    break; // Дальше искать нет смысла, т.к  уже найдена ближайшая гласная с конца
                }
            }
        }

        return longest;
    }
}
