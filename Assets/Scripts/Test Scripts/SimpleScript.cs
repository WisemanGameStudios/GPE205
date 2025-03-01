using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScript : MonoBehaviour
{

    public string theText = "Hello World";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Displays the variable stored in theText
        Debug.Log(theText);
        
    }

}
