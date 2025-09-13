using Unity.Cinemachine;
using UnityEngine;

public class Teleporer : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private CinemachineCamera followCam;
    [SerializeField] private CinemachineCamera destinationCam;

    public Transform GetDestination()
    {
        followCam.gameObject.SetActive(false);
        destinationCam.gameObject.SetActive(true);
        Destroy(gameObject);
        return destination;
        
    }


}
