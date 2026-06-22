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
            var stackFrames = ParseFrames(exception.StackTrace);
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
                StackFrames = stackFrames,
                FilteredStackFrames = new ObservableCollection<StackFrameViewModel>(stackFrames),
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

        private static ObservableCollection<StackFrameViewModel> ParseFrames(string? stackTrace)
        {
            var frames = new ObservableCollection<StackFrameViewModel>();
            if (string.IsNullOrEmpty(stackTrace)) return frames;

            foreach (var parsed in StackTraceParser.Parse(stackTrace,
                (frame, type, method, paramList, parameters, file, lineNum) =>
                    new { Type = type, Method = method, ParameterList = paramList, File = file, Line = lineNum }))
            {
                var lastDot = parsed.Type.LastIndexOf('.');
                var hasFile = !string.IsNullOrEmpty(parsed.File);

                frames.Add(new StackFrameViewModel
                {
                    ParsedVisibility = Visibility.Visible,
                    Prefix = "   at ",
                    Namespace = lastDot >= 0 ? parsed.Type[..(lastDot + 1)] : string.Empty,
                    ClassName = lastDot >= 0 ? parsed.Type[(lastDot + 1)..] : parsed.Type,
                    MethodDot = ".",
                    MethodName = parsed.Method,
                    Parameters = parsed.ParameterList,
                    InPart = hasFile ? " in " : string.Empty,
                    FilePath = parsed.File,
                    FileLine = parsed.Line,
                    FileNavigationParam = hasFile ? $"{parsed.File}|{parsed.Line}" : string.Empty,
                    FileLinkVisibility = hasFile ? Visibility.Visible : Visibility.Collapsed,
                });
            }

            return frames;
        }
    }
}
