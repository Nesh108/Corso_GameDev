using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private float MinSpeed = 1;                       // Velocita' minima del nemico
    [SerializeField] private float MaxSpeed = 5;                       // Velocita' massima del nemico

    #endregion

    private float _currentSpeed;

    // Funzione di Unity che viene eseguita quando il gameobject si "sveglia"
    void Awake()
    {
        // Scegliamo a caso la velocita' che avra'
        _currentSpeed = Random.Range(MinSpeed, MaxSpeed);
    }

    // Funzione di Unity che viene eseguita piu' velocemente possibile
    void Update()
    {
        // Spostiamo il nemico verso sinistra (-X) alla velocita' indicata
        transform.position = new Vector2(transform.position.x - (_currentSpeed * Time.deltaTime), transform.position.y);

        // Se e' andato troppo indietro, lo distruggiamo
        if (transform.position.x < -100f)
        {
            Destroy(gameObject);
        }
    }

    // Funzione chiamata da 'EnemyCollision' ogni volta che il giocatore colpisce il nemico in testa
    public void HitByPlayer()
    {
        // Bye bye nemico
        Destroy(gameObject);
    }
}
