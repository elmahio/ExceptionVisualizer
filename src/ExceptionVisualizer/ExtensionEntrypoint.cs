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
                    id: "ExceptionVisualizer.79fc9a70-ae6e-4c14-8931-ee37a81e5bdf",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "elmahio",
                    displayName: "Exception Debugger Visualizer",
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