using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    const uint WM_SETTEXT = 0x000C;
    const uint WM_CLOSE = 0x0010;

    static void Main()
    {
        Console.WriteLine("Выберите задание (1 или 2):");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Zadanie1();
        }
        else if (choice == "2")
        {
            Zadanie2();
        }
    }

    static void Zadanie1()
    {
        MessageBox(IntPtr.Zero, "Имя: Иван", "Информация", 0);
        MessageBox(IntPtr.Zero, "Возраст: 19", "Информация", 0);
        MessageBox(IntPtr.Zero, "Группа: ПКС-21", "Информация", 0);
    }

    static void Zadanie2()
    {
        Console.WriteLine("Введите заголовок окна:");
        string title = Console.ReadLine();
        IntPtr hWnd = FindWindow(null, title);

        if (hWnd == IntPtr.Zero)
        {
            Console.WriteLine("Окно не найдено");
            return;
        }

        Console.WriteLine("1 — Изменить заголовок");
        Console.WriteLine("2 — Закрыть окно");
        Console.WriteLine("3 — Мигание окна");
        string act = Console.ReadLine();

        if (act == "1")
        {
            Console.WriteLine("Введите новый заголовок:");
            string newTitle = Console.ReadLine();
            SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, Marshal.StringToHGlobalAuto(newTitle));
        }
        else if (act == "2")
        {
            SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        else if (act == "3")
        {
            for (int i = 0; i < 3; i++)
            {
                SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, Marshal.StringToHGlobalAuto("*****"));
                Thread.Sleep(300);
                SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, Marshal.StringToHGlobalAuto(title));
                Thread.Sleep(300);
            }
        }
    }
}
