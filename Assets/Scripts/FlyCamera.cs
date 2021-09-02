using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyCamera : MonoBehaviour
{
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
     
     
    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.15f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun= 1.0f;
    //private string InputField;
    //private string LastInputField = "";
    public bool permissionToMoveMouse = true; // для того, чтобы не крутилась камера мышкой, когда открыто меню настроек
    public bool permissionToMoveKey = true; // для того чтобы не нажимались WASDQE, когда открыто меню настроек
    Vector3 p;
    private float sensitivity = 3; // чувствительность мышки
    private float X, Y;
    private float limit = 80;
     
    void Update () {
        
        //Cursor.lockState = CursorLockMode.Locked; 
        //Cursor.visible = false;

        // вместо использования Input.mousePosition, который читает положение курсора с экрана
        lastMouse = Input.mousePosition - lastMouse ;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y*2, 0);
        //transform.eulerAngles = lastMouse;
        lastMouse =  Input.mousePosition;

        if(permissionToMoveMouse)
        {
            // юзаю Input.GetAxis("Mouse X") который берет дельту перемещения мыши напрямуюя
            X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            Y += Input.GetAxis("Mouse Y") * sensitivity;
            Y = Mathf.Clamp (Y, -limit, limit);
            transform.eulerAngles = new Vector3(-Y, X, 0);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            permissionToMoveMouse = !permissionToMoveMouse;
        }






        //InputField = GameObject.Find("InputFieldJSON").transform.Find("MyText").GetComponent<Text>().text;

        // if(Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     permissionToMove = true;
        //     Debug.Log("permissionToMove = true");
        // }
        //Mouse  camera angle done.
       
        //Keyboard commands
        //float f = 0.0f;
        // if(InputField != LastInputField){ // если ввели что-то новое в поле ввода
        //     permissionToMove = false;
        //     Debug.Log("permissionToMove = false");
        // }
        // else{
        //     if (permissionToMove == true){
                p = GetBaseInput();
        //     }
        // }
        if (Input.GetKey(KeyCode.Space)){
            mainSpeed = 1.0f;
        }
        else{
            mainSpeed = 10.0f;
        }
        // if (Input.GetKey (KeyCode.LeftShift)){
        //     totalRun += Time.deltaTime;
        //     p  = p * totalRun * shiftAdd;
        //     p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
        //     p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
        //     p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        // }
        // else{
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        // }
       
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.LeftShift)){ //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else{
            transform.Translate(p);
        }

        //LastInputField = InputField;
    }
     
    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (permissionToMoveKey)
        {
            if (Input.GetKey (KeyCode.W)){
                p_Velocity += new Vector3(0, 0 , 1);
            }
            if (Input.GetKey (KeyCode.S)){
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey (KeyCode.A)){
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey (KeyCode.D)){
                p_Velocity += new Vector3(1, 0, 0);
            }
            if (Input.GetKey (KeyCode.Q)){
                p_Velocity += new Vector3(0, 1, 0);
            }
            if (Input.GetKey (KeyCode.E)){
                p_Velocity += new Vector3(0, -1, 0);
            }
        }
        return p_Velocity;
    }
}
