using System;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float range=3f;
    public float damage = 10f;
    public float fireRate = 0.5f;
    public float rotationSpeed = 120f;
    public float snapSpeed = 10f;
    public Transform Player;
    public GameObject body;
    public ParticleSystem muzzleFlash;

    private float shootCountDown = 0f;
    bool hasFoundPlayer = false;
    System.Random rnd = new System.Random();
    void Start()
    {
        body.transform.rotation = Quaternion.Euler(0f, rnd.Next(0,360), 0f);
    }

    void Update()
    {
        if (!hasFoundPlayer)
        {
            Search();
        }
        else
        {
            Hunt();
        }
    }

    private void Search()
    {
        body.transform.Rotate(0f, Time.deltaTime * rotationSpeed, 0f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.red);
            if (hit.transform.name.StartsWith("Player"))
            {
                hasFoundPlayer = true;
            }
        }
    }
    private void Hunt()
    {
        float distance = Vector3.Distance(body.transform.position, Player.transform.position);
        if (distance > range)
        {
            hasFoundPlayer = false;
            return;
        }
        Vector3 dir = Player.transform.position - body.transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, lookRotation, Time.deltaTime * snapSpeed);
        }
        if(shootCountDown <= 0)
        {
            Shoot();
            shootCountDown = 1f / fireRate;
        }
        shootCountDown -= Time.deltaTime;
    }
    private void Shoot()
    {
        RaycastHit hit;
        muzzleFlash.Play();
        if (!PauseMenu.isPaused)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }
}
