using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CurveMovement_LJH : MonoBehaviour
{
    [SerializeField] private Ease ease;
    [SerializeField] private Transform waypointsRoot;
    [SerializeField] private int resolution;

    private List<Vector3> wayPositions;

    private void Awake()
    {
        wayPositions = new List<Vector3>();

        foreach(var w in waypointsRoot.GetComponentsInChildren<Transform>())
        {
            if (w == waypointsRoot) continue;
            wayPositions.Add(w.position);
        }

        transform.position = wayPositions[0];

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        transform.DOPath(wayPositions.ToArray(), 6.0f, PathType.CatmullRom, resolution: this.resolution).SetLookAt(new Vector3(0f, 0f, 0f)).SetEase(ease);
    }
}
