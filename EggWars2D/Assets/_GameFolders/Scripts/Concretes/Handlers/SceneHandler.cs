using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EggWars2D.Handlers
{
    public class SceneHandler : MonoBehaviour
    {
        async void Start()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(2));
            await SceneManager.LoadSceneAsync(1);
        }
    }    
}

