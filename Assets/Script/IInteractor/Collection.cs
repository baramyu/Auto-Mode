
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Collection : MonoBehaviour, IInteractor
{
    #region Interactor Interface
    public string interactorName => collectName;
    public Sprite interactorImage => collectImage;
    public Vector3 position => transform.position;
    public bool IsInteractAbleDistance(Vector3 position) { return Vector3.Distance(position, transform.position) <= collectAbleDistance; }
    public void Interact()
    {
        coroutine_collect = Coroutine_Collect();
        StartCoroutine(coroutine_collect);
    }
    public void StopInteract()
    {
        if(coroutine_collect != null)
            StopCoroutine(coroutine_collect);
    }
    #endregion
    private IEnumerator coroutine_collect;

    [SerializeField]
    private string collectName = "ºÓÀº ²É";
    [SerializeField]
    private Sprite collectImage;
    [SerializeField]
    private float collectTime = 3f;
    [SerializeField]
    private float collectAbleDistance = 1f;
    [SerializeField]
    private float collectSuccessRate = 100f;
    [SerializeField]
    private string collectAnimationTriggerName = "Gathering";
    [SerializeField]
    private ItemObject collectItemObject;

    private readonly string ANI_TRIGGER_SUCCESS = "Success";
    private readonly string ANI_TRIGGER_FAIL = "Fail";


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectAbleDistance);
    }
    public void DropItem()
    {
        Vector3 dropPosition = transform.position;
        dropPosition += Vector3.forward * Random.Range(-collectAbleDistance, collectAbleDistance) ;
        dropPosition += Vector3.right * Random.Range(-collectAbleDistance, collectAbleDistance) ;
        Instantiate(collectItemObject, dropPosition, Quaternion.identity);
    }

    public bool IsCollectSuccess()
    {
        return Random.Range(0f, 100f) <= collectSuccessRate;
    }


    private IEnumerator Coroutine_Collect()
    {
        Animator playerAnimator = PlayerController.instance.GetAnimator();
        playerAnimator.SetTrigger(collectAnimationTriggerName);
        yield return new WaitForSeconds(collectTime);

        if (IsCollectSuccess())
        {
            playerAnimator.SetTrigger(ANI_TRIGGER_SUCCESS);
            DropItem();
        }
        else
            playerAnimator.SetTrigger(ANI_TRIGGER_FAIL);

        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime != 0f)
        {
            yield return null;
        }
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
        {
            yield return null;
        }
        PlayerController.instance.EndCollection();
        coroutine_collect = null;
    }

    public Item GetItem()
    {
        return collectItemObject.GetItem();
    }

    public bool IsCollecting()
    {
        return coroutine_collect != null;
    }
}
