using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Класс, реализующий управление видимостью радарограмм при помощи клавиатуры
public class KeyboardController : MonoBehaviour
{
    // массив самих объектов - радарограмм
    private GameObject[] RGArray = new GameObject[11]; // используется с 1 до 10
    // массив флагов активности радарограмм
    private bool[] RGActiveArray = new bool[11]; // используется с 1 до 10

    // флаг, указывающий на то, что радарограммы были найдены
    private bool RadarogramWereFound = false;

    [SerializeField] private Text TextLog;

    void Start()
    {
        for(int i = 1; i < 11; i++)
        {
            RGActiveArray[i] = true;
        }
    }

    void Update()
    {
        if (GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey) // для того, чтобы не нажимались кнопки, когда открыто меню настроек
        {
            // скрытие/отображение радарограмм по номерам (цифровая клавиатура)
            if (Input.GetKeyDown(KeyCode.Keypad1)||Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[1] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[1] = !RGActiveArray[1];
                    RGArray[1].SetActive(RGActiveArray[1]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2)||Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[2] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[2] = !RGActiveArray[2];
                    RGArray[2].SetActive(RGActiveArray[2]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad3)||Input.GetKeyDown(KeyCode.Alpha3))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[3] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[3] = !RGActiveArray[3];
                    RGArray[3].SetActive(RGActiveArray[3]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad4)||Input.GetKeyDown(KeyCode.Alpha4))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[4] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[4] = !RGActiveArray[4];
                    RGArray[4].SetActive(RGActiveArray[4]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad5)||Input.GetKeyDown(KeyCode.Alpha5))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[5] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[5] = !RGActiveArray[5];
                    RGArray[5].SetActive(RGActiveArray[5]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad6)||Input.GetKeyDown(KeyCode.Alpha6))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[6] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[6] = !RGActiveArray[6];
                    RGArray[6].SetActive(RGActiveArray[6]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad7)||Input.GetKeyDown(KeyCode.Alpha7))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[7] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[7] = !RGActiveArray[7];
                    RGArray[7].SetActive(RGActiveArray[7]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad8)||Input.GetKeyDown(KeyCode.Alpha8))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[8] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[8] = !RGActiveArray[8];
                    RGArray[8].SetActive(RGActiveArray[8]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad9)||Input.GetKeyDown(KeyCode.Alpha9))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[9] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[9] = !RGActiveArray[9];
                    RGArray[9].SetActive(RGActiveArray[9]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad0)||Input.GetKeyDown(KeyCode.Alpha0))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                if(RGArray[10] != null) //если такая радарограмма вообще существует
                {
                    RGActiveArray[10] = !RGActiveArray[10];
                    RGArray[10].SetActive(RGActiveArray[10]);
                }
            }

            // показать все
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                for(int i = 1; i < 11; i++)
                {
                    if(RGArray[i] != null) //если такая радарограмма вообще существует
                    {
                        RGActiveArray[i] = true;
                        RGArray[i].SetActive(RGActiveArray[i]);
                    }
                }
            }

            // скрыть все
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                for(int i = 1; i < 11; i++)
                {
                    if(RGArray[i] != null) //если такая радарограмма вообще существует
                    {
                        RGActiveArray[i] = false;
                        RGArray[i].SetActive(RGActiveArray[i]);
                    }
                }
            }

            // показывать в порядке убывания (поменял на порядок возрастания)
            if (Input.GetKeyDown(KeyCode.Equals)||Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                // for(int i = 10; i > 0; i--)
                for(int i = 1; i < 11; i++)
                {
                    if(RGArray[i] != null) //если такая радарограмма вообще существует
                    {
                        if(RGActiveArray[i] == false)
                        {
                            RGActiveArray[i] = true;
                            RGArray[i].SetActive(RGActiveArray[i]);
                            break;
                        }
                    }
                }
            }

            // скрыть в порядке возрастания (поменял на порядок убывания)
            if (Input.GetKeyDown(KeyCode.Minus)||Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if(RadarogramWereFound == false) FindRadarogram();
                // for(int i = 1; i < 11; i++)
                for(int i = 10; i > 0; i--)
                {
                    if(RGArray[i] != null) //если такая радарограмма вообще существует
                    {
                        if(RGActiveArray[i] == true)
                        {
                            RGActiveArray[i] = false;
                            RGArray[i].SetActive(RGActiveArray[i]);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void FindRadarogram()
    {
        for(int i = 1; i < 11; i++)
        {
            // TextLog.text = "попытка найти " + i.ToString();
            RGArray[i] = GameObject.Find("radarogramPrefab_" + i.ToString());
            if(RGArray[i] != null)
            {
                //TextLog.text = "нашёл радарограмму " + i.ToString();
                RadarogramWereFound = true;
            }
        }
    }
}
