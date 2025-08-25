using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The executor for the <see cref="GiveMoneyRuntimeBlockNode"/> node.
    /// </summary>
    public class GiveMoneyExecutor : IDialogueNodeExecutor<GiveMoneyRuntimeBlockNode>
    {
        /// <summary>
        /// Asynchronously waits for user input to be detected before proceeding with the execution of the visual novel graph.
        /// </summary>
        public async Task ExecuteAsync(GiveMoneyRuntimeBlockNode _, DialogueDirector ctx)
        {
            await ctx.InputProvider.InputDetected();
        }
    }
}

