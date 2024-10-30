using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class tetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    
    private float prevTime;
    public float fallTime = 1f;

    public static double leftt = -.001;
    public static double rightt = 2.00001;
    public static double bottomm = 0;

    [SerializeField] private object leftButton;
    [SerializeField] private object  rightButton;
    [SerializeField] private object downButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - prevTime > fallTime) 
        {
            transform.position += new Vector3(0, -.25f, 0);
            if(!ValidMove())
                transform.position -= new Vector3(0, -.25f, 0);
            prevTime = Time.time;

        }

    }

    
    public void MoveLeft()
    {
        

            transform.position += new Vector3(-.25f, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-.25f, 0, 0);
            }
       
    }
    
    public void MoveRight()
    {
        transform.position += new Vector3(.25f, 0, 0);
        if (!ValidMove())
        {
            transform.position -= new Vector3(.25f, 0, 0);
        }
    }
    public void MoveDown()
    {
        for (int i = 0; i < 15; i++)
        {
            
                transform.position += new Vector3(0, -.25f, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -.25f, 0);
                }
                prevTime = Time.time;
            
        }
    }

    public void Rotate()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1f), 90); 
        if (!ValidMove())
        { 
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 0.1f), -90);
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < leftt || roundedX > rightt || roundedY < bottomm)
            {
                return false;
            }
            
        }
        return true;
    }
}
