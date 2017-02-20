using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using AutoMapper;
using GlucoseNotifier.Services.Implementation;
using GlucoseNotifier.Services.Models.DexcomApi.Requests;
using GlucoseNotifier.Services.Models.DexcomApi.Responses;
using GlucoseNotifier.Services.ViewModels;

namespace GlucoseNotifier.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _sessionId;
        private bool _isExit;
        NotifyIcon _notifyIcon;
        private DexcomShareService _dexcomShareService;
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(60) };
            timer.Tick += timer_Tick;
            timer.Start();
            _notifyIcon = new NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = Properties.Resources._1487629902_13;
            _notifyIcon.Visible = true;

            CreateContextMenu();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            WorkFlow();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();


            //_notifyIcon.
        }

        private async void WorkFlow()
        {
            if (_dexcomShareService == null)
                _dexcomShareService = new DexcomShareService();

            _sessionId = await _dexcomShareService.Authorize(new LoginRequest() { AccountName = "", Password = "" });
            List<BGResponse> listBg = await _dexcomShareService.GetLatestBg(new GlucoseRequest() { SessionId = _sessionId });

            if (listBg == null)
            {
                _sessionId = await _dexcomShareService.Authorize(new LoginRequest() { AccountName = "", Password = "" });
                listBg = await _dexcomShareService.GetLatestBg(new GlucoseRequest() { SessionId = _sessionId });
            }

            var vm = Mapper.Map<IEnumerable<BGResponse>, IEnumerable<BGViewModel>>(listBg).ToList();
            DateTime startTime = vm[0].SugarTime;
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            if (span.Minutes > 4)
                UpdateNotificationIcon(vm[0].CurrentSugar, span.Minutes);


            _notifyIcon.Text = $@"{vm[0].CurrentSugar} mg/dl, last update {span.Minutes} ago";
            timePassed.Content = $"{span.Minutes} minutes ago";
            currentBg.Content = vm[0].CurrentSugar;
        }

        private void UpdateNotificationIcon(int currSugar, int passedMinutes)
        {
            _notifyIcon.BalloonTipTitle = @"Current blood sugar";
            _notifyIcon.BalloonTipText = $@"{currSugar} mg/dl";
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.ShowBalloonTip(10);

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            WorkFlow();
        }
        private void ShowMainWindow()
        {
            if (IsVisible)
            {
                if (WindowState == WindowState.Minimized)
                    WindowState = WindowState.Normal;
                Activate();
            }
            else
                Show();

        }
        private void ExitApplication()
        {
            _isExit = true;
            Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!_isExit)
            {
                e.Cancel = true;
                Hide(); // A hidden window can be shown again, a closed one not
            }
        }
    }
}
