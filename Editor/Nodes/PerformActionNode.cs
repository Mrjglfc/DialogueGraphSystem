using Mrjglfc.DialogueGraphSystem.Runtime;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class PerformActionNode : DialogueNode
    {
        const string m_ActionType= "ActionType";
        const string m_MoneyCount = "MoneyCount";
        const string m_ReputationCount = "ReputationCount";

        // Called before OnDefinePorts
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption(m_ActionType, defaultValue: ActionType.Money);
        }

        // Called after OnDefineOptions
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);

            INodeOption actionTypeOption = GetNodeOptionByName(m_ActionType);

            if (actionTypeOption is null)
            {
                Debug.LogError($"A NodeOption named: {m_ActionType} is null");
                return;
            }

            actionTypeOption.TryGetValue(out ActionType actionType);

            switch (actionType)
            {
                case ActionType.Money:
                    context.AddInputPort<int>(m_MoneyCount);
                    break;

                case ActionType.Reputation:
                    context.AddInputPort<int>(m_ReputationCount);
                    break;
            }
        }

        
    }
}
