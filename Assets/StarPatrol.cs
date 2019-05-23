using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPatrol : MonoBehaviour {

    private Transform player;
    private float followSpeed = 0.03f;
	
	void Start () {
        player = GameObject.Find("Player").transform;
        GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		if(player)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed);
        }
	}
}
