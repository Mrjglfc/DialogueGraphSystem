using Mrjglfc.DialogueGraphSystem.Runtime.InputProvider;
using Mrjglfc.DialogueGraphSystem.Runtime.Nodes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mrjglfc.DialogueGraphSystem.Runtime
{
    public class DialogueDirector : MonoBehaviour
    {
        [Header("Graph")]
        // The runtime graph to execute. Note that the runtime graph asset is the same as the authoring graph asset
        // because we export the runtime asset object into the same asset and set it as the 'main' asset in our importer.
        // This allows us to edit the authoring graph in the editor and drag-drop the same asset into inspector fields
        // that expect the runtime graph.
        public DialogueRuntimeGraph RuntimeGraph;

        [Header("Scene References")]
        public Image BackgroundImage;
        public List<Image> ActorLocationList;
        public GameObject DialoguePanel;
        public TextMeshProUGUI DialogueText;
        public TextMeshProUGUI ActorNameText;

        [Header("Settings")]
        public float GlobalFadeDuration = 0.5f;
        public float GlobalTextDelayPerCharacter = 0.03f;

        [Header("Input")]
        public MonoBehaviour InputComponent;
        public IDialogueInputProvider InputProvider => InputComponent as IDialogueInputProvider;

        readonly StartExecutor startExecutor = new();
        readonly SetBackgroundExecutor setBackgroundExecutor = new();
        readonly SetDialogueExecutor setDialogueExecutor = new();
        readonly WaitForInputExecutor waitForInputExecutor = new();
        readonly PerformActionExecutor performActionExecutor = new();

        private async void Start()
        {
            // Execute each node in the runtime graph sequentially
            foreach (DialogueRuntimeNode node in RuntimeGraph.Nodes)
            {
                switch (node)
                {
                    case StartRuntimeNode startRuntimeNode:
                        await startExecutor.ExecuteAsync(startRuntimeNode, this);
                        break;

                    case SetBackgroundRuntimeNode bgNode:
                        await setBackgroundExecutor.ExecuteAsync(bgNode, this);
                        break;

                    case SetDialogueRuntimeNode dialogueNode:
                        await setDialogueExecutor.ExecuteAsync(dialogueNode, this);
                        break;

                    case SetDialogueRuntimeNodeWithPreviousActor dialogueNode:
                        await setDialogueExecutor.ExecuteAsync(dialogueNode, this);
                        break;

                    case WaitForInputRuntimeNode waitNode:
                        await waitForInputExecutor.ExecuteAsync(waitNode, this);
                        break;

                    case PerformActionRuntimeNode performActionRuntimeNode:
                        await performActionExecutor.ExecuteAsync(performActionRuntimeNode, this);
                        break;

                    default:
                        Debug.LogError($"No executor found for node type: {node.GetType()}");
                        break;
                }
            }
        }

        internal void SetDialoguePanel(bool isEnabled, string actorName = "")
        {
            if (actorName != "")
            {
                ActorNameText.SetText(actorName);
            }

            DialoguePanel.SetActive(isEnabled);
        }

        internal void SetActorLocation(int actorIndex, Sprite actorSprite)
        {
            Image img = ActorLocationList[actorIndex];
            img.enabled = true;
            img.sprite = actorSprite;
        }
    }
}