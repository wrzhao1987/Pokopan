using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Collections;

public class GameCenterHandler : MonoBehaviour {
	
	static public GameCenterHandler Instance;
	
	private bool point1wReported = false;
	
	void Awake()
	{
		Instance = this;
	}
	
	void Start()
	{
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		if (! Social.localUser.authenticated)
		{
			Social.localUser.Authenticate(HandleAuthenticated);
		}
	}
	
	void Update()
	{
		if (Manager.Instance.Score >= 10000 && (! point1wReported))
		{
			ReportAchievement("point1w", (double)100.0f);
			point1wReported = true;
		}
	}
	void HandleAuthenticated(bool success)
    {
        Debug.Log("*** HandleAuthenticated: success = " + success);
		/**
        if (success)
		{
            Social.localUser.LoadFriends(HandleFriendsLoaded);
            Social.LoadAchievements(HandleAchievementsLoaded);
            Social.LoadAchievementDescriptions(HandleAchievementDescriptionsLoaded);
        }
        **/
    }
	
	void HandleFriendsLoaded(bool success)
    {
        Debug.Log("*** HandleFriendsLoaded: success = " + success);
        foreach (IUserProfile friend in Social.localUser.friends) {
            Debug.Log("*   friend = " + friend.ToString());
        }
    }
   
    void HandleAchievementsLoaded(IAchievement[] achievements)
    {
        Debug.Log("*** HandleAchievementsLoaded");
        foreach (IAchievement achievement in achievements) {
            Debug.Log("*   achievement = " + achievement.ToString());
        }
    }
   
    private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
    {
        Debug.Log("*** HandleAchievementDescriptionsLoaded");
        foreach (IAchievementDescription achievementDescription in achievementDescriptions) {
            Debug.Log("*   achievementDescription = " + achievementDescription.ToString());
        }
    }
	
	public void ShowLeaderboard(string identifier, TimeScope scope)
	{
		GameCenterPlatform.ShowLeaderboardUI(identifier, scope);
	}
	
	public void ReportScore(string identifier, long score)
	{
		if (Social.localUser.authenticated)
		{
			Social.ReportScore(score, identifier, HandleScoreReported);
		}
	}
	
	private void HandleScoreReported(bool success)
	{
		Debug.Log("*** HandleScoreReported: success = " + success);
	}
	
	public void ReportAchievement(string identifier, double progress)
	{
		if (Social.localUser.authenticated)
		{
			Social.ReportProgress(identifier, progress, HandleAchievementReported);
		}
	}
	
	private void HandleAchievementReported(bool success)
	{
		Debug.Log("*** HandleProgressReported: success = " + success);
	}
}
