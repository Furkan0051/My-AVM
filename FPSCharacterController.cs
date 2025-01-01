using UnityEngine;



public class FPSCharacterController : MonoBehaviour
{

    public float speed = 1f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public float mouseSensitivity = 100.0f;
   

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentVelocity;
    private float verticalRotation = 0.0f;

    private Animator animator;

    public float headBobSpeed = 14.0f;
    public float headBobAmount = 0.05f;
    private float headBobTimer = 0.0f;

    public float runSpeed = 2.5f;
    private float defaultSpeed;

    public Camera playerCamera;


    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // Mouse imlecini gizle
        Cursor.lockState = CursorLockMode.Locked;
        defaultSpeed = speed;
    }

    
    void Update()
    {

        // Mouse hareketlerini al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Kamera dikey hareketini sýnýrlamak (yukarý-aþaðý dönüþ için)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -60.0f, 60.0f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Karakter yatay dönüþ
        transform.Rotate(Vector3.up * mouseX);

        // Hareket girdisi
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 targetVelocity = (transform.right * moveX + transform.forward * moveZ) * speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = defaultSpeed;
        }

        animator.SetFloat("Speed", moveZ);

        // Hareketin yumuþak geçiþi
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * 10);
        controller.Move(currentVelocity * Time.deltaTime);

        // Hareket
        controller.Move(move * speed * Time.deltaTime);

        // Animator Parametresi Güncelle
        float speedValue = new Vector3(moveX, 0, moveZ).magnitude;
      //  animator.SetFloat("Speed", speedValue);

       


        // Zýplama
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            animator.SetTrigger("Jump");
        }

        // Yerçekimi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Hareket varsa kamerayý sallama
        if (controller.velocity.magnitude > 0.1f && controller.isGrounded)
        {
            headBobTimer += Time.deltaTime * headBobSpeed;
            float bobOffset = Mathf.Sin(headBobTimer) * headBobAmount;
           // cameraTransform.localPosition = new Vector3(0, 1.8f + bobOffset, 0);
        }
        else
        {
            headBobTimer = 0;
           // cameraTransform.localPosition = new Vector3(0, 1.8f, 0);
        }

       
    }
}
