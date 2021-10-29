using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// скрипт для добавления новых баз (основа для сцены) в нужную точку плоскости
/// </summary>
public class BasesInstaller : MonoBehaviour
{
    [SerializeField] private GameObject BaseGizmoPrefab;
    [SerializeField] private GameObject BasePlanePrefab;
    GameObject currentGizmo;
    GameObject currentPlane;
    private int PlaneNum = 0;
    //int side = 0; // сторона куда перемещаем объект: 1-вправо, 2-влево, 3-вверх, 4-вниз
    Vector3 direction;
    bool choseAxis = false;
    int holdtime = 0;
    List<KeyCode> keyPadList = new List<KeyCode>(){KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9};
    List<KeyCode> keyAlphaList = new List<KeyCode>(){KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};
    List<GameObject> BasePlanesList = new List<GameObject>(); // хранит все базовые плоскости для AR сцен
    [SerializeField] private GameObject myscrollview;
    [SerializeField] private Text textLOG;
    float GizmoSpeed = 1f; //скорость перемещения Gizmo (1 метр за одно нажатие)

    void Start()
    {
        BasePlanesList.Add(GameObject.Find("BasePlane_main")); // первой добавим в список главную базовую плоскость
        myscrollview.SetActive(false);
        // textLOG.text = "";
        // foreach (var item in BasePlanesList)
        // {
        //     textLOG.text += item.name + "\n";
        // }

    //     GameObject[] allGo = FindObjectsOfType<GameObject>();
    //     foreach (GameObject go in allGo)
    //     {
    //         if (go.tag == "BasePlane")
    //         {
    //             string NameNumber = go.name.Substring(go.name.Length - 1);

    //         }
    //     }
    }

    void Update()
    {
        if (GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey) // для того, чтобы не нажимались кнопки, когда открыто меню настроек
        {
            if(Input.GetKey(KeyCode.G))
            {
                holdtime++;
                if (holdtime == 60) BasesInstallerButton(); // кнопку держат 1 секунду
            }
            if(Input.GetKeyUp(KeyCode.G)) holdtime = 0;

            if (Input.GetKey(KeyCode.Space)){
                GizmoSpeed = 0.1f;
            }
            else{
                GizmoSpeed = 1f;
            }


            if(Input.GetKeyDown(KeyCode.RightArrow)){
                //currentGizmo.transform.position = currentGizmo.transform.position + new Vector3(GizmoSpeed,0,0);
                direction = Vector3.right;
                currentGizmo.transform.position = currentGizmo.transform.position + direction * GizmoSpeed;
                distanceTextMove();
                choseAxis = true;
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                direction = Vector3.left;
                currentGizmo.transform.position = currentGizmo.transform.position + direction * GizmoSpeed;
                distanceTextMove();
                choseAxis = true;
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                direction = Vector3.forward;
                currentGizmo.transform.position = currentGizmo.transform.position + direction * GizmoSpeed;
                distanceTextMove();
                choseAxis = true;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                direction = Vector3.back;
                currentGizmo.transform.position = currentGizmo.transform.position + direction * GizmoSpeed;
                distanceTextMove();
                choseAxis = true;
            }
            
            for(int i = 0; i < keyAlphaList.Count; i++) // по количеству верхних цифр, потому что и там и там их одинаково
            {
                if (Input.GetKeyDown(keyAlphaList[i]) || Input.GetKeyDown(keyPadList[i])) // для верхних цифр и цифрового блока
                {
                    if (choseAxis == true) // если до этого выбирали новую ось - обнуляем
                    {
                        currentGizmo.transform.position = currentGizmo.transform.position + (-1) * Vector3.Scale(MyVectorAbs(direction), currentGizmo.transform.position); // обнуляем выбранную координату
                        // currentGizmo.transform.position = Vector3.Scale(Vector3.one - MyVectorAbs(direction), currentGizmo.transform.position); // альтернативный вариант
                        choseAxis = false;
                    }
                    // умножаем текущее положение на 10, чтобы повысить порядок, и прибавляем введенную цифру
                    Vector3 distanceToMove = currentGizmo.transform.position * 10 + new Vector3(i,i,i); // тут i - цифра нажатой кнопки
                    //currentGizmo.transform.position = new Vector3(distanceToMove,0,0); // тут перемещение реализовано только вправо

                    // НИЖЕ В 2х строках direction ПОКА ЧТО БЕРЕТСЯ ПО МОДУЛЮ, ТО ЕСТЬ ПЕРЕМЕЩЕНИЕ ТОЛЬКО В ПОЛОЖИТЕЛЬНУЮ ЧАСТЬ (без клавиши минус)
                    currentGizmo.transform.position = Vector3.Scale((Vector3.one - MyVectorAbs(direction)), currentGizmo.transform.position); // у текущего положения обнулим нужную координату,
                    currentGizmo.transform.position += Vector3.Scale(distanceToMove, MyVectorAbs(direction)); // а затем прибавим сдвиг
                    distanceTextMove(); // вывод расстояния в UI
                }
                if(Input.GetKeyDown(KeyCode.Backspace))
                {
                    currentGizmo.transform.position = currentGizmo.transform.position + (-1) * Vector3.Scale(MyVectorAbs(direction), currentGizmo.transform.position); // обнуляем выбранную координату
                    distanceTextMove(); // вывод расстояния в UI
                }
            }
        }
    }

