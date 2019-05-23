using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoColorRectangle : MonoBehaviour {

    private float primaryRatio;
    private float oscillationVelocity=2f;

    public Color primaryColor;
    public Color secondaryColor;
    public bool gapless = false;
    public bool moving = false;
    

    private void Start()
    {
        if (gapless)
        {
            primaryRatio = Random.Range(0.2f, 0.8f);

            transform.Find("Color (1)").localScale = new Vector2(primaryRatio, 0.05f);

            transform.Find("Color (2)").localScale = new Vector2(1 - primaryRatio, 0.05f);

            float amountToMoveBy = primaryRatio * 5.68f;
            transform.Find("Color (2)").localPosition += Vector3.right * amountToMoveBy;
        }
        else
        {
            primaryRatio = Random.Range(0.2f, 0.6f);

            transform.Find("Color (1)").localScale = new Vector2(primaryRatio, 0.05f);

            transform.Find("Color (2)").localScale = new Vector2(0.8f - primaryRatio, 0.05f);

            float amountToMoveBy = (primaryRatio+0.2f) * 5.68f;
            transform.Find("Color (2)").localPosition += Vector3.right * amountToMoveBy;
        }

        transform.Find("Color (1)").GetComponent<SpriteRenderer>().color = primaryColor;
        transform.Find("Color (2)").GetComponent<SpriteRenderer>().color = secondaryColor;
    }

    private void Update()
    {
        if (moving)
        {
            transform.Translate(Vector2.right * oscillationVelocity * Time.deltaTime);
            if (Mathf.Abs(transform.position.x) >= 1)
            {
                oscillationVelocity *= -1;
            }
        }
    }
}
