using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace ExceptionVisualizer
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "ExceptionVisualizerVsix.59bb613d-3a6f-4cc0-90ae-e31743f8efde",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "elmahio",
                    displayName: "Exception Visualizer",
                    description: "Show a debugger visualizer for exceptions in Visual Studio.")
            {
                Tags = ["visualizer", "exception", "debug", "debugging"],
                Icon = "icon.png",
            }
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            Telemetry.Initialize(ExtensionAssemblyVersion?.ToString());
            try
            {
                base.InitializeServices(serviceCollection);
            }
            catch (Exception ex)
            {
                Telemetry.TrackException(ex);
                throw;
            }

            // You can configure dependency injection here by adding services to the serviceCollection.
        }
    }
}