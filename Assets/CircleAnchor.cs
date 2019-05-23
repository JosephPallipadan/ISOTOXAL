using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnchor : MonoBehaviour {

    private GameObject circler;

    private Vector3 circlerDestination;
    private Vector3 returnPosition;
    private Quaternion anchorRotation;

    private bool firing=false;
    private bool returning=false;

    private int rotationSpeed = 3;
    private float fireSpeed = 0.2f;

    private int fireCount = 0;

	// Use this for initialization
	void Start () {
        circler = transform.Find("Circler").gameObject;
        Invoke("destroy", 2f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotationSpeed);
        if(firing)
        {
            transform.rotation = anchorRotation;
            circler.transform.position = Vector3.MoveTowards(circler.transform.position, circlerDestination, fireSpeed);
            if(circler.transform.position == circlerDestination)
            {
                firing = false;
                returning = true;
                rotationSpeed = 0;
            }
        }
        else if(returning)
        {
            transform.rotation = anchorRotation;
            circler.transform.position = Vector3.MoveTowards(circler.transform.position, returnPosition, 0.1f);
            if (circler.transform.position == returnPosition)
            {
                returning = false;
                rotationSpeed = 3;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!firing && !returning)
        {
            anchorRotation = transform.rotation;
            circlerDestination = GameObject.Find("Player").transform.position;
            firing = true;
            returnPosition = circler.transform.position;

            fireCount++;
            if (fireCount >= 3)
            {
                Destroy(gameObject);
            }
        }
    }

    void destroy()
    {
        Destroy(gameObject);
    }
}
