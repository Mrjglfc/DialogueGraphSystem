using System.Threading.Tasks;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The executor for the <see cref="PerformActionRuntimeNode"/> node.
    /// </summary>
    public class PerformActionExecutor : IDialogueNodeExecutor<PerformActionRuntimeNode>
    {
        readonly GiveMoneyExecutor moneyExecutor = new();
        readonly GiveReputationExecutor reputationExecutor = new();

        public async Task ExecuteAsync(PerformActionRuntimeNode performActionRuntimeNode, DialogueDirector ctx)
        {
            foreach(DialogueRuntimeNode node in performActionRuntimeNode.blockNodes)
            {
                switch(node)
                {
                    case GiveMoneyRuntimeBlockNode giveMoneyRuntimeBlockNode:
                        await moneyExecutor.ExecuteAsync(giveMoneyRuntimeBlockNode, ctx);
                        break;

                    case GiveReputationRuntimeBlockNode giveReputationRuntimeBlockNode:
                        await reputationExecutor.ExecuteAsync(giveReputationRuntimeBlockNode, ctx);
                        break;

                    default:
                        Debug.LogError($"No ContextNode executor found for node type: {node.GetType()}");
                        break;
                }
            }
            await ctx.InputProvider.InputDetected();
        }
    }
}
