using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class UnityiOSKit : MonoBehaviour {
	
	[DllImport ("__Internal")]
	private static extern void _startup();
	
	[DllImport ("__Internal")]
	private static extern void _showWarningBox(string strTitle, string strText);
	
	[DllImport ("__Internal")]
	private static extern bool _isGameCenterAvailable();
	
	[DllImport ("__Internal")]
	private static extern void _authenticateLocalPlayer();
	
	[DllImport ("__Internal")]
	private static extern void _reportScore(string identifier, int hiscore);
	
	[DllImport ("__Internal")]
	private static extern void _reportAchievement(string identifier, float percent);
	
	[DllImport ("__Internal")]
	private static extern void _showLeaderboard(string leaderboard);
	
	[DllImport ("__Internal")]
	private static extern void _showAchievement();
	
	// Use this for initialization
	static public void Start()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_startup();
		}
	}
	
	static public void ShowWarningBox(string strTitle, string strText)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_showWarningBox(strTitle, strText);
		}
	}
	
	static public bool IsGameCenterAvailable()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _isGameCenterAvailable();
		}
		return false;
	}
	
	static public void AuthenticateLocalPlayer()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_authenticateLocalPlayer();
		}
	}
	
	static public void ReportScore(string identifier, int hiscore)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_reportScore(identifier, hiscore);
		}
	}
	
	static public void ReportAchievement(string identifier, float percent)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_reportAchievement(identifier, percent);
		}
	}
	
	static public void ShowLeaderboard(string identifier)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_showLeaderboard(identifier);
		}
	}
	
	static public void ShowAchievements()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_showAchievement();
		}
	}
}
