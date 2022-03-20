using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [SerializeField] private bool drawAIPrediction;
    [SerializeField] private KeyCode getTrajectoryKey;
    [SerializeField] private int aiMovementSpeed;
    [SerializeField] private Transform walls;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject leftPaddle;
    [SerializeField] private GameObject rightPaddle;
    [SerializeField] private LineRenderer line;
    [SerializeField] private int maxPhysicsFrameIterations = 100;

    private Scene simulationScene;
    private PhysicsScene2D physicsScene;
    private PaddleMovement leftScript;
    private PaddleMovement rightScript;
    private BallMovement ballScript;
    private Rigidbody2D ballRB;
    private GameObject ghostBall;

    private void Start()
    {
        leftScript = leftPaddle.GetComponent<PaddleMovement>();
        rightScript = rightPaddle.GetComponent<PaddleMovement>();
        ballScript = ball.GetComponent<BallMovement>();
        ballRB = ball.GetComponent<Rigidbody2D>();

        CreatePhysicsScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(getTrajectoryKey))
            SimulateTrajectory(ball, true);

        if (leftScript.SimpleAI && leftScript.BetterAI || rightScript.SimpleAI && rightScript.BetterAI)
            return;

        if (leftScript.SimpleAI && ballScript.GoingLeft)
        {
            Vector2 sim = SimulateOneStep(ball);
            if (sim.y > leftScript.boundY)
                leftScript.MoveToPosition(leftScript.boundY, false);
            else if (sim.y < -leftScript.boundY)
                leftScript.MoveToPosition(-leftScript.boundY, false);
            else
                leftScript.MoveToPosition(sim.y, false);
        }

        if (rightScript.SimpleAI && !ballScript.GoingLeft)
        {
            Vector2 sim = SimulateOneStep(ball);
            if (sim.y > rightScript.boundY)
                rightScript.MoveToPosition(rightScript.boundY, false);
            else if (sim.y < -rightScript.boundY)
                rightScript.MoveToPosition(-rightScript.boundY, false);
            else
                rightScript.MoveToPosition(sim.y, false);

        }

        if (leftScript.BetterAI && ballScript.GoingLeft)
        {
            SimulateTrajectory(ball, drawAIPrediction);

            for (int i = 0; i < line.positionCount; i++)
            {
                Vector2 pos = line.GetPosition(i);

                if (pos.x < -7.75f)
                {
                    if (pos.y > leftScript.boundY)
                        leftScript.MoveToPosition(leftScript.boundY);
                    else if (pos.y < -leftScript.boundY)
                        leftScript.MoveToPosition(-leftScript.boundY);
                    else
                        leftScript.MoveToPosition(pos.y);

                    break;
                }
            }
        }

        if (rightScript.BetterAI && !ballScript.GoingLeft)
        {
            SimulateTrajectory(ball, drawAIPrediction);

            for (int i = 0; i < line.positionCount; i++)
            {
                Vector2 pos = line.GetPosition(i);

                if (pos.x < 7.75f)
                {
                    if (pos.y > rightScript.boundY)
                        rightScript.MoveToPosition(rightScript.boundY);
                    else if (pos.y < -rightScript.boundY)
                        rightScript.MoveToPosition(-rightScript.boundY);
                    else
                        rightScript.MoveToPosition(pos.y);

                    break;
                }
            }
        }
    }

    private Vector2 SimulateOneStep(GameObject ball)
    {
        // create a new ball instance and move to the physics scene
        ghostBall = Instantiate(ball);
        SceneManager.MoveGameObjectToScene(ghostBall, simulationScene);

        // get the script
        BallMovement ghostScript = ghostBall.GetComponent<BallMovement>();

        // init the ball and set it's velocity
        ghostScript.Init(true);
        ghostScript.SetVelocity(ballRB.velocity);

        line.positionCount = maxPhysicsFrameIterations;

        // simulate the physics
        physicsScene.Simulate(Time.fixedDeltaTime);

        Vector2 position = ghostBall.transform.position;

        // destroy the ball when finished
        Destroy(ghostBall);

        return position;
    }

    void CreatePhysicsScene()
    {
        // create a new physics scene
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        // move every wall to the new scene
        foreach(Transform obj in walls)
        {
            GameObject ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = true;

            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    public void SimulateTrajectory(GameObject ball, bool draw = true)
    {
        // create a new ball instance and move to the physics scene
        ghostBall = Instantiate(ball);
        SceneManager.MoveGameObjectToScene(ghostBall, simulationScene);

        // get the script
        BallMovement ghostScript = ghostBall.GetComponent<BallMovement>();

        // init the ball and set it's velocity
        ghostScript.Init(true);
        ghostScript.SetVelocity(ballRB.velocity);

        line.positionCount = maxPhysicsFrameIterations;

        // simulate the physics
        for (int i = 0; i < maxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);

            line.SetPosition(i, ghostBall.transform.position);
        }

        line.enabled = draw;

        // destroy the ball when finished
        Destroy(ghostBall);
    }
}
