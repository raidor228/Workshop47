using System;
using System.Collections;
using UnityEngine;

namespace Workshop47.Scripts.Utils
{
    public class Coroutines : MonoBehaviour
    {
        public static Coroutines Instance => _instance;

        private static Coroutines _instance;
        
        public static void Invoke(Action action, float delay)
        {
            Instance.StartCoroutine(Instance.InvokeRoutine(action, delay));
        }

        public static IEnumerator Invoke(IEnumerator routine, float delay)
        {
            yield return new WaitForSeconds(delay);
            Instance.StartCoroutine(routine);
        }
        
        private IEnumerator InvokeRoutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        
        private void Awake()
        {
            _instance = this;
        }
    }
}