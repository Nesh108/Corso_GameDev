using UnityEngine;

public class Player : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [Header("Settings")]
    [SerializeField] private float MoveSpeed = 5000f;                       // Valore che contiene la velocita' di movimento del giocatore
    [SerializeField] private float JumpSpeed = 1500f;                       // Valore che contiene la velocita' di salto del giocatore
    [SerializeField] private Vector2 MaxVelocity = new Vector2(10f, 25f);   // Velocita' massima consentita al personaggio 
    [SerializeField] private int Lives = 3;                                 // Quante vite ha il personaggio?

    #endregion

    #region === Variabili private ===

    private Vector2 _initialPlayerScale;                // Salviamo il valore delle dimensioni del giocatore per poter cambiare direzione
    private Rigidbody2D _playerRigidbody2D;             // Il 'Corpo Rigido' del personaggio, ci servira' per applicare forze fisiche e poterlo muovere
    private float _horizontalAxis;                      // Variabile che contiene il valore dell'input del giocatore (ex. tastiera o controller)
    private float _horizontalMove, _verticalMove;       // Variabili che contengono i valori di movimento del personaggio 
    private int _currentLives;                          // Variabile dove salviamo il numero attuale di vite
    #endregion

    // Funzione chiamata quando il gioco comincia (solo se lo script/oggetto e' attivo)
    void Start()
    {
        _initialPlayerScale = transform.localScale;

        // Cerchiamo il componente RigidBody2D dell'oggetto stesso
        _playerRigidbody2D = GetComponent<Rigidbody2D>();

        // Se non lo troviamo, scriviamo un messaggio d'errore, visto che il componente e' essenziale
        if(_playerRigidbody2D == null)
        {
            Debug.LogError($"RigidBody2D non trovato in: {name}");
        }

        _currentLives = Lives;
        UIManager.Instance.SetLives(_currentLives);
    }

    // Funzione di Unity chiamata ad ogni 'tick' del motore grafico
    // Qui facciamo tutte le cose che devono essere eseguite ad ogni frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            // Leggiamo i valori attuali della tastiera/controller del giocatore
            // Per ricevere i valori corretti, le funzioni 'Input' devono essere chiamate in 'Update'
            _horizontalAxis = Input.GetAxisRaw("Horizontal");

            // Se il giocatore preme "Spazio", lo faremo saltare al prossimo FixedUpdate
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalMove = JumpSpeed;
            }

            // Facciamo il giocatore guardare dove si sta muovendo
            if (Mathf.Abs(_horizontalAxis) > 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(_horizontalAxis) * _initialPlayerScale.x, transform.localScale.y);
            }

            // Limitiamo la velocita' del personaggio per evitare che vada come un missile (le forze si accumulano)
            _playerRigidbody2D.velocity = new Vector2(x: Mathf.Clamp(_playerRigidbody2D.velocity.x, -MaxVelocity.x, MaxVelocity.x),
                                                      y: Mathf.Clamp(_playerRigidbody2D.velocity.y, -MaxVelocity.y, MaxVelocity.y));
        }
    }

    // Funzione di Unity chiamata ad ogni 'tick' del motore fisico (la frequenza e' impostata in 'Physics Settings')
    // Qui facciamo tutte le cose relative alla fisica (movimenti, forze, gravita, etc.)
    void FixedUpdate()
    {
        // Calcoliamo quanta forza e' necessaria per muovere il personaggio
        // 'Time.fixedDeltaTime' e' necessario nella formula per poter avere un movimento fluido (anche se il computer non e' potente)
        _horizontalMove = _horizontalAxis * MoveSpeed * Time.fixedDeltaTime;

        // Controlliamo che ci sia qualche movimento da fare, altrimenti evitiamo di applicare forza al personaggio
        if (Mathf.Abs(_horizontalMove) > 0 || _verticalMove > 0)
        {
            _playerRigidbody2D.AddForce(new Vector2(_horizontalMove, _verticalMove));
            _verticalMove = 0f;
        }
    }

    // Funzione di Unity che viene chimata quando il personaggio viene a contatto con un "trigger"
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Controlliamo cosa abbiamo toccato
        if(collision.CompareTag("Coin"))
        {
            // Se e' una moneta, la aggiungiamo alla collezione e la distruggiamo
            AddCoin();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("Castle"))
        {
            // Se e' il castello, il gioco finisce
            UIManager.Instance.ShowGameOver();
        }
    }

    // Funzione che gestice l'aggiunta della moneta raccolta alla collezione
    void AddCoin()
    {
        Debug.Log("Got coin");
        UIManager.Instance.AddCoin();
    }


    // Funzione che gestisce cosa succede quando il giocatore e' colpito da un nemico
    public void HitByEnemy()
    {
        Debug.Log("Ouch! Colpito da un nemico!");

        // Siamo stati colpiti da un nemico, togliamo una vita, aggiorniamo l'intefaccia
        _currentLives--;
        UIManager.Instance.SetLives(_currentLives);

        // E controlliamo se e' gameover
        if (_currentLives <= 0)
        {
            UIManager.Instance.ShowGameOver();
        }
    }
}
