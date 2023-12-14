using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventureOfKnowledge.UI
{
    public class ButtonAnimation:MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float scaleEndValue = 1.2f;
        [SerializeField] private float animationDuration = 0.5f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(new Vector3(scaleEndValue, scaleEndValue, 0), animationDuration).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, animationDuration).SetUpdate(true);
        }

        public void ScaleDown() => transform.DOScale(Vector3.one, animationDuration).SetUpdate(true);
    }
}
