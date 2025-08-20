using System;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    [Serializable]
    public class PerformActionRuntimeNode : DialogueRuntimeNode
    {
        public bool IsMoneyEnabled;
        public bool IsReputationEnabled;

        public int MoneyAmount;
        public int ReputationAmount;

        public PerformActionRuntimeNode(bool isMoneyEnabled, bool isRepEnabled, int moneyAmount, int repAmount)
        {
            IsMoneyEnabled = isMoneyEnabled;
            IsReputationEnabled = isRepEnabled;
            MoneyAmount = moneyAmount;
            ReputationAmount = repAmount;
        }
    }
}
