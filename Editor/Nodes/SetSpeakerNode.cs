using Mrjglfc.DialogueGraphSystem.Runtime;
using Mrjglfc.DialogueGraphSystem.Runtime.ScriptableObjects;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class SetSpeakerNode : DialogueNode
    {
        public const string m_Character = "Character";
        public const string m_SpriteLocation = "SpriteLocation";
        public const string m_Dialogue = "Dialogue";
        public const string m_Expression = "Expression";

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<CharacterSO>(m_Character, "Character");
            context.AddNodeOption<CharacterSpritePosition>(m_SpriteLocation, "Sprite Location");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);
            context.AddInputPort<string>(m_Dialogue);
            context.AddInputPort<Sprite>(m_Expression);
        }
    }
}
