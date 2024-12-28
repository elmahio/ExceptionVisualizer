using ExceptionVisualizer.Models;
using ExceptionVisualizerSource;
using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Collections.ObjectModel;
using System.Windows;

namespace ExceptionVisualizer
{
    internal class ExceptionUserControl : RemoteUserControl
    {
        private readonly VisualizerTarget visualizerTarget;

        public ExceptionUserControl(VisualizerTarget visualizerTarget) : base(new ViewModel())
        {
            this.visualizerTarget = visualizerTarget;
        }

        private ViewModel ViewModel => (ViewModel)this.DataContext!;


        public override Task ControlLoadedAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    ExceptionModel? exception = await this.visualizerTarget.ObjectSource.RequestDataAsync<ExceptionModel?>(jsonSerializer: null, CancellationToken.None);
                    if (exception != null)
                    {
                        var viewModel = ToViewModel(exception);
                        ViewModel.Exceptions.Add(viewModel);
                        Subscribe(viewModel);
                        viewModel.IsSelected = true;
                    }
                }
                catch (StreamJsonRpc.RemoteInvocationException rie) when (rie.Message.Contains("Could not load file or assembly 'Newtonsoft.Json"))
                {
                    MessageBox.Show("There's currently a bug in Visual Studio causing the Exception Visualizer extension to crash when debugging projects containing a reference to Newtonsoft.Json older than version 13. Either upgrade to version 13 of Newtonsoft.Json or follow this issue on GitHub: https://github.com/microsoft/VSExtensibility/issues/248.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ExceptionVisualizer failed with exception:\n{ex}");
                    Telemetry.TrackException(ex);
                }
            });
            return Task.CompletedTask;
        }

        private void Subscribe(ExceptionViewModel model)
        {
            model.PropertyChanged += Exception_PropertyChanged;
            foreach (var inner in model.InnerExceptions)
            {
                Subscribe(inner);
            }
        }

        private ExceptionViewModel ToViewModel(ExceptionModel exception)
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

        private void Exception_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var model = sender as ExceptionViewModel;
            if (model == null) return;

            if (model.IsSelected)
            {
                ViewModel.SelectedItem = model;
            }
        }
    }
}
