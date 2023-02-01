#if !UNITY_SERVER
using SeweralIdeas.UnityUtils.Drawers;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableParticles")]
    public class ReplayableParticles : Playable
    {
        [Button("Configure particles", nameof(ConfigureParticles))]
        public bool dummyVar;
        public ParticleSystem[] particles;

        private void ConfigureParticles()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObjects(particles, "Configure Particles");
#endif

            foreach ( var par in particles )
            {
                var main = par.main;
                var parName = par.ToString();
                if ( main.playOnAwake )
                {
                    main.playOnAwake = false;
                    Debug.Log(parName + " main.playOnAwake set to false");
                }
                if ( main.loop )
                {
                    main.loop = false;
                    Debug.Log(parName + " main.loop set to false");
                }
            }
#if UNITY_EDITOR
            foreach(var particle in particles)
                UnityEditor.EditorUtility.SetDirty(particle);
#endif
        }
        
        public void Reset()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }
        
        public override void Play()
        {
            foreach ( var par in particles )
            {
                par.Play(false);
            }
        }

        public override void Fwd( float deltaTime )
        {
            foreach ( var par in particles )
            {
                par.Simulate(deltaTime, false, false);
                par.Play(false);

            }
        }
    }
}
#endif