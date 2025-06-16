using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string input = Console.ReadLine() ?? "";

        string result = ProcessString(input);

        Console.WriteLine($"Обработанная строка: {result}");
    }

    static string ProcessString(string input)
    {
        int length = input.Length;

        if (length % 2 == 0)
        {
            // Чётное количество символов
            int half = length / 2;
            string part1 = Reverse(input.Substring(0, half));
            string part2 = Reverse(input.Substring(half));
            return part1 + part2;
        }
        else
        {
            // Нечётное количество символов
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
