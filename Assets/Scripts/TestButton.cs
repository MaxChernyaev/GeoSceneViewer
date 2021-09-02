using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    private bool MenuActive = false;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            testClickBut();
        }
    }

    public void testClickBut()
    {
        StartCoroutine("ClickScale");
        MenuActive = !MenuActive;
        SettingsMenu.SetActive(MenuActive);
        GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveMouse = !GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveMouse;
        GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey = !GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey;
    }

    IEnumerator ClickScale()
    {
        transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }
}
