using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.Windows.Threading;

namespace Client
{
 
    public partial class PlayWindow : Window
    {
        
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

            if (hwndSource != null)
            {
                hwndSource.AddHook(HwndSourceHook);
            }

        }

        private bool allowClosing = false;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;

        private const uint SC_CLOSE = 0xF060;

        private const int WM_SHOWWINDOW = 0x00000018;
        private const int WM_CLOSE = 0x10;

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SHOWWINDOW:
                    {
                        IntPtr hMenu = GetSystemMenu(hwnd, false);
                        if (hMenu != IntPtr.Zero)
                        {
                            EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                        }
                    }
                    break;
                case WM_CLOSE:
                    if (!allowClosing)
                    {
                        handled = true;
                    }
                    break;
            }
            return IntPtr.Zero;
        }


        private void Window1_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
        //************************************************************************************************************************
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer_data = new DispatcherTimer();
        DispatcherTimer timer_command = new DispatcherTimer();
        Process process = new Process();
        static sql_request sql = new sql_request();
        MainWindow main = new MainWindow();

        Errors err = new Errors();

        public static double price = sql.get_price();
        public double tarif = price / 60;

        double balance_now = 0;
        
        public PlayWindow()
        {
            InitializeComponent();
            this.SourceInitialized += Window1_SourceInitialized;
            this.DataContext = new MyViewModel();


            balance_now = Convert.ToDouble(sql.get_balance(File.ReadAllText("config.txt")));
            balance.Content = "Баланс: " + Convert.ToString(Math.Round(balance_now, 2));
            Mins.Content = Convert.ToString(Math.Round(balance_now / tarif, 0)) + " мин";


            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += check_balance;
            timer.Start();

            timer_data.Interval = TimeSpan.FromSeconds(1);
            timer_data.Tick += data;
            timer_data.Start();

            timer_command.Interval = TimeSpan.FromSeconds(3);
            timer_command.Tick += command;
            timer_command.Start();            
        }

        //main.cpl - mouse
        //control mmsys.cpl sounds - sound
       
        class MyViewModel
        {
            public string cabinet
            {
                get { return sql.get_logo("cabinet"); }
            }
            public string csgo
            {
                get { return sql.get_logo("csgo"); }
            }
            public string browser
            {
                get { return sql.get_logo("browser"); }
            }
            public string discord
            {
                get { return sql.get_logo("discord"); }
            }
            public string dota2
            {
                get { return sql.get_logo("dota2"); }
            }
            public string pubg
            {
                get { return sql.get_logo("pubg"); }
            }
            public string faceit
            {
                get { return sql.get_logo("faceit"); }
            }
            public string lol
            {
                get { return sql.get_logo("lol"); }
            }
            public string warface
            {
                get { return sql.get_logo("warface"); }
            }
            public string steam
            {
                get { return sql.get_logo("steam"); }
            }
            public string origin
            {
                get { return sql.get_logo("origin"); }
            }
            public string battlenet
            {
                get { return sql.get_logo("battlenet"); }
            }
            public string wot
            {
                get { return sql.get_logo("wot"); }
            }
            public string wargaming
            {
                get { return sql.get_logo("wargaming"); }
            }
            public string ts3
            {
                get { return sql.get_logo("ts3"); }
            }
            public string epicgames
            {
                get { return sql.get_logo("epicgames"); }
            }
            public string mouse
            {
                get { return sql.get_logo("mouse"); }
            }
            public string sound
            {
                get { return sql.get_logo("sound"); }
            }
            public string nvidia
            {
                get { return sql.get_logo("nvidia"); }
            }
            public string _out
            {
                get { return sql.get_logo("out"); }
            }
        }
        public void command(object sender, EventArgs e)
        {

            if(sql.get_command() == "taskmgr.exe")
            {
                
                if (sql.set_command())
                {
                    Process.Start("taskmgr.exe");
                }
            }

            if (sql.get_command() == "Explorer")
            {

                if (sql.set_command())
                {
                    Process.Start("Explorer");
                }
            }
            if (sql.get_command() == "Shutdown /s /t 0") // выклбючить пк
            {

                if (sql.set_command())
                {
                    Process.Start("shutdown", "/s /t 0");
                }
            }

            if (sql.get_command() == "Shutdown /r /t 0") // перезапустить
            {

                if (sql.set_command())
                {
                    Process.Start("shutdown", "/r /t 0");
                }
            }

        }
        public void data(object sender, EventArgs e)
        {

            Data.Content = DateTime.Now;
        }
        public void check_balance(object sender, EventArgs e)
        {
            try
            {
                balance_now = Convert.ToDouble(sql.get_balance(File.ReadAllText("config.txt")));

                balance_now = balance_now - tarif;
                   

                if (balance_now < tarif || balance_now == 0)
                {
                    Errors.Error = "Недостаточно денег на балансе!";
                    Message mess = new Message();
                    mess.Show();
                    /*timer.Stop();
                    timer_data.Stop();
                    timer_command.Stop();*/
                    File.WriteAllText("config.txt", "");
                    main.Show();
                    Hide();

                }

                sql.set_balance(File.ReadAllText("config.txt"), balance_now);

                Mins.Content = Convert.ToString(Math.Round(balance_now / tarif, 0)) + " мин";
                balance.Content = "Баланс: " + Convert.ToString(Math.Round(balance_now,2));

                if (Convert.ToInt32(Mins.Content) == 15)
                {
                    Errors.Error = "У вас осталось мало времени!";
                    Message mess = new Message();
                    mess.Show();
                }
                
            }
            catch
            {

            }
            
        }
        
        public void run_procces(string path)
        {
            try
            {
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = "-n";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
           }
           catch
           {
               Errors.Error = "Неправильно указан путь!\nОбратитесь к администратору!";
               Message mess = new Message();
               mess.Show();
               // MessageBox.Show("Неправильно указан путь!\nОбратитесь к администратору!");
           }
           
        }

        private void Button_Click_Browser(object sender, RoutedEventArgs e)
        {
 
            run_procces(sql.get_path("browser"));
        }

        private void Button_Click_Discord(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("discord"));
        }

        private void Button_Click_Dota(object sender, RoutedEventArgs e)
        {

            run_procces(sql.get_path("dota2"));
        }

      
        private void Button_Click_CSGO(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("csgo"));
        }

        private void Button_Click_PUBG(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("pubg"));
        }

        private void Button_Click_Faceit(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("faceit"));
        }

        private void Button_Click_LOL(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("lol"));
        }

        private void Button_Click_Warface(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("warface"));
        }

        private void Button_Click_OUT(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            File.WriteAllText("config.txt", "");
            main.Show();
            Hide();
            //Process.Start("shutdown", "/r /t 0");
            // Environment.Exit(0);
        }

        private void Button_Click_Mouse(object sender, RoutedEventArgs e)
        {
            Process.Start("main.cpl");
        }

        private void Button_Click_Sound(object sender, RoutedEventArgs e)
        {
            Process.Start("mmsys.cpl");
        }

        private void Button_Click_Nvidia(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("nvidia"));
        }

        private void Button_Click_Cabinet(object sender, RoutedEventArgs e)
        {
            cabinet cab = new cabinet();
            cab.Show();
        }

        private void Button_Click_Steam(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("steam"));
        }

        private void Button_Click_BattleNet(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("battlenet"));
        }

        private void Button_Click_EpicGames(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("epicgames"));
        }

        private void Button_Click_WarGaming(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("wargaming"));
        }

        private void Button_Click_TS3(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("ts3"));
        }

        private void Button_Click_Origin(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("origin"));
        }

        private void Button_Click_Wot(object sender, RoutedEventArgs e)
        {
            run_procces(sql.get_path("wot"));
        }
    }
}
