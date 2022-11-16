using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;

    Rigidbody2D rb;
    float movement;
    float direction;
    float directionRemember = 1f;
    float levelWidth;

    CameraFollow cameraFollow;

    Vector3 maxHeightAchieved;

    int plateformReached = -1;
    int nbTimesPlatfomsReach;
    
    float minHeight;

    bool alive = false;

    List<float> directionsToFollow;
    int indiceDirectionToFollow;

    float jumpForceMax = 10f;
    float jumpForce;
    List<float> jumpForcesToFollow;

    List<int> platformsToFollow;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        updateDirection();
    }

    void Update()
    {
        if (alive)
        {
            move();
            updateCamera();
            updateMaxHeight();
        }
        else
        {
            if (directionRemember < 0)
            {
                transform.Rotate(0f, 180.0f, 0.0f, Space.Self);
                direction = 0;
                directionRemember = 1f;

                jumpForce = jumpForceMax;

            }
            transform.position = maxHeightAchieved;
        }
    }

    void updateDirection()
    {
        if (indiceDirectionToFollow < directionsToFollow.Count)
        {
            direction = directionsToFollow[indiceDirectionToFollow];

            jumpForce = jumpForcesToFollow[indiceDirectionToFollow];

            indiceDirectionToFollow++;
        }
        else
        {
            direction = Random.Range(-1, 2);

            directionsToFollow.Add(direction);

            jumpForce = (int)Random.Range(5, jumpForceMax);
            jumpForcesToFollow.Add(jumpForce);

            indiceDirectionToFollow++;
        }
    }

    public void resetPlayer(Vector3 defaultPosition, float defaultMinHeight)
    {
        this.minHeight = defaultMinHeight;
        this.transform.position = defaultPosition;
        this.maxHeightAchieved = defaultPosition;
        this.indiceDirectionToFollow = 0;
        this.alive = true;

        direction = 0;
        directionRemember = 1f;

        nbTimesPlatfomsReach = 0;

        this.jumpForce = jumpForceMax;

        movement = 0;
        Vector2 velocity = Vector3.zero;
        velocity.x = movement;
        rb.velocity = velocity;
    }

    public void resetPlayer(List<float> directions, List<float> jumpForces, List<int> platforms, Vector3 defaultPosition, float defaultMinHeight)
    {
        directionsToFollow = directions;
        jumpForcesToFollow = jumpForces;
        platformsToFollow = platforms;
        resetPlayer(defaultPosition, defaultMinHeight);
    }

    void updateMaxHeight()
    {
        if(maxHeightAchieved.y < transform.position.y)
        {
            maxHeightAchieved = transform.position;
        }
    }

    void updateCamera()
    {
        cameraFollow.setMaxHeightAchieved(maxHeightAchieved);
    }

    void move()
    {
        movement = direction * movementSpeed;

        if ((directionRemember < 0 && direction > 0) || (directionRemember > 0 && direction < 0))
        {
            transform.Rotate(0f, 180.0f, 0.0f, Space.Self);
            directionRemember = direction;
        }
        if (direction < 0 && transform.position.x < -levelWidth)
        {
            transform.position = new Vector3(levelWidth, transform.position.y, 0);
        }
        else if (direction > 0 && transform.position.x > levelWidth)
        {
            transform.position = new Vector3(-levelWidth, transform.position.y, 0);
        }
        
        if(transform.position.y < (cameraFollow.getPosition().y - 12))
        {
            alive = false;
            Debug.Log("alive = false");
        }
    }

    public void initialisation(Vector3 defaultPosition, float new_minHeight, float new_levelWidth, List<float> directions, List<float> jumpForces, List<int> platforms)
    {
        movement = 0f;
        direction = 0f;
        this.transform.position = defaultPosition;
        this.minHeight = new_minHeight;
        this.levelWidth = new_levelWidth*2;
        this.directionsToFollow = directions;

        this.jumpForcesToFollow = jumpForces;
        this.platformsToFollow = platforms;

        this.indiceDirectionToFollow = 0;

        maxHeightAchieved = transform.position;

        this.alive = true;
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 position = collision.gameObject.transform.position;
        if (position != null && collision.relativeVelocity.y > 0f)
        {
            Platform p = collision.gameObject.GetComponent<Platform>();
            if (p.getNumero() != plateformReached)
            {
                plateformReached = p.getNumero();
                nbTimesPlatfomsReach = 0;
            }
            else
            {
                nbTimesPlatfomsReach++;
                if(nbTimesPlatfomsReach == 5)
                {
                    alive = false;
                }
            }

            if(position.y >= minHeight)
            {
                minHeight = position.y;

                if (indiceDirectionToFollow >= directionsToFollow.Count)
                {
                    platformsToFollow.Add(p.getNumero());
                }
                updateDirection();
            }
            else if(position.y < (minHeight - 2))
            {
                alive = false;
            }

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;
            }
        }
    }


    public bool isAlive()
    {
        return alive;
    }

    public float getMaxHeightAchieved()
    {
        return maxHeightAchieved.y;
    }

    public int getNbDirectionsFollowed()
    {
        return directionsToFollow.Count;
    }

    public List<float> getDirectionsToFollow()
    {
        return directionsToFollow;
    }

    public List<float> getJumpForcesToFollow()
    {
        return jumpForcesToFollow;
    }

    public int getNbJumpForcesFollowed()
    {
        return jumpForcesToFollow.Count;
    }

    public List<int> getPlateformToFollow()
    {
        return platformsToFollow;
    }

    public int getNbPlatformFollowed()
    {
        return platformsToFollow.Count;
    }
}
