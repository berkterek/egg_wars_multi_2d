using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EggWars2D.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] int _targetFrame = 30;

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

        async void Start()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1));
            await SceneManager.LoadSceneAsync(1);
        }
    }    
}

