using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI difficultyLabel;
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private OptionsPopup optionsPopup;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnOKButton()
    {
        PlayerPrefs.SetInt("difficulty", (int)difficultySlider.value);
        Close();
        optionsPopup.Open();
    }

    public void OnCancelButton()
    {
        Close();
        optionsPopup.Open();
    }
    public void Open()
    {
        difficultySlider.value = PlayerPrefs.GetInt("difficulty", 1);
        UpdateDifficulty(difficultySlider.value);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void UpdateDifficulty(float difficulty)
    {
        difficultyLabel.text = "Difficulty: " + ((int)difficulty).ToString();
    }

    public void OnDifficultyValueChanged(float difficulty)
    {
        UpdateDifficulty(difficulty);
    }

}
