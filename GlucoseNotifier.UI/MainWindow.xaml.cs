using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        public string sessionId;
        private DexcomShareService _dexcomShareService;
        public MainWindow()
        {
            InitializeComponent();
            _dexcomShareService = new DexcomShareService();
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            WorkFlow();
        }

        private async void WorkFlow()
        {
            sessionId = await _dexcomShareService.Authorize(new LoginRequest() { AccountName = "", Password = "" });
            List<BGResponse> listBg = await _dexcomShareService.GetLatestBg(new GlucoseRequest() { SessionId = sessionId });

            if (listBg == null)
            {
                sessionId = await _dexcomShareService.Authorize(new LoginRequest() { AccountName = "", Password = "" });
                listBg = await _dexcomShareService.GetLatestBg(new GlucoseRequest() { SessionId = sessionId });
            }

            var vm = Mapper.Map<IEnumerable<BGResponse>, IEnumerable<BGViewModel>>(listBg).ToList();
            currentBg.Content = vm[0].CurrentSugar;
            time.Content = vm[0].SugarTime.ToLongTimeString();
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            //WorkFlow();
        }
    }
}
