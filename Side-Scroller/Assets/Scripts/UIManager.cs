using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;                // Rendiamo la classe disponibile in maniera statica (quindi non e' necessario avere la referenza diretta)

    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private TMPro.TMP_Text CoinCounterText;        // Componente che utilizzeremo per mostrare il numero di monete collezionate
    [SerializeField] private GameObject GameOverObject;
    [SerializeField] private GameObject[] LivesUIObjects;           // Array che contengono i gameobject per visualizzare le vite restanti nell'interfaccia

    #endregion

    #region === Variabili private ===

    private int _totalCoins = 0;                    // Variabile che contiene il numero totale di monete nel livello
    private int _collectedCoins = 0;                // Variabile che contiene il numero totale di monete collezionate
    private int _currentPlayerLives = 0;            // Variabile che useremo per mostrare il numero di vite del giocatore

    #endregion

    // Funzione di Unity eseguita nonappena il gioco comincia (prima di Start)
    void Awake()
    {
        Instance = this;
        _collectedCoins = 0;
        _totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;

        // Facciamo ripartire il tempo (utile se ricarichiamo il livello di nuovo)
        Time.timeScale = 1f;
    }

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    // Funzione per ritornare al menu principale
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void Update()
    {
        CoinCounterText.text = $"<b>Coins</b>: {_collectedCoins}/{_totalCoins}";
        ShowCurrentLives();
    }

    // Funzione che aggiunge una moneta alla collezione. Chiamata da 'Player'
    public void AddCoin()
    {
        _collectedCoins++;

        // Controlliamo se abbiamo collezionato tutte le monete disponibili
        if(_collectedCoins >= _totalCoins)
        {
            Debug.Log($"Yeeah!!!! Tutte le monete: {_collectedCoins}/{_totalCoins}");
        }
    }

    // Il giocatore ci aggiornera' di quante vita ha, per poi mostrarle nella interfaccia
    public void SetLives(int lives)
    {
        _currentPlayerLives = lives;
    }

    // Funzione che aggiorna l'interfaccia delle vite rimaste al giocatore
    void ShowCurrentLives()
    {
        for(int i = 0; i < LivesUIObjects.Length; i++)
        {
            if(i <= _currentPlayerLives - 1)
            {
                LivesUIObjects[i].SetActive(true);
            } else
            {
                LivesUIObjects[i].SetActive(false);
            }
        }
    }
}
