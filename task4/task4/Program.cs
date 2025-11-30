using System;
using System.IO;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("Выберите задание (1–5):");
        int z = int.Parse(Console.ReadLine());

        if (z == 1) Zad1();
        if (z == 2) Zad2();
        if (z == 3) Zad3();
        if (z == 4) Zad4();
        if (z == 5) Zad5();
    }


    static void Zad1()
    {
        Thread t = new Thread(() =>
        {
            for (int i = 0; i <= 50; i++)
                Console.WriteLine(i);
        });
        t.Start();
        t.Join();
    }


    static void Zad2()
    {
        Console.Write("Начало: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Конец: ");
        int b = int.Parse(Console.ReadLine());

        Thread t = new Thread(() =>
        {
            for (int i = a; i <= b; i++)
                Console.WriteLine(i);
        });
        t.Start();
        t.Join();
    }


    static void Zad3()
    {
        Console.Write("Потоков: ");
        int k = int.Parse(Console.ReadLine());
        Console.Write("Начало: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Конец: ");
        int b = int.Parse(Console.ReadLine());

        Thread[] arr = new Thread[k];

        for (int i = 0; i < k; i++)
        {
            arr[i] = new Thread(() =>
            {
                for (int n = a; n <= b; n++)
                    Console.WriteLine("Поток " + Thread.CurrentThread.ManagedThreadId + ": " + n);
            });
            arr[i].Start();
        }

        foreach (var t in arr) t.Join();
    }


    static int[] nums;
    static int max, min;
    static double avg;

    static void Zad4()
    {
        nums = new int[10000];
        Random r = new Random();
        for (int i = 0; i < nums.Length; i++) nums[i] = r.Next(1, 10000);

        Thread t1 = new Thread(() =>
        {
            max = nums[0];
            foreach (var x in nums) if (x > max) max = x;
        });

        Thread t2 = new Thread(() =>
        {
            min = nums[0];
            foreach (var x in nums) if (x < min) min = x;
        });

        Thread t3 = new Thread(() =>
        {
            long s = 0;
            foreach (var x in nums) s += x;
            avg = s / (double)nums.Length;
        });

        t1.Start(); t2.Start(); t3.Start();
        t1.Join(); t2.Join(); t3.Join();

        Console.WriteLine("Максимум: " + max);
        Console.WriteLine("Минимум: " + min);
        Console.WriteLine("Среднее: " + avg);
    }


    static void Zad5()
    {
        nums = new int[10000];
        Random r = new Random();
        for (int i = 0; i < nums.Length; i++) nums[i] = r.Next(1, 10000);

        Thread t1 = new Thread(() =>
        {
            max = nums[0];
            foreach (var x in nums) if (x > max) max = x;
        });

        Thread t2 = new Thread(() =>
        {
            min = nums[0];
            foreach (var x in nums) if (x < min) min = x;
        });

        Thread t3 = new Thread(() =>
        {
            long s = 0;
            foreach (var x in nums) s += x;
            avg = s / (double)nums.Length;
        });

        Thread writer = new Thread(() =>
        {
            using (StreamWriter w = new StreamWriter("output.txt"))
            {
                foreach (var x in nums) w.WriteLine(x);
                w.WriteLine("Максимум: " + max);
                w.WriteLine("Минимум: " + min);
                w.WriteLine("Среднее: " + avg);
            }
        });

        t1.Start(); t2.Start(); t3.Start();
        t1.Join(); t2.Join(); t3.Join();

        writer.Start();
        writer.Join();

        Console.WriteLine("Готово. Данные записаны в output.txt");
    }
}
