using UnityEngine;

public class BulletMover : MonoBehaviour
{


    #region === Variabili private ===
    public Player ShootingPlayerScript;        // Variabile per salvare la referenza al giocatore che ha sparato il proiettile (flessibile per avere piu' giocatori)

    #endregion

    #region === Variabili private ===

    private float _currentDuration = 0;
    private Rigidbody2D _bulletRigidBody2D;
    private float _duration = 10f;            // Variabile per gestire la durata del proiettile
    private bool _hasAlreadyHit = false;      

    #endregion

    void Awake()
    {
        // Cerchiamo il componente RigidBody2D dell'oggetto stesso
        _bulletRigidBody2D = GetComponent<Rigidbody2D>();

        // Se non lo troviamo, scriviamo un messaggio d'errore, visto che il componente e' essenziale
        if (_bulletRigidBody2D == null)
        {
            Debug.LogError($"RigidBody2D not found on: {name}");
        }
    }

    // Funzione utilizzata dal Player per sparare il proiettile
    public void Shoot(float speed, float playerDirection, float duration)
    {
        // Spariamo il proiettile verso la direzione dove il personaggio sta guardando alla velocita' stabilita
        _bulletRigidBody2D.AddForce(new Vector2(playerDirection * speed, 0), ForceMode2D.Impulse);
        _duration = duration;
    }

    void Update()
    {
        _currentDuration += Time.deltaTime;

        // Se il proiettile e' durato piu' del valore definito, lo distruggiamo
        if(_currentDuration >= _duration)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Il proiettile non ha colpito niente ed e' troppo distante dal giocatore, lo distruggiamo
        if (transform.position.x > 50f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Controlliamo se il proiettile ha gia' colpito qualcosa
        if (!_hasAlreadyHit)
        {
            // Se il proiettile colpisce un nemico, lo uccidiamo
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<Enemy>().Die();
                ShootingPlayerScript.AddKill();
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<Player>().Die();
            }
            else
            {
                // Ha colpito qualcos'altro, lo disabilitiamo
                _hasAlreadyHit = true;
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
