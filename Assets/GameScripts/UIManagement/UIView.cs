using System;
using GameScripts.UIManagement.Animations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameScripts.UIManagement
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(RectTransform))]
    public class UIView : MonoBehaviour
    {
        [SerializeField] private bool _changeVisibility = true;
        public UIViewId ViewId;
        [HorizontalGroup(GroupID = "Start Position")]
        public Vector2 StartPosition;
        [SerializeField] 
        private UIViewAwakeAction AwakeAction = UIViewAwakeAction.InstantHide;
        [SerializeField] 
        protected UIAnimation _showAnimation;
        [SerializeField] 
        protected UIAnimation _hideAnimation;

        [FoldoutGroup("Events")]
        public UnityEvent ShowAnimationStarted = new();
        [FoldoutGroup("Events")]
        public UnityEvent ShowAnimationFinished = new();
        [FoldoutGroup("Events")]
        public UnityEvent HideAnimationStarted = new();
        [FoldoutGroup("Events")]
        public UnityEvent HideAnimationFinished = new();

        private UIViewState _state = UIViewState.Hidden;
        private Canvas _canvas;
        private GraphicRaycaster _graphicRaycaster;
        private RectTransform _rectTransform;

        public virtual bool IsPopup => false;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _rectTransform.anchoredPosition = StartPosition;
            
            CallAwakeAction();
            if (_showAnimation)
                _showAnimation.AnimationFinished += OnShowAnimationFinished;
            if (_hideAnimation)
                _hideAnimation.AnimationFinished += OnHideAnimationFinished;
            
            UIManager.Instance.Register(this);
        }

        private void CallAwakeAction()
        {
            switch (AwakeAction)
            {
                case UIViewAwakeAction.Show:
                    InstantHide();
                    Show();
                    break;
                case UIViewAwakeAction.InstantShow:
                    InstantShow();
                    break;
                case UIViewAwakeAction.InstantHide:
                    InstantHide();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HorizontalGroup(GroupID = "Start Position")]
        [Button(Name = "Get")]
        public void GetStartPosition()
        {
            StartPosition = GetComponent<RectTransform>().anchoredPosition;
        }

        [ButtonGroup]
        [Button]
        [HideInEditorMode]
        public virtual void Show()
        {
            if (!_showAnimation)
            {
                InstantShow();
                return;
            }
            if (_state == UIViewState.Hidden)
            {
                _state = UIViewState.Showing;
                ShowAnimationStarted?.Invoke();
                _showAnimation.StartAnimation();
                ChangeVisibility(true);
                return;
            }

            if (_state == UIViewState.Hiding)
            {
                _state = UIViewState.Showing;
                var progress = _hideAnimation.Progress;
                _hideAnimation.ForceStopAnimation();
                ShowAnimationStarted?.Invoke();
                _showAnimation.StartAnimation(progress);
            }
        }

        [ButtonGroup]
        [Button]
        [HideInEditorMode]
        public virtual void Hide()
        {
            if (!_hideAnimation)
            {
                InstantHide();
                return;
            }
            
            if (_state == UIViewState.Shown)
            {
                _state = UIViewState.Hiding;
                HideAnimationStarted?.Invoke();
                _hideAnimation.StartAnimation();
                return;
            }

            if (_state == UIViewState.Showing)
            {
                _state = UIViewState.Hiding;
                var progress = _showAnimation.Progress;
                _showAnimation.ForceStopAnimation();
                HideAnimationStarted?.Invoke();
                _hideAnimation.StartAnimation(progress);
            }
        }

        [ButtonGroup]
        [Button]
        [HideInEditorMode]
        public virtual void InstantShow()
        {
            ChangeVisibility(true);
            if (_showAnimation)
                _showAnimation.StartInstantAnimation();
            _state = UIViewState.Shown;
        }

        [ButtonGroup]
        [Button]
        [HideInEditorMode]
        public virtual void InstantHide()
        {
            ChangeVisibility(false);
            if (_showAnimation)
                _showAnimation.ForceStopAnimation();
            if (_hideAnimation)
                _hideAnimation.StartInstantAnimation();
            _state = UIViewState.Hidden;
        }

        private void OnShowAnimationFinished(bool isFinished)
        {
            if (!isFinished)
                return;
            ShowAnimationFinished?.Invoke();
            _state = UIViewState.Shown;
        }

        private void OnHideAnimationFinished(bool isFinished)
        {
            if (!isFinished)
                return;
            HideAnimationFinished?.Invoke();
            _state = UIViewState.Hidden;
            ChangeVisibility(false);
        }

        private void ChangeVisibility(bool visible)
        {
            if(!_changeVisibility) return;
            _canvas.enabled = visible;
            _graphicRaycaster.enabled = visible;
        }
        
        public virtual UIView GetUIView()
        {
            return this;
        }

    }

    public enum UIViewState
    {
        Shown,
        Showing,
        Hidden,
        Hiding
    }
    
    public enum UIViewAwakeAction{
        Show,
        InstantShow,
        InstantHide,
    }
}