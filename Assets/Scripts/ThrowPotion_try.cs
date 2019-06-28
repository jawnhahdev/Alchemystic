using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPotion_try : MonoBehaviour
{
    public GameObject TrajectoryPointPrefeb;
    public GameObject PotionPrefab;

    private GameObject potion;
    private bool isPressed, isPotionThrown;
    private float power = 25;
    private int numOfTrajectoryPoints = 50;
    private List<GameObject> trajectoryPoints;

    void Start()
    {
        trajectoryPoints = new List<GameObject>();
        isPressed = isPotionThrown = false;
        //   TrajectoryPoints are instatiated
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            GameObject dot = (GameObject)Instantiate(TrajectoryPointPrefeb);
            Renderer renderer = dot.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            trajectoryPoints.Insert(i, dot);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            if (!potion)
                createPotion();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            if (!isPotionThrown)
            {
                throwPotion();
            }
        }
        // when mouse button is pressed, cannon is rotated as per mouse movement and projectile trajectory path is displayed.
        if (isPressed)
        {
            Vector3 vel = GetForceFrom(potion.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
            Rigidbody2D rigid = potion.GetComponent<Rigidbody2D>();
            setTrajectoryPoints(transform.position, vel / rigid.mass);
        }


    }

    //---------------------------------------    
    // Following method creates new Potion
    //---------------------------------------    
    private void createPotion()
    {
        potion = (GameObject)Instantiate(PotionPrefab);
        Vector3 pos = transform.position;
        pos.z = 1;
        potion.transform.position = pos;
        potion.SetActive(false);
    }
    //---------------------------------------    
    // Following method gives force to the Potion
    //---------------------------------------    
    private void throwPotion()
    {
        potion.SetActive(true);
        Rigidbody2D rigid = potion.GetComponent<Rigidbody2D>();
        rigid.gravityScale = 1;
        rigid.AddForce(GetForceFrom(potion.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)), ForceMode2D.Impulse);
        isPotionThrown = true;
    }
    //---------------------------------------    
    // Following method returns force by calculating distance between given two points
    //---------------------------------------    
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
    }
    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(Potion) and initial velocity of object(Potion).
    //---------------------------------------    
    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            trajectoryPoints[i].transform.position = pos;
            Renderer renderer = trajectoryPoints[i].GetComponent<SpriteRenderer>();
            renderer.enabled = true;
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }
}
