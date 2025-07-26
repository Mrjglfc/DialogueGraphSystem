using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class ChoiceNode : DialogueNode
    {
        const string m_ChoiceCount = "ChoiceCount";

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption(m_ChoiceCount, "Port Count", defaultValue: 2, attributes: new[] { new DelayedAttribute() });
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("result").Build();

            INodeOption portCountOption = GetNodeOptionByName(m_ChoiceCount);
            portCountOption.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddOutputPort($"{i}").Build();
            }
        }
    }
}
