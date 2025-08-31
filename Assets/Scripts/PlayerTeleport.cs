using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currenTeleport;
    [SerializeField] GameObject tele;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currenTeleport != null)
            {
                transform.position = currenTeleport.GetComponent<Teleporer>().GetDestination().position;
                Destroy(tele);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleport"))
        {
            currenTeleport = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleport"))
        {
            if (collision.gameObject == currenTeleport)
            {
                currenTeleport = null;
            }
        }
    }

}
