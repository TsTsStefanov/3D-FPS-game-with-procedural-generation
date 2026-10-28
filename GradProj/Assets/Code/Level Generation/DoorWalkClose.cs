using UnityEngine;

public class DoorWalkClose : MonoBehaviour
{
    public SlidingDoors slidingDoors;
    public GameObject Trigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slidingDoors.CloseDoors();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Destroy trigger");
        Destroy(Trigger);
    }
}
