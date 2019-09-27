using JetBrains.Annotations;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{

    public GameObject itemPrefab;
    public RectTransform content;

    public Animator animator;
    public GameObject loading;

    public void Show()
    {
        foreach (Transform child in content) {
            Destroy(child.gameObject);
        }
        
        loading.SetActive(true);
        animator.SetBool("ShowLeaderboard", true);
        
        Load();
    }

    public void Hide()
    {
        animator.SetBool("ShowLeaderboard", false);
    }

    private void Load()
    {
        StartCoroutine(GameStateManager.instance.LoadLeaderboard(this));
    }

    public void Loaded([CanBeNull] ApiService.LeaderboardPlayer[] players)
    {
        loading.SetActive(false);
        
        if (players == null || players.Length <= 0)
        {
            return;
        }
        
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];
            GameObject item = Instantiate(itemPrefab, content);
            LeaderboardItem leaderboardItem = item.GetComponent<LeaderboardItem>();
            leaderboardItem.Set(i+1, player.nickname, player.score);
        }
    }
    
}
