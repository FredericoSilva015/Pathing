﻿using UnityEngine;
using System.Collections;

public  class UnitAir : MonoBehaviour {

    //vars para selecção
    public bool selected = false;
    private bool selectedByClick = false;
    //shader
    private Shader outlineShader, standardShader;
    //movement
    private Vector3 moveToDest = Vector3.zero;
    public float floorOffset = -1;
    public float speed = 3;
    public float stopDistanceOffSet = 0.5f;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        outlineShader = Shader.Find("Outlined/Silhouetted Diffuse");
        standardShader = Shader.Find("Standard");
    }

   

    // Update is called once per frame
    void Update()
    {
        CheckUnitSelection();
    }

    void CheckUnitSelection()
    {
        if (GetComponent<Renderer>() && Input.GetMouseButton(0))
        {
            if (!selectedByClick)
            {
                Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
                camPos.y = CameraController.InvertMouseY(camPos.y);
                selected = CameraController.selection.Contains(camPos);
            }

            if (selected)
            {
                GetComponent<Renderer>().material.shader = outlineShader;
            }
            else
            {
                GetComponent<Renderer>().material.shader = standardShader;
            }

        }

        //Obtem o destino do click direito
        if (selected && Input.GetMouseButtonUp(1))
        {
            Vector3 destination = CameraController.GetDestination();

            //se o destino for um valor diferente de zero
            if (destination != Vector3.zero)
            {
                moveToDest = destination;
                moveToDest.y = floorOffset;
            }
        }
        UpdateMove();
    }

    private void UpdateMove()
    {
        if (moveToDest != Vector3.zero && moveToDest != transform.position)
        {
            Vector3  direction = (moveToDest - transform.position).normalized * Time.deltaTime * speed;
            direction.y = 0;
            
            CharacterController controller = GetComponent<CharacterController>();
            controller.Move(direction);

            if (Vector3.Distance(transform.position, moveToDest) < stopDistanceOffSet)
            {

                moveToDest = Vector3.zero;
                direction = Vector3.zero;
            }
        }
    }

    private void OnMouseDown()
    {
        selectedByClick = true;
        selected = true;
    }

    private void OnMouseUp()
    {
        if (selectedByClick)
            selected = true;
        selectedByClick = false;

    }
}
