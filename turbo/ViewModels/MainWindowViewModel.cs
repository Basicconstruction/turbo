using ReactiveUI;
using System;
using System.Diagnostics;
using System.Drawing.Printing;

namespace turbo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static
        private int _webPort = 8887;
        public string RelativeRootPath = Environment.ProcessPath!;
        public int WebPort
        {
            get => _webPort;
            set => this.RaiseAndSetIfChanged(ref _webPort,value);
        }
        private int _proxyPort = 8888;
        public int ProxyPort
        {
            get => _proxyPort;
            set => this.RaiseAndSetIfChanged(ref _proxyPort,value);
        }

        private Process? _webProcess;

        public Process? WebProcess
        {
            get => _webProcess;
            set => this.RaiseAndSetIfChanged(ref _webProcess, value);
        }
        private Process? _proxyProcess;
        
        public Process? ProxyProcess
        {
            get => _proxyProcess;
            set => this.RaiseAndSetIfChanged(ref _proxyProcess, value);
        }
        
        public string CombinedString => $"http://localhost:{WebPort}";
        

        public MainWindowViewModel()
        {
            this.WhenAnyValue(d => d.WebPort).Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(CombinedString));
            });
        }
    }
}
