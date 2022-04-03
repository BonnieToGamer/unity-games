using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HealthbarManager : MonoBehaviour
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private RectTransform canvasRect;

    private List<Slider> HealthBars;

    // Start is called before the first frame update
    void Start()
    {
        HealthBars = new List<Slider>();
        IEnumerable<IEnemy> enemies = FindObjectsOfType<MonoBehaviour>().OfType<IEnemy>();
        Debug.Log(enemies.Count());
        foreach (IEnemy enemy in enemies)
        {
            Debug.Log(enemy.Name);
            GameObject temp = Instantiate(HealthBar);
            temp.transform.parent = transform;
            temp.name = enemy.Name + " Healthbar";
            Slider slider = temp.GetComponent<Slider>();

            // Offset position above object bbox (in world space)
            float offsetPosY = enemy.Position.y + .75f;

            // Final position of marker above GO in world space
            Vector3 offsetPos = new Vector3(enemy.Position.x, offsetPosY, 0);

            // Calculate *screen* position (note, not a canvas/recttransform position)
            Vector2 canvasPos;
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

            // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

            // Set
            temp.transform.localPosition = canvasPos;

            HealthBars.Add(slider);
            enemy.HealthBar = slider;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
