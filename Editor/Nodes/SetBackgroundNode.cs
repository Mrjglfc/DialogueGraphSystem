using System;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class SetBackgroundNode : DialogueNode
    {
        public const string m_BackgroundName = "Background";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);

            context.AddInputPort<Sprite>(m_BackgroundName);
        }
    }
}
