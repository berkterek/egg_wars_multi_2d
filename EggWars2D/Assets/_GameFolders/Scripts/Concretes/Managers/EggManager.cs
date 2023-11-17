using EggWars2D.Controllers;
using EggWars2D.Enums;
using Unity.Netcode;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class EggManager : NetworkBehaviour
    {
        [SerializeField] EggController _prefab;

        public override void OnNetworkSpawn()
        {
            GameManager.Instance.OnGameStateChanged += HandleOnGameStateChanged;
        }

        public override void OnNetworkDespawn()
        {
            GameManager.Instance.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        void HandleOnGameStateChanged(StateEnum gameState)
        {
            switch (gameState)
            {
                case StateEnum.Game:
                    SpawnEgg();
                    break;
            }
        }

        void SpawnEgg()
        {
            if (!IsServer) return;

            var eggInstance = Instantiate(_prefab, Vector3.up * 5f, Quaternion.identity);

            eggInstance.GetComponent<NetworkObject>().Spawn();
            //SpawnEggServerRpc();
        }

        // [ServerRpc]
        // void SpawnEggServerRpc()
        // {
        //     SpawnEggClientRpc();
        // }
        //
        // [ClientRpc]
        // void SpawnEggClientRpc()
        // {
        //     var eggInstance = Instantiate(_prefab, Vector3.up * 5f, Quaternion.identity);
        // }
    }    
}