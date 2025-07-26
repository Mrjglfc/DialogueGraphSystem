using System;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    [Serializable]
    public class SetDialogueRuntimeNode : DialogueRuntimeNode
    {
        public string ActorName;
        public Sprite ActorSprite;
        public CharacterSpritePosition SpritePosition;
        public string DialogueText;
        public int LocationIndex;
    }
}