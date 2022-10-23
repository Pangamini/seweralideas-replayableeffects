#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SeweralIdeas.ReplayableEffects
{
    public abstract class Playable : MonoBehaviour
    {
        public abstract void Play();
        public abstract void Fwd( float deltaTime );
    }
    
}
#endif