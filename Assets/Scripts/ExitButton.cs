using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private GameObject ExitMenu;

    // Start is called before the first frame update
    void Start()
    {
        ExitMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))  // если нажата клавиша Esc (Escape)
      	{
            ExitMenu.SetActive(true);
            //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMove = false;
      	}
    }

    public void ExitYesButton()
    {
        Application.Quit();    // закрыть приложение
    }

    public void ExitNoButton()
    {
        //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMove = true;
        ExitMenu.SetActive(false);
    }
    
}
