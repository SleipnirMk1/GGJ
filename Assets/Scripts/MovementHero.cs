using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHero : MonoBehaviour
{
    [SerializeField] Transform path;
    [SerializeField] List<Vector2> pathPoints = new List<Vector2> { };
    bool isFollowPath = true;
    bool checkPath = false;
    int indexMove;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        indexMove = 0;
        path = GameObject.Find("Main Path").transform;
        GeneratePathPoints();
        transform.position = pathPoints[0];
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
                        print(ranVal);
                        if (ranVal != 0) // Path ke-0 di branch harus diisi dengan path asal
                        {
                            path = pathBranch.paths[ranVal];
                            print(path.name);
                            GeneratePathPoints();
                            indexMove = 0;
                        } else indexMove++;
                    }
                    else
                    {
                        indexMove++;
                    }
                }
            }
            else
            {
                isFollowPath = false;
            }
        }
    }
}
