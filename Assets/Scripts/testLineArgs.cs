using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class testLineArgs : MonoBehaviour
{
    [SerializeField] private Text LogText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Console.WriteLine();
        //  Invoke this sample with an arbitrary set of command line arguments.
        string[] arguments = Environment.GetCommandLineArgs();
        //Debug.Log(arguments);

        LogText.text = string.Join(", ", arguments);
        //LogText.text = arguments[0];
        //Console.WriteLine("GetCommandLineArgs: {0}", string.Join(", ", arguments));
        
    }
}