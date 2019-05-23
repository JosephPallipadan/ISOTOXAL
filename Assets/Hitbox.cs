using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.parent==null)
        {
            return;
        }

        Color color = Color.white;

        if(collision.gameObject.GetComponent<SpriteRenderer>())
        {
            color = collision.gameObject.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            return;
        }

        if(collision.gameObject.name.StartsWith("MazeSquare"))
        {
            if(color == Color.white)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                collision.gameObject.transform.parent.parent.GetComponent<Maze>().LengthToCoverForPlayer--;
            }
            else if (color == Color.red || color == Color.black)
            {
                GetComponentInParent<PlayerInner>().Health -= 0.1f;
            }
            else if(color == Color.green)
            {
                GetComponentInParent<PlayerInner>().Health += 0.3f;
            }
        }

        else if(!collision.gameObject.GetComponent<ColorChanger>() && collision.gameObject.GetComponent<SpriteRenderer>() )
        {
            if (color != GetComponent<SpriteRenderer>().color)
            {
                GetComponentInParent<PlayerInner>().Health -= 0.1f;
                Time.timeScale = 0.2f;
                Invoke("resetTimeScale", 0.1f);
            }
            else
            {
                GetComponentInParent<PlayerInner>().Polarity += 1;
            }
        }
    }

    void resetTimeScale()
    {
        Time.timeScale = 1f;
    }

    private void destroy()
    {
       Destroy(gameObject);
    }
}
