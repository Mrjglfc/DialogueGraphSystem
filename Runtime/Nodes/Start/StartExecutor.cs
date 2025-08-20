using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// The executor for the <see cref="StartRuntimeNode"/> node.
    /// </summary>
    public class StartExecutor : IDialogueNodeExecutor<StartRuntimeNode>
    {
        public async Task ExecuteAsync(StartRuntimeNode _, DialogueDirector ctx)
        {
            await Task.Yield();
        }
    }
}
