using System;
using UnityEngine.UI;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    [Serializable]
    public class ChoiceRuntimeNode : DialogueRuntimeNode
    {
        public Button[] choiceButtons;
        public string[] dialogueOptions;

        public ChoiceRuntimeNode(string[] dialogueChoices)
        {
            dialogueOptions = dialogueChoices;
        }
    }
}
