using System;
using UnityEngine;

namespace GameScripts.UI
{
    public class CameraSizeScaler : MonoBehaviour
    {
        private void Awake()
        {
            var camera = GetComponent<Camera>();
            camera.orthographicSize = 5 / camera.aspect;
        }
    }
}