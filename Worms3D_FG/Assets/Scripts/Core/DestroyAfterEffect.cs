using UnityEngine;

namespace WormsGame.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        ParticleSystem particle;
        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }
        void Update()
        {
            if (!particle.IsAlive())
                Destroy(gameObject);
        }
    }
    
}
