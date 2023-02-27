using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerURLSet : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private string fileName;

    void Start()
    {
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
    }
}
