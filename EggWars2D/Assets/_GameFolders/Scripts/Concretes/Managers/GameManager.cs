using EggWars2D.Enums;
using Unity.Netcode;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] int _targetFrame = 30;

        StateEnum _gameState;
        int _connectedPlayers;

        public event System.Action<StateEnum> OnGameStateChanged;
        public static GameManager Instance { get; private set; }

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

            Application.targetFrameRate = _targetFrame;
        }

        void Start()
        {
            _gameState = StateEnum.Menu;
        }

        public override void OnNetworkSpawn()
        {
            NetworkManager.OnServerStarted += HandleOnServerStarted;
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.OnServerStarted -= HandleOnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleOnClientConnectedCallback;
        }

        void HandleOnServerStarted()
        {
            if (!IsServer) return;

            NetworkManager.Singleton.OnClientConnectedCallback += HandleOnClientConnectedCallback;
            NetworkManager.Singleton.OnServerStopped += HandleOnServerStopped;
        }

        void HandleOnClientConnectedCallback(ulong clientId)
        {
            _connectedPlayers++;

            if (_connectedPlayers >= 2)
            {
                StartGame();
            }
        }

        void HandleOnServerStopped(bool value)
        {
            _connectedPlayers = 0;
        }

        void StartGame()
        {
            StartGameClientRpc();
        }

        [ClientRpc]
        void StartGameClientRpc()
        {
            SetGameState(StateEnum.Game);
        }

        public void SetGameState(StateEnum stateEnum)
        {
            _gameState = stateEnum;
            OnGameStateChanged?.Invoke(_gameState);
        }
    }
}