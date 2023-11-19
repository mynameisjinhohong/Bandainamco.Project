using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class LotusPetalController
{
    private List<LotusPetal_LJH> petals;
    private int delaySec;
    private int numOfPetalsToSpawn;
    private float petalSpeed;
    private bool isStart;

    public LotusPetalController(LotusPetal_LJH[] petalsArr, int delaySec, int numOfPetalsToSpawn, float petalSpeed, float disappearTime)
    {
        petals = new List<LotusPetal_LJH>();
        petals.AddRange(petalsArr);
        this.delaySec = delaySec;
        this.numOfPetalsToSpawn = numOfPetalsToSpawn;

        foreach (var p in petals)
            p.Init(petalSpeed, disappearTime);
    }

    public async void StartPetal()
    {
        isStart = true;

        while (isStart)
        {
            ShootPetal();

            await UniTask.Delay(delaySec * 1000);
        }
    }

    private void ShootPetal()
    {
        List<int> randomIndexes = GetRandomIndex(numOfPetalsToSpawn);
    
        foreach(var i in randomIndexes)
        {
            petals[i].gameObject.SetActive(true);
            petals[i].Move();
        }
    }

    public void StopPetal()
    {
        isStart = false;

        foreach(var p in petals)
        {
            p.Stop();
        }
    }

    public List<int> GetRandomIndex(int num)
    {
        List<int> indexList = new List<int>();
        int count = 0;
        while(indexList.Count < num || count >= 1000) { 
            int index = Random.Range(0, petals.Count);
            if (!indexList.Contains(index))
                indexList.Add(index);

            count += 1;
        }

        return indexList;
    }
    //private float duration;
    //private Ease ease;

    //private CurveMovement_LJH[] petals;
    //private Transform[] waypointsRoots;

    //private Dictionary<int, Vector3[]> waypointsDic;


    //public LotusPetalController(CurveMovement_LJH[] petals, Transform[] waypointsRoots, float duration, Ease ease)
    //{
    //    this.petals = petals;
    //    this.waypointsRoots = waypointsRoots;
    //    this.duration = duration;
    //    this.ease = ease;

    //    waypointsDic = new Dictionary<int, Vector3[]>();

    //    for(int i=0;i<waypointsRoots.Length;i++)
    //    {
    //        List<Vector3> temp = new List<Vector3>();

    //        foreach (var t in waypointsRoots[i].GetComponentsInChildren<Transform>(true))
    //        {
    //            if (t == waypointsRoots[i]) continue;
    //            temp.Add(t.position);
    //        }

    //        waypointsDic.Add(i, temp.ToArray());
    //    }
    //}

    //public void StartPetal()
    //{
    //    CurveMovement_LJH petal = GetRandomPetal();
    //    Vector3[] path = GetRandomPath();

    //    petal.gameObject.SetActive(true);
    //    petal.DoPath(path, duration, ease, () => {
    //        petal.KillSeqeunce();
    //        petal.gameObject.SetActive(false);
    //        StartPetal();
    //    });
    //}

    //public CurveMovement_LJH GetRandomPetal()
    //{
    //    int ran = Random.Range(0, petals.Length);
    //    return petals[ran];
    //}

    //public Vector3[] GetRandomPath()
    //{
    //    int ran = Random.Range(0, waypointsRoots.Length);
    //    return waypointsDic[ran];
    //}

    //public void StopAllPetal()
    //{
    //    foreach (var petal in petals)
    //        petal.StopPetal();
    //}
}
