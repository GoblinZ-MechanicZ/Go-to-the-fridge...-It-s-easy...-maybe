using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{
    public enum MazeBlockState
    {
        Null,
        Empty,
        Border,
        Room,
        Path,
        Start,
        Finish

    }
    public class IntVector2
    {
        public int x { get; set; }
        public int y { get; set; }

        public IntVector2(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    public class MazeBlockInfo
    {
        public string name;
        public IntVector2 adress;
        MazeBlockState State;
        List<IntVector2> transition;

        public MazeBlockInfo(int _x, int _y, MazeBlockState _state)
        {
            adress = new IntVector2(_x, _y);

            State = _state;
            transition = new List<IntVector2>();

        }

        public IntVector2 getAdress()
        {
            return adress;
        }

        public MazeBlockState getState()
        {
            return State;
        }

        public void setState(MazeBlockState _state)
        {
            State = _state;
        }

        public List<IntVector2> getTransition()
        {
            return transition;
        }

        public bool removeTransition(IntVector2 room)
        {
            return transition.Remove(room);
        }
        public int addTransition(IntVector2 room)
        {
            transition.Add(room);
            return transition.Count - 1;
        }
        public void clearTransition()
        {
            transition.Clear();
        }

    }

    public class Maze
    {
        MazeBlockInfo[,] mazeMatrix;
        IntVector2 curPos = null;
        float fillValue = 0.75f;
        int maxPathItteration = 10;

        //!!!!!!!!!!!!!!!
        // List<IntVector2> transition;
        //!!!!!!!!!!!!!!!!


        public Maze(IntVector2 maxSize, float fill)
        {
            createMaze(maxSize, fill);
        }

        public Maze(int maxSizeX, int maxSizeY, float fill)
        {
            createMaze(new IntVector2(maxSizeX, maxSizeY), fill);
        }

        void createMaze(IntVector2 maxSize, float fill)
        {

            maxPathItteration = (int)((maxSize.x * maxSize.y) * fill);
            if (mazeMatrix != null) mazeMatrix = null;
            mazeMatrix = new MazeBlockInfo[maxSize.x, maxSize.y];
            ClearMatrix();
            MakeBorder();
            MakeStartPosition();
            MakePath();
        }

        public MazeBlockInfo getBlock(IntVector2 adress)
        {
            if (adress.x >= 0 || adress.y >= 0 | adress.x < mazeMatrix.GetLength(0) || adress.y < mazeMatrix.GetLength(1)) return mazeMatrix[adress.x, adress.y];
            else return null;
        }

        public void ClearMatrix()
        {
            for (int y = 0; y < mazeMatrix.GetLength(1); y++)
                for (int x = 0; x < mazeMatrix.GetLength(0); x++)
                {
                    mazeMatrix[x, y] = new MazeBlockInfo(x, y, MazeBlockState.Empty);
                    mazeMatrix[x, y].clearTransition();
                }
        }

        void MakeBorder()
        {
            for (int y = 0; y < mazeMatrix.GetLength(1); y++)
            {
                mazeMatrix[mazeMatrix.GetLength(0) - 1, y].setState(MazeBlockState.Border);
                mazeMatrix[0, y].setState(MazeBlockState.Border);
            }
            for (int x = 0; x < mazeMatrix.GetLength(0); x++)
            {
                mazeMatrix[x, mazeMatrix.GetLength(1) - 1].setState(MazeBlockState.Border);
                mazeMatrix[x, 0].setState(MazeBlockState.Border);
            }
        }

        void MakeStartPosition()
        {

            IntVector2 pos = new IntVector2(0, 0);
            if ((int)UnityEngine.Random.Range(0, 1) == 0)
            {
                pos.x = (int)UnityEngine.Random.Range(1, mazeMatrix.GetLength(0) - 1);
                pos.y = (int)(UnityEngine.Random.Range(0, 1)) * (mazeMatrix.GetLength(1) - 1);
            }
            else
            {
                pos.y = (int)UnityEngine.Random.Range(1, mazeMatrix.GetLength(1) - 1);
                pos.x = ((int)(UnityEngine.Random.Range(0, 1))) * (mazeMatrix.GetLength(0) - 1);
            }

            pos.x = 5;
            pos.y = 0;

            mazeMatrix[pos.x, pos.y].setState(MazeBlockState.Room);




            if ((pos.y == 0 || pos.y == mazeMatrix.GetLength(1) - 1) && pos.x > 0 && pos.x < mazeMatrix.GetLength(0) - 1)
            {
                //Vert
                if ((pos.y - 1) > 0 && mazeMatrix[pos.x, pos.y - 1].getState() == MazeBlockState.Empty)
                    mazeMatrix[pos.x, pos.y].addTransition(new IntVector2(0, -1));

                if ((pos.y + 1) < mazeMatrix.GetLength(1) - 1 && mazeMatrix[pos.x, pos.y + 1].getState() == MazeBlockState.Empty)
                    mazeMatrix[pos.x, pos.y].addTransition(new IntVector2(0, 1));

            }
            if ((pos.x == 0 || pos.x == mazeMatrix.GetLength(0) - 1) && pos.y > 0 && pos.y < mazeMatrix.GetLength(1) - 1)
            {
                //Hor
                if ((pos.x - 1) > 0 && mazeMatrix[pos.x - 1, pos.y].getState() == MazeBlockState.Empty)
                    mazeMatrix[pos.x, pos.y].addTransition(new IntVector2(-1, 0));

                if ((pos.x + 1) < mazeMatrix.GetLength(0) - 1 && mazeMatrix[pos.x + 1, pos.y].getState() == MazeBlockState.Empty)
                    mazeMatrix[pos.x, pos.y].addTransition(new IntVector2(1, 0));

            }
            mazeMatrix[pos.x, pos.y].name = "start";
            // mazeMatrix[pos.x, pos.y].setState(MazeBlockState.Start);
            curPos = new IntVector2(pos.x, pos.y);
            Debug.Log("make start position complite. start pos : [" + pos.x + "," + pos.y + "]; transition = " + mazeMatrix[pos.x, pos.y].getTransition().Count + " >  [" + mazeMatrix[pos.x, pos.y].getTransition()[0].x + "," + mazeMatrix[pos.x, pos.y].getTransition()[0].y + "]");
        }

        void MakePath()
        {

            int _i = 0;
            List<IntVector2> path = new List<IntVector2>();
            path.Add(curPos);

            //if (curPos == null) return;

            for (int inc = maxPathItteration; inc > 0; inc--)
            {
                Debug.Log("make path 1");
                List<IntVector2> variantsPath = new List<IntVector2>();
                // if (curPos.x - 1 >= 0 && curPos.x + 1 < mazeMatrix.GetLength(0))
                {
                    if ((curPos.x - 1 > 0 && curPos.x < mazeMatrix.GetLength(0)) && (mazeMatrix[curPos.x - 1, curPos.y].getState() == MazeBlockState.Empty))
                    {
                        variantsPath.Add(new IntVector2(-1, 0));
                    }

                    if ((curPos.x + 1 < mazeMatrix.GetLength(0) - 1) && (curPos.x > 0) && (mazeMatrix[curPos.x + 1, curPos.y].getState() == MazeBlockState.Empty))
                    {
                        variantsPath.Add(new IntVector2(1, 0));
                    }
                }
                // if ((curPos.y - 1 >= 0) && curPos.y < mazeMatrix.GetLength(1))
                {

                    if ((curPos.y - 1 >= 0 && curPos.y < mazeMatrix.GetLength(1) - 1) && (mazeMatrix[curPos.x, curPos.y - 1].getState() == MazeBlockState.Empty))
                    {
                        variantsPath.Add(new IntVector2(0, -1));
                    }

                    if ((curPos.y + 1 < mazeMatrix.GetLength(1) - 1 && curPos.y >= 0) && (mazeMatrix[curPos.x, curPos.y + 1].getState() == MazeBlockState.Empty))
                    {
                        variantsPath.Add(new IntVector2(0, 1));
                    }
                    Debug.Log("variantsPath.Count : " + variantsPath.Count);
                }

                if (variantsPath.Count < 1)
                {
                    Debug.Log("variantsPath.Count < 1");
                    if (_i > 1)
                    {
                        int j = (int)(UnityEngine.Random.Range(1, _i));

                        curPos = new IntVector2(path[j].x, path[j].y);
                        // _i = j;
                    }
                    // return;
                }
                else
                {

                    int incRandom = (int)(UnityEngine.Random.Range(0, variantsPath.Count));
                    IntVector2 point = variantsPath[incRandom];

                    _i++;

                    IntVector2 newPos = new IntVector2(curPos.x + point.x, curPos.y + point.y);
                    mazeMatrix[curPos.x, curPos.y].addTransition(new IntVector2(point.x, point.y));

                    mazeMatrix[newPos.x, newPos.y].name = "## " + _i.ToString();
                    mazeMatrix[newPos.x, newPos.y].setState(MazeBlockState.Room);
                    mazeMatrix[newPos.x, newPos.y].addTransition(new IntVector2(point.x * -1, point.y * -1));
                    // if (_i < path.Count)
                    // {
                    //     path[_i].x = newPos.x; path[_i].y = newPos.y;
                    // }
                    // else
                    // {
                    path.Add(new IntVector2(newPos.x, newPos.y));
                    // }


                    curPos = new IntVector2(newPos.x, newPos.y);
                }
            }
            Debug.Log("make path complite");

            mazeMatrix[path[0].x, path[0].y].setState(MazeBlockState.Start);
            // for (int __i = 1; __i < path.Count - 1; __i++)
            // {
            //     mazeMatrix[path[__i].x, path[__i].y].setState(MazeBlockState.Room);
            // }
            int _j = path.Count - 1;
            mazeMatrix[path[_j].x, path[_j].y].setState(MazeBlockState.Finish);
        }

    }

[Header("Maze size")]
    [SerializeField] int maxSizeX = 10;
    [SerializeField] int maxSizeY = 10;
[Header("Maze fill")]
    [Range(0.01f, 1f)][SerializeField] float mazeFillValue = 0.75f;
[Header("Maze blocks")]
    [SerializeField] Vector3 blockSize = new Vector3(1, 1, 1);
    [SerializeField] GameObject prefBorder = null;
    [SerializeField] GameObject[] prefRoomLoop = null;
    [SerializeField] GameObject[] prefRoomTunel = null;
    [SerializeField] GameObject[] prefRoomTurn = null;
    [SerializeField] GameObject[] prefRoomTries = null;
    [SerializeField] GameObject[] prefRoomCross = null;
    // [SerializeField] GameObject prefWay = null;
[Header("Primary Attributes")]
    [SerializeField] GameObject prefRoomStart = null;
    [SerializeField] GameObject prefRoomFinish = null;
[Header("Secondary Attributes")]
    [SerializeField] GameObject[] wallAttributes = null;
    [SerializeField] GameObject[] floorAttributes = null;
    [SerializeField] GameObject[] roofAttributes = null;
[Header("Characters")]
    [SerializeField] GameObject prefCharacter = null;
    [SerializeField] int maxEnemyOnLevel = 10;
    [SerializeField] GameObject[] prefEnemy = null;
    // [SerializeField] GameObject prefDoor = null;
    

    IntVector2 maxSizeMatrix;
    Maze maze;
    int EnemyOnLevel = 0;
    
    void Start()
    {
        maxSizeMatrix = new IntVector2(maxSizeX, maxSizeY);
        maze = new Maze(maxSizeMatrix, mazeFillValue);
        mazeInstans(maze);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void mazeInstans(Maze _maze)
    {
        Vector3 shiftValue = new Vector3(blockSize.x, blockSize.y, blockSize.z);
        for (int my = 0; my < maxSizeMatrix.y; my++)
        {
            for (int mx = 0; mx < maxSizeMatrix.x; mx++)
            {
                Vector3 pos = new Vector3(mx * shiftValue.x, 0, my * shiftValue.y);
                GameObject _pref = null;
                MazeBlockInfo MBI = _maze.getBlock(new IntVector2(mx, my));
                MazeBlockState _blockState = MBI.getState();
                float angle = 0;
                _pref = choiceRoomPref(MBI, out angle);


                if (_pref != null)
                {
                    GameObject go = Instantiate(_pref, pos, Quaternion.identity);
                    go.transform.eulerAngles = new Vector3(0, angle,0);
                    go.name = MBI.name;
                }

                if (MBI.getState()==MazeBlockState.Start) {
                    GameObject character = Instantiate(prefCharacter, pos, Quaternion.identity);
                    character.transform.eulerAngles = new Vector3(0, angle,0);
                } 
                
                if (MBI.getState()==MazeBlockState.Room&&EnemyOnLevel<maxEnemyOnLevel) {
                    int fillState = (int)((100*maxSizeMatrix.y*maxSizeMatrix.x) / ((maxSizeMatrix.y-2)*my + mx));
                    int chanceAnimals = UnityEngine.Random.Range(0, fillState);
                    if (chanceAnimals<=(maxEnemyOnLevel- EnemyOnLevel)) {
                        GameObject pref = prefEnemy[(int)(UnityEngine.Random.Range(0, prefEnemy.Length - 1))];
                        Instantiate(pref, pos, Quaternion.identity);
                        EnemyOnLevel++;
                    } else {
                        int chanceAcces = UnityEngine.Random.Range(0, 5);
                        if (chanceAcces==1&&wallAttributes!=null&&wallAttributes.Length>0){
                            GameObject wallGO = Instantiate(wallAttributes[(int)(UnityEngine.Random.Range(0, wallAttributes.Length - 1))], pos, Quaternion.identity);
                            wallGO.transform.eulerAngles = new Vector3(0, (UnityEngine.Random.Range(0, 3)*90) ,0);
                        }
                        if (chanceAcces==2&&floorAttributes!=null&&floorAttributes.Length>0){
                            GameObject floorGO = Instantiate(floorAttributes[(int)(UnityEngine.Random.Range(0, floorAttributes.Length - 1))], pos, Quaternion.identity);
                            floorGO.transform.eulerAngles = new Vector3(0, (UnityEngine.Random.Range(0, 3)*90) ,0);
                        }
                        if (chanceAcces==3&&roofAttributes!=null&&roofAttributes.Length>0){
                            GameObject roofGO = Instantiate(roofAttributes[(int)(UnityEngine.Random.Range(0, roofAttributes.Length - 1))], pos, Quaternion.identity);
                            roofGO.transform.eulerAngles = new Vector3(0, (UnityEngine.Random.Range(0, 3)*90) ,0);
                        }
                    }
                }
                

            }
        }
    }

    GameObject choiceRoomPref(MazeBlockInfo block, out float angle)
    {
        angle = 0;
        GameObject blockPref = null;

        switch (block.getState())
        {
            case MazeBlockState.Border:
                blockPref = prefBorder;
                break;
            case MazeBlockState.Room:
                switch (block.getTransition().Count)
                {
                    case 1:
                        //добавить выборку для старт/финиш

                        blockPref = prefRoomLoop[(int)(UnityEngine.Random.Range(0, prefRoomLoop.Length - 1))];
                        
                        if (block.getTransition()[0].x>0) {angle = 90;}
                        if (block.getTransition()[0].x<0) {angle = -90;}
                        if (block.getTransition()[0].y>0) {angle = 0;}
                        if (block.getTransition()[0].y<0) {angle = 180;}
                        break;
                    case 2:
                        int x1 = 0; int y1 = 0;
                        foreach (var item in block.getTransition())
                        {
                            x1 += item.x;
                            y1 += item.y;
                        }
                        if (block.getTransition()[0].x+block.getTransition()[1].x!=0) {
                            blockPref = prefRoomTurn[(int)(UnityEngine.Random.Range(0, prefRoomTurn.Length - 1))];
                            if (x1==1&&y1==1) angle=-90;
                            if (x1==1&&y1==-1) angle=0;
                            if (x1==-1&&y1==-1) angle=90;
                            if (x1==-1&&y1==1) angle=180;
                            }
                        else {
                            blockPref = prefRoomTunel[(int)(UnityEngine.Random.Range(0, prefRoomTunel.Length - 1))];
                            if (block.getTransition()[0].x != 0 ) angle=90; else angle=0;
                            
                            }
                        break;
                    case 3:
                        blockPref = prefRoomTries[(int)(UnityEngine.Random.Range(0, prefRoomTries.Length - 1))];
                        int x2 = 0; int y2 = 0;
                        foreach (var item in block.getTransition())
                        {
                            x2 += item.x;
                            y2 += item.y;
                        }
                        if (x2>0) {angle = 180;}
                        if (x2<0) {angle = 0;}
                        if (y2>0) {angle = 90;}
                        if (y2<0) {angle = -90;}

                        break;
                        case 4:
                        blockPref = prefRoomCross[(int)(UnityEngine.Random.Range(0, prefRoomCross.Length - 1))];
                        break;
                }

                break;
            case MazeBlockState.Start:
               blockPref = prefRoomLoop[(int)(UnityEngine.Random.Range(0, prefRoomLoop.Length - 1))];
                        
                        if (block.getTransition()[0].x>0) {angle = 90;}
                        if (block.getTransition()[0].x<0) {angle = -90;}
                        if (block.getTransition()[0].y>0) {angle = 0;}
                        if (block.getTransition()[0].y<0) {angle = 180;}
                break;
            case MazeBlockState.Finish:
                blockPref = prefRoomLoop[(int)(UnityEngine.Random.Range(0, prefRoomLoop.Length - 1))];
                        
                        if (block.getTransition()[0].x>0) {angle = 90;}
                        if (block.getTransition()[0].x<0) {angle = -90;}
                        if (block.getTransition()[0].y>0) {angle = 0;}
                        if (block.getTransition()[0].y<0) {angle = 180;}
                break;
            
            default:

                break;
        }
        return blockPref;
    }
}
