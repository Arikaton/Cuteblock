using System;
using UnityEngine;

namespace GameScripts
{
    public class TargetFramerate : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}