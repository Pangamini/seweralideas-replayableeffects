#if !UNITY_SERVER
using UnityEngine;
using SeweralIdeas.UnityUtils.Drawers;

#pragma warning disable CS0649  
namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableSound")]
    public class ReplayableSound : Playable
    {
        [Button("Configure audio sources", "ConfigureSources")]
        public AudioSource source;
        public AudioClipCollection clipCollection;
        [SerializeField] private bool loop;
        [Condition("loop"), SerializeField] private AnimationCurve volumeOverTime;
        [Condition("loop"), SerializeField] private float duration = 5f;
        [Tooltip("When set to true, each play will produce new voice. When false, each play will stop previous voice. Game load can only fast-forward when set to false")]
        [Condition("loop", invert = true)]
        public bool playAsNew = false;
        private Vector2 m_volumeOverTime_timeRange;

        private float m_progress;
        private void ConfigureSources()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(source, "Undo Configure Audio Sources");
#endif
            if ( source.playOnAwake )
            {
                source.playOnAwake = false;
                Debug.Log(source.ToString()+" playOnAwake set to false");
            }
        }

        private void Start()
        {
            if (volumeOverTime.length > 0)
            {
                var keys = volumeOverTime.keys;
                m_volumeOverTime_timeRange.x = keys[0].time;
                if (keys.Length > 1)
                    m_volumeOverTime_timeRange.y = keys[keys.Length - 1].time;
                else
                    m_volumeOverTime_timeRange.y = m_volumeOverTime_timeRange.x;
            }
        }

        public void Reset()
        {
            source = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public override void Play()
        {
            if ( !gameObject.activeInHierarchy ) return;
            AudioClip clip;
            float pitch;
            if (clipCollection != null)
            {
                pitch = clipCollection.PickRandomPitch();
                clip = clipCollection.PickRandom();
            }
            else
            {
                pitch = source.pitch;
                clip = source.clip;
            }

            source.clip = clip;
            source.loop = loop;
            if ( loop )
            {
                m_progress = 0;
                enabled = true;
            }

            source.pitch = pitch;
            if ( playAsNew && !loop)         
                source.PlayOneShot(clip);
            else
                source.Play();
        }

        private void Update()
        {
            if ( !loop || m_progress > duration )
            {
                enabled = false;
                return;
            }

            m_progress += Time.deltaTime;
            var relativeProgress = m_progress / duration;          
            source.volume = volumeOverTime.Evaluate(Mathf.Lerp(m_volumeOverTime_timeRange.x, m_volumeOverTime_timeRange.y, relativeProgress));

        }

        private void OnDisable()
        {
            if(loop)
                source.Stop();
        }

        public override void Fwd( float deltaTime )
        {
            m_progress += deltaTime;
            if ( source.clip == null )  //this may happen when playAsNew is false
                return;
            float newTime = source.time + deltaTime;
            if ( newTime >= duration)
                source.Stop();
            else
                source.time = Mathf.Repeat( newTime, source.clip.length);
        }
    }
}
#endif