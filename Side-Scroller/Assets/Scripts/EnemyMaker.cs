using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private GameObject EnemyPrefab;                // Il Prefab che contiene la base per i nemici che verranno creati
    [SerializeField] private float EnemyCreationInterval = 5;       // Ogni quanto viene un creato un nuovo nemico?

    #endregion

    private float _timer;

    void Start()
    {
        // Se non troviamo la base per fare i nemici, scriviamo un messaggio d'errore, visto che l'oggetto e' essenziale
        if (EnemyPrefab == null)
        {
            Debug.LogWarning($"EnemyPrefab non trovato in: {name}");
        }

        // Cosi' nonappena il gioco comincia, un nuovo nemico verra' creato
        _timer = EnemyCreationInterval;
    }

    void Update()
    {
        // Aggiorniamo il time con il tempo appena trascorso
        _timer += Time.deltaTime;

        // Se il timer raggiunge l'intervallo prestabilito per creare un nuovo nemico, lo creiamo
        if(_timer >= EnemyCreationInterval)
        {
            CreateEnemy();
            // E resettiamo il timer per ricominciare
            _timer = 0;
        }
    }

    // Funzione che crea una copia di nemico nello stesso posto dove si trova questo oggetto
    void CreateEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity, transform.parent);
    }
}
