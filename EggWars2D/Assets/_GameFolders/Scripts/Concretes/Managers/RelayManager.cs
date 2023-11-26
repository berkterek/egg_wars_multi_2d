using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class RelayManager : MonoBehaviour
    {
        const int MAX_CONNECTIONS = 2;
        const string CONNECTION_TYPE = "dtls";

        [SerializeField] TMP_Text _joinCodeText;
        [SerializeField] TMP_InputField _joinCodeField;

        public string RelayJoinCode { get; private set; }
        public static RelayManager Instance { get; private set; }

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

        void Start()
        {
            AuthenticatePlayerAsync();
        }

        async void AuthenticatePlayerAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                var playerId = AuthenticationService.Instance.PlayerId;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log(e.Message);
            }
        }

        /// <summary>
        /// This is host using create join code 
        /// </summary>
        async UniTask<RelayServerData> AllocateRelayServerAndGetJoinCodeAsync(int maxConnection, string region = null)
        {
            Allocation allocation;
            string createJoinCode = string.Empty;

            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection, region);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log("Relay create allocation request failed " + e.Message);
                throw;
            }

            Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server: {allocation.AllocationId}");

            try
            {
                createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                _joinCodeText.SetText(createJoinCode);
            }
            catch
            {
                Debug.Log("Relay create join code request failed");
                throw;
            }

            return new RelayServerData(allocation, CONNECTION_TYPE);
        }

        public async UniTask ConfigureTransportAdnStartAsHostAsync()
        {
            RelayServerData relayServerData = await AllocateRelayServerAndGetJoinCodeAsync(MAX_CONNECTIONS);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }

        async UniTask<RelayServerData> JoinRelayServerFromJoinCodeAsync(string joinCode)
        {
            JoinAllocation allocation;

            try
            {
                allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch 
            {
                Debug.Log("Relay create join code request failed");
                throw;
            }

            Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
            Debug.Log($"client: {allocation.AllocationId}");

            return new RelayServerData(allocation, CONNECTION_TYPE);
        } 
        
        public async UniTask ConfigureTransportAdnStartAsClientAsync()
        {
            RelayServerData relayServerData = await JoinRelayServerFromJoinCodeAsync(_joinCodeField.text);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
    }
}