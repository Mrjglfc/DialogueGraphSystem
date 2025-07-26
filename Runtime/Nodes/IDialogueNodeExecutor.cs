using System.Threading.Tasks;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    public interface IDialogueNodeExecutor<in TNode> where TNode : DialogueRuntimeNode
    {
        Task ExecuteAsync(TNode node, DialogueDirector ctx);
    }
}
