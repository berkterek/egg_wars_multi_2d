using Cysharp.Threading.Tasks;
using EggWars2D.Enums;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace EggWars2D.Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] GameObject _connectionObject;
        [SerializeField] GameObject _waitingObject;
        [SerializeField] GameObject _gameObject;
        [SerializeField] Button _hostButton;
        [SerializeField] Button _clientButton;

        void Start()
        {
            _connectionObject.SetActive(true);
            _gameObject.SetActive(false);
            _waitingObject.SetActive(false);
        }

        async void OnEnable()
        {
            _hostButton.onClick.AddListener(HandleOnHostButtonClicked);
            _clientButton.onClick.AddListener(HandleOnClientButtonClicked);

            while (GameManager.Instance == null) await UniTask.Yield();

            GameManager.Instance.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void OnDisable()
        {
            _hostButton.onClick.RemoveListener(HandleOnHostButtonClicked);
            _clientButton.onClick.RemoveListener(HandleOnClientButtonClicked);
            GameManager.Instance.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        void HandleOnHostButtonClicked()
        {
            ShowWaitingUi();
            NetworkManager.Singleton.StartHost();
        }

        void HandleOnClientButtonClicked()
        {
            ShowWaitingUi();
            NetworkManager.Singleton.StartClient();
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
        
        void HandleOnGameStateChanged(StateEnum gameState)
        {
            switch (gameState)
            {
                case StateEnum.Game:
                    SetActiveGameUi();
                    break;
                case StateEnum.Win:
                    break;
                case StateEnum.Lose:
                    break;
                default:
                    break;
            }
        }
    }    
}

