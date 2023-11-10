using System.Collections.Generic;
using UnityEngine;

public enum MainState
{
    Pause, Play, GameFinish, UiOn
}

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    private List<ManagerBase> managers;

    private MainState mainState;

    public MainState MainState
    {
        get { return mainState; }
        set
        {
            switch (value)
            {
                case MainState.Pause:
                    Time.timeScale = 0f;
                    break;
                case MainState.Play:
                    Time.timeScale = 1f;
                    break;
                case MainState.GameFinish:
                    Time.timeScale = 0f;
                    foreach (var manager in GetComponentsInChildren<ManagerBase>())
                        manager.GameOver();
                    break;
                case MainState.UiOn:
                    Time.timeScale = 0f;
                    break;
            }
            mainState = value;
        }
    }


    private void Awake()
    {
        Instance = this;
        managers = new List<ManagerBase>();
        MainState = MainState.Play;
        foreach (var manager in GetComponentsInChildren<ManagerBase>())
        {
            manager.Init();
            managers.Add(manager);
        }
    }

    public void NotifyItemEffect(ItemType itemType, bool start)
    {
        foreach (var m in managers)
        {
            m.ItemEffect(itemType, start);
        }
    }

    public void NotifyBackgroundEffect(ItemType itemType, bool start)
    {
        foreach (var m in managers)
        {
            m.BackgroundEffect(itemType, start);
        }
    }

    public void NotifyReset()
    {
        foreach (var manager in managers)
            manager.Reset();
    }
}
