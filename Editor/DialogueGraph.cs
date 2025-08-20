using Mrjglfc.DialogueGraphSystem.Editor.Nodes;
using System;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Mrjglfc.DialogueGraphSystem.Editor
{
    [Graph(AssetExtension)]
    [Serializable]
    public class DialogueGraph : Graph
    {
        public const string AssetExtension = "dgs";

        [MenuItem("Assets/Create/DialogueGraphSystem/Dialogue Graph", false)]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
        }

        public override void OnGraphChanged(GraphLogger infos)
        {
            base.OnGraphChanged(infos);

            DetectStartNodeErrors(infos);
            DetectEndNodeErrors(infos);
        }

        private void DetectStartNodeErrors(GraphLogger infos)
        {
            int startNodesCount = GetNodes().OfType<StartNode>().Count();
            switch (startNodesCount)
            {
                case 0:
                    infos.LogError("Add a StartNode in your DialogueGraph.", this);
                    break;
                case > 1:
                    {
                        infos.LogWarning($"DialogueGraph only supports one StartNode per graph. Only the first created one will be used.");
                        break;
                    }
            }
        }

        private void DetectEndNodeErrors(GraphLogger infos)
        {
            int endNodesCount = GetNodes().OfType<EndNode>().Count();
            switch (endNodesCount)
            {
                case 0:
                    infos.LogError("Add a EndNode in your DialogueGraph.", this);
                    break;
                case > 1:
                    {
                        infos.LogWarning($"DialogueGraph only supports one EndNode per graph. Only the first created one will be used.");
                        break;
                    }
            }
        }

    }
}
