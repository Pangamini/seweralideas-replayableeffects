#if !UNITY_SERVER
using System.Collections.Generic;
using SeweralIdeas.UnityUtils;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/EffectsManager")]
    public class EffectsManager : SingletonBehaviour<EffectsManager>
    {
        private readonly Dictionary<ReplayableEffect, EffectPool> m_poolDict = new Dictionary<ReplayableEffect, EffectPool>();
        
        public EffectPool GetPool( ReplayableEffect prefab )
        {
            if (m_poolDict.TryGetValue(prefab, out var pool))
                return pool;
            
            pool = ScriptableObject.CreateInstance<EffectPool>();
            pool.Prefab = prefab;
            pool.Destroyed += ()=>{ OnPoolDestroyed(prefab); };
            m_poolDict.Add(prefab, pool);
            return pool;
        }

        private void OnPoolDestroyed( ReplayableEffect poolPrefab )
        {
            m_poolDict.Remove(poolPrefab);
        }
        
        public void DestroyAllEffects()
        {
            foreach ( var pair in m_poolDict )
            {
                if(pair.Value)
                    pair.Value.DestroyAll();
            }
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            DestroyAllEffects();
        }

    }
}
#endif