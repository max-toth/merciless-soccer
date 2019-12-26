using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float shootPower;
    public float passPower;

    public GameObject ball;

    private float x;
    private float y;

    private Player[] teamMates;
    
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        teamMates = FindObjectsOfType<Player>().Where(player => player != this).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Debug.Log("X=" + x + " Y=" + y);

        var direction = new Vector3(
            x * speed * Time.deltaTime,
            y * speed * Time.deltaTime, 0f);

        transform.position += direction;
        Debug.Log("Player" + transform.position);

        if (ball != null)
        {
            Debug.DrawLine(ball.transform.position, new Vector3(x, y, 0f) * 10f, Color.yellow);

            if (x <= 0)
            {
                ball.transform.position = new Vector3(
                    transform.position.x - 1,
                    transform.position.y - 0.5f,
                    0f
                );
            }

            if (x > 0)
            {
                ball.transform.position = new Vector3(
                    transform.position.x + 1,
                    transform.position.y - 0.5f,
                    0f
                );
            }

            var targetPlayer = FindPlayerInDirection(direction);

            if (targetPlayer != null)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    PassBallToPlayer(targetPlayer);
                }
            }
        }

        if (Input.GetKey(KeyCode.D) && ball != null)
        {
            Shoot();
        }
    }

    private void PassBallToPlayer(Player targetPlayer)
    {
        var direction = DirectionTo(targetPlayer);
        Rigidbody2D rbody = ball.GetComponent<Rigidbody2D>();
        rbody.isKinematic = false;
        rbody.AddForce(
            new Vector2(
                direction.x * Time.deltaTime * passPower,
                direction.y * Time.deltaTime * passPower),
            ForceMode2D.Impulse);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.ToString());
        if (other.gameObject.name == "Ball")
        {
            ball = other.gameObject;
            Rigidbody2D ballBody = ball.GetComponent<Rigidbody2D>();
            ballBody.velocity = Vector2.zero;
        }
    }

    void Shoot()
    {
        Rigidbody2D ballBody = ball.GetComponent<Rigidbody2D>();
        ballBody.isKinematic = false;
        ballBody.AddForce(
            new Vector2(
                shootPower * Time.deltaTime * Input.GetAxis("Horizontal"),
                shootPower * Time.deltaTime * Input.GetAxis("Vertical")),
            ForceMode2D.Impulse
        );

        ball = null;
    }

    private Vector3 DirectionTo(Player player)
    {
        return Vector3.Normalize(player.transform.position - ball.transform.position);
    }

    private Player FindPlayerInDirection(Vector3 direction)
    {
        var closestAngle = teamMates
            .OrderBy(player => Vector3.Angle(direction, DirectionTo(player)))
            .FirstOrDefault();

        return closestAngle;
    }
}