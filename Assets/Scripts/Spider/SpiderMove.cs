using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : MonoBehaviour
{
    public enum SpiderState
    {
        idge,
        move,
        turn,
        scared,
        atack

    }

    [SerializeField] private Animator goblinAnimator;
    [SerializeField] private Rigidbody goblinBody;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float changeActionTimeout = 5f;

    private float actionTimeout = 0f;
    private Vector3 targetPosition = new Vector3();
    private Vector3 oldPosition = new Vector3();
    private Vector3 raycastPosition = new Vector3();
    private float targetTurn = 0;
    private bool isAgry = false;



    SpiderState curretState = SpiderState.idge;


    // Start is called before the first frame update
    void Start()
    {
        // actionTimeout = 0;
        // changeActionTimeout;
        // curretState = SpiderState.move;

        // int i = 
        // Debug.Log(i);
    }

    // Update is called once per frame
    void Update()
    {


        if (isAgry)
        {
            agryPlayer();
        }
        else if (actionTimeout < 0)
        {
            chooseAction();
            Debug.Log("spider choose");
        }
        else
        {

            if (curretState == SpiderState.idge)
            {
                    Debug.Log("spider idge");
            }
            else if (curretState == SpiderState.move)
            {
                spiderMove(Time.deltaTime);
                Debug.Log("spider move");
            }
            else if (curretState == SpiderState.turn)
            {
                spiderTurn();
                Debug.Log("spider turn");
            }
        }

        actionTimeout -= Time.deltaTime;

    }

    void chooseAction()
    {
        List<SpiderState> actionList = new List<SpiderState> { SpiderState.idge, SpiderState.turn, SpiderState.move };
        curretState = actionList[(int)UnityEngine.Random.Range(0, actionList.Count)];

        curretState = SpiderState.move;

        if (curretState == SpiderState.move)
        {
            setNewWay();
        }
        if (curretState == SpiderState.turn)
        {
            setNewTurn();
        }

        actionTimeout = changeActionTimeout;
    }

    Vector3 checkWay()
    {
        // int layerMask = 1 << 8;

        int layerMask = LayerMask.GetMask("wall", "floor");
        Vector3 _target = transform.position;
        Vector3 origin = transform.position + transform.up;
        for (int i = 2; i < 10; i++)
        {
            Vector3 target = transform.position + (transform.forward * i / 10);
            RaycastHit hit;
            if (Physics.Raycast(origin, target - origin, out hit, 2, layerMask))
            {
                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                // Debug.Log("Hit : " +  hit.transform.name + "; pos = [" + hit.point + "]");
                
                
                
                // Debug.DrawRay(origin, target - origin, Color.yellow, 4);
                _target = hit.point;

            }
            else
            {
                return (transform.position + transform.forward * (i - 1) / 10);
            }
        }

        return _target;
    }

    void setNewWay()
    {
        oldPosition = transform.position;
        targetPosition = checkWay();
        if (Vector3.Distance(targetPosition, oldPosition) < 0.1f)
        {
            curretState = SpiderState.move;
        }

    }

    void setNewTurn()
    {

    }

    void agryPlayer()
    {

    }

    void spiderMove(float dt)
    {
        Debug.Log(" -- spider move");
        int layerMask = LayerMask.GetMask("wall", "floor");

        Vector3 _target = targetPosition;
        Vector3 origin = transform.position+transform.up ;

        Vector3 target = (targetPosition-transform.position) * dt*movementSpeed;
        RaycastHit hit;
        Debug.DrawRay(origin, target , Color.green, 4);
        Debug.Log("_target = " + _target);

        if (Physics.Raycast(origin, target, out hit, 1, layerMask))
        {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Hit : " +  hit.transform.name + "; pos = [" + hit.point + "]");
            Debug.DrawRay(origin, target , Color.red, 4);
            _target = hit.normal;

            transform.up = (_target.normalized+transform.up.normalized)/2;
            transform.position = hit.point;

        }
        Debug.Log("no.." + hit);
    }

    void spiderTurn()
    {


    }
}
