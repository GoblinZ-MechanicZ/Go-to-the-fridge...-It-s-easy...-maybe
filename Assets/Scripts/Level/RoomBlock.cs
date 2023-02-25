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
    public LevelMaker.MazeBlockState BlockState { get { return mazeBlockInfo.getState(); }}

    private LevelMaker.MazeBlockInfo mazeBlockInfo;
    private RoomDirections _roomDirections;

    public void InitRoom(LevelMaker.MazeBlockInfo info)
    {
        mazeBlockInfo = info;
        
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