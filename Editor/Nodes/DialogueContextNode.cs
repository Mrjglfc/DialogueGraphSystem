using System;
using Unity.GraphToolkit.Editor;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    /// <summary>
    /// Dialogue Base Node model.
    /// </summary>
    /// <remarks> It is best practice to group all the nodes of a tool under a base node. It improves organization,
    /// scalability, maintenance and debugging.</remarks>
    [Serializable]
    internal abstract class DialogueContextNode : ContextNode
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";

        /// <summary>
        /// Defines common input and output execution ports for all nodes in the Visual Novel Director tool.
        /// </summary>
        /// <param name="scope">The scope to define the node.</param>
        protected void AddInputOutputExecutionPorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);
            AddOutputExecutionPort(context);
        }

        protected static void AddOutputExecutionPort(IPortDefinitionContext context)
        {
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                   .WithDisplayName(string.Empty)
                   .WithConnectorUI(PortConnectorUI.Arrowhead)
                   .Build();
        }

        protected static void AddInputExecutionPort(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                   .WithDisplayName(string.Empty)
                   .WithConnectorUI(PortConnectorUI.Arrowhead)
                   .Build();
        }
    }
}
