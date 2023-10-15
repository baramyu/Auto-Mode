using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeOutImage;
    [SerializeField]
    private GameObject titleCanvas;
    private Color curColor;
    private bool anyKeyDown;
    private float fadeOutSpeed = 3f;
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(titleCanvas);
        curColor = fadeOutImage.color;
    }
    void Update()
    {
        if (Input.anyKeyDown)
            StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        while(curColor.a < 1f)
        {
            curColor.a += fadeOutSpeed * Time.deltaTime;
            fadeOutImage.color = curColor;
            yield return null;
        }
        SceneManager.LoadScene(1);
        while (curColor.a > 0f)
        {
            curColor.a -= fadeOutSpeed * Time.deltaTime;
            fadeOutImage.color = curColor;
            yield return null;
        }
        Destroy(titleCanvas);
        Destroy(gameObject);
    }
}
