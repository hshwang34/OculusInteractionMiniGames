using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform barrelLocation;  // This is where the bullet will be instantiated. It should be an empty GameObject at the desired location on your gun model.
    public Transform casingExit;
    public float shootForce = 30f;
    public float casingExitForce = 10f;
    public int maxCasings = 10;
    private Queue<GameObject> casingsQueue = new Queue<GameObject>();
    private float bulletLifetime = 3f;
    public AudioSource audioSource;
    public AudioSource emptyGunAudioSource;

    public int maxBullets = 7;
    private int currentBullets;
    public float reloadDuration = 3f;
    private bool isReloading = false;
    public AudioSource reloadAudioSource;
    private Coroutine reloadRoutine;

    public GameObject lineRendererPrefab;
    private LineRenderer laser;

    public GameObject reticlePrefab;
    private GameObject currentReticle;
    private bool laserActive = false;

    public float slowMotionFactor = 0.1f;  // 10% of the original speed
    public float slowMotionDuration = 2f;  // The duration for how long the slow-motion effect will last

    private void Start()
    {
        currentBullets = maxBullets;
        // Ensure barrelLocation has a value; if not, default to the current transform.
        if (!barrelLocation)
        {
            barrelLocation = transform;
            Debug.LogWarning("Barrel_Location not set, defaulting to current GameObject's transform.");
        }
        currentReticle = Instantiate(reticlePrefab);
        currentReticle.SetActive(false);

        GameObject laserObject = Instantiate(lineRendererPrefab, barrelLocation.position, barrelLocation.rotation);
        laser = laserObject.GetComponent<LineRenderer>();
        laser.enabled = true;
    }

    private void Update()
    {
        if (laser)
        {
            laser.SetPosition(0, barrelLocation.position);
        }
        // if (true)
        //{
        ShowAimingLaserAndReticle();
        //}
    }

    public void turnOnLaser()
    {
        laserActive = true;
        laser.enabled = true;
        currentReticle.SetActive(true);
    }

    public void turnOffLaser()
    {
        laserActive = false;
        laser.enabled = false;
        currentReticle.SetActive(false);
    }

    //this should only be called when the gun is selected
    void ShowAimingLaserAndReticle()
    {
        RaycastHit hit;
        if (Physics.Raycast(barrelLocation.position, barrelLocation.TransformDirection(Vector3.forward), out hit))
        {
            // If the ray hits something, set the end of the laser to that point and position the reticle there.
            laser.SetPosition(0, barrelLocation.position);
            laser.SetPosition(1, hit.point);

            currentReticle.SetActive(true);
            currentReticle.transform.position = hit.point;
            currentReticle.transform.forward = hit.normal;  // Orient the reticle to face outwards from the hit surface.
        }
        else
        {
            // If the ray doesn't hit anything, extend the laser to some arbitrary distance, and hide the reticle.
            laser.SetPosition(0, barrelLocation.position);
            laser.SetPosition(1, barrelLocation.position + barrelLocation.forward * 100f); // 100 is an arbitrary number; adjust as needed.

            currentReticle.SetActive(false);
        }
    }

    public void Shoot()
    {
        if (currentBullets > 0)
        {
            // Instantiate the projectile at the barrelLocation's position and rotation.
            GameObject bulletInstance = Instantiate(projectilePrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(bulletInstance, bulletLifetime);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

            Transform bulletChild = bulletInstance.transform.Find("Bullet");
            Transform casingChild = bulletInstance.transform.Find("BulletCasing");

            audioSource.Play();

            if (bulletChild)
            {
                Rigidbody bulletRb = bulletChild.GetComponent<Rigidbody>();
                if (bulletRb)
                {
                    bulletRb.velocity = barrelLocation.TransformDirection(Vector3.forward) * shootForce; // Adjust force as needed

                }
            }

            if (casingChild)
            {
                Rigidbody casingRb = casingChild.GetComponent<Rigidbody>();
                if (casingRb)
                {
                    Vector3 randomForceDirection = new Vector3(
                        Random.Range(0.5f, 1.0f),  // x-axis
                        Random.Range(0.5f, 1.0f),  // y-axis
                        0                          // z-axis (keeping it 0 to avoid forward/backward ejection)
                    );


                    casingRb.velocity = casingExit.TransformDirection(randomForceDirection.normalized * casingExitForce);
                    casingsQueue.Enqueue(casingChild.gameObject);
                    if (casingsQueue.Count > maxCasings)
                    {
                        Destroy(casingsQueue.Dequeue()); // Destroy the oldest casing
                    }

                }
                // Move the casing to the casing exit location
                casingChild.position = casingExit.position;
            }
            currentBullets--;
        } else
        {
            emptyGunAudioSource.Play();
        }

        //this will slow down the entire game, not what i am looking for.
        //StartCoroutine(SlowMotionEffect());

    }

    //this slowmotion effect 
    IEnumerator SlowMotionEffect()
    {
        Time.timeScale = slowMotionFactor;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }

    public void reloadGun()
    {
        if (reloadRoutine == null)
        {
            reloadRoutine = StartCoroutine(reload());
        }
    }

    IEnumerator reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        currentBullets = maxBullets;
        isReloading = false;
        //play sound to communicate to the user that reload has been complete
        reloadAudioSource.Play();
        reloadRoutine = null;

    }

    public void reloadUnSelected() {
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
            isReloading = false;
        }
    }

}
