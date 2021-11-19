using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Этот скрипт вешать на тогл включения/отключения фотки
/// </summary>
public class SetActivePhoto : MonoBehaviour
{
    bool togglePhotoActive;
    [SerializeField] GameObject Photo_plane;

    void Start()
    {
        Photo_plane.SetActive(false);
        transform.gameObject.GetComponent<Toggle>().isOn = false;
    }

    void Update()
    {
        togglePhotoActive = transform.gameObject.GetComponent<Toggle>().isOn;
        if (togglePhotoActive) // true - включаем фотку, false - выключаем
        {
            Photo_plane.SetActive(true);
        }
        else
        {
            Photo_plane.SetActive(false);
        }
    }
}