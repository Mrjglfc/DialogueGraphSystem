using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Mrjglfc.DialogueGraphSystem.Runtime.Nodes
{
    public class SetDialogueExecutor : IDialogueNodeExecutor<SetDialogueRuntimeNode>, IDialogueNodeExecutor<SetDialogueRuntimeNodeWithPreviousActor>
    {
        /// <summary>
        /// Executes the <see cref="SetDialogueRuntimeNode"/> node, setting the dialogue text and actor sprite settings.
        /// </summary>
        public async Task ExecuteAsync(SetDialogueRuntimeNode runtimeNode, DialogueDirector ctx)
        {
            if (string.IsNullOrEmpty(runtimeNode.DialogueText))
            {
                ctx.SetDialoguePanel(false);
                return;
            }

            ctx.SetDialoguePanel(true, runtimeNode.ActorName);

            foreach (Image location in ctx.ActorLocationList)
                location.enabled = false;

            if (runtimeNode.ActorSprite != null)
            {
                ctx.SetActorLocation(runtimeNode.LocationIndex, runtimeNode.ActorSprite);
            }

            await TypeTextWithSkipAsync(runtimeNode.DialogueText, ctx);
        }

        /// <summary>
        /// Executes the <see cref="SetDialogueRuntimeNodeWithPreviousActor"/> node, and keeps all previous actor settings
        /// while changing the dialogue text.
        /// </summary>
        public async Task ExecuteAsync(SetDialogueRuntimeNodeWithPreviousActor runtimeNode, DialogueDirector ctx)
        {
            if (string.IsNullOrEmpty(runtimeNode.DialogueText))
            {
                ctx.SetDialoguePanel(false);
                return;
            }

            ctx.SetDialoguePanel(true);

            await TypeTextWithSkipAsync(runtimeNode.DialogueText, ctx);
        }

        /// <summary>
        /// Executes a typewriter effect on the given <see cref="TextMeshProUGUI"/> label.
        /// </summary>
        /// <param name="dialogueText">The text to set</param>
        /// <param name="ctx">The <see cref="VisualNovelDirector"/> context to get settings and input from</param>
        /// <remarks>
        /// Input is used to skip the typewriter effect if it's in-progress.
        /// </remarks>
        static async Task TypeTextWithSkipAsync(string dialogueText, DialogueDirector ctx)
        {
            TMPro.TextMeshProUGUI label = ctx.DialogueText;
            float delayPerCharSeconds = ctx.GlobalTextDelayPerCharacter;

            label.SetText("");
            StringBuilder builder = new();

            bool insideRichTag = false;

            // Start listening for skip input
            Task skipInputDetected = ctx.InputProvider.InputDetected();

            foreach (char c in dialogueText)
            {
                // Handle rich text tags (e.g., <b>, </i>)
                if (c == '<')
                    insideRichTag = true;

                builder.Append(c);

                if (c == '>')
                    insideRichTag = false;

                // Skip delay if rich text
                if (insideRichTag || char.IsWhiteSpace(c)) continue;

                label.SetText(builder.ToString());

                float timer = 0f;
                while (timer < delayPerCharSeconds)
                {
                    if (skipInputDetected.IsCompleted)
                    {
                        label.SetText(dialogueText);
                        return;
                    }
                    timer += Time.deltaTime;
                    await Task.Yield();
                }
            }

            label.SetText(dialogueText);
        }
    }
}