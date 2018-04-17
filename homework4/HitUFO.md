[toc]
### 1.作业要求
**编写一个简单的鼠标打飞碟（Hit UFO）游戏**

- 游戏内容要求：
游戏有 n 个 round，每个 round 都包括10 次 trial；
每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
每个 trial 的飞碟有随机性，总体难度随 round 上升；
鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。

- 游戏的要求：
使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
尽可能使用前面 MVC 结构实现人机交互与游戏模型分离(这些可以参考我的前一个博客：[牧师与魔鬼改进版](https://blog.csdn.net/m0_37782473/article/details/79887184))

### 2.具体设计
##### (1).制备预制体作为飞碟
我选用Sphere和Capsule来制备飞碟，将Sphere重命名为disk，将Capsule作为其子对象，然后调整Capsule的缩放比例，如图：
![disk](https://img-blog.csdn.net/20180417210817158?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)     ![Capsule的缩放比例](https://img-blog.csdn.net/20180417210858930?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)    ![disk prefab](https://img-blog.csdn.net/20180417211834135?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
##### (2).了解一下Singleton模板类
单例模式:是一种常用的软件设计模式,在它的核心结构中值包含一个被称为单例的特殊类。一个类只有一个实例,即一个类只有一个对象实例。单由于其经常使用，所以为一个需要使用单例模式的类写一份几乎重复代码很浪费资源，增加代码量。而Singleton模板类则完美解决了这个问题，通过使用泛型来为每一个需要使用单例模式的类创建一个且唯一的一个对象实例。具体代码如下(C#)：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}
```
例子：```DiskFactory df = Singleton<DiskFactory>.Instance```

##### (3).了解一下工厂模式
工厂模式（Factory Pattern）是编程中最常用的设计模式之一。这种类型的设计模式属于创建型模式，它提供了一种创建对象的最佳方式。在工厂模式中，我们在创建对象时不会对客户端暴露创建逻辑，并且是通过使用一个共同的接口来指向新创建的对象，所以能够创建不同类型的实现了同一接口的对象实例。

**为什么要使用工厂模式呢？**

- 游戏对象的创建与销毁高成本，必须减少销毁次数，如游戏中的子弹。
- 屏蔽创建与销毁的业务逻辑，使程序易于扩展。

想要更深入了解工厂模式的同学可以参考以下链接：
[设计模式(一) 工厂模式 五种写法总结](https://blog.csdn.net/zxt0601/article/details/52798423)
[模式的秘密——工厂模式(慕课网)](https://www.imooc.com/learn/261)

##### (3).设计具体要实现的类
- 导演类Director，单例模式，继承System.Object(会不被Unity内存管理，但所有Scene都能访问到它)，主要控制场景切换(虽然现在只有一个场景)。
- 接口场景类ISceneController，负责指明具体实现的场景类要实现的方法，而且便于更多的类能通过接口来访问场景类，由FirstSceneController具体场景实现类来实现。
- 接口类IUserAction，负责指明由用户行为引发的变化的方法，由FirstSceneController这个最高级的控制类来实现。
- 飞碟数据类DiskData，说明当前飞碟的状态，用于描述飞碟。
- 飞碟工厂类DiskFactory，用于制造和销毁飞碟的工厂。
- 所有动作的基础类SSAction，用于规定所有动作的基础规范，继承ScriptableObject(ScriptableObject是不需要绑定GameObject对象的可编程基类，这些基类受Unity引擎场景管理)。
- 飞碟飞行动作类CCFlyAction。
- 组合动作管理类SSActionManager，用于管理一系列的动作，负责创建和销毁它们。
- 动作事件接口类ISSActionCallback，定义了事件处理的接口，事件管理器必须实现它。
- 事件管理类CCActionManager，继承了SSActionManager，实现了ISSActionCallback，负责事件的处理。
- 最高级的控制类FirstSceneController，负责底层数据与用户操作的GUI的交互，实现ISceneControl和IUserAction。
- 用户界面类UserGUI，负责生成界面交于用户操作。
- 模板类Singleton，用于给需要的类生成一个唯一的实例。
- 记分员ScoreRecorder，用于给用户计分。

### 3.程序代码
Director.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object {
    public ISceneControl current { set; get; }

    private static Director _Instance;

    public static Director getInstance()
    {
        return _Instance ?? (_Instance = new Director());
    }
}
```
ISceneController.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControl {
    void loadResources();
}
```
IUserAction.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, PAUSE, START, FUNISH}

public interface IUserAction{
    GameState getGameState();
    void setGameState(GameState gameState);
    int getScore();
    void hit(Vector3 pos);
}
```
DiskData.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskData : MonoBehaviour {
    private Vector3 size;
    private Color color;
    private float speed;
    private Vector3 direction;

    public DiskData() { }

    public Vector3 getSize()
    {
        return size;
    }

    public float getSpeed()
    {
        return speed;
    }

    public Vector3 getDirection()
    {
        return direction;
    }

    public Color getColor()
    {
        return color;
    }

    public void setDiskData(Vector3 size, Color color, float speed, Vector3 direction)
    {
        this.size = size;
        this.color = color;
        this.speed = speed;
        this.direction = direction;
    }
}
```
DiskFactory.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    public GameObject diskPrefab;
    public List<DiskData> used = new List<DiskData>();
    public List<DiskData> free = new List<DiskData>();

    private void Awake()
    {
        diskPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"), Vector3.zero, Quaternion.identity);
        diskPrefab.SetActive(false);
    }

    public GameObject getDisk(int round)
    {
        GameObject disk = null;
        if(free.Count > 0)
        {
            disk = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            disk = GameObject.Instantiate<GameObject>(diskPrefab, Vector3.zero, Quaternion.identity);
            disk.AddComponent<DiskData>();
        }

        int start;
        switch (round)
        {
            case 0: start = 0; break;
            case 1: start = 100; break;
            default: start = 200; break;
        }
        int selectColor = Random.Range(start, round * 499);
        round = selectColor / 250;
        DiskData diskData = disk.GetComponent<DiskData>();
        Renderer renderer = disk.GetComponent<Renderer>();
        Renderer childRenderer = disk.transform.GetChild(0).GetComponent<Renderer>();
        float ranX = Random.Range(-1, 1) < 0 ? -1.2f : 1.2f;
        Vector3 direction = new Vector3(ranX, 1, 0);
        switch (round)
        {
            case 0:
                diskData.setDiskData(new Vector3(1.35f, 1.35f, 1.35f), Color.white, 4.0f, direction);
                renderer.material.color = Color.white;
                childRenderer.material.color = Color.white;
                break;
            case 1:
                diskData.setDiskData(new Vector3(1f, 1f, 1f), Color.gray, 6.0f, direction);
                renderer.material.color = Color.gray;
                childRenderer.material.color = Color.gray;
                break;
            case 2:
                diskData.setDiskData(new Vector3(0.7f, 0.7f, 0.7f), Color.black, 8.0f, direction);
                renderer.material.color = Color.black;
                childRenderer.material.color = Color.black;
                break;
        }
        used.Add(diskData);
        diskData.name = diskData.GetInstanceID().ToString();
        disk.transform.localScale = diskData.getSize();
        
        return disk;
    }
 
    public void freeDisk(GameObject disk)
    {
        DiskData temp = null;
        foreach (DiskData i in used)
        {
            if (disk.GetInstanceID() == i.gameObject.GetInstanceID())
            {
                temp = i;
            }
        }
        if (temp != null)
        {
            temp.gameObject.SetActive(false);
            free.Add(temp);
            used.Remove(temp);
        }
    }
}
```
SSAction.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject {
    public bool enable = false;
    public bool destroy = false;

    public GameObject gameObject { set; get; }
    public Transform transform { set; get; }
    public ISSActionCallback callback { set; get; }

    protected SSAction() { }

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }

    public void reset()
    {
        enable = false;
        destroy = false;
        gameObject = null;
        transform = null;
        callback = null;
    }
}
```
CCFlyAction.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction
{
    float acceleration;
    float horizontalSpeed;
    Vector3 direction;
    float time;

    public static CCFlyAction getCCFlyAction()
    {
        CCFlyAction action =  ScriptableObject.CreateInstance<CCFlyAction>();
        return action;
    }

    public override void Start()
    {
        enable = true;
        acceleration = 9.8f;
        time = 0;
        horizontalSpeed = gameObject.GetComponent<DiskData>().getSpeed();
        direction = gameObject.GetComponent<DiskData>().getDirection();
    }

    public override void Update()
    {
        if (gameObject.activeSelf)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.down * acceleration * time * Time.deltaTime);
            transform.Translate(direction * horizontalSpeed * Time.deltaTime);
            if(this.transform.position.y < -4)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
    }
}

```
SSActionManager.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Update()
    {
        foreach (SSAction action in waitingAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();

        foreach(KeyValuePair<int, SSAction> i in actions)
        {
            SSAction value = i.Value;
            if (value.destroy)
            {
                waitingDelete.Add(value.GetInstanceID());
            }
            else if (value.enable)
            {
                value.Update();
            }
        }

        foreach(int i in waitingDelete)
        {
            SSAction ac = actions[i];
            actions.Remove(i);
            DestroyObject(ac);
        }
    }

    public void runAction(GameObject gameObject, SSAction action, ISSActionCallback manager)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}
```
ISSActionCallback.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Completed}

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intPram = 0
        , string strParm = null, Object objParm = null);
}
```
CCActionManager.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback {
    private FirstSceneControl sceneControl;
    private List<CCFlyAction> flys = new List<CCFlyAction>();
    private int diskNumber = 0;

    private List<SSAction> used = new List<SSAction>();
    private List<SSAction> free = new List<SSAction>();

    public void setDiskNumber(int dn)
    {
        diskNumber = dn;
    }

    public int getDiskNumber()
    {
        return diskNumber;
    }

    public SSAction getSSAction()
    {
        SSAction action = null;
        if(free.Count > 0)
        {
            action = free[0];
            free.Remove(free[0]);
        }
        else
        {
            action = ScriptableObject.Instantiate<CCFlyAction>(flys[0]);
        }
        used.Add(action);
        return action;
    }

    public void freeSSAction(SSAction action)
    {
        foreach(SSAction a in used)
        {
            if(a.GetInstanceID() == action.GetInstanceID())
            {
                a.reset();
                free.Add(a);
                used.Remove(a);
                break;
            }
        }
    }

    protected void Start()
    {
        sceneControl = (FirstSceneControl)Director.getInstance().current;
        sceneControl.actionManager = this;
        flys.Add(CCFlyAction.getCCFlyAction());
    }

    private new void Update()
    {
        if (sceneControl.getGameState() == GameState.RUNNING)
            base.Update();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intPram = 0
        , string strParm = null, Object objParm = null)
    {
        if(source is CCFlyAction)
        {
            diskNumber--;
            Singleton<DiskFactory>.Instance.freeDisk(source.gameObject);
            freeSSAction(source);
        }
    }

    public void startThrow(Queue<GameObject> diskQueue)
    {
        foreach(GameObject i in diskQueue)
        {
            runAction(i, getSSAction(), (ISSActionCallback)this);
        }
    }
}
```
FirstSceneController.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneControl : MonoBehaviour, ISceneControl, IUserAction {
    public CCActionManager actionManager { set; get; }
    public ScoreRecorder scoreRecorder { set; get; }
    public Queue<GameObject> diskQueue = new Queue<GameObject>();
    private int diskNumber = 0;
    private int currentRound = -1;
    private float time = 0;
    private GameState gameState = GameState.START;

    void Awake()
    {
        Director director = Director.getInstance();
        director.current = this;
        diskNumber = 10;
        this.gameObject.AddComponent<ScoreRecorder>();
        this.gameObject.AddComponent<DiskFactory>();
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        director.current.loadResources();
    }

    public void loadResources()
    {
        
    }

    private void Update()
    {
        if(actionManager.getDiskNumber() == 0 && gameState == GameState.RUNNING)
        {
            gameState = GameState.ROUND_FINISH;
            if(currentRound == 2)
            {
                gameState = GameState.FUNISH;
                return;
            }
        }
        if(actionManager.getDiskNumber() == 0 && gameState == GameState.ROUND_START)
        {
            currentRound++;
            nextRound();
            actionManager.setDiskNumber(10);
            gameState = GameState.RUNNING;
        }
        if(time > 1 && gameState != GameState.PAUSE)
        {
            throwDisk();
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    private void nextRound()
    {
        DiskFactory diskFactory = Singleton<DiskFactory>.Instance;
        for(int i = 0; i < diskNumber; i++)
        {
            diskQueue.Enqueue(diskFactory.getDisk(currentRound));
        }
        actionManager.startThrow(diskQueue);
    }

    void throwDisk()
    {
        if(diskQueue.Count != 0)
        {
            GameObject disk = diskQueue.Dequeue();
            Vector3 pos = new Vector3(-disk.GetComponent<DiskData>().getDirection().x * 10, Random.Range(0f, 4f), 0);
            disk.transform.position = pos;
            disk.SetActive(true);
        }
    }

    public int getScore()
    {
        return scoreRecorder.score;
    }

    public GameState getGameState()
    {
        return gameState;
    }

    public void setGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public void hit(Vector3 pos)
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(pos));
        for(int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if(hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                scoreRecorder.record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -5, 0);
            }
        }
    }
}
```
UserGUI.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    bool isFirst = true;
    GUIStyle red;
    GUIStyle black;

    // Use this for initialization
    void Start () {
        action = Director.getInstance().current as IUserAction;
        black = new GUIStyle("button");
        black.fontSize = 20;
        red = new GUIStyle();
        red.fontSize = 30;
        red.fontStyle = FontStyle.Bold;
        red.normal.textColor = Color.red;
        red.alignment = TextAnchor.UpperCenter;
    }

    private void OnGUI()
    {
        if (action.getGameState() == GameState.FUNISH)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 100), action.getScore() >= 30 ? "You win" : "You fail", red);
            if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 - 50, 120, 40), "Restart", black))
            {
                SceneManager.LoadScene("DiskAttack");
            }
            return;
        }
        Rect rect = new Rect(Screen.width / 2 - 100, 0, 200, 40);
        Rect rect2 = new Rect(Screen.width / 2 - 45, 60, 120, 40);

        if (Input.GetButtonDown("Fire1") && action.getGameState() != GameState.PAUSE)
        {
            Vector3 pos = Input.mousePosition;
            action.hit(pos);
        }

        if (!isFirst)
        {
            GUI.Label(rect, "Your score: " + action.getScore().ToString(), red);
        }
        else
        {
            GUIStyle blackLabel = new GUIStyle();
            blackLabel.fontSize = 16;
            blackLabel.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width / 2 - 250, 120, 500, 200), "There are 3 rounds, every round has 10 disk " +
                "whose color is different.\nIf you attack the white one, you will get 1 score. And you will get 2 score\n" +
                "if you attack the gray one. Finally, if you can attack the black and most\nfast one, you will get 4 " +
                "score. Once you get 30 scores, you win!", blackLabel);
        }

        if (action.getGameState() == GameState.RUNNING && GUI.Button(rect2, "Paused", black))
        {
            action.setGameState(GameState.PAUSE);
        }
        else if(action.getGameState() == GameState.PAUSE && GUI.Button(rect2, "Run", black))
        {
            action.setGameState(GameState.RUNNING);
        }

        if (isFirst && GUI.Button(rect2, "Start", black))
        {
            isFirst = false;
            action.setGameState(GameState.ROUND_START);
        }

        if(!isFirst && action.getGameState() == GameState.ROUND_FINISH && GUI.Button(rect2, "Next Round", black))
        {
            action.setGameState(GameState.ROUND_START);
        }
    }
}
```
Singleton.cs(前面已有详细代码)
ScoreRecorder.cs
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour {
    public int score;
    private Dictionary<Color, int> scoreTable = new Dictionary<Color, int>();

    void Start()
    {
        score = 0;
        scoreTable.Add(Color.white, 1);
        scoreTable.Add(Color.gray, 2);
        scoreTable.Add(Color.black, 4);
    }

    public void reset()
    {
        score = 0;
    }

    public void record(GameObject disk)
    {
        score += scoreTable[disk.GetComponent<DiskData>().getColor()];
    }
}
```

### 成果视频
[已上传到优酷的成果视频](http://v.youku.com/v_show/id_XMzU0NTY3MzU2OA==.html?spm=a2h0k.8191407.0.0&from=s1.8-1-1.2)