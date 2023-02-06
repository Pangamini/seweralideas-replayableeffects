#if !UNITY_SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableEffect")]
    public class ReplayableEffect : MonoBehaviour
    {
        [FormerlySerializedAs("duration")]
        [SerializeField]
        private float m_duration = 1f;

        private readonly List<EffectComponent> m_components = new();

        internal void AddEffectComponent(EffectComponent effectComponent)
        {
            m_components.Add(effectComponent);
        }
        
        internal void RemoveEffectComponent(EffectComponent effectComponent)
        {
            m_components.Remove(effectComponent);
        }
        
        public float Duration => m_duration;
        
        public void Play()
        {
            foreach ( var comp in m_components )
            {
                comp.Play();
            }
        }
        
        public void FastForward( float deltaTime )
        {
            if ( deltaTime <= 0 )
            {
                return;
            }
            foreach ( var comp in m_components )
            {
                comp.FastForward(deltaTime);
            }
        }
    }
}
#endif