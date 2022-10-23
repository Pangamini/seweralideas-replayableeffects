#if !UNITY_SERVER
using UnityEngine;

namespace SeweralIdeas.ReplayableEffects
{
    [AddComponentMenu("SeweralIdeas/ReplayableEffects/RandomRotation")]
    public class RandomRotation : Playable
    {
        public bool RotateX;
        public bool RotateY;
        public bool RotateZ = true;

        // Update is called once per frame
        public override void Play()
        {
            var rotateVector = Vector3.zero;
            if ( RotateX )
                rotateVector.x = Random.Range(0, 360);
            if ( RotateY )
                rotateVector.y = Random.Range(0, 360);
            if ( RotateZ )
                rotateVector.z = Random.Range(0, 360);
            transform.Rotate(rotateVector);
        }

        public override void Fwd( float deltaTime )
        {
        }
    }
}
#endif