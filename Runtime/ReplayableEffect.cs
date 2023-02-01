#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using SeweralIdeas.UnityUtils.Drawers;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableEffect")]
    public class ReplayableEffect : MonoBehaviour
    {
        [Button(new[]{nameof(Play), nameof(FindComponents)})]
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
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, nameof(FindComponents));
#endif
            m_components = GetComponentsInChildren<Playable>();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        void Reset()
        {
            FindComponents();
        }
    }
}
#endif