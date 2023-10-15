using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private Inventory inventory;
    public InventorySlot[] inventorySlotArray = new InventorySlot[Inventory.MAX_ITEM_SLOT];
    public int gold = 0;

    #region PlayerInfo
    public int playerLevel = 1;
    #endregion

    #region Quest
    [SerializeField]
    private List<Quest> completeQuestList = new List<Quest>();
    [SerializeField]
    private List<Quest> ongoingQuestList = new List<Quest>();
    #endregion

    private void Awake()
    {
        instance = this;
    }


    public int GetPlayerLevel()
    {
        return playerLevel;
    }
    public List<Quest> GetCompleteQuestList()
    {
        return completeQuestList;
    }
    public void AddOngoingQuest(Quest quest)
    {
        quest.questState = QusetState.ONGOING;
        ongoingQuestList.Add(quest);
        //TEST
        Debug.Log(quest.questClient.transform.position);
        //TESTEND
        UIManager.instance.AddQuest(quest);
    }
    public void AddCompleteQuest(Quest quest)
    {
        ongoingQuestList.Remove(quest);
        completeQuestList.Add(quest);
        UIManager.instance.CompleteQuest(quest);
    }
}