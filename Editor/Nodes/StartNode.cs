using System;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class StartNode : DialogueNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort("Output").Build();
        }
    }
}
