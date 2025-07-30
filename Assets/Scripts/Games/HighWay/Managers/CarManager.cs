using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ColorSprite
{
    public string name;
    public Color color;
    public Sprite sprite;
}

[System.Serializable]
public struct LineStruct
{
    public short lineNumber;
    public GameObject startPoint;
    public GameObject endPoint;

    public GameObject colliderLineGameObject;
    public GameObject colliderIntranceGameObject;

    public SpriteRenderer entranceColorSprite;

    public Image entrance;
    public Image stop;
    public Image gate;
}

public class CarManager : Singleton<CarManager>
{
    #region Public Variables
    [Header("Lines Variables")]
    public GameObject[] lines_3;
    public GameObject[] lines_4;
    [Space]

    [Header("Color And Sprites")]
    public ColorSprite[] colorSprites;
    public Sprite goGateSprite;
    public Sprite stopGateSprite;

    public HighwayCodingFactory hiddenDataEncoder;
    #endregion


    #region Private Variables
    GameObject[] chosenLines;

    GameObject lastCar;

    Dictionary<int, ColorSprite> entranceColorSprites;

    List<ColorSprite> usingColorSprites;
    [SerializeField] List<LineStruct> linesInfo;

    Coroutine Spawning;

    Queue<GameObject> unSelectedCars;
    List<GameObject> selectedCars;
    HighwayParameters parameters;
    
    public float carsSpeed = 0f;

    int lineNumber;
    int EntranceNumber;
    bool randomCarSpriteColor;
    bool randomEntranceSpriteColor;
    bool entranceBlackColor;
    #endregion

    #region Properties
    public bool TouchEnable { get; private set; } = false;

    [SerializeField]
    public float TimeBetweenCarSpawns= 4f;
    //public float InitialTimeBetweenCarSpawns { get; set; } = 4;
    #endregion

    private void OnEnable()
    {
        Initialize();
        for (int i = 0; i < 3; i++) lines_3[i].SetActive(false);
        for (int i = 0; i < 4; i++) lines_4[i].SetActive(false);
    }

    public void Initialize()
    {
        unSelectedCars = new Queue<GameObject>();
        selectedCars = new List<GameObject>();
        entranceColorSprites = new Dictionary<int, ColorSprite>();
        lastCar = null;
        hiddenDataEncoder = new HighwayCodingFactory();
    }

    public void Start()
    {
        AddListeners();
    }

    public void AddListeners()
    {
        while (!EventManager.Instance.IsInitializedEM) ;
        EventManager.Instance.AddListeners("TouchedGameObject", ProcessSelectingLine);
    }

    #region Enable and Disable Touching
    public void EnableTouching()
    {
        TouchEnable = true;
    }

    public void DisableTouching()
    {
        TouchEnable = false;
    }
    #endregion

    #region Start and Stop Spawning
    public void StartSpawning(HighwayParameters _parameters)
    {
        hiddenDataEncoder.linesName.Clear();
        hiddenDataEncoder.linesColor.Clear();
        hiddenDataEncoder.carsName.Clear();
        hiddenDataEncoder.carsColor.Clear();
        hiddenDataEncoder.carsLine.Clear();

        ProcessRules(_parameters);

        linesInfo = LinesHandling(EntranceNumber);

        usingColorSprites = ChooseUsingColorSprites(EntranceNumber);

        parameters = _parameters;

        //TimeBetweenCarSpawns = InitialTimeBetweenCarSpawns;

        Spawning = StartCoroutine(Spawn());

        EnableTouching();
    }
    public void StopSpawning()
    {
        if (Spawning != null)
        {
            StopCoroutine(Spawning);
        }
        DestoryAllCars();
    }
    #endregion

