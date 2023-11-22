using UnityEditor;
using UnityEditor.SceneManagement;

namespace EggWars2D.Editor
{
    [InitializeOnLoad]
    public static class StartupSceneLoader
    {
        static StartupSceneLoader()
        {
            EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
        }

        static void HandleOnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (EditorSceneManager.GetActiveScene().buildIndex != 0)
                {
                    EditorSceneManager.LoadScene(0);
                }
            }
        }
    }    
}

