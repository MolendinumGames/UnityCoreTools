using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Pooling
{
    public class LocalPoolController : MonoBehaviour
    {
        [SerializeField]
        bool createOnAwake = true;
        public bool CreateOnAwake
        {
            get => createOnAwake;
            set => createOnAwake = value;
        }

        [SerializeField]
        GameObjectPool pool;

        void Start()
        {
            if (CreateOnAwake)
                InitializePool();
        }

        public void InitializePool() =>
            pool.Initialize();

        public GameObject GetPooledGameObject() =>
            pool.RequestObject();

        public GameObjectPool GetPool() => pool;
    }
}
