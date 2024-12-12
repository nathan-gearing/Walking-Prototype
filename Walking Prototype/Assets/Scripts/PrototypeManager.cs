using UnityEngine;
using System.Collections;
using TMPro;

public enum DisplayMode { Mode1, Mode2, Mode3}
public class PrototypeManager : MonoBehaviour
{
    public TextMeshProUGUI messageDisplay;
    public TextMeshProUGUI timerDisplay;
    public string[] messages;
    private int currentMessageIndex = 0;
    public float delayTime = 5f;
    private Coroutine timerCoroutine;
    public DisplayMode currentMode;
    public Color[] modeColors; 
   

    void Start()
    {
        messageDisplay.text = "Please Select mode: 1, 2, or 3";
        timerDisplay.text = "";
    }
    // Update is called once per frame
    void Update()
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

    public void SetMode(DisplayMode mode)
    {
        currentMode = mode;
        messageDisplay.color = modeColors[(int)currentMode];
        messageDisplay.text = "Mode selected. Press 'A' for a timed message and 'B' for an instant message";
    }
    private IEnumerator DisplayMessageWithDelay()
    {
        float remainingTime = delayTime;
        while (remainingTime > 0)
        {
            timerDisplay.text = $"Time remaining: {remainingTime:F1} seconds";
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
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
        if (currentMessageIndex < messages.Length)
        {
            messageDisplay.text = messages[currentMessageIndex];
            messageDisplay.color = modeColors[(int)currentMode] ;
            currentMessageIndex++;
        }
        else
        {
            messageDisplay.text = "End of Messages";
        }
    }
}
