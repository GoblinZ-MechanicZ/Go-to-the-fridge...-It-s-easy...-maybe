using UnityEngine;

[System.Serializable]
public struct RoomDirections
{
    public bool forward;
    public bool backward;
    public bool left;
    public bool right;
}

public class RoomBlock : MonoBehaviour
{

    public RoomDirections RoomDirections { get { return _roomDirections; } }
    public LevelMaker.MazeBlockState BlockState { get { return mazeBlockInfo.getState(); } }

    public bool IsStart { get { return _isStart; } }
    public bool IsFinish { get { return _isFinish; } }

    public bool HasEnemy { get { return _hasEnemy; } }
    public bool HasBonus { get { return _hasBonus; } }

    private LevelMaker.MazeBlockInfo mazeBlockInfo;
    private RoomDirections _roomDirections;
    private bool _hasBonus;
    private bool _hasEnemy;
    private bool _isFinish;
    private bool _isStart;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Player Enter");
        }
        else if (collider.tag == "Enemy")
        {
            Debug.Log("Enemy Enter");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Player Exit");
        }
        else if (collider.tag == "Enemy")
        {
            Debug.Log("Enemy Exit");
        }
    }

    public void InitRoom(LevelMaker.MazeBlockInfo info)
    {
        mazeBlockInfo = info;

        _isStart = mazeBlockInfo.getState() == LevelMaker.MazeBlockState.Start;
        _isFinish = mazeBlockInfo.getState() == LevelMaker.MazeBlockState.Finish;

        var transitions = mazeBlockInfo.getTransition();
        for (int i = 0; i < transitions.Count; i++)
        {
            var vec = transitions[i];
            if (vec.x < 0) _roomDirections.left = true;
            else if (vec.x > 0) _roomDirections.right = true;
            if (vec.y > 0) _roomDirections.forward = true;
            else if (vec.y < 0) _roomDirections.backward = true;
        }

    }
}