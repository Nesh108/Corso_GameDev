using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Transform RightMovementTransform;

    #endregion

    #region === Variabili private ===

    #endregion

    void LateUpdate()
    {
        if (PlayerTransform.position.x > RightMovementTransform.position.x)
        {
            float dif = RightMovementTransform.position.x - PlayerTransform.position.x;
            transform.position = new Vector3(transform.position.x - dif, transform.position.y, transform.position.z);
        }

    }
}


