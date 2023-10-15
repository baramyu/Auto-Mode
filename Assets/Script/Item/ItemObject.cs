using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    private Item item;
    public int number = 1;

    [SerializeField]
    private bool onEnablePickUp = true;
    private readonly float onEnablePickUpDelay = 1f;

    private Transform pickUpFlyTarget;
    private readonly float pickUpFlySpeed = 2.5f;
    private readonly float pickUpFlyFinishDistanceThreshold = 0.1f;

    private void Start()
    {
        if (onEnablePickUp)
            StartCoroutine(PickUp());
    }

    IEnumerator PickUp()
    {
        yield return new WaitForSeconds(onEnablePickUpDelay);
        pickUpFlyTarget = PlayerController.instance.center;
        while (Vector3.Distance(pickUpFlyTarget.position, transform.position) > pickUpFlyFinishDistanceThreshold)
        {
            PickUpFly();
            yield return null;
        }
        if (Inventory.instance.PickUpItem(item, number))
        {
            PlayerController.instance.ParticleSystemPlay_ItemPickUp();
            Destroy(gameObject);
        }
    }

    private void PickUpFly()
    {
        Vector3 dir = (pickUpFlyTarget.position - transform.position).normalized;
        Debug.DrawRay(transform.position, dir);
        transform.position += dir * pickUpFlySpeed * Time.deltaTime;
    }

    public Item GetItem()
    {
        return item;
    }
}
