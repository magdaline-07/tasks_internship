using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
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
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");

            // Поиск подстроки с началом и концом на гласную
            string vowelSubstring = FindLongestVowelSubstring(result);
            Console.WriteLine("Наибольшая подстрока с началом и концом на гласную:");
            Console.WriteLine(vowelSubstring.Length > 0 ? vowelSubstring : "(не найдена)");

            // Метод сортировки
            Console.WriteLine("Выберите метод сортировки: Quicksort/ Tree sort");
            string choice = Console.ReadLine() ?? "Quicksort";
            string sorted = choice.ToLower() == "Tree sort" ? TreeSort(result) : QuickSort(result.ToCharArray(), 0, result.Length - 1);
            Console.WriteLine($"Отсортированная строка ({choice}): {sorted}");

            // Случайное число
            int randomIndex = await GetRandomNumberAsync(result.Length);
            Console.WriteLine($"Удаляем символ на позиции: {randomIndex}");

            string reduced = RemoveAt(result, randomIndex);
            Console.WriteLine($"Урезанная строка: {reduced}");
        }
    }

    static string ProcessString(string input)
    {
        if (input.Length % 2 == 0)
        {
            int half = input.Length / 2;
            return Reverse(input[..half]) + Reverse(input[half..]);
        }
        return Reverse(input) + input;
    }

    static string Reverse(string s)
    {
        var arr = s.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }

    static Dictionary<char, int> CountCharacters(string s)
    {
        var dict = new Dictionary<char, int>();
        foreach (var c in s)
        {
            if (dict.ContainsKey(c)) dict[c]++;
            else dict[c] = 1;
        }
        return dict;
    }

    // Поиск наибольшей подстроки, начинающейся и заканчивающейся на гласную
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
                    var sub = s.Substring(i, j - i + 1);
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


    // Получение случайного числа через API или fallback
    static async Task<int> GetRandomNumberAsync(int maxExclusive) // Асинхронное получение случайное число 
    {
        try
        {
            using HttpClient client = new(); // отправка HTTP-запросов
            string url = $"https://www.randomnumberapi.com/api/v1.0/random?min=0&max={maxExclusive - 1}&count=1"; // URL для API 
            var response = await client.GetStringAsync(url); // get запрос, получение ответа 
            var numbers = JsonSerializer.Deserialize<List<int>>(response); // Преобразование строки JSON в список целых чисел
            return numbers?.FirstOrDefault() ?? new Random().Next(0, maxExclusive); // Возвращение первого числа из списка (или генерация случайного, если список пустой)
        }
        catch
        {
            Console.WriteLine("(API недоступно, использован локальный генератор)");

            // Случайное число генерируется средства .NET, если API недоступно
            return new Random().Next(0, maxExclusive);
        }
    }

    // Метод для удаления символа по заданному индексу
    static string RemoveAt(string s, int index)
    {
        return s.Remove(index, 1);
    }
}
