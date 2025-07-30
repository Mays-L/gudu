using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Central game controller (singleton)
public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance { get; private set; }

    public GameSession currentSession = new GameSession();

    private StageData currentStage;
    private CrossroadChoice currentChoice;

    // dialoug help UI
    [SerializeField] private UnityEngine.UI.Button animalButton;
    [SerializeField] private UnityEngine.UI.Image animalImage;

    // data loaded from Resources/DialogueStageData.json
    private List<DialogueStageData> stageDataList;

    // state tracking
    private int currentStageIndex;
    private bool initialDialogShown;

    void Awake()
    {
        // Ensure only one instance persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // load dialog & image info once at startup
        TextAsset json = Resources.Load<TextAsset>("Jsons/DialogueStageData");
        var wrapper = JsonUtility.FromJson<DialogueStageDataList>(json.text);
        stageDataList = wrapper.stages;
    }

    void Start()
    {
        // Automatically start the first stage
        StartStage(1);
    }

    // Begin a new stage
    public void StartStage(int stageIndex)
    {
        currentStageIndex = stageIndex;
        Debug.Log("in StartStage and currentStageIndex : " + currentStageIndex);
        initialDialogShown = false;

        currentStage = new StageData
        {
            stageIndex = stageIndex,
            stageStartTime = Time.time
        };

        // setup the animal button & image for this stage
        var data = stageDataList.Find(d => d.stageIndex == stageIndex);
        Sprite sprite = Resources.Load<Sprite>("MonstersSprite/" + data.imageName);
        animalImage.sprite = sprite;

        animalButton.onClick.RemoveAllListeners();
        animalButton.onClick.AddListener(()=>TriggerStageDialogue(stageIndex));
        animalButton.gameObject.SetActive(true);
    }

    // Called when dialog needs to show at crossroad center
    public void EnterCrossroad(int crossroadIndex)
    {
        // test
        //currentChoice = new CrossroadChoice
        //{
        //    crossroadIndex = crossroadIndex,
        //    entryPosition = Player.Instance.transform.position,
        //    responseStartTime = Time.time
        //};

        //// Show dialog (message chosen inside DialogueUI)
        //DialogueUI.Instance.Show(
        //    "Choose your path!"
        //);

        Debug.Log("initialDialogShown : " + initialDialogShown + "\n crossroadIndex : " + crossroadIndex + "\n currentStageIndex :" + currentStageIndex);
        if (!initialDialogShown && crossroadIndex == currentStageIndex)
        {
            currentChoice = new CrossroadChoice
            {
                crossroadIndex = crossroadIndex,
                entryPosition = Player.Instance.transform.position,
                responseStartTime = Time.time
            };
            TriggerStageDialogue(crossroadIndex);
        }
    }



    // Records the choice when player enters a path collider
    public void RecordDirection(int crossroadIndex, Direction dir, bool isCorrect)
    {
        currentChoice.chosenDirection = dir;
        currentChoice.isCorrect = isCorrect;
        currentChoice.responseTime = Time.time - currentChoice.responseStartTime;

        currentStage.choices.Add(currentChoice);
    }

    // Called when player reaches the dead-end return-trigger
    //public void ReturnToCenter(Vector2 centerPosition)
    //{
    //    Player.Instance.transform.position = centerPosition;
    //    // test2
    //    //Player.Instance.CanMove = false;
    //    // show dialog again if needed, or re-enable movement
    //    DialogueUI.Instance.Show("Try again!");
    //}

    // Called when player reaches the correct house
    public void FinishStage()
    {
        currentStage.stageEndTime = Time.time;
        currentSession.allStages.Add(currentStage);
        Debug.Log("You Won");
        animalButton.gameObject.SetActive(false);
        //SceneManager.LoadScene("MiniGameScene");

        // temp: for test
        ReturnFromMiniGame();
    }

    // Called by the mini-game when it ends
    public void ReturnFromMiniGame()
    {
        Debug.Log("in ReturnFromMiniGame");
        int nextStage = currentStage.stageIndex + 1;
        if (nextStage <= 4)
        {
            //SceneManager.LoadScene("MainScene");
            StartStage(nextStage);
        }
        else
        {
            // TODO: show final summary or end-game UI
        }
    }

    #region Dialouge Methods 
    // show the stage-specific dialog (called on first enter OR on button click)
    public void TriggerStageDialogue(int crossroadIndex)
    {
        var data = stageDataList.Find(d => d.stageIndex == currentStageIndex);
        // test2 
        //Player.Instance.CanMove = false;
        DialogueUI.Instance.Show(data.message, crossroadIndex);
        initialDialogShown = true;
    }

    // Called by DialogueUI when OK pressed
    public void OnDialogOk()
    {
        DialogueUI.Instance.Hide();
        // test
        Player.Instance.CanMove = true;

        // Start listening for direction trigger colliders instead of joystick
        // No direct movement watcher here anymore.
    }

    #endregion
}
