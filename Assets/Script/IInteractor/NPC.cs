using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NPC : MonoBehaviour, IInteractor
{
    #region Interactor Interface
    public string interactorName => npcName;
    public Sprite interactorImage => talkImage;
    public Vector3 position => transform.position;
    public bool IsInteractAbleDistance(Vector3 position) { return Vector3.Distance(position, transform.position) <= talkableDistance; }
    public void Interact()
    {
        npcCamera.Priority = 11;
        string randomGreeting = greeting[Random.Range(0, greeting.Length)];


        List<Quest> completeableQuestList = GetCompleteableQuestList();
        List<Quest> startableQuestList = GetStartableQuestList();
        if(completeableQuestList.Count > 0)
        {
            completeableQuestList[0].Complete();
            DialogController.instance.SetMainSpeechBubble(completeableQuestList[0]);
        }
        else if (startableQuestList.Count > 0)
        {
            DialogController.instance.SetMainSpeechBubble(startableQuestList[0]);
        }
        else
        {
            DialogController.instance.SetMainSpeechBubble(randomGreeting);
        }
    }
    public void StopInteract()
    {
        npcCamera.Priority = 9;
        DialogController.instance.ClearMainSpeechBubble();
    }
    #endregion

    [SerializeField]
    private List<Quest> questList;

    [SerializeField]
    private string npcName = "타르라크";
    [SerializeField]
    private string[] greeting = { "안녕하세요." };
    [SerializeField]
    private Sprite talkImage;
    [SerializeField]
    private float talkableDistance = 3f;
    [SerializeField]
    private CinemachineVirtualCamera npcCamera;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, talkableDistance);
    }

    private List<Quest> GetStartableQuestList()
    {
        List<Quest> startableQuestList = questList.FindAll(quest => quest.IsStartableQuest());

        return startableQuestList;
    }
    private List<Quest> GetCompleteableQuestList()
    {
        List<Quest> completeableQuestList = questList.FindAll(quest => quest.IsCompleteableQuest());

        return completeableQuestList;
    }

    void Start()
    {
        foreach(var quest in questList)
        {
            quest.questClient = this;
        }
    }
}