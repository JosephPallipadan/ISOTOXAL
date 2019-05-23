using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleWave : MonoBehaviour {
    private int direction;

    private void Start()
    {
        direction = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    void Update () {
        if (gameObject.name.Equals("RectangleWave(Clone)"))
        {
            transform.Translate(Vector2.down * Time.deltaTime * 4);

            direction = Mathf.Abs(transform.position.x) > 2.8f ? direction * -1 : direction;
            transform.Translate(Vector2.right * Time.deltaTime * 2 * direction);
        }
        else
        {
            transform.Translate(Vector2.right * Time.deltaTime * 4);

            if(transform.position.x < 0.5f || transform.position.x > 2.8f)
            {
                direction *= -1;
            }
            transform.Translate(Vector2.down * Time.deltaTime * direction);
        }
    }
}