    #region Get Lines Elements
    List<LineStruct> LinesHandling(int linesNumber)
    {
        if (linesNumber <= 3)
        {
            for (int i = 0; i < 3; i++) lines_3[i].SetActive(true);
            for (int i = 0; i < 4; i++) lines_4[i].SetActive(false);
            chosenLines = lines_3;
        }
        else
        {
            for (int i = 0; i < 3; i++) lines_3[i].SetActive(false);
            for (int i = 0; i < 4; i++) lines_4[i].SetActive(true);
            chosenLines = lines_4;
        }

        List<LineStruct> linesStruct = new List<LineStruct>();

        for (int i = 0; i < chosenLines.Length; i++)
        {
            LineStruct newLineStruct;
            newLineStruct.lineNumber = (short) i;
            newLineStruct.startPoint = chosenLines[i].transform.Find("StartPoint").gameObject;
            newLineStruct.endPoint = chosenLines[i].transform.Find("EndPoint").gameObject;
            Transform entranceTransform = chosenLines[i].transform.Find("Entrance");

            newLineStruct.entrance = entranceTransform.gameObject.GetComponent<Image>();

            newLineStruct.entranceColorSprite = entranceTransform.Find("EntranceSprite").gameObject.GetComponent<SpriteRenderer>();

            Transform stopTransform = entranceTransform.Find("Stop");
            newLineStruct.stop = stopTransform.gameObject.GetComponent<Image>();
            newLineStruct.gate = stopTransform.Find("Gate").gameObject.GetComponent<Image>();

            newLineStruct.colliderIntranceGameObject = entranceTransform.Find("TriggerEntranceGameObject").Find("EntranceCollider").gameObject;
            newLineStruct.colliderLineGameObject = chosenLines[i].transform.Find("TriggerLineGameObject").Find("LineCollider").gameObject;

            linesStruct.Add(newLineStruct);
        }
        return linesStruct;
    }
    #endregion


    void ProcessSelectingLine(GameObject lineGameObject)
    {
        if (TouchEnable)
        {
            for (int i = 0; i < EntranceNumber; i++)
            {
                if (linesInfo[i].colliderLineGameObject == lineGameObject || linesInfo[i].colliderIntranceGameObject == lineGameObject)
                {

                    LineSelectionHandling(i);
                    break;
                }
            }
        }
    }

    #region Get Game Rules
    void ProcessRules(HighwayParameters parameters)
    {
        lineNumber = parameters.lineNumber;
        EntranceNumber = parameters.entranceNumber;
        switch (parameters.carRule)
        {
            case 1:
                randomCarSpriteColor = false;
                break;
            case 2:
                randomCarSpriteColor = true;
                break;
        }
        switch (parameters.entranceRule)
        {
            case 1:
                randomEntranceSpriteColor = false;
                entranceBlackColor = false;
                break;
            case 2:
                randomEntranceSpriteColor = false;
                entranceBlackColor = true;
                break;
            case 3:
                randomEntranceSpriteColor = true;
                entranceBlackColor = false;
                break;
        }
    }
    #endregion

