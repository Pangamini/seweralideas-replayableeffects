#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using SeweralIdeas.UnityUtils;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/EffectsManager")]
    public class EffectsManager : SingletonBehaviour<EffectsManager>
    {
        //public List<ReplayableEffect> serializedInstances = new List<ReplayableEffect>();
        private Dictionary<ReplayableEffect, EffectPool> m_poolDict = new Dictionary<ReplayableEffect, EffectPool>();
        
        public EffectPool GetPool( ReplayableEffect prefab )
        {
            EffectPool pool;
            if ( !m_poolDict.TryGetValue(prefab, out pool) )
            {
                pool = ScriptableObject.CreateInstance<EffectPool>();
                pool.prefab = prefab;
                pool.onDestroy += ()=>{ OnPoolDestroyed(prefab); };
                m_poolDict.Add(prefab, pool);
            }
            return pool;
        }

        private void OnPoolDestroyed( ReplayableEffect poolPrefab )
        {
            m_poolDict.Remove(poolPrefab);
        }

        void Add( Transform transf )
        {
            transf.SetParent(transform, true);
        }
        /*
        public Dictionary<string, List<PooledEffect.State>> Serialize()
        {
            Dictionary<string, List<PooledEffect.State>> data = new Dictionary<string, List<PooledEffect.State>>();
            foreach ( var prefab in serializedInstances )
            {
                EffectPool pool;
                if ( !m_poolDict.TryGetValue(prefab, out pool) ) continue;

                var list = pool.Serialize();
                if( list.Count > 0 )
                    data.Add(prefab.name, list);
            }

            return data;
        }
        */
        public void DestroyAllEffects()
        {
            foreach ( var pair in m_poolDict )
                if(pair.Value)
                    pair.Value.DestroyAll();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            DestroyAllEffects();
        }

    }
}
#endif