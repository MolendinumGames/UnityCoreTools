using UnityEngine;

namespace CoreTools
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool quitting = false;
        private static readonly object Lock = new object();
        protected abstract bool Persistent { get; }
        private static T instance;
        public static T Instance
        {
            get
            {
                if (quitting)
                    return null;

                lock (Lock)
                {
                    if (instance == null)
                    {
                        var instances = FindObjectsOfType(typeof(T));
                        if (instances.Length > 0)
                        {
                            instance = instances[0] as T;
                            for (int i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i]);
                            }
                        }
                        if (instance == null)
                        {
                            instance = new GameObject($"{typeof(T)}Singleton").AddComponent<T>();
                        }
                    }
                    return instance;
                }
            }
        }
        protected virtual void Awake()
        {
            if (Persistent)
                DontDestroyOnLoad(gameObject);
        }
        private void OnApplicationQuit() => quitting = true;
    }
}

