#if !UNITY_SERVER
using System.Collections.Generic;
using SeweralIdeas.ObjectPooling;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SeweralIdeas.ReplayableEffects
{
    /// <summary>
    /// A visual effect made of <see cref="EffectComponent"/>s, which all play and fast-forward together, and which is
    /// over after its duration. It is a <see cref="Poolable"/>: to play one, call <see cref="Spawn(Scene, Vector3, Quaternion, float)"/>
    /// on the prefab, and a pooled instance plays and releases itself when it is over.
    /// </summary>
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableEffect")]
    public class ReplayableEffect : Poolable
    {
        [FormerlySerializedAs("duration")]
        [SerializeField]
        private float m_duration = 1f;

        private readonly List<EffectComponent> m_components = new();
        private float _timePassed;
        private bool  _playing;

        internal void AddEffectComponent(EffectComponent effectComponent)
        {
            m_components.Add(effectComponent);
        }

        internal void RemoveEffectComponent(EffectComponent effectComponent)
        {
            m_components.Remove(effectComponent);
        }

        public float Duration => m_duration;

        /// <summary>Whether the effect is playing, i.e. has not run out its duration or been stopped.</summary>
        public bool IsPlaying => _playing;

        /// <summary>
        /// Plays the effect at <paramref name="where"/>. Call it on the prefab (or any effect asset): the instance that
        /// plays is taken from the scene's pool of the prefab, and returns to it when the effect is over.
        /// </summary>
        public ReplayableEffect Spawn(Scene scene, Transform where, float fastForward = 0f)
            => Spawn(scene, where.position, where.rotation, fastForward);

        /// <inheritdoc cref="Spawn(Scene, Transform, float)"/>
        /// <param name="fastForward">How far into the effect it starts, in seconds.</param>
        public ReplayableEffect Spawn(Scene scene, Vector3 position, Quaternion rotation, float fastForward = 0f)
        {
            if(!Application.isPlaying)
                return null;

            ObjectPoolManager manager = ObjectPoolManager.GetOrCreate(scene);

            // Take it inactive, put it in place, and activate it. Its components have had their Awake by then.
            ReplayableEffect instance = manager.GetPool(this).Take<ReplayableEffect>(active: false, parent: manager.transform);
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);

            instance.Play();
            instance.FastForward(fastForward);
            return instance;
        }

        /// <summary>(Re)starts the effect, here and now, from the beginning.</summary>
        public void Play()
        {
            _timePassed = 0f;
            _playing = true;
            foreach ( var comp in m_components )
            {
                comp.Play();
            }
        }

        /// <summary>Ends the effect early. A pooled instance returns to its pool; any other (one just placed in a scene to look at) stays.</summary>
        public void Stop()
        {
            if(!_playing)
                return;

            _playing = false;
            if(TryGetComponent(out PoolReference _))
                Release();
        }

        public void FastForward( float deltaTime )
        {
            if ( deltaTime <= 0 )
            {
                return;
            }

            _timePassed += deltaTime;
            foreach ( var comp in m_components )
            {
                comp.FastForward(deltaTime);
            }
        }

        protected void Update()
        {
            if(!_playing)
                return;

            _timePassed += Time.deltaTime;
            if(_timePassed >= m_duration)
                Stop();
        }

        protected override void OnDespawn()
        {
            _playing = false;
            base.OnDespawn();
        }
    }
}
#endif
