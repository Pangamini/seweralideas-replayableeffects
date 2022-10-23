#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/PooledEffect")]
    public class PooledEffect : MonoBehaviour
    {
        public ReplayableEffect effect;
        public float timePassed;
        public EffectPool pool;
        private bool m_active;
        /*
        [Serializable]
        public struct State
        {
            [Save] public Vector3 position;
            [Save] public Quaternion rotation;
            [Save] public float time;
        }

        public State Serialize()
        {
            return new State()
            {
                position = transform.position,
                rotation = transform.rotation,
                time = timePassed
            };
        }
        */
        public bool IsActive()
        {
            return m_active;
        }

        private void Update()
        {
            if ( timePassed >= effect.duration )
            {
                Stop();
            }
            timePassed += Time.deltaTime;
        }

        private void Stop()
        {
            if ( !IsActive() ) return;
            gameObject.SetActive(false);
            m_active = false;
            pool.ReturnEffect(this);
        }

        public void Play()
        {
            timePassed = 0;
            gameObject.SetActive(true);
            m_active = true;
            effect.Play();
        }

        public void Fwd( float deltaTime )
        {
            timePassed += deltaTime;
            effect.Fwd(deltaTime);
        }
    }
}
#endif