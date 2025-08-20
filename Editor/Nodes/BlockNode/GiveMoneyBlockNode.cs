using System;
using Unity.GraphToolkit.Editor;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [UseWithContext(typeof(PerformActionContextNode))]
    [Serializable]
    internal class GiveMoneyBlockNode : DialogueBlockNode
    {
        public const string m_MoneyCount = "MoneyCount";
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<int>(m_MoneyCount);
        }
    }
}
