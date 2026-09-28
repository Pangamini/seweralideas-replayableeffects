using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace SeweralIdeas.ReplayableEffects
{
    public class PrewarmEffect : MonoBehaviour
    {
        [SerializeField]
        private EffectPool _effectPool;

        [SerializeField]
        private int _instanceCount = 5;
        
        void Start()
        {
            for(int i = 0; i<_instanceCount; ++i)
                _effectPool.PlayEffect(transform);

            using (ListPool<PooledEffect>.Get(out var list))
            {
                _effectPool.GetActiveEffects(list);
                foreach (PooledEffect pooledEffect in list)
                    pooledEffect.Stop();
            }
            
            StartCoroutine(Render());
        }
        
        private IEnumerator Render()
        {
            PooledEffect instance = _effectPool.PlayEffect(transform, _effectPool.Prefab.Duration *0.5f);
            yield return new WaitForEndOfFrame();
            instance.Stop();
        }
    }
}
