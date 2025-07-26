using System;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The serializable data representing a runtime node in the visual novel graph that sets the background image.
    /// </summary>
    [Serializable]
    public class SetBackgroundRuntimeNode : DialogueRuntimeNode
    {
        public Sprite BackgroundSprite;
    }
}
