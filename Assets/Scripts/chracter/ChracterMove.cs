using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMove : MonoBehaviour
{
    [SerializeField] float maxSpeedMove = 1;
    [SerializeField] float maxSpeedTurn = 1;
    [SerializeField] float turnTimeOut = 1;
    float time = 0;
    Rigidbody rb = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        float _h = Input.GetAxis("Horizontal");
        float _v = Input.GetAxis("Vertical");
        Vector3 move = Vector3.zero;
        Vector3 turn = Vector3.zero;
        if (_h != 0)
        {
            if (_h > 0) move += Vector3.forward;
            else move -= Vector3.forward;
            move = transform.TransformVector(move * Time.deltaTime * maxSpeedMove);
        }

        if (_v != 0)
        {
            if (_v < 0) turn += Vector3.up;
            else turn -= Vector3.up;
            turn = turn * 90;
           
        }

        rb.MovePosition(rb.position + move);
        // transform.Translate();
        // rb.rotation .eulerAngles += ;
        if (time <= 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(turn);
            rb.MoveRotation(rb.rotation * deltaRotation);
            time = turnTimeOut;
        }
    }
}
