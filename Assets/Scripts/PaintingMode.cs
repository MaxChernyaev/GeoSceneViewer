using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
/// Этот скрипт включает режим рисования - блокирует камеру, даёт разрешение на рисование, активирует канвас режима рисования, смотрит какой цвет выбран
///</summary>
public class PaintingMode : MonoBehaviour
{
    [SerializeField] private GameObject PaintingModeCanvas;
    [SerializeField] private Button ColorButtonRED, ColorButtonGREEN, ColorButtonBLUE, ColorButtonYELLOW, ColorButtonORANGE, ColorButtonPURPLE, ColorButtonBROWN, ColorButtonBLACK, ColorButtonGREY;
    private List<Button> ColorButtonArray = new List<Button>();
    [SerializeField] private Button EraserButton;
    [SerializeField] private Button SelectingAreaButton;
    [SerializeField] private Button BrushButton;
    [SerializeField] private Sprite SpriteColorButton;
    [SerializeField] private Sprite SpriteColorButtonPressed;
    [SerializeField] public Slider _sliderAlpha;
    [SerializeField] private Text _alphaPersent;
    [SerializeField] public Slider _sliderSize;
    [SerializeField] private Text _sizePersent;
    private Color _colorButton;
    private bool _paintingMode = false;
    private GameObject ButtonLoadRadarogram;
    private GameObject SettingsButton;
    private int LayerCount = 1; // количество слоёв - в начале один
    [SerializeField] private GameObject RawImagePrefab;
    [SerializeField] private GameObject _newLayerButton;
    private bool firstInstall = true;

    void Start()
    {
        ColorButtonArray.Add(ColorButtonRED);
        ColorButtonArray.Add(ColorButtonGREEN);
        ColorButtonArray.Add(ColorButtonBLUE);
        ColorButtonArray.Add(ColorButtonYELLOW);
        ColorButtonArray.Add(ColorButtonORANGE);
        ColorButtonArray.Add(ColorButtonPURPLE);
        ColorButtonArray.Add(ColorButtonBROWN);
        ColorButtonArray.Add(ColorButtonBLACK);
        ColorButtonArray.Add(ColorButtonGREY);
        
        ButtonLoadRadarogram = GameObject.Find("ButtonLoadRadarogram");
        SettingsButton = GameObject.Find("SettingsButton");
        PaintingModeCanvas.SetActive(true);
        REDButtonPressed(); // по умолчанию включаем красную
        BrushButtonPressed(); // по умолчанию кисть
        PaintingModeCanvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (_paintingMode)
            {
                GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveMouse = true;
                //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey = true;
                PaintingModeCanvas.SetActive(false);

                ButtonLoadRadarogram.SetActive(true); // показываем кнопки R и настройки
                SettingsButton.SetActive(true);

                transform.GetComponent<PaintingOnRadarograms>()._permissionToPainting = false;

                _paintingMode = false;
            }
            else
            {
                //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveMouse = false; // блокируем вращение камеры
                //GameObject.Find("Main Camera").GetComponent<FlyCamera>().permissionToMoveKey = false; // блокируем перемещение
                PaintingModeCanvas.SetActive(true); // активируем канвас режима рисования

                ButtonLoadRadarogram.SetActive(false); // скрываем кнопки R и настройки
                SettingsButton.SetActive(false);

                transform.GetComponent<PaintingOnRadarograms>()._permissionToPainting = true; // даём разрешение на рисование
                
                _paintingMode = true;
            }
        }

