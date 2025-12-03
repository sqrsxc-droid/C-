using System;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nВыберите задание:");
            Console.WriteLine("1 – События: генерирование чисел и анализ");
            Console.WriteLine("2 – Критическая секция: анализ файла и модификация");
            Console.WriteLine("3 – Критическая секция: сортировка и поиск");
            Console.WriteLine("0 – Выход");
            Console.Write("Ввод: ");

            string cmd = Console.ReadLine();
            if (cmd == "0") return;

            if (cmd == "1") Task1_Events();
            else if (cmd == "2") Task2_CriticalSection_File();
            else if (cmd == "3") Task3_CriticalSection_Array();
            else Console.WriteLine("Неверный выбор!\n");
        }
    }


    static void Task1_Events()
    {
        Console.WriteLine("\n=== Задание 1: события ===");

        int[] numbers = new int[1000];
        ManualResetEvent generateDone = new ManualResetEvent(false);

        // Поток генерации чисел
        Thread generator = new Thread(() =>
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
                numbers[i] = r.Next(0, 5000);

            Console.WriteLine("Генерация завершена.");
            generateDone.Set(); // Сигнал остальным потокам
        });

        Thread tMax = new Thread(() =>
        {
            generateDone.WaitOne();
            int max = numbers.Max();
            Console.WriteLine("Максимум: " + max);
        });

        Thread tMin = new Thread(() =>
        {
            generateDone.WaitOne();
            int min = numbers.Min();
            Console.WriteLine("Минимум: " + min);
        });

        Thread tAvg = new Thread(() =>
        {
            generateDone.WaitOne();
            double avg = numbers.Average();
            Console.WriteLine("Среднее: " + avg);
        });

        generator.Start();
        tMax.Start();
        tMin.Start();
        tAvg.Start();

        generator.Join();
        tMax.Join();
        tMin.Join();
        tAvg.Join();
    }


    static void Task2_CriticalSection_File()
    {
        Console.WriteLine("\n=== Задание 2: критическая секция (файл) ===");

        Console.Write("Введите путь к файлу: ");
        string path = Console.ReadLine();

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        object locker = new object();
        string fileContent = "";
        int sentenceCount = 0;

        Thread reader = new Thread(() =>
        {
            lock (locker)
            {
                fileContent = File.ReadAllText(path);

                // Подсчёт предложений: ., ! или ?
                sentenceCount = fileContent.Count(c => ".!?".Contains(c));

                Console.WriteLine("Количество предложений: " + sentenceCount);
            }
        });

        Thread writer = new Thread(() =>
        {
            reader.Join(); // ждем завершения первого потока

            lock (locker)
            {
                string modified = fileContent.Replace("!", "#");
                File.WriteAllText(path, modified);

                Console.WriteLine("Замена '!' на '#' выполнена.");
            }
        });

        reader.Start();
        writer.Start();

        reader.Join();
        writer.Join();
    }

    static void Task3_CriticalSection_Array()
    {
        Console.WriteLine("\n=== Задание 3: критическая секция (массив) ===");

        int[] arr = { 5, 1, 9, 3, 7, 2, 8, 4, 6 };
        object locker = new object();

        Console.Write("Введите число для поиска: ");
        int searchNum = int.Parse(Console.ReadLine());

        Thread sorter = new Thread(() =>
        {
            lock (locker)
            {
                Array.Sort(arr);
                Console.WriteLine("Массив отсортирован.");
            }
        });

        Thread finder = new Thread(() =>
        {
            sorter.Join(); // ждем завершения сортировки

            lock (locker)
            {
                bool found = Array.BinarySearch(arr, searchNum) >= 0;
                Console.WriteLine("Число " + searchNum + (found ? " найдено." : " НЕ найдено."));
            }
        });

        sorter.Start();
        finder.Start();

        sorter.Join();
        finder.Join();

        Console.WriteLine("Отсортированный массив: " + string.Join(", ", arr));
    }
}
