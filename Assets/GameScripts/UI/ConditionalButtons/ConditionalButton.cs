using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameScripts.UI.ConditionalButtons
{
    public class ConditionalButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        public Condition condition;

        [FoldoutGroup("Events")]
        public UnityEvent immediateClickAction;
        [FoldoutGroup("Events")]
        public UnityEvent conditionIsMetAction;
        [FoldoutGroup("Events")]
        public UnityEvent conditionNotMetAction;

        private void Awake()
        {
            button.OnClickAsObservable().Subscribe(_ => Click()).AddTo(this);
            if(condition == null) Debug.LogWarning("Conditional button - 'condition' is missing", this);
        }

        private void Click()
        {
            immediateClickAction?.Invoke();
            condition?.Check(Callback);
        }

        private void Callback(bool conditionIsMet)
        {
            if(conditionIsMet)
                conditionIsMetAction.Invoke();
            else
                conditionNotMetAction?.Invoke();
        }
    }
}