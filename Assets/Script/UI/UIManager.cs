using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private InteractionButton interactionButton;
    [SerializeField]
    private StopButton stopButton;

    [SerializeField]
    private QuestPanel questPanelPrefab;
    private List<QuestPanel> questPanelList = new List<QuestPanel>();

    [SerializeField]
    private Transform questListPanel;



    void Start()
    {
        instance = this;
    }

    public void AddQuest(Quest quest)
    {
        QuestPanel questPanel = Instantiate(questPanelPrefab, questListPanel);
        questPanel.Init(quest);
        questPanelList.Add(questPanel);
    }
    public void CompleteQuest(Quest quest)
    {
        QuestPanel completeQuestPanel = questPanelList.Find(questPanel => questPanel.GetQuest() == quest);
        Destroy(completeQuestPanel.gameObject);
        questPanelList.Remove(completeQuestPanel);
    }
}
