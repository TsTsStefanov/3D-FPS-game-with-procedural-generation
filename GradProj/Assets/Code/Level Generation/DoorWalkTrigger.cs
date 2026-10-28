//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class DoorWalkTrigger : MonoBehaviour
{
    public SlidingDoors slidingDoors;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slidingDoors.OpenDoors();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slidingDoors.CloseDoors();
        }
    }
}
