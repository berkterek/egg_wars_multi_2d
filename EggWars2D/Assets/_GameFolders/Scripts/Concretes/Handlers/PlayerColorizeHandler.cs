using Unity.Netcode;
using UnityEngine;

namespace EggWars2D.Handlers
{
    public class PlayerColorizeHandler : NetworkBehaviour
    {
        [SerializeField] SpriteRenderer[] _spriteRenderers;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner || IsServer) return;
            // if (!IsOwner) return;
            
            //all clients are red
            ColorizeServerRpc(Color.red);
        }

        [ServerRpc]
        private void ColorizeServerRpc(Color color)
        {
            ColorizeClientRpc(color);
        }
        
        [ClientRpc]
        private void ColorizeClientRpc(Color color)
        {
            foreach (var spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.color = color;
            }
        }
    }    
}

