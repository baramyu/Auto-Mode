using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopButton : MonoBehaviour
{
    private Animator _animator;
    private Button _button;

    #region AnimationString
    private readonly string ANI_TRIGGER_OPEN = "Open";
    private readonly string ANI_TRIGGER_ClOSE = "Close";
    #endregion

    void Start()
    {
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayerController.instance.Stop);
        PlayerController.instance.onTryInteract.AddListener(Open);
        PlayerController.instance.onStartAuto.AddListener(Open);
        PlayerController.instance.onStop.AddListener(Close);
    }

    public void Open()
    {
        _animator.ResetTrigger(ANI_TRIGGER_ClOSE);
        _animator.SetTrigger(ANI_TRIGGER_OPEN);
    }
    public void Close()
    {
        _animator.ResetTrigger(ANI_TRIGGER_OPEN);
        _animator.SetTrigger(ANI_TRIGGER_ClOSE);
    }
}
