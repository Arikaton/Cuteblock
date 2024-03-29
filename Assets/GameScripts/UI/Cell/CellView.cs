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
        private const string StateNormal = "normal";
        private const string StateShadowed = "shadowed";
        private const string StateOccupied = "occupied";
        private const string StateHighlighted = "highlighted";

        [SerializeField] private Image image;
        [SerializeField] private Color normalColor = new Color(0.74f, 0.81f, 1f);
        [SerializeField] private Color normalDarkColor = new Color(0.74f, 0.81f, 1f);
        [SerializeField] private Color occupiedColor = new Color(0.74f, 0.81f, 1f);
        [SerializeField] private Color shadowedColor = new Color(0.73f, 0.74f, 0.84f);
        [SerializeField] private Color highlightedColor = new Color(0.46f, 1f, 0.65f);

        private CellViewModel _cellViewModel;
        private TweenStateMachine _stateMachine;
        private bool _shadowed;
        private bool _occupied;
        private bool _highlighted;

        private bool _initialized;

        private void Awake()
        {
           
        }

        private void Update()
        {
            if(_initialized)
                _stateMachine.Tick();
        }

        public void Bind(CellViewModel cellViewModel)
        {
            _cellViewModel = cellViewModel;
            _cellViewModel.Occupied.Subscribe(value => _occupied = value ).AddTo(this);
            _cellViewModel.Shadowed.Subscribe(value =>_shadowed = value).AddTo(this);
            _cellViewModel.Highlighted.Subscribe(value => _highlighted = value).AddTo(this);
            _stateMachine = new TweenStateMachine();
            InitializeStateMachine();
            _initialized = true;
        }

        private void InitializeStateMachine()
        {
            _stateMachine.AddState(StateNormal, image.TTColor(GetNormalCellColor(), Duration));
            _stateMachine.AddState(StateShadowed, image.TTColor(shadowedColor, Duration));
            _stateMachine.AddState(StateOccupied, image.TTColor(occupiedColor, Duration));
            _stateMachine.AddState(StateHighlighted, image.TTColor(highlightedColor, Duration));

            _stateMachine.AddTransition(StateNormal, StateOccupied, Occupied);
            _stateMachine.AddTransition(StateNormal, StateHighlighted, Highlighted);
            _stateMachine.AddTransition(StateNormal, StateShadowed, Shadowed);
            
            _stateMachine.AddTransition(StateShadowed, StateNormal, () => !Shadowed());
            _stateMachine.AddTransition(StateShadowed, StateHighlighted, Highlighted);
            
            _stateMachine.AddTransition(StateOccupied, StateNormal, () => !Occupied());
            _stateMachine.AddTransition(StateOccupied, StateHighlighted, Highlighted);

            _stateMachine.AddTransition(StateHighlighted, StateNormal, () => !Highlighted());
            _stateMachine.AddTransition(StateHighlighted, StateOccupied, () => !Highlighted());

            _stateMachine.SetState(StateNormal);
        }

        private Color GetNormalCellColor()
        {
            var subgridId = GetSubgridId(_cellViewModel.PositionOnField);
            return subgridId switch
            {
                1 => normalDarkColor,
                3 => normalDarkColor,
                5 => normalDarkColor,
                7 => normalDarkColor,
                _ => normalColor
            };
        }

        private int GetSubgridId(Vector2Int pos)
        {
            return pos.x / 3 + pos.y / 3 * 3;
        }

        private bool Shadowed() => _shadowed;
        private bool Occupied() => _occupied;
        private bool Highlighted() => _highlighted;
    }
}