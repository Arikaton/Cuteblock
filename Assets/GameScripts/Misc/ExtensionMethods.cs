using UnityEngine;

namespace GameScripts.Misc
{
    public static class ExtensionMethods
    {
        public static void DestroyAllChildren(this Transform transform) 
        {
            for (int i = transform.childCount - 1; i >= 0; i--) {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();
        }
    }
}