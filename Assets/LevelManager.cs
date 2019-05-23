using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour {
    public Sprite loadingSprite;

    private void Start()
    {
        if(GameObject.Find("Score"))
        {
            GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Reached Wave " + PlayerPrefs.GetInt("Score");
            GameObject.Find("High Score").GetComponent<TextMeshProUGUI>().text = "Highest: Wave " + PlayerPrefs.GetInt("High Score");
        }
    }

    public void loadLevel(int level)
    {
        if (level <= 4)
        {
            Destroy(GameObject.Find("Rotating Logo"));
            GameObject.Find("Image").GetComponent<Image>().sprite = loadingSprite;
            SceneManager.LoadScene(level);
        }
        else
        {
            Application.OpenURL("www.soundimage.org");
        }
    }
}
