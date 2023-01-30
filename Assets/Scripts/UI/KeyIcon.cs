using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyIcon : MonoBehaviour
{

    Image keyIconColor;
    Color setBYellow;
    public TMP_Text key;
    // Start is called before the first frame update
    void Start()
    {
        keyIconColor = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IconOnColor()
    {

        setBYellow = keyIconColor.color;
        setBYellow.b = 0f;
        key.text = 1.ToString();
        keyIconColor.color = setBYellow;
    }
}
