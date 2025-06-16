using System;
using System.Linq;

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
            Console.WriteLine(string.Join(", ", invalidChars));
        }
        else
        {
            string result = ProcessString(input);
            Console.WriteLine($"Обработанная строка: {result}");
        }
    }

    // Задача 1
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
}
