using UnityEngine;

public class Player : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [Header("UI")]
    [SerializeField] private UIManager UIManagerScript;       // Referenza al script che gestice l'interfaccia

    [Header("Player")]
    [SerializeField] private float MoveSpeed = 1000f;         // Valore che contiene la velocita' di movimento del giocatore
    [SerializeField] private float JumpSpeed = 2000f;         // Valore che contiene la velocita' di salto del giocatore

    [Header("Weapon")]
    [SerializeField] private GameObject BulletPrefab;           // Variabile per poter sparare
    [SerializeField] private float BulletSpeed;                 // Variabile per la velocita' del proiettile
    [SerializeField] private float BulletDuration;              // Variabile per la durata del proiettile
    [SerializeField] private float ShootingCooldown;            // Variabile per la ricarica (altrimenti la pistola diventa una mitraglia!)

    #endregion

    #region === Variabili private ===

    private Vector2 _initialPlayerScale;                // Salviamo il valore delle dimensioni del giocatore per poter cambiare direzione
    private Rigidbody2D _playerRigidbody2D;             // Il 'Corpo Rigido' del personaggio, ci servira' per applicare forze fisiche e poterlo muovere
    private Vector2 _minScreenBounds, _maxScreenBounds; // Variabili che contengono i limiti dello schermo
    private float _horizontalAxis;                      // Variabile che contiene il valore dell'input del giocatore (ex. tastiera o controller)
    private float _horizontalMove, _verticalMove;       // Variabili che contengono i valori di movimento del personaggio
    private BulletMover _bullet;                        // Variable dove metteremo il nuovo proiettile
    private float _currentShootingCooldown;             // Variable per gestire la ricarica
    private float _playerDirection = 1;                 // Variabile per gestire la direzione del giocatore e del proiettile
    private int _numberOfEnemiesKilled = 0;             // Variabile che contiene il numero di nemici uccisi da questo giocatore
    private int _totalNumberOfEnemies;                  // Variabile che contiene il totale di nemici quando il gioco comincia

    #endregion

    // Funzione chiamata quando il gioco comincia (solo se lo script/oggetto e' attivo)
    void Start()
    {
        // Possiamo cercare tutti i nemici nella scena attuale e contare quanti ne dobbiamo uccidere per vincere
        _totalNumberOfEnemies = FindObjectsOfType<Enemy>().Length;

        _initialPlayerScale = transform.localScale;

        // Cerchiamo il componente RigidBody2D dell'oggetto stesso
        _playerRigidbody2D = GetComponent<Rigidbody2D>();

        // Se non lo troviamo, scriviamo un messaggio d'errore, visto che il componente e' essenziale
        if(_playerRigidbody2D == null)
        {
            Debug.LogError($"RigidBody2D not found on: {name}");
        }

        CalculateScreenBounds();
    }

    // Funzione che calcola dinamicamente i limiti di dove il giocatore puo' spostarsi (usato per non far uscire il giocatore dallo schermo)
    void CalculateScreenBounds()
    {
        _minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        _maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    // Funzione chiamata ad ogni 'tick' del motore grafico
    // Qui facciamo tutte le cose che devono essere eseguite ad ogni frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            // Salviamo la direzione del giocatore per poter sparare nella direzione giusta
            _playerDirection = Mathf.Sign(transform.localScale.x);

            // Leggiamo i valori attuali della tastiera/controller del giocatore
            // Per ricevere i valori corretti, le funzioni 'Input' devono essere chiamate in 'Update'
            _horizontalAxis = Input.GetAxisRaw("Horizontal");

            // Se il giocatore preme "Spazio", lo faremo saltare al prossimo FixedUpdate
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalMove = JumpSpeed;
            }

            // Controlliamo se dobbiamo sparare e aggiorniamo le variabili
            HandleShooting();

            // Ci assicuriamo che il giocatore non vada fuori dallo schermo e se lo fosse, lo spostiamo dentro
            transform.position = new Vector2(x: Mathf.Clamp(transform.position.x, _minScreenBounds.x, _maxScreenBounds.x),
                                            y: transform.position.y);

            // Facciamo il giocatore guardare dove si sta muovendo
            if (Mathf.Abs(_horizontalAxis) > 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(_horizontalAxis) * _initialPlayerScale.x, transform.localScale.y);
            }
        }
    }

    // Funzione chiamata ad ogni 'tick' del motore fisico (la frequenza e' impostata in 'Physics Settings')
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

    // Funzione che tie
    void HandleShooting()
    {
        // Se il giocatore ha gia' sparato, diminuiamo il tempo di ricarica (Time.deltaTime e' il tempo che passato dall'ultimo Update)
        if (_currentShootingCooldown > 0f)
        {
            _currentShootingCooldown -= Time.deltaTime;
        }

        // Se il cooldown e' finito e il giocatore ha premuto 'Fire1' (definito in 'Project Settings > Input Manager'), spara
        if (_currentShootingCooldown <= 0.001f && Input.GetButtonUp("Fire1"))
        {
            ShootBullet();
        }
    }

    // Funzione che crea un nuovo proiettile e lo imposta
    void ShootBullet()
    {
        _bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletMover>();
        _bullet.ShootingPlayerScript = this;
        _bullet.gameObject.layer = LayerMask.NameToLayer("Player"); // Impostiamo il Layer cosicche' possa colpire i nemici correttamente
        _currentShootingCooldown = ShootingCooldown;                // Resettiamo il cooldown
        _bullet.Shoot(BulletSpeed, _playerDirection, BulletDuration);
    }

    // Funzione utilizzata per uccidere il giocatore e mostrare la schermata di Game Over
    public void Die()
    {
        Destroy(gameObject);
        UIManagerScript.ShowGameOver();
    }

    public void AddKill()
    {
        _numberOfEnemiesKilled++;

        // Se ne abbiamo uccisi abbastanza, il gioco finisce
        if(_numberOfEnemiesKilled >= _totalNumberOfEnemies)
        {
            UIManagerScript.ShowGameOver();
        }
    }
}
