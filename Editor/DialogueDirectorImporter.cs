using Mrjglfc.DialogueGraphSystem.Editor.Nodes;
using Mrjglfc.DialogueGraphSystem.Runtime;
using Mrjglfc.DialogueGraphSystem.Runtime.Nodes;
using System;
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
            // (Only using the first node is a simplification we made for this sample)
            StartNode startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNodeModel == null)
            {
                // No need to log an error here, as the DialogueGraphProcessor is already logging an error in the console
                // See DialogueGraph.CheckGraphErrors(GraphLogger).
                return;
            }

            // Build the runtime asset by walking the graph and adding the relevant nodes.
            DialogueRuntimeGraph runtimeAsset = ScriptableObject.CreateInstance<DialogueRuntimeGraph>();
            INode nextNodeModel = GetNextNode(startNodeModel);
            while (nextNodeModel != null)
            {
                List<DialogueRuntimeNode> runtimeNodes = TranslateNodeModelToRuntimeNodes(nextNodeModel);
                runtimeAsset.Nodes.AddRange(runtimeNodes);

                nextNodeModel = GetNextNode(nextNodeModel);
            }

            // Add the runtime object to the graph asset and set it to be the main asset.
            // This allows the same asset to be used in inspectors wherever a runtime asset is expected.
            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        /// <summary>
        /// Gets the node that is executed after the given node.
        /// </summary>
        /// <param name="currentNode">The current node</param>
        /// <returns>The next node in the graph</returns>
        static INode GetNextNode(INode currentNode)
        {
            IPort outputPort = currentNode.GetOutputPortByName(DialogueNode.EXECUTION_PORT_DEFAULT_NAME);
            IPort nextNodePort = outputPort.firstConnectedPort;
            INode nextNode = nextNodePort?.GetNode();

            return nextNode;
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
        static List<DialogueRuntimeNode> TranslateNodeModelToRuntimeNodes(INode nodeModel)
        {
            List<DialogueRuntimeNode> returnedNodes = new();
            switch (nodeModel)
            {
                case SetBackgroundNode setBackgroundNodeModel:
                    returnedNodes.Add(new SetBackgroundRuntimeNode
                    {
                        BackgroundSprite = GetInputPortValue<Sprite>(setBackgroundNodeModel.GetInputPortByName(SetBackgroundNode.m_BackgroundName))
                    });

                    // Note: We deliberately don't add a WaitForInputRuntimeNode here to enable updating multiple
                    // visual novel elements (the background, music, dialogue, etc) all at once. This creates a seamless
                    // transition involving more than one element.
                    break;

                case SetSpeakerNode setSpeakerNodeModel:
                    returnedNodes.Add(new SetDialogueRuntimeNode
                    {
                        ActorName = GetInputPortValue<string>(setSpeakerNodeModel.GetInputPortByName(SetSpeakerNode.m_CharacterName)),
                        ActorSprite = GetInputPortValue<Sprite>(setSpeakerNodeModel.GetInputPortByName(SetSpeakerNode.m_CharacterSprite)),
                        DialogueText = GetInputPortValue<string>(setSpeakerNodeModel.GetInputPortByName(SetSpeakerNode.m_Dialogue))
                    });

                    // Insert a WaitForInputNode after dialogue to create the expected visual novel behaviour.
                    // This ensures narrative flow pauses until the player signals readiness to continue.
                    returnedNodes.Add(new WaitForInputRuntimeNode());
                    break;

                case WaitForInputNode _:
                    returnedNodes.Add(new WaitForInputRuntimeNode());
                    break;

                default:
                    throw new ArgumentException($"Unsupported node model type: {nodeModel.GetType()}");
            }

            return returnedNodes;
        }

        /// <summary>
        /// Gets the value of an input port on a node.
        /// <br/><br/>
        /// The value is obtained from (in priority order):<br/>
        /// 1. Connections to the port (variable nodes, constant nodes, wire portals)<br/>
        /// 2. Embedded value on the port<br/>
        /// 3. Default value of the port<br/>
        /// </summary>
        static T GetInputPortValue<T>(IPort port)
        {
            T value = default;

            // If port is connected to another node, get value from connection
            if (port.isConnected)
            {
                switch (port.firstConnectedPort.GetNode())
                {
                    case IVariableNode variableNode:
                        variableNode.variable.TryGetDefaultValue(out value);
                        return value;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue(out value);
                        return value;
                    default:
                        break;
                }
            }
            else
            {
                port.TryGetValue(out value);
            }

            return value;
        }
    }
}
