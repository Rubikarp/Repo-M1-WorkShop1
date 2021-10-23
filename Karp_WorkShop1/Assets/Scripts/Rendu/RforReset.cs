using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class RforReset : MonoBehaviour
{
    public InputHandler input;
    [Scene]
    public int scene;

    void Update()
    {
        if (input.Reset)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
