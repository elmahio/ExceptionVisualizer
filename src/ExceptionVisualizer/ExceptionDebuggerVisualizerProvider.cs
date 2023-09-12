using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using ExceptionVisualizerSource;

namespace ExceptionVisualizer
{
    /// <summary>
    /// Debugger visualizer provider class for <see cref="System.String"/>.
    /// </summary>
    [VisualStudioContribution]
    internal class ExceptionDebuggerVisualizerProvider : DebuggerVisualizerProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDebuggerVisualizerProvider"/> class.
        /// </summary>
        /// <param name="extension">Extension instance.</param>
        /// <param name="extensibility">Extensibility object.</param>
        public ExceptionDebuggerVisualizerProvider(ExtensionEntrypoint extension, VisualStudioExtensibility extensibility)
            : base(extension, extensibility)
        {
        }

        /// <inheritdoc/>
        public override DebuggerVisualizerProviderConfiguration DebuggerVisualizerProviderConfiguration => new("Exception Visualizer", typeof(Exception))
        {
            VisualizerObjectSourceType = new(typeof(ExceptionModelSource)),
        };

        /// <inheritdoc/>
        public override Task<IRemoteUserControl> CreateVisualizerAsync(VisualizerTarget visualizerTarget, CancellationToken cancellationToken)
        {
            return Task.FromResult<IRemoteUserControl>(new ExceptionUserControl(visualizerTarget));
        }
    }
}
