using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private Text TextLog;
    [SerializeField] private GameObject ExitMenu;
    private bool MenuActive;

    // Start is called before the first frame update
    void Start()
    {
        MenuActive = false;
        ExitMenu.SetActive(MenuActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (MenuActive == false))  // если нажата клавиша Esc (Escape) и меню было ЗЫКРЫТО
      	{
            MenuActive = true;
            ExitMenu.SetActive(MenuActive);

            //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMove = false;
      	}
        else if (Input.GetKeyDown(KeyCode.Escape) && (MenuActive == true))  // если нажата клавиша Esc (Escape) и меню было ОТКРЫТО
        {
            ExitNoButton();
            //xyuButton();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && (MenuActive == true)) // если нажата клавиша Enter и меню было ОТКРЫТО
        {
            ExitYesButton();
        }
    }

    public void ExitYesButton()
    {
        Application.Quit();    // закрыть приложение
    }

    public void ExitNoButton()
    {
        //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMove = true;
        MenuActive = false;
        ExitMenu.SetActive(MenuActive);
    }
}
