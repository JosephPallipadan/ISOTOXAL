using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoRotator : MonoBehaviour {

    float color = 0.7f;
    float increment = -0.001f;
	
	void Update () {
       transform.eulerAngles += Vector3.forward+Vector3.forward*Time.timeSinceLevelLoad/50;

        if (!GameObject.Find("Score"))
        {
            GameObject.Find("Image").GetComponent<Image>().color = new Color(color, color, color);
        }

        color += increment;
        if(color<=0.3f || color>=0.7f)
        {
            increment *= -1;
        }
    }
}
