using UnityEngine;
using System;

public class RedFlag : MonoBehaviour
{
    private string separator = ",";

    public RedFlagSaveData GetData()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        return new RedFlagSaveData()
        {
            RedFlagPosition = transform.localPosition,
            RedFlagRotation = transform.localRotation,
            objNum = float.Parse(transform.transform.Find("Number").GetComponent<TextMesh>().text)
        };
    }
    
    public void SetData(RedFlagSaveData data)
    {
        transform.localPosition = data.RedFlagPosition;
        transform.localRotation = data.RedFlagRotation;

        // if(Environment.UserName == "tech")
        // {
        //     separator = "."; // для Дины этот
        // }
        if(data.objNum.ToString() == "0"+separator+"1" || data.objNum.ToString() == "0"+separator+"2" || data.objNum.ToString() == "0"+separator+"3" || data.objNum.ToString() == "0"+separator+"4" || data.objNum.ToString() == "0"+separator+"5" || data.objNum.ToString() == "0"+separator+"6" || data.objNum.ToString() == "0"+separator+"7" || data.objNum.ToString() == "0"+separator+"8")
        {
            float d = data.objNum*10;
            int i = (int)d;
            //int fractional = data.objNum - d;
            transform.Find("Number").GetComponent<MeshRenderer>().enabled = false; // Выключаю отображение номера
            transform.Find("Number_fix").GetComponent<TextMesh>().text = i.ToString();
        }

        transform.transform.Find("Number").GetComponent<TextMesh>().text = data.objNum.ToString();
    }
}
