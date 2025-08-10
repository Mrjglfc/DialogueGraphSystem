using System.Collections.Generic;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Runtime
{
    public class DialogueRuntimeGraph : ScriptableObject
    {
        [SerializeReference]
        public List<DialogueRuntimeNode> Nodes = new();

        [SerializeReference]
        public Graph<DialogueRuntimeNode> Graph = new();
    }
}