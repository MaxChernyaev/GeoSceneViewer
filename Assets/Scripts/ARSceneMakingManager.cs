using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.IO;

// ПРОЕКТ ДЛЯ ОТОБРАЖЕНИЯ СЦЕНЫ НА ПК

public class ARSceneMakingManager : MonoBehaviour
{
    private GameObject FindPlane; // найденная плоскость
    [SerializeField] private GameObject radarogramPrefab;

    [SerializeField] private GameObject Line; // поставь нужный префаб линии

    [SerializeField] private GameObject CatObj;
    [SerializeField] private GameObject TestText;
    [SerializeField] private Text TextLog;
    [SerializeField] private Text Obj1Distance;
    [SerializeField] private Text Obj2Distance;
    [SerializeField] private GameObject IntersectionObj;
    [SerializeField] private Text InputFieldRyvenIP;

    //[SerializeField] private ObjectManager ObjectManagerScript;
    WebClient webClient = new WebClient();
    JsonRadarogramReader.CommonData myJsonRadarogramData;
    private double lastTime;

    // private int WhiteFlagNum1 = 0;
    // private int WhiteFlagNum2 = 0;
    // private int WhiteFlagNum3 = 0;
    // private int WhiteFlagNum4 = 0;

    GameObject FirstObg_1;
    Vector3 FirstPosition_1;
    GameObject SecondObg_1;
    Vector3 SecondPosition_1;
    GameObject FirstObg_2;
    Vector3 FirstPosition_2;
    GameObject SecondObg_2;
    Vector3 SecondPosition_2;
    GameObject FirstObg_3;
    Vector3 FirstPosition_3;
    GameObject SecondObg_3;
    Vector3 SecondPosition_3;
    GameObject FirstObg_4;
    Vector3 FirstPosition_4;
    GameObject SecondObg_4;
    Vector3 SecondPosition_4;
    GameObject FirstObg_5;
    Vector3 FirstPosition_5;
    GameObject SecondObg_5;
    Vector3 SecondPosition_5;
    GameObject FirstObg_6;
    Vector3 FirstPosition_6;
    GameObject SecondObg_6;
    Vector3 SecondPosition_6;
    GameObject FirstObg_7;
    Vector3 FirstPosition_7;
    GameObject SecondObg_7;
    Vector3 SecondPosition_7;
    GameObject FirstObg_8;
    Vector3 FirstPosition_8;
    GameObject SecondObg_8;
    Vector3 SecondPosition_8;
    GameObject FirstObg_9;
    Vector3 FirstPosition_9;
    GameObject SecondObg_9;
    Vector3 SecondPosition_9;
    GameObject FirstObg_10;
    Vector3 FirstPosition_10;
    GameObject SecondObg_10;
    Vector3 SecondPosition_10;
    GameObject RG_1;
    GameObject RG_2;
    GameObject RG_3;
    GameObject RG_4;
    GameObject RG_5;
    GameObject RG_6;
    GameObject RG_7;
    GameObject RG_8;
    GameObject RG_9;
    GameObject RG_10;
    Vector3 normalizedDirection_1;
    Vector3 normalizedDirection_2;
    Vector3 normalizedDirection_3;
    Vector3 normalizedDirection_4;
    Vector3 normalizedDirection_5;
    Vector3 normalizedDirection_6;
    Vector3 normalizedDirection_7;
    Vector3 normalizedDirection_8;
    Vector3 normalizedDirection_9;
    Vector3 normalizedDirection_10;
    Vector3 MyCenter_1;
    Vector3 MyCenter_2;
    Vector3 MyCenter_3;
    Vector3 MyCenter_4;
    Vector3 MyCenter_5;
    Vector3 MyCenter_6;
    Vector3 MyCenter_7;
    Vector3 MyCenter_8;
    Vector3 MyCenter_9;
    Vector3 MyCenter_10;
    int testNUM;

    public string RadarogramPath;

    float x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,x6,y6;

    private string separator = ",";

    private int RadarogramCounter = 0;
    private int frames = 0;
    public InputField mainInputField;
    public string RelativePath;
    private int EnterCounter = 0;
    private bool firstInstall = true;
    private bool SceneExists = false;

    GameObject FirstRedFlag_1;
    GameObject SecondRedFlag_1;
    GameObject FirstRedFlag_2;
    GameObject SecondRedFlag_2;

    //public string WebServerIP = "http://192.168.110.33:8000";
    public string WebServerIP = "";
    public static string inputTextIP = "";

    private bool FlagRyvenServerConnect = false;

    private string input;
    private bool SmoothMove = false;

    //  private bool WhichSide = true;

    // public void InputFieldRyvenIP(string s)
    // {
    //     input = s;
    //     Debug.Log(input);
    //     TextLog.text = input;
    // }

    public void InitWeb()
    {
        //webClient.DownloadFile(WebServerIP + "/scene.json", Application.persistentDataPath + "/scene.json"); // скачиваю новый scene.json
        //GameObject.Find("scripts").GetComponent<ObjectManager>().LoadButton(); // Загружаю сцену из файла scene.json
        Directory.CreateDirectory(RelativePath + "\\RyvenRadarograms");

        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\data.json"))
        {
            // DATA.JSON найден!
            File.Delete(RelativePath + "\\RyvenRadarograms" + "\\data.json");
        }
        // Debug.Log(myJsonRadarogramData.time);
        // Debug.Log(myJsonRadarogramData.images.jpg1[0]);

        // Ищу основные объекты на сцене
        // FindWhiteFlag();
        
        // Ставлю заготовки под радарограммы, которые потом можно текстурировать
        // InstallBlank();
    }

    void Start()
    {
        Screen.fullScreen = true;
        Application.targetFrameRate = 60; // ограничение в 60 fps
        RelativePath = Directory.GetCurrentDirectory();
        //QualitySettings.vSyncCount = 0; // попытка снизить нагрузку на GPU
        
        // if(Environment.UserName == "tech")
        // {
        //     separator = "."; // для Дины этот
        // }
    }

    void Update()
    {
        if (FlagRyvenServerConnect)
        {
            InstallRadarogramFromWebServer();
        }

        if (GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey) // для того, чтобы не нажимались R и Alt, когда открыто меню настроек
        {
            if (Input.GetKey(KeyCode.R))
            {
                InstallRadarogram();
            }

            // плавное обнуление положения камеры по кнопке Alt
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                SmoothMove = true;
            if ((transform.position - new Vector3(0,1,0)).y > 0.1f && SmoothMove == true)
                transform.position = Vector3.Lerp(transform.position, new Vector3(0,1,0), 0.05f);
            else
                SmoothMove = false;

        }

        // try
        // {
        //     GameObject Cube = GameObject.Find("radarogramPrefab_1");
        //     // Vector3 enemyDirectionLocal = transform.InverseTransformPoint(Cube.transform.position);
        //     // if (enemyDirectionLocal.x < 0)
        //     // {
        //     //         WhichSide = false;
        //     //         //TextLog.text = "LEFT";
        //     // }
        //     // else if (enemyDirectionLocal.x > 0)
        //     // {
        //     //         WhichSide = true;
        //     //         //TextLog.text = "RIGHT";
        //     // }
        //     TextLog.text = " ";
        // }
        // catch(Exception e)
        // {
        //     TextLog.text = "&^* ";
        //     TextLog.text += e.Message;
        // }


        // ДЛЯ ИЗМЕРЕНИЯ РАССТОЯНИЯ МЕЖДУ ОБЪЕКТАМИ
        // if (GameObject.Find("point11") && GameObject.Find("point22"))
        // {
        //     Vector3 FirstPosition = GameObject.Find("point11").transform.position;
        //     Vector3 SecondPosition = GameObject.Find("point22").transform.position;
        //     Vector3 DirectionVector = SecondPosition - FirstPosition;
        //     float MyMagnitude = DirectionVector.magnitude;
        //     //Debug.Log("DirectionVector: " + DirectionVector.ToString());
        //     Debug.Log("MyMagnitude: " + MyMagnitude.ToString());
        // }
    }
    
