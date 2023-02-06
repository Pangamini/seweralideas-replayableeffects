#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SeweralIdeas.ReplayableEffects
{
    [CreateAssetMenu(menuName ="SeweralIdeas/ReplayableEffects/EffectPool")]
    public class EffectPool : ScriptableObject
    {
        [FormerlySerializedAs("prefab")]
        [SerializeField]
        private ReplayableEffect m_prefab;
        
        private readonly Stack<PooledEffect> m_availableStack = new Stack<PooledEffect>();
        private readonly HashSet<PooledEffect> m_allInstances = new HashSet<PooledEffect>();
        private GameObject m_gameObject;

        public event System.Action Destroyed;

        public ReplayableEffect Prefab
        {
            get => m_prefab;
            set => m_prefab = value;
        }

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

        public void GetActiveEffects(ICollection<PooledEffect> list)
        {
            list.Clear();
            foreach ( var inst in m_allInstances )
            {
                if ( inst.IsActive() )
                {
                    list.Add(inst);
                }
            }
        }
        

        public void PlayEffect( Transform where, float fwd = 0 )
        {
            PlayEffect(where.position, where.rotation, fwd);
        }

        public void PlayEffect( Vector3 position, Quaternion rotation, float fwd = 0)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
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
                var effInstance = Instantiate(m_prefab, position, rotation, m_gameObject.transform);
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
            Destroyed?.Invoke();
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