using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] Levels;
    public GameObject Player;
    public int StartLevel = 0;

    private static GameObject[] _Levels;
    private static GameObject _Player;
    private static int _CurrentLevel;
    private static GameObject _Level;

    // Start is called before the first frame update
    void Start()
    {
        _CurrentLevel = StartLevel >= Levels.Length ? 0 : StartLevel;
        _Levels = Levels;
        _Player = Player;
        BuildLevel();
    }

    public static void ChangeLevel()
    {
        _CurrentLevel = ++_CurrentLevel >= _Levels.Length ? 0 : _CurrentLevel;

        Destroy(_Level);
        BuildLevel();
    }

    private static void BuildLevel()
    {
        _Level = Instantiate(_Levels[_CurrentLevel]);
        Transform spawnPoint = _Level.transform.Find("SpawnPoint");
        _Player.transform.position = spawnPoint.position;
    }
}
