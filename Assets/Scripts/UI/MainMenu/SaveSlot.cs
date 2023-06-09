using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject emptySlot;
    [SerializeField] private GameObject hasDataSlot;
    [SerializeField] private TMP_Text levelText;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // There is data for this profile id
        if (data != null)
        {
            emptySlot.SetActive(false);
            hasDataSlot.SetActive(true);

            levelText.text = "Level : " + data.GetGameLevel();
        }
        // No data for this profileId
        else
        {
            emptySlot.SetActive(true);
            hasDataSlot.SetActive(false);
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
