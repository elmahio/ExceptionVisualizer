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
        public ExtensionEntrypoint()
        {
            System.Diagnostics.Debugger.Launch();
        }

        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "ExceptionVisualizer.79fc9a70-ae6e-4c14-8931-ee37a81e5bdf",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Microsoft",
                    displayName: "Exception Debugger Visualizer Extension"),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
        }
    }
}