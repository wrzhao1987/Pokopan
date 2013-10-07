//
//  UGameManager.h
//  UGameManager
//
//  Created by Martin on 13-10-5.
//  Copyright (c) 2013年 Martin. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GameKit/GameKit.h"

@protocol UGameManagerDelegate <NSObject>

@optional
- (void) processGameCenterAuth: (NSError*) error;
@end

@interface UGameManager : NSObject
{
    id <UGameManagerDelegate, NSObject> delegate;
    id unityViewController;
    
    BOOL enableGameCenter;
}
@property (nonatomic, retain) id <UGameManagerDelegate> delegate;
@property (nonatomic, retain) id unityviewController;
@property (nonatomic) BOOL enableGameCenter;
// 获取UGameManager对象的实例
+ (UGameManager*) instance;

// 显示警告窗口
- (void) ShowWarningBox:(NSString*) strTitle text:(NSString*) strText;

- (void) registerForAuthenticationNotification;
- (void) authenticationChanged;

// 检查Game Center是否可用
+ (BOOL) isGameCenterAvailable;
// 认证本地用户
- (void) authenticateLocalPlayer;

// 上传积分
- (void) reportScore: (NSString*)identifier hiscore:(int64_t) score;

// 上传成就
- (void) reportAchievementIdentifier: (NSString*) identifier percentComplete:(float) percent;

// 显示排行榜
- (void) showLeaderboard: (NSString*) leaderboard;

// 显示成就
- (void) showAchievements;
// ********************************
// C函数，将被导出到Unity中
// ********************************
void _startup(void);
void _showWarningBox(char* strTitle, char* strText);
bool _isGameCenterAvailable(void);
void _authenticateLocalPlayer(void);
void _reportScore(char* identifier, int score);
void _reportAchievementIdentifier(char* identifier, float percent);
void _showLeaderboard(char* leaderboard);
void _showAchievements(void);

@end
