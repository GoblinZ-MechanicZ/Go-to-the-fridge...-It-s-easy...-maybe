using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CatAttackTest : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private GameObject catAttackUI;
    [SerializeField] private RenderTexture catAttackRT;
    [SerializeField] private float showDelay = 0.2f;

    private bool show = false;
    private IEnumerator showAttack;
    private void Start()
    {
        player.loopPointReached += OnEndOfVideo;
        showAttack = ShowCatAttack();
    }

    void Update()
    {
        if (!show && Input.GetKeyDown(KeyCode.C))
        {
            show = true;
            player.enabled = true;
            StopCoroutine(showAttack);
            showAttack = ShowCatAttack();
            StartCoroutine(showAttack);
        }
    }

    private IEnumerator ShowCatAttack()
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
    }
}
