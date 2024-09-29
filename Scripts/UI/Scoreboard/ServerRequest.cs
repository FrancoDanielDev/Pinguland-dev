using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerRequest : MonoBehaviour
{
    private string server_url = "https://defaltuser.000webhostapp.com/jam_dv_2023/";

    /// <summary>
    /// Executes "GetScores_Coroutine()" coroutine that sends a GET request to the web server to obtain the scoreboard data.
    /// </summary>
    public void GetScores()
    {
        StartCoroutine(GetScores_Coroutine());
    }

    /// <summary>
    /// Sends a GET request to the web server.
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetScores_Coroutine()
    {
        string url = server_url + "?action=get-scores"; //Server URL where to do the GET request and obtain the scores.

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ScoreboardUIHelper.Instance.UpdateScoreboard(webRequest.downloadHandler.text);
            }
            else
            {
                //If request failed.
            }
        }
    }

    /// <summary>
    /// Executes "SaveScore_Coroutine" coroutine that sends a POST request to the web server to upload the scoreboard data.
    /// </summary>
    /// <param name="playerScore"></param>
    /// <param name="playerName"></param>
    public void SaveScore(string playerScore, string playerName)
    {
        StartCoroutine(SaveScore_Coroutine(playerScore, playerName));
    }

    /// <summary>
    /// Sends a POST request to the web server.
    /// </summary>
    /// <param name="playerScore"></param>
    /// <param name="playerName"></param>
    /// <returns></returns>
    IEnumerator SaveScore_Coroutine(string playerScore, string playerName)
    {
        string url = server_url + "?action=add-score"; //Server URL where to do the POST request and upload a the score.

        //Create form, add inputs and values.
        WWWForm form = new WWWForm();
        form.AddField("name", playerName);
        form.AddField("score", playerScore);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                ScoreboardUIHelper.Instance.HideSubmitOnlineFromDeathScreen();
            }
            else
            {
                //If request failed.
            }
        }
    }
}
