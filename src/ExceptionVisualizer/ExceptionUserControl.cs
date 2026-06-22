using ExceptionVisualizer.Models;
using ExceptionVisualizerSource;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.Extensibility.UI;
using Microsoft.VisualStudio.RpcContracts.OpenDocument;
using VsRange = Microsoft.VisualStudio.RpcContracts.Utilities.Range;
using System.Windows;

namespace ExceptionVisualizer
{
    internal class ExceptionUserControl(VisualizerTarget visualizerTarget, VisualStudioExtensibility extensibility)
        : RemoteUserControl(new ViewModel())
    {
        private readonly VisualizerTarget visualizerTarget = visualizerTarget;
        private readonly VisualStudioExtensibility extensibility = extensibility;

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
                        Subscribe(viewModel);
                        viewModel.IsSelected = true;
                        ViewModel.Exceptions.Add(viewModel);
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
                Subscribe(inner);
        }

        private void Exception_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is not ExceptionViewModel model) return;

            if (e.PropertyName == nameof(ExceptionViewModel.SelectedFrame)
                && model.SelectedFrame?.FileNavigationParam is string param
                && param.Length > 0)
            {
                _ = NavigateToFrame(param);
            }

            if (model.IsSelected)
                ViewModel.SelectedItem = model;
        }

        private async Task NavigateToFrame(string param)
        {
            var sep = param.IndexOf('|');
            var filePath = sep > 0 ? param[..sep] : param;
            _ = int.TryParse(sep > 0 ? param[(sep + 1)..] : null, out var lineNumber);
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                var uri = new Uri(filePath);
                var line0 = lineNumber > 0 ? lineNumber - 1 : 0;
                var range = new VsRange(line0, 0, line0, 0);
                var options = new OpenDocumentOptions(selection: range, ensureVisible: range, ensureVisibleOptions: null, isPreview: false, activate: null, logicalView: null, projectId: null, editorType: null);
                await extensibility.Documents().OpenDocumentAsync(uri, options, CancellationToken.None);
            }
            catch { }
        }
    }
}
