using Unity.Cinemachine;
using UnityEngine;

public class Teleporer : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private CinemachineCamera MainMap;
    [SerializeField] private CinemachineCamera teleMap;

    public Transform GetDestination()
    {
        MainMap.gameObject.SetActive(false);
        teleMap.gameObject.SetActive(true);

        return destination;
        Destroy(gameObject);
    }


}
