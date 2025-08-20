using System;
using System.Collections.Generic;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    [Serializable]
    public class PerformActionRuntimeNode : DialogueRuntimeNode
    {
        public List<DialogueRuntimeNode> blockNodes;

        public PerformActionRuntimeNode(List<DialogueRuntimeNode> blockNodes)
        {
            this.blockNodes = blockNodes;
        }
    }
}
