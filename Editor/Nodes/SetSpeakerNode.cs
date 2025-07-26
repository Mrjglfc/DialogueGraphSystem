using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Editor.Nodes
{
    [Serializable]
    internal class SetSpeakerNode : DialogueNode
    {
        public const string m_CharacterName = "CharacterName";
        public const string m_CharacterSprite = "CharacterSprite";
        public const string m_SpriteLocation = "SpriteLocation";
        public const string m_Dialogue = "Dialogue";

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<CharacterName>(m_CharacterName, "Character Name");
            context.AddNodeOption<Sprite>(m_CharacterSprite, "Character Sprite");
            context.AddNodeOption<CharacterSpritePosition>(m_SpriteLocation, "Sprite Location");
            context.AddNodeOption<string>(m_Dialogue, "Dialogue");

        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);
        }
    }

    public enum CharacterName
    {
        Bob, 
        Jim
    }

    public enum CharacterSpritePosition
    {
        Left,
        Right
    }
}
