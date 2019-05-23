using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public GameObject lifeForce;
    public GameObject heatSeeker;
    public GameObject circler;
    public GameObject maze;
    public GameObject bulletRectangle;
    public GameObject starPatrol;
    public GameObject spinnerBulletSet;

    private Vector3 destination = Vector3.up*1000;

    private const float velocityMagnitude = 3;

    private float hoseBulletRotation = -90;
    private float hoseBulletRotation2 = 0;
    private float hoseBulletRotation3 = -45;

    private float hoseBulletIncrement = 5;
    private float hoseBulletIncrement2 = 5;
    private float hoseBulletIncrement3 = 5;

    private bool circleAroundPlayer=false;
    private float circleAroundAngle = 0;
    private float circleRadius = 2.2f;

    private bool rotateButDontMove = false;
    private string attackName;

    private float rotationSpeed;
    public float RotationSpeed
    {
        get
        {
            return rotationSpeed;
        }

        set
        {
            rotationSpeed = value;
        }
    }

    private float health = 3;
    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            if(health == 0)
            {
                Destroy(gameObject);
                GameObject.Find("Spawner").GetComponent<Spawner>().CancelInvokes();
            }
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, health / 3.0f);
            RotationSpeed += 1;
        }
    }

    private bool shouldMove=true;
    public bool ShouldMove
    {
        get
        {
            return shouldMove;
        }

        set
        {
            shouldMove = value;
            if (value == false)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
    

    void Start()
    {
        RotationSpeed = 1;
        InvokeRepeating("ChangeDirection", 0f, 2f);
        Attack();
    }

    void Update()
    {
        if (ShouldMove || rotateButDontMove)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + RotationSpeed);
        }

        if(transform.position.x >= 2.4f || transform.position.x <= -2.4f)
        {
            GetComponent<Rigidbody2D>().velocity *= Vector2.left;
        }

        if(transform.position.y >= 4.6f || transform.position.y <= -4.6f)
        {
            GetComponent<Rigidbody2D>().velocity *= Vector2.down;
        }

        if(destination != Vector3.up*1000)
        {
            Vector3 newPos =  Vector3.MoveTowards(transform.position, destination, 0.5f);
            transform.position = newPos;

            if(transform.position == destination)
            {
                destination = Vector3.up*1000;
            }
        }

        if(circleAroundPlayer && GameObject.Find("Player"))
        {     
            if (circleAroundAngle % 90 == 0)
            {
                int angleToUse = (int)((circleAroundAngle % 360) / 90);
                int baseAngle = 0;
                switch(angleToUse)
                {
                    case 0:
                        baseAngle = 270;
                        break;
                    case 1:
                        baseAngle = 0;
                        break;
                    case 2:
                        baseAngle = 90;
                        break;
                    case 3:
                        baseAngle = 180;
                        break;
                }
                Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, baseAngle));
                Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, baseAngle+15));
                Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, baseAngle-15));
            }
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + RotationSpeed);

            Vector3 center = GameObject.Find("Player").transform.position;
            center.y += Mathf.Sin(circleAroundAngle * Mathf.Deg2Rad) * circleRadius;
            center.x += Mathf.Cos(circleAroundAngle * Mathf.Deg2Rad) * circleRadius;

            destination = center;
            circleAroundAngle += name.StartsWith("Tap") ? 1.5f : 3;
        }
    }

    private void ChangeDirection()
    {
        if (ShouldMove)
        {
            float theta = Random.Range(0, 2 * Mathf.PI);
            Vector2 newDirection = new Vector2(Mathf.Cos(theta) * velocityMagnitude, Mathf.Sin(theta) * velocityMagnitude);
            GetComponent<Rigidbody2D>().velocity = newDirection;
        }
    }

    void Attack()
    {
        float spawnIn = 0.25f;
        float cancelInvokesIn = 10f;
        int attackType = gameObject.name.Equals("TapBoss(Clone)") ? Random.Range(0, 3) : Random.Range(3, 8);
        attackName = "";

        switch(attackType)
        {
            case 0:
                ShouldMove = true;
                InvokeRepeating("spawnHeatseeker", spawnIn, 3f);
                attackName = "spawnHeatseeker";
                cancelInvokesIn = 12f;
                break;

            case 1:
                ShouldMove = false;
                InvokeRepeating("spawnCircler", spawnIn, 2f);
                attackName = "spawnCircler";
                break;

            case 2:
                Invoke("makeBossCircle", 0.5f);
                break;

            case 3:
                ShouldMove = false;
                destination = new Vector3(0, 4.5f, 0);
                InvokeRepeating("spawnHoseBullet", spawnIn, 0.2f);
                attackName = "spawnHoseBullet";
                break;

            case 4:
                Invoke("makeBossCircle", 0.5f);
                break;

            case 5:
                ShouldMove = false;
                destination = new Vector3(0, 4.5f, 0);
                InvokeRepeating("spawnStarStorm", spawnIn, 0.5f);
                Instantiate(starPatrol, transform.position, Quaternion.identity);
                attackName = "spawnStarStorm";
                break;

            case 6:
                ShouldMove = true;
                destination = new Vector3(0, 4.5f, 0);
                InvokeRepeating("spawnBulletCircle", spawnIn, 0.8f);
                attackName = "spawnBulletCircle";
                break;

            case 7:
                destination = Vector3.zero;
                rotateButDontMove = true;
                ShouldMove = false;
                InvokeRepeating("spawnSpinnerBulletSet", spawnIn, 0.2f);
                attackName = "spawnSpinnerBulletSet";
                break;
        }

        Invoke("CancelInvokes", cancelInvokesIn);
    }

    void makeBossCircle()
    {
        circleAroundPlayer = true;
    }

    void CancelInvokes()
    {
        circleAroundPlayer = false;
        CancelInvoke(attackName);
        SpawnLifeForce();
        Invoke("Attack", 3.5f);
    }

    void SpawnLifeForce()
    {
        ShouldMove = true;
        GameObject life = Instantiate(lifeForce);
        life.GetComponent<ColorChanger>().isNormal = false;
        life.GetComponent<ColorChanger>().boss = gameObject;
    }

    void spawnHeatseeker()
    {
        Instantiate(heatSeeker, transform.position, Quaternion.identity);
    }

    void spawnCircler()
    {
        destination = Vector3.zero;
        Instantiate(circler, transform);
    }

    void spawnMaze()
    {
        GameObject.Find("Screen").GetComponent<Movement>().movementChanging = true;
        destination = Vector3.up * 4;
        Instantiate(maze);
        Invoke("allowMovement", 2f);
    }

    void spawnHoseBullet()
    {
        Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, hoseBulletRotation));
        Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, -hoseBulletRotation));

        Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, hoseBulletRotation2));
        Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, -hoseBulletRotation2));

        Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, hoseBulletRotation3));
        Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, -hoseBulletRotation3));

        if (hoseBulletRotation < -90 || hoseBulletRotation > 90)
        {
            hoseBulletIncrement *= -1;
        }

        if (hoseBulletRotation2 < -90 || hoseBulletRotation2 > 90)
        {
            hoseBulletIncrement2 *= -1;
        }

        if (hoseBulletRotation3 < -45 || hoseBulletRotation3 > 45)
        {
            hoseBulletIncrement3 *= -1;
        }

        hoseBulletRotation += hoseBulletIncrement;
        hoseBulletRotation2 += hoseBulletIncrement2;
        hoseBulletRotation3 += hoseBulletIncrement3;
    }

    void spawnStarStorm()
    {

        Vector3 topPos = Vector3.up * 4.5f;
        Vector3 leftPos = new Vector3(-2.5f, 2, 0);
        Vector3 rightPos = new Vector3(2.5f, 2, 0); ;

        Quaternion topPosRotation1 = Quaternion.Euler(0, 0, 18);
        Quaternion topPosRotation2 = Quaternion.Euler(0, 0, -18); 

        Quaternion leftPosRotation1 = Quaternion.Euler(0, 0, 90);
        Quaternion leftPosRotation2 = Quaternion.Euler(0, 0, 36); 

        Quaternion rightPosRotation = Quaternion.Euler(0, 0, -36);

        Instantiate(bulletRectangle, topPos, topPosRotation1);
        Instantiate(bulletRectangle, topPos, topPosRotation2);

        Instantiate(bulletRectangle, leftPos, leftPosRotation1);
        Instantiate(bulletRectangle, leftPos, leftPosRotation2);

        Instantiate(bulletRectangle, rightPos, rightPosRotation);
    }

    void spawnBulletCircle()
    {
        for(int x = 0; x<=360; x+=10)
        {
            Instantiate(bulletRectangle, transform.position, Quaternion.Euler(0, 0, x));
        }
    }

    void spawnSpinnerBulletSet()
    {
        Instantiate(spinnerBulletSet, transform.position, transform.rotation);
    }

    void allowMovement()
    {
        GameObject.Find("Screen").GetComponent<Movement>().movementChanging = false;
    }
}
