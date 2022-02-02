using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UIManagement.Animations
{
    public class MoveUIAnimation : UIAnimationBase
    {
        [SerializeField] private UIView _uiView;
        [SerializeField] private float _duration;
        [SerializeField] private MoveUIAnimationDirection _direction;
        [SerializeField] private Ease _ease;
        [SerializeField] private UIAnimationType _type;

        private RectTransform _uiViewRect;
        private RectTransform _canvasRect;

        private Vector2 _screenSize;
        private Vector2 _viewSize;

        private void Awake()
        {
            _uiViewRect = _uiView.GetComponent<RectTransform>();
            var rootCanvas = _uiView.GetComponent<Canvas>().rootCanvas;
            _canvasRect = rootCanvas.GetComponent<RectTransform>();
            float canvasScale = Screen.width / rootCanvas.GetComponent<CanvasScaler>().referenceResolution.x;
            _screenSize = _canvasRect.sizeDelta / canvasScale;
            _viewSize = _uiViewRect.sizeDelta;
        }

        protected override void StartAnimationInternal(Sequence sequence, float durationPercent)
        {
            var outsidePosition = CalculatePositionOutsideScreen();
            if (_type == UIAnimationType.Show)
            {
                _uiViewRect.anchoredPosition = outsidePosition;
                sequence.Append(_uiViewRect.DOAnchorPos(_uiView.StartPosition, _duration * durationPercent)
                    .SetEase(_ease));
            }
            else
            {
                sequence.Append(_uiViewRect.DOAnchorPos(outsidePosition, _duration * durationPercent)
                    .SetEase(_ease));
            }
        }

        protected override void StartInstantAnimationInternal()
        {
            if (_type == UIAnimationType.Show)
            {
                _uiViewRect.anchoredPosition = _uiView.StartPosition;
            }
            else
            {
                _uiViewRect.anchoredPosition = CalculatePositionOutsideScreen();
            }
        }

        private Vector2 CalculatePositionOutsideScreen()
        {
            switch (_direction)
            {
                case MoveUIAnimationDirection.Top:
                    return new Vector2(_uiView.StartPosition.x, _uiView.StartPosition.y + _screenSize.y);
                case MoveUIAnimationDirection.Right:
                    return new Vector2(_screenSize.x + _uiViewRect.sizeDelta.x / 2, _uiView.StartPosition.y);
                case MoveUIAnimationDirection.Bottom:
                    return new Vector2(_uiView.StartPosition.x, _uiView.StartPosition.y - _screenSize.y);
                case MoveUIAnimationDirection.Left:
                    return new Vector2(-_uiViewRect.sizeDelta.x / 2 - _screenSize.x, _uiView.StartPosition.y);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum MoveUIAnimationDirection
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public enum UIAnimationType
    {
        Show,
        Hide
    }
}