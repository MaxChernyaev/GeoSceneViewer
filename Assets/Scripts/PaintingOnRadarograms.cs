using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaintingOnRadarograms : MonoBehaviour {

    // [Range(2, 512)]
    // [SerializeField] private int _textureSize = 512;
    [SerializeField] private int _textureSizeX = 512;
    [SerializeField] private int _textureSizeY = 512;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Texture2D _texture = null; // раньше тут было без "= null" и в OnValidate() новая текстура создавалась не каждый раз. Пока оставлю так
    [SerializeField] public List<Texture2D> _textureList;
    [SerializeField] private Material _material;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Collider> _colliderList;
    [SerializeField] private Collider _collider;
    [SerializeField] public Color _color;
    [SerializeField] public int _brushSize = 14;
    private int Old_brushSize; // хранит текущий размер кисти при рисовании на земле
    private bool beforePaintedOnGND = false; // true - если до этого рисовали на земле
    // [SerializeField] public Material[] _materialArray;
    [SerializeField] public List<Material> _materialArray;
    //[SerializeField] private GameObject[] GND;
    [SerializeField] public List<GameObject> GND;
    // [SerializeField] private Material _materialGND;
    public bool _permissionToPainting = false;

    private int _oldRayX, _oldRayY;
    
    public bool EraserON = false; // ластик (вкл/выкл)
    public bool SelectingAreaON = false; // выделение области (маски) (вкл/выкл)
    private bool NewMouseClick = true;
    private int StartPixel;
    //[SerializeField] private LayerMask _layerMask;
    [SerializeField] private int LayerCount;
    [SerializeField] private LayerMask _currentLayer;

    void Awake()
    {
        //_layerMask = ~_layerMask; // переворачиваю значение, чтобы учитывались все слои кроме этого
    }
    void Start()
    {

        foreach(GameObject item in GND)
        {
            AddColliderList(item.GetComponent<MeshCollider>());
        }
        // AddColliderList(GND.GetComponent<MeshCollider>());
        StartCoroutine("textureApplyCoroutine");
    }

    public void AddColliderList(Collider QuadCollider)
    {
        // добавляем коллайдер этой радарограммы в список ПОТОМ ПЛАНИРУЮ ПРОХОДИТЬСЯ ПО КОЛЛАЙДЕРАМ, СМОТРЕТЬ В КАКОЙ ПОПАЛ И МАТЕРИАЛ ТОГО ПРИСВАИВАТЬ В _material ЧТОБЫ ДАЛЬШЕ РИСОВАТЬ НА НЁМ
        _colliderList.Add(QuadCollider);
        // _collider = QuadCollider; // ВРЕМЕННО
        _material = QuadCollider.GetComponentInParent<MeshRenderer>().material;
        _materialArray.Add(_material);
        // Если последняя добавленая плоскость не имеет текстуры
        //if (QuadCollider.GetComponentInParent<MeshRenderer>().material.mainTexture == null){
            // у последнего добавленного коллайдера берём разрешение (размер)
            _textureSizeX = QuadCollider.GetComponentInParent<PaintQuadProperties>().textureSizeX;
            _textureSizeY = QuadCollider.GetComponentInParent<PaintQuadProperties>().textureSizeY;
            // _texture = (Texture2D)QuadCollider.GetComponentInParent<MeshRenderer>().material.mainTexture;
            _texture = new Texture2D(_textureSizeX, _textureSizeY);
            clearTexture();
            _texture.wrapMode = _textureWrapMode;
            _texture.filterMode = _filterMode;
            QuadCollider.GetComponentInParent<MeshRenderer>().material.mainTexture = _texture;
            _textureList.Add(_texture);
            _texture.Apply();
        //}
    }

    // public void colliderList_revers()
    // {
    //     _colliderList.Reverse();
    //     _textureList.Reverse();
    // }

    public void ColliderList_clear()
    {
        _colliderList.Clear();
        _textureList.Clear();
        Start(); // добавляем заново текстуру земли
    }

    public void TextureClear() // РАБОТАЕТ СТРАННО, ИСЧЕЗАЮТ ТЕКСТУРЫ ТОЛЬКО ПОСЛЕ ТОГО КАК НАЧНЁШЬ НА НИХ РИСОВАТЬ
    {
        foreach (var item in _textureList)
        {
            Color fillColor = Color.clear;
            Color[] fillPixels = new Color[item.width * item.height];
            for (int i = 0; i < fillPixels.Length; i++)
            {
                fillPixels[i] = fillColor;
            }
            item.SetPixels(fillPixels);
        }
    }

    void MyOnValidate() {
        if (_texture == null) {
            _texture = new Texture2D(_textureSizeX, _textureSizeY);
            clearTexture();
        }
        if (_texture.width != _textureSizeX) {
            _texture.Resize(_textureSizeX, _textureSizeY);
            clearTexture();
        }
        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _material.mainTexture = _texture;
        _texture.Apply();
    }

    void clearTexture() // для заполнения созданной текстуры прозрачными пикселями
    {
        //Debug.Log("СОЗДАЮ НОВУЮ ПРОЗРАЧНУЮ ТЕКСТУРУ ДЛЯ РИСОВАНИЯ");
        Color fillColor = Color.clear;
        Color[] fillPixels = new Color[_texture.width * _texture.height];
        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
        }
        _texture.SetPixels(fillPixels);
    }

    private void Update() {

        _brushSize += (int)Input.mouseScrollDelta.y;
        if (Input.GetMouseButtonDown(0)) { // детектируем первое (новое) нажатие
            NewMouseClick = true;
        }
        if (Input.GetMouseButton(0) && _permissionToPainting) { // нажали ЛКМ и есть разрешение на рисование
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerCount = GameObject.Find("Paint").GetComponent<WhichLayerActive>()._layerNumberActive;

            /*
            foreach (var item in _colliderList)
            {
                if (item.Raycast(ray, out hit, 100f)) {
                    int index = _colliderList.IndexOf(item); // Номер материала, который нужно менять!
                    Debug.Log("index:" + index);
                    _material = _materialArray[index];
                    Debug.Log("material.name:" + _material.name);
                    //_texture = (Texture2D)_material.mainTexture; // ВИДИМО ПРИДЕТСЯ СОЗДАВАТЬ МАССИВ ТЕКСТУР И ПОТОМ ТУТ ВЫБИРАТЬ ИЗ НИХ, А НЕ БРАТЬ ОТ МАТЕРИАЛА
                    _texture = _textureList[index];

                    _textureSizeX = item.GetComponentInParent<PaintQuadProperties>().textureSizeX;
                    _textureSizeY = item.GetComponentInParent<PaintQuadProperties>().textureSizeY;

                    int rayX = (int)(hit.textureCoord.x * _textureSizeX);
                    int rayY = (int)(hit.textureCoord.y * _textureSizeY);
                    
                    if (_oldRayX != rayX || _oldRayY != rayY) {
                        //DrawQuad(rayX, rayY);
                        DrawCircle(rayX, rayY);
                        _oldRayX = rayX;
                        _oldRayY = rayY;
                    }
                    _texture.Apply();
                    break; // если было найдено первое пересечение, выходим из цикла. Чтобы не детектировать пересечение с другими коллайдерами насквозь первого
                }
            }
            */

            //_currentLayer = LayerMask.NameToLayer("PaintLayer_" + LayerCount.ToString());
            _currentLayer = LayerMask.GetMask("PaintLayer_" + LayerCount.ToString());

            if(Physics.Raycast(ray, out hit, 100f, _currentLayer)) // будет учитываться столкновение только с тем слоем, который указан тут
            {
                // Debug.Log(_layerMask.value);
                if (EventSystem.current.IsPointerOverGameObject()){ // Это условие для того, чтобы не происходило рисование, при нажатии на Canvas!
                    return;
                }
                else
                {
                    int index = _colliderList.IndexOf(hit.collider); // Номер материала, который нужно менять!
                    // Debug.Log("index:" + index);
                    _material = _materialArray[index];
                    //Debug.Log("material.name:" + _material.name);
                    // if (_material.name == "alphaPaintMaterialGND")
                    // {
                    //     if (_brushSize != 10)
                    //         Old_brushSize = _brushSize;
                    //     _brushSize = 10;

                    //     beforePaintedOnGND = true;
                    // }
                    // else
                    // {
                    //     if (beforePaintedOnGND == true)
                    //     {
                    //         _brushSize = Old_brushSize;
                    //     }
                    //     beforePaintedOnGND = false;
                    // }
                    //_texture = (Texture2D)_material.mainTexture; // ВИДИМО ПРИДЕТСЯ СОЗДАВАТЬ МАССИВ ТЕКСТУР И ПОТОМ ТУТ ВЫБИРАТЬ ИЗ НИХ, А НЕ БРАТЬ ОТ МАТЕРИАЛА
                    _texture = _textureList[index];

                    _textureSizeX = hit.collider.GetComponentInParent<PaintQuadProperties>().textureSizeX;
                    _textureSizeY = hit.collider.GetComponentInParent<PaintQuadProperties>().textureSizeY;

                    int rayX = (int)(hit.textureCoord.x * _textureSizeX);
                    int rayY = (int)(hit.textureCoord.y * _textureSizeY);
                    
                    if (_oldRayX != rayX || _oldRayY != rayY) {
                        //DrawQuad(rayX, rayY);
                        if (SelectingAreaON)
                        {
                            DrawSelectingArea(rayX, rayY);
                        }
                        else
                        {
                            DrawCircle(rayX, rayY);
                        }
                        _oldRayX = rayX;
                        _oldRayY = rayY;
                    }
                    // _texture.Apply();
                }
            }
        }

        //MyOnValidate(); // эта функция работала только в редакторе, поэтому я отсюда её запущу для билда (убрать My чтобы работала в редакторе)
    }

    void DrawQuad(int rayX, int rayY) {
        for (int y = 0; y < _brushSize; y++) {
            for (int x = 0; x < _brushSize; x++) {
                _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
            }
        }
    }

    void DrawSelectingArea(int rayX, int rayY) {
        if(_permissionToPainting) // разрешение на рисование
        {
            if (NewMouseClick)
            {
                NewMouseClick = false;
                //Debug.Log("ПЕРВЫЙ КЛИК");
                StartPixel = rayX;
            }

            for(int i = StartPixel; i < rayX; i++)
            {
                for(int j = 0; j < _textureSizeY; j++)
                {
                    _texture.SetPixel(i, j, _color);
                }
            }
            for(int i = StartPixel; i > rayX; i--)
            {
                for(int j = 0; j < _textureSizeY; j++)
                {
                    _texture.SetPixel(i, j, _color);
                }
            }

            // _texture.SetPixel(rayX, rayY, _color);
        }
    }

    void DrawCircle(int rayX, int rayY) {
        if(_permissionToPainting) // разрешение на рисование
        {
            for (int y = 0; y < _brushSize; y++) {
                for (int x = 0; x < _brushSize; x++) {

                    float x2 = Mathf.Pow(x - _brushSize / 2, 2);
                    float y2 = Mathf.Pow(y - _brushSize / 2, 2);
                    float r2 = Mathf.Pow(_brushSize / 2 - 0.5f, 2);

                    if (x2 + y2 < r2) {
                        int pixelX = rayX + x - _brushSize / 2;
                        int pixelY = rayY + y - _brushSize / 2;

                        if (pixelX >= 0 && pixelX < _textureSizeX && pixelY >= 0 && pixelY < _textureSizeY) { // для того, чтобы не передавать координаты, которые за границей текстуры. Что приводило к линии по краю
                            // if (NewMouseClick)
                            // {
                            //     NewMouseClick = false;
                            //     Debug.Log("ПЕРВЫЙ КЛИК");
                            // }
                            Color oldColor = _texture.GetPixel(pixelX, pixelY);
                            Color resultColor = Color.Lerp(oldColor, _color, _color.a);


                            if (EraserON) // если активен ластик
                            {
                                resultColor.a = 0;
                            }
                            else
                            {
                                //resultColor.a = transform.GetComponent<PaintingMode>()._sliderAlpha.GetComponent<Slider>().value; // убрал это, потому что PaintingMode уже передаёт мне каждый кадр значение альфа-слайдера
                            }
                            _texture.SetPixel(pixelX, pixelY, resultColor);
                        }

                    }

                    
                }
            }
        }
    }

    IEnumerator textureApplyCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f); // попытка сделать обновление текстуры быстрее, чем раз в кадр. Ничего не меняет, нужно попробовать именно красить пиксели чаще чем раз в кадр
            _texture.Apply();
        }
    }

}
