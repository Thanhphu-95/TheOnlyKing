using UnityEngine;

public class Teleporer : MonoBehaviour
{
    [SerializeField] private Transform destination;

    public Transform GetDestination()
    {
        return destination;
        Destroy(gameObject);
    }
}
