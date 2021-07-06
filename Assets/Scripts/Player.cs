using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    //config params
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float paddle = 1f;
    [SerializeField] int health = 200;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 1f;

    float xMax;
    float xMin;
    float yMax;
    float yMin;
    Coroutine firingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        SetTheScreenBounds();
        
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();


    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            firingCoroutine = StartCoroutine(FireContiniously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

    }

    IEnumerator FireContiniously()

    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }


    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal")*Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical")*Time.deltaTime * moveSpeed;
        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX,xMin,xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY,yMin,yMax);


        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetTheScreenBounds()
    {
        Camera camera = Camera.main;
        xMax = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x -paddle;
        xMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x +paddle;
        yMax = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddle;
        yMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y+paddle;


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

