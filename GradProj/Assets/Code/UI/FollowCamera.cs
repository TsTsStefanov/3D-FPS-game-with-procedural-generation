using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Player;

    //Transform playerCamera;
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Player.Find("PlayerCamera").transform.forward);
    }
}
