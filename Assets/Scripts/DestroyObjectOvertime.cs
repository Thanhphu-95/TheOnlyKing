using UnityEngine;

public class DestroyObjectOvertime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;

    // Update is called once per frame
    private void Star()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
