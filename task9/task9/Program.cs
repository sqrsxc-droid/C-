using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nВыберите задание:");
            Console.WriteLine("1 – Task: время и дата (3 способа запуска)");
            Console.WriteLine("2 – Task: простые числа 0–1000");
            Console.WriteLine("3 – Task: простые числа в диапазоне");
            Console.WriteLine("4 – Task: минимум, максимум, среднее, сумма массива");
            Console.WriteLine("5 – continuation tasks: удаление дублей, сортировка, поиск");
            Console.WriteLine("0 – выход");

            Console.Write("Ввод: ");
            string cmd = Console.ReadLine();

            if (cmd == "0") break;
            if (cmd == "1") Task1();
            if (cmd == "2") Task2();
            if (cmd == "3") Task3();
            if (cmd == "4") Task4();
            if (cmd == "5") Task5();
        }
    }


    static void Task1()
    {
        Console.WriteLine("\n=== Задание 1 ===");

        Task t1 = new Task(() =>
        {
            Console.WriteLine("Start: " + DateTime.Now);
        });
        t1.Start();
        t1.Wait();

        Task t2 = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Factory.StartNew: " + DateTime.Now);
        });
        t2.Wait();

        Task t3 = Task.Run(() =>
        {
            Console.WriteLine("Task.Run: " + DateTime.Now);
        });
        t3.Wait();
    }


    static void Task2()
    {
        Console.WriteLine("\n=== Задание 2 ===");

        Task t = Task.Run(() =>
        {
            for (int i = 2; i <= 1000; i++)
            {
                if (IsPrime(i))
                    Console.Write(i + " ");
            }
        });

        t.Wait();
        Console.WriteLine("\nЗавершено.");
    }


    static void Task3()
    {
        Console.WriteLine("\n=== Задание 3 ===");

        Console.Write("Введите начало: ");
        int a = int.Parse(Console.ReadLine());

        Console.Write("Введите конец: ");
        int b = int.Parse(Console.ReadLine());

        int count = 0;

        Task t = Task.Run(() =>
        {
            for (int i = a; i <= b; i++)
            {
                if (IsPrime(i))
                    count++;
            }
        });

        t.Wait();
        Console.WriteLine("Количество простых чисел: " + count);
    }

    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

  
    static void Task4()
    {
        Console.WriteLine("\n=== Задание 4 ===");

        int[] arr = { 5, 2, 9, 1, 7, 3, 4, 8, 11, 6 };

        int min = 0, max = 0, sum = 0;
        double avg = 0;

        Task[] tasks = new Task[4];

        tasks[0] = Task.Run(() => min = arr.Min());
        tasks[1] = Task.Run(() => max = arr.Max());
        tasks[2] = Task.Run(() => sum = arr.Sum());
        tasks[3] = Task.Run(() => avg = arr.Average());

        Task.WaitAll(tasks);

        Console.WriteLine("Минимум: " + min);
        Console.WriteLine("Максимум: " + max);
        Console.WriteLine("Сумма: " + sum);
        Console.WriteLine("Среднее: " + avg);
    }

   
    static void Task5()
    {
        Console.WriteLine("\n=== Задание 5 ===");

        int[] arr = { 3, 1, 2, 3, 5, 1, 7, 2, 9, 5 };

        Task<int[]> removeDup = Task.Run(() =>
        {
            return arr.Distinct().ToArray();
        });

        Task<int[]> sort = removeDup.ContinueWith(prev =>
        {
            int[] sorted = prev.Result;
            Array.Sort(sorted);
            return sorted;
        });

        Task search = sort.ContinueWith(prev =>
        {
            Console.Write("Введите число для поиска: ");
            int x = int.Parse(Console.ReadLine());

            bool found = Array.BinarySearch(prev.Result, x) >= 0;

            Console.WriteLine("\nОтсортированный массив:");
            Console.WriteLine(string.Join(", ", prev.Result));

            Console.WriteLine(found ? "Число найдено." : "Число НЕ найдено.");
        });

        search.Wait();
    }
}