        _alphaPersent.text = ((int)(_sliderAlpha.GetComponent<Slider>().value*100)).ToString() + "%"; // пишем проценты слайдера прозрачность
        _sizePersent.text = ((int)(_sliderSize.GetComponent<Slider>().value*100)).ToString() + "px"; // пишем сколько пикселей размер (1-100)
        transform.GetComponent<PaintingOnRadarograms>()._brushSize = (int)(_sliderSize.GetComponent<Slider>().value*100); // устанавливаю размер кисти от значения слайдера
        transform.GetComponent<PaintingOnRadarograms>()._color.a = (float)(_sliderAlpha.GetComponent<Slider>().value); // устанавливаю непрозрачность по слайдеру
        // if(Input.GetKeyDown(KeyCode.L)) // для дальнейшей очистки нарисованного (ластик всего)
        // {
        //     transform.GetComponent<PaintingOnRadarograms>().ColliderList_clear();
        //      // тут надо пройтись по всем радарограммам и заново создать текстуры
        // }
    }

    public void REDButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonRED.GetComponent<Image>().sprite = SpriteColorButtonPressed; // КРАСНАЯ НАЖАТА
        ColorButtonRED.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = Color.red;
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void GREENButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonGREEN.GetComponent<Image>().sprite = SpriteColorButtonPressed; // ЗЕЛЁНАЯ НАЖАТА
        ColorButtonGREEN.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = Color.green;
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void BLUEButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonBLUE.GetComponent<Image>().sprite = SpriteColorButtonPressed; // СИНЯЯ НАЖАТА
        ColorButtonBLUE.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = Color.blue;
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void YELLOWButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonYELLOW.GetComponent<Image>().sprite = SpriteColorButtonPressed; // ЖЕЛТАЯ НАЖАТА
        ColorButtonYELLOW.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = Color.yellow;
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void ORANGEButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonORANGE.GetComponent<Image>().sprite = SpriteColorButtonPressed; // ОРАНЖЕВАЯ НАЖАТА
        ColorButtonORANGE.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = new Color(1f, 0.5f, 0f, 1f);
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void PURPLEButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonPURPLE.GetComponent<Image>().sprite = SpriteColorButtonPressed; // ПУРПУРНАЯ НАЖАТА
        ColorButtonPURPLE.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = new Color(0.6117647f, 0.2901961f, 0.854902f, 1f);
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void BROWNButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonBROWN.GetComponent<Image>().sprite = SpriteColorButtonPressed; // КОРИЧНЕВАЯ НАЖАТА
        ColorButtonBROWN.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = new Color(0.4528302f, 0.2787058f, 0.1174795f, 1f);
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void BLACKButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonBLACK.GetComponent<Image>().sprite = SpriteColorButtonPressed; // ЧЁРНАЯ НАЖАТА
        ColorButtonBLACK.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = new Color(0f, 0f, 0f, 1f);
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }
    public void GREYButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        ColorButtonGREY.GetComponent<Image>().sprite = SpriteColorButtonPressed; // СЕРАЯ НАЖАТА
        ColorButtonGREY.GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _colorButton = new Color(0.5660378f, 0.5660378f, 0.5660378f, 1f);
        transform.GetComponent<PaintingOnRadarograms>()._color = _colorButton; // передаём какой выбран цвет

        if (transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON == false) // если НЕ БЫЛО НАЖАТО выделение маски
            BrushButtonPressed();
    }

    public void EraserButtonPressed()
    {
        foreach (var item in ColorButtonArray) // отжимаем все кнопки всех цветов
        {
            item.GetComponent<Image>().sprite = SpriteColorButton;
            item.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        }
        transform.GetComponent<PaintingOnRadarograms>().EraserON = true; // активируем ластик
        EraserButton.GetComponent<Transform>().localScale = new Vector3(1.25f, 1.25f, 1.25f);
        SelectingAreaButton.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        BrushButton.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON = false; // выкл выделение маски (вкл кисть) (ластик работает как кисть)
    }

    public void SelectingAreaButtonPressed()
    {
        transform.GetComponent<PaintingOnRadarograms>().EraserON = false; // выкл ластик
        EraserButton.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        BrushButton.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON = true; // вкл выделение маски
        SelectingAreaButton.GetComponent<Transform>().localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }
    public void BrushButtonPressed()
    {
        transform.GetComponent<PaintingOnRadarograms>().EraserON = false; // выкл ластик
        EraserButton.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        SelectingAreaButton.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        transform.GetComponent<PaintingOnRadarograms>().SelectingAreaON = false; // выкл выделение маски (вкл кисть)
        BrushButton.GetComponent<Transform>().localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    public void NewLayerButton() // при нажатии на кнопку "+ новый слой"
    {
        if (LayerCount < 8)
        {
            LayerCount++;

            GameObject NewRawImage = Instantiate(RawImagePrefab, GameObject.Find("PaintRIGHTPanel").transform); // дублировать RawImage
            NewRawImage.GetComponent<LayerUI>()._layerNumber = LayerCount; // у него в компоненте Layer UI поменять LayerNumber на порядковый номер слоя
            NewRawImage.transform.Find("Заголовок").GetComponent<Text>().text = "Слой " + LayerCount + ":";
            NewRawImage.transform.position = new Vector3(NewRawImage.transform.position.x, NewRawImage.transform.position.y - 125 * (LayerCount-1), NewRawImage.transform.position.z); // передвинуть новый RawImage по Y на -125
            // NewRawImage.GetComponent<LayerUI>().LayerActiveButton(); // и сделать его активным

            foreach(var item in GameObject.FindGameObjectsWithTag("Radarogram"))
            {
                GameObject NewPaintRadarogramsLayer = Instantiate(item.transform.Find("RadarogramsLayer1").gameObject, item.transform.Find("RadarogramsLayer1").transform.parent); // дублируем прошлый слой каждой радарограммы
                NewPaintRadarogramsLayer.name = "RadarogramsLayer" + LayerCount.ToString();
                foreach (Transform i in NewPaintRadarogramsLayer.transform)
                {
                    i.gameObject.SetActive(true); // на случай, если первый слой был выключен, а мы скопировали его объекты
                    i.gameObject.layer = LayerMask.NameToLayer("PaintLayer_" + LayerCount.ToString());
                    GameObject.Find("Paint").GetComponent<PaintingOnRadarograms>().AddColliderList(i.GetComponent<MeshCollider>()); // передаём в скрипт рисования коллайдеры плоскостей для рисования
                }
            }

            GameObject NewPaintLayer = Instantiate(GameObject.Find("Layer1"), GameObject.Find("Layer1").transform.parent); // дублируем прошлый слой земли
            NewPaintLayer.name = "Layer" + LayerCount.ToString();

            foreach (Transform item in NewPaintLayer.transform)
            {
                item.gameObject.SetActive(true); // на случай, если первый слой был выключен, а мы скопировали его объекты
                item.gameObject.layer = LayerMask.NameToLayer("PaintLayer_" + LayerCount.ToString());
                transform.GetComponent<PaintingOnRadarograms>().AddColliderList(item.gameObject.GetComponent<MeshCollider>()); // каждый объект нового слоя нужно добавить в коллайдеры, и там создадутся текстуры для нового слоя
                if (item.name == "Quad_GND") // если созданный объект - слой земли, то добавляем его в список объектов GND, чтобы при нажатии на R они не удалились
                {
                    transform.GetComponent<PaintingOnRadarograms>().GND.Add(item.gameObject);
                }
            }
            NewRawImage.GetComponent<LayerUI>().Start(); // принудительно запускаю старт нового объекта, чтобы не было ошибки на следующей строчке, потому что видимо по умолчанию start сработает только на следующем кадре
            NewRawImage.GetComponent<LayerUI>().LayerActiveButton(); // и сделать новый созданный слой активным

            if (LayerCount == 8)
            {
                // при создании десятого слоя (пока восьмого, больше в экран FullHD не влезло) деактивируем кнопку
                _newLayerButton.SetActive(false);
            }
        }

        // for(int i = 0; i < 25; i++)
        // {
        //     GameObject LastPaintLayer =  GameObject.Find("Quad_GND");
        //     LastPaintLayer.name = LastPaintLayer.name + "1";
        //     GameObject NewPaintLayer = Instantiate(LastPaintLayer, LastPaintLayer.transform.parent.transform.parent);
        //     NewPaintLayer.name = "Quad_GND";
        //     NewPaintLayer.layer = LayerMask.NameToLayer("PaintLayer_2");
        //     transform.GetComponent<PaintingOnRadarograms>().AddColliderList(NewPaintLayer.GetComponent<MeshCollider>()); // каждый объект нового слоя нужно добавить в коллайдеры, и там создадутся текстуры для нового слоя
        // }
        // for(int i = 0; i < 25; i++)
        // {
        //     GameObject LastPaintLayer =  GameObject.Find("Quad_GND1");
        //     LastPaintLayer.name = "Quad_GND";
        // }
        // GameObject TESTprefab =  GameObject.Find("Quad_GND");
        // Instantiate(TESTprefab,TESTprefab.transform.parent);
    }

    public void FirstInstallRadarogramLayers() // эта функция должна вызываться при первом создании радарограмм, для того чтобы наложить на них нужное количество слоёв (если они были созданы до создания радарограмм)
    {
        if(LayerCount > 1 && firstInstall == true)
        {
            firstInstall = false;
            // для каждого существующего слоя пройдёмся по радарограммам и всем им добавим этот слой
            for(int layerNum = 2; layerNum <= LayerCount; layerNum++)
            {
                foreach(GameObject item in GameObject.FindGameObjectsWithTag("Radarogram"))
                {
                    bool permission = true;
                    foreach (Transform child in item.transform) // пройдёмся по всем детям слоя и посмотрим нет ли уже такого, какой хотим создавать
                    {
                        if(child.gameObject.name == "RadarogramsLayer" + layerNum.ToString()) // если слой с таким номером уже есть
                        {
                            permission = false;
                            break;  
                        } 
                    }

                    if(permission)
                    {
                        GameObject NewPaintRadarogramsLayer = Instantiate(item.transform.Find("RadarogramsLayer1").gameObject, item.transform.Find("RadarogramsLayer1").transform.parent); // дублируем прошлый слой каждой радарограммы
                        NewPaintRadarogramsLayer.name = "RadarogramsLayer" + layerNum.ToString();
                        foreach (Transform i in NewPaintRadarogramsLayer.transform)
                        {
                            i.gameObject.SetActive(true); // на случай, если первый слой был выключен, а мы скопировали его объекты
                            i.gameObject.layer = LayerMask.NameToLayer("PaintLayer_" + layerNum.ToString());
                            GameObject.Find("Paint").GetComponent<PaintingOnRadarograms>().AddColliderList(i.GetComponent<MeshCollider>()); // передаём в скрипт рисования коллайдеры плоскостей для рисования
                        }
                    }
                }
            }
        }
    }
}
