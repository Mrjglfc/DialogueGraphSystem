using System;
using System.Collections.Generic;

namespace Mrjglfc.DialogueGraphSystem.Runtime
{
    [Serializable]
    public abstract class DialogueRuntimeNode
    {
        public List<int> NextNodeIndices = new();
    }
}