using EggWars2D.Controllers;
using EggWars2D.Enums;
using EggWars2D.Managers;
using Unity.Netcode;

namespace EggWars2D.Handlers
{
    public class PlayerSelectorHandler : NetworkBehaviour
    {
        bool _isHostTurn;
        
        public override void OnNetworkSpawn()
        {
            NetworkManager.OnServerStarted += HandleOnNetworkStarted;
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.OnServerStarted -= HandleOnNetworkStarted;
            GameManager.Instance.OnGameStateChanged -= HandleOnGameStateChanged;
            EggController.OnHit -= HandleOnHit;
        }

        void HandleOnNetworkStarted()
        {
            if (!IsServer) return;

            GameManager.Instance.OnGameStateChanged += HandleOnGameStateChanged;
            EggController.OnHit += HandleOnHit;
        }

        void HandleOnHit()
        {
            SwitchPlayers();
        }

        void HandleOnGameStateChanged(StateEnum gameState)
        {
            switch (gameState)
            {
                case StateEnum.Game:
                    Initialized();
                    break;
            }
        }

        void Initialized()
        {
            PlayerStateManager[] playerStateManagers = FindObjectsOfType<PlayerStateManager>();

            foreach (var playerStateManager in playerStateManagers)
            {
                if (playerStateManager.GetComponent<NetworkObject>().IsOwnedByServer)
                {
                    if (_isHostTurn)
                    {
                        playerStateManager.EnablePlayer();
                    }
                    else
                    {
                        playerStateManager.DisablePlayer();
                    }
                }
                else
                {
                    if (_isHostTurn)
                    {
                        playerStateManager.DisablePlayer();
                    }
                    else
                    {
                        playerStateManager.EnablePlayer();
                    }
                }
            }
        }
        
        void SwitchPlayers()
        {
            _isHostTurn = !_isHostTurn;
            
            Initialized();
        }
    }
}