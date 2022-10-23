#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableEffect")]
    public class ReplayableEffect : MonoBehaviour
    {
        //[Button(new string[]{"Play", "FindComponents"})]
        public float duration = 1f;
        public Playable[] m_components;

        public void Play()
        {
            foreach ( var comp in m_components )
                comp.Play();
        }
        
        public void Fwd( float deltaTime )
        {
            if ( deltaTime <= 0 ) return;
            foreach ( var comp in m_components )
                comp.Fwd(deltaTime);
        }

        public void FindComponents()
        {
            m_components = GetComponentsInChildren<Playable>();
        }

        void Reset()
        {
            FindComponents();
        }
    }
}
#endif