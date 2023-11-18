using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public GameObject playerCamera;
  public GameObject orientation;
  public GameController gameController;
  public float sensitivity;
  public float speed;
  public float runSpeed;
  public float smoothValue;
  public float bobSpeed = 4.8f;
  public float bobAmount = 0.05f;
  public AudioSource audioSource;
  public float castRadius;
  public float castDistance;
  public LayerMask castLayerMask;
  public bool doorHeld, isHidding, isNearDoor, isLookAtWindow;
  private float increaseSpeed = 5f;
  private float decreaseSpeed = 5f;
  private float customHorizontalInput = 0f;
  private float customVerticalInput = 0f;

  private float timer = Mathf.PI / 2;
  private float defaultPosY = 0;
  private float xRotation;
  private float yRotation;
  private Rigidbody rb;
  private float buttonPressTimer;

  private enum State
  {
    Disable,
    Idle,
    IsHolding
  };
  private State playerState;
  // Start is called before the first frame update
  void Start()
  {
    playerState = State.Idle;
    defaultPosY = playerCamera.transform.localPosition.y;
    rb = GetComponent<Rigidbody>();
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    doorHeld = false;
    isHidding = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (playerState == State.Disable)
    {
      audioSource.enabled = false;
      return;
    }
    CheckSpherecast(castRadius, castDistance, castLayerMask);
    float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity * GameInput.Key.Sensitivity;
    float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity * GameInput.Key.Sensitivity;
    float movementX = GetCustomHorizontalInput();
    float movementY = GetCustomVerticalInput();

    yRotation += mouseX;
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    orientation.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    transform.rotation = Quaternion.Euler(0, yRotation, 0);

    if (Input.GetKey(KeyCode.LeftShift))
    {
      rb.transform.Translate(runSpeed * Time.deltaTime * new Vector3(movementX, 0, movementY));
    }
    else
    {
      rb.transform.Translate(speed * Time.deltaTime * new Vector3(movementX, 0, movementY));
    }
    timer += bobSpeed * Time.deltaTime;

    if (movementX != 0 || movementY != 0)
    {
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
  private float GetCustomHorizontalInput()
  {
    if (GameInput.Key.GetKey("Right"))
    {
      customHorizontalInput = Mathf.MoveTowards(customHorizontalInput, 1f, increaseSpeed * Time.deltaTime);
    }
    else if (GameInput.Key.GetKey("Left"))
    {
      customHorizontalInput = Mathf.MoveTowards(customHorizontalInput, -1f, decreaseSpeed * Time.deltaTime);
    }
    else
    {
      customHorizontalInput = Mathf.MoveTowards(customHorizontalInput, 0f, decreaseSpeed * Time.deltaTime);
    }

    return customHorizontalInput;
  }

  private float GetCustomVerticalInput()
  {
    if (GameInput.Key.GetKey("Forward"))
    {
      customVerticalInput = Mathf.MoveTowards(customVerticalInput, 1f, increaseSpeed * Time.deltaTime);
    }
    else if (GameInput.Key.GetKey("Backward"))
    {
      customVerticalInput = Mathf.MoveTowards(customVerticalInput, -1f, decreaseSpeed * Time.deltaTime);
    }
    else
    {
      customVerticalInput = Mathf.MoveTowards(customVerticalInput, 0f, decreaseSpeed * Time.deltaTime);
    }

    return customVerticalInput;
  }

  void LateUpdate()
  {
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

      gameController.ShowPrompt(hitInfo.transform.gameObject);
      if (Input.GetKeyDown(KeyCode.E))
      {
        if (hitInfo.transform.gameObject.CompareTag("Door")) { isNearDoor = true; }
        else { isNearDoor = false; }

        buttonPressTimer = 0;
        doorHeld = false;
      }
      else if (Input.GetKeyUp(KeyCode.E))
      {
        if (doorHeld == false)
        {
          //Debug.Log("Press E")
          gameController.InteractObject(hitInfo.transform.gameObject);

        }
        doorHeld = false;
      }
      else if (Input.GetKey(KeyCode.E))
      {
        buttonPressTimer += Time.deltaTime;
        if (buttonPressTimer >= 0.25f)
        {
          // Debug.Log("Hold E");
          gameController.holdingDoor(hitInfo.transform.gameObject);
          doorHeld = true;
        }
      }

      if (hitInfo.transform.gameObject.CompareTag("Window")) { isLookAtWindow = true; }
      else { isLookAtWindow = false; }

    }
    else
    {
      gameController.HidePrompt();
    }

  }

  public void SetIdle()
  {
    playerState = State.Idle;
  }

  public void SetDisable()
  {
    playerState = State.Disable;
  }

  public void SetIsHolding(bool isHolding)
  {
    if (isHolding) playerState = State.IsHolding;
    else playerState = State.Idle;
  }

  public bool GetIsHolding()
  {
    return playerState == State.IsHolding;
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Closet"))
    {
      isHidding = true;
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("Closet"))
    {
      isHidding = false;
    }
  }
}
