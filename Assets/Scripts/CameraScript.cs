using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Components
    public Transform playerTransform;
    public Transform transform;
    public Transform endPoint;

    // Others
    public float y_endRange;
    public float x_difference; // Character movment tollerability
    public float y_difference; // /
    public float max_y_difference; // Max distance at which the camera follows the character and moves directly to that
    public float max_x_difference; // /
    public float x_move; // Movment sensistivity
    public float y_move; // /
    public float x_speed;
    public float y_speed;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            /// - - - - -  HORIZONTAL MOVMENT  - - - - - ///
            // The player is on the left of the camera
            if (playerTransform.position.x < transform.position.x)
            {
                // The player is too far from the camera
                if (transform.position.x - playerTransform.position.x > max_x_difference)
                {
                    transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
                }
                else if (transform.position.x - playerTransform.position.x > x_difference)
                {
                    // Moving the camera to the left
                    float step = x_speed * Time.deltaTime;
                    float target_pos = transform.position.x - x_move;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target_pos,transform.position.y,transform.position.z), step);
                }
            }

            // The player is on the right of the camera
            if (playerTransform.position.x > transform.position.x)
            {
                // The player is too far from the camera
                if (playerTransform.position.x - transform.position.x > max_x_difference)
                {
                        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
                }
                else if (playerTransform.position.x - transform.position.x > x_difference)
                {
                    // Moving the camera to the right
                    float step = x_speed * Time.deltaTime;
                    float target_pos = transform.position.x + x_move;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target_pos,transform.position.y,transform.position.z), step);
                    
                } 
            }
            ///  - - - - - -  ///


            /// - - - - -  VERTICAL MOVMENT  - - - - - ///
            if (playerTransform.position.y < transform.position.y)
            {
                if (transform.position.y - playerTransform.position.y > max_y_difference)
                {
                    Move_y(4, false);
                }
                if (transform.position.y - playerTransform.position.y > y_difference)
                {
                    Move_y(1, false);
                }
            }

            if (playerTransform.position.y > transform.position.y)
            {
                if (playerTransform.position.y - transform.position.y > y_difference)
                {
                    Move_y(1, true);
                } 
            }
            ///  - - - - -  ///
        }

        // If the player reached the end the camera stops from following him
        canMove = !((endPoint.position.y - y_endRange) < playerTransform.position.y && 
            playerTransform.position.y < (endPoint.position.y + y_endRange) &&
            playerTransform.position.x >= endPoint.position.x);
    }


    void Move_y(float mult, bool going_up)
    {
        float step = y_speed * Time.deltaTime * mult;
        float target_y_pos;
        
        if (going_up)
        {
            target_y_pos = transform.position.y + y_move;
        }
        else
        {
            target_y_pos = transform.position.y - y_move;
        }

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(transform.position.x, target_y_pos, transform.position.z), 
            step);
    }   
}
