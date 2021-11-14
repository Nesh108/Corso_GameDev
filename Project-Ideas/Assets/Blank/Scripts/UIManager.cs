using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Blank/Scenes/Menu", LoadSceneMode.Single);
    }

}
