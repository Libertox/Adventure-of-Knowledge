using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [CreateAssetMenu(fileName = "DialogMessage", menuName = "ScriptableObject/DialogMessage")]
    public class DialogMessageSO:ScriptableObject
    {
        [TextArea]
        [SerializeField] private List<string> dialogList;

        public string GetRandomDialogFromList()
        {
            int dialogIndex = UnityEngine.Random.Range(0, dialogList.Count);  
            return dialogList[dialogIndex];
        }

    }
}
