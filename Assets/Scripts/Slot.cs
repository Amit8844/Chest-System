using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] ChestView chestView;
    public bool isEmpty;
    [HideInInspector]
    public ChestController chestController;

    private void Start()
    {
        isEmpty = true;
        SetSlotReference();
    }

    public void SpawnRandomChest(ChestScriptableObject randomChestSO)
    {
        chestController = ChestService.Instance.GetChest(randomChestSO, chestView);
        isEmpty = false;
    }

    public void SetSlotReference()
    {
        chestView.slotReference = this;
    }

}
