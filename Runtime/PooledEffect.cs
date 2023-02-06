#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    public class PooledEffect : MonoBehaviour
    {
        internal ReplayableEffect Effect;
        internal EffectPool Pool;

        private bool m_active;
        private float timePassed;

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
            if ( timePassed >= Effect.Duration )
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
            Pool.ReturnEffect(this);
        }

        public void Play()
        {
            timePassed = 0;
            gameObject.SetActive(true);
            m_active = true;
            Effect.Play();
        }

        public void Fwd( float deltaTime )
        {
            timePassed += deltaTime;
            Effect.FastForward(deltaTime);
        }
    }
}
#endif