using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Класс для сохранения и загрузки.
/// </summary>
public class Saver : MonoBehaviour
{
    [SerializeField] private Text TextLog; // лог на канвас
    public string[] arguments;
    private string[] GetCommandLineArgs;
    void Start()
    {
        //GameObject.Find("Main Camera").GetComponent<ObjectManager>().LoadButton(); // ПРИНУДИТЕЛЬНО ЗАГРУЖАЕТ СЦЕНУ (НЕ ИСПОЛЬЗУЕТСЯ)
        arguments = new string[2];
        try
        {
            GetCommandLineArgs = Environment.GetCommandLineArgs();
            arguments[1] = GetCommandLineArgs[1];
            if(arguments != null)
            {
                GameObject.Find("Main Camera").GetComponent<ObjectManager>().LoadButton(); // Загружаю сцену из файла
            }
        }
        catch (Exception e)
        {
            // print("Exception thrown " + e.Message);
            TextLog.text = "Exception thrown :" + e.Message;
        }
    }
    //string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
    string RelativePath = Directory.GetCurrentDirectory();
    //Debug.Log(path);

    /// <summary>
    /// Относительный путь к файлу загрузки данных.
    /// </summary>
    //[SerializeField]
    private string _cubeDataJsonPath;
    
    /// <summary>
    /// Полный путь к файлу загрузки данных.
    /// </summary>
    private string _saveDataPath;

    /// <summary>
    /// Полный путь к файлу загрузки данных.
    /// </summary>
    public string SaveDataPath {
        get
        {
            #if UNITY_EDITOR
                //_cubeDataJsonPath = GameObject.Find("InputFieldJSON").transform.Find("MyText").GetComponent<Text>().text;

                //_saveDataPath = arguments[1];

                //_saveDataPath = "C:\\Users\\DilBert\\Downloads\\Telegram Desktop\\scenes22july\\scene.json";

                // if (_saveDataPath == null)
                // {
                //     //_saveDataPath = Path.Combine(Application.persistentDataPath, _cubeDataJsonPath);
                //     //_saveDataPath = Path.Combine("\\ARScenes", _cubeDataJsonPath);
                //     _saveDataPath = Path.Combine(RelativePath + "\\Scenes", _cubeDataJsonPath);
                //     //Debug.Log(_saveDataPath);
                // }

                _saveDataPath =  "C:\\Users\\DilBert\\Desktop\\GEOtest\\parking.json"; // ДЛЯ СОХРАНЕНИЯ РАССТАВЛЕННОЙ ВРУЧНУЮ СЦЕНЫ В ФАЙЛ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return _saveDataPath;
            #endif
            #if UNITY_STANDALONE_WIN
                _saveDataPath = arguments[1];
                return _saveDataPath;
            #endif
        }
    }

    private string FULLteststring;
    
    public string newSaveDataPath
    {
        get
        {
            string pathstring = GameObject.Find("Main Camera").GetComponent<ARSceneMakingManager>().RadarogramPath;
            if (FULLteststring == null)
            {
                //FULLteststring = Path.Combine(RelativePath + "\\RyvenRadarograms", teststring);
                FULLteststring = RelativePath + pathstring;
            }

            return FULLteststring;
        }
    }

    /// <summary>
    /// Загрузить данные объектов
    /// </summary>
    public CommonSaveData Load()
    {
        if(File.Exists(SaveDataPath))
        {
            //TextLog.text = "Файл найден, загружаю сцену...";
            using (FileStream fileStream = File.Open(SaveDataPath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                CommonSaveData loadData = JsonUtility.FromJson<CommonSaveData>(reader.ReadToEnd());

                return loadData;
            }
        }
        return null;

    }




    public JsonRadarogramReader.CommonData LoadJsonRadarogram()
    {
        if(File.Exists(newSaveDataPath))
        {
            using (FileStream fileStream = File.Open(newSaveDataPath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                JsonRadarogramReader.CommonData loadRadData = JsonUtility.FromJson<JsonRadarogramReader.CommonData>(reader.ReadToEnd());

                return loadRadData;
            }
        }
        return null;

    }

    public void TestSaveJson(JsonRadarogramReader.CommonData MySaveData)
    {
        using (FileStream fileStream = File.Open(newSaveDataPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fileStream.SetLength(0);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonUtility.ToJson(MySaveData, true)); // тут можно поставить true вторым аргуметом, чтобы был красивый вывод JSON с отступами
            }
        }
    }
    



    /// <summary>
    /// Сохранить данные объектов
    /// </summary>
    public void Save(CommonSaveData saveData)
    {
        if(File.Exists(SaveDataPath))
        {
            string testtime = System.DateTime.Now.ToString();
            string testname = Path.Combine(Application.persistentDataPath, _cubeDataJsonPath + testtime);
            File.Move(SaveDataPath, testname);
        }
        //TextLog.text = "Сохраняю данные в файл...";
        using (FileStream fileStream = File.Open(SaveDataPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fileStream.SetLength(0);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonUtility.ToJson(saveData)); // тут можно поставить true вторым аргуметом, чтобы был красивый вывод JSON с отступами
            }
        }
    }

    /// <summary>
    /// Оправить данные сцены по сокету.
    /// </summary>
    public void SendSaveJSON()
    {
        if(File.Exists(SaveDataPath))
        {
            //TextLog.text = "Файл найден. Отправляю по сокету...";
            using (FileStream fileStream = File.Open(SaveDataPath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                CommonSaveData loadData = JsonUtility.FromJson<CommonSaveData>(reader.ReadToEnd());
                
                UdpClient client = new UdpClient(5600);
                try
                {
                    //TextLog.text = DataHolder.TextIP;
                    // client.Connect("192.168.43.217", 8080);
                    client.Connect(DataHolder.TextIP, 8080);
                    byte[] sendBytes = Encoding.ASCII.GetBytes(JsonUtility.ToJson(loadData));
                    client.Send(sendBytes, sendBytes.Length);
                    //TextLog.text = "Отправил данные на IP: " + DataHolder.TextIP;
                    Debug.Log("Отправил данные на IP: " + DataHolder.TextIP);
                }
                catch(Exception e)
                {
                    print("Exception thrown " + e.Message);
                    //TextLog.text = "Exception thrown " + e.Message;
                }

            }
        }

    }
}