    public void InstallRadarogram()
    {
        RadarogramPath = "\\Radarograms\\data.json";
        if (firstInstall == true)
        {
            //TextLog.text = "Попытка найти белые линии (профили)";
            FindProfile();
            if (SceneExists == true)
            {
                InstallBlank();
            }
            else
            {
                TextLog.text = "Зачем ты пытаешься загрузить радарограммы, если нет сцены? =(";
            }
        }
        //TextLog.text = "Начинаю установку радарограмм";
        if (SceneExists == true)
        {
            InstallRadarogramFromFile();
        }
        else
        {
            TextLog.text = "Зачем ты пытаешься загрузить радарограммы, если нет сцены? =(";
        }
    }

    public void FindProfile()
    {
        // firstInstall = false;
        // Ищу основные объекты на сцене
        GameObject[] allGo = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allGo)
        {
            if (go.name == "Flag_white(Clone)")
            {
                SceneExists = true; // если был найден хоть один белый флаг - считаем что сцена существует. Иначе нет, и установка радарограмм запрещена
                if (go.transform.Find("Number").GetComponent<TextMesh>().text == "1")
                {
                    RadarogramCounter++;
                    FirstObg_1 = go;
                    FirstPosition_1 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                    //TextLog.text = "Нашел 1";
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "1"+separator+"1")
                {
                    SecondObg_1 = go;
                    SecondPosition_1 = go.transform.position;
                    //TextLog.text = go.transform.Find("Number").GetComponent<TextMesh>().text;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "2")
                {
                    RadarogramCounter++;
                    FirstObg_2 = go;
                    FirstPosition_2 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                    //TextLog.text = "Нашел 2";
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "2"+separator+"1")
                {
                    SecondObg_2 = go;
                    SecondPosition_2 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "3")
                {
                    RadarogramCounter++;
                    FirstObg_3 = go;
                    FirstPosition_3 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                    //TextLog.text = "Нашел 3";
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "3"+separator+"1")
                {
                    SecondObg_3 = go;
                    SecondPosition_3 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "4")
                {
                    RadarogramCounter++;
                    FirstObg_4 = go;
                    FirstPosition_4 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                    //TextLog.text = "Нашел 4";
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "4"+separator+"1")
                {
                    SecondObg_4 = go;
                    SecondPosition_4 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "5")
                {
                    RadarogramCounter++;
                    FirstObg_5 = go;
                    FirstPosition_5 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "5"+separator+"1")
                {
                    SecondObg_5 = go;
                    SecondPosition_5 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "6")
                {
                    RadarogramCounter++;
                    FirstObg_6 = go;
                    FirstPosition_6 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "6"+separator+"1")
                {
                    SecondObg_6 = go;
                    SecondPosition_6 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "7")
                {
                    RadarogramCounter++;
                    FirstObg_7 = go;
                    FirstPosition_7 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "7"+separator+"1")
                {
                    SecondObg_7 = go;
                    SecondPosition_7 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "8")
                {
                    RadarogramCounter++;
                    FirstObg_8 = go;
                    FirstPosition_8 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "8"+separator+"1")
                {
                    SecondObg_8 = go;
                    SecondPosition_8 = go.transform.position;
                }

                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "9")
                {
                    RadarogramCounter++;
                    FirstObg_9 = go;
                    FirstPosition_9 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "9"+separator+"1")
                {
                    SecondObg_9 = go;
                    SecondPosition_9 = go.transform.position;
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "10")
                {
                    RadarogramCounter++;
                    FirstObg_10 = go;
                    FirstPosition_10 = go.transform.position;
                    //go.transform.Find("Canvas").gameObject.SetActive(true);
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "10"+separator+"1")
                {
                    SecondObg_10 = go;
                    SecondPosition_10 = go.transform.position;
                }


            }
            else if (go.name == "BasePlane(Clone)")
            {
                FindPlane = go;
            }
        }
    }

    // установка заготовок под радарограммы, которые потом можно текстурировать
    private void InstallBlank()
    {
        firstInstall = false;
        if(FirstObg_1 != null)
        {
            Vector3 DirectionVector_1 = SecondPosition_1 - FirstPosition_1;
            normalizedDirection_1 = DirectionVector_1.normalized;
            SecondPosition_1 = FirstPosition_1 + (normalizedDirection_1 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_1.transform.position = SecondPosition_1; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_1 = Vector3.Lerp(FirstPosition_1, SecondPosition_1, 0.5f);
            RG_1 = Instantiate(radarogramPrefab, MyCenter_1, Quaternion.identity, FindPlane.transform);
            RG_1.transform.LookAt(SecondPosition_1);
            RG_1.transform.Rotate(0,-90,0);
            RG_1.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_2 != null)
        {
            Vector3 DirectionVector_2 = SecondPosition_2 - FirstPosition_2;
            normalizedDirection_2 = DirectionVector_2.normalized;
            SecondPosition_2 = FirstPosition_2 + (normalizedDirection_2 * 5/* * myJsonRadarogramData.images.jpg1[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_2.transform.position = SecondPosition_2; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_2 = Vector3.Lerp(FirstPosition_2, SecondPosition_2, 0.5f);
            RG_2 = Instantiate(radarogramPrefab, MyCenter_2, Quaternion.identity, FindPlane.transform);
            RG_2.transform.LookAt(SecondPosition_2);
            RG_2.transform.Rotate(0,-90,0);
            RG_2.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_3 != null)
        {
            Vector3 DirectionVector_3 = SecondPosition_3 - FirstPosition_3;
            normalizedDirection_3 = DirectionVector_3.normalized;
            SecondPosition_3 = FirstPosition_3 + (normalizedDirection_3 * 5/* * myJsonRadarogramData.images.jpg2[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_3.transform.position = SecondPosition_3; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_3 = Vector3.Lerp(FirstPosition_3, SecondPosition_3, 0.5f);
            RG_3 = Instantiate(radarogramPrefab, MyCenter_3, Quaternion.identity, FindPlane.transform);
            RG_3.transform.LookAt(SecondPosition_3);
            RG_3.transform.Rotate(0,-90,0);
            RG_3.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_4 != null)
        {
            Vector3 DirectionVector_4 = SecondPosition_4 - FirstPosition_4;
            normalizedDirection_4 = DirectionVector_4.normalized;
            SecondPosition_4 = FirstPosition_4 + (normalizedDirection_4 * 5/* * myJsonRadarogramData.images.jpg3[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_4.transform.position = SecondPosition_4; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_4 = Vector3.Lerp(FirstPosition_4, SecondPosition_4, 0.5f);
            RG_4 = Instantiate(radarogramPrefab, MyCenter_4, Quaternion.identity, FindPlane.transform);
            RG_4.transform.LookAt(SecondPosition_4);
            RG_4.transform.Rotate(0,-90,0);
            RG_4.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_5 != null)
        {
            Vector3 DirectionVector_5 = SecondPosition_5 - FirstPosition_5;
            normalizedDirection_5 = DirectionVector_5.normalized;
            SecondPosition_5 = FirstPosition_5 + (normalizedDirection_5 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_5.transform.position = SecondPosition_5; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_5 = Vector3.Lerp(FirstPosition_5, SecondPosition_5, 0.5f);
            RG_5 = Instantiate(radarogramPrefab, MyCenter_5, Quaternion.identity, FindPlane.transform);
            RG_5.transform.LookAt(SecondPosition_5);
            RG_5.transform.Rotate(0,-90,0);
            RG_5.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_6 != null)
        {
            Vector3 DirectionVector_6 = SecondPosition_6 - FirstPosition_6;
            normalizedDirection_6 = DirectionVector_6.normalized;
            SecondPosition_6 = FirstPosition_6 + (normalizedDirection_6 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_6.transform.position = SecondPosition_6; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_6 = Vector3.Lerp(FirstPosition_6, SecondPosition_6, 0.5f);
            RG_6 = Instantiate(radarogramPrefab, MyCenter_6, Quaternion.identity, FindPlane.transform);
            RG_6.transform.LookAt(SecondPosition_6);
            RG_6.transform.Rotate(0,-90,0);
            RG_6.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_7 != null)
        {
            Vector3 DirectionVector_7 = SecondPosition_7 - FirstPosition_7;
            normalizedDirection_7 = DirectionVector_7.normalized;
            SecondPosition_7 = FirstPosition_7 + (normalizedDirection_7 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_7.transform.position = SecondPosition_7; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_7 = Vector3.Lerp(FirstPosition_7, SecondPosition_7, 0.5f);
            RG_7 = Instantiate(radarogramPrefab, MyCenter_7, Quaternion.identity, FindPlane.transform);
            RG_7.transform.LookAt(SecondPosition_7);
            RG_7.transform.Rotate(0,-90,0);
            RG_7.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_8 != null)
        {
            Vector3 DirectionVector_8 = SecondPosition_8 - FirstPosition_8;
            normalizedDirection_8 = DirectionVector_8.normalized;
            SecondPosition_8 = FirstPosition_8 + (normalizedDirection_8 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_8.transform.position = SecondPosition_8; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_8 = Vector3.Lerp(FirstPosition_8, SecondPosition_8, 0.5f);
            RG_8 = Instantiate(radarogramPrefab, MyCenter_8, Quaternion.identity, FindPlane.transform);
            RG_8.transform.LookAt(SecondPosition_8);
            RG_8.transform.Rotate(0,-90,0);
            RG_8.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_9 != null)
        {
            Vector3 DirectionVector_9 = SecondPosition_9 - FirstPosition_9;
            normalizedDirection_9 = DirectionVector_9.normalized;
            SecondPosition_9 = FirstPosition_9 + (normalizedDirection_9 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_9.transform.position = SecondPosition_9; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_9 = Vector3.Lerp(FirstPosition_9, SecondPosition_9, 0.5f);
            RG_9 = Instantiate(radarogramPrefab, MyCenter_9, Quaternion.identity, FindPlane.transform);
            RG_9.transform.LookAt(SecondPosition_9);
            RG_9.transform.Rotate(0,-90,0);
            RG_9.transform.position += new Vector3(0, 1.5f, 0);
        }

        if(FirstObg_10 != null)
        {
            Vector3 DirectionVector_10 = SecondPosition_10 - FirstPosition_10;
            normalizedDirection_10 = DirectionVector_10.normalized;
            SecondPosition_10 = FirstPosition_10 + (normalizedDirection_10 * 5/* * myJsonRadarogramData.images.jpg0[0]/100*/); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
            SecondObg_10.transform.position = SecondPosition_10; // отодвигаем второй белый флаг, чтобы влезла радарограмма
            MyCenter_10 = Vector3.Lerp(FirstPosition_10, SecondPosition_10, 0.5f);
            RG_10 = Instantiate(radarogramPrefab, MyCenter_10, Quaternion.identity, FindPlane.transform);
            RG_10.transform.LookAt(SecondPosition_10);
            RG_10.transform.Rotate(0,-90,0);
            RG_10.transform.position += new Vector3(0, 1.5f, 0);
        }
    }


    private void InstallRadarogramFromWebServer()
    {
        //TextLog.text = frames.ToString();
        frames++;
        if(frames == 10)
        {
            //frames = 0;
            if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\data.json"))
            {
                    lastTime = myJsonRadarogramData.time;            // запоминаю какое время было в прошлом data.json файле
            }
            try
            {
                webClient.DownloadFileAsync(new Uri(WebServerIP + "/data.json"), RelativePath + "\\RyvenRadarograms" + "\\data.json"); // скачиваю новый data.json
            }
            catch (System.Exception)
            {
                TextLog.text += "Ошибка загрузки data.json ";
            }

            //SummTextLog += " Async ";
            //TextLog.text = SummTextLog;
        }
        if(frames == 20)
        {
            frames = 0;
            //myJsonRadarogramData = GameObject.Find("scripts").GetComponent<ObjectManager>().LoadDataJson(); // Читаю data.json
            if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\data.json"))
            {   
                //SummTextLog += " DC ";
                //TextLog.text = SummTextLog;
                //myJsonRadarogramData = GameObject.Find("scripts").GetComponent<ObjectManager>().LoadDataJson(); // Читаю data.json
                myJsonRadarogramData = GameObject.Find("Main Camera").GetComponent<ObjectManager>().LoadDataJson(); // Читаю data.json
                //SummTextLog += " ReadD ";
                //TextLog.text += "ReadD ";
                //TextLog.text = myJsonRadarogramData.time.ToString();

                if(myJsonRadarogramData.time != lastTime) // Если прилетели новые радарограммы
                {
                    //TextLog.text += "NR ";
                    //SummTextLog += " NR ";
                    //TextLog.text = SummTextLog;
                    
                    try
                    {
                        webClient.DownloadFile(WebServerIP + "/0.png", RelativePath + "\\RyvenRadarograms" + "\\0.png");
                        webClient.DownloadFile(WebServerIP + "/1.png", RelativePath + "\\RyvenRadarograms" + "\\1.png");
                        webClient.DownloadFile(WebServerIP + "/2.png", RelativePath + "\\RyvenRadarograms" + "\\2.png");
                        webClient.DownloadFile(WebServerIP + "/3.png", RelativePath + "\\RyvenRadarograms" + "\\3.png");
                        webClient.DownloadFile(WebServerIP + "/4.png", RelativePath + "\\RyvenRadarograms" + "\\4.png");
                        webClient.DownloadFile(WebServerIP + "/5.png", RelativePath + "\\RyvenRadarograms" + "\\5.png");
                        webClient.DownloadFile(WebServerIP + "/6.png", RelativePath + "\\RyvenRadarograms" + "\\6.png");
                        webClient.DownloadFile(WebServerIP + "/7.png", RelativePath + "\\RyvenRadarograms" + "\\7.png");
                        webClient.DownloadFile(WebServerIP + "/8.png", RelativePath + "\\RyvenRadarograms" + "\\8.png");
                        webClient.DownloadFile(WebServerIP + "/9.png", RelativePath + "\\RyvenRadarograms" + "\\9.png");
                    }
                    catch (System.Exception)
                    {
                        //TextLog.text += "EXC ";
                        //TextLog.text = "Exception thrown " + e.Message;
                        //throw;
                    }
                    if(FirstObg_10 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\9.png") && (myJsonRadarogramData.images.jpg9[0] != 0))
                        {
                            SecondPosition_10 = FirstPosition_10 + (normalizedDirection_10 * myJsonRadarogramData.images.jpg9[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_10.transform.position = SecondPosition_10; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_10 = Vector3.Lerp(FirstPosition_10, SecondPosition_10, 0.5f);
                            RG_10.transform.position = MyCenter_10;
                            RG_10.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\9.png");
                            RG_10.transform.position += new Vector3(0, 1.5f, 0);
                            RG_10.name = "radarogramPrefab_10";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 9.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_9 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\8.png") && (myJsonRadarogramData.images.jpg8[0] != 0))
                        {
                            SecondPosition_9 = FirstPosition_9 + (normalizedDirection_9 * myJsonRadarogramData.images.jpg8[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_9.transform.position = SecondPosition_9; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_9 = Vector3.Lerp(FirstPosition_9, SecondPosition_9, 0.5f);
                            RG_9.transform.position = MyCenter_9;
                            RG_9.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\8.png");
                            RG_9.transform.position += new Vector3(0, 1.5f, 0);
                            RG_9.name = "radarogramPrefab_9";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 8.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_8 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\7.png") && (myJsonRadarogramData.images.jpg7[0] != 0))
                        {
                            SecondPosition_8 = FirstPosition_8 + (normalizedDirection_8 * myJsonRadarogramData.images.jpg7[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_8.transform.position = SecondPosition_8; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_8 = Vector3.Lerp(FirstPosition_8, SecondPosition_8, 0.5f);
                            RG_8.transform.position = MyCenter_8;
                            RG_8.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\7.png");
                            RG_8.transform.position += new Vector3(0, 1.5f, 0);
                            RG_8.name = "radarogramPrefab_8";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 7.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_7 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\6.png") && (myJsonRadarogramData.images.jpg6[0] != 0))
                        {
                            SecondPosition_7 = FirstPosition_7 + (normalizedDirection_7 * myJsonRadarogramData.images.jpg6[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_7.transform.position = SecondPosition_7; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_7 = Vector3.Lerp(FirstPosition_7, SecondPosition_7, 0.5f);
                            RG_7.transform.position = MyCenter_7;
                            RG_7.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\6.png");
                            RG_7.transform.position += new Vector3(0, 1.5f, 0);
                            RG_7.name = "radarogramPrefab_7";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 6.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_6 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\5.png") && (myJsonRadarogramData.images.jpg5[0] != 0))
                        {
                            SecondPosition_6 = FirstPosition_6 + (normalizedDirection_6 * myJsonRadarogramData.images.jpg5[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_6.transform.position = SecondPosition_6; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_6 = Vector3.Lerp(FirstPosition_6, SecondPosition_6, 0.5f);
                            RG_6.transform.position = MyCenter_6;
                            RG_6.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\5.png");
                            RG_6.transform.position += new Vector3(0, 1.5f, 0);
                            RG_6.name = "radarogramPrefab_6";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 5.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_5 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\4.png") && (myJsonRadarogramData.images.jpg4[0] != 0))
                        {
                            SecondPosition_5 = FirstPosition_5 + (normalizedDirection_5 * myJsonRadarogramData.images.jpg4[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_5.transform.position = SecondPosition_5; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_5 = Vector3.Lerp(FirstPosition_5, SecondPosition_5, 0.5f);
                            RG_5.transform.position = MyCenter_5;
                            RG_5.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\4.png");
                            RG_5.transform.position += new Vector3(0, 1.5f, 0);
                            RG_5.name = "radarogramPrefab_5";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 4.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_4 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\3.png") && (myJsonRadarogramData.images.jpg3[0] != 0))
                        {
                            SecondPosition_4 = FirstPosition_4 + (normalizedDirection_4 * myJsonRadarogramData.images.jpg3[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_4.transform.position = SecondPosition_4; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_4 = Vector3.Lerp(FirstPosition_4, SecondPosition_4, 0.5f);
                            RG_4.transform.position = MyCenter_4;
                            RG_4.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\3.png");
                            RG_4.transform.position += new Vector3(0, 1.5f, 0);
                            RG_4.name = "radarogramPrefab_4";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 3.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_3 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\2.png") && (myJsonRadarogramData.images.jpg2[0] != 0))
                        {
                            SecondPosition_3 = FirstPosition_3 + (normalizedDirection_3 * myJsonRadarogramData.images.jpg2[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_3.transform.position = SecondPosition_3; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_3 = Vector3.Lerp(FirstPosition_3, SecondPosition_3, 0.5f);
                            RG_3.transform.position = MyCenter_3;
                            RG_3.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\2.png");
                            RG_3.transform.position += new Vector3(0, 1.5f, 0);
                            RG_3.name = "radarogramPrefab_3";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 2.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_2 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\1.png") && (myJsonRadarogramData.images.jpg1[0] != 0))
                        {
                            SecondPosition_2 = FirstPosition_2 + (normalizedDirection_2 * myJsonRadarogramData.images.jpg1[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_2.transform.position = SecondPosition_2; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_2 = Vector3.Lerp(FirstPosition_2, SecondPosition_2, 0.5f);
                            RG_2.transform.position = MyCenter_2;
                            RG_2.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\1.png");
                            RG_2.transform.position += new Vector3(0, 1.5f, 0);
                            RG_2.name = "radarogramPrefab_2";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 1.jpg (или не описан в data.json)";
                        }
                    }

                    if(FirstObg_1 != null)
                    {
                        if(File.Exists(RelativePath + "\\RyvenRadarograms" + "\\0.png") && (myJsonRadarogramData.images.jpg0[0] != 0))
                        {
                            SecondPosition_1 = FirstPosition_1 + (normalizedDirection_1 * myJsonRadarogramData.images.jpg0[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                            SecondObg_1.transform.position = SecondPosition_1; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                            MyCenter_1 = Vector3.Lerp(FirstPosition_1, SecondPosition_1, 0.5f);
                            RG_1.transform.position = MyCenter_1;
                            RG_1.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\RyvenRadarograms" + "\\0.png");
                            RG_1.transform.position += new Vector3(0, 1.5f, 0);
                            RG_1.name = "radarogramPrefab_1";
                        }
                        else
                        {
                            //TextLog.text = "Не найден файл 0.jpg (или не описан в data.json)";
                        }
                    }
                }
            }
        }
    }


    private void InstallRadarogramFromFile()
    {
        if(File.Exists(RelativePath + "\\Radarograms" + "\\data.json"))
        {
            myJsonRadarogramData = GameObject.Find("Main Camera").GetComponent<ObjectManager>().LoadDataJson(); // Читаю data.json

            if(FirstObg_10 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\9.png") && (myJsonRadarogramData.images.jpg9[0] != 0))
                {
                    SecondPosition_10 = FirstPosition_10 + (normalizedDirection_10 * myJsonRadarogramData.images.jpg9[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_10.transform.position = SecondPosition_10; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_10 = Vector3.Lerp(FirstPosition_10, SecondPosition_10, 0.5f);
                    RG_10.transform.position = MyCenter_10;
                    RG_10.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\9.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_10.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    // }
                    // else
                    // {
                    //     RG_10.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    // }

                    //RG_10.transform.Find("Number").GetComponent<TextMesh>().text = RG_10.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_10.transform.position += new Vector3(0, 1.5f, 0);
                    RG_10.name = "radarogramPrefab_10";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\9.jpg") && (myJsonRadarogramData.images.jpg9[0] != 0))
                {
                    SecondPosition_10 = FirstPosition_10 + (normalizedDirection_10 * myJsonRadarogramData.images.jpg9[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_10.transform.position = SecondPosition_10; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_10 = Vector3.Lerp(FirstPosition_10, SecondPosition_10, 0.5f);
                    RG_10.transform.position = MyCenter_10;
                    RG_10.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\9.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_10.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    // }
                    // else
                    // {
                    //     RG_10.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    // }

                    //RG_10.transform.Find("Number").GetComponent<TextMesh>().text = RG_10.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_10.transform.position += new Vector3(0, 1.5f, 0);
                    RG_10.name = "radarogramPrefab_10";
                }
                else
                {
                    //TextLog.text = "Не найден файл 9.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_9 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\8.png") && (myJsonRadarogramData.images.jpg8[0] != 0))
                {
                    SecondPosition_9 = FirstPosition_9 + (normalizedDirection_9 * myJsonRadarogramData.images.jpg8[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_9.transform.position = SecondPosition_9; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_9 = Vector3.Lerp(FirstPosition_9, SecondPosition_9, 0.5f);
                    RG_9.transform.position = MyCenter_9;
                    RG_9.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\8.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_9.GetComponent<SpriteRenderer>().sortingOrder = 9;
                    // }
                    // else
                    // {
                    //     RG_9.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    // }

                    //RG_9.transform.Find("Number").GetComponent<TextMesh>().text = RG_9.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_9.transform.position += new Vector3(0, 1.5f, 0);
                    RG_9.name = "radarogramPrefab_9";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\8.jpg") && (myJsonRadarogramData.images.jpg8[0] != 0))
                {
                    SecondPosition_9 = FirstPosition_9 + (normalizedDirection_9 * myJsonRadarogramData.images.jpg8[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_9.transform.position = SecondPosition_9; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_9 = Vector3.Lerp(FirstPosition_9, SecondPosition_9, 0.5f);
                    RG_9.transform.position = MyCenter_9;
                    RG_9.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\8.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_9.GetComponent<SpriteRenderer>().sortingOrder = 9;
                    // }
                    // else
                    // {
                    //     RG_9.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    // }

                    //RG_9.transform.Find("Number").GetComponent<TextMesh>().text = RG_9.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_9.transform.position += new Vector3(0, 1.5f, 0);
                    RG_9.name = "radarogramPrefab_9";
                }
                else
                {
                    //TextLog.text = "Не найден файл 8.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_8 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\7.png") && (myJsonRadarogramData.images.jpg7[0] != 0))
                {
                    SecondPosition_8 = FirstPosition_8 + (normalizedDirection_8 * myJsonRadarogramData.images.jpg7[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_8.transform.position = SecondPosition_8; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_8 = Vector3.Lerp(FirstPosition_8, SecondPosition_8, 0.5f);
                    RG_8.transform.position = MyCenter_8;
                    RG_8.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\7.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_8.GetComponent<SpriteRenderer>().sortingOrder = 8;
                    // }
                    // else
                    // {
                    //     RG_8.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    // }

                    //RG_8.transform.Find("Number").GetComponent<TextMesh>().text = RG_8.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_8.transform.position += new Vector3(0, 1.5f, 0);
                    RG_8.name = "radarogramPrefab_8";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\7.jpg") && (myJsonRadarogramData.images.jpg7[0] != 0))
                {
                    SecondPosition_8 = FirstPosition_8 + (normalizedDirection_8 * myJsonRadarogramData.images.jpg7[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_8.transform.position = SecondPosition_8; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_8 = Vector3.Lerp(FirstPosition_8, SecondPosition_8, 0.5f);
                    RG_8.transform.position = MyCenter_8;
                    RG_8.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\7.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_8.GetComponent<SpriteRenderer>().sortingOrder = 8;
                    // }
                    // else
                    // {
                    //     RG_8.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    // }

                    //RG_8.transform.Find("Number").GetComponent<TextMesh>().text = RG_8.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_8.transform.position += new Vector3(0, 1.5f, 0);
                    RG_8.name = "radarogramPrefab_8";
                }
                else
                {
                    //TextLog.text = "Не найден файл 7.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_7 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\6.png") && (myJsonRadarogramData.images.jpg6[0] != 0))
                {
                    SecondPosition_7 = FirstPosition_7 + (normalizedDirection_7 * myJsonRadarogramData.images.jpg6[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_7.transform.position = SecondPosition_7; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_7 = Vector3.Lerp(FirstPosition_7, SecondPosition_7, 0.5f);
                    RG_7.transform.position = MyCenter_7;
                    RG_7.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\6.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_7.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    // }
                    // else
                    // {
                    //     RG_7.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    // }

                    //RG_7.transform.Find("Number").GetComponent<TextMesh>().text = RG_7.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_7.transform.position += new Vector3(0, 1.5f, 0);
                    RG_7.name = "radarogramPrefab_7";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\6.jpg") && (myJsonRadarogramData.images.jpg6[0] != 0))
                {
                    SecondPosition_7 = FirstPosition_7 + (normalizedDirection_7 * myJsonRadarogramData.images.jpg6[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_7.transform.position = SecondPosition_7; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_7 = Vector3.Lerp(FirstPosition_7, SecondPosition_7, 0.5f);
                    RG_7.transform.position = MyCenter_7;
                    RG_7.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\6.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_7.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    // }
                    // else
                    // {
                    //     RG_7.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    // }

                    //RG_7.transform.Find("Number").GetComponent<TextMesh>().text = RG_7.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_7.transform.position += new Vector3(0, 1.5f, 0);
                    RG_7.name = "radarogramPrefab_7";
                }
                else
                {
                    //TextLog.text = "Не найден файл 6.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_6 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\5.png") && (myJsonRadarogramData.images.jpg5[0] != 0))
                {
                    SecondPosition_6 = FirstPosition_6 + (normalizedDirection_6 * myJsonRadarogramData.images.jpg5[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_6.transform.position = SecondPosition_6; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_6 = Vector3.Lerp(FirstPosition_6, SecondPosition_6, 0.5f);
                    RG_6.transform.position = MyCenter_6;
                    RG_6.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\5.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_6.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    // }
                    // else
                    // {
                    //     RG_6.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    // }

                    //RG_6.transform.Find("Number").GetComponent<TextMesh>().text = RG_6.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_6.transform.position += new Vector3(0, 1.5f, 0);
                    RG_6.name = "radarogramPrefab_6";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\5.jpg") && (myJsonRadarogramData.images.jpg5[0] != 0))
                {
                    SecondPosition_6 = FirstPosition_6 + (normalizedDirection_6 * myJsonRadarogramData.images.jpg5[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_6.transform.position = SecondPosition_6; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_6 = Vector3.Lerp(FirstPosition_6, SecondPosition_6, 0.5f);
                    RG_6.transform.position = MyCenter_6;
                    RG_6.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\5.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_6.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    // }
                    // else
                    // {
                    //     RG_6.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    // }

                    //RG_6.transform.Find("Number").GetComponent<TextMesh>().text = RG_6.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_6.transform.position += new Vector3(0, 1.5f, 0);
                    RG_6.name = "radarogramPrefab_6";
                }
                else
                {
                    //TextLog.text = "Не найден файл 5.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_5 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\4.png") && (myJsonRadarogramData.images.jpg4[0] != 0))
                {
                    SecondPosition_5 = FirstPosition_5 + (normalizedDirection_5 * myJsonRadarogramData.images.jpg4[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_5.transform.position = SecondPosition_5; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_5 = Vector3.Lerp(FirstPosition_5, SecondPosition_5, 0.5f);
                    RG_5.transform.position = MyCenter_5;
                    RG_5.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\4.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_5.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    // }
                    // else
                    // {
                    //     RG_5.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    // }

                    //RG_5.transform.Find("Number").GetComponent<TextMesh>().text = RG_5.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_5.transform.position += new Vector3(0, 1.5f, 0);
                    RG_5.name = "radarogramPrefab_5";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\4.jpg") && (myJsonRadarogramData.images.jpg4[0] != 0))
                {
                    SecondPosition_5 = FirstPosition_5 + (normalizedDirection_5 * myJsonRadarogramData.images.jpg4[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_5.transform.position = SecondPosition_5; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_5 = Vector3.Lerp(FirstPosition_5, SecondPosition_5, 0.5f);
                    RG_5.transform.position = MyCenter_5;
                    RG_5.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\4.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_5.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    // }
                    // else
                    // {
                    //     RG_5.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    // }

                    //RG_5.transform.Find("Number").GetComponent<TextMesh>().text = RG_5.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_5.transform.position += new Vector3(0, 1.5f, 0);
                    RG_5.name = "radarogramPrefab_5";
                }
                else
                {
                    //TextLog.text = "Не найден файл 4.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_4 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\3.png") && (myJsonRadarogramData.images.jpg3[0] != 0))
                {
                    SecondPosition_4 = FirstPosition_4 + (normalizedDirection_4 * myJsonRadarogramData.images.jpg3[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_4.transform.position = SecondPosition_4; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_4 = Vector3.Lerp(FirstPosition_4, SecondPosition_4, 0.5f);
                    RG_4.transform.position = MyCenter_4;
                    RG_4.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\3.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_4.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    // }
                    // else
                    // {
                    //     RG_4.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    // }

                    //RG_4.transform.Find("Number").GetComponent<TextMesh>().text = RG_4.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_4.transform.position += new Vector3(0, 1.5f, 0);
                    RG_4.name = "radarogramPrefab_4";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\3.jpg") && (myJsonRadarogramData.images.jpg3[0] != 0))
                {
                    SecondPosition_4 = FirstPosition_4 + (normalizedDirection_4 * myJsonRadarogramData.images.jpg3[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_4.transform.position = SecondPosition_4; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_4 = Vector3.Lerp(FirstPosition_4, SecondPosition_4, 0.5f);
                    RG_4.transform.position = MyCenter_4;
                    RG_4.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\3.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_4.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    // }
                    // else
                    // {
                    //     RG_4.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    // }

                    //RG_4.transform.Find("Number").GetComponent<TextMesh>().text = RG_4.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_4.transform.position += new Vector3(0, 1.5f, 0);
                    RG_4.name = "radarogramPrefab_4";
                }
                else
                {
                    //TextLog.text = "Не найден файл 3.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_3 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\2.png") && (myJsonRadarogramData.images.jpg2[0] != 0))
                {
                    SecondPosition_3 = FirstPosition_3 + (normalizedDirection_3 * myJsonRadarogramData.images.jpg2[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_3.transform.position = SecondPosition_3; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_3 = Vector3.Lerp(FirstPosition_3, SecondPosition_3, 0.5f);
                    RG_3.transform.position = MyCenter_3;
                    RG_3.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\2.png");
                    
                    // if(WhichSide == false)
                    // {
                    //     RG_3.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    // }
                    // else
                    // {
                    //     RG_3.GetComponent<SpriteRenderer>().sortingOrder = 8;
                    // }

                    //RG_3.transform.Find("Number").GetComponent<TextMesh>().text = RG_3.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_3.transform.position += new Vector3(0, 1.5f, 0);
                    RG_3.name = "radarogramPrefab_3";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\2.jpg") && (myJsonRadarogramData.images.jpg2[0] != 0))
                {
                    SecondPosition_3 = FirstPosition_3 + (normalizedDirection_3 * myJsonRadarogramData.images.jpg2[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_3.transform.position = SecondPosition_3; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_3 = Vector3.Lerp(FirstPosition_3, SecondPosition_3, 0.5f);
                    RG_3.transform.position = MyCenter_3;
                    RG_3.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\2.jpg");
                    
                    // if(WhichSide == false)
                    // {
                    //     RG_3.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    // }
                    // else
                    // {
                    //     RG_3.GetComponent<SpriteRenderer>().sortingOrder = 8;
                    // }

                    //RG_3.transform.Find("Number").GetComponent<TextMesh>().text = RG_3.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_3.transform.position += new Vector3(0, 1.5f, 0);
                    RG_3.name = "radarogramPrefab_3";
                }
                else
                {
                    //TextLog.text = "Не найден файл 2.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_2 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\1.png") && (myJsonRadarogramData.images.jpg1[0] != 0))
                {
                    SecondPosition_2 = FirstPosition_2 + (normalizedDirection_2 * myJsonRadarogramData.images.jpg1[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_2.transform.position = SecondPosition_2; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_2 = Vector3.Lerp(FirstPosition_2, SecondPosition_2, 0.5f);
                    RG_2.transform.position = MyCenter_2;
                    RG_2.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\1.png");
                    
                    // if(WhichSide == false)
                    // {
                    //     RG_2.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    // }
                    // else
                    // {
                    //     RG_2.GetComponent<SpriteRenderer>().sortingOrder = 9;
                    // }

                    //RG_2.transform.Find("Number").GetComponent<TextMesh>().text = RG_2.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_2.transform.position += new Vector3(0, 1.5f, 0);
                    RG_2.name = "radarogramPrefab_2";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\1.jpg") && (myJsonRadarogramData.images.jpg1[0] != 0))
                {
                    SecondPosition_2 = FirstPosition_2 + (normalizedDirection_2 * myJsonRadarogramData.images.jpg1[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_2.transform.position = SecondPosition_2; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_2 = Vector3.Lerp(FirstPosition_2, SecondPosition_2, 0.5f);
                    RG_2.transform.position = MyCenter_2;
                    RG_2.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\1.jpg");
                    
                    // if(WhichSide == false)
                    // {
                    //     RG_2.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    // }
                    // else
                    // {
                    //     RG_2.GetComponent<SpriteRenderer>().sortingOrder = 9;
                    // }

                    //RG_2.transform.Find("Number").GetComponent<TextMesh>().text = RG_2.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_2.transform.position += new Vector3(0, 1.5f, 0);
                    RG_2.name = "radarogramPrefab_2";
                }
                else
                {
                    //TextLog.text = "Не найден файл 1.jpg (или не описан в data.json)";
                }
            }

            if(FirstObg_1 != null)
            {
                if(File.Exists(RelativePath + "\\Radarograms" + "\\0.png") && (myJsonRadarogramData.images.jpg0[0] != 0))
                {
                    SecondPosition_1 = FirstPosition_1 + (normalizedDirection_1 * myJsonRadarogramData.images.jpg0[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_1.transform.position = SecondPosition_1; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_1 = Vector3.Lerp(FirstPosition_1, SecondPosition_1, 0.5f);
                    RG_1.transform.position = MyCenter_1;
                    RG_1.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\0.png");

                    // if(WhichSide == false)
                    // {
                    //     RG_1.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    // }
                    // else
                    // {
                    //     RG_1.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    // }

                    //RG_1.transform.Find("Number").GetComponent<TextMesh>().text = RG_1.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_1.transform.position += new Vector3(0, 1.5f, 0);
                    RG_1.name = "radarogramPrefab_1";
                }
                else if(File.Exists(RelativePath + "\\Radarograms" + "\\0.jpg") && (myJsonRadarogramData.images.jpg0[0] != 0))
                {
                    SecondPosition_1 = FirstPosition_1 + (normalizedDirection_1 * myJsonRadarogramData.images.jpg0[0]/100); // умножается на количество метров (длина радарограммы). Делю на 100, потому что сейчас 100 пикселей на метр
                    SecondObg_1.transform.position = SecondPosition_1; // отодвигаем второй белый флаг, чтобы влезла радарограмма
                    MyCenter_1 = Vector3.Lerp(FirstPosition_1, SecondPosition_1, 0.5f);
                    RG_1.transform.position = MyCenter_1;
                    RG_1.GetComponent<SpriteRenderer>().sprite = LoadSprite(RelativePath + "\\Radarograms" + "\\0.jpg");

                    // if(WhichSide == false)
                    // {
                    //     RG_1.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    // }
                    // else
                    // {
                    //     RG_1.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    // }

                    //RG_1.transform.Find("Number").GetComponent<TextMesh>().text = RG_1.GetComponent<SpriteRenderer>().sortingOrder.ToString();
                    RG_1.transform.position += new Vector3(0, 1.5f, 0);
                    RG_1.name = "radarogramPrefab_1";
                }
                else
                {
                    //TextLog.text = "Не найден файл 0.jpg (или не описан в data.json)";
                }
            }
        }
        else
        {
            TextLog.text = "Не найден файл data.json";
        }
    }

    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    public void Intersection()
    {
        // Ищу флаги границ красной линии
        GameObject[] allGo = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allGo)
        {
            if (go.name == "Flag_red(Clone)")
            {
                if (go.transform.Find("Number").GetComponent<TextMesh>().text == "0,1")
                {
                    FirstRedFlag_1 = go;
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "0,2")
                {
                    SecondRedFlag_1 = go;
                }
                else if (go.transform.Find("Number").GetComponent<TextMesh>().text == "0,3")
                {
                    FirstRedFlag_2 = go;
                }
                else if(go.transform.Find("Number").GetComponent<TextMesh>().text == "0,4")
                {
                    SecondRedFlag_2 = go;
                }
            }

        }

        // если белая линия найдена
        if(FirstObg_1 != null)
        {
            // Белая линия (флаги 1 и 1.1) 
            x1 = FirstObg_1.transform.position.x;//-1.717579f;
            y1 = FirstObg_1.transform.position.z;//0.07475325f;
            x2 = SecondObg_1.transform.position.x;//-0.8017522f;
            y2 = SecondObg_1.transform.position.z;//15.05572f;
        }

        // если первая красная линия найдена
        if(FirstRedFlag_1 != null)
        {
            // Первая красная линия (флаги 0.1 и 0.2)
            x3 = FirstRedFlag_1.transform.position.x;//4.241f;
            y3 = FirstRedFlag_1.transform.position.z;//5.299f;
            x4 = SecondRedFlag_1.transform.position.x;//-13.841f;
            y4 = SecondRedFlag_1.transform.position.z;//5.868f;
        }

        // если вторая красная линия найдена
        if(FirstRedFlag_2 != null)
        {
            // Вторая красная линия (флаги 0.3 и 0.4)
            x5 = FirstRedFlag_2.transform.position.x;//4.241f;
            y5 = FirstRedFlag_2.transform.position.z;//5.299f;
            x6 = SecondRedFlag_2.transform.position.x;//-13.841f;
            y6 = SecondRedFlag_2.transform.position.z;//5.868f;
        }

        // ЧЕРЕЗ ОПРЕДЕЛИТЕЛИ С ВИКИПЕДИИ (результат неверный)
        //float Px = (x1*y2-y1*x2)*(x3-x4)-(x1-x2)*(x3*y4-y3*x4)/(x1-x2)*(y3-y4)-(y1-y2)*(x3-x4);
        //float Py = (x1*y2-y1*x2)*(y3-y4)-(y1-y2)*(x3*y4-y3*x4)/(x1-x2)*(y3-y4)-(y1-y2)*(x3-x4);

        //Debug.Log("Px: " + Px); // получилось -139,7272
        //Debug.Log("Py: " + Py); // получилось -1199,851

        // ЧЕРЕЗ УРАВНЕНИЯ ПРЯМЫХ
        if((x3 != 0) && (x4 != 0))
        {
            // Первая точка пересечения
            float A1 = (y1 - y2) / (x1 - x2);
            float A2 = (y3 - y4) / (x3 - x4);
            float b1 = y1- A1 * x1;
            float b2 = y3 - A2 * x3;

            float X1 = (b2 - b1) / (A1 - A2); // получилось -1,38737940234523
            float Y1 = A2 * X1 + b2;           // получилось 5,47611235097617

            Vector3 point1 = new Vector3(X1,0,Y1);
            var PointIntersection1 = Instantiate(IntersectionObj, point1, IntersectionObj.transform.rotation); // ставим в определенное заранее место наш объект + делаем его потомком плоскости
            
            // ДЛЯ ИЗМЕРЕНИЯ РАССТОЯНИЯ МЕЖДУ ОБЪЕКТАМИ
            if ((FirstObg_1 != null) && (PointIntersection1 != null))
            {
                Vector3 FirstPosition = FirstObg_1.transform.position;
                Vector3 SecondPosition = PointIntersection1.transform.position;
                Vector3 DirectionVector = SecondPosition - FirstPosition;
                float MyMagnitude = DirectionVector.magnitude;
                //Debug.Log("DirectionVector: " + DirectionVector.ToString());
                //Debug.Log("MyMagnitude: " + MyMagnitude.ToString());
                Obj1Distance.text = Math.Round(MyMagnitude, 1).ToString() + " м";
            }
        }

        if((x5 != 0) && (x6 != 0))
        {
            // Вторая точка пересечения
            float A1 = (y1 - y2) / (x1 - x2);
            float A2 = (y5 - y6) / (x5 - x6);
            float b1 = y1- A1 * x1;
            float b2 = y5 - A2 * x5;

            float X2 = (b2 - b1) / (A1 - A2);
            float Y2 = A2 * X2 + b2;

            Vector3 point2 = new Vector3(X2,0,Y2);
            var PointIntersection2 = Instantiate(IntersectionObj, point2, IntersectionObj.transform.rotation); // ставим в определенное заранее место наш объект + делаем его потомком плоскости
        
            // ДЛЯ ИЗМЕРЕНИЯ РАССТОЯНИЯ МЕЖДУ ОБЪЕКТАМИ
            if ((FirstObg_1 != null) && (PointIntersection2 != null))
            {
                Vector3 FirstPosition = FirstObg_1.transform.position;
                Vector3 SecondPosition = PointIntersection2.transform.position;
                Vector3 DirectionVector = SecondPosition - FirstPosition;
                float MyMagnitude = DirectionVector.magnitude;
                //Debug.Log("DirectionVector: " + DirectionVector.ToString());
                //Debug.Log("MyMagnitude: " + MyMagnitude.ToString());
                //Math.Round(MyMagnitude, 1);
                Obj2Distance.text = Math.Round(MyMagnitude, 1).ToString() + " м";
            }

        }
    }

    public void RadarogramDownloadButton()
    {
        if(GameObject.Find("Dropdown").GetComponent<DropDownMenu>().selectItem.text != "") // если что-то ввели
        {
            WebServerIP = "http://"+GameObject.Find("Dropdown").GetComponent<DropDownMenu>().selectItem.text+":8000";
        }
        else // если поле осталось пустое - IP по умолчанию
        {
            //WebServerIP = "http://192.168.110.33:8000";
        }
        GameObject.Find("SettingsButton").GetComponent<TestButton>().testClickBut(); // закрываю окно settings
        InitWeb();
        RadarogramPath = "\\RyvenRadarograms\\data.json";
        if (firstInstall == true)
        {
            FindProfile();
            if (SceneExists == true)
            {
                InstallBlank();
            }
            else
            {
                TextLog.text = "Зачем ты пытаешься загрузить радарограммы, если нет сцены? =(";
            }
        }
        if (SceneExists == true)
        {
            StartCoroutine("LogCoroutine");
            FlagRyvenServerConnect = true;
        }
        else
        {
            TextLog.text = "Зачем ты пытаешься загрузить радарограммы, если нет сцены? =(";
        }
    }

    IEnumerator LogCoroutine()
    {
        TextLog.text = "Подключаюсь по IP: " + WebServerIP.ToString() + ".";
        yield return new WaitForSeconds(0.8f);
        TextLog.text = "Подключаюсь по IP: " + WebServerIP.ToString() + "..";
        yield return new WaitForSeconds(0.8f);
        TextLog.text = "Подключаюсь по IP: " + WebServerIP.ToString() + "...";
        yield return new WaitForSeconds(0.8f);
        TextLog.text = "Подключаюсь по IP: " + WebServerIP.ToString() + "....";
        yield return new WaitForSeconds(0.8f);
        TextLog.text = "Подключаюсь по IP: " + WebServerIP.ToString() + ".....";
        yield return new WaitForSeconds(0.8f);
        TextLog.text = " ";
    }

    public void DownloadScene()
    {
        // удаляем старый файл, если такой есть
        if(File.Exists(RelativePath + "\\RyvenScene" + "\\scene.json"))
        {
            File.Delete(RelativePath + "\\RyvenScene" + "\\scene.json");
        }
        
        if(GameObject.Find("Dropdown").GetComponent<DropDownMenu>().selectItem.text != "") // если что-то ввели
        {
            WebServerIP = "http://"+GameObject.Find("Dropdown").GetComponent<DropDownMenu>().selectItem.text+":8000";
        }
        else // если поле осталось пустое - IP по умолчанию
        {
            //WebServerIP = "http://192.168.110.33:8000";
        }
        GameObject.Find("SettingsButton").GetComponent<TestButton>().testClickBut(); // закрываю окно settings
        StartCoroutine("LogCoroutine");
        Directory.CreateDirectory(RelativePath + "\\RyvenScene");
        try // попытка подключиться и загрузить сцену
        {
            webClient.DownloadFileAsync(new Uri(WebServerIP + "/scene.json"), RelativePath + "\\RyvenScene" + "\\scene.json"); // скачиваю новый scene.json
        }
        catch(Exception e)
        {
            TextLog.text = e.Message;
        }
        StartCoroutine("SceneInstall"); // эта корутина для того, чтобы сцена устанавливалась через 1 секунду после начала загрузки
    }

    IEnumerator SceneInstall()
    {
        yield return new WaitForSeconds(1.0f);
        try
        {
            GameObject.Find("Main Camera").GetComponent<Saver>().arguments[1] = RelativePath + "\\RyvenScene" + "\\scene.json";
        }
        catch(Exception e)
        {
            TextLog.text = e.Message;
        }
        LoadScene();
    }

    public void LoadScene()
    {
        //DeactivateInputField();
        //GameObject.Find("InputFieldJSON").transform.DeactivateInputField();
        //mainInputField.DeactivateInputField();
        GameObject.Find("Main Camera").GetComponent<ObjectManager>().LoadButton(); // Загружаю сцену из файла
    }

}