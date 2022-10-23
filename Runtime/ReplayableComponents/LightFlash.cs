#if !UNITY_SERVER
using System;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/LightFlash")]
    public class LightFlash : Playable
    {
        public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;

        private bool canUpdate;
        private float m_time = Mathf.Infinity;
        private Light lightSource;

        private void Awake()
        {
            lightSource = GetComponent<Light>();
            lightSource.intensity = LightCurve.Evaluate(0);
        }

        public override void Play()
        {
            m_time = 0f;
            enabled = true;
        }
        
        public override void Fwd( float deltaTime )
        {
            m_time += deltaTime;
        }

        private void Update()
        {
            var eval = LightCurve.Evaluate(m_time / GraphTimeMultiplier) * GraphIntensityMultiplier;
            lightSource.intensity = eval;
            
            if ( m_time >= GraphTimeMultiplier )
                enabled = false;
            m_time += Time.deltaTime;
        }

        private void OnEnable()
        {
            lightSource.enabled = true;
        }

        private void OnDisable()
        {
            lightSource.enabled = false;
        }
    }
}
#endif