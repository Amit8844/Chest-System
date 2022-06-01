using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour
{
    public ChestController chestController;
    [HideInInspector]
    public Slot slotReference;

    [SerializeField] private Sprite EmptySlotSprite;

    [SerializeField] public Text chestTimerTxt;
    [SerializeField] private Image chestSlotSprite;
    [SerializeField] private Text chestTypeTxt;
    [SerializeField] private Image coinImage;
    [SerializeField] private Text coinsTxt;
    [SerializeField] private Image gemImage;
    [SerializeField] private Text gemsTxt;

    [SerializeField] private Button ChestButton;

    private ChestState currentState;

    public void SetControllerReference(ChestController chestController)
    {
        this.chestController = chestController;
    }

    private void Start()
    {
        InitializeEmptyChestView();
    }

    public void InitializeEmptyChestView()
    {
        chestTimerTxt.gameObject.SetActive(false);
        chestSlotSprite.sprite = EmptySlotSprite;
        chestTypeTxt.gameObject.SetActive(false);
        coinImage.gameObject.SetActive(false);
        coinsTxt.gameObject.SetActive(false);
        gemImage.gameObject.SetActive(false);
        gemsTxt.gameObject.SetActive(false);
        ChestButton.enabled = false;
        currentState = ChestState.None;
    }

    public void InitialiseViewUIForLockedChest()
    {
        chestTimerTxt.gameObject.SetActive(false);
        chestSlotSprite.sprite = chestController.chestModel.ChestSprite;
        chestTypeTxt.gameObject.SetActive(true);
        chestTypeTxt.text = chestController.chestModel.ChestName;
        coinImage.gameObject.SetActive(true);
        coinsTxt.gameObject.SetActive(true);
        coinsTxt.text = chestController.chestModel.CoinCost.ToString();
        gemImage.gameObject.SetActive(true);
        gemsTxt.gameObject.SetActive(true);
        gemsTxt.text = chestController.GetGemCost().ToString();
        ChestButton.enabled = true;
        currentState = ChestState.Locked;
    }

    public void InitialiseViewUIForUnlockingChest()
    {
        chestTimerTxt.gameObject.SetActive(true);
        chestSlotSprite.sprite = chestController.chestModel.ChestSprite;
        chestTypeTxt.gameObject.SetActive(true);
        chestTypeTxt.text = chestController.chestModel.ChestName;
        coinImage.gameObject.SetActive(false);
        coinsTxt.gameObject.SetActive(false);
        gemImage.gameObject.SetActive(false);
        gemsTxt.gameObject.SetActive(false);
        ChestButton.enabled = false;
        currentState = ChestState.Unlocking;
    }

    public void InitialiseViewUIForUnlockedChest()
    {
        chestTimerTxt.gameObject.SetActive(true);
        chestSlotSprite.sprite = chestController.chestModel.ChestSprite;
        chestTypeTxt.gameObject.SetActive(true);
        chestTypeTxt.text = chestController.chestModel.ChestName;
        coinImage.gameObject.SetActive(false);
        coinsTxt.gameObject.SetActive(false);
        gemImage.gameObject.SetActive(false);
        gemsTxt.gameObject.SetActive(false);
        ChestButton.enabled = true;
        currentState = ChestState.Unlocked;
    }


    public void OnClickChestButton()
    {
        if (currentState == ChestState.Locked)
        {
            if (SlotsController.Instance.isUnlocking)
            {
                UIHandler.Instance.ToggleIsBusyUnlockingPopup(true);
            }
            else
            {
                ChestService.Instance.selectedController = chestController;
                UIHandler.Instance.ToggleUnlockChestPopup(true);
            }



            /*
            
            0. Check if any other Chest is still unlocking usingSlotsController.isUnlocking and if yes then show the "Wait for your Chest to unlock" Popup.
            1. Open a dialog box with option to either Start the timer through coins or Instantly open through gems.
            2. The POPUP will open. When any button of the two is clicked in the popup, a method in view is called which checks if enough resources are there or not.
            3. If not then a popup using UIHandler shows saying Insufficient Resources.
            4. If enough resources are present then we will call a method EnteringUnlockingState() or OpenInstantly() from the ChestView.

             */
        }
        else if (currentState == ChestState.Unlocking)
        {
            /*
            
            1. Show a popup using UIHandler to UNLOCK Chest through gems(cost calculated by controller) instantly.
            2. If option chosen then call method EnteringUnlockedState() from the ChestView.

             */
        }
        else if (currentState == ChestState.Unlocked)
        {
            ChestService.Instance.selectedController = chestController;
            OpenChest();
            ChestService.Instance.ToggleRewardsPopup(true);

            /*
            
            1. Call a method OpenChest() from ChestView.

             */
        }
    }


    public void EnteringUnlockingState()
    {
        SlotsController.Instance.isUnlocking = true;
        InitialiseViewUIForUnlockingChest();
        StartCoroutine(chestController.StartTimer());

    }

    public void OpenInstantly()
    {
        InitializeEmptyChestView();
        ReceiveChestRewards();
        ChestService.Instance.selectedController = chestController;
        slotReference.isEmpty = true;
        ChestService.Instance.ToggleRewardsPopup(true);
        slotReference.chestController = null;
    }

    public void EnteringUnlockedState()
    {
        SlotsController.Instance.isUnlocking = false;
        InitialiseViewUIForUnlockedChest();
        chestTimerTxt.text = "OPEN!";
    }

    public void OpenChest()
    {
        InitializeEmptyChestView();
        ReceiveChestRewards();
        slotReference.isEmpty = true;
        slotReference.chestController = null;
    }

    public void ReceiveChestRewards()
    {
        ResourceHandler.Instance.IncreaseCoins(chestController.chestModel.CoinsReward);
        ResourceHandler.Instance.IncreaseGems(chestController.chestModel.GemsReward);
    }


}
