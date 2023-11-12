using Cysharp.Threading.Tasks;
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

        void OnEnable()
        {
            _hostButton.onClick.AddListener(HandleOnHostButtonClicked);
            _clientButton.onClick.AddListener(HandleOnClientButtonClicked);
        }

        void OnDisable()
        {
            _hostButton.onClick.RemoveListener(HandleOnHostButtonClicked);
            _clientButton.onClick.RemoveListener(HandleOnClientButtonClicked);
        }

        void HandleOnHostButtonClicked()
        {
            SetActiveWaitingUi();
            NetworkManager.Singleton.StartHost();
        }

        void HandleOnClientButtonClicked()
        {
            SetActiveWaitingUi();
            NetworkManager.Singleton.StartClient();
        }

        private void SetActiveWaitingUi()
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
    }    
}

