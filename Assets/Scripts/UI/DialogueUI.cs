using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Singleton UI manager for showing/hiding dialog with an OK button
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RTLTMPro.RTLTextMeshPro messageText;
    [SerializeField] private Button okButton;
    [SerializeField] private GameObject dialougPanel;
    [SerializeField] private Image bubleRenderer;
    [SerializeField] private Image friendRenderer;



    private bool isDialogueOpen = false;
    [SerializeField] Sprite _buble1;
    [SerializeField] Sprite _buble2;
    [SerializeField] Sprite _buble3;
    [SerializeField] Sprite _buble4;

    [SerializeField] Sprite _friend1;
    [SerializeField] Sprite _friend2;
    [SerializeField] Sprite _friend3;
    [SerializeField] Sprite _friend4;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Hide();
    }
    private void Start()
    {
        
    }

    // Show the dialog with the given message
    public void Show(string message,int crossroadIndex)
    {
        // Prevent opening several time at a while
        if(isDialogueOpen) return;
        isDialogueOpen = true;
        switch (crossroadIndex)
        {
            case 0:
                break;
            case 1:
                bubleRenderer.sprite = _buble1;
                friendRenderer.sprite = _friend1;
                break;
            case 2:
                bubleRenderer.sprite = _buble2;
                friendRenderer.sprite = _friend2;
                break;
            case 3:
                bubleRenderer.sprite = _buble3;

                friendRenderer.sprite = _friend3;
                break;
            case 4:
                friendRenderer.sprite = _friend4;
                bubleRenderer.sprite = _buble4;

                break;
        }
        StartCoroutine(TypeDialogueText(message));



    }
    private IEnumerator TypeDialogueText(string p)
    {
        messageText.text = "";  // clear at start

        // while we haven't shown all characters
        //while (true)
        //{
        //    // advance time
        //    elapsedTime += Time.deltaTime;

        //    // calculate how many chars to show based on speed
        //    // e.g. typeSpeed = characters per second
        //    int charIndex = Mathf.FloorToInt(elapsedTime * 0.01f);

        //    // clamp between 0 and full length
        //    charIndex = Mathf.Clamp(charIndex, 0, p.Length);

        //    // update the visible substring
        //    messageText.text = p.Substring(0, charIndex);

        //    // if we've shown all, break
        //    if (charIndex >= p.Length)
        //        break;

        //    yield return null;
        //}
        canvasGroup.alpha = 1f;
        dialougPanel.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        for (int i = 1; i <= p.Length; i++)
        {
            // Set substring from 0 to i
            messageText.text = p.Substring(0, i);

            // Wait before next character
            yield return new WaitForSeconds(0.08f);
        }
        // ensure full text at end
        messageText.text = p;


        okButton.onClick.AddListener(OnOkClicked);
    }
    // Hide the dialog
    public void Hide()
    {
        isDialogueOpen = false;
        Debug.Log("nt- dialoge hide");
        okButton.onClick.RemoveListener(OnOkClicked);
        Player.Instance.CanMove = true;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        dialougPanel.SetActive(false);

    }

    // Called when OK button is clicked
    private void OnOkClicked()
    {
        Hide();
        MainGameManager.Instance.OnDialogOk();
    }
}
