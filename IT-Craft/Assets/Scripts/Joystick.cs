using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour
{
    public float maxEffectDistanceInPercentOfScreen = 20;
    public GameObject player;
    public float groundCheckDistance = 0.3f;
    public float jumpPower = 10f;
    public float timeToCrouch = 10f;

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isGrounded;
    private float lastAction = 0;
    private Rigidbody rigidbody;

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    private Animator anim = null;

    // Use this for initialization
    void Start()
    {
        // get the transform of the main camera
        if (Camera.allCameras.Length > 0)
        {
            m_Cam = Camera.allCameras[0].transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = player.GetComponent<ThirdPersonCharacter>();
        anim = player.GetComponent<Animator>();
        rigidbody = player.GetComponent<Rigidbody>();
    }

    void CheckGroundStatus()
    {
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
        RaycastHit hitInfo;
        if (Physics.Raycast(player.transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
        {
            isGrounded = true;
            //anim.applyRootMotion = true;
        }
        else
        {
            isGrounded = false;
            //anim.applyRootMotion = false;
        }
    }

    void Update()
    {
        float speed = 0, turn = 0;
        CheckGroundStatus();
        if (player == null) return;

        Touch[] touches = Input.touches;

        if (touches.Length == 0 && !Input.GetMouseButtonDown(0))
        {
            speed = turn = 0;
        }
        
        if ((isGrounded && Input.GetMouseButtonDown(1) || 
            (touches.Length == 2 && touches[1].phase == TouchPhase.Began)))
        {
            m_Jump = true;
            lastAction = Time.fixedTime;
        }

        if (Input.GetMouseButtonDown(0) ||
            (touches.Length > 0 && touches[0].phase == TouchPhase.Began))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) || touches.Length > 0)
        {
            endPos = Input.mousePosition;
            int sw = Screen.width;
            int sh = Screen.height;

            int s = Mathf.Min(sw, sh);


            float maxDelta = s * maxEffectDistanceInPercentOfScreen / 100;

            Vector2 delta = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

            speed = Mathf.Clamp(delta.y / maxDelta, 0, 3);
            turn = Mathf.Clamp(delta.x / maxDelta, -1, 1) / 2;

            //CrossPlatformInputManager.SetAxis("Vertical", speed);
            //CrossPlatformInputManager.SetAxis("Horisontal", turn);

            //anim.SetFloat("Forward", speed, 0.2f, Time.deltaTime);
            //anim.SetFloat("Turn", turn, 0.2f, Time.deltaTime);
            
            //player.transform.RotateAround(player.transform.position, Vector3.up, turn * Time.deltaTime * 180);
            lastAction = Time.fixedTime;
        }
        else
        {
            //anim.SetFloat("Forward", 0, 0.5f, Time.deltaTime);
            //anim.SetFloat("Turn", 0, 0.5f, Time.deltaTime);
        }


        bool crouch = Time.fixedTime - lastAction >= timeToCrouch;

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = speed * m_CamForward + turn * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = speed * Vector3.forward + turn * Vector3.right;
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }
}