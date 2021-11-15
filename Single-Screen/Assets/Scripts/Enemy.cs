using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private float DieAfter = 0.1f;       // Quanti secondi prima che il nemico scompaia

    #endregion


    #region === Variabili private ===

    private bool _isDead = false;                       // Variable per salvare se il nemico e' morto

    #endregion

    // Funzione utilizzata per uccidere il nemico
    public void Die()
    {
        _isDead = true;
        Destroy(gameObject, DieAfter);
    }

    private void Update()
    {
        if(!_isDead)
        {

        }
    }
}
