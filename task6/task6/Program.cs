using System;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

public class Form1 : Form
{
    TextBox primesBox;
    TextBox fibBox;

    TextBox minBox;
    TextBox maxBox;

    Button startBtn;
    Button stopPrimeBtn;
    Button stopFibBtn;
    Button pausePrimeBtn;
    Button resumePrimeBtn;
    Button pauseFibBtn;
    Button resumeFibBtn;
    Button restartBtn;

    Thread primeThread;
    Thread fibThread;

    bool primeStop = false;
    bool fibStop = false;

    bool primePaused = false;
    bool fibPaused = false;

    public Form1()
    {
        Width = 700;
        Height = 600;

        primesBox = new TextBox();
        primesBox.Multiline = true;
        primesBox.SetBounds(10, 10, 300, 500);
        Controls.Add(primesBox);

        fibBox = new TextBox();
        fibBox.Multiline = true;
        fibBox.SetBounds(330, 10, 300, 500);
        Controls.Add(fibBox);

        minBox = new TextBox();
        minBox.SetBounds(10, 520, 100, 25);
        minBox.PlaceholderText = "min";
        Controls.Add(minBox);

        maxBox = new TextBox();
        maxBox.SetBounds(120, 520, 100, 25);
        maxBox.PlaceholderText = "max";
        Controls.Add(maxBox);

        startBtn = new Button();
        startBtn.Text = "Старт";
        startBtn.SetBounds(230, 520, 100, 25);
        startBtn.Click += StartAll;
        Controls.Add(startBtn);

        stopPrimeBtn = new Button();
        stopPrimeBtn.Text = "Стоп простые";
        stopPrimeBtn.SetBounds(340, 520, 120, 25);
        stopPrimeBtn.Click += (s, e) => primeStop = true;
        Controls.Add(stopPrimeBtn);

        stopFibBtn = new Button();
        stopFibBtn.Text = "Стоп Фиб";
        stopFibBtn.SetBounds(470, 520, 120, 25);
        stopFibBtn.Click += (s, e) => fibStop = true;
        Controls.Add(stopFibBtn);

        pausePrimeBtn = new Button();
        pausePrimeBtn.Text = "Пауза простые";
        pausePrimeBtn.SetBounds(340, 550, 120, 25);
        pausePrimeBtn.Click += (s, e) => primePaused = true;
        Controls.Add(pausePrimeBtn);

        resumePrimeBtn = new Button();
        resumePrimeBtn.Text = "Продолжить";
        resumePrimeBtn.SetBounds(470, 550, 120, 25);
        resumePrimeBtn.Click += (s, e) => primePaused = false;
        Controls.Add(resumePrimeBtn);

        pauseFibBtn = new Button();
        pauseFibBtn.Text = "Пауза Фиб";
        pauseFibBtn.SetBounds(340, 580, 120, 25);
        pauseFibBtn.Click += (s, e) => fibPaused = true;
        Controls.Add(pauseFibBtn);

        resumeFibBtn = new Button();
        resumeFibBtn.Text = "Продолжить";
        resumeFibBtn.SetBounds(470, 580, 120, 25);
        resumeFibBtn.Click += (s, e) => fibPaused = false;
        Controls.Add(resumeFibBtn);

        restartBtn = new Button();
        restartBtn.Text = "Рестарт";
        restartBtn.SetBounds(10, 550, 210, 25);
        restartBtn.Click += RestartAll;
        Controls.Add(restartBtn);
    }

    void StartAll(object sender, EventArgs e)
    {
        primeStop = false;
        fibStop = false;

        primePaused = false;
        fibPaused = false;

        primesBox.Clear();
        fibBox.Clear();

        int start = 2;
        int end = int.MaxValue;

        if (int.TryParse(minBox.Text, out int m)) start = m;
        if (int.TryParse(maxBox.Text, out int x)) end = x;

        primeThread = new Thread(() => GeneratePrimes(start, end));
        fibThread = new Thread(GenerateFib);

        primeThread.Start();
        fibThread.Start();
    }

    void RestartAll(object sender, EventArgs e)
    {
        primeStop = true;
        fibStop = true;

        primeThread?.Abort();
        fibThread?.Abort();

        primesBox.Clear();
        fibBox.Clear();

        StartAll(null, null);
    }

    void GeneratePrimes(int start, int end)
    {
        for (int i = start; i <= end && !primeStop; i++)
        {
            while (primePaused) Thread.Sleep(100);

            if (IsPrime(i))
            {
                Invoke(new Action(() => primesBox.AppendText(i + "\r\n")));
                Thread.Sleep(50);
            }
        }
    }

    bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    void GenerateFib()
    {
        long a = 0;
        long b = 1;

        while (!fibStop)
        {
            while (fibPaused) Thread.Sleep(100);

            Invoke(new Action(() => fibBox.AppendText(a + "\r\n")));

            long c = a + b;
            a = b;
            b = c;

            Thread.Sleep(50);
        }
    }
}

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new Form1());
    }
}
