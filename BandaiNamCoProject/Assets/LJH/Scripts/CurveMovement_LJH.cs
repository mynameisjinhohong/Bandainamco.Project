using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CurveMovement_LJH : MonoBehaviour
{
    [SerializeField] private Ease ease;
    [SerializeField] private Transform waypointsRoot;
    [SerializeField] private int resolution;

    private List<Vector3> wayPositions;
    private Sequence seq;

    private void Awake()
    {
        //wayPositions = new List<Vector3>();

        //foreach(var w in waypointsRoot.GetComponentsInChildren<Transform>())
        //{
        //    if (w == waypointsRoot) continue;
        //    wayPositions.Add(w.position);
        //}

        //transform.position = wayPositions[0];

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        //transform.DOPath(wayPositions.ToArray(), 6.0f, PathType.CatmullRom, resolution: this.resolution).SetEase(ease);
    }

    public void DoPath(Vector3[] wayPoints, float duration, Ease ease,  TweenCallback callback = null)
    {
        transform.position = wayPoints[0];

        seq = DOTween.Sequence();
        seq.Append(transform.DOPath(wayPoints, duration, PathType.CatmullRom)).SetEase(ease);
        seq.AppendCallback(callback);
    }

    public void KillSeqeunce()
    {
        seq.Kill();
        transform.DOKill();

        seq = null;
    }

    public void StopPetal()
    {
        if(seq != null)
        { KillSeqeunce(); }
        gameObject.SetActive(false);
    }
}
