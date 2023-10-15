using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text contentTextPrefab;

    private Quest quest;
    private List<TMP_Text> contentTextList = new List<TMP_Text>();

    public void Init(Quest quest)
    {
        titleText.text = quest.questName;
        List<string> contentTextList = quest.GetContentStringList();
        this.quest = quest;

        for (int i = 0; i < contentTextList.Count; i++)
        {
            TMP_Text contentText = Instantiate(contentTextPrefab, transform);
            contentText.text = contentTextList[i];
            this.contentTextList.Add(contentText);
            Debug.Log(i);
            Debug.Log(contentTextList[i]);
        }

        Inventory.instance.onChangeItem.AddListener(Refresh);
    }
    public void Refresh()
    {
        List<string> contentTextList = quest.GetContentStringList();

        for (int i = 0; i < this.contentTextList.Count; i++)
        {
            this.contentTextList[i].text = contentTextList[i];
            Debug.Log(i);
            Debug.Log(contentTextList[i]);
        }
    }

    public Quest GetQuest()
    {
        return quest;
    }
}
