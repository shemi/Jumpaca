using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{

    public TextMeshProUGUI indexText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nicknameText;

    public void Set(int index, string nickname, int score, bool me = false)
    {
        indexText.text = index.ToString();
        nicknameText.text = nickname;
        scoreText.text = score.ToString();
    }
    
}
