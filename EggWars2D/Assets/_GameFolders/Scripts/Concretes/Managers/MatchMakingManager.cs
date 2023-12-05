using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class MatchMakingManager : MonoBehaviour
    {
        [SerializeField] string _joinCode;

        Lobby _lobby;

        public static MatchMakingManager Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public async void PlayButtonCallback()
        {
            await AuthenticatePlayerAsync();

            _lobby = await QuickJoinLobbyAsync() ?? await CreateLobbyAsync();
        }

        async UniTask<Lobby> CreateLobbyAsync()
        {
            try
            {
                int maxPlayerCount = 2;
                string lobbyName = "MyCoolLobby";

                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayerCount);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                        { { _joinCode, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }
                };

                var lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayerCount, createLobbyOptions);

                //Ping lobby
                HeartbeatLobbyAsync(lobby.Id, 15);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData
                (
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData
                );

                NetworkManager.Singleton.StartHost();
                return lobby;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log(e.Message);
                return null;
            }
        }

        async UniTask<Lobby> QuickJoinLobbyAsync()
        {
            try
            {
                Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

                JoinAllocation allocation =
                    await RelayService.Instance.JoinAllocationAsync(lobby.Data[_joinCode].Value);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData
                (
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData,
                    allocation.HostConnectionData
                );

                NetworkManager.Singleton.StartClient();
                return lobby;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                Debug.Log(e.Message);
                return null;
            }
        }

        async UniTask AuthenticatePlayerAsync()
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

        async void HeartbeatLobbyAsync(string lobbyId, float waitTimeSeconds)
        {
            while (true)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                await UniTask.Delay(System.TimeSpan.FromSeconds(waitTimeSeconds));
            }
        }
    }
}