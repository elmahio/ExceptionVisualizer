using ExceptionVisualizer.Models;
using ExceptionVisualizerSource;
using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Collections.ObjectModel;
using System.Windows;

namespace ExceptionVisualizer
{
    internal class ExceptionUserControl(VisualizerTarget visualizerTarget) : RemoteUserControl(new ViewModel())
    {
        private readonly VisualizerTarget visualizerTarget = visualizerTarget;

        private ViewModel ViewModel => (ViewModel)DataContext!;


        public override Task ControlLoadedAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    ExceptionModel? exception = await this.visualizerTarget.ObjectSource.RequestDataAsync<ExceptionModel?>(jsonSerializer: null, CancellationToken.None);
                    if (exception != null)
                    {
                        var viewModel = exception.ToViewModel();
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
            }, cancellationToken);
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

        private void Exception_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is not ExceptionViewModel model) return;

            if (model.IsSelected)
            {
                ViewModel.SelectedItem = model;
            }
        }
    }
}
