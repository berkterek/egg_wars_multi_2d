using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EggWars2D.Controllers
{
    public class EggController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigidbody2D;
        [SerializeField] float _bounceVelocity = 5f;

        bool _isAlive;

        public static event System.Action OnHit;
        public static event System.Action OnFellWater;

        void OnValidate()
        {
            if (_rigidbody2D == null) GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _isAlive = true;
            
            WaitAndFallAsync();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isAlive) return;

            if (other.collider.TryGetComponent(out PlayerController playerController))
            {
                Bounce(other.GetContact(0).normal);
                OnHit?.Invoke();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(_isAlive);
            if (!_isAlive) return;

            if (other.CompareTag("Water"))
            {
                _isAlive = false;
                OnFellWater?.Invoke();
            }
        }

        void Bounce(Vector3 normal)
        {
            _rigidbody2D.velocity = normal * _bounceVelocity;
        }

        private async void WaitAndFallAsync()
        {
            var gravityScale = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;

            await UniTask.Delay(2000);

            _rigidbody2D.gravityScale = gravityScale;
        }

        public void Reuse()
        {
            transform.position = Vector3.up * 5f;
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.angularVelocity = 0f;
            _isAlive = true;
            
            WaitAndFallAsync();            
        }
    }
}