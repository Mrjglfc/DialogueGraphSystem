using System;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class EndNode : DialogueNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);
        }
    }
}
