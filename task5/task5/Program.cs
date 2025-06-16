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
            Console.WriteLine(string.Join(", ", invalidChars));
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

            // Выбор сортировки
            Console.WriteLine("Выберите метод сортировки: Quicksort/ Tree sort");
            string choice = Console.ReadLine() ?? "quick";
            string sorted = "";

            if (choice.ToLower() == "tree")
                sorted = TreeSort(result);
            else
                sorted = QuickSort(result.ToCharArray(), 0, result.Length - 1);

            Console.WriteLine($"Отсортированная строка ({choice}): {sorted}");
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

    static string FindLongestVowelSubstring(string s)
    {
        string vowels = "aeiouy";
        int maxLen = 0;
        string longest = "";

        for (int i = 0; i < s.Length; i++)
        {
            if (!vowels.Contains(s[i])) continue;

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
                    break;
                }
            }
        }

        return longest;
    }

    // Быстрая сортировка (Quicksort)
    static string QuickSort(char[] arr, int left, int right)
    {
        int i = left, j = right;
        char pivot = arr[(left + right) / 2];

        while (i <= j)
        {
            while (arr[i] < pivot) i++;
            while (arr[j] > pivot) j--;

            if (i <= j)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                i++; j--;
            }
        }

        if (left < j) QuickSort(arr, left, j);
        if (i < right) QuickSort(arr, i, right);

        return new string(arr);
    }

    // Сортировка деревом (Tree sort)
    class TreeNode
    {
        public char Value;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(char value)
        {
            Value = value;
        }

        public void Insert(char newValue)
        {
            if (newValue < Value)
            {
                if (Left == null) Left = new TreeNode(newValue);
                else Left.Insert(newValue);
            }
            else
            {
                if (Right == null) Right = new TreeNode(newValue);
                else Right.Insert(newValue);
            }
        }

        public void InOrderTraversal(List<char> result)
        {
            Left?.InOrderTraversal(result);
            result.Add(Value);
            Right?.InOrderTraversal(result);
        }
    }

    static string TreeSort(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        TreeNode root = new(input[0]);

        for (int i = 1; i < input.Length; i++)
        {
            root.Insert(input[i]);
        }

        List<char> sorted = new();
        root.InOrderTraversal(sorted);
        return new string(sorted.ToArray());
    }
}
