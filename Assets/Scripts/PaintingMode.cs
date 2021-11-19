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
}
