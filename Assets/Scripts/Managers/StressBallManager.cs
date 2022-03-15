using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StressBallManager : MonoBehaviour
{
    [SerializeField] private StressBallSO[] StressBallTypes;

    [SerializeField] private GameObject StressBallGO;

    private StressBallSO SelectedStressBall;
    private MeshRenderer meshRendererStressBall; //Stressball Mesh Renderer


    public StressBallSO GetCurrentStressBall()
    {
        return SelectedStressBall;
    }

    private void Awake()
    {
        SelectedStressBall = StressBallTypes[0];
        meshRendererStressBall = StressBallGO.GetComponent<MeshRenderer>();
        meshRendererStressBall.material.color = SelectedStressBall.selectedColor;
    }
    
    private void FixedUpdate() //Updates Stressball Size each frame
    {
        UpdateStressBallSize();
    }
    public void OnSizeChanged(int i)
    {
        SelectedStressBall = StressBallTypes[i];
        meshRendererStressBall.material.color = SelectedStressBall.selectedColor;
    }
    public void OnColorSelected(int index)
    {
        Color selectedColor = SelectedStressBall.possibleColors[index];

        if (!SelectedStressBall.sizeType.Equals(SizeType.LARGE) &&
            SelectedStressBall.possibleColors[index].Equals(Color.black))
        {
            SelectedStressBall = StressBallTypes[StressBallTypes.Length - 1];
        }

        SelectedStressBall.ChangeColor(selectedColor);
        meshRendererStressBall.material.color = selectedColor;
    }

    public void UpdateCurrentSize(float value) //Update current stressball size
    {
        SelectedStressBall.currentSize = value;
    } 

    private void UpdateStressBallSize() //Update the scale of the stressball
    {
        float radiusToScale = SelectedStressBall.GetSize() * 2;
        StressBallGO.transform.localScale = new Vector3(radiusToScale, radiusToScale, radiusToScale);
    }

}
