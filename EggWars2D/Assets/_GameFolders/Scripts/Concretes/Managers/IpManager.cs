using System.Linq;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace EggWars2D.Managers
{

    public class IpManager : MonoBehaviour
    {
        [SerializeField] TMP_InputField _ipInputField;
        [SerializeField] TMP_Text _ipText;

        public static IpManager Instance;

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
            _ipText.SetText(GetLocalPv4());

            UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            utp.SetConnectionData(GetLocalPv4(), 7777);
        }

        string GetLocalPv4()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        }

        public string GetInputIp()
        {
            return _ipInputField.text;
        }
    }
}
