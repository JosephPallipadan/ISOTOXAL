using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WaveBullet : MonoBehaviour {

    void Start() {
        int length = gameObject.name.Length;
        int number = Int32.Parse(gameObject.name.Substring(length - 2, 1)) % 4;

        Spawner spwnr = GameObject.Find("Spawner").GetComponent<Spawner>();
        GetComponent<SpriteRenderer>().color = UnityEngine.Random.Range(0, 3) == 0 ? spwnr.primaryColor : spwnr.secondaryColor;

        switch (number)
        {
            case 0:
                GetComponent<Rigidbody2D>().velocity = Vector2.up * 0.8f;
                break;
            case 1:
                GetComponent<Rigidbody2D>().velocity = Vector2.up * 0.2f;
                break;
            case 2:
                GetComponent<Rigidbody2D>().velocity = Vector2.up * 0.4f;
                break;
            case 3:
                GetComponent<Rigidbody2D>().velocity = Vector2.up * 0.6f;
                break;
        }
        GetComponent<Rigidbody2D>().velocity *= 3f;

        length = gameObject.transform.parent.name.Length;
        int parentNumber = Int32.Parse(gameObject.transform.parent.name.Substring(length - 2, 1));
        if (parentNumber == 3 || parentNumber == 4)
        {
            GetComponent<Rigidbody2D>().velocity *= -1;
        }

        InvokeRepeating("invertVelocity", 0.25f, 0.5f);
	}
	
	void invertVelocity()
    {
        GetComponent<Rigidbody2D>().velocity *= -1;
    }
}
