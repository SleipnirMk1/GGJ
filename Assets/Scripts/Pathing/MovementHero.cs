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
    [SerializeField] int indexMove;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        indexMove = 0;
        path = GameObject.Find("Main Path").transform;
        GeneratePathPoints();
        transform.position = pathPoints[0];
        speed = GetComponent<Hero>().moveSpeed;
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
