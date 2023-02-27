using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerOverlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private GameObject catAttackUI;
    [SerializeField] private RenderTexture catAttackRT;
    [SerializeField] private float showDelay = 0.2f;
    [SerializeField] private bool isCatAttack = false;
    [SerializeField] private bool isGrabSmetana = false;

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
        if (isGrabSmetana) character.OnSmetanaFound += ShowVideo;
        if (isCatAttack) character.OnAttacked += ShowVideo;
    }

    public void ShowVideo()
    {
        show = true;
        player.enabled = true;
        if (showVideo != null) { StopCoroutine(showVideo); }
        showVideo = ShowVideoEnum();
        StartCoroutine(showVideo);
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
