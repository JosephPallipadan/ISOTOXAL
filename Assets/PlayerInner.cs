using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerInner : MonoBehaviour {

    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip polarizeOneSound;
    public AudioClip polarizeSetSound;

    private float health;
    private int polarity;
    private int rotationSpeed;

    private Movement screen;

    private Vector3 destination = Vector3.positiveInfinity;

    private TextMeshProUGUI statusDisplay;
    private GameObject plus;
    private GameObject minus;
    private Spawner spawner;

    public Vector3 Destination
    {
        get
        {
            return destination;
        }

        set
        {
            destination = value;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            if (value < health)
            {
                displayMinus();
                AudioSource.PlayClipAtPoint(hitSound, Vector3.zero);
            }

            if(value>1)
            {
                value = 1f;
            }

            RotationSpeed += Mathf.RoundToInt((health-value)*10);

            Debug.Log(RotationSpeed + " " + health + " " + value);

            health = value;
            GetComponent<SpriteRenderer>().color = statusDisplay.color = Color.Lerp(Color.red, Color.white, health);
            statusDisplay.text = "Damage: " + ((int)((1 - health) * 10));


            if (health<=0)
            {
                destroyPlayer();
            }
        }
    }

    public int RotationSpeed
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

    public int Polarity
    {
        get
        {
            return polarity;
        }

        set
        {
            polarity = value;
            //GameObject.Find("PolarityMeter").GetComponent<PolarityMeter>().Polarity = value;
            if(value%10 == 0)
            {
                AudioSource.PlayClipAtPoint(polarizeSetSound, Vector3.zero);
                Health += 0.1f;
                displayPlus();
            }
            else
            {
                AudioSource.PlayClipAtPoint(polarizeSetSound, Vector3.zero);
            }
            statusDisplay.text = "Damage: " + ((int)((1 - health) * 10));
        }
    }

    // Use this for initialization
    void Start () {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        plus = GameObject.Find("Plus");
        minus = GameObject.Find("Minus");
        screen = GameObject.Find("Screen").GetComponent<Movement>();
        statusDisplay = GameObject.Find("Stats").GetComponent<TextMeshProUGUI>();
        
        if(destination == Vector3.positiveInfinity)
        {
            destination = transform.position;
        }

        health = 1f;
        RotationSpeed = 1;
    }
	
	// Update is called once per frame
	void Update () {
        minus.transform.position = transform.position + new Vector3(-0.5f, 0.5f);
        plus.transform.position = transform.position + new Vector3(0.5f, 0.5f);

        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + RotationSpeed);

        if (!Destination.Equals(transform.position) && screen.MoveByTap)
        {
            transform.position = Vector3.MoveTowards(transform.position, Destination, 0.5f);
        }
	}

    private void destroyPlayer()
    {
        AudioSource.PlayClipAtPoint(deathSound, Vector3.zero);

        GameObject.Find("Hitbox").transform.parent = null;
        GameObject.Find("Hitbox").GetComponent<Animator>().SetTrigger("Destroy Core");
        Destroy(GameObject.Find("Hitbox").GetComponent<CircleCollider2D>());

        Time.timeScale = 0.2f;

        spawner.CancelInvoke();
        spawner.SetUpRestart();

        Destroy(GameObject.Find("Minus"));
        Destroy(GameObject.Find("Plus"));
        Destroy(GameObject.Find("PolarityMeter"));
        Destroy(gameObject);
    }

    void displayPlus()
    {
        plus.GetComponent<SpriteRenderer>().color = spawner.primaryColor;
        Invoke("hidePlus", 0.5f);
    }

    void hidePlus()
    {
        plus.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    void displayMinus()
    {
        minus.GetComponent<SpriteRenderer>().color = spawner.secondaryColor;
        Invoke("hideMinus", 0.5f);
    }

    void hideMinus()
    {
        minus.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
