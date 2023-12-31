using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovementHero : MonoBehaviour
{
    [SerializeField] Transform path;
    [SerializeField] List<Vector2> pathPoints = new List<Vector2> { };
    bool isFollowPath = true;
    bool isReversed = false;
    public int currentLevel;
    Rigidbody2D rb;
    [SerializeField] int indexMove;
    [SerializeField] float speed;
    LevelManager levelManager;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        indexMove = 0;
        path = LevelManager.Instance.levelProps[0].initialPath;
        GeneratePathPoints();
        transform.position = pathPoints[0];
        speed = GetComponent<Hero>().moveSpeed;
        rb = GetComponent<Rigidbody2D>();
        currentLevel = 0;
        levelManager = LevelManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void GeneratePathPoints()
    {
        pathPoints.Clear();
        for (int i = 0; i < path.childCount; i++)
        {
            pathPoints.Add(path.GetChild(i).position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowPath)
        {
            if (indexMove < pathPoints.Count)
            {
                transform.position = Vector2.MoveTowards(transform.position, pathPoints[indexMove], speed * Time.deltaTime);
                //Vector2 direction = (pathPoints[indexMove] - (Vector2)transform.position).normalized;
                //rb.MovePosition((Vector2)transform.position + (direction*speed*10*Time.deltaTime));
                if (Vector2.Distance(transform.position, pathPoints[indexMove]) < 0.01f)
                {
                    if (path.GetChild(indexMove).GetComponent<PathBranch>())
                    {
                        PathBranch pathBranch = path.GetChild(indexMove).GetComponent<PathBranch>();
                        int ranVal = Random.Range(0, pathBranch.paths.Length);
                        StartCoroutine(FindBranch(ranVal, pathBranch));
                    }
                    else
                    {
                        if (isReversed) indexMove--;
                        else indexMove++;
                    }
                }
            }
            else
            {
                StartCoroutine(DeadEnd());
            }
            if (isReversed)
            {
                if (Vector2.Distance(transform.position, (Vector2)levelManager.levelProps[currentLevel].unitSpawnPoint.position) < 0.1f)
                {
                    if (currentLevel == 0)
                    {
                        HeroParty.Instance.fleeingHero.Add(GetComponent<Hero>().characterScriptableObject);
                        GetComponent<Hero>().FleeBattle();
                    }
                    else
                    {
                        levelManager.AskTeleport(transform, currentLevel, false);
                        path = levelManager.levelProps[currentLevel].initialPath.parent.GetChild(levelManager.levelProps[currentLevel].initialPath.parent.childCount-2);
                        GeneratePathPoints();
                        indexMove = pathPoints.Count - 2;
                        print(indexMove);
                    }
                }
            }
            else
            {
                if(currentLevel != levelManager.levelProps.Length - 1){
                    if (Vector2.Distance(transform.position, levelManager.levelProps[currentLevel].finishPoint.position) < 0.1f)
                    {
                        levelManager.AskTeleport(transform, currentLevel, true);
                        indexMove = 1;
                        path = levelManager.levelProps[currentLevel].initialPath;
                        GeneratePathPoints();
                    }
                }
            }
            if (transform.position.x < pathPoints[indexMove].x) spriteRenderer.flipX = true;
            if (transform.position.x > pathPoints[indexMove].x) spriteRenderer.flipX = false;
        }
    }

    public void StartWalking()
    {
        isFollowPath = true;
    }

    public void StopWalking()
    {
        isFollowPath = false;
    }

    public void FleeBattle()
    {
        isFollowPath = true;
        isReversed = true;
        indexMove--;
    }

    IEnumerator DeadEnd()
    {
        StopWalking();
        yield return new WaitForSeconds(0.5f);
        StopAllCoroutines();
        isReversed = true;
        indexMove--;
        StartWalking();
    }

    IEnumerator FindBranch(int ranVal, PathBranch pathBranch)
    {
        StopWalking();
        yield return new WaitForSeconds(1f);
        StopAllCoroutines();
        isReversed = false;
        path = pathBranch.paths[ranVal];
        GeneratePathPoints();
        indexMove = 1;
        StartWalking();
    }
}
