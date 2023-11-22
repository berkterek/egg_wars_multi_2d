using EggWars2D.Controllers;
using EggWars2D.Enums;
using EggWars2D.Handlers;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace EggWars2D.Managers
{
    public class ScoreManager : NetworkBehaviour
    {
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] int _hostScore;
        [SerializeField] int _clientScore;

        void Start()
        {
            UpdateScoreText();
        }

        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.OnServerStarted += HandleOnServerStarted;
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.Singleton.OnServerStarted -= HandleOnServerStarted;
            EggController.OnFellWater -= HandleOnFellWater;
            GameManager.Instance.OnGameStateChanged -= HandleOnGameStateChanged;
        }
        
        void HandleOnServerStarted()
        {
            if (!IsServer) return;

            EggController.OnFellWater += HandleOnFellWater;
            GameManager.Instance.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void HandleOnFellWater()
        {
            if (PlayerSelectorHandler.Instance.IsHostTurn)
            {
                _clientScore++;
            }
            else
            {
                _hostScore++;
            }

            UpdateScoreClientRpc(_clientScore,_hostScore);
            CheckForEndGame();
        }

        [ClientRpc]
        void UpdateScoreClientRpc(int clientScore, int hostScore)
        {
            _clientScore = clientScore;
            _hostScore = hostScore;
            
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            _scoreText.SetText($"<color=blue>{_hostScore}</color> - <color=red>{_clientScore}</color>");
        }
        
        void HandleOnGameStateChanged(StateEnum gameState)
        {
            switch (gameState)
            {
                case StateEnum.Game:
                    ResetScores();
                    break;
            }
        }

        void ResetScores()
        {
            UpdateScoreClientRpc(0, 0);
        }
        
        void CheckForEndGame()
        {
            if (_hostScore >= 3)
            {
                //host win
            }
            else if (_clientScore >= 3)
            {
                //client win
            }
            else
            {
                //Respawn ball
                ReuseEgg();
            }
        }

        void ReuseEgg()
        {
            EggManager.Instance.ReuseEgg();
        }
    }
}