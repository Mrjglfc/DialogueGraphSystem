using System;
using Unity.GraphToolkit.Editor;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [UseWithContext(typeof(PerformActionContextNode))]
    [Serializable]
    internal class GiveReputationBlockNode : DialogueBlockNode
    {
        public const string m_ReputationCount = "ReputationCount";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<int>(m_ReputationCount);
        }
    }
}
