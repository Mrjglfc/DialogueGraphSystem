using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The executor for the <see cref="GiveReputationRuntimeBlockNode"/> node.
    /// </summary>
    public class GiveReputationExecutor : IDialogueNodeExecutor<GiveReputationRuntimeBlockNode>
    {
        public async Task ExecuteAsync(GiveReputationRuntimeBlockNode giveReputation, DialogueDirector ctx)
        {
            // Give Reputation
            await Task.CompletedTask;
        }
    }
}
