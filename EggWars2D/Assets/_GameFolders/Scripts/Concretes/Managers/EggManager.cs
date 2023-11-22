using Cysharp.Threading.Tasks;
using EggWars2D.Controllers;
using EggWars2D.Enums;
using Unity.Netcode;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class EggManager : NetworkBehaviour
    {
        [SerializeField] EggController _prefab;

        EggController _eggController;

        public static EggManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        async void OnEnable()
        {
            while (GameManager.Instance == null)
            {
                await UniTask.Yield();
            }

            GameManager.Instance.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void OnDisable()
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
                case StateEnum.Lose:
                    DeleteEgg();
                    break;
                case StateEnum.Win:
                    DeleteEgg();
                    break;
            }
        }

        public void ReuseEgg()
        {
            if (!IsServer) return;

            if (transform.childCount <= 0) return;

            transform.GetChild(0).GetComponent<EggController>().Reuse();
        }

        void SpawnEgg()
        {
            if (!IsServer || _eggController != null) return;

            var eggInstance = Instantiate(_prefab, Vector3.up * 5f, Quaternion.identity);

            eggInstance.GetComponent<NetworkObject>().Spawn();
            eggInstance.transform.SetParent(transform);
            _eggController = eggInstance;
            //SpawnEggServerRpc();
        }

        void DeleteEgg()
        {
            if (!IsServer) return;
            
            _eggController.GetComponent<NetworkObject>().Despawn();
            Destroy(_eggController);
            _eggController = null;
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