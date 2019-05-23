using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeker : MonoBehaviour {

    public GameObject explosion;

    private void Start()
    {
        Invoke("explode", 3f);
    }

    private void Update()
    {
        if (GameObject.Find("Player"))
        {
            transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, 0.05f);
        }
    }

    void explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
