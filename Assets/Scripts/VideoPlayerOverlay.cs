using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerOverlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private GameObject catAttackUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float showDelay = 0.2f;
    [SerializeField] private bool isCatAttack = false;
    [SerializeField] private bool isGrabSmetana = false;
    [SerializeField] private bool isBonus = false;
    [SerializeField] private bool isWin = false;
    [SerializeField] private bool isWinBonus = false;
    [SerializeField] private bool isLose = false;
    [SerializeField] private bool hasAudio = false;

    private CharacterController character;

    private bool show = false;
    private IEnumerator showVideo;
    private void Start()
    {
        player.loopPointReached += OnEndOfVideo;
        showVideo = ShowVideoEnum();
    }

    public void SetCharacter(CharacterController characterController)
    {
        character = characterController;
        if (isGrabSmetana)
        {
            character.OnSmetanaFound += ShowVideo;
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Taking Smetana.mp4");
        }
        if (isCatAttack)
        {
            character.OnAttacked += ShowVideo;
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "CatAttack.mp4");
        }
        if (isBonus)
        {
            character.OnBonusFound += ShowVideo;
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "TakingBonus.mp4");
        }
        if (isWin)
        {
            character.OnEndGame += ShowVideo;
        }
        if (isLose)
        {
            character.OnLose += ShowVideo;
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "BadEnd.mp4");
        }
    }

    public void ShowVideo()
    {
        show = true;
        player.enabled = true;
        if (hasAudio) { audioSource.Play(); }
        if (showVideo != null) { StopCoroutine(showVideo); }
        showVideo = ShowVideoEnum();
        StartCoroutine(showVideo);
    }

    public void ShowVideo(bool hasGold)
    {
        if (isWin && !hasGold)
        {
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "victory 1.mp4");
            ShowVideo();
        }
        else if (isWinBonus && hasGold)
        {
            player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "WinwithBonuce.mp4");
            ShowVideo();
        }
    }

    public void ShowVideo(EnemyType enemyType)
    {
        if (enemyType == EnemyType.Cat)
        {
            if (character.HasSmetana)
                ShowVideo();
        }
    }

    private IEnumerator ShowVideoEnum()
    {
        yield return new WaitForSeconds(showDelay);
        catAttackUI.SetActive(true);
    }

    private void OnEndOfVideo(VideoPlayer player)
    {
        player.frame = 0;
        catAttackUI.SetActive(false);
        player.enabled = false;
        show = false;
        if (isCatAttack) character.OnAfterAttacked?.Invoke();
    }
}
