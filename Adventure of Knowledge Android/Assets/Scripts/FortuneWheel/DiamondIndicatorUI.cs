
using DG.Tweening;
using UnityEngine;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class DiamondIndicatorUI:MonoBehaviour
    {
        [SerializeField] private RectTransform endPosition;

        private readonly float animationDuration = 0.5f;

        private void Start() => Hide();
       
        public void SetupStartPosition(Vector3 startPosition) => transform.position = startPosition;
       
        public void MoveTowards()
        {
            Show();
            transform.DOMove(endPosition.position, animationDuration).OnComplete(() =>
            {
                FortuneWheelManager.Instance.AddAward();
                Hide();
            });
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
