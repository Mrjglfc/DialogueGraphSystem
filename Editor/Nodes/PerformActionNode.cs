using System;
using Unity.GraphToolkit.Editor;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class PerformActionNode : DialogueNode
    {
        public const string m_MoneyCheckbox = "MoneyCheckbox";
        public const string m_ReputationCheckbox = "ReputationCheckbox";

        public const string m_MoneyCount = "MoneyCount";
        public const string m_ReputationCount = "ReputationCount";

        // Called before OnDefinePorts
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption(m_MoneyCheckbox, "Give Money", defaultValue: false);
            context.AddNodeOption(m_ReputationCheckbox, "Give Reputation", defaultValue: false);
        }

        // Called after OnDefineOptions
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);

            INodeOption moneyCheckboxOption = GetNodeOptionByName(m_MoneyCheckbox);
            INodeOption reputationOption = GetNodeOptionByName(m_ReputationCheckbox);

            moneyCheckboxOption.TryGetValue(out bool money);
            reputationOption.TryGetValue(out bool reputation);

            if (money)
            {
                context.AddInputPort<int>(m_MoneyCount);
            }

            if (reputation)
            {
                context.AddInputPort<int>(m_ReputationCount);
            }
        }
    }
}
