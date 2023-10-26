using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public GameObject playerCamera;
  public GameObject orientation;
  public float sensitivity;
  public float speed;
  public float smoothValue;
  float xRotation;
  float yRotation;
  Rigidbody rb;
  
  public float bobSpeed = 4.8f;
  public float bobAmount = 0.05f;
  public AudioSource audioSource;
  private float timer = Mathf.PI / 2;
  private float defaultPosY = 0;
  public float castRadius;
  public float castDistance;
  public LayerMask castLayerMask;
  public GameController gameController;
  private enum State {
    Disable,
    Enable
  };
  private State playerState;
  private GameObject raycastHitObject;
  // Start is called before the first frame update
  void Start()
  {
    playerState = State.Enable;
    defaultPosY = playerCamera.transform.localPosition.y;
    rb = GetComponent<Rigidbody>();
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (playerState == State.Disable) return;
    CheckSpherecast(castRadius, castDistance, castLayerMask);
    float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
    float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
    float movementX = Input.GetAxis("Horizontal");
    float movementY = Input.GetAxis("Vertical");

    yRotation += mouseX;
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    orientation.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    transform.rotation = Quaternion.Euler(0, yRotation, 0);

    rb.transform.Translate(speed * Time.deltaTime * new Vector3(movementX, 0, movementY));
    timer += bobSpeed * Time.deltaTime;

    if (Input.GetKeyDown(KeyCode.E) && raycastHitObject != null) {
      gameController.InteractObject(raycastHitObject);
    }

    if (movementX != 0 || movementY != 0) {
      audioSource.enabled = true;
      timer += Time.deltaTime * bobSpeed;
      playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, 
        Mathf.Clamp(playerCamera.transform.localPosition.y + Mathf.Sin(timer) * bobAmount, 6, 6.3f),
        playerCamera.transform.localPosition.z
      );
    }
    else
    {
      audioSource.enabled = false;
      timer = 0;
      playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
        Mathf.Lerp(playerCamera.transform.localPosition.y, defaultPosY, Time.deltaTime * bobSpeed),
        playerCamera.transform.localPosition.z
      );
    }
  
  }

  void LateUpdate() {
    playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, 
      orientation.transform.rotation, 
      smoothValue * Time.deltaTime
    );
  }

  void CheckSpherecast(float radius, float detectableDistance, LayerMask detectableLayer)
  {
    Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    RaycastHit hitInfo;
    if (Physics.SphereCast(playerCamera.transform.position, radius, ray.direction, out hitInfo, detectableDistance, detectableLayer))
    {
      raycastHitObject = hitInfo.transform.gameObject;
      gameController.ShowPrompt(raycastHitObject);
    } else {
      gameController.HidePrompt();
      raycastHitObject = null;
    }
  }

  public void SetEnable() {
    playerState = State.Enable;
  }

  public void SetDisable() {
    playerState = State.Disable;
  }
}