    #region Choose and Set Line Color Sprites
    List<ColorSprite> ChooseUsingColorSprites(int lineNumber)
    {
        List<ColorSprite> returnedColorSprites = new List<ColorSprite>();
        List<int> possibleIndices = new List<int>();

        if (colorSprites.Length < lineNumber)
        {
            Debug.LogError("There is not enough Color added to the CarManager.");
        }
        else 
        {
            List<int> tempIndices = new List<int>();

            DeleteEntrancesSprite();

            for (int i = 0; i < colorSprites.Length; i++) tempIndices.Add(i);
            for (int j = 0; j < EntranceNumber; j++)
            {
                int RandomIndex = Random.Range(0, tempIndices.Count);

                int selectedColorSpriteIndex = tempIndices[RandomIndex];
                tempIndices.RemoveAt(RandomIndex);

                returnedColorSprites.Add(colorSprites[selectedColorSpriteIndex]);
            }

            bool whileCond = true;
            int randi = -1;
            while (whileCond)
            {
                possibleIndices.Clear();
                for (int i = 0; i < EntranceNumber; i++)
                {
                    randi = Random.Range(0, EntranceNumber);
                    while (possibleIndices.Contains(randi))
                        randi = Random.Range(0, EntranceNumber);
                    possibleIndices.Add(randi);
                }

                whileCond = false;
                for (int i = 0; i < EntranceNumber; i++)
                {
                    if (possibleIndices[i] == i)
                    {
                        whileCond = true;
                        break;
                    }
                }
            }

            
            for (int j = 0; j < EntranceNumber; j++)
            {
                ColorSprite entranceColorSprite = returnedColorSprites[j];
                if (randomEntranceSpriteColor)
                {
                    /*int randomIndex = Random.Range(0, possibleIndices.Count);
                    int whileCount = 0;
                    while (randomIndex == j && whileCount<1000)
                    {
                        randomIndex = Random.Range(0, possibleIndices.Count);
                        whileCount++;
                    }*/
                    entranceColorSprite.color = returnedColorSprites[possibleIndices[j]].color;
                    //possibleIndices.Remove(possibleIndices[randomIndex]);
                }
                else if (entranceBlackColor) entranceColorSprite.color = Color.black;
                SetEntranceSprites(j, entranceColorSprite);
            }
            
        }

        //SET COMMON HIDDEN DATA
        // BRYG = [0,1,2,3]
        ColorSprite tempCS;
        int iColor, iSprite;
        for (int i = 0; i < entranceColorSprites.Count; i++)
        {
            tempCS = entranceColorSprites[i];
            for (iColor = 0; iColor < colorSprites.Length; iColor++)
                if (tempCS.color == colorSprites[iColor].color) break;
            for (iSprite = 0; iSprite < colorSprites.Length; iSprite++)
                if (tempCS.sprite == colorSprites[iSprite].sprite) break;
            hiddenDataEncoder.linesName.Add(iSprite);
            hiddenDataEncoder.linesColor.Add(iColor);
        }

        if (returnedColorSprites != null) return returnedColorSprites;
        else return null;
    }

    void DeleteEntrancesSprite()
    {
        ColorSprite emptyColorSprite = new ColorSprite();
        emptyColorSprite.color = Color.black;
        emptyColorSprite.name = "empty";
        emptyColorSprite.sprite = null;

        for (int k = 0; k < chosenLines.Length; k++) SetEntranceSprites(k, emptyColorSprite);
    }


    void SetEntranceSprites(int EntranceNumber, ColorSprite colorSprite)
    {
        SpriteRenderer entranceSpriteRenderer = linesInfo[EntranceNumber].entranceColorSprite;
        entranceSpriteRenderer.sprite = colorSprite.sprite;
        entranceSpriteRenderer.color = colorSprite.color;

        if (entranceColorSprites.ContainsKey(EntranceNumber)) entranceColorSprites[EntranceNumber] = colorSprite;
        else entranceColorSprites.Add(EntranceNumber, colorSprite);
    }
    #endregion

