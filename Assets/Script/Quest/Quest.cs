using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    MAIN,
    NORMAL,
    EVENT,
    REPEAT
}
public enum QusetState
{
    STANDBY,
    ONGOING,
    COMPLETE
}

public enum QuestContentType
{
    TALK_NPC,
    COLLECT_ITEM,
    HUNT_MONSTER
}
[System.Serializable]
public class QuestContent
{
    //TODO: IQuestTarget 혹은 GUI 개선을 통한 타입 별 Target 노출 필요
    //public IQuestTarget questTaget;
    public NPC questTagetNPC;
    public Item questTagetItem;
    public Monster questTagetMonster;
    public int targetNumber;
    public bool IsComplete() { return targetNumber <= Inventory.instance.hasItem(questTagetItem); }
}
[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    [Header("퀘스트 정보")]
    public QuestType questType;
    public QusetState questState;
    public string questName;
    public NPC questClient;

    [Header("퀘스트 선행 조건")]
    public int playerLevel;
    public List<Quest> preQuestList;

    [Header("퀘스트 내용")]
    public QuestContentType questContentType;
    public QuestContent[] questContentArray;
    public List<string> questRequestScript;
    public List<string> questCompleteScript;
    public List<string> GetContentStringList()
    {
        List<string> contentStringList = new List<string>();

        foreach (var questContent in questContentArray)
        {
            switch (questContentType)
            {
                case QuestContentType.TALK_NPC:
                    //TODO: contentStringList.Add($"{questContent.questTagetNPC.interactorName} 와(과) 대화하기");
                    break;
                case QuestContentType.COLLECT_ITEM:
                    contentStringList.Add($"{questContent.questTagetItem.itemName} 전달하기 ({Mathf.Min(Inventory.instance.hasItem(questContent.questTagetItem), questContent.targetNumber)}/{questContent.targetNumber})");
                    break;
                case QuestContentType.HUNT_MONSTER:
                    //TODO: contentStringList.Add($"{questContent.questTagetMonster.monsterName} 사냥하기 ({Mathf.Min(questContent.currentNumber, questContent.targetNumber)}/{questContent.targetNumber})");
                    break;
            }
        }

        return contentStringList;
    }

    public bool IsStartableQuest()
    {
        if (questState != QusetState.STANDBY)
            return false;
        if (DataManager.instance.GetPlayerLevel() < playerLevel)
            return false;
        
        foreach(Quest quest in preQuestList)
            if (!DataManager.instance.GetCompleteQuestList().Contains(quest))
                return false;
        
        return true;
    }
    public bool IsCompleteableQuest()
    {
        if (questState != QusetState.ONGOING)
            return false;

        foreach(QuestContent questContent in questContentArray)
        {
            switch(questContentType)
            {
                case QuestContentType.COLLECT_ITEM:
                    if(Inventory.instance.hasItem(questContent.questTagetItem) < questContent.targetNumber)
                        return false;
                    break;
                case QuestContentType.HUNT_MONSTER:
                    //TODO: 구현예정
                    return false;
                case QuestContentType.TALK_NPC:
                    //TODO: 구현예정
                    return false;
            }
        }

        return true;
    }
    public void Complete()
    {
        questState = QusetState.COMPLETE;

        foreach (QuestContent questContent in questContentArray)
        {
            switch (questContentType)
            {
                case QuestContentType.COLLECT_ITEM:
                    Inventory.instance.CutdownItem(questContent.questTagetItem, questContent.targetNumber);
                    break;
                case QuestContentType.HUNT_MONSTER:
                    //TODO: 구현예정
                    break;
                case QuestContentType.TALK_NPC:
                    //TODO: 구현예정
                    break;
            }
        }
    }
}
