using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    public AudioClip captureSound;

    private Color color;
    private int velocityMagnitude = 3;

    public int waveNumber = -1;
    public bool isNormal = true;

    public GameObject boss;

    public Color Color
    {
        get
        {
            return color;
        }

        set
        {
            color = value;
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    void Start () {
        transform.Translate(Vector3.right * Random.Range(-2f, 2f));

        InvokeRepeating("ChangeDirection", 0.5f, 0.5f);
        Invoke("destroy", 3f);
	}

    private void ChangeDirection()
    {
        float theta = Random.Range(0, 2*Mathf.PI);
        Vector2 newDirection = new Vector2(Mathf.Cos(theta)*velocityMagnitude, Mathf.Sin(theta)*velocityMagnitude );
        GetComponent<Rigidbody2D>().velocity = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Hitbox>())
        {
            AudioSource.PlayClipAtPoint(captureSound, Vector3.zero);
            if (isNormal)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color;
            }
            else
            {
                boss.GetComponent<Boss>().Health--;
            }
            destroy();
        }
    }

    void destroy()
    {
        CancelInvoke();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetTrigger("Destroy");
        Destroy(GetComponent<CircleCollider2D>());
    }

    void actuallyDestroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (isNormal && GameObject.Find("Spawner"))
        {
            if (GameObject.Find("Spawner").GetComponent<Spawner>().Wave % 5 == 0)
            {
                GameObject.Find("Spawner").GetComponent<Spawner>().SpawnWave(Random.Range(0, 2)+8);
            }
            else
            {
                GameObject.Find("Spawner").GetComponent<Spawner>().SpawnWave(waveNumber == -1 ? Random.Range(0, 8) : waveNumber);
            }
        }
    }
}
