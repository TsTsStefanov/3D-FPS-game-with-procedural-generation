using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
    public GameObject Door01_L;
    public GameObject Door01_R;
    public float doorSpeed = 5f;
    public float slideAmount = 0.5f;

    private Vector3 leftLocalPos;
    private Vector3 rightLocalPos;
    private Vector3 targetLeft;
    private Vector3 targetRight;
    private bool isOpen = false;
    void Start()
    {
        leftLocalPos = Door01_L.transform.localPosition;
        rightLocalPos = Door01_R.transform.localPosition;

        targetLeft = leftLocalPos;
        targetRight = rightLocalPos;
    }

    void Update()
    {
        Door01_L.transform.localPosition = Vector3.MoveTowards(Door01_L.transform.localPosition, targetLeft, doorSpeed * Time.deltaTime);
        Door01_R.transform.localPosition = Vector3.MoveTowards(Door01_R.transform.localPosition, targetRight, doorSpeed * Time.deltaTime);
    }
    public void OpenDoors()
    {
        isOpen = true;
        targetLeft = new Vector3(leftLocalPos.x - slideAmount, leftLocalPos.y, leftLocalPos.z);
        targetRight = new Vector3(rightLocalPos.x + slideAmount, rightLocalPos.y, rightLocalPos.z);
    }
            

    public void CloseDoors()
    {
        isOpen = false;
        targetLeft = leftLocalPos;
        targetRight = rightLocalPos;
    }

    public void ToggleDoors()
    {
        if (isOpen) CloseDoors();
        else OpenDoors();
    }
}
