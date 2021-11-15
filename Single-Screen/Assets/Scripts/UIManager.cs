using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private GameObject GameOverObject;

    #endregion

    // Funzione che mostra la schermata di GameOver
    public void ShowGameOver()
    {
        // Mettiamo il gioco in pausa
        Time.timeScale = 0f;

        GameOverObject.SetActive(true);
    }

    // Funzione che ricarica il livello attuale
    public void RestartGame()
    {
        // Facciamo ripartire il tempo
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

}
