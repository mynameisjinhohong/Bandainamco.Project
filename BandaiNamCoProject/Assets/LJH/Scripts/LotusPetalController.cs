using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LotusPetalController
{
    private float duration;
    private Ease ease;

    private CurveMovement_LJH[] petals;
    private Transform[] waypointsRoots;

    private Dictionary<int, Vector3[]> waypointsDic;


    public LotusPetalController(CurveMovement_LJH[] petals, Transform[] waypointsRoots, float duration, Ease ease)
    {
        this.petals = petals;
        this.waypointsRoots = waypointsRoots;
        this.duration = duration;
        this.ease = ease;

        waypointsDic = new Dictionary<int, Vector3[]>();

        for(int i=0;i<waypointsRoots.Length;i++)
        {
            List<Vector3> temp = new List<Vector3>();

            foreach (var t in waypointsRoots[i].GetComponentsInChildren<Transform>(true))
            {
                if (t == waypointsRoots[i]) continue;
                temp.Add(t.position);
            }

            waypointsDic.Add(i, temp.ToArray());
        }
    }

    public void StartPetal()
    {
        CurveMovement_LJH petal = GetRandomPetal();
        Vector3[] path = GetRandomPath();

        petal.gameObject.SetActive(true);
        petal.DoPath(path, duration, ease, () => {
            petal.KillSeqeunce();
            petal.gameObject.SetActive(false);
            StartPetal();
        });
    }

    public CurveMovement_LJH GetRandomPetal()
    {
        int ran = Random.Range(0, petals.Length);
        return petals[ran];
    }

    public Vector3[] GetRandomPath()
    {
        int ran = Random.Range(0, waypointsRoots.Length);
        return waypointsDic[ran];
    }

    public void StopAllPetal()
    {
        foreach (var petal in petals)
            petal.StopPetal();
    }
}
