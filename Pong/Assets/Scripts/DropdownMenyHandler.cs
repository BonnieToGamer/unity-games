using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMenyHandler : MonoBehaviour
{
    [SerializeField] private Button ArrowButton;
    [SerializeField] private GameObject Menu;
    [SerializeField] private BallMovement Ball;
    [SerializeField] private PaddleMovement LeftPaddle;
    [SerializeField] private PaddleMovement RightPaddle;
    [SerializeField] private Toggle LeftFutureAiToggle;
    [SerializeField] private Toggle LeftNowAiToggle;
    [SerializeField] private Toggle RightFutureAiToggle;
    [SerializeField] private Toggle RightNowAiToggle;
    [SerializeField] private Toggle BallMovementToggle;
    [SerializeField] private float ButtonUpPosition;
    [SerializeField] private float ButtonDownPosition;
    [SerializeField] private float MenuOffset;

    void Update()
    {
        LeftPaddle.BetterAI = LeftFutureAiToggle.isOn;
        LeftPaddle.SimpleAI = LeftNowAiToggle.isOn;

        RightPaddle.BetterAI = RightFutureAiToggle.isOn;
        RightPaddle.SimpleAI = RightNowAiToggle.isOn;

        Ball.newBounceMethod = BallMovementToggle.isOn;
    }

    private bool down = false;
    public void Move()
    {
        if (down)
            MoveUp();
        else
            MoveDown();

        down = !down;
    }

    private void MoveDown()
    {
       Animate(ref ButtonDownPosition);
       Ball.PauseBall();
    }

    private void MoveUp()
    {
        Animate(ref ButtonUpPosition);
        Ball.PauseBall();
    }

    private void Animate(ref float btnPos)
    {
        RectTransform btnRect = ArrowButton.GetComponent<RectTransform>();
        RectTransform menRect = Menu.GetComponent<RectTransform>();
        ArrowButton.enabled = false;
        StartCoroutine(SmoothMove(btnRect.anchoredPosition, new Vector2(btnRect.anchoredPosition.x, btnPos), 75, 100, 
        (res) =>
        {
            btnRect.anchoredPosition = res;
            menRect.anchoredPosition = new Vector2(menRect.anchoredPosition.x, res.y + MenuOffset);
        }, 
        () => 
        {
            ArrowButton.enabled = true;
        }));
        
        Text buttonText = ArrowButton.GetComponentInChildren<Text>();
        buttonText.transform.Rotate(0, 180, 0);
    }

    private IEnumerator SmoothMove(Vector2 origin, Vector2 target, float speed, int maxIterations, System.Action<Vector2> callback, System.Action finished = null)
    {
        int iterations = 0;
        while (origin != target)
        {
            origin = Vector2.Lerp(origin, target, speed * Time.deltaTime);
            callback(origin);

            if (++iterations > maxIterations)
                origin = target;

            yield return null;
        }

		finished?.Invoke();
	}

    private IEnumerator Smooth(float a, float b, float speed, System.Action<float> callback)
    {
        while (a != b) {
            a = Lerp(a, b, speed * Time.deltaTime);
            callback(a);
            yield return null;
        }
    }

    private float Lerp(float a, float b, float t) 
    {
        return a+(b-a)*t;
    }
}
