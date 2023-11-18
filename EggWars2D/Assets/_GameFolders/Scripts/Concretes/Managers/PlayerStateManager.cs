using Unity.Netcode;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class PlayerStateManager : NetworkBehaviour
    {
        [SerializeField] Collider2D _collider;
        [SerializeField] SpriteRenderer[] _spriteRenderers;
        
        public void EnablePlayer()
        {
            EnableClientRpc();
        }

        [ClientRpc]
        private void EnableClientRpc()
        {
            _collider.enabled = true;
            
            foreach (var spriteRenderer in _spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }

        public void DisablePlayer()
        {
            DisableClientRpc();
        }

        [ClientRpc]
        private void DisableClientRpc()
        {
            _collider.enabled = false;
            
            foreach (var spriteRenderer in _spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = 0.2f;
                spriteRenderer.color = color;
            }
        }
    }
}