using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

    private Spawner spawner;

    private void Start()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ColorChanger>())
        {
            Destroy(collision.gameObject);
        }

        if(spawner.ongoingWave >= 8)
        {
            if (collision.gameObject.transform.parent && collision.gameObject.transform.parent.childCount == 1)
            {
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }

        if (!gameObject.name.Equals("Top"))
        {
            switch (spawner.ongoingWave)
            {
                case 0:
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    break;

                case 1:
                    Destroy(collision.gameObject.transform.parent.parent.gameObject);
                    break;

                case 2:
                    if(gameObject.name.Equals("Bottom"))
                    {
                        Destroy(collision.gameObject.transform.parent.gameObject);
                    }
                    break;

                case 3:
                    Destroy(collision.gameObject);
                    break;

                case 4:
                    if (collision.gameObject.transform.parent.childCount == 1)
                    {
                        Destroy(collision.gameObject.transform.parent.gameObject);
                    }
                    else
                    {
                        Destroy(collision.gameObject);
                    }
                    break;

                case 5:
                    Destroy(collision.gameObject);
                    break;
                case 6:
                    if (gameObject.name.Equals("Bottom"))
                    {
                        Destroy(collision.gameObject.transform.parent.parent.gameObject);
                    }
                    break;
                case 7:
                    if(gameObject.name.Equals("Right"))
                    {
                        Destroy(collision.gameObject.transform.parent.parent.gameObject);
                    }
                    break;
            }
        }

    }
}
