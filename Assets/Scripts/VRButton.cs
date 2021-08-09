using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class VRButton : MonoBehaviour
{
    GameObject RG1;
    GameObject RG2;
    GameObject RG3;
    GameObject RG4;

    private bool RG1Active = true;
    private bool RG2Active = true;
    private bool RG3Active = true;
    private bool RG4Active = true;
    private bool test = false;
    JsonRadarogramReader.CommonData myJsonRadarogramData;
    [SerializeField] private GameObject textPanel;
    void Start()
    {
        //FindRadarogram();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideRadarogram1()
    {
        if(test == false)
        {
            //Debug.Log("test == false");
            FindRadarogram();
        }
        RG1Active = !RG1Active;
        RG1.SetActive(RG1Active);
        //GameObject.Find("radarogramPrefab_" + transform.parent.Find("Number").GetComponent<TextMesh>().text).SetActive(false);
        if (transform.Find("TextHR1").GetComponent<Text>().text == "Скрыть 1")
        {
            transform.Find("TextHR1").GetComponent<Text>().text = "Отобразить 1";
        }
        else if (transform.Find("TextHR1").GetComponent<Text>().text == "Отобразить 1")
        {
            transform.Find("TextHR1").GetComponent<Text>().text = "Скрыть 1";
        }
    }
    public void HideRadarogram2()
    {
        if(test == false)
        {
            //Debug.Log("test == false");
            FindRadarogram();
        }
        RG2Active = !RG2Active;
        RG2.SetActive(RG2Active);
        //GameObject.Find("radarogramPrefab_" + transform.parent.Find("Number").GetComponent<TextMesh>().text).SetActive(false);
        if (transform.Find("TextHR2").GetComponent<Text>().text == "Скрыть 2")
        {
            transform.Find("TextHR2").GetComponent<Text>().text = "Отобразить 2";
        }
        else if (transform.Find("TextHR2").GetComponent<Text>().text == "Отобразить 2")
        {
            transform.Find("TextHR2").GetComponent<Text>().text = "Скрыть 2";
        }
    }
    public void HideRadarogram3()
    {
        if(test == false)
        {
            //Debug.Log("test == false");
            FindRadarogram();
        }
        RG3Active = !RG3Active;
        RG3.SetActive(RG3Active);
        //GameObject.Find("radarogramPrefab_" + transform.parent.Find("Number").GetComponent<TextMesh>().text).SetActive(false);
        if (transform.Find("TextHR3").GetComponent<Text>().text == "Скрыть 3")
        {
            transform.Find("TextHR3").GetComponent<Text>().text = "Отобразить 3";
        }
        else if (transform.Find("TextHR3").GetComponent<Text>().text == "Отобразить 3")
        {
            transform.Find("TextHR3").GetComponent<Text>().text = "Скрыть 3";
        }
    }
    public void HideRadarogram4()
    {
        if(test == false)
        {
            //Debug.Log("test == false");
            FindRadarogram();
        }
        RG4Active = !RG4Active;
        RG4.SetActive(RG4Active);
        //GameObject.Find("radarogramPrefab_" + transform.parent.Find("Number").GetComponent<TextMesh>().text).SetActive(false);
        if (transform.Find("TextHR4").GetComponent<Text>().text == "Скрыть 4")
        {
            transform.Find("TextHR4").GetComponent<Text>().text = "Отобразить 4";
        }
        else if (transform.Find("TextHR4").GetComponent<Text>().text == "Отобразить 4")
        {
            transform.Find("TextHR4").GetComponent<Text>().text = "Скрыть 4";
        }
    }

    public void SaveScene()
    {
        myJsonRadarogramData = GameObject.Find("scripts").GetComponent<ObjectManager>().LoadDataJson(); // Читаю data.json;
        string CurrentTime = System.DateTime.Now.ToString();
        transform.Find("TextMessage").GetComponent<Text>().text = "Радарограммы сохранены в папку исследования:   " + myJsonRadarogramData.id;
        transform.Find("TextMessage2").GetComponent<Text>().text = "Подпапка:   " + CurrentTime;
        Directory.CreateDirectory(Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime);
        
        File.Copy(Application.persistentDataPath + "/RadarogramTexture" + "/0.jpg", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime + "/0.jpg");
        File.Copy(Application.persistentDataPath + "/RadarogramTexture" + "/1.jpg", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime + "/1.jpg");
        File.Copy(Application.persistentDataPath + "/RadarogramTexture" + "/2.jpg", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime + "/2.jpg");
        File.Copy(Application.persistentDataPath + "/RadarogramTexture" + "/3.jpg", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime + "/3.jpg");
        
        File.Copy(Application.persistentDataPath + "/data.json", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime + "/data.json");
        File.Copy(Application.persistentDataPath + "/scene.json", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + CurrentTime + "/scene.json");
    }

    public void SaveScreenshot()
    {
        StartCoroutine(TimerScreenshotCoroutine());
    }

    IEnumerator TimerScreenshotCoroutine()
    {
        textPanel.GetComponent<Text>().text = "5";
        yield return new WaitForSeconds(1f);
        textPanel.GetComponent<Text>().text = "4";
        yield return new WaitForSeconds(1f);
        textPanel.GetComponent<Text>().text = "3";
        yield return new WaitForSeconds(1f);
        textPanel.GetComponent<Text>().text = "2";
        yield return new WaitForSeconds(1f);
        textPanel.GetComponent<Text>().text = "1";
        yield return new WaitForSeconds(1f);
        textPanel.GetComponent<Text>().text = "";
        //ScreenCapture.CaptureScreenshot(Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + myJsonRadarogramData.id + "/" + System.DateTime.Now.ToString());
        ScreenCapture.CaptureScreenshot("NewScreenshot.png");
        yield return new WaitForSeconds(1f);
        Directory.CreateDirectory(Application.persistentDataPath  + "/" + "Saved Radarograms");
        File.Copy(Application.persistentDataPath + "/NewScreenshot.png", Application.persistentDataPath  + "/" + "Saved Radarograms" + "/" + System.DateTime.Now.ToString() + ".png");
        yield return new WaitForSeconds(1f);
        File.Delete(Application.persistentDataPath + "/NewScreenshot.png");
        //ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/скриншот");
    }

    private void FindRadarogram()
    {
        test = true;
        RG1 =  GameObject.Find("radarogramPrefab_1");
        RG2 =  GameObject.Find("radarogramPrefab_2");
        RG3 =  GameObject.Find("radarogramPrefab_3");
        RG4 =  GameObject.Find("radarogramPrefab_4");
    }
}
