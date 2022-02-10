using UnityEngine;

namespace GameScripts
{
    public class SetTargetFramerate : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}