using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The executor for the <see cref="ChoiceRuntimeNode"/> node.
    /// </summary>
    public class ChoiceExecutor : IDialogueNodeExecutor<ChoiceRuntimeNode>
    {
        public async Task ExecuteAsync(ChoiceRuntimeNode choiceRuntimeNode, DialogueDirector ctx)
        {
            await ctx.InputProvider.InputDetected();
        }
    }
}
