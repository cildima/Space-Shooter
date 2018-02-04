using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    private MeshCollider mc;
    private MeshRenderer mr;
    public Boundary boundary;
    public float speed;
    public float tilt;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        mc = GetComponent<MeshCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        myTime = myTime + Time.deltaTime;

        if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            nextFire = myTime + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();

            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    public void DisableRenderer()
    {
        mr.enabled = false;
    }

    public void EnableRenderer()
    {
        mr.enabled = true;
    }

    public void DisableTrigger()
    {
        mc.enabled = false;
    }

    public void EnableTrigger()
    {
        mc.enabled = true;
    }

    public IEnumerator Respawn()
    {
        DisableTrigger();
        for (int i = 0; i < 3; i++)
        {
            DisableRenderer();
            yield return new WaitForSeconds(0.5f);
            EnableRenderer();
            yield return new WaitForSeconds(0.5f);
        }
        EnableTrigger();
    }
}
