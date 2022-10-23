#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [CreateAssetMenu(menuName ="SeweralIdeas/ReplayableEffects/EffectPool")]
    public class EffectPool : ScriptableObject
    {
        public ReplayableEffect prefab;
        private Stack<PooledEffect> m_availableStack = new Stack<PooledEffect>();
        private HashSet<PooledEffect> m_allInstances = new HashSet<PooledEffect>();
        private GameObject m_gameObject;

        public event System.Action onDestroy;
        /*
        public List<PooledEffect.State> Serialize()
        {
            var list = new List<PooledEffect.State>();
            foreach ( var inst in m_allInstances )
            {
                if ( !inst.IsActive() ) continue;
                list.Add(inst.Serialize());
            }
            return list;
        }
        
        public void Deserialize( List<PooledEffect.State> data )
        {
            foreach ( var state in data )
            {
                PlayEffect(state.position, state.rotation, state.time);
            }
        }
        */
        public void DestroyAll()
        {
            foreach (var inst in m_allInstances)
            {
                if(inst)
                    Destroy(inst.gameObject);
            }
            m_availableStack.Clear();
            m_allInstances.Clear();
        }

        public void GetActiveEffects(List<PooledEffect> list)
        {
            list.Clear();
            foreach ( var inst in m_allInstances )
                if ( inst.IsActive() )
                    list.Add(inst);
        }
        

        public void PlayEffect( Transform where, float fwd = 0 )
        {
            PlayEffect(where.position, where.rotation, fwd);
        }

        public void PlayEffect( Vector3 position, Quaternion rotation, float fwd = 0)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
            EnsureGoExists();
#endif
            PooledEffect instance;
            if ( m_availableStack.Count > 0 )
            {
                instance = m_availableStack.Pop();
                instance.transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                var effInstance = Instantiate(prefab, position, rotation, m_gameObject.transform);
                instance = effInstance.gameObject.AddComponent<PooledEffect>();
                instance.effect = effInstance;
                instance.pool = this;
                m_allInstances.Add(instance);
            }

            instance.Play();
            if ( fwd > 0 )
                instance.Fwd(fwd);

        }

        internal void ReturnEffect( PooledEffect effect )
        {
            m_availableStack.Push(effect);
        }

        private void EnsureGoExists()
        {
            if (m_gameObject == null)
            {
                m_gameObject = new GameObject("EffectPool(" + name + ")");
                DontDestroyOnLoad(m_gameObject);
                m_gameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            }
        }

        private void Destroy()
        {
            DestroyAll();
            Destroy(m_gameObject);
            m_gameObject = null;
            onDestroy?.Invoke();
        }

        protected void OnDestroy()
        {
            Destroy();
        }


#if UNITY_EDITOR
        protected void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += PlaymodeChanged;
        }

        protected void OnDisable()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= PlaymodeChanged;
        }

        private void PlaymodeChanged(UnityEditor.PlayModeStateChange change)
        {
            switch (change)
            {
                case UnityEditor.PlayModeStateChange.EnteredPlayMode:
                    EnsureGoExists();
                    break;
                case UnityEditor.PlayModeStateChange.ExitingPlayMode:
                    Destroy();
                    break;
            }
        }
#else
        protected void OnEnable()
        {
            EnsureGoExists();
        }
#endif
    }

}
#endif