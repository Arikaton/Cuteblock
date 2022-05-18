using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;
using UnityEngine;
using Zenject;

namespace GameScripts
{
    public class SetInitialHintsAmount : MonoBehaviour
    {
        private IResourceStorage _resourceStorage;

        [Inject]
        public void Construct(IResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
        }
        
        private void Start()
        {
            if (!PlayerPrefs.HasKey("FirstLaunch"))
            {
                PlayerPrefs.SetInt("FirstLaunch", 0);
                _resourceStorage.Add<ReplacementHint>(3);
                _resourceStorage.Add<RotateHint>(3);
            }
        }
    }
}