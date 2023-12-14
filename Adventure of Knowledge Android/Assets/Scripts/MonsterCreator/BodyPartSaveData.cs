using System;
using System.Collections.Generic;

namespace AdventureOfKnowledge
{
    [Serializable]
    public struct BodyPartSaveData
    {
        public float distanceFromBodyX;
        public float distanceFromBodyY;
        public float scale;
        public int scaleStep;
        public bool isFlip;
        public int spriteIndex;
        public int colorIndex;
        public TypeOfBodyPart bodyPart;
        public List<BodyPartSaveData> child;

        public bool IsRightArm() => bodyPart == TypeOfBodyPart.Arm && !isFlip;

        public bool IsBody() => bodyPart == TypeOfBodyPart.Body;
    }

}

