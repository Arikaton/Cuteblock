using GameScripts.Game;
using TweensStateMachine;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    [RequireComponent(typeof(CellAnimator))]
    public class CellView : MonoBehaviour
    {
        private const float Duration = 0.15f;
        
        [SerializeField] private Image image;

        private CellViewModel _cellViewModel;
        private CompositeDisposable _disposables = new();
        private TweenStateMachine _stateMachine;
        private bool _shadowed;
        private bool _occupied;

        private void Awake()
        {
            _stateMachine = new TweenStateMachine();
            InitializeStateMachine();
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        public void Bind(CellViewModel cellViewModel)
        {
            _cellViewModel = cellViewModel;
            _cellViewModel.CellState.Subscribe(value => _occupied = value == CellStates.Occupied).AddTo(_disposables);
            _cellViewModel.Shadowed.Subscribe(value =>_shadowed = value).AddTo(_disposables);
        }

        private void InitializeStateMachine()
        {
            _stateMachine.AddState("normal", image.TTColor(Color.white, Duration));
            _stateMachine.AddState("shadowed", image.TTColor(new Color(0.73f, 0.74f, 0.84f), Duration));
            _stateMachine.AddState("occupied", image.TTColor(new Color(0.74f, 0.81f, 1f), Duration));

            _stateMachine.AddTransition("normal", "occupied", Occupied);
            _stateMachine.AddTransition("normal", "shadowed", Shadowed);
            _stateMachine.AddTransition("shadowed", "occupied", Occupied);
            _stateMachine.AddTransition("shadowed", "normal", () => !Shadowed());
            _stateMachine.AddTransition("occupied", "normal", () => !Occupied());
            _stateMachine.SetState("normal");
        }

        private bool Shadowed() => _shadowed;
        private bool Occupied() => _occupied;

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}