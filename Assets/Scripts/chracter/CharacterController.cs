using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator goblinAnimator;
    [SerializeField] private Rigidbody goblinBody;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float turnCooldown = 2f;
    [SerializeField] private bool inMiddle = false;
    [SerializeField] private float middleThreshold = 0.5f;
    [SerializeField] private string middleTag = "Middle";
    [SerializeField] private float torchRenewLight = 1.5f;
    [SerializeField] private ParticleSystem torchParticle;
    [SerializeField] private Light torchLight;

    public bool IsCrouch { get { return _isCrouch; } }

    private float turnTimer = 0f;
    private float renewTimer = 0f;
    private bool _isCrouch = false;
    private bool _isTurn = false;

    private void Update()
    {
        if (turnTimer > 0f)
        {
            turnTimer = Mathf.Clamp(turnTimer - Time.deltaTime, 0, turnCooldown);
        }
        else if (_isTurn)
        {
            var gobEuler = goblinAnimator.transform.localEulerAngles;
            goblinAnimator.transform.localEulerAngles = new Vector3(gobEuler.x, SnapRotation(goblinAnimator.transform.localEulerAngles.y), gobEuler.z);
            _isTurn = false;
        }

        if (renewTimer > 0f)
        {
            renewTimer = Mathf.Clamp(renewTimer - Time.deltaTime, 0, torchRenewLight);
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

        if (Input.GetButtonDown("Renew"))
        {
            goblinAnimator.SetTrigger("RenewTorch");
            renewTimer = torchRenewLight;
            var emission = torchParticle.emission;
            emission.enabled = false;
            torchLight.enabled = false;
        }

        if (renewTimer > 0f)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            goblinAnimator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            goblinAnimator.SetTrigger("HevyAttack");
        }

        if (_h != 0f && inMiddle && turnTimer <= 0f)
        {
            if (_h > 0f)
            {
                goblinAnimator.SetTrigger("TurnRight");
            }
            else
            {
                goblinAnimator.SetTrigger("TurnLeft");
            }

            _isTurn = true;

            goblinAnimator.ResetTrigger("Attack");
            goblinAnimator.ResetTrigger("HevyAttack");
            turnTimer = turnCooldown;
        }

        if (_v != 0f)
        {
            goblinBody.isKinematic = false;
            if (_v > 0f)
            {
                goblinAnimator.SetBool("Forward", true);
                goblinAnimator.SetBool("Backward", false);
            }
            else
            {
                goblinAnimator.SetBool("Forward", false);
                goblinAnimator.SetBool("Backward", true);
            }
            goblinAnimator.ResetTrigger("TurnRight");
            goblinAnimator.ResetTrigger("TurnLeft");
            goblinBody.velocity = goblinAnimator.transform.forward * _v * movementSpeed;
        }
        else
        {
            goblinAnimator.SetBool("Forward", false);
            goblinAnimator.SetBool("Backward", false);
            goblinBody.isKinematic = true;
        }
    }

    private void OnTriggerStay(Collider collider)
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