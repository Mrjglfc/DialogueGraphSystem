using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The executor for the <see cref="PerformActionRuntimeNode"/> node.
    /// </summary>
    public class PerformActionExecutor : IDialogueNodeExecutor<PerformActionRuntimeNode>
    {
        /// <summary>
        /// Asynchronously waits for user input to be detected before proceeding with the execution of the visual novel graph.
        /// </summary>
        public async Task ExecuteAsync(PerformActionRuntimeNode performActionRuntimeNode, DialogueDirector ctx)
        {
            foreach(DialogueRuntimeNode node in performActionRuntimeNode.blockNodes)
            {
                //TODO: WE need to get the executor of the node somehow
            }
            await ctx.InputProvider.InputDetected();
        }
    }
}
