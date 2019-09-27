using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public class ApiService
{
    
    private readonly string _url;
    [CanBeNull] private string _gameToken;
    [CanBeNull] private string _token;
 
    [Serializable]
    public struct LeaderboardPlayer
    {
        public int score;
        public string nickname;
        public int playerId;
    }
    
    public ApiService(string url, [CanBeNull] string gameToken)
    {
        _url = url;
        _gameToken = gameToken;
    }

    public string GetDeviceId()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
    
    public ApiService SetToken(string token)
    {
        _token = token;

        return this;
    }
    
    UnityWebRequest Request(string path, string json, [CanBeNull] Dictionary<string, string> headers = null, string method = "POST")
    {
        var uwr = new UnityWebRequest(_url+path, method);

        if (!string.IsNullOrEmpty(json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
        }
        
        uwr.downloadHandler = new DownloadHandlerBuffer();
        
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Accept", "application/json");

        if (headers != null)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                uwr.SetRequestHeader(headers.Keys.ElementAt(i), headers[headers.Keys.ElementAt(i)]);    
            }
        }

        return uwr;
    }

    private string ToQueryString(NameValueCollection query)
    {
        var array = (
            from key in query.AllKeys
            from value in query.GetValues(key)
            select $"{UnityWebRequest.EscapeURL(key)}={UnityWebRequest.EscapeURL(value)}"
        ).ToArray();
        
        return "?" + string.Join("&", array);
    }
    
    UnityWebRequest Get(string path, NameValueCollection query, [CanBeNull] Dictionary<string, string> headers = null)
    {
        query.Add("gameToken", _gameToken);
        
        path += ToQueryString(query);

        return Request(path, "", headers, "GET");
    }

    public IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = Get("/api/v1/leaderboard", new NameValueCollection());
        
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            yield return null;
        }

        yield return JsonHelper.FromJson<LeaderboardPlayer>(request.downloadHandler.text);
    }
    
}
