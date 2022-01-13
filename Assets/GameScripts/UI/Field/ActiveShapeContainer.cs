using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class ActiveShapeContainer : MonoBehaviour
    {
        public ReactiveProperty<Vector2Int> HoveredCell;
        
        public RectTransform cellContainerRect;
        public RectTransform containerRect;
        [SerializeField] private Image image;

        private Sequence _sequence;
        private Vector2 _initialPosition;
        private Rotation _rotation;
        private bool _hovering;

        private void Awake()
        {
            _initialPosition = containerRect.anchoredPosition;
            HoveredCell = new ReactiveProperty<Vector2Int>(new Vector2Int(-1, -1));
        }

        private void Update()
        {
            HoveredCell.Value = CalculateShapeOriginInField();
        }

        private Vector2Int CalculateShapeOriginInField()
        {
            if (!_hovering)
            {
                return new Vector2Int(-1, -1);;
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(cellContainerRect,
                GetRayOrigin(_rotation),
                null, out var shapeOriginInField);

            var shapeOriginInFieldNormalized =
                (shapeOriginInField + new Vector2(cellContainerRect.rect.width * 0.5f, cellContainerRect.rect.height * 0.5f))
                / (cellContainerRect.rect.width / 9);

            if (shapeOriginInFieldNormalized.x < 0 || shapeOriginInFieldNormalized.x >= 9 ||
                shapeOriginInFieldNormalized.y < 0 || shapeOriginInFieldNormalized.y >= 9)
            {
                return new Vector2Int(-1, -1);;
            }

            var currentPlacementPosition = new Vector2Int(
                Math.Clamp((int) (shapeOriginInFieldNormalized.x + 0.5f), 0, 8),
                Math.Clamp((int) (shapeOriginInFieldNormalized.y + 0.5f), 0, 8)
            );

            return currentPlacementPosition;
        }

        private Vector2 GetRayOrigin(Rotation rotation)
        {
            Vector3[] corners = new Vector3[4];
            containerRect.GetWorldCorners(corners);
            switch (rotation)
            {
                case Rotation.Deg0:
                    return corners[0];
                case Rotation.Deg90:
                    return corners[3];
                case Rotation.Deg180:
                    return corners[2];
                case Rotation.Deg270:
                    return corners[1];
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        public void AnimatePopUp()
        {
            _sequence?.Complete();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0f, image.DOFade(1, 0.3f));
            _hovering = true;
        }

        public void AnimateHide()
        {
            _sequence?.Complete();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0f, image.DOFade(0, 0.3f));
            _hovering = false;
        }

        public void ResetAnchoredPosition()
        {
            containerRect.anchoredPosition = _initialPosition;
        }
    }
}