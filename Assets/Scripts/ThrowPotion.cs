using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPotion : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerScript;

    public GameObject firePotionPrefab;
    public GameObject icePotionPrefab;
    public GameObject spacePotionPrefab;
    private GameObject potion;
    private Vector3 throwPoint;
    private Vector2 mouseOriginPoint;
    private Vector2 mouseDiff;
    private Vector2 force;

    private bool isAim;
    private int check;

    private List<GameObject> trajectoryPointsList;
    private const int NUM_OF_POINTS = 30;
    public GameObject pointPrefab;

    public Vector2 power;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();
        isAim = false;

        trajectoryPointsList = new List<GameObject>();
        //Intialize the trajectory point
        for(int i = 0; i < NUM_OF_POINTS; i++)
        {
            GameObject point = Instantiate(pointPrefab);
            point.SetActive(false);
            trajectoryPointsList.Add(point);
        }

    }

    // Update is called once per frame
    void Update()
    {
        throwPoint = transform.position;
        Debug.Log("ThrowPoint" + transform.position);
        if (Input.GetMouseButtonDown(0))
        {
            mouseOriginPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            potion = createPotion();

            isAim = true;
        }
        
        if (Input.GetMouseButtonUp(0) && potion)
        {
            Rigidbody2D potionRB2D = potion.GetComponent<Rigidbody2D>();
            potionRB2D.gravityScale = 1;
            potionRB2D.velocity = force;    //Throw potion
            
            clearTrajectoryPath();
            isAim = false;
        }

        if (isAim)
        {
            Vector2 mouseStretchPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            mouseDiff = mouseOriginPoint - mouseStretchPoint;

            angle = Vector2.SignedAngle(Vector2.right, mouseDiff);

            potion.transform.position = throwPoint;
            potion.GetComponent<Rigidbody2D>().gravityScale = 0;
            Vector2 velocity = power * mouseDiff;

            float force_x = velocity.x * Mathf.Cos(angle * Mathf.Deg2Rad);
            float force_y = velocity.y * Mathf.Sin(angle * Mathf.Deg2Rad);
            force = new Vector2(force_x, force_y);

            drawTrajectoryPath(angle, velocity);
            //Debug.Log("Cos("+angle+") = " + Mathf.Cos(angle));

        }

    }

    private void drawTrajectoryPath(float angle, Vector2 velocity)
    {
        float time = 0;
        float x_coordinate;
        float y_coordinate;

        for(int i = 0; i < NUM_OF_POINTS; i++)
        {
            time = i * 0.1f;
            x_coordinate = throwPoint.x + (velocity.x * Mathf.Cos(angle * Mathf.Deg2Rad) * time);
            y_coordinate = throwPoint.y + (velocity.y * Mathf.Sin(angle * Mathf.Deg2Rad) * time) - (Physics2D.gravity.magnitude / 2.0f * time * time);

            Vector2 coordinate = new Vector2(x_coordinate, y_coordinate);

            trajectoryPointsList[i].transform.position = coordinate;
            trajectoryPointsList[i].SetActive(true);
        }
    }

    private GameObject createPotion()
    {
        if (playerScript.fireMode)
            return Instantiate(firePotionPrefab, transform.position, transform.rotation);
        else if (playerScript.iceMode)
            return Instantiate(icePotionPrefab, transform.position, transform.rotation);
        else if (playerScript.spaceMode)
            return Instantiate(spacePotionPrefab, transform.position, transform.rotation);
        else
            return null;

    }
    private void clearTrajectoryPath()
    {
        for (int i = 0; i < NUM_OF_POINTS; i++)
        {
            trajectoryPointsList[i].SetActive(false);
        }
    }

}