    #region Spawn Handling
    IEnumerator Spawn()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, lineNumber);
            GameObject newCar = Pool.InstantiateGameObjectByName("Car",(Vector2) linesInfo[randomIndex].startPoint.transform.position, Quaternion.identity);

            List<Transform> endPoints = new List<Transform>();
            List<GameObject> entranceCollisionGameObjects = new List<GameObject>();
            for (int i = 0; i < EntranceNumber; i++)
            {
                endPoints.Add(linesInfo[i].endPoint.transform);
                entranceCollisionGameObjects.Add(linesInfo[i].colliderIntranceGameObject);
            }
            Car carComponent = newCar.GetComponent<Car>();
            carComponent.SetEndPoints(endPoints, entranceCollisionGameObjects);
            carComponent.SetLine(randomIndex);
            carComponent.speed = GetCarSpeed(carsSpeed);

            if (lastCar != null)
            {
                unSelectedCars.Enqueue(newCar);
                carComponent.HighlightCar(false);
            }
            else
            {
                lastCar = newCar;
                carComponent.HighlightCar(true);
            }

            randomIndex = Random.Range(0, usingColorSprites.Count);
            ColorSprite carColorSprite = usingColorSprites[randomIndex];
            if (randomCarSpriteColor)
            {
                int randomIndex2 = Random.Range(0, usingColorSprites.Count);
                int whileCount = 0;
                while (randomIndex2 == randomIndex && whileCount < 1000)
                {
                    randomIndex2 = Random.Range(0, usingColorSprites.Count);
                    whileCount++;
                }
                carColorSprite.color = usingColorSprites[randomIndex2].color;
                carColorSprite.name = usingColorSprites[randomIndex2].name;
            }
            carComponent.SetSpriteColor(carColorSprite);

            //SET COMMON HIDDEN DATA
            // BRYG = [0,1,2,3]
            ColorSprite tempCS = carComponent.ColorSprite;
            int iColor = 0;
            for (iColor = 0; iColor < colorSprites.Length; iColor++)
                if (tempCS.color == colorSprites[iColor].color) break;
            int iSprite = 0;
            for (iSprite = 0; iSprite < colorSprites.Length; iSprite++)
                if (tempCS.sprite == colorSprites[iSprite].sprite) break;
            hiddenDataEncoder.carsColor.Add(iColor);
            hiddenDataEncoder.carsName.Add(iSprite);
            hiddenDataEncoder.carsLine.Add(carComponent.Line);

            carComponent.Go = true;
            yield return new WaitForSeconds(TimeBetweenCarSpawns);
        }
    }

    float GetCarSpeed(float m)
    {
        float screenWidth = System.Math.Abs(CameraController.Instance.camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 
            CameraController.Instance.camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
        return (float)screenWidth*m / 10f;
    }

    void LineSelectionHandling(int lineNumber)
    {
        if (lastCar != null)
        {
            lastCar.GetComponent<Car>().HighlightCar(false);
            lastCar.GetComponent<Car>().SelectLine(lineNumber);
            selectedCars.Add(lastCar);
        }
        lastCar = null;
        if (unSelectedCars != null && unSelectedCars.Count > 0)
        {
            lastCar = unSelectedCars.Dequeue();
            lastCar.GetComponent<Car>().HighlightCar(true);
        }
    }

    #endregion

    #region Process Reaching the Entrance
    public void NonReachedSelectedCar(GameObject car)
    {
        hiddenDataEncoder.selectedCar = 1;

        Car carComponent = car.GetComponent<Car>();

        hiddenDataEncoder.unselectedNumber = unSelectedCars.Count;
        hiddenDataEncoder.gateLine = carComponent.Line;

        if (carComponent.Line < entranceColorSprites.Count
            && carComponent.ColorSprite.name == entranceColorSprites[carComponent.Line].name)
        {
            EventManager.Instance.InvokeEvent("TrueAnswerEvent");
            /*TimeBetweenCarSpawns -= 0.1f;
            if (TimeBetweenCarSpawns < 1f) TimeBetweenCarSpawns = 1f;*/
            StartCoroutine(OpenStopGate(carComponent.Line));
        }
        else
        {
            EventManager.Instance.InvokeEvent("FalseAnswerEvent");
            /*TimeBetweenCarSpawns = InitialTimeBetweenCarSpawns;*/
            int i;
            for (i = 0; i < EntranceNumber; i++) if (entranceColorSprites[i].name == carComponent.ColorSprite.name) break;
            StartCoroutine(HighlightCorrectAnswer(linesInfo[i].entrance));
        }
    }


    public void ReachedEntranceHandling(GameObject car)
    {
        hiddenDataEncoder.selectedCar = selectedCars.Contains(car) ? 1 : 0;

        if (lastCar == car)
        {
            selectedCars.Add(lastCar);
            lastCar.GetComponent<Car>().HighlightCar(false);
            lastCar = null;
            if (unSelectedCars.Count > 0)
            {
                lastCar = unSelectedCars.Dequeue();
                lastCar.GetComponent<Car>().HighlightCar(true);
            }
        }

        Car carComponent = car.GetComponent<Car>();

        if (carComponent.IsSelected)
            return;

        hiddenDataEncoder.unselectedNumber = unSelectedCars.Count + (lastCar != null ? 1 : 0);
        hiddenDataEncoder.gateLine = carComponent.Line;

        if (LevelFactory.Instance.CurrentState != LevelDiffStates.LevelChanged)
        {
            if (carComponent.Line < entranceColorSprites.Count
                && carComponent.ColorSprite.name == entranceColorSprites[carComponent.Line].name)
            {
                EventManager.Instance.InvokeEvent("TrueAnswerEvent");
                /*TimeBetweenCarSpawns -= 0.1f;
                if (TimeBetweenCarSpawns < 1f) TimeBetweenCarSpawns = 1f;*/
                StartCoroutine(OpenStopGate(carComponent.Line));
            }
            else
            {
                EventManager.Instance.InvokeEvent("FalseAnswerEvent");
                /*TimeBetweenCarSpawns = InitialTimeBetweenCarSpawns;*/
                int i;
                for (i = 0; i < EntranceNumber; i++) if (entranceColorSprites[i].name == carComponent.ColorSprite.name) break;
                StartCoroutine(HighlightCorrectAnswer(linesInfo[i].entrance));
            }
        }
    }

    IEnumerator HighlightCorrectAnswer(Image entranceSprite)
    {
        entranceSprite.color = Color.green;
        yield return new WaitForSeconds(1f);
        entranceSprite.color = Color.white;
    }

    IEnumerator OpenStopGate(int gateNumber)
    {
        Image stop = linesInfo[gateNumber].stop;
        Image gate = linesInfo[gateNumber].gate;

        gate.sprite = goGateSprite;
        while (stop.fillAmount > 0)
        {
            yield return new WaitForSeconds(0.01f);
            stop.fillAmount -= 0.1f;
        }
        yield return new WaitForSeconds(2);
        gate.sprite = stopGateSprite;
        while (stop.fillAmount < 1)
        {
            yield return new WaitForSeconds(0.01f);
            stop.fillAmount += 0.1f;
        }
    }
    #endregion

    #region Destory Cars Methods
    public void DestroyCar(GameObject car)
    {
        if (selectedCars.Contains(car)) selectedCars.Remove(car);
        else Debug.LogError("The car doesn't exist in CarManager.");

        Pool.DestroyGameObject(car.name, car);
    }

    public void DestoryAllCars()
    {
        GameObject destroyedCar;
        if (lastCar != null)
            Pool.DestroyGameObject(lastCar.name, lastCar);
        lastCar = null;

        while (unSelectedCars.Count > 0)
        {
            destroyedCar = unSelectedCars.Dequeue();
            if (destroyedCar != null)
                Pool.DestroyGameObject(destroyedCar.name, destroyedCar);
            destroyedCar = null;
        }
        while (selectedCars.Count > 0)
        {
            destroyedCar = selectedCars[selectedCars.Count - 1];
            selectedCars.RemoveAt(selectedCars.Count - 1);
            if (destroyedCar != null)
                Pool.DestroyGameObject(destroyedCar.name, destroyedCar);
            destroyedCar = null;
        }
    }
    #endregion


    private void OnDisable()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveListeners("TouchedGameObject", ProcessSelectingLine);
    }

    internal Vector3 GetTheAnswerPosition()
    {
        if (lastCar != null)
        {
            Car carComponent = lastCar.GetComponent<Car>();
            for (int i = 0; i < chosenLines.Length; i++)
            {

                if (carComponent.ColorSprite.name == entranceColorSprites[i].name)
                    return chosenLines[i].transform.localPosition;
            }
            return new Vector3(0, 0, 10);
        }
        return new Vector3(0, 0, 10);
    }
}
