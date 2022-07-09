using UnityEngine;

namespace OVAWORKT
{
    public static class CoroutineUtility
    {
        public static void EnsureCoroutineStopped(this MonoBehaviour monoBehaviour, ref Coroutine routine)
        {
            if (routine != null)
            {
                monoBehaviour?.StopCoroutine(routine);
                routine = null;
            }
        }
    }
}