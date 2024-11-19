using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [field: SerializeField] public KeyCode ResetKey { get; private set; } = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(ResetKey))
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
