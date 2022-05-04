using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singelton
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

#endregion

    [SerializeField] private TextAsset[] levels;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<Song> Songs;

    public Rect ScreenSize;

    private Level level;
    private ObjectPooler objectPooler;

    #region classes
    [System.Serializable]
    public class Song
    {
        public string Name;
        public AudioClip Audio;
    }

    [System.Serializable]
    public class Bullet
    {
        public float[] direction;
        public float speed;
        public float time;
        public float[] pos;
    }

    [System.Serializable]
    public class Level
    {
        public string song;
        public Bullet[] objects;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CalculateScreenSize(0, Vector3.zero);
        objectPooler = ObjectPooler.Instance;
        level = JsonUtility.FromJson<Level>(levels[0].text);
        StartCoroutine(nameof(StartLevel));
    }

    public IEnumerator StartLevel()
    {
        audioSource.PlayOneShot(Songs.Find(s => s.Name == level.song).Audio);
        foreach (Bullet obj in level.objects)
        {
            yield return new WaitForSecondsRealtime(obj.time - 0.01f);
            BulletMovement bulletScript = objectPooler.SpawnFromPool("Bullet", new Vector2(obj.pos[0], obj.pos[1]), Quaternion.identity).GetComponent<BulletMovement>();
            bulletScript.Direction = new Vector2(obj.direction[0], obj.direction[1]);
            bulletScript.Speed = obj.speed;
        }
    }

    public void CalculateScreenSize(float distance, Vector3 pointSize)
    {
        distance -= Camera.main.transform.position.z;
        ScreenSize = new Rect
        {
            max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance)) - (pointSize / 2),
            min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)) + (pointSize / 2)
        };

    }
}
