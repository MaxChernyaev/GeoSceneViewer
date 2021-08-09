using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionONOFF : MonoBehaviour
{
	[SerializeField] private GameObject instruction;
    private bool hideFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleInstructionONOFF()
    {
        hideFlag = !hideFlag;
    	//instruction.SetActive(false);
        if(hideFlag == true)
        {
            Vector3 transf = new Vector3(0,0,0);
            transf.x = instruction.transform.position.x;
            transf.y = instruction.transform.position.y + 305;
            instruction.transform.position = transf;
        }
        else
        {
            Vector3 transf = new Vector3(0,0,0);
            transf.x = instruction.transform.position.x;
            transf.y = instruction.transform.position.y - 305;
            instruction.transform.position = transf;
        }
    }
}
