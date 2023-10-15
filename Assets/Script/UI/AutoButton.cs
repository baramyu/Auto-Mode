using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour
{
    private Animator _animator;
    private Button _button;

    [SerializeField]
    private List<Quest> autoQuestList = new List<Quest>();
    private IEnumerator autoCoroutine;

    #region AnimationString
    private readonly string ANI_TRIGGER_OPEN = "Open";
    private readonly string ANI_TRIGGER_ClOSE = "Close";
    #endregion

    void Start()
    {
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayerController.instance.StartAuto);

        PlayerController.instance.onChangeNearestInteractor.AddListener(Refresh);
        PlayerController.instance.onStartAuto.AddListener(StartAuto);
        PlayerController.instance.onStop.AddListener(StopAuto);

        Refresh();
    }
    public void Refresh()
    {
        if (PlayerController.instance.GetNearestInteractor() == null && PlayerController.instance.IsInputMoveable())
        {
            _animator.ResetTrigger(ANI_TRIGGER_ClOSE);
            _animator.SetTrigger(ANI_TRIGGER_OPEN);
        }
        else
        {
            _animator.ResetTrigger(ANI_TRIGGER_OPEN);
            _animator.SetTrigger(ANI_TRIGGER_ClOSE);
        }
    }
    public void Close()
    {
        _animator.ResetTrigger(ANI_TRIGGER_OPEN);
        _animator.SetTrigger(ANI_TRIGGER_ClOSE);
    }

    private Quest GetQuestToDoNow()
    {
        Quest questToDoNow = null;
        
        foreach (var autoQuest in autoQuestList)
        {
            if (autoQuest.questState == QusetState.COMPLETE)
                continue;
            else
            {
                questToDoNow = autoQuest;
                break;
            }
        }

        return questToDoNow;
    }
    private void StartAuto()
    {
        Close();
        Quest questToDoNow = GetQuestToDoNow();
        if(questToDoNow.questState == QusetState.STANDBY)
        {
            PlayerController.instance.TryInteract(questToDoNow.questClient);
        }
        else
        {
            switch (questToDoNow.questContentType)
            {
                case QuestContentType.TALK_NPC:
                    //TODO: 구현 필요
                    break;
                case QuestContentType.COLLECT_ITEM:
                    autoCoroutine = ContinuousCollect(questToDoNow);
                    StartCoroutine(autoCoroutine);
                    break;
                case QuestContentType.HUNT_MONSTER:
                    //TODO: 구현 필요
                    break;
            }
        }
    }
    public void StopAuto()
    {
        if (autoCoroutine != null)
            StopCoroutine(autoCoroutine);
        Refresh();
    }

    private Collection FindNearestCollection(Quest collectItemQuest)
    {
        List<Item> targetItemList = collectItemQuest.questContentArray
            .Where(content => !content.IsComplete())
            .Select(content => content.questTagetItem)
            .ToList();

        List<Collection> targetCollectionList = FindObjectsOfType<Collection>()
            .Where(collection => targetItemList.Contains(collection.GetItem()))
            .ToList();

        Vector3 playerPos = PlayerController.instance.transform.position;
        targetCollectionList.Sort((col1, col2) => Vector3.Distance(playerPos, col1.position).CompareTo(Vector3.Distance(playerPos, col2.position)));

        return targetCollectionList.First();
    }
    private IEnumerator ContinuousCollect(Quest quest)
    {
        float itemPickUpwaitTime = 2f;

        while(!quest.IsCompleteableQuest())
        {
            Collection nearestCollection = FindNearestCollection(quest);
            PlayerController.instance.TryInteract(nearestCollection);
            yield return new WaitWhile(() => !nearestCollection.IsCollecting());
            yield return new WaitForSeconds(itemPickUpwaitTime);
        }

        PlayerController.instance.TryInteract(quest.questClient);
    }

}
