using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {

    public GameObject colorChanger;
    public GameObject twoColorRectangle;
    public GameObject oneColorRectangle;
    public GameObject bulletRectangleSet;
    public GameObject verticalTwoColorRectangleSet1;
    public GameObject verticalTwoColorRectangleSet2;
    public GameObject rainDrop;
    public GameObject rectangleWave;
    public GameObject verticalRectangleWave;
    public GameObject tapBoss;
    public GameObject dragBoss;

    public GameObject gameOverDisplay;
    public GameObject restartDisplay;
    public GameObject returnDisplay;

    public Color primaryColor;
    public Color secondaryColor;
    public int ongoingWave;

    private Movement screen;
    private GameObject hitBox;
    private bool spawnMovingRectangles=false;
    private int wave=0;
    private float bulletRectangleSpawnPosX = -1;
    private float bulletRectangleSpawnPosXIncrement = 0.2f;

    public int Wave
    {
        get
        {
            return wave;
        }

        set
        {
            wave = value;
            GameObject.Find("Wave Display").GetComponent<TextMeshProUGUI>().text = "Wave " + wave;
            GameObject.Find("Wave Display").GetComponent<Animator>().SetTrigger("Fade");
            GameObject.Find("Stats").GetComponent<Animator>().SetTrigger("Fade");
        }
    }

    void Start () {
        hitBox = GameObject.Find("Hitbox");
        screen = GameObject.Find("Screen").GetComponent<Movement>();
        Wave++;
        Invoke("SetupWave", 8f);
    }

    void SetupWave()
    {
        screen.MoveByTap = true;
        Invoke("actuallySetupWave", 1f);
    }

    void actuallySetupWave()
    {
        primaryColor = Random.ColorHSV();
        secondaryColor = new Color(1 - primaryColor.r, 1 - primaryColor.g, 1 - primaryColor.b, 1);

        GameObject colorChangerObject = Instantiate(colorChanger);
        colorChangerObject.GetComponent<ColorChanger>().Color = primaryColor;
    }

    public void SpawnWave(int type)
    {
        ongoingWave = type;
        float startSpawningIn=2f;

        bool bossWave = type == 8 || type == 9;

        switch (type)
        {
            case 0:
                screen.MoveByTap = false;

                spawnMovingRectangles = false;
                InvokeRepeating("SpawnHorizontalTwoColorRectangle", startSpawningIn, 0.25f);
                break;
            case 1:
                screen.MoveByTap = false;

                InvokeRepeating("SpawnVerticalTwoColorRectangleSet", startSpawningIn, 2f);
                break;
            case 2:
                screen.MoveByTap = false;

                spawnMovingRectangles = true;
                InvokeRepeating("SpawnHorizontalTwoColorRectangle", startSpawningIn, 0.8f);
                break;
            case 3:
                screen.MoveByTap = true;

                InvokeRepeating("SpawnVerticalOneColorRectangleSet", startSpawningIn, 1f);
                break;

            case 4:
                screen.MoveByTap = false;

                InvokeRepeating("SpawnBulletRectangleSet", startSpawningIn, 0.6f);
                break;

            case 5:
                screen.MoveByTap = true;

                InvokeRepeating("SpawnVerticalRectangleRain", startSpawningIn, 0.6f);
                break;

            case 6:
                screen.MoveByTap = false;

                InvokeRepeating("SpawnRectangleWave", startSpawningIn-1.5f, 1f);
                break;
            case 7:
                screen.MoveByTap = false;

                InvokeRepeating("SpawnVerticalRectangleWave", startSpawningIn - 1.5f, 1f);
                break;
            case 8:
                Instantiate(tapBoss);
                break;
            case 9:
                screen.MoveByTap = false;

                Invoke("InstantiateDragBoss", 1f);
                break;
        }

        if (!bossWave)
        {
            Invoke("CancelInvokes", 15);
        }
    }

    void InstantiateDragBoss()
    {
        Instantiate(dragBoss);
    }

    public void CancelInvokes()
    {
        CancelInvoke();
        Invoke("DisplayWave", 2f);
    }

    void DisplayWave()
    {
        Wave++;
        Invoke("SetupWave", 8f);
    }

    void SpawnHorizontalTwoColorRectangle()
    {
        GameObject thisObject = Instantiate(twoColorRectangle);
        thisObject.GetComponent<Rigidbody2D>().velocity = Vector2.down * 5;
        if (spawnMovingRectangles)
        {
            thisObject.GetComponent<TwoColorRectangle>().moving = true;
            thisObject.transform.localScale = new Vector3(2, 0.8f, 0);
            thisObject.GetComponent<Rigidbody2D>().velocity = Vector2.down * 2;
        }

        if (Random.Range(0, 2) == 0)
        {
            thisObject.GetComponent<TwoColorRectangle>().primaryColor = primaryColor;
            thisObject.GetComponent<TwoColorRectangle>().secondaryColor = secondaryColor;
        }
        else
        {
            thisObject.GetComponent<TwoColorRectangle>().primaryColor = secondaryColor;
            thisObject.GetComponent<TwoColorRectangle>().secondaryColor = primaryColor;
        }
    }

    void SpawnVerticalTwoColorRectangleSet()
    {
        GameObject thisObject = Instantiate(Random.Range(0, 2) == 0 ? verticalTwoColorRectangleSet1 : verticalTwoColorRectangleSet2);
        thisObject.GetComponent<Rigidbody2D>().velocity = Vector2.down * 10;
        foreach(TwoColorRectangle child in thisObject.GetComponentsInChildren<TwoColorRectangle>())
        {
            if (Random.Range(0, 2) == 0)
            {
                child.primaryColor = primaryColor;
                child.secondaryColor = secondaryColor;
            }
            else
            {
                child.primaryColor = secondaryColor;
                child.secondaryColor = primaryColor;
            }
        }
    }

    void SpawnVerticalOneColorRectangleSet()
    {
        float hitBoxXPos = hitBox.transform.position.x;

        float rectangleXPos;
        float secondRectangleXPos;
        if(hitBoxXPos<=-1)
        {
            rectangleXPos = -2.85f;
            secondRectangleXPos = Random.Range(0, 2) == 0 ? -0.95f : 0.925f;
        }
        else if(hitBoxXPos>-1 && hitBoxXPos<=1)
        {
            rectangleXPos = -0.95f;
            secondRectangleXPos = Random.Range(0, 2) == 0 ? -2.85f : 0.925f;
        }
        else
        {
            rectangleXPos = 0.925f;
            secondRectangleXPos = Random.Range(0, 2) == 0 ? -0.95f : -2.85f;
        }

        Vector2 spawnPos = new Vector2(rectangleXPos, 11);
        Vector2 secondSpawnPos = new Vector2(secondRectangleXPos, 11);

        GameObject firstRectangle = Instantiate(oneColorRectangle, spawnPos, Quaternion.identity);
        GameObject secondRectangle = Instantiate(oneColorRectangle, secondSpawnPos, Quaternion.identity);

        firstRectangle.GetComponent<Rigidbody2D>().velocity = Vector2.down * 15;
        secondRectangle.GetComponent<Rigidbody2D>().velocity = Vector2.down * 15;

        firstRectangle.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        secondRectangle.GetComponent<SpriteRenderer>().color = secondaryColor;
    }

    void SpawnBulletRectangleSet()
    {
        Instantiate(bulletRectangleSet, new Vector3(bulletRectangleSpawnPosX, 6f), Quaternion.identity);
        Instantiate(bulletRectangleSet, new Vector3(-bulletRectangleSpawnPosX, 6f), Quaternion.identity);
        bulletRectangleSpawnPosX += bulletRectangleSpawnPosXIncrement;
        if(Mathf.Abs(bulletRectangleSpawnPosX)>=2.5)
        {
            bulletRectangleSpawnPosXIncrement *= -1;
        }
    }

    void SpawnVerticalRectangleRain()
    {
        GameObject rain = Instantiate(rainDrop, hitBox.transform.position, Quaternion.identity);
        rain.GetComponent<SpriteRenderer>().color = Random.Range(0, 2)==0 ? primaryColor : secondaryColor;
        rain.transform.Translate(Vector2.up * 9);
        rain.GetComponent<Rigidbody2D>().velocity = Vector2.down * 20;

        if (Random.Range(0, 2) == 0)
        {
            Vector3 pos = hitBox.transform.position * Vector2.left;
            GameObject rain1 = Instantiate(rainDrop, pos, Quaternion.identity);
            rain1.GetComponent<SpriteRenderer>().color = Random.Range(0, 2) == 0 ? primaryColor : secondaryColor;
            rain1.transform.Translate(Vector2.up * 9);
            rain1.GetComponent<Rigidbody2D>().velocity = Vector2.down * 20;
        }
    }

    void SpawnRectangleWave()
    {
        Instantiate(rectangleWave);
    }

    void SpawnVerticalRectangleWave()
    {
        Instantiate(verticalRectangleWave);
    }

    public void SetUpRestart()
    {
        Invoke("actuallySetUpRestart", 0.8f);
    }

    void actuallySetUpRestart()
    {
        if(PlayerPrefs.HasKey("High Score"))
        {
            PlayerPrefs.SetInt("High Score", Mathf.Max(PlayerPrefs.GetInt("High Score"), wave));
        }
        else
        {
            PlayerPrefs.SetInt("High Score", wave);
        }
        PlayerPrefs.SetInt("Score", wave);
        SceneManager.LoadScene(4);
    }

    public void loadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
