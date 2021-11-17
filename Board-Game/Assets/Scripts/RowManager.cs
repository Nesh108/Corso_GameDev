using UnityEngine;

public class RowManager : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private GameObject BoxObject;                              // L'oggetto che useremo per i quadrati (un prefab)

    #endregion

    // Funzione che genera il contenuto di una riga (i quadrati da cliccare)
    public void GenerateRow(int number)
    {
        // Creiamo il numero indicato di quadrati
        for (int i = 0; i < number; i++)
        {
            Instantiate(BoxObject, transform);
        }
    }

}
