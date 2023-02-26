using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Screens: ")]
    [SerializeField] private GMenuItem mainMenu;
    [SerializeField] private GMenuItem newGame;
    [SerializeField] private GMenuItem settings;
    [SerializeField] private GMenuItem about;
    [SerializeField] private GMenuItem loadingBar;

    [Header("Fade Effect")]
    [SerializeField] private Image fader;
    [SerializeField] private Color fadeColor;
    [SerializeField] private Color showColor;
    [SerializeField] private float transitionScreenTime = 1f;

    [Header("Settings")]
    [SerializeField] private GlobalSettingsScriptable globalSettings;

    [Header("LoadingBar")]
    [SerializeField] private Slider loadingBarSlider;

    private IEnumerator changeScreenEnum;

    private void Start() {
        ChangeToMain();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameEnum());
    }

    private IEnumerator StartGameEnum()
    {
        loadingBarSlider.value = 0f;
        yield return ChangeScreenEnum(loadingBar);
        var asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");

        float progress = 0f;
        while (!asyncOp.isDone)
        {
            progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            loadingBarSlider.value = 100f * progress;
            yield return null;
        }

    }

    public void ChangeScreen(GMenuItem newScreen)
    {
        if (changeScreenEnum != null) { StopCoroutine(changeScreenEnum); }

        changeScreenEnum = ChangeScreenEnum(newScreen);
        StartCoroutine(changeScreenEnum);
    }

    public void ChangeSubScreen(GMenuItem newScreen, List<GMenuItem> subScreens)
    {
        if (changeScreenEnum != null) { StopCoroutine(changeScreenEnum); }

        changeScreenEnum = ChangeScreenEnum(newScreen, subScreens);
        StartCoroutine(changeScreenEnum);
    }

    public void ChangeToMain()
    {
        ChangeScreen(mainMenu);
    }

    private IEnumerator ChangeScreenEnum(GMenuItem newScreen)
    {
        yield return FadeIn();
        mainMenu.gameObject.SetActive(false);
        newGame.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        about.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(false);

        newScreen.gameObject.SetActive(true);
        yield return FadeOut();
    }

    private IEnumerator ChangeScreenEnum(GMenuItem newScreen, List<GMenuItem> allScreens)
    {
        yield return FadeIn();
        foreach (var item in allScreens)
        {
            item.gameObject.SetActive(false);
        }

        newScreen.gameObject.SetActive(true);
        yield return FadeOut();
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer <= transitionScreenTime / 2)
        {
            timer += Time.unscaledDeltaTime;
            fader.color = Color.Lerp(showColor, fadeColor, timer / (transitionScreenTime / 2));
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer <= transitionScreenTime / 2)
        {
            timer += Time.unscaledDeltaTime;
            fader.color = Color.Lerp(fadeColor, showColor, timer / (transitionScreenTime / 2));
            yield return new WaitForEndOfFrame();
        }
    }
}
