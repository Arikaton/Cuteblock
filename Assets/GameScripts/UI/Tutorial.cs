using System.Collections;
using System.Collections.Generic;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class Tutorial : MonoBehaviour
    {
        private const string TutorialCompletedPrefsKey = "TutorialCompleted";

        public RectTransform tutorialView;
        public List<RectTransform> pages;
        public Button nextButton;

        private int _currentPage;
    
        private IEnumerator Start()
        {
            bool tutorialCompleted = PlayerPrefs.GetInt(TutorialCompletedPrefsKey, 0) != 0;
            if (tutorialCompleted)
            {
                yield break;
            }

            nextButton.OnClickAsObservable().Subscribe(_ => OpenNextPage()).AddTo(this);
            _currentPage = -1;
            yield return new WaitForEndOfFrame();
            UIManager.Instance.ShowPopup(UIViewId.PopupTutorial);
            OpenNextPage();
        }

        private void OpenNextPage()
        {
            if (_currentPage + 1 >= pages.Count)
            {
                FinishTutorial();
                return;
            }

            _currentPage++;
            foreach (var page in pages)
                page.gameObject.SetActive(false);
            pages[_currentPage].gameObject.SetActive(true);
        }

        private void FinishTutorial()
        {
            PlayerPrefs.SetInt(TutorialCompletedPrefsKey, 1);
            UIManager.Instance.HideLastPopup();
        }
    }
}
