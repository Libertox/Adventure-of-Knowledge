using System;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [CreateAssetMenu(fileName = "SoundEffectsClips", menuName = "ScriptableObject/SoundEffectsClips")]
    public class SoundEffectsClipsSO:ScriptableObject
    {
        [field:SerializeField] public AudioClip[] ButtonClips{get;private set;}
        [field: SerializeField] public AudioClip BuyClip { get; private set; }
        [field: SerializeField] public AudioClip VictoryClip { get; private set; }
        [field: SerializeField] public AudioClip CorrectAnswerClip { get; private set; }
        [field: SerializeField] public AudioClip InteractClip { get; private set; }
        [field: SerializeField] public AudioClip SpinningWheelClip { get; private set; }

    }
}
