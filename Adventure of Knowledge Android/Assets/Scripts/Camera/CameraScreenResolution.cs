using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class CameraScreenResolution:MonoBehaviour
    {
        private const float TARGET_WIDTH = 1920f;
        private const float TARGET_HEIGHT = 1080f;

        [SerializeField] private float minOrthographicSize;

        private void Start()
        {
            AdjustCameraOrthographicSize();
        }

        private void AdjustCameraOrthographicSize()
        {
            float targetAspect = TARGET_WIDTH / TARGET_HEIGHT;

            float currentAspect = (float)Screen.width / Screen.height;

            float ratioDifference = targetAspect / currentAspect;

            Camera.main.orthographicSize *= ratioDifference;

            if (Camera.main.orthographicSize < minOrthographicSize)
                Camera.main.orthographicSize = minOrthographicSize;
        }
    }
}
