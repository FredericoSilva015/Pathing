using UnityEngine;
using System.Collections;

public class UnitSelection : MonoBehaviour {

    //vars para selecção
    public bool selected = false;
    private bool selectedByClick = false;
    //shader
    private Shader outlineShader, standardShader;
    //movement
    private Vector3 moveToDest = Vector3.zero;
    public float floorOffset = -1;
    public float speed =   3;
    public float stopDistanceOffSet = 0.5f;
    Vector3 direction = Vector3.zero;

	// Use this for initialization
	void Start () {
        outlineShader = Shader.Find("Outlined/Silhouetted Diffuse");
        standardShader = Shader.Find("Standard");
	}

    void FixedUpdate() 
    {
        if(direction != Vector3.zero)
        GetComponent<Rigidbody>().velocity = direction;
    }

	// Update is called once per frame
	void Update () 
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
                //navMesh
                //gameObject.GetComponent<NavMeshAgent>().SetDestination(destination);

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
            direction = (moveToDest - transform.position).normalized* Time.deltaTime*speed;
            direction.y = 0;
            transform.Translate(direction , Space.World);

            
         
            if (Vector3.Distance(transform.position, moveToDest) < stopDistanceOffSet)
            {
               
                moveToDest = Vector3.zero;
                direction = Vector3.zero;
            }
        }
        ////else
        ////{
        ////    Debug.Log("wut");
        ////    GetComponent<Rigidbody>().velocity = Vector3.zero;
        ////}
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
