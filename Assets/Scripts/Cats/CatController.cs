using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatController : MonoBehaviour
{
    [SerializeField] private GlobalSettingsScriptable globalSettings;
    [SerializeField] private RoomBlock currentBlock;
    [SerializeField] private string roomTag = "Room";

    private bool canMoveForward => currentBlock.RoomDirections.forward;
    private bool canMoveBackward => currentBlock.RoomDirections.backward;
    private bool canMoveLeft => currentBlock.RoomDirections.left;
    private bool canMoveRight => currentBlock.RoomDirections.right;

    private CharacterController.LookDirection lookDirection;

    private Vector3 startMovement, endMovement;
    private Quaternion startRotate, endRotate;

    private void OnEnable()
    {
        StartCoroutine(Movement());
    }

    private void OnTriggerEnter(Collider collider)
    {
        HandleRoom(collider);
    }

    private void HandleRoom(Collider collider)
    {
        if (collider.tag != roomTag)
        {
            return;
        }
        currentBlock = collider.GetComponentInParent<RoomBlock>();
    }

    public IEnumerator Movement()
    {
        yield return new WaitUntil(() => { return currentBlock != null; });
        while (true)
        {
            if (canMoveRight)
            {
                if (lookDirection != CharacterController.LookDirection.Right)
                {
                    yield return Turn(CharacterController.LookDirection.Right, globalSettings.catTurnTime);
                }
                yield return Move(CharacterController.LookDirection.Right, globalSettings.catMoveTime);
            }
            else if (canMoveForward)
            {
                if (lookDirection != CharacterController.LookDirection.Forward)
                {
                    yield return Turn(CharacterController.LookDirection.Forward, globalSettings.catTurnTime);
                }
                yield return Move(CharacterController.LookDirection.Forward, globalSettings.catMoveTime);
            }
            else if (canMoveBackward)
            {
                if (lookDirection != CharacterController.LookDirection.Backward)
                {
                    yield return Turn(CharacterController.LookDirection.Backward, globalSettings.catTurnTime);
                }
                yield return Move(CharacterController.LookDirection.Backward, globalSettings.catMoveTime);
            }
            else if (canMoveLeft)
            {
                if (lookDirection != CharacterController.LookDirection.Left)
                {
                    yield return Turn(CharacterController.LookDirection.Left, globalSettings.catTurnTime);
                }
                yield return Move(CharacterController.LookDirection.Left, globalSettings.catMoveTime);
            }

            yield return new WaitForSeconds(globalSettings.catWaitTime);
        }
    }

    private Vector3 GetDirection(CharacterController.LookDirection direction)
    {
        switch (direction)
        {
            case (CharacterController.LookDirection.Forward):
                return Vector3.forward;
            case (CharacterController.LookDirection.Right):
                return Vector3.right;
            case (CharacterController.LookDirection.Backward):
                return -Vector3.forward;
            case (CharacterController.LookDirection.Left):
                return Vector3.left;
        }
        return Vector3.forward;
    }

    private float GetDirectionAngle(CharacterController.LookDirection direction)
    {
        switch (direction)
        {
            case (CharacterController.LookDirection.Forward):
                return 0;
            case (CharacterController.LookDirection.Right):
                return 90;
            case (CharacterController.LookDirection.Backward):
                return 180;
            case (CharacterController.LookDirection.Left):
                return 270;
        }
        return 0;
    }

    private IEnumerator Turn(CharacterController.LookDirection direction, float speed)
    {
        float timer = 0f;

        lookDirection = direction;

        startRotate = transform.rotation;
        endRotate = Quaternion.Euler(0f, GetDirectionAngle(direction), 0f);

        //currently without animation;
        while (timer <= speed)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotate, endRotate, timer / speed);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Move(CharacterController.LookDirection direction, float speed)
    {
        float timer = 0f;
        startMovement = transform.position;
        endMovement = transform.position + GetDirection(direction) * 4;

        // goblinAnimator.SetBool("Forward", true);
        // goblinAnimator.SetBool("Backward", false);

        while (timer <= speed)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startMovement, endMovement, timer / speed);
            yield return new WaitForEndOfFrame();
        }

        // goblinAnimator.SetBool("Forward", false);
        // goblinAnimator.SetBool("Backward", false);
    }
}