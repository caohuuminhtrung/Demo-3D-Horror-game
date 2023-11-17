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

  Animator enemyAnim;
  int idleAnim, waveAnim, crawlAnim, booAnim;

  public int RNGcount, RNGlimit;
  float jumpScareCounter, attackCancelCounter;

  public bool isRNGcount, attackCancel, jumpscareFlag;

  public AudioSource windowKnocking1, windowKnocking2, footstep1, footstep2, laughing;

  public GameObject jumpscareEnemy, playerCam, jumpscareCam, jumpscareSound;

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

    currentState = enemyInitialState;
    currentState.enterState(this);

    StartEnemyBehaviour();

    // Debug.Log("jumpscare test");
    // StartCoroutine(playAttackAnimID());

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

    AttackCountdown();
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
      if (isRNGcount)
      {
        if (!currentState.GetType().Equals(typeof(EnemyPrepareState)))
        {
          if (Random.Range(0, 2) == 0)
          {
            laughing.Play();
          }
        }
        // Debug.Log("RNG counting...");

        if (Random.Range(0, 2) == 0)
        {
          RNGcount++;
          Debug.Log("RNG counter:" + RNGcount);
        }
        if (RNGcount == RNGlimit && !currentState.GetType().Equals(typeof(EnemyPrepareState)))
        {
          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("Jump failed!");
            RNGcount = 0;
          }
          else
          {
            Debug.Log("Jumping!");
            SwitchState(enemyPrepareState);
          }
          RNGcount = 0;
        }
      }

    }
  }

  public void AttackCountdown()
  {
    if (currentState.GetType().Equals(typeof(EnemyPrepareState)))
    {
      jumpScareCounter += Time.deltaTime;
      checkPlayerAction();
      if (jumpScareCounter > Random.Range(10f, 12f))
      {
        if (isEnemyAtDoor())
        {
          Debug.Log("Run doorRNG");
          doorRNG();
        }
        SwitchState(enemyAttackState);
        jumpScareCounter = 0;
        return;
      }

      if (isEnemyInCloset())
      {
        // Debug.Log("Run ClosetRNG");
        closetRNG();
      }

      if (checkPlayerAction())
      {
        jumpScareCounter = 0;
        SwitchState(enemyInitialState);
        return;
      }
      // Debug.Log("jumscare in: " + jumpScareCounter);
    }
  }

  public bool checkPlayerAction()
  {
    if (player.doorHeld && door.currentState.GetType().Equals(typeof(DoorHalfCloseState)))
    {
      attackCancelCounter += Time.deltaTime;
      Debug.Log("holding door for: " + attackCancelCounter);
      if (attackCancelCounter > Random.Range(1.5f, 2.9f))
      {
        Debug.Log("Hold door long enough!");
        return true;
      }
    }
    else if (player.isHidding && (closet.currentState.GetType().Equals(typeof(ClosetCloseState)) || closet.currentState.GetType().Equals(typeof(ClosetHalfCloseState))))
    {
      attackCancelCounter += Time.deltaTime;
      Debug.Log("holding closet for: " + attackCancelCounter);
      if (attackCancelCounter > Random.Range(1.8f, 3.2f))
      {
        Debug.Log("Hide long enough!");
        return true;
      }
    }
    else if (player.doorHeld && closet.currentState.GetType().Equals(typeof(ClosetHalfCloseState)))
    {
      Debug.Log("Player holding closet doors");
      attackCancelCounter += Time.deltaTime;
      Debug.Log("holding closet for: " + attackCancelCounter);
      if (attackCancelCounter > Random.Range(1.2f, 2.5f))
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

  void doorRNG()
  {
    if (!player.doorHeld && door.currentState.GetType().Equals(typeof(DoorHalfOpenState)))

      if (Random.Range(0, 2) == 0)
      {
        Debug.Log("Jump from door to closet");
        SwitchState(enemyPrepareState);
      }
      else if (player.isNearDoor)
      {
        Debug.Log("Player check door too late");
        SwitchState(enemyAttackState);
        jumpScareCounter = 0;
        return;
      }
      else
      {
        jumpScareCounter += Time.deltaTime;
        if (jumpScareCounter >= Random.Range(11.5f, 20f))
        {
          Debug.Log("Player didn't check door. RIP");
          SwitchState(enemyAttackState);
          jumpScareCounter = 0;
          return;
        }
      }
  }

  void closetRNG()
  {
    jumpscareFlag = false;
    // Debug.Log("jumpscare?");
    if (!jumpscareFlag && closet.currentState.GetType().Equals(typeof(ClosetHalfOpenState)))
    {
      // Debug.Log("jumpscare!!!");
      jumpscareFlag = true;
      playBooAnimID();
    }
    // Debug.Log("Attack???");
    if (closet.currentState.GetType().Equals(typeof(ClosetOpenState)))
    {
      // Debug.Log("Attack?");
      SwitchState(enemyAttackState);
      return;
    }
  }

  public void playIdleAnimID()
  {
    Debug.Log("Play idle animation");
    transform.localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(idleAnim);
  }

  public IEnumerator playWaveAnimID()
  {
    yield return new WaitForSeconds(Random.Range(5.1f, 7.9f));
    if (isEnemyAtDoor())
    {
      if (Random.Range(0, 3) > 0)
      {
        Debug.Log("Play wave animation");
        door.switchState(door.doorHalfOpenState);
        enemyAnim.Play(waveAnim);
      }
      else Debug.Log("Enemy doesn't play wave animation");
    }
  }

  public IEnumerator playCrawlAnimID()
  {
    yield return new WaitForSeconds(Random.Range(1.7f, 2.9f));
    Debug.Log("Play crawl animation");
    transform.SetPositionAndRotation(windowPos.transform.position, windowPos.transform.rotation);
    transform.localScale = new Vector3(-4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(crawlAnim);
  }

  public void playBooAnimID()
  {
    Debug.Log("Play BOO! animation");
    transform.localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(booAnim);
  }

  public IEnumerator playAttackAnimID()
  {
    // StartCoroutine(jumpscare.Play(jumpscareEnemy, playerCam, jumpscareCam, jumpscareSound.GetComponent<AudioSource>()));
    jumpscareCam.SetActive(true);
    playerCam.SetActive(false);

    jumpscareEnemy.GetComponent<Animator>().Play("Jumpscare");
    jumpscareSound.GetComponent<AudioSource>().Play();

    yield return new WaitForSeconds(3f);
    Debug.Log("return to normal");
    jumpscareCam.SetActive(false);
    playerCam.SetActive(true);

    // jumpscareEnemy.GetComponent<Animation>().Stop();
    jumpscareSound.GetComponent<AudioSource>().Stop();

  }

  public bool isEnemyAtDoor()
  {
    if (transform.position == doorPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool isEnemyAtWindow()
  {
    if (transform.position == windowPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool isEnemyAtHallway()
  {
    if (transform.position == hallwayPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool isEnemyAtPrewindow()
  {
    if (transform.position == preWindowPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool isEnemyInCloset()
  {
    if (transform.position == closetPos.transform.position)
    {
      return true;
    }
    return false;
  }

  public bool isEnemyAtBase()
  {
    if (transform.position == basePos.transform.position)
    {
      return true;
    }
    return false;
  }

}
