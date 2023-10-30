using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;//向左按钮
    public Button btnRight;//向右按钮
    public Button btnStart;//开始按钮
    public Button btnBack;//返回按钮
     
    public Image imgScene;//面板场景图
    public Text txtInfo;//面板文本

    private int nowIndex;//场景对象索引(0开始)
    private SceneInfo nowSceneInfo;//单条场景数据
    protected override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if( nowIndex < 0 )
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if ( nowIndex >= GameDataMgr.Instance.sceneInfoList.Count )
                nowIndex = 0;
            ChangeScene();
        });
        btnStart.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();//隐藏场景面板
            //同步加载bug：不能保证下一场景所有资源加载完毕就直接切换场景,导致HeroBornPos未出现就切换场景造成其他脚本空引用
            //SceneManager.LoadScene(nowSceneInfo.sceneName);//切换场景
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);//异步加载返回一个事件
            ao.completed += (obj) =>//关卡初始化
            {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);//初始化游戏场景的关卡管理器
            };
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();//隐藏场景面板
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();//显示选角面板
        });
        ChangeScene();//打开面板时也更新
    }
    /// <summary>
    /// 根据索引值更新场景
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];//读取数据
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);//更新图片
        txtInfo.text = "名称：\n" + nowSceneInfo.name + "\n" + "描述：\n" + nowSceneInfo.tips;//更新文字
    }
}
