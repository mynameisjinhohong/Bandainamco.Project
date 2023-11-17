using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusBackground_LJH : MonoBehaviour
{
    [SerializeField] private Transform petalsGroup;
    [SerializeField] private Transform waypointsGroup;
    [SerializeField] private float duration;
    [SerializeField] private DG.Tweening.Ease ease;

    private LotusPetalController controller;

    private void Awake()
    {
        List<Transform> temp = new List<Transform>();
        for(int i=0;i<waypointsGroup.childCount;i++)
            temp.Add(waypointsGroup.GetChild(i).transform);

        controller = new LotusPetalController(petalsGroup.GetComponentsInChildren<CurveMovement_LJH>(true),
                                              temp.ToArray(), duration, ease);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
            controller.StartPetal();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
            controller.StopAllPetal();
    }
}
