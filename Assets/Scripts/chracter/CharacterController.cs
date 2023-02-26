using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour
{
    public enum LookDirection
    {
        Forward,
        Right,
        Backward,
        Left,
    }

    [SerializeField] private Animator goblinAnimator;
    [SerializeField] private Rigidbody goblinBody;
    [SerializeField] private LookDirection goblinLookDir = LookDirection.Forward;
    [SerializeField] private float distanceMovement = 4f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float panicMovementSpeed = 2.5f;
    [SerializeField] private float turnSpeed = 2f;
    [SerializeField] private float panicTurnSpeed = 1f;
    [SerializeField] private bool inMiddle = false;
    [SerializeField] private float middleThreshold = 0.5f;
    [SerializeField] private string middleTag = "Middle";
    [SerializeField] private string roomTag = "Room";
    [SerializeField] private float torchRenewLight = 1.5f;
    [SerializeField] private ParticleSystem torchParticle;
    [SerializeField] private Light torchLight;
    [SerializeField] private GameObject smetanaJar;
    [SerializeField] private GameObject droppedJarPrefab;
    [SerializeField] private RoomBlock currentBlock;
    [SerializeField] private GlobalSettingsScriptable globalSettings;
    [SerializeField] private AudioSource panicSource;

    public System.Action<EnemyType> OnAttacked;
    public System.Action OnSmetanaFound;
    public System.Action OnPanic;

    public bool HasSmetana { get { return _hasSmetana; } }
    public bool IsCrouch { get { return _isCrouch; } }
    public bool IsMoving { get { return _isMoving; } }
    public bool RoomWithEnemy { get { return currentBlock.HasEnemy; } }
    public bool WasAttacked { get { return _wasAttacked; } }

    private float _turnTimer = 0f;
    private float _renewTimer = 0f;
    private float _movementTimer = 0f;

    private bool canMoveForward => currentBlock.RoomDirections.forward;
    private bool canMoveBackward => currentBlock.RoomDirections.backward;
    private bool canMoveLeft => currentBlock.RoomDirections.left;
    private bool canMoveRight => currentBlock.RoomDirections.right;

    private bool _isCrouch = false;
    private bool _isTurn = false;
    private bool _isMoving = false;
    private bool _hasSmetana = false;
    private bool _renewTorch = false;
    private bool _onPanic = false;
    private bool _wasAttacked = false;
    private bool _hasGoldPot = false;

    private int _look = 0;

    private Vector3 startMovement, endMovement;
    private Quaternion startRotate, endRotate;

    private void Start()
    {
        smetanaJar.SetActive(false);
    }

    private void Update()
    {

        if (_renewTimer > 0f)
        {
            _renewTimer = Mathf.Clamp(_renewTimer - Time.deltaTime, 0, torchRenewLight);
        }
        else if (_renewTorch)
        {
            _renewTorch = false;
            var emission = torchParticle.emission;
            emission.enabled = true;
            torchLight.enabled = true;
        }
        if (_onPanic) return;

        float _h = Input.GetAxis("Horizontal");
        float _v = Input.GetAxis("Vertical");

        // if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire3"))
        // {
        //     _isCrouch = !_isCrouch;
        //     goblinAnimator.SetBool("Crouch", _isCrouch);
        //     goblinAnimator.ResetTrigger("Attack");
        //     goblinAnimator.ResetTrigger("HevyAttack");
        //     goblinAnimator.ResetTrigger("TurnRight");
        //     goblinAnimator.ResetTrigger("TurnLeft");
        // }

        if (_isCrouch)
        {
            return;
        }

        if (_renewTimer > 0f)
        {
            return;
        }

        if (Input.GetButtonDown("Renew"))
        {
            goblinAnimator.SetTrigger("RenewTorch");
            _renewTimer = torchRenewLight;
            _renewTorch = true;
            var emission = torchParticle.emission;
            emission.enabled = false;
            torchLight.enabled = false;
        }

        // if (Input.GetButtonDown("Fire1"))
        // {
        //     goblinAnimator.SetTrigger("Attack");
        // }

        // if (Input.GetButtonDown("Fire2"))
        // {
        //     goblinAnimator.SetTrigger("HevyAttack");
        // }

        HandleTurn(_h);
        if (HandleMovement(_v))
        {
            //Play negative sound
        }
    }

    public void TakeDamage(EnemyType enemyType)
    {
        OnAttacked?.Invoke(enemyType);
    }

    //return true if can't move
    private bool HandleMovement(float dir)
    {
        if (_turnTimer > 0f) return false;

        if (IsMoving)
        {
            goblinAnimator.ResetTrigger("TurnRight");
            goblinAnimator.ResetTrigger("TurnLeft");

            _movementTimer += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startMovement, endMovement, _movementTimer / movementSpeed);
            if (_movementTimer / movementSpeed >= 1)
            {
                _isMoving = false;
                goblinAnimator.SetBool("Forward", false);
                goblinAnimator.SetBool("Backward", false);

                if (currentBlock.HasBonus)
                {
                    Debug.Log("Room with bonus");
                }
                else if (currentBlock.IsFinish)
                {
                    OnSmetanaFound?.Invoke();
                    _hasSmetana = true;
                    smetanaJar.SetActive(true);
                }
                else if (_hasSmetana && currentBlock.IsStart)
                {
                    if (_hasGoldPot)
                    {
                        Debug.Log("GameOver You WIN! YAAAAY!!!");
                    }
                    else
                    {
                        Debug.Log("GameOver You LOSE! YAAAAY!!!");
                    }
                }
            }
            return false;
        }

        if (dir == 0) { return false; }

        if (dir > 0f)
        {
            if (!CanMoveLookDirection())
            {
                return true;
            }
            else
            {
                goblinAnimator.SetBool("Forward", true);
                goblinAnimator.SetBool("Backward", false);
                _isMoving = true;
                endMovement = transform.localPosition + GetDirection(goblinLookDir) * distanceMovement;
            }
        }
        else
        {
            if (!CanMoveBackLookDirection())
            {
                return true;
            }
            else
            {
                goblinAnimator.SetBool("Forward", false);
                goblinAnimator.SetBool("Backward", true);
                _isMoving = true;
                endMovement = transform.localPosition - GetDirection(goblinLookDir) * distanceMovement;
            }
        }

        startMovement = transform.localPosition;
        _movementTimer = 0f;

        return false;
    }

    private void HandleTurn(float dir)
    {
        if (_turnTimer > 0f)
        {
            _turnTimer = Mathf.Clamp(_turnTimer - Time.deltaTime, 0, turnSpeed);
            transform.rotation = Quaternion.Lerp(startRotate, endRotate, (turnSpeed - _turnTimer) / turnSpeed);
        }
        else if (_isTurn)
        {
            var gobEuler = transform.localEulerAngles;
            _isTurn = false;
        }

        if (dir != 0f && inMiddle && _turnTimer <= 0f)
        {
            if (dir > 0f)
            {
                goblinAnimator.SetTrigger("TurnRight");
                _look = (_look + 1) % 4;
            }
            else
            {
                goblinAnimator.SetTrigger("TurnLeft");
                if (_look - 1 < 0)
                {
                    _look = 3;
                }
                else
                {
                    _look = _look - 1;
                }
            }

            _isTurn = true;
            goblinLookDir = (LookDirection)_look;

            goblinAnimator.ResetTrigger("Attack");
            goblinAnimator.ResetTrigger("HevyAttack");
            _turnTimer = turnSpeed;
            startRotate = goblinAnimator.transform.rotation;
            endRotate = Quaternion.Euler(0f, GetDirectionAngle(goblinLookDir), 0f);
        }
    }


    private void OnTriggerStay(Collider collider)
    {
        HandleMiddle(collider);
    }

    private void OnTriggerEnter(Collider collider)
    {
        HandleRoom(collider);
    }

    private void HandleMiddle(Collider collider)
    {
        if (collider.tag != middleTag)
        {
            return;
        }
        if ((collider.transform.position - transform.position).magnitude > middleThreshold)
        {
            return;
        }

        inMiddle = true;
    }

    private void HandleRoom(Collider collider)
    {
        if (collider.tag != roomTag)
        {
            return;
        }
        currentBlock = collider.GetComponentInParent<RoomBlock>();
    }

    private bool CanMoveLookDirection()
    {
        switch (goblinLookDir)
        {
            case (LookDirection.Forward):
                return canMoveForward;
            case (LookDirection.Right):
                return canMoveRight;
            case (LookDirection.Backward):
                return canMoveBackward;
            case (LookDirection.Left):
                return canMoveLeft;
        }
        return false;
    }
    private bool CanMoveBackLookDirection()
    {
        var temp = (_look + 2) % 4;
        switch ((LookDirection)temp)
        {
            case (LookDirection.Forward):
                return canMoveForward;
            case (LookDirection.Right):
                return canMoveRight;
            case (LookDirection.Backward):
                return canMoveBackward;
            case (LookDirection.Left):
                return canMoveLeft;
        }
        return false;
    }

    private Vector3 GetDirection(LookDirection direction)
    {
        switch (direction)
        {
            case (LookDirection.Forward):
                return Vector3.forward;
            case (LookDirection.Right):
                return Vector3.right;
            case (LookDirection.Backward):
                return -Vector3.forward;
            case (LookDirection.Left):
                return Vector3.left;
        }
        return Vector3.forward;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag != middleTag)
        {
            return;
        }

        inMiddle = false;
    }

    public IEnumerator DoPanic()
    {
        _onPanic = true;
        var emission = torchParticle.emission;
        _renewTorch = false;
        emission.enabled = false;
        torchLight.enabled = false;
        panicSource.Play();

        goblinAnimator.ResetTrigger("TurnLeft");
        goblinAnimator.ResetTrigger("TurnRight");
        goblinAnimator.ResetTrigger("RenewTorch");

        OnPanic?.Invoke();

        yield return PanicMovement();
    }


    public IEnumerator DoChill()
    {
        _onPanic = false;
        goblinAnimator.SetTrigger("RenewTorch");
        _renewTimer = torchRenewLight;
        _renewTorch = true;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator PanicMovement()
    {
        float moveCount = 0f;

        while (moveCount < globalSettings.panicMoves)
        {
            moveCount++;

            if (canMoveRight)
            {
                if (goblinLookDir != LookDirection.Right)
                {
                    yield return Turn(LookDirection.Right, panicTurnSpeed);
                }
                yield return Move(LookDirection.Right, panicMovementSpeed);
            }
            else if (canMoveForward)
            {
                if (goblinLookDir != LookDirection.Forward)
                {
                    yield return Turn(LookDirection.Forward, panicTurnSpeed);
                }
                yield return Move(LookDirection.Forward, panicMovementSpeed);
            }
            else if (canMoveBackward)
            {
                if (goblinLookDir != LookDirection.Backward)
                {
                    yield return Turn(LookDirection.Backward, panicTurnSpeed);
                }
                yield return Move(LookDirection.Backward, panicMovementSpeed);
            }
            else if (canMoveLeft)
            {
                if (goblinLookDir != LookDirection.Left)
                {
                    yield return Turn(LookDirection.Left, panicTurnSpeed);
                }
                yield return Move(LookDirection.Left, panicMovementSpeed);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private float GetDirectionAngle(LookDirection direction)
    {
        switch (direction)
        {
            case (LookDirection.Forward):
                return 0;
            case (LookDirection.Right):
                return 90;
            case (LookDirection.Backward):
                return 180;
            case (LookDirection.Left):
                return 270;
        }
        return 0;
    }

    private IEnumerator Turn(LookDirection direction, float speed)
    {
        float timer = 0f;

        goblinLookDir = direction;

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

    private IEnumerator Move(LookDirection direction, float speed)
    {
        float timer = 0f;
        startMovement = transform.position;
        endMovement = transform.position + GetDirection(direction) * distanceMovement;

        goblinAnimator.SetBool("Forward", true);
        goblinAnimator.SetBool("Backward", false);

        while (timer <= speed)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startMovement, endMovement, timer / speed);
            yield return new WaitForEndOfFrame();
        }

        goblinAnimator.SetBool("Forward", false);
        goblinAnimator.SetBool("Backward", false);
    }
}