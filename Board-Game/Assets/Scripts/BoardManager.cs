using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;                            // Il riferimento statico (accessibile ovunque) dello script, cosi ogni quadrato puo' facilmente comunicare con questo script                   

    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private UIManager UIManagerScript;             // Il riferimento allo script che gestisce l'interfaccia

    [Header("Settings")]
    [SerializeField] private GameObject RowObject;                  // Referenza all'oggetto per ogni riga, lo copieremo quante volte ci serve
    [SerializeField] private GameObject BoxObject;                  // Referenza al quadrato da premere, lo passeremo allo script della riga per fare copie

    [SerializeField] private int MinNumberOfRows = 2;               // Numero minimo di righe/quadrati che possono essere generati
    [SerializeField] private int MaxNumberOfRows = 6;               // Numero massimo di righe/quadrati che possono essere generati

    #endregion

    #region === Variabili private ===

    private int _selectedNumberOfRows;                              // Il numero attuale di righe/quadrati selezionato
    private int _numberOfSquaresCreated;                            // Il numero di spazi creati
    private int _numberOfSquaresLeft;                               // Il numero di spazi rimasti
    private RowManager _currentRowManager;                          // Riferimento al manager della riga attuale

    #endregion

    // Funzione di Unity chiamata quando il gioco comincia (solo se lo script/oggetto e' attivo)
    void Start()
    {
        // Impostiamo il riferimento statico allo script
        Instance = this;

        // Selezioniamo a caso la dimensione della tavola di gioco
        _selectedNumberOfRows = Random.Range(MinNumberOfRows, MaxNumberOfRows);

        // Siccome la tavola e' un quadrato di NxN, facciamo gia' fare la moltiplicazione
        _numberOfSquaresCreated = _selectedNumberOfRows * _selectedNumberOfRows;
        _numberOfSquaresLeft = _numberOfSquaresCreated;

        // Aggiorniamo l'interfaccia che mostra il contatore di quadrati rimasti
        UIManagerScript.UpdateBoxCounterText(_numberOfSquaresLeft, _numberOfSquaresCreated);

        // Generiamo la tavola
        GenerateBoard();
    }

    // Funzione che genera la tavola di gioco
    void GenerateBoard()
    {
        // Creiamo una tavola NxN, a seconda del valore scelto in precendenza
        for (int i = 0; i < _selectedNumberOfRows; i++) {
            _currentRowManager = Instantiate(RowObject, transform).GetComponent<RowManager>();

            // Comunichiamo con lo script della riga per farlo creare il numero corretto di quadrati
            _currentRowManager.GenerateRow(_selectedNumberOfRows);
        }
    }

    // Funzione chiamata da ogni quadrato individualmente per riferire che e' stato distrutto
    public void DecreaseNumberOfBoxes()
    {
        // Togliamo uno dal contatore e controlliamo se tutto e' stato distrutto
        _numberOfSquaresLeft--;

        // Aggiorniamo l'interfaccia che mostra il contatore di quadrati rimasti
        UIManagerScript.UpdateBoxCounterText(_numberOfSquaresLeft, _numberOfSquaresCreated);

        // Se tutti sono distrutti e' gameover
        if (_numberOfSquaresLeft <= 0)
        {
            UIManagerScript.ShowGameOver();
        }
    }

}
