using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerUI : MonoBehaviour
{
    private List<Texture2D> _textureList;
    [SerializeField] public int _layerNumber;
    private GameObject PaintScript;
    private bool ButtonVisibility = true; // видимость активна - глаз открыт
    [SerializeField] private Sprite EyeOpenSprite;
    [SerializeField] private Sprite EyeCloseSprite;
    [SerializeField] private GameObject ButtonEye;
    private HashSet<GameObject> GoOfThisLayer = new HashSet<GameObject>(); // множество объектов слоя (использую множество чтобы не копились одинаковые)

    public void Start()
    {
        _textureList = GameObject.Find("Paint").GetComponent<PaintingOnRadarograms>()._textureList;
        PaintScript =  GameObject.Find("Paint");

        if(_layerNumber == 1)
        {
            LayerActiveButton();
        }
    }

    void Update()
    {
        //this.GetComponent<Renderer>().material.mainTexture = _textureList[0];
        // try
        // {
        this.GetComponent<RawImage>().texture = _textureList[ (_layerNumber-1) * 25];
        // }
        // catch (System.Exception)
        // {
            
        //     throw;
        // }
        
    }

    public void LayerActiveButton()
    {
        // найти все объекты RawImage
        foreach (Transform item in this.transform.parent.transform)
        {
            // взять компонент LayerUI и у всех sizeDelta = new Vector2(100, 100);
            if (item.name == "RawImage" || item.name == "RawImage(Clone)")
            {
                item.transform.Find("Frame").GetComponent<RectTransform>().sizeDelta = new Vector2(99, 99);
                //пройдёмся по всем дочерним объектам рамки и поменяем цвет на красный
                foreach (Transform itemPanel in item.transform.Find("Frame").transform)
                {
                    itemPanel.GetComponent<Image>().color = Color.white;
                }
            }
        }
        this.transform.Find("Frame").GetComponent<RectTransform>().sizeDelta = new Vector2(101, 101); // затем только у данного повысить до 105

        //пройдёмся по всем дочерним объектам рамки и поменяем цвет на красный
        foreach (Transform item in this.transform.Find("Frame").transform)
        {
            item.GetComponent<Image>().color = Color.red;
        }
        // ВОТ ЭТО ТУТ САМОЕ ГЛАВНОЕ!
        PaintScript.GetComponent<WhichLayerActive>()._layerNumberActive = _layerNumber; // передаём в класс синхронизатор номер слоя с которым работаем. И при рисовании будет считать столкновение только с этим слоем

    }

    public void LayerVisibility()
    {
        if (ButtonVisibility) // если видимость ON
        {
            ButtonEye.GetComponent<Image>().sprite = EyeCloseSprite; // закрыли глаз
            ButtonVisibility = false;
            
            GameObject[] allGo = FindObjectsOfType<GameObject>(); // возьмём все объекты сцены
            foreach (GameObject item in allGo)
            {
                if(item.layer == LayerMask.NameToLayer("PaintLayer_" + _layerNumber.ToString())) // если они имеют слой текущей кнопки
                {
                    GoOfThisLayer.Add(item); // добавляем их во множество объектов этого слоя
                    item.SetActive(false); // деактивируем
                }
            }
        }
        else
        {
            ButtonEye.GetComponent<Image>().sprite = EyeOpenSprite; // открыли глаз
            ButtonVisibility = true;

            foreach (GameObject item in GoOfThisLayer) // проходимся по множеству объектов этого слоя, созданного при выключении видимости
            {
                item.SetActive(true); // активируем
            }
        }
    }
}
