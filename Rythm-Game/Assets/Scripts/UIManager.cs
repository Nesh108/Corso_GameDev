using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private GameObject GameOverObject;            // Riferimento all'oggetto che mostra la schermata di gameover

    #endregion

    void Awake()
    {
        // Facciamo ripartire il tempo (utile se ricarichiamo il livello di nuovo)
        Time.timeScale = 1f;
    }

    // Funzione per ritornare al menu principale
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    // Funzione che ricarica il livello attuale
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    // Funzione che mostra la schermata di GameOver
    public void ShowGameOver()
    {
        // Mettiamo il gioco in pausa
        Time.timeScale = 0f;

        GameOverObject.SetActive(true);
    }

}
