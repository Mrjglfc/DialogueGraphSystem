using System;
using Unity.GraphToolkit.Editor;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class ChoiceNode : DialogueNode
    {
        internal const string m_ChoiceCount = "ChoiceCount";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(m_ChoiceCount)
                .WithDisplayName("Port Count")
                .WithDefaultValue(2)
                .Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputExecutionPort(context);

            INodeOption portCountOption = GetNodeOptionByName(m_ChoiceCount);
            portCountOption.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<string>($"Dialogue {i}").Build();
                context.AddOutputPort($"Choice {i}").Build();
            }
        }
    }
}
