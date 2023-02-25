using UnityEngine;

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
    [SerializeField] private float turnCooldown = 2f;
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
    public bool IsCrouch { get { return _isCrouch; } }
    public bool IsMoving { get { return _isMoving; } }

    private float _turnTimer = 0f;
    private float _renewTimer = 0f;
    private float _movementTimer = 0f;
    private bool _isCrouch = false;
    private bool _isTurn = false;
    private bool _isMoving = false;
    private int _look = 0;

    private Vector3 startMovement, endMovement;

    private void Start()
    {
        smetanaJar.SetActive(false);
    }

    private void Update()
    {
        if (_turnTimer > 0f)
        {
            _turnTimer = Mathf.Clamp(_turnTimer - Time.deltaTime, 0, turnCooldown);
        }
        else if (_isTurn)
        {
            var gobEuler = goblinAnimator.transform.localEulerAngles;
            goblinAnimator.transform.localEulerAngles = new Vector3(gobEuler.x, SnapRotation(goblinAnimator.transform.localEulerAngles.y), gobEuler.z);
            _isTurn = false;
        }

        if (_renewTimer > 0f)
        {
            _renewTimer = Mathf.Clamp(_renewTimer - Time.deltaTime, 0, torchRenewLight);
        }
        else
        {
            var emission = torchParticle.emission;
            emission.enabled = true;
            torchLight.enabled = true;
        }

        float _h = Input.GetAxis("Horizontal");
        float _v = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire3"))
        {
            _isCrouch = !_isCrouch;
            goblinAnimator.SetBool("Crouch", _isCrouch);
            goblinAnimator.ResetTrigger("Attack");
            goblinAnimator.ResetTrigger("HevyAttack");
            goblinAnimator.ResetTrigger("TurnRight");
            goblinAnimator.ResetTrigger("TurnLeft");
        }

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
            var emission = torchParticle.emission;
            emission.enabled = false;
            torchLight.enabled = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            goblinAnimator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            goblinAnimator.SetTrigger("HevyAttack");
        }

        HandleTurn(_h);
        if (HandleMovement(_v))
        {
            //Play negative sound
        }
    }

    public void OnJarFound() {
        smetanaJar.SetActive(true);
    }

    //return true if can't move
    private bool HandleMovement(float dir)
    {
        if(_turnTimer > 0f) return false;

        if (IsMoving)
        {
            goblinAnimator.ResetTrigger("TurnRight");
            goblinAnimator.ResetTrigger("TurnLeft");

            _movementTimer += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startMovement, endMovement, _movementTimer / movementSpeed);
            if(_movementTimer / movementSpeed >= 1) {
                _isMoving = false;
                goblinAnimator.SetBool("Forward", false);
                goblinAnimator.SetBool("Backward", false);
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
                endMovement = transform.localPosition + goblinAnimator.transform.forward * distanceMovement;
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
                endMovement = transform.localPosition - goblinAnimator.transform.forward * distanceMovement;
            }
        }

        startMovement = transform.localPosition;
        _movementTimer = 0f;

        return false;
    }

    private void HandleTurn(float dir)
    {
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
            _turnTimer = turnCooldown;
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
        Debug.Log("Enter new room!");
        currentBlock = collider.GetComponentInParent<RoomBlock>();
    }

    private bool CanMoveLookDirection()
    {
        switch (goblinLookDir)
        {
            case (LookDirection.Forward):
                return currentBlock.roomDirections.forward;
            case (LookDirection.Right):
                return currentBlock.roomDirections.right;
            case (LookDirection.Backward):
                return currentBlock.roomDirections.backward;
            case (LookDirection.Left):
                return currentBlock.roomDirections.left;
        }
        return false;
    }
    private bool CanMoveBackLookDirection()
    {
        var temp = (_look + 2) % 4;
        switch ((LookDirection)temp)
        {
            case (LookDirection.Forward):
                return currentBlock.roomDirections.forward;
            case (LookDirection.Right):
                return currentBlock.roomDirections.right;
            case (LookDirection.Backward):
                return currentBlock.roomDirections.backward;
            case (LookDirection.Left):
                return currentBlock.roomDirections.left;
        }
        return false;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag != middleTag)
        {
            return;
        }

        inMiddle = false;
    }

    private float SnapRotation(float y)
    {
        float result = 0f;
        if (y < 45f)
        {
            result = 0f;
        }
        else if (y < 135f)
        {
            result = 90f;
        }
        else if (y < 225f)
        {
            result = 180f;
        }
        else if (y < 315f)
        {
            result = 270f;
        }
        else
        {
            result = 360f;
        }

        return result;
    }
}