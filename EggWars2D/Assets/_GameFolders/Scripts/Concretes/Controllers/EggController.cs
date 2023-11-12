using UnityEngine;

namespace EggWars2D.Controllers
{
    public class EggController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigidbody2D;
        [SerializeField] float _bounceVelocity = 5f;

        void OnValidate()
        {
            if (_rigidbody2D == null) GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            Bounce(other.GetContact(0).normal);
        }

        void Bounce(Vector3 normal)
        {
            _rigidbody2D.velocity = normal * _bounceVelocity;
        }
    }
}