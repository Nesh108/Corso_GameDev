using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private float DieAfter = 0.1f;                 // Quanti secondi prima che il nemico scompaia

    [Header("Weapon")]
    [SerializeField] private GameObject BulletPrefab;               // Variabile per poter sparare
    [SerializeField] private float MinBulletSpeed = 20;                  // Variabile per la velocita' minima del proiettile
    [SerializeField] private float MaxBulletSpeed = 50;                  // Variabile per la velocita' massima del proiettile
    [SerializeField] private float BulletDuration;                  // Variabile per la durata del proiettile
    [SerializeField] private float MinShootingCooldown = 0.5f;      // Variabile per la ricarica minima
    [SerializeField] private float MaxShootingCooldown = 5f;        // Variabile per la ricarica massima

    #endregion


    #region === Variabili private ===

    private bool _isDead = false;                       // Variable per salvare se il nemico e' morto
    private BulletMover _bullet;                        // Variable dove metteremo il nuovo proiettile
    private float _currentShootingCooldown;             // Variable per gestire la ricarica
    private float _enemyDirection = 1;                  // Variabile per gestire la direzione del nemico e del proiettile

    #endregion

    // Funzione di Unity chiamata quando il gioco comincia
    void Start()
    {
        _currentShootingCooldown = Random.Range(MinShootingCooldown, MaxShootingCooldown);     // Impostiamo il cooldown a caso
    }

    // Funzione utilizzata per uccidere il nemico
    public void Die()
    {
        _isDead = true;
        Destroy(gameObject, DieAfter);
    }

    private void Update()
    {
        // Controlliamo se il nemico non e' gia' morto e se il gioco non e' in pausa
        if (!_isDead && Time.timeScale > 0)
        {
            // Salviamo la direzione del nemico per poter sparare nella direzione giusta
            _enemyDirection = Mathf.Sign(transform.localScale.x);

            // Il nemico spara sempre, se possibile
            HandleShooting();
        }
    }

    // Funzione che tie
    void HandleShooting()
    {
        // Se il nermico ha gia' sparato, diminuiamo il tempo di ricarica (Time.deltaTime e' il tempo che passato dall'ultimo Update)
        if (_currentShootingCooldown > 0f)
        {
            _currentShootingCooldown -= Time.deltaTime;
        }

        // Se il cooldown e' finito, spara
        if (_currentShootingCooldown <= 0.001f)
        {
            ShootBullet();
        }
    }

    // Funzione che crea un nuovo proiettile e lo imposta
    void ShootBullet()
    {
        _bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletMover>();
        _bullet.gameObject.layer = LayerMask.NameToLayer("Enemy");                             // Impostiamo il Layer cosicche' possa colpire i nemici correttamente
        _currentShootingCooldown = Random.Range(MinShootingCooldown, MaxShootingCooldown);     // Resettiamo il cooldown a caso
        _bullet.Shoot(Random.Range(MinBulletSpeed, MaxBulletSpeed), _enemyDirection, BulletDuration);
    }
}
