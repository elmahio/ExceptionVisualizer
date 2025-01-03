﻿using Microsoft.VisualStudio.Extensibility;
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
    }
}
