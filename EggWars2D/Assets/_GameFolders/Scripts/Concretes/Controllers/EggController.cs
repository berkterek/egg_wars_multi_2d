using UnityEngine;

namespace EggWars2D.Controllers
{
    public class EggController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigidbody2D;
        [SerializeField] float _bounceVelocity = 5f;

        public static event System.Action OnHit;
        
        void OnValidate()
        {
            if (_rigidbody2D == null) GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out PlayerController playerController))
            {
                Bounce(other.GetContact(0).normal);
                OnHit?.Invoke();
            }
        }

        void Bounce(Vector3 normal)
        {
            _rigidbody2D.velocity = normal * _bounceVelocity;
        }
    }
}