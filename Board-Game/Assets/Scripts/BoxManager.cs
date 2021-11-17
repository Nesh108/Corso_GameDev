using UnityEngine;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private Gradient ColorGradient;                       // Il gradiente che useremo per colorare il quadrato a seconda dei click rimasti
    [SerializeField] private int MinNumberOfClicks = 2;                    // Il numero minimo di click necessari per distruggere il quadrato
    [SerializeField] private int MaxNumberOfClicks = 5;                    // Il numero massimo di click necessari per distruggere il quadrato

    #endregion

    #region === Variabili private ===

    private int _selectedNumberOfClicks;                   // Il numero selezionato di click necessari per distruggere il quadrato
    private int _clicksLeft;                               // Il numero rimasto di click
    private Image _image;                                  // La referenza al componente Image del quadrato

    #endregion

    // Funzione di Unity chiamata quando il gioco comincia (solo se lo script/oggetto e' attivo)
    void Start()
    {
        // Selezioniamo un numero a caso di clicks 
        _selectedNumberOfClicks = Random.Range(MinNumberOfClicks, MaxNumberOfClicks);

        // Resettiamo il numero di clicks fatti dal giocatore
        _clicksLeft = _selectedNumberOfClicks;

        // Cerchiamo il componente UI.Image dell'oggetto stesso
        _image = GetComponent<Image>();

        // Se non lo troviamo, scriviamo un messaggio d'errore, visto che il componente e' essenziale
        if (_image == null)
        {
            Debug.LogError($"UI.Image non trovato in: {name}");
        }
    }

    // Funzione di Unity chiamata ogni frame
    void Update()
    {
        // Selezioniamo dal gradiente pre-stabilito il colore a seconda di quanti click sono rimasti
        _image.color = ColorGradient.Evaluate(Mathf.Clamp((float)_clicksLeft / MaxNumberOfClicks, 0f, 1f));
    }

    // Funzione chiamata quando c'e' un click e gestisce il conto
    public void DoClick()
    {
        // Togliamo uno dal contatore
        _clicksLeft--;
        Debug.Log($"Cliccato! Vita rimasta: {_clicksLeft}/{_selectedNumberOfClicks}");

        // E controlliamo se e' necessario rimuovere il quadrato (se non ha piu' vita rimasta)
        if (_clicksLeft <= 0)
        {
            RemoveBox();
        }
    }

    // Funzione che 'rimuove' il quadrato (lo nascondiamo semplicemente)
    void RemoveBox()
    {
        _image.enabled = false;
        BoardManager.Instance.DecreaseNumberOfBoxes();
    }
}
