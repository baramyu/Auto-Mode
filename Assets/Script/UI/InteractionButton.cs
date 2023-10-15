using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    private Animator _animator;
    private Image _image;
    private Button _button;

    private bool opened;

    #region AnimationString
    private readonly string ANI_TRIGGER_OPEN = "Open";
    private readonly string ANI_TRIGGER_ClOSE = "Close";
    private readonly string ANI_TRIGGER_CHANGE = "Change";
    #endregion

    #region DefalutImages
    [SerializeField]
    private Sprite stopImage;
    #endregion

    IInteractor currentInteractor;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayerController.instance.TryInteract);
        PlayerController.instance.onChangeNearestInteractor.AddListener(RefreshButton);
        PlayerController.instance.onStop.AddListener(RefreshButton);
        PlayerController.instance.onTryInteract.AddListener(Close);
    }

    private void RefreshButton()
    {
        if (!PlayerController.instance.IsInteractable())
        {
            Close();
            return;
        }
        if (PlayerController.instance.GetNearestInteractor() == null)
        {
            Close();
            currentInteractor = null;
            return;
        }

        if (currentInteractor != PlayerController.instance.GetNearestInteractor() || !IsOpened())
        {
            currentInteractor = PlayerController.instance.GetNearestInteractor();
            _image.sprite = currentInteractor.interactorImage;
            Open();
        }

    }

    public void Open()
    {
        if (opened == true)
        {
            _animator.SetTrigger(ANI_TRIGGER_CHANGE);
            return;
        }
        _animator.SetTrigger(ANI_TRIGGER_OPEN);
        opened = true;
    }

    public void Close()
    {
        if (opened == false)
            return;
        _animator.SetTrigger(ANI_TRIGGER_ClOSE);
        opened = false;
    }
    public bool IsOpened()
    {
        return opened;
    }
}
