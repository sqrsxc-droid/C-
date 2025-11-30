using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

class Program : Form
{
    ListBox list;
    Timer timer;
    Button killBtn;
    Button startBtn;
    NumericUpDown intervalBox;
    Label info;

    public Program()
    {
        Width = 600;
        Height = 400;

        list = new ListBox();
        list.SetBounds(10, 10, 250, 330);
        list.SelectedIndexChanged += (s, e) => ShowInfo();
        Controls.Add(list);

        intervalBox = new NumericUpDown();
        intervalBox.SetBounds(270, 10, 100, 30);
        intervalBox.Minimum = 1;
        intervalBox.Value = 3;
        Controls.Add(intervalBox);

        startBtn = new Button();
        startBtn.Text = "Обновить";
        startBtn.SetBounds(380, 10, 150, 30);
        startBtn.Click += (s, e) => RefreshList();
        Controls.Add(startBtn);

        info = new Label();
        info.SetBounds(270, 60, 260, 180);
        info.AutoSize = false;
        info.BorderStyle = BorderStyle.FixedSingle;
        Controls.Add(info);

        killBtn = new Button();
        killBtn.Text = "Завершить процесс";
        killBtn.SetBounds(270, 250, 260, 40);
        killBtn.Click += (s, e) => KillProcess();
        Controls.Add(killBtn);

        Button runApps = new Button();
        runApps.Text = "Запуск приложений";
        runApps.SetBounds(270, 300, 260, 40);
        runApps.Click += (s, e) => RunOtherApps();
        Controls.Add(runApps);

        timer = new Timer();
        timer.Interval = 3000;
        timer.Tick += (s, e) => RefreshList();

        intervalBox.ValueChanged += (s, e) =>
        {
            timer.Interval = (int)intervalBox.Value * 1000;
        };

        timer.Start();
        RefreshList();
    }

    void RefreshList()
    {
        list.Items.Clear();
        var procs = Process.GetProcesses().OrderBy(p => p.ProcessName);
        foreach (var p in procs) list.Items.Add(p.ProcessName);
    }

    void ShowInfo()
    {
        if (list.SelectedItem == null) return;
        string name = list.SelectedItem.ToString();
        var arr = Process.GetProcessesByName(name);
        if (arr.Length == 0) return;
        var p = arr[0];

        string text = "";
        text += "ID: " + p.Id + "\n";
        text += "Старт: " + SafeStart(p) + "\n";
        text += "CPU: " + SafeCpu(p) + "\n";
        text += "Потоки: " + p.Threads.Count + "\n";
        text += "Копий: " + arr.Length + "\n";

        info.Text = text;
    }

    string SafeStart(Process p)
    {
        try { return p.StartTime.ToString(); }
        catch { return "-"; }
    }

    string SafeCpu(Process p)
    {
        try { return p.TotalProcessorTime.ToString(); }
        catch { return "-"; }
    }

    void KillProcess()
    {
        if (list.SelectedItem == null) return;
        string name = list.SelectedItem.ToString();
        var arr = Process.GetProcessesByName(name);

        foreach (var p in arr)
        {
            try { p.Kill(); }
            catch { }
        }

        RefreshList();
        info.Text = "";
    }

    void RunOtherApps()
    {
        Form f = new Form();
        f.Width = 300;
        f.Height = 250;

        Button b1 = new Button();
        b1.Text = "Блокнот";
        b1.SetBounds(50, 20, 200, 30);
        b1.Click += (s, e) => Process.Start("notepad.exe");
        f.Controls.Add(b1);

        Button b2 = new Button();
        b2.Text = "Калькулятор";
        b2.SetBounds(50, 60, 200, 30);
        b2.Click += (s, e) => Process.Start("calc.exe");
        f.Controls.Add(b2);

        Button b3 = new Button();
        b3.Text = "Paint";
        b3.SetBounds(50, 100, 200, 30);
        b3.Click += (s, e) => Process.Start("mspaint.exe");
        f.Controls.Add(b3);

        Button b4 = new Button();
        b4.Text = "Другое";
        b4.SetBounds(50, 140, 200, 30);
        b4.Click += (s, e) =>
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
                Process.Start(d.FileName);
        };
        f.Controls.Add(b4);

        f.ShowDialog();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new Program());
    }
}