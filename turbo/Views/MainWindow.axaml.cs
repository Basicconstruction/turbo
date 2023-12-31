using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using turbo.ViewModels;

namespace turbo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel Model => (MainWindowViewModel)this.DataContext!;
        private void OpenBrowser_OnClick(object? sender, RoutedEventArgs e)
        {
            var url = $"http://localhost:{Model.WebPort}"; // 替换为您要打开的链接
            try
            {
                Process.Start("explorer.exe", url); // 打开默认的浏览器并访问链接
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void StartWebProcess()
        {
            // Console.WriteLine(Model.RelativeRootPath);
            // 启动或重新启动 Web 进程
            var path = Path.GetDirectoryName(Model.RelativeRootPath)!;
            Model.WebProcess?.Kill(); // 终止当前进程
            try
            {
                Model.WebProcess = Process.Start($"{path}/turbo-server/turbo-pool.exe", $"{Model.WebPort} \"{path}/turbo-server/wwwroot\"");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //
            }
        }
        private void StartProxyProcess()
        {
            // 启动或重新启动代理进程
            Model.ProxyProcess?.Kill(); // 终止当前进程
            var path = Path.GetDirectoryName(Model.RelativeRootPath)!;

            try
            {
                Model.ProxyProcess = Process.Start($"{path}/turbo-server/turbo-proxy.exe", $"--port={Model.ProxyPort}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //
            }
        }

        private void WebServer_OnClick(object? sender, RoutedEventArgs e)
        {
            StartWebProcess();
        }

        private void ProxyServer_OnClick(object? sender, RoutedEventArgs e)
        {
            StartProxyProcess();
        }
    }
}