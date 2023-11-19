using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusBackground_LJH : MonoBehaviour
{
    [SerializeField] private Transform petalsGroup;
    [SerializeField] private int delaySec;
    [SerializeField] private int numOfPetalsToSpawn;
    [SerializeField] private float petalSpeed;
    [SerializeField] private float petalDisappearTime;

    private LotusPetalController controller;

    private void Awake()
    {
        controller = new LotusPetalController(petalsGroup.GetComponentsInChildren<LotusPetal_LJH>(true),
                                                delaySec,
                                                numOfPetalsToSpawn,
                                                petalSpeed,
                                                petalDisappearTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
            controller.StartPetal();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
            controller.StopPetal();
    }
}
