using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarityMeter : MonoBehaviour {
    public Sprite[] sprites;
    private int polarity;

    public int Polarity
    {
        get
        {
            return polarity;
        }

        set
        {
            polarity = value%11;
            GetComponent<SpriteRenderer>().sprite = sprites[polarity];
        }
    }

    private void Update()
    {
        if (GameObject.Find("Player"))
        {
            transform.position = GameObject.Find("Player").transform.position + new Vector3(0.02f, 0.03f, 0);
        }
    }


}
