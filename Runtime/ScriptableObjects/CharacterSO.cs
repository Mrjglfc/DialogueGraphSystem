using System.Collections.Generic;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character",menuName = "DialogueGraphSystem/Character")]
    public class CharacterSO : ScriptableObject
    {
        public string Name;
        public List<Sprite> Expressions;

        private void OnValidate()
        {
            if (Name == null)
            {
                Debug.LogError($"Name is null in CharacterSO: {name}");
            }

            if(Expressions == null)
            {
                Debug.LogError($"Expressions is null in CharacterSO: {name}. A Character should have atleast 1 default expression Sprite");
            }
        }
    }
}
