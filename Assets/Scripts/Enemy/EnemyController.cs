using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
  public int AIlevel = 10;

  public GameController gameController;
  public PlayerController player;
  public DoorManager door;
  public ClosetManager closet;

  public GameObject windowPos, preWindowPos, hallwayPos, doorPos, closetPos, basePos;

  public EnemyBaseState currentState;
  public EnemyInitialState enemyInitialState;
  public EnemyIdleState enemyIdleState;
  public EnemyAttackState enemyAttackState;
  public EnemyPrepareState enemyPrepareState;

  public Animator enemyAnim, doorAnim, closetAnim1, closetAnim2;
  int idleAnim, waveAnim, crawlAnim, booAnim;

  public int RNGcount, RNGlimit;
  float jumpScareCounter, attackCancelCounter;

  public bool isRNGcount, attackCancel, jumpscareClosetFlag, jumpscareWindowFlag, waveDoorFlag;

  public bool jumpscareAtCloset, jumpscareAtDoor, jumpscareAtWindow;

  public AudioSource windowKnocking1, windowKnocking2, footstep1, footstep2, laughing, windowJump, closetJump;

  public GameObject jumpscareEnemy, playerCam, jumpscareCam, jumpscareSound, enemySpotlight;
  public bool isLosingGame;

  void Awake()
  {
    idleAnim = Animator.StringToHash("Idle");
    waveAnim = Animator.StringToHash("Wave");
    crawlAnim = Animator.StringToHash("Crawl");
    booAnim = Animator.StringToHash("Boo");
    // attackAnim = Animator.StringToHash("Attack");
  }

  // Start is called before the first frame update
  void Start()
  {

    gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    door = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorManager>();
    closet = GameObject.FindGameObjectWithTag("Closet").transform.parent.Find("door_L").GetComponent<ClosetManager>();

    enemyAnim = GameObject.FindGameObjectWithTag("EnemyModel").GetComponent<Animator>();

    enemyInitialState = gameObject.AddComponent<EnemyInitialState>();
    enemyIdleState = gameObject.AddComponent<EnemyIdleState>();
    enemyAttackState = gameObject.AddComponent<EnemyAttackState>();
    enemyPrepareState = gameObject.AddComponent<EnemyPrepareState>();

    basePos = GameObject.FindGameObjectWithTag("BaseMovementPos");
    windowPos = GameObject.FindGameObjectWithTag("WindowMovementPos");
    preWindowPos = GameObject.FindGameObjectWithTag("PreWindowMovementPos");
    hallwayPos = GameObject.FindGameObjectWithTag("HallwayMovementPos");
    doorPos = GameObject.FindGameObjectWithTag("DoorMovementPos");
    closetPos = GameObject.FindGameObjectWithTag("WardrobeMovementPos");

    isRNGcount = true;
    attackCancel = false;

    jumpscareAtCloset = false;
    jumpscareAtDoor = false;
    jumpscareAtWindow = false;

    isLosingGame = false;

    currentState = enemyInitialState;
    currentState.enterState(this);

    // StartEnemyBehaviour();
  }

  void Update()
  {
    //check whether or not isInitialState to increase RNG counter
    if (currentState.GetType().Equals(typeof(EnemyPrepareState)) && isRNGcount == true)
    {
      isRNGcount = false;
    }
    else if (!currentState.GetType().Equals(typeof(EnemyPrepareState)) && isRNGcount == false)
    {
      isRNGcount = true;
    }

    if (currentState.GetType().Equals(typeof(EnemyPrepareState)))
    {
      AttackBehaviour();
    }
  }

  public void StartEnemyBehaviour()
  {
    Debug.Log("enemy Start");
    // behaviourRunning = true;
    StartCoroutine(EnemyMovementBehaviour(AIlevel));
    StartCoroutine(ClosetMovementRNG());

    RNGcount = 0;
    jumpScareCounter = 0;
    attackCancelCounter = 0;
  }

  // public void StopEnemyBehaviour()
  // {
  //   Debug.Log("enemy Stop, ready to Jumpscare");
  //   behaviourRunning = false;
  //   isRNGcount = false;
  //   StopCoroutine(EnemyMovementBehaviour(AIlevel));
  //   StopCoroutine(WardrobeMovementRNG());
  // }

  public IEnumerator EnemyMovementBehaviour(int level)
  {
    Debug.Log("runnning AI behaviour");

    while (true)
    {
      yield return new WaitForSeconds(4f);
      if (!currentState.GetType().Equals(typeof(EnemyPrepareState)))
      {
        int movement = Random.Range(1, 21);

        if (level >= movement)
        {
          currentState.updateState(this);
        }
        else if (level < movement)
        {
          Debug.Log("enemy failed to move");
        }
      }
      // Debug.Log("randomizing...");
    }

  }

  public void SwitchState(EnemyBaseState state)
  {
    currentState = state;
    state.enterState(this);
  }

  public void SetBasePosition()
  {
    transform.SetPositionAndRotation(basePos.transform.position, basePos.transform.rotation);
  }

  //RNG calculate for when enemy jump into wardrobe
  IEnumerator ClosetMovementRNG()
  {
    isRNGcount = true;

    while (true)
    {
      yield return new WaitForSeconds(1.0f);

      if (IsEnemyInCloset() && !currentState.GetType().Equals(typeof(EnemyPrepareState)))
      {
        if (Random.Range(0, 10) == 0)
        {
          laughing.Play();
        }
      }

      if (isRNGcount)
      {
        // Debug.Log("RNG counting...");

        if (Random.Range(0, 2) == 0)
        {
          RNGcount++;
          Debug.Log("RNG counter:" + RNGcount);
        }
        if (RNGcount == RNGlimit && !currentState.GetType().Equals(typeof(EnemyPrepareState)))
        {

          if (Random.Range(0, 3) > 0)
          {
            Debug.Log("Jumping!");
            SwitchState(enemyPrepareState);
          }
          else
          {
            Debug.Log("Jump failed!");
          }

          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("play laugh");
            laughing.Play();
          }

          RNGcount = 0;
        }
      }

    }
  }

  //Countdown to attack (10 -> 12sec)
  public void AttackBehaviour()
  {
    jumpScareCounter += Time.deltaTime;
    CheckPlayerAction();
    if (jumpScareCounter > Random.Range(10f, 12f))
    {
      if (IsEnemyAtDoor())
      {
        Debug.Log("Run doorRNG");
        DoorBehaviour();
        return;
      }

      if (IsEnemyAtWindow())
      {
        if (player.isHidding)
        {
          SetClosetJumpscareAnim();
        }
        jumpscareAtWindow = true;
      }
      if (IsEnemyInCloset())
      {
        SetClosetJumpscareAnim();
        jumpscareAtCloset = true;
      }

      SwitchState(enemyAttackState);
      jumpScareCounter = 0;
      return;
    }

    if (IsEnemyAtDoor() && waveDoorFlag && !player.doorHeld && door.currentState.GetType().Equals(typeof(DoorHalfCloseState)))
    {
      door.switchState(door.doorHalfOpenState);
    }

    if (!jumpscareWindowFlag && player.isLookAtWindow && IsEnemyAtWindow())
    {
      jumpscareWindowFlag = true;
      windowJump.Play();
    }

    if (IsEnemyInCloset())
    {
      // Debug.Log("Run ClosetBehaviour");
      ClosetBehaviour();
    }

    if (CheckPlayerAction())
    {
      jumpScareCounter = 0;
      SwitchState(enemyInitialState);
      return;
    }

  }

  public bool CheckPlayerAction()
  {
    if (IsEnemyAtDoor() && player.doorHeld && player.isNearDoor && door.currentState.GetType().Equals(typeof(DoorHalfCloseState)))
    {
      attackCancelCounter += Time.deltaTime;
      Debug.Log("holding door for: " + attackCancelCounter);
      if (attackCancelCounter > Random.Range(1.9f, 2.9f))
      {
        Debug.Log("Hold door long enough!");
        return true;
      }
    }
    else if (IsEnemyAtWindow() && player.isHidding && (closet.currentState.GetType().Equals(typeof(ClosetCloseState)) || closet.currentState.GetType().Equals(typeof(ClosetHalfCloseState)) || closet.currentState.GetType().Equals(typeof(ClosetHalfOpenState))))
    {
      attackCancelCounter += Time.deltaTime;
      Debug.Log("holding closet for: " + attackCancelCounter);
      if (attackCancelCounter > Random.Range(2.7f, 4.4f))
      {
        Debug.Log("Hide long enough!");
        return true;
      }
    }
    else if (IsEnemyInCloset() && player.doorHeld && !player.isNearDoor && closet.currentState.GetType().Equals(typeof(ClosetHalfCloseState)))
    {
      Debug.Log("Player holding closet doors");
      attackCancelCounter += Time.deltaTime;
      Debug.Log("holding closet for: " + attackCancelCounter);
      if (attackCancelCounter > Random.Range(1.98f, 2.5f))
      {
        Debug.Log("Hold closet long enough!");
        return true;
      }
    }
    else
    {
      attackCancelCounter = 0;
      return false;
    }
    return false;
  }

  void DoorBehaviour()
  {
    if (player.isNearDoor)
    {
      Debug.Log("Player check door too late");
      SetDoorJumpscareAnim();
      SwitchState(enemyAttackState);
      jumpScareCounter = 0;
      jumpscareAtDoor = true;
      return;
    }
    if (!player.isNearDoor)

      if (Random.Range(0, 3) > 0)
      {
        Debug.Log("Jump from door to closet");
        SwitchState(enemyPrepareState);
        jumpScareCounter = 0;
        return;
      }
      else
      {
        SetBasePosition();
        jumpScareCounter += Time.deltaTime;
        if (jumpScareCounter >= Random.Range(1.2f, 19.8f))
        {
          Debug.Log("Player didn't check door. RIP");
          SetDoorJumpscareAnim();
          SwitchState(enemyAttackState);
          jumpScareCounter = 0;
          jumpscareAtDoor = true;
          return;
        }
      }
  }

  void ClosetBehaviour()
  {
    if (!jumpscareClosetFlag && closet.currentState.GetType().Equals(typeof(ClosetHalfOpenState)))
    {
      jumpscareClosetFlag = true;
      closetJump.Play();
      PlayBooAnimID();
    }
    if (closet.currentState.GetType().Equals(typeof(ClosetOpenState)))
    {
      SwitchState(enemyAttackState);
      jumpscareAtCloset = true;
      return;
    }
  }

  public void PlayIdleAnimID()
  {
    Debug.Log("Play idle animation");
    transform.localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(idleAnim);
  }

  public IEnumerator PlayWaveAnimID()
  {
    yield return new WaitForSeconds(Random.Range(4.3f, 7.9f));
    if (IsEnemyAtDoor())
    {
      if (Random.Range(0, 5) > 0)
      {
        Debug.Log("Play wave animation");
        door.switchState(door.doorHalfOpenState);
        enemyAnim.Play(waveAnim);
        waveDoorFlag = true;
      }
      else Debug.Log("Enemy doesn't play wave animation");
    }
  }

  public IEnumerator PlayCrawlAnimID()
  {
    yield return new WaitForSeconds(Random.Range(1.4f, 2.9f));
    Debug.Log("Play crawl animation");
    transform.SetPositionAndRotation(windowPos.transform.position, windowPos.transform.rotation);
    transform.localScale = new Vector3(-4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(crawlAnim);
  }

  public void PlayBooAnimID()
  {
    Debug.Log("Play BOO! animation");
    transform.localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(booAnim);
  }

  public IEnumerator PlayAttackAnimID()
  {
    // StartCoroutine(jumpscare.Play(jumpscareEnemy, playerCam, jumpscareCam, jumpscareSound.GetComponent<AudioSource>()));
    jumpscareCam.SetActive(true);
    playerCam.SetActive(false);
    player.enabled = false;
    // Cursor.lockState = CursorLockMode.Locked;
    jumpscareEnemy.GetComponent<Animator>().Play("Jumpscare");
    jumpscareSound.GetComponent<AudioSource>().Play();

    yield return new WaitForSeconds(3f);
    closetAnim1.SetFloat("Speed", 1);
    closetAnim2.SetFloat("Speed", 1);
    doorAnim.SetFloat("Speed", 1);
    Debug.Log("return to normal");
    jumpscareCam.SetActive(false);
    playerCam.SetActive(true);
    player.enabled = true;
    isLosingGame = true;
    // Cursor.lockState = CursorLockMode.None;
    // jumpscareEnemy.GetComponent<Animation>().Stop();
    jumpscareSound.GetComponent<AudioSource>().Stop();

  }

  public bool IsEnemyAtDoor()
  {
    if (transform.position == doorPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool IsEnemyAtWindow()
  {
    if (transform.position == windowPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool IsEnemyAtHallway()
  {
    if (transform.position == hallwayPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool IsEnemyAtPrewindow()
  {
    if (transform.position == preWindowPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool IsEnemyInCloset()
  {
    if (transform.position == closetPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool IsEnemyAtBase()
  {
    if (transform.position == basePos.transform.position)
    {
      return true;
    }
    return false;
  }

  void SetClosetJumpscareAnim()
  {
    Debug.Log("open closet?");

    closetAnim1.SetFloat("Speed", 5);
    closetAnim2.SetFloat("Speed", 5);
    closet.switchState(closet.closetOpenState);
  }

  void SetDoorJumpscareAnim()
  {
    Debug.Log("open door?");
    doorAnim.SetFloat("Speed", 3.5f);
    door.switchState(door.doorOpenState);
  }

}
