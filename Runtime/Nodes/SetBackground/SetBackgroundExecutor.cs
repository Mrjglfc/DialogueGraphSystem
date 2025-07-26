using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    /// <summary>
    /// Executor for the <see cref="SetBackgroundRuntimeNode"/> node.
    /// </summary>
    public class SetBackgroundExecutor : IDialogueNodeExecutor<SetBackgroundRuntimeNode>
    {
        /// <summary>
        /// Sets the background image of the visual novel director context to the specified sprite.
        /// </summary>
        public async Task ExecuteAsync(SetBackgroundRuntimeNode runtimeNode, DialogueDirector ctx)
        {
            ctx.BackgroundImage.sprite = runtimeNode.BackgroundSprite;
            await Task.Yield();
        }
    }
}
