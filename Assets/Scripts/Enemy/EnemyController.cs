using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
  public int AIlevel = 18;

  public GameObject windowPos, preWindowPos, hallwayPos, doorPos, WardrobePos, basePos, player;
  public GameController gameController;
  public EnemyBaseState currentState;
  public EnemyInitialState enemyInitialState;
  public EnemyIdleState enemyIdleState;
  public EnemyAttackState enemyAttackState;
  public EnemyPrepareState enemyPrepareState;

  Animator enemyAnim;
  int idleAnim, waveAnim, crawlAnim;

  public int RNGcount, RNGlimit = 10;
  float jumpScareCountdown;

  bool isRNGcount;

  public AudioSource windowKnocking1, windowKnocking2;

  void Awake()
  {
    idleAnim = Animator.StringToHash("Idle");
    waveAnim = Animator.StringToHash("Wave");
    crawlAnim = Animator.StringToHash("Crawl");
  }

  // Start is called before the first frame update
  void Start()
  {

    gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    enemyAnim = GameObject.FindGameObjectWithTag("EnemyModel").GetComponent<Animator>();

    enemyInitialState = gameObject.AddComponent<EnemyInitialState>();
    enemyIdleState = gameObject.AddComponent<EnemyIdleState>();
    enemyAttackState = gameObject.AddComponent<EnemyAttackState>();
    enemyPrepareState = gameObject.AddComponent<EnemyPrepareState>();

    player = GameObject.FindGameObjectWithTag("Player");

    basePos = GameObject.FindGameObjectWithTag("BaseMovementPos");
    windowPos = GameObject.FindGameObjectWithTag("WindowMovementPos");
    preWindowPos = GameObject.FindGameObjectWithTag("PreWindowMovementPos");
    hallwayPos = GameObject.FindGameObjectWithTag("HallwayMovementPos");
    doorPos = GameObject.FindGameObjectWithTag("DoorMovementPos");
    WardrobePos = GameObject.FindGameObjectWithTag("WardrobeMovementPos");

    isRNGcount = true;

    currentState = enemyInitialState;
    currentState.enterState(this);

    Debug.Log("enemy Start");
    StartCoroutine(EnemyMovementBehaviour(AIlevel));
    RNGcount = 0;
    StartCoroutine(WardrobeMovementRNG());

    jumpScareCountdown = 0;
  }

  void Update()
  {
    if (!currentState.GetType().Equals(typeof(EnemyInitialState)) && isRNGcount == true)
    {
      isRNGcount = false;
    }
    else if (currentState.GetType().Equals(typeof(EnemyInitialState)) && isRNGcount == false)
    {
      isRNGcount = true;
    }

    if (currentState.GetType().Equals(typeof(EnemyPrepareState)))
    {
      jumpScareCountdown += Time.deltaTime;
      if (jumpScareCountdown <= Random.Range(8f, 10f))
      {
        SwitchState(enemyAttackState);
        jumpScareCountdown = 0;
      }
    }
    else
    {
      jumpScareCountdown = 0;
    }
  }


  public IEnumerator EnemyMovementBehaviour(int level)
  {
    Debug.Log("runnning behaviour");

    while (!currentState.GetType().Equals(typeof(EnemyPrepareState)))
    {
      Debug.Log("randomizing...");
      yield return new WaitForSeconds(4f);

      int movement = Random.Range(1, 21);
      // if (level >= movement && currentState.GetType().Equals(typeof(EnemyIdleState)))
      if (level >= movement)
      {
        currentState.updateState(this);
      }
      else if (level < movement)
      {
        Debug.Log("enemy failed to move");
      }
    }
  }

  IEnumerator WardrobeMovementRNG()
  {
    isRNGcount = true;
    //RNG calculate for when enemy jump into wardrobe
    while (isRNGcount)
    {
      yield return new WaitForSeconds(1.0f);
      if (Random.Range(0, 2) == 0)
      {
        RNGcount++;
        // Debug.Log(RNGcount);
      }
      if (RNGcount == RNGlimit)
      {
        if (Random.Range(0, 2) == 0)
        {
          SwitchState(enemyPrepareState);
        }
        RNGcount = 0;
      }
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


  public void playIdleAnimID()
  {
    Debug.Log("Play idle animation");
    transform.localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(idleAnim);
  }

  public void playWaveAnimID()
  {
    Debug.Log("Play wave animation");
    transform.localScale = new Vector3(-4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(waveAnim);
  }

  public void playCrawlAnimID()
  {
    Debug.Log("Play crawl animation");
    transform.localScale = new Vector3(-4, transform.localScale.y, transform.localScale.z);
    enemyAnim.Play(crawlAnim);
  }
}
