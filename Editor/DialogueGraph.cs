using Mrjglfc.DialogueGraphSystem.Editor.Nodes;
using System;
using System.Collections.Generic;
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

            CheckGraphErrors(infos);
        }

        void CheckGraphErrors(GraphLogger infos)
        {
            List<StartNode> startNodes = GetNodes().OfType<StartNode>().ToList();
            List<EndNode> endNodes = GetNodes().OfType<EndNode>().ToList();

            switch (startNodes.Count)
            {
                case 0:
                    infos.LogError("Add a StartNode in your DialogueGraph.", this);
                    break;
                case > 1:
                    {
                        infos.LogWarning($"DialogueGraph only supports one StartNode per graph. Only the first created one will be used.", startNodes[0]);
                        break;
                    }
            }

            switch (endNodes.Count)
            {
                case 0:
                    infos.LogError("Add a StartNode in your DialogueGraph.", this);
                    break;
                case > 1:
                    {
                        infos.LogWarning($"DialogueGraph only supports one EndNode per graph. Only the first created one will be used.", endNodes[0]);
                        break;
                    }
            }
        }
    }
}
