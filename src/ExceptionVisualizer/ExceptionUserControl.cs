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
                ExceptionModel? exception = await this.visualizerTarget.ObjectSource.RequestDataAsync<ExceptionModel?>(jsonSerializer: null, CancellationToken.None);
                if (exception != null)
                {
                    var viewModel = ToViewModel(exception);
                    ViewModel.Exceptions.Add(viewModel);
                    Subscribe(viewModel);
                    viewModel.IsSelected = true;
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
                ShowData = exception.Data.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                HelpLink = exception.HelpLink,
                HResult = exception.HResult,
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                TargetSite = exception.TargetSite,
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
