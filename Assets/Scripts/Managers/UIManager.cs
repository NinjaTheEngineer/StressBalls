using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject SizeSelection;
    public Color PressedButtonColor;
    public Slider SizeSlider; //Slider component
    public TextMeshProUGUI Text_SliderMinValue, Text_SliderMaxValue; //Slider Max and Min Values
    public GameObject ColorSelectionGO; // Object of the color selection part of the UI
    public GameObject SliderContainer; // Slider and its texts Container 
    public GameObject ColorButtonPrefab; //Color button prefab
    public List<GameObject> ColorButtons; //List of all color buttons Instantiated
    public StressBallManager StressBallManager;
    public TextMeshProUGUI TextCurrentRadius;
    private string currentRadiusString = "Current Radius ";
    private StressBallSO currentStressBall;
    private bool changedSize = false;
    private Button[] sizeButtons;

    private void Start()
    {
        sizeButtons = SizeSelection.GetComponentsInChildren<Button>();
        InstantiateColorButtons();
        UpdateSizeSlider();
        OnColorSelected(0);
    }
    private void InstantiateColorButtons() //Instantiate all the colored buttons
    {
        currentStressBall = StressBallManager.GetCurrentStressBall();
        if (currentStressBall == null) { 
            Debug.LogWarning("No stressball instantiated");
            return;
        }

        if (ColorButtons.Count > 0) //If list initialized
        {

            //Handles the colored buttons that should be active and available
            for (int i = 0; i < currentStressBall.possibleColors.Length; i++) // 3
            {
                for (int j = 0; j < ColorButtons.Count; j++) // 2
                {
                    if (j > i)
                    {
                        ColorButtons[j].SetActive(false);
                    }
                    else if (j.Equals(i))
                    {
                        int colorIndex = j;
                        ColorButtons[j].SetActive(true);

                        ColorButtons[j].GetComponent<Button>().onClick.RemoveAllListeners();
                        ColorButtons[j].GetComponent<Button>().onClick.AddListener(
                            () => OnColorSelected(colorIndex));
                        ColorButtons[j].GetComponent<Image>().color = currentStressBall.possibleColors[j];
                    }
                }
                if (i >= ColorButtons.Count)
                {
                    GameObject currentButton = Instantiate(ColorButtonPrefab, ColorSelectionGO.transform);
                    currentButton.GetComponent<Image>().color = currentStressBall.possibleColors[i];
                    int colorIndex = i;
                    currentButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        OnColorSelected(colorIndex);
                    });
                    ColorButtons.Add(currentButton);
                }
            } 
        }
        else //Else Initialize the buttons list
        {
            for (int color = 0; color < currentStressBall.possibleColors.Length; color++)
            {
                GameObject currentButton = Instantiate(ColorButtonPrefab, ColorSelectionGO.transform);
                currentButton.GetComponent<Image>().color = currentStressBall.possibleColors[color];
                int colorIndex = color;
                currentButton.GetComponent<Button>().onClick.AddListener(
                    () => OnColorSelected(colorIndex));
                ColorButtons.Add(currentButton);
            }
        }

    }
    public void OnSizeSelected(int i) //Receives the size selected
    {
        changedSize = true;
        StressBallManager.OnSizeChanged(i);
        for (int index = 0; index < sizeButtons.Length; index++)
        {
            if (index.Equals(i))
                sizeButtons[index].GetComponent<Image>().color = PressedButtonColor;
            else
                sizeButtons[index].GetComponent<Image>().color = Color.white;
        }
        UpdateSizeSlider();
        InstantiateColorButtons();
        if (currentStressBall.isBlack)
        {
            UpdateCurrentRadiusText(StressBallManager.GetCurrentStressBall().sizeIfBlack);
            SliderContainer.gameObject.SetActive(false);
        }
        else
        {
            SliderContainer.gameObject.SetActive(true);

        }
    } //Handles on size type selected
    private void OnColorSelected(int index)
    {
        StressBallManager.OnColorSelected(index);

        if (StressBallManager.GetCurrentStressBall().isBlack)
        {
            SliderContainer.gameObject.SetActive(false);
            OnSizeSelected(sizeButtons.Length - 1);
        }
        else
        {
            SliderContainer.gameObject.SetActive(true);
        }
        UpdateCurrentRadiusText(StressBallManager.GetCurrentStressBall().GetSize());

    } //Handles on color selected
    public void OnSliderValueChange(float value)
    {
        value = Mathf.Round(value * 10.0f) * 0.1f;
        UpdateCurrentRadiusText(value);
        if (!changedSize)
            StressBallManager.UpdateCurrentSize(value);
        else
            changedSize = false;
    } //Handles slider value changes
    private void UpdateCurrentRadiusText(float radius)
    {
        TextCurrentRadius.text = currentRadiusString + "<b><color=#000000>" +
                                radius.ToString("0.0") + "</color></b> cm.";
    } //Updates radius current text
    private void UpdateSizeSlider()
    {
        currentStressBall = StressBallManager.GetCurrentStressBall();

        SizeSlider.minValue = currentStressBall.minSize;

        Text_SliderMinValue.text = currentStressBall.minSize.ToString("0.0") + "cm";
        SizeSlider.maxValue = currentStressBall.maxSize;
        Text_SliderMaxValue.text = currentStressBall.maxSize.ToString("0.0") + "cm";
        SizeSlider.value = currentStressBall.currentSize;
    } //Updates the size of the Slider
}
