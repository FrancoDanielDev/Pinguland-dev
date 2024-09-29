using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class LocalScoreboard : MonoBehaviour
{
    [SerializeField] private string             _path;
    [SerializeField] private string             _fileName;
    [SerializeField] private TMP_InputField     _text;

    public ScoreboardInfo board;

    private void Awake()    { _path = Application.streamingAssetsPath + "/Data/"; }

    public void AddScore(PlayerScore score)
    {
        board = JSONLocal.JSONDesSerielization<ScoreboardInfo>(_path, _fileName, board); //Cargo la Scoreboard
        board.scores.Add(score); //Una vez cargada llamó a su lista y le agregó un nuevo elemento
        JSONLocal.JSONSerielization<ScoreboardInfo>(_path, _fileName, board); //La sobreescribo
    }

    public void ShowScore()
    {
        //Cargo la ScoreBoard
        board = JSONLocal.JSONDesSerielization<ScoreboardInfo>(_path, _fileName, board);

        if (board.scores.Count == 0) _text.text = "No Data";
        else
        {
            //Ordeno primero los elementos de la lista de mayor a menor puntaje, luego de eso hago un Select
            //Para que creé una nueva lista que contenga el nombre y el score(en tiempo) de cada jugador
            //Por último agarro los primeros 10 nombres de la lista.
            List<string> namesWithNumbers = board.scores.OrderByDescending(x => x.playerRealScore)
                                                .Select(x => x.playerName + "  |\t" + x.playerScore)
                                                .Take(10).ToList();
        
            _text.text = namesWithNumbers.ToBoard();
        }
    }
}