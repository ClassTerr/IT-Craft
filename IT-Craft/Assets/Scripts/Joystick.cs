using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

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

    private Animator anim = null;

    // Use this for initialization
    void Start()
    {
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
        CheckGroundStatus();
        if (player == null) return;

        Touch[] touches = Input.touches;
        
        if ((isGrounded && Input.GetMouseButtonDown(1) || 
            (touches.Length == 2 && touches[1].phase == TouchPhase.Began)))
        {
            Vector3 v = rigidbody.velocity;
            v.y = jumpPower;
            rigidbody.velocity = v;
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

            float speed = Mathf.Clamp(delta.y / maxDelta, 0, 1);
            float turn = Mathf.Clamp(delta.x / maxDelta, -1, 1) / 2;

            anim.SetFloat("Forward", speed, 0.2f, Time.deltaTime);
            anim.SetFloat("Turn", turn, 0.2f, Time.deltaTime);
            
            player.transform.RotateAround(player.transform.position, Vector3.up, turn * Time.deltaTime * 180);
            lastAction = Time.fixedTime;
        }
        else
        {
            anim.SetFloat("Forward", 0, 0.5f, Time.deltaTime);
            anim.SetFloat("Turn", 0, 0.5f, Time.deltaTime);
        }

        anim.SetBool("Crouch", Time.fixedTime - lastAction >= timeToCrouch);
    }
}