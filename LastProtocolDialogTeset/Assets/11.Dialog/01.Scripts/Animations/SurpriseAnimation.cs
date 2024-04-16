using DG.Tweening;

namespace MyDialogSystem
{
    public class SurpriseAnimation : CharactorAnimation
    {
        private Sequence seq;

        public override void Animation()
        {
            isAnimating = true;

            seq = DOTween.Sequence();

            float originPos = rect.anchoredPosition.y;

            seq.Append(rect.DOAnchorPosY(originPos + 150, 0.1f).SetEase(Ease.Linear))
                .Append(rect.DOAnchorPosY(originPos, 0.1f).SetEase(Ease.Linear))
                .OnComplete(() => isAnimating = false);
        }
    }

}
