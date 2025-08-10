using Mrjglfc.DialogueGraphSystem.Editor.Nodes;
using Mrjglfc.DialogueGraphSystem.Runtime;
using Mrjglfc.DialogueGraphSystem.Runtime.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Editor
{
    /// <summary>
    /// DialogueDirectorImporter is a <see cref="ScriptedImporter"/> that imports the <see cref="DialogueGraph"/>
    /// and builds the corresponding <see cref="DialogueRuntimeGraph"/>.
    /// </summary>
    [ScriptedImporter(1, DialogueGraph.AssetExtension)]
    internal class DialogueDirectorImporter : ScriptedImporter
    {
        /// <summary>
        /// Unity calls this method when the editor imports the asset. This method then processes the imported <see cref="DialogueGraph"/>.
        /// </summary>
        /// <param name="ctx">The asset import context.</param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            DialogueGraph graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);

            // The `graph` may be null if the `GraphDatabase.LoadGraphForImporter` method
            // fails to load the asset from the specified `ctx.assetPath`.
            // This can occur under the following circumstances:
            // - The asset path is incorrect, or the asset does not exist at the specified location.
            // - The asset located at the specified path is not of type `DialogueGraph`.
            // - The asset file itself is problematic. For example, it is corrupted, or stored in an unsupported format.
            //
            // Best practice to deal with serialization is to thoroughly validate and safeguard against
            // impaired or incomplete data, to account for potential deserialization issues.
            if (graph == null)
            {
                Debug.LogError($"Failed to load Visual Novel Director graph asset: {ctx.assetPath}");
                return;
            }

            // Get the first Start Node
            StartNode startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNodeModel == null)
            {
                // No need to log an error here, as the DialogueGraphProcessor is already logging an error in the console
                return;
            }

            // Build the runtime asset by walking the graph and adding the relevant nodes.
            DialogueRuntimeGraph runtimeAsset = ScriptableObject.CreateInstance<DialogueRuntimeGraph>();
            DFSIterative(runtimeAsset, startNodeModel);

            // Add the runtime object to the graph asset and set it to be the main asset.
            // This allows the same asset to be used in inspectors wherever a runtime asset is expected.
            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        /// <summary>
        /// Converts a <see cref="DialogueNode"/> to a list of one or more runtime <see cref="DialogueRuntimeNode"/>s.
        /// </summary>
        /// <param name="nodeModel">The <see cref="DialogueNode"/> to convert.</param>
        /// <returns>
        /// A list of <see cref="DialogueRuntimeNode"/>s that represent the runtime behavior of the input node.
        /// Multiple runtime nodes may be generated from a single input <see cref="DialogueNode"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the <see cref="NodeModel"/> passed in is unsupported and cannot be converted.
        /// </exception>
        /// <remarks>
        /// This conversion is not always 1:1. For example: the <see cref="SetSpeakerNode"/> node is converted to
        /// a <see cref="SetDialogueRuntimeNode"/> and a <see cref="WaitForInputRuntimeNode"/>. This is so that the
        /// runtime pauses execution and waits for player input after a dialogue is displayed. This approach allows
        /// more complex behaviour to be composed of multiple simpler runtime nodes.
        /// <br/><br/>
        /// </remarks>
        static DialogueRuntimeNode TranslateNodeModelToRuntimeNodes(INode nodeModel)
        {
            switch (nodeModel)
            {
                case StartNode:
                    return new StartRuntimeNode();

                case SetBackgroundNode setBackgroundNodeModel:
                    return new SetBackgroundRuntimeNode
                    {
                        BackgroundSprite = GetNodeOptionValue<Sprite>(setBackgroundNodeModel.GetNodeOptionByName(SetBackgroundNode.m_BackgroundName))
                    };

                case SetSpeakerNode setSpeakerNodeModel:
                    return new SetDialogueRuntimeNode
                    {
                        ActorName = GetNodeOptionValue<string>(setSpeakerNodeModel.GetNodeOptionByName(SetSpeakerNode.m_CharacterName)),
                        ActorSprite = GetNodeOptionValue<Sprite>(setSpeakerNodeModel.GetNodeOptionByName(SetSpeakerNode.m_CharacterSprite)),
                        DialogueText = GetNodeOptionValue<string>(setSpeakerNodeModel.GetNodeOptionByName(SetSpeakerNode.m_Dialogue))
                    };

                case WaitForInputNode _:
                    return new WaitForInputRuntimeNode();

                case ChoiceNode choiceNode:
                    choiceNode.GetNodeOptionByName(ChoiceNode.m_ChoiceCount).TryGetValue(out int choiceCount);
                    string[] dialogueChoices = new string[choiceCount];

                    for (int i = 0; i < choiceCount; i++)
                    {
                        choiceNode.GetOutputPortByName($"Choice {i}").TryGetValue(out string choice);
                        dialogueChoices[i] = choice;
                    }

                    return new ChoiceRuntimeNode
                    {
                        dialogueOptions = dialogueChoices
                    };

                case EndNode:
                    return new EndRuntimeNode();

                default:
                    throw new ArgumentException($"Unsupported node model type: {nodeModel.GetType()}");
            }
        }

        static T GetNodeOptionValue<T>(INodeOption option)
        {
            option.TryGetValue(out T value);
            return value;
        }

        // Iterative DFS (using Stack)
        void DFSIterative(DialogueRuntimeGraph graph, DialogueNode startNode)
        {
            var visited = new HashSet<DialogueRuntimeNode>();
            var runtimeStack = new Stack<DialogueRuntimeNode>();
            var editorStack = new Stack<DialogueNode>();

            DialogueRuntimeNode startRuntimeNode = TranslateNodeModelToRuntimeNodes(startNode);
            runtimeStack.Push(startRuntimeNode);
            editorStack.Push(startNode);
            DialogueRuntimeNode previousRuntimeNode = startRuntimeNode;
            DialogueNode previousEditorNode = startNode;

            while (runtimeStack.Count > 0)
            {
                DialogueRuntimeNode currentRuntimeNode = runtimeStack.Pop();
                DialogueNode currentEditorNode = editorStack.Pop();

                if (currentRuntimeNode != null && !visited.Contains(currentRuntimeNode))
                {
                    visited.Add(currentRuntimeNode);

                    Debug.Log($"DFS Graph Import: Adding node - {currentRuntimeNode} to graph");
                    graph.Graph.AddNode(currentRuntimeNode);

                    if (currentRuntimeNode is not StartRuntimeNode)
                    {
                        graph.Graph.AddEdge(previousRuntimeNode, currentRuntimeNode);
                    }

                    previousRuntimeNode = currentRuntimeNode;

                    if (currentEditorNode is EndNode || currentEditorNode == null) continue;
                    foreach (IPort outputPort in currentEditorNode.GetOutputPorts())
                    {
                        INode connectedNode = outputPort.firstConnectedPort.GetNode();

                        if (connectedNode == null) continue;
                        DialogueRuntimeNode returnedNode = TranslateNodeModelToRuntimeNodes(connectedNode);

                        if (!visited.Contains(returnedNode))
                        {
                            runtimeStack.Push(returnedNode);
                            editorStack.Push(connectedNode as DialogueNode);
                        }
                    }
                }
            }
        }
    }
}
