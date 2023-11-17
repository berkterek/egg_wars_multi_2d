using Cysharp.Threading.Tasks;
using EggWars2D.Enums;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            
            Application.targetFrameRate = _targetFrame;
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

        async void Start()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1));
            await SceneManager.LoadSceneAsync(1);
            _gameState = StateEnum.Menu;
        }
        
        void HandleOnServerStarted()
        {
            if (!IsServer) return;

            NetworkManager.Singleton.OnClientConnectedCallback += HandleOnClientConnectedCallback;
        }

        void HandleOnClientConnectedCallback(ulong clientId)
        {
            _connectedPlayers++;

            if (_connectedPlayers >= 2)
            {
                StartGame();
            }
        }

        void StartGame()
        {
            StartGameClientRpc();
        }

        [ClientRpc]
        void StartGameClientRpc()
        {
            _gameState = StateEnum.Game;
            OnGameStateChanged?.Invoke(_gameState);
        }
    }    
}