using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPotion : MonoBehaviour
{
    public Transform target;
    public Transform throwPoint;
    public GameObject fire_potion;
    public float timeTillHit = 1f;

    void Start()
    {
        //Throw();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y + 1);

            Vector2 direction = target - myPos;
            direction.Normalize();
            GameObject projectile = Instantiate(fire_potion, myPos, Quaternion.identity) as GameObject;
            Rigidbody2D rigid;
            rigid = projectile.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * 20.0f;
        }
    }

    void Throw()
    {
        float xdistance;
        xdistance = target.position.x - throwPoint.position.x;

        float ydistance;
        ydistance = target.position.y - throwPoint.position.y;

        float throwAngle; // in radian
                          //OLD
                          //throwAngle = Mathf.Atan ((ydistance + 4.905f) / xdistance);
                          //UPDATED
        throwAngle = Mathf.Atan((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);
        //OLD
        //float totalVelo = xdistance / Mathf.Cos(throwAngle) ;
        //UPDATED
        float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);

        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos(throwAngle);
        yVelo = totalVelo * Mathf.Sin(throwAngle);

        GameObject potionInstance = Instantiate(fire_potion, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        Rigidbody2D rigid;
        rigid = potionInstance.GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector2(xVelo, yVelo);
    }
}
