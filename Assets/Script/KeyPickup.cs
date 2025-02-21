using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            other.GetComponent<Player>().PickUpKey();
            Destroy(gameObject);
        }
    }
}
