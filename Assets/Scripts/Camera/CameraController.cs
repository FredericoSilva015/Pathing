using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {


    //vars de controlo do select através da camara
    public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0, 0, 0, 0);
    private Vector3 startClick = -Vector3.one;

    private static Vector3 moveToDestination = Vector3.zero;
    private static List<string> passables = new List<string>() { "Plane" };
   

	
	// Update is called once per frame
	void Update () 
    {
        CheckSelection();
        CleanUp();
	}

    /// Function to process the selection process
    void CheckSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startClick = Input.mousePosition; 
        }
        else if (Input.GetMouseButtonUp(0))         
        { 
            startClick = -Vector3.one; 
        }


        if (Input.GetMouseButton(0))
        {
            selection = new Rect(startClick.x, InvertMouseY(startClick.y),
            Input.mousePosition.x - startClick.x,
            InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));

            //os valores da posição width e height são alterados, caso sejam negativos passam a ser positivos
            if (selection.width < 0)
            {
                selection.x += selection.width;
                selection.width = -selection.width;
            }
            if (selection.height < 0)
            {
                selection.y += selection.height;
                selection.height = -selection.height;
            }
        }
    }

    //desenha a caixa de selecção no ecrã 
    private void OnGUI()
    {
        if (startClick != -Vector3.one)
        {
            //atribui a cor branca?? aos bordos??, o elemento final representa a transparencia do interior da caixa
            GUI.color = new Color(1, 1, 1, 0.5f);
            //textura seleccionada
            GUI.DrawTexture(selection, selectionHighlight);
        }
    }

    //inversor da coordenada Y do ecrã
    public static float InvertMouseY(float y)
    {
        return Screen.height - y;
    }

    private void CleanUp()
    {
        //quando detecta que o botão se levantou, moveToDestination passa para um valor Vector3.zero,
        //basicamente detecta se houve um click do botão direito
        if (!Input.GetMouseButtonUp(1))
            moveToDestination = Vector3.zero;
    }

    public static Vector3 GetDestination()
    {

        //se houve um click do botão direito
        if (moveToDestination == Vector3.zero)
        {
            //inicia um raycast
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            //dependendo no que raycast bater
            // se não bate no "floor"(o plano em que os objectos se situam) quebra o loop
            if (Physics.Raycast(r, out hit))
            {
                    while (!passables.Contains(hit.transform.gameObject.name))
                    {
                        
                        if (!Physics.Raycast(hit.point + r.direction * 0.1f, r.direction, out hit))
                            break;
                    }  
            }

            //bate no "Plane", marca posição
            if (hit.transform == true)                
                moveToDestination = hit.point;
            
        }
       
        return moveToDestination;
    }
}
