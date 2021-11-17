using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private UIManager UIManagerScript;       // Il riferimento allo script che gestisce l'interfaccia
    [SerializeField] private TMPro.TMP_Text TimerText;        // Componente che utilizzeremo per mostrare il numero di monete collezionate
    [SerializeField] private int GameTimeInSeconds = 60;      // Quanto tempo dura il livello
    [SerializeField] private Gradient TextColorGradient;      // Utilizziamo un gradiente per cambiare il colore del testo, per avvertire il giocatore

    #endregion

    private float _currentTime;                               // Variabile che tiene quanto tempo e' rimasto al giocatore

    void Start()
    {
        _currentTime = GameTimeInSeconds;
        UpdateTimeText();
    }

    void Update()
    {
        // Controlliamo che il gioco non sia in pausa
        if(Time.timeScale > 0)
        {
            _currentTime -= Time.deltaTime;
            UpdateTimeText();

            // Tempo e' finito => Gameover
            if (_currentTime <= 0)
            {
                UIManagerScript.ShowGameOver();
            }
        }
    }

    // Funzione che aggiorna il testo che mostra il timer
    void UpdateTimeText()
    {
        // Convertiamo da secondi a mm:ss (ex. 00:56)
        TimerText.text = $"{TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss")}";

        // Usiamo il gradiente e cambiamo il colore del testo
        TimerText.color = TextColorGradient.Evaluate(1 - _currentTime/GameTimeInSeconds);
    }
}
