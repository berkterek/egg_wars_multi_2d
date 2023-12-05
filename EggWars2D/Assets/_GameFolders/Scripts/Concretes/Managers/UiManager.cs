using Cysharp.Threading.Tasks;
using EggWars2D.Enums;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EggWars2D.Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] GameObject _connectionObject;
        [SerializeField] GameObject _waitingObject;
        [SerializeField] GameObject _gameObject;
        [SerializeField] GameObject _winObject;
        [SerializeField] GameObject _loseObject;
        [SerializeField] Button _hostButton;
        [SerializeField] Button _clientButton;
        [SerializeField] Button _playButton;

        void Start()
        {
            _connectionObject.SetActive(true);
            _gameObject.SetActive(false);
            _waitingObject.SetActive(false);
            _winObject.SetActive(false);
            _loseObject.SetActive(false);
        }

        async void OnEnable()
        {
            _playButton.onClick.AddListener(HandleOnPlayButtonClicked);
            _hostButton.onClick.AddListener(HandleOnHostButtonClicked);
            _clientButton.onClick.AddListener(HandleOnClientButtonClicked);

            while (GameManager.Instance == null) await UniTask.Yield();

            GameManager.Instance.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void OnDisable()
        {
            _playButton.onClick.RemoveListener(HandleOnPlayButtonClicked);
            _hostButton.onClick.RemoveListener(HandleOnHostButtonClicked);
            _clientButton.onClick.RemoveListener(HandleOnClientButtonClicked);

            if (GameManager.Instance == null) return;
            GameManager.Instance.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        async void HandleOnHostButtonClicked()
        {
            ShowWaitingUi();
            //NetworkManager.Singleton.StartHost();

            await RelayManager.Instance.ConfigureTransportAdnStartAsHostAsync();
        }

        async void HandleOnClientButtonClicked()
        {
            ShowWaitingUi();

            // string ipAddress = IpManager.Instance.GetInputIp();
            // UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            // utp.SetConnectionData(ipAddress,7777);
            // NetworkManager.Singleton.StartClient();

            await RelayManager.Instance.ConfigureTransportAdnStartAsClientAsync();
        }
        
        void HandleOnPlayButtonClicked()
        {
            ShowWaitingUi();

            MatchMakingManager.Instance.PlayButtonCallback();
        }

        private void ShowWaitingUi()
        {
            _waitingObject.SetActive(true);
            _connectionObject.SetActive(false);
            _gameObject.SetActive(false);
        }

        private void SetActiveGameUi()
        {
            _waitingObject.SetActive(false);
            _connectionObject.SetActive(false);
            _gameObject.SetActive(true);
        }

        private void ShowWinPanel()
        {
            _gameObject.SetActive(false);
            _winObject.SetActive(true);
        }

        private void ShowLosePanel()
        {
            _gameObject.SetActive(false);
            _loseObject.SetActive(true);
        }

        public async void NextButton()
        {
            await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            NetworkManager.Singleton.Shutdown();
        }
        
        void HandleOnGameStateChanged(StateEnum gameState)
        {
            switch (gameState)
            {
                case StateEnum.Game:
                    SetActiveGameUi();
                    break;
                case StateEnum.Win:
                    ShowWinPanel();
                    break;
                case StateEnum.Lose:
                    ShowLosePanel();
                    break;
            }
        }
    }    
}

