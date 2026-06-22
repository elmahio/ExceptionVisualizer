using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows;

namespace ExceptionVisualizer.Models
{
    [DataContract]
    public class ExceptionViewModel : INotifyPropertyChanged
    {
        [DataMember]
        public IAsyncCommand Navigate { get; set; } = new NavigateCommand();

        [DataMember]
        public IAsyncCommand Copy { get; set; } = new CopyCommand();

        private StackFrameViewModel? selectedFrame;

        [DataMember]
        public StackFrameViewModel? SelectedFrame
        {
            get => selectedFrame;
            set
            {
                selectedFrame = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFrame)));
            }
        }

        [DataMember]
        public string Message { get; internal set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public string FormattedStackTrace => $"{Type}: {Message}\r\n{StackTrace}";

        [DataMember]
        public ObservableCollection<StackFrameViewModel> StackFrames { get; set; } = new ObservableCollection<StackFrameViewModel>();

        private ObservableCollection<StackFrameViewModel> filteredStackFrames = new();

        [DataMember]
        public ObservableCollection<StackFrameViewModel> FilteredStackFrames
        {
            get => filteredStackFrames;
            set
            {
                filteredStackFrames = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilteredStackFrames)));
            }
        }

        private bool myCodeOnly;

        [DataMember]
        public bool MyCodeOnly
        {
            get => myCodeOnly;
            set
            {
                myCodeOnly = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyCodeOnly)));
            }
        }

        public void ApplyFilter()
        {
            FilteredStackFrames = myCodeOnly
                ? new ObservableCollection<StackFrameViewModel>(StackFrames.Where(f => f.FileLinkVisibility == Visibility.Visible))
                : new ObservableCollection<StackFrameViewModel>(StackFrames);
        }

        [DataMember]
        public string Demystified { get; set; }

        [DataMember]
        public ObservableCollection<DataViewModel> Data { get; set; } = new ObservableCollection<DataViewModel>();

        [DataMember]
        public ObservableCollection<DataViewModel> Properties { get; set; } = new ObservableCollection<DataViewModel>();

        [DataMember]
        public Visibility ShowData { get; set; } = Visibility.Collapsed;

        [DataMember]
        public Visibility ShowProperties { get; set; } = Visibility.Collapsed;

        [DataMember]
        public ObservableCollection<ExceptionViewModel> InnerExceptions { get; set; } = new ObservableCollection<ExceptionViewModel>();

        [DataMember]
        public string Source { get; internal set; }

        [DataMember]
        public string TargetSite { get; internal set; }

        [DataMember]
        public int HResult { get; internal set; }

        [DataMember]
        public string HResultDisplay
        {
            get
            {
                return $"0x{HResult:X8}";
            }
        }

        [DataMember]
        public string? HResultFacility
        {
            get
            {
                var hResult = Elmah.Io.HResults.HResult.Parse(HResult);
                return hResult.Facility.Name;
            }
        }

        [DataMember]
        public string? HResultCode
        {
            get
            {
                var hResult = Elmah.Io.HResults.HResult.Parse(HResult);
                return hResult.Code.Name;
            }
        }

        [DataMember]
        public string HelpLink { get; internal set; }

        private int selectedTabIndex;

        [DataMember]
        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                selectedTabIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTabIndex)));
            }
        }

        private bool isSelected;

        [DataMember]
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private class CopyCommand : IAsyncCommand
        {
            public bool CanExecute => true;

            public Task ExecuteAsync(object? parameter, IClientContext clientContext, CancellationToken cancellationToken)
            {
                var text = parameter as string ?? string.Empty;
                if (string.IsNullOrEmpty(text)) return Task.CompletedTask;
                var thread = new Thread(() => System.Windows.Clipboard.SetText(text));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                return Task.CompletedTask;
            }
        }

        private class NavigateCommand : IAsyncCommand
        {
            public bool CanExecute => true;

            public Task ExecuteAsync(object? parameter, IClientContext clientContext, CancellationToken cancellationToken)
            {
                var hyperlink = parameter as string;
                if (!string.IsNullOrWhiteSpace(hyperlink) && Uri.TryCreate(hyperlink, UriKind.Absolute, out var url))
                {
                    Process.Start(new ProcessStartInfo { FileName = url.AbsoluteUri, UseShellExecute = true });
                }

                return Task.CompletedTask;
            }
        }
    }
}
