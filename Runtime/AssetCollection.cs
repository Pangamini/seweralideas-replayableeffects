﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    public class AssetCollection<T> : ScriptableObject// where T:Object
    {
        [SerializeField] private T[] m_collection;

        public T this[int index] => m_collection[index];
        public int Count => m_collection.Length;

        public T PickRandom()
        {
            if (m_collection.Length == 0)
                return default;
            return m_collection[Random.Range(0, m_collection.Length)];
        }

    }
}