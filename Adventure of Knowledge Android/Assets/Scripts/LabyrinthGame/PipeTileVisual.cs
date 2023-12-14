using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.LabyrinthGame
{
    public class PipeTileVisual:MonoBehaviour
    {
        private const string SHADER_MAIN_TEXTURE_REF = "_MainTexture";
        private const string SHADER_DISSOLVE_AMOUNT_REF = "_DissolveAmount";

        [SerializeField] private Image tileImage;
        [SerializeField] private Image pipeImage;
        [SerializeField] private Image filledPipeImage;

        private readonly float fillSpeed = 4f;

        private void Awake()
        {
            Material pipeMaterial = new Material(pipeImage.material);
            pipeImage.material = pipeMaterial;
            pipeImage.material.SetTexture(SHADER_MAIN_TEXTURE_REF, pipeImage.sprite.texture);
        }

        public void SetFillAmount(float fillAmount) => filledPipeImage.fillAmount = fillAmount;

        public IEnumerator FillCoroutine(PipeTile.Direction fillDirection, Action actionAfterFilled)
        {
            SetupFillDirection(fillDirection);
            while(filledPipeImage.fillAmount < 1)
            {
                filledPipeImage.fillAmount += Time.deltaTime * fillSpeed;
                yield return null;
            }

            actionAfterFilled?.Invoke();
        }

        private void SetupFillDirection(PipeTile.Direction fillDirection)
        {
            if (tileImage.transform.eulerAngles.z == 0 || tileImage.transform.eulerAngles.z == 90)
            {
                filledPipeImage.fillMethod = Image.FillMethod.Vertical;
                if (fillDirection == PipeTile.Direction.Right || fillDirection == PipeTile.Direction.Down)
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Bottom;
                else
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Top;
            }
            else
            {
                filledPipeImage.fillMethod = Image.FillMethod.Vertical;
                if (fillDirection == PipeTile.Direction.Right || fillDirection == PipeTile.Direction.Down)
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Top;
                else
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Bottom;
            }
        }


        public IEnumerator UnfillCoroutine(PipeTile.Direction unfillDirection)
        {
            SetupUnfillDirection(unfillDirection);
            while (filledPipeImage.fillAmount > 0)
            {
                filledPipeImage.fillAmount -= Time.deltaTime * fillSpeed;
                yield return null;
            }
        }

        private void SetupUnfillDirection(PipeTile.Direction fillDirection)
        {
            if (tileImage.transform.eulerAngles.z == 0 || tileImage.transform.eulerAngles.z == 90)
            {
                filledPipeImage.fillMethod = Image.FillMethod.Vertical;
                if (fillDirection == PipeTile.Direction.Right || fillDirection == PipeTile.Direction.Down)
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Top;
                else
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Bottom;
            }
            else
            {
                filledPipeImage.fillMethod = Image.FillMethod.Vertical;
                if (fillDirection == PipeTile.Direction.Right || fillDirection == PipeTile.Direction.Down)
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Bottom;
                else
                    filledPipeImage.fillOrigin = (int)Image.OriginVertical.Top;
            }
        }

        public IEnumerator AppearCoroutine()
        {
            float appearAmount = 0;
            while (appearAmount < 1)
            {
                appearAmount += Time.deltaTime;
                tileImage.material.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, appearAmount);
                pipeImage.material.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, appearAmount);

                yield return null;
            }
        }
    }
}
