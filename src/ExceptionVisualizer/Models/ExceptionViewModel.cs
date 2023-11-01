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
        public string Message { get; internal set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

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
                return $"0x{HResult.ToString("X8")} ({FacilityToString((HResult & 0x7FFF0000) >> 16)} / {(HResult & 0xFFFF)})";
            }
        }

        [DataMember]
        public string HelpLink { get; internal set; }

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

        private static string FacilityToString(int facility)
        {
            switch (facility)
            {
                case 0: return "FACILITY_NULL";
                case 1: return "FACILITY_RPC";
                case 2: return "FACILITY_DISPATCH";
                case 3: return "FACILITY_STORAGE";
                case 4: return "FACILITY_ITF";
                case 7: return "FACILITY_WIN32";
                case 8: return "FACILITY_WINDOWS";
                case 9: return "FACILITY_SECURITY";
                case 10: return "FACILITY_CONTROL";
                case 11: return "FACILITY_CERT";
                case 12: return "FACILITY_INTERNET";
                case 13: return "FACILITY_MEDIASERVER";
                case 14: return "FACILITY_MSMQ";
                case 15: return "FACILITY_SETUPAPI";
                case 16: return "FACILITY_SCARD";
                case 17: return "FACILITY_COMPLUS";
                case 18: return "FACILITY_AAF";
                case 19: return "FACILITY_URT";
                case 20: return "FACILITY_ACS";
                case 21: return "FACILITY_DPLAY";
                case 22: return "FACILITY_UMI";
                case 23: return "FACILITY_SXS";
                case 24: return "FACILITY_WINDOWS_CE";
                case 25: return "FACILITY_HTTP";
                case 26: return "FACILITY_USERMODE_COMMONLOG";
                case 31: return "FACILITY_USERMODE_FILTER_MANAGER";
                case 32: return "FACILITY_BACKGROUNDCOPY";
                case 33: return "FACILITY_CONFIGURATION";
                case 34: return "FACILITY_STATE_MANAGEMENT";
                case 35: return "FACILITY_METADIRECTORY";
                case 36: return "FACILITY_WINDOWSUPDATE";
                case 37: return "FACILITY_DIRECTORYSERVICE";
                case 38: return "FACILITY_GRAPHICS";
                case 39: return "FACILITY_SHELL";
                case 40: return "FACILITY_TPM_SERVICES";
                case 41: return "FACILITY_TPM_SOFTWARE";
                case 48: return "FACILITY_PLA";
                case 49: return "FACILITY_FVE";
                case 50: return "FACILITY_FWP";
                case 51: return "FACILITY_WINRM";
                case 52: return "FACILITY_NDIS";
                case 53: return "FACILITY_USERMODE_HYPERVISOR";
                case 54: return "FACILITY_CMI";
                case 55: return "FACILITY_USERMODE_VIRTUALIZATION";
                case 56: return "FACILITY_USERMODE_VOLMGR";
                case 57: return "FACILITY_BCD";
                case 58: return "FACILITY_USERMODE_VHD";
                case 60: return "FACILITY_SDIAG";
                case 61: return "FACILITY_WEBSERVICES";
                case 80: return "FACILITY_WINDOWS_DEFENDER";
                case 81: return "FACILITY_OPC";
            }

            return "" + facility;
        }
    }
}