    private Vector3 MyVectorAbs(Vector3 v)
    {
        return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    // Добавляет новый Gizmo
    public void BasesInstallerButton()
    {
        myscrollview.SetActive(true);
        currentGizmo = Instantiate(BaseGizmoPrefab, new Vector3(5f,0,5f), new Quaternion(0,0,0,0));

        // 2 след строки убрать
        // currentGizmo.transform.Find("Distance between Gizmo X").GetComponent<TextMesh>().text = "0 м";
        // currentGizmo.transform.Find("Distance between Gizmo Z").GetComponent<TextMesh>().text = "0 м";

        currentPlane = Instantiate(BasePlanePrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0), currentGizmo.transform);
        PlaneNum++;
        currentPlane.name = "BasePlane_" + PlaneNum;
        BasePlanesList.Add(currentPlane);
        textLOG.text = "";
        foreach (var item in BasePlanesList)
        {
            textLOG.text += item.name + "\n";
        }
    }

    // Эта функция будет изменять положение текста с подписью расстояния от базового Gizmo до дополнительных
    private void distanceTextMove()
    {
        // Vector3 FirstGizmoPosition = GameObject.Find("BasePlane_main").transform.parent.transform.position;
        Vector3 SecondGizmoPosition = currentGizmo.transform.position;
        Vector3 AddFirstPosX = new Vector3(SecondGizmoPosition.x,0,0);
        Vector3 AddFirstPosZ = new Vector3(0,0,SecondGizmoPosition.z);
        Vector3 DirectionVectorX = SecondGizmoPosition - AddFirstPosX;
        Vector3 DirectionVectorZ = SecondGizmoPosition - AddFirstPosZ;
        float MyMagnitudeX = DirectionVectorX.magnitude;
        float MyMagnitudeZ = DirectionVectorZ.magnitude;
        MyMagnitudeX = (float)Math.Round(MyMagnitudeX, 1);
        MyMagnitudeZ = (float)Math.Round(MyMagnitudeZ, 1);
        //Vector3 CenterBetweenGizmoX = Vector3.Lerp(AddFirstPosX, SecondGizmoPosition, 0.5f);
        //Vector3 CenterBetweenGizmoZ = Vector3.Lerp(AddFirstPosZ, SecondGizmoPosition, 0.5f);

        // Vector3 DirectionVector = SecondGizmoPosition - FirstGizmoPosition;
        // float MyMagnitude = DirectionVector.magnitude;
        // Vector3 CenterBetweenGizmo = Vector3.Lerp(FirstGizmoPosition,SecondGizmoPosition,0.5f);

        // Vector3 PositionDistanceX = new Vector3(CenterBetweenGizmo.x, 0, currentGizmo.transform.position.z);
        // Vector3 PositionDistanceZ = new Vector3(currentGizmo.transform.position.x, 0, CenterBetweenGizmo.z);
        //currentGizmo.transform.Find("Distance between Gizmo X").transform.position = CenterBetweenGizmoZ;
        currentGizmo.transform.Find("Distance between Gizmo X").GetComponent<TextMesh>().text = MyMagnitudeZ.ToString() + " м";
        
        //currentGizmo.transform.Find("Distance between Gizmo Z").transform.position = CenterBetweenGizmoX;
        currentGizmo.transform.Find("Distance between Gizmo Z").GetComponent<TextMesh>().text = MyMagnitudeX.ToString() + " м";
        
    }

}
