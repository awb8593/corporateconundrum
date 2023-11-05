using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine.Events;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    SpVoice voice = new SpVoice();
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        // Subscribe to the PointerEnter event
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { SpeakButtonText(); });
        trigger.triggers.Add(entry);
    }

    private void OnDisable()
    {
        // Unsubscribe from the PointerEnter event
        EventTrigger trigger = GetComponent<EventTrigger>();
        trigger.triggers.Clear();
    }
    private void SpeakButtonText()
    {
        string textToSpeak = buttonText.text;

        if (!string.IsNullOrEmpty(textToSpeak))
        {
            voice.Rate = -2;
            voice.Speak(textToSpeak, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }
    }
}
