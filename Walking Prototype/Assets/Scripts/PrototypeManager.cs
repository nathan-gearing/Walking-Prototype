using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum DisplayMode { Mode1, Mode2, Mode3}
public class PrototypeManager : MonoBehaviour
{
    public TextMeshProUGUI messageDisplay;
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI restartText;
    public TMP_InputField timerInputField;
    public TMP_InputField userInputField;
    public Button submitButton;
    public Button timerSubmitButton;
    private List<string> userMessages = new List<string>();
    private int currentMessageIndex = 0;
    public float delayTime = 5f;
    private Coroutine timerCoroutine;
    public DisplayMode currentMode;
    public Dictionary<DisplayMode, Color> modeColors;

    private bool modeSelected = false;
    private bool readyStartDisplaying = false;
    

    void Start()
    {
        messageDisplay.text = "Please Select mode: 1, 2, or 3";
        timerDisplay.text = "";
        timerInputField.text = delayTime.ToString("F2");

        modeColors = new Dictionary<DisplayMode, Color>
        {
            { DisplayMode.Mode1, Color.red },
            { DisplayMode.Mode2, Color.green },
            { DisplayMode.Mode3, Color.blue },
        };

        submitButton.onClick.AddListener(OnSubmitButtonClicked);
        timerSubmitButton.onClick.AddListener(OnTimerSubmitClicked);
        userInputField.gameObject.SetActive(false);
        timerSubmitButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        timerInputField.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || (Input.GetKey(KeyCode.RightControl))) 
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("WalkingPrototype");
            }
        }
        if (!modeSelected)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetMode(DisplayMode.Mode1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetMode(DisplayMode.Mode2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetMode(DisplayMode.Mode3);
            }
        }
        if (modeSelected && userMessages.Count > 1 && Input.GetKeyDown(KeyCode.Tab))
        {
            messageDisplay.text = "Ready to begin. Press A for a delayed message and B for an instant message";
            timerDisplay.text = "";
            readyStartDisplaying = true;
            submitButton.gameObject.SetActive(false);
            timerSubmitButton.gameObject .SetActive(false);
            userInputField.gameObject.SetActive(false);
            restartText.gameObject.SetActive(false);
            timerInputField.gameObject.SetActive(false);
        }
        if (readyStartDisplaying)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine);
                }
                timerCoroutine = StartCoroutine(DisplayMessageWithDelay());
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DisplayInstantMessage();
            }
        }
    }

    public void SetMode(DisplayMode mode)
    {
        currentMode = mode;
        modeSelected = true;
        if (modeColors.ContainsKey(currentMode))
        {
            messageDisplay.color = modeColors[currentMode];
        }
        messageDisplay.text = "Mode selected. Please enter your messages below";
        timerDisplay.text = "Please Set timer below";
        userInputField.gameObject.SetActive(true);
        timerInputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        timerSubmitButton.gameObject.SetActive(true);
        
    }

    private void OnTimerSubmitClicked()
    {
        if (!string.IsNullOrEmpty(timerInputField.text))
        {
            if (float.TryParse(timerInputField.text, out float parsedTime))
            {
                if (parsedTime > 0)
                {
                    delayTime = parsedTime;
                    timerInputField.text = delayTime.ToString("F2");
                    timerDisplay.text = "Valid Time";
                }
                else
                {
                    timerDisplay.text = "Invaid timer duration. Please enter a value greater than 0";
                }
            }
            else
            {
                timerDisplay.text = "Invalid timer duration. Please enter a valid number";
            }
        }
        else
        {
            timerInputField.text = delayTime.ToString("F2");
        }
    }
    private void OnSubmitButtonClicked()
    {
        if (!string.IsNullOrEmpty(userInputField.text))
        {
            userMessages.Add(userInputField.text);
            userInputField.text = "";
        }
        messageDisplay.text = $"Message {userMessages.Count} entered. Enter another message or press 'Tab' to begin";
        
    }
    private IEnumerator DisplayMessageWithDelay()
    {
        float remainingTime = delayTime;
        while (remainingTime > 0)
        {
            timerDisplay.text = $"Time remaining: {remainingTime:F2} seconds";
            yield return new WaitForSeconds(0.01f);
            remainingTime -= 0.01f;
        }
        timerDisplay.text = "";
        DisplayNextMessage();
    }

    private void DisplayInstantMessage()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerDisplay.text = "";
        }
        DisplayNextMessage();
    }
    private void DisplayNextMessage()
    {
        if (currentMessageIndex < userMessages.Count)
        {
            messageDisplay.text = userMessages[currentMessageIndex];
            if (modeColors.ContainsKey(currentMode))
            {
                messageDisplay.color = modeColors[currentMode];
            }
            currentMessageIndex++;
        }
        else
        {
            messageDisplay.text = "End of Messages";
        }
    }
}
