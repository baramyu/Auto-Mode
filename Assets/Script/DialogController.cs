using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogController : MonoBehaviour
{
    public static DialogController instance;

    [SerializeField]
    private GameObject mainSpeechBubblePanel;
    [SerializeField]
    private TMP_Text mainSpeechBubbleText;
    private readonly float startDelay = 1.0f;
    private IEnumerator mainSpeechBubbleTyping;
    [SerializeField]
    private GameObject questCompletePanel;
    void Awake()
    {
        instance = this;
    }
    public void SetMainSpeechBubble(string text)
    {
        mainSpeechBubbleTyping = MainSpeechBubbleTyping(text);
        StartCoroutine(mainSpeechBubbleTyping);
    }
    public void SetMainSpeechBubble(Quest quest)
    {
        mainSpeechBubbleTyping = MainSpeechBubbleTyping(quest);
        StartCoroutine(mainSpeechBubbleTyping);
    }
    public void ClearMainSpeechBubble()
    {
        if(mainSpeechBubbleTyping != null)
            StopCoroutine(mainSpeechBubbleTyping);
        mainSpeechBubblePanel.SetActive(false);
        mainSpeechBubbleText.text = "";
    }

    IEnumerator MainSpeechBubbleTyping(string text)
    {
        yield return new WaitForSeconds(startDelay);
 
        mainSpeechBubbleText.text = "";
        mainSpeechBubblePanel.SetActive(true);
        foreach (char letter in text.ToCharArray())
        {
            mainSpeechBubbleText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        PlayerController.instance.Stop();
    }
    IEnumerator MainSpeechBubbleTyping(Quest quest)
    {
        yield return new WaitForSeconds(startDelay);

        mainSpeechBubbleText.text = "";
        mainSpeechBubblePanel.SetActive(true);
        List<string> questScript = quest.questState == QusetState.COMPLETE ? quest.questCompleteScript : quest.questRequestScript;
        foreach (string text in questScript)
        {
            foreach (char letter in text.ToCharArray())
            {
                mainSpeechBubbleText.text += letter;
                yield return new WaitForSeconds(0.1f);
            }
            while(!Input.anyKeyDown)
            {
                yield return null;
            }
            mainSpeechBubbleText.text = "";
        }
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        if (quest.questState == QusetState.COMPLETE)
        {
            DataManager.instance.AddCompleteQuest(quest);
            questCompletePanel.SetActive(true);
        }
        else
            DataManager.instance.AddOngoingQuest(quest);
        PlayerController.instance.Stop();
    }
}
