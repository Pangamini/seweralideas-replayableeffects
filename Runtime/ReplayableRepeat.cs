#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/ReplayableRepeat")]
    [RequireComponent(typeof(ReplayableEffect))]
    public class ReplayableRepeat : MonoBehaviour
    {
        //[MinMaxSlider(0, 20)]
        public Vector2 randomIntervals = new Vector2(10, 16);
        private ReplayableEffect m_playable;
        
        void Awake()
        {
            m_playable = GetComponent<ReplayableEffect>();
        }
        
        void OnEnable()
        {
            StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            yield return new WaitForSeconds(Random.Range(0, randomIntervals.y - randomIntervals.x));
            while ( true )
            {
                yield return new WaitForSeconds(Random.Range(randomIntervals.x, randomIntervals.y)); ;
                m_playable.Play();
            }
        }
    }
}
#endif