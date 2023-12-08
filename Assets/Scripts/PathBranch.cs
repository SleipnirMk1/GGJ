using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBranch : MonoBehaviour
{
    public Transform[] paths;

    void Start()
    {
        paths[0] = transform.parent; // Asumsi path sudah diatur dengan benar, untuk bantuan saja
    }
}
