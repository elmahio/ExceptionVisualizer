using ExceptionVisualizer.Models;
using ExceptionVisualizerSource;
using System.Collections.ObjectModel;
using System.Windows;

namespace ExceptionVisualizer
{
    internal static class ExceptionModelExtensions
    {
        internal static ExceptionViewModel ToViewModel(this ExceptionModel exception)
        {
            var viewModel = new ExceptionViewModel
            {
                Data = new ObservableCollection<DataViewModel>(exception.Data.Select(d => new DataViewModel
                {
                    Key = d.Key,
                    Value = d.Value,
                })),
                Properties = new ObservableCollection<DataViewModel>(exception.Properties.Select(d => new DataViewModel
                {
                    Key = d.Key,
                    Value = d.Value,
                })),
                ShowData = exception.Data.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                ShowProperties = exception.Properties.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                HelpLink = exception.HelpLink,
                HResult = exception.HResult,
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                TargetSite = exception.TargetSite,
                Demystified = exception.Demystified,
                @Type = exception.Type,
            };
            foreach (var inner in exception.InnerExceptions)
            {
                viewModel.InnerExceptions.Add(ToViewModel(inner));
            }

            return viewModel;
        }
    }
}
