using UnityEngine;

namespace GameScripts
{
    public class Singleton<T> : MonoBehaviour
    {
        public static T Instance => _instance;
        private static T _instance;
        private void Awake()
        {
            if (_instance is null)
            {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}