using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRectangle : MonoBehaviour {

	void Start () {
        Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        Color primary = spawner.primaryColor;
        Color secondary = spawner.secondaryColor;

        if (GetComponentInParent<CircleAnchor>())
        {
            GetComponent<SpriteRenderer>().color = secondary;
        }
        else
        {
            float rotation = transform.rotation.eulerAngles.z;
            rotation *= Mathf.Deg2Rad;
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sin(rotation), Mathf.Cos(rotation) * -1) * 3;

            if (spawner.ongoingWave == 9)
            {
                GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Random.Range(0, 3) == 0 ? primary : secondary;
            }
        }
    }
}
