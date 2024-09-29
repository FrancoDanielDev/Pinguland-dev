using UnityEngine;
using TMPro;
using Assets.SimpleLocalization;

[RequireComponent(typeof(ServerRequest))]
public class ScoreboardUIHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject submitLocal_button;    
    [SerializeField]
    private GameObject submitOnline_button;
    [SerializeField]
    private GameObject name_input;
    [SerializeField]
    private GameObject name_label;
    
    public string ScoringKey;
    public string NoDataKey;

    public static ScoreboardUIHelper Instance;

    [SerializeField]
    private TMP_InputField userName;
    [SerializeField]
    private TMP_InputField scoreboard;
    [SerializeField]
    private GameTime gameTime;

    private ServerRequest server;
    [SerializeField]private LocalScoreboard localScoreboard;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        server = GetComponent<ServerRequest>();
    }

    public void UpdateScoreboard(string json_data)
    {
        scoreboard.text = "";
        ServerScoreboardData[] json = JsonHelper.FromJson<ServerScoreboardData>(json_data);

        if (json.Length == 0 || json == null)
        {
            scoreboard.text = LocalizationManager.Localize(NoDataKey);
            return;
        }

        int posInBoard = 1;        

        foreach (ServerScoreboardData data in json)
        {
            int minutes = data.user_score / 60;
            int seconds = data.user_score % 60;
            string time = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');

            scoreboard.text = scoreboard.text + "#" + posInBoard.ToString().PadLeft(2, '0') + " | " + data.name + " | " + LocalizationManager.Localize(ScoringKey) + ": " + time + "\n";
            posInBoard++;
        }
    }

    public void SaveScore()
    {
        if (userName.text == "")
            return;

        server.SaveScore(gameTime.GetTotalSeconds().ToString(), userName.text);
        userName.text = "";
    }

    public void SaveLocalScore()
    {
        if (userName.text == "") return;

        PlayerScore score = new PlayerScore();
        score.playerName = userName.text;
        score.playerScore = gameTime.timeText;
        score.playerRealScore = gameTime.GetTotalSeconds();

        localScoreboard.AddScore(score);
        //HideSubmitLocalFromDeathScreen();
    }

    public void RefreshScores()
    {
        scoreboard.text = "";
        server.GetScores();
    }

    public void ClearScores()
    {
        scoreboard.text = "";
    }

    public void HideSubmitLocalFromDeathScreen()
    {
        submitLocal_button.SetActive(false);
        name_input.SetActive(false);
        name_label.SetActive(false);
    }

    public void HideSubmitOnlineFromDeathScreen()
    {
        submitOnline_button.SetActive(false);
        name_input.SetActive(false);
        name_label.SetActive(false);
    }
}
