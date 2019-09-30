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
    
    [Serializable]
    public struct RegisterRequest
    {
        public string gameToken;
        public string nickname;
        public string deviceType;
        public string deviceId;
    }
    
    [Serializable]
    public struct RegisterRespond
    {
        public string token;
        public bool exists;
        public int playerId;
    }
    
    [Serializable]
    public struct PlayerUpdateRequest
    {
        public string nickname;
        public int highScore;
        public int coins;
        public string skinId;
        public string[] inventory;
        public string[] wears;
    }
    
    [Serializable]
    public struct PlayerRespond
    {
        public int playerId;
        public string[] inventory;
        public string[] wears;
        public string skinId;
        public string nickname;
        public int highScore;
        public int coins;
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
    
    public string GetDeviceType()
    {
        return SystemInfo.deviceModel;
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

    UnityWebRequest Post(string path, object obj, [CanBeNull] Dictionary<string, string> headers = null)
    {
        string json = JsonUtility.ToJson(obj);
        
        return Request(path, json, headers);
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

    public IEnumerator Register(string nickname)
    {
        var json = new RegisterRequest()
        {
            deviceId = GetDeviceId(),
            deviceType = GetDeviceType(),
            nickname = nickname,
            gameToken = _gameToken
        };

        UnityWebRequest request = Post("/api/v1/register", json);
        
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            yield return null;
        }

        yield return JsonUtility.FromJson<RegisterRespond>(request.downloadHandler.text);
    }
    
    public IEnumerator Me()
    {
        if (String.IsNullOrEmpty(GameStateManager.instance.PlayerToken))
        {
            yield break;
        }
        
        var headers = new Dictionary<string, string>();
        headers.Add("X-API-TOKEN", GameStateManager.instance.PlayerToken);
        
        UnityWebRequest request = Get("/api/v1/me", new NameValueCollection(), headers);
        
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            yield return null;
        }

        yield return JsonUtility.FromJson<PlayerRespond>(request.downloadHandler.text);
    }

    public IEnumerator Update(PlayerUpdateRequest json)
    {
        var headers = new Dictionary<string, string> {{"X-API-TOKEN", GameStateManager.instance.PlayerToken}};

        UnityWebRequest request = Post("/api/v1/update", json, headers);
        
        yield return request.SendWebRequest();
        
        if (request.isNetworkError)
        {
            yield return null;
        }

        yield return true;
    }
    
}
