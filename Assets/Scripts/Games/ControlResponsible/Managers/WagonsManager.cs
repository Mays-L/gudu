using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonsManager : Singleton<WagonsManager>
{
    public bool IsTrueWagonTurn = false;

    public Sprite[] SelectedSprites;
    public List<Sprite> TrueSprites;

    List<GameObject> Wagons = new List<GameObject>();
    List<GameObject> StaticWagons = new List<GameObject>();
    TrainPositionFactory TrainPositionFactory;

    [SerializeField]
    GameObject Train;

    [SerializeField]
    GameObject MainWagon;

    [SerializeField]
    GameObject StaticTrain;

    [SerializeField]
    GameObject TempWagon;

    [SerializeField]
    Sprite[] Colors;

    [SerializeField]
    GameObject WasButton;

    [SerializeField]
    GameObject Rail;

    int colorIndex = 0;

    /*int wagonTurn = 0;
    [SerializeField]
    int trueRate = 3;
    [SerializeField]
    float speed;
    float initSpeed;*/

    public bool ActiveButton = false;

    int AllWagonsNumber = 0;
    List<bool> WagonsConds = new List<bool>();

    public TrainCodingFactory hiddenDataEncoder = new TrainCodingFactory();

    private void Start()
    {
        TrainPositionFactory = new TrainPositionFactory();
        EventManager.Instance.AddListeners("NewWagonEnter", CreateWagon);
    }

    private void Update()
    {
        Rail.transform.localPosition = Rail.transform.localPosition + ((-Rail.transform.localPosition.y + Train.transform.localPosition.y) * Vector3.up);
    }

    internal void ShowWagons(float _speed, int _trueWagonsNumber, int _falseWagonsNumber)
    {
        hiddenDataEncoder.wagonsImagesIndex.Clear();
        WagonsConds.Clear();
        SelectSprites(false); //ABSTRACT = FALSE
        SelectTrueSprites(_trueWagonsNumber);
        AllWagonsNumber = _trueWagonsNumber + _falseWagonsNumber;
        SetTrainSpeed(_speed);
        ShowTrueWagons();
        ActiveStaticTrain();
    }

    private void ActiveStaticTrain()
    {
        ActiveButton = false;
        WasButton.SetActive(false);
        StaticTrain.SetActive(true);
        Train.SetActive(false);
        Train.GetComponent<Train>().StopMoving();
    }

    private void ShowTrueWagons()
    {
        List<Vector3> positions = TrainPositionFactory.GetInitialPositions(TrueSprites.Count, TempWagon);
        for(int i=0;i < TrueSprites.Count;i++)
        {
            GameObject newWagon = CreateNewWagon(positions[i], StaticTrain.transform, TrueSprites[i]);
            newWagon.GetComponent<Wagon>().SetStatic();
            StaticWagons.Add(newWagon);
        }

    }

    internal void WagonGoIn(GameObject wagon)
    {
        if (IsTrueWagonTurn && ActiveButton)
            EventManager.Instance.InvokeEvent("FalseAnswerEvent");
        IsTrueWagonTurn = wagon.GetComponent<Wagon>().IsAnswer;

        int wagonIndex = wagon.GetComponent<Wagon>().myIndex;
        if (Wagons.Count > wagonIndex + 1)
            Wagons[wagonIndex + 1].GetComponent<Wagon>().OnSprite();
        if (Wagons.Count > wagonIndex + 2)
            Wagons[wagonIndex + 2].GetComponent<Wagon>().OnSprite();

        ActiveButton = true;
        OffWagons();
        wagon.GetComponent<Wagon>().SetOnWagon();
        hiddenDataEncoder.wagonIndex = wagonIndex;
        Timers.Instance.StartTimer("ResponseTimer", 0);
        Timers.Instance.StartAnswerTimer();
    }

    private void OffWagons()
    {
        foreach (GameObject wagon in Wagons)
        {
            wagon.GetComponent<Wagon>().SetOffWagon();
        }
    }

    internal void EnableActiveButton(bool v)
    {
        ActiveButton = v;
    }
    
    public void SetTrainSpeed(float _speed)
    {
        Train.GetComponent<Train>().Speed = _speed;
    }

    /*public void AddTrainSpeed(float add)
    {
        speed += add;
        if (speed < initSpeed)
            speed = initSpeed;
        Train.GetComponent<Train>().Speed = speed;
        SetTrueRate();
    }

    private void SetTrueRate()
    {
        trueRate = (int)(Math.Abs(ResultsHandling.Instance.LevelTrueAnswersCounter / 2));
        if (trueRate < 2)
            trueRate = 2;
        if (trueRate > 5)
            trueRate = 5;
    }*/

    private bool GetWagonCondition(bool _first)
    {
        bool _out;
        if (WagonsConds.Count < 1)
        {
            for (int i=0;i<AllWagonsNumber;i++)
                WagonsConds.Add(false);
            if (_first)
                WagonsConds[0] = true;
            else
                WagonsConds[UnityEngine.Random.Range(0, WagonsConds.Count)] = true;
        }
        _out = WagonsConds[0];
        WagonsConds.RemoveAt(0);
        return _out;
    }

    private void SelectTrueSprites(int trueWagonsNumber)
    {
        List<int> SelectedIndex = RandomGenerator.GetRandomList(trueWagonsNumber, SelectedSprites.Length);
        TrueSprites = new List<Sprite>();
        foreach (int index in SelectedIndex)
            TrueSprites.Add(SelectedSprites[index]);

        hiddenDataEncoder.selectedImagesIndex = SelectedIndex;
    }

    private void SelectSprites(bool isabstract)
    {
        string PhotoesDirectory = "ControlResponsible/Sprites/Photos";
        int random = RandomGenerator.GetRandomNumber(7)+1;
        //Load sprites
        SelectedSprites = new List<Sprite>().ToArray();
        if (isabstract)
            SelectedSprites = Resources.LoadAll<Sprite>(PhotoesDirectory + "/abstract/"+random);
        else
            SelectedSprites = Resources.LoadAll<Sprite>(PhotoesDirectory + "/Real/" + random);

        hiddenDataEncoder.categoryIndex = random - 1;
   
    }

    internal bool IsTrueWagon()
    {
        return IsTrueWagonTurn;
    }

    internal void RemoveAllWagons()
    {
        if (Wagons.Count > 0)
            foreach (GameObject w in Wagons)
                Destroy(w);
        if (StaticWagons.Count > 0)
            foreach (GameObject w in StaticWagons)
                Destroy(w);
        Train.GetComponent<Train>().StopMoving();

        Rail.SetActive(false);
    }

    internal void StartMovingTrain()
    {
        Rail.SetActive(true);
        ActiveMovingTrain();
        Train.GetComponent<Train>().IsMoving = true;
        FixTrainPosition();
        Wagons = new List<GameObject>();
        CreateWagon();
    }

    private void ActiveMovingTrain()
    {
        StaticTrain.SetActive(false);
        Train.SetActive(true);
        WasButton.SetActive(true);
        Train.GetComponent<Train>().StartMoving();
    }

    private void FixTrainPosition()
    {
        Train.transform.localPosition = TrainPositionFactory.GetTrainInitialPosition(TempWagon);
    }

    void CreateWagon()
    {
        if (Train.GetComponent<Train>().IsMoving)
        {
            bool isAnswer = GetWagonCondition(Wagons.Count == 0);
            Sprite RandomImage;
            int randomIndex;
            if (isAnswer) 
            {
                randomIndex = RandomGenerator.GetRandomNumber(TrueSprites.Count);
                RandomImage = TrueSprites[randomIndex];
                hiddenDataEncoder.wagonsImagesIndex.Add(hiddenDataEncoder.selectedImagesIndex[randomIndex]);
            }
            else
            {
                do
                {
                    randomIndex = RandomGenerator.GetRandomNumber(SelectedSprites.Length);
                    RandomImage = SelectedSprites[randomIndex];
                } while (TrueSprites.IndexOf(RandomImage) != -1);
                hiddenDataEncoder.wagonsImagesIndex.Add(randomIndex);
            }
            GameObject lastWagon = Wagons.Count > 0 ? Wagons[Wagons.Count - 1]: MainWagon;
            Vector3 position = TrainPositionFactory.GetNewWagonPosition(lastWagon);
            GameObject newWagon = CreateNewWagon(position, Train.transform, RandomImage,Colors[colorIndex%Colors.Length], isAnswer);
            newWagon.GetComponent<Wagon>().myIndex = Wagons.Count;
            if (Wagons.Count > 2)
                newWagon.GetComponent<Wagon>().OffSprite();
            Wagons.Add(newWagon);
            colorIndex++;
            //yield return new WaitForSeconds(1/ Train.GetComponent<Train>().Speed);
        }
    }

    GameObject CreateNewWagon(Vector3 position, Transform parent, Sprite image, Sprite color= null,bool isAnswer=false )
    {
        GameObject newWagon = Instantiate(TempWagon, new Vector3(0, 0, 0), Quaternion.identity, parent);
        newWagon.transform.localPosition = position;
        newWagon.GetComponent<Wagon>().SetSprite(image);
        newWagon.GetComponent<Wagon>().IsAnswer = isAnswer;
        if(!(color is null))
            newWagon.GetComponent<Wagon>().SetColor(color);
        return newWagon;
    }
}
