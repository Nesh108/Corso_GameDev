using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    #region === Variabili private ===

    private Enemy _enemy;

    #endregion

    void Start()
    {
        // Cerchiamo lo script del nemico nell'oggetto superiore (il 'parent')
        _enemy = GetComponentInParent<Enemy>();
    }

    // Funzione di Unity che viene chimata quando il nemico viene a contatto con un "trigger"
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Abbiamo colpito il personaggio con la parte frontale, verra' colpito!
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().HitByEnemy();
        }
    }

    // Funzione di Unity che viene chimata quando il nemico viene colpito da una "collision"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Il nemico e' stato colpito in testa dal giocatore, lo facciamo morire
            _enemy.HitByPlayer();
        }
    }
}
