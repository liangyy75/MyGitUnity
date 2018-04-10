### **1、简答并用程序验证**
- 游戏对象运动的本质是什么？
- 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）
- 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

##### 游戏对象运动的本质
游戏对象的运动其实是每一帧游戏对象的位置、行为发生的些许变化组成。每一帧都是静止的，但每一帧相对上一帧都在发生变化，连续变化的帧使得游戏对象有了运动。而这些位置的变化是通过坐标系统来实现的，行为则是事件驱动的，通过每一帧都会调用的update()函数等来实现游戏对象的运动。

##### 物体的抛物线运动
1.通过改变物体的position，给予物体一个向左与向上的初速度以及一个向下的加速度：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabolic : MonoBehaviour {

    private float speed = -5f;

	// Use this for initialization
	void Start () {
        Debug.Log("Start");
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position += Vector3.left * 5 * Time.deltaTime;
        this.transform.position += Vector3.down * speed * Time.deltaTime;
        speed += 0.5f;
	}
}
```

2.通过改变一个新建的vector3来更新物体的position以达到物体的抛物运动的效果：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabolic2 : MonoBehaviour {

    // Use this for initialization
    private float speed = 3f;
	void Start () {
        Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += new Vector3(-5f * Time.deltaTime, speed * Time.deltaTime, 0);
        speed -= 0.2f;
	}
}
```

3.通过改变vector3来改变物体的position达到类似效果：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabolic3 : MonoBehaviour {

    // Use this for initialization
    private float speed = 3f;
	void Start () {
        Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector3(-5f * Time.deltaTime, speed * Time.deltaTime, 0));
        speed -= 0.2f;
    }
}
```

##### 实现完整太阳系

**对象设计如下：**
![这里写图片描述](https://img-blog.csdn.net/20180401230415500?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
在这里设定太阳是绝对不动的点，所有行星相对太阳运动(自转和公转)，所以不考虑太阳自转。
但由于地球自转会影响到月亮的运动，所以创建一个空对象作为月亮的父对象，这个空对象位置与地球的位置相同。

**事先准备：**
根据太阳系各大行星与太阳的远近距离以及相对大小调整好位置，然后将提前下载的贴图拉到对应的游戏对象上：
![这里写图片描述](https://img-blog.csdn.net/20180401223611683?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

**具体实现：**

- 先通过GameObject.Find找到对应的游戏对象，然后通过它transform里的RotateAround与Rotate函数实现公转与自转。
- RotateAround的三个参数分别为旋转的中心、旋转轴、速度，把中心设为太阳，改变旋转轴让它们位于不同的法平面上。
- Rotate的参数可以只为一个，即方向与速度，只要将Vector3.up(表示对象绕自己的Y轴旋转)乘以对应的角速度就可以实现自转。

**太阳系代码如下：**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("Mercury").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), 100 * Time.deltaTime);
        GameObject.Find("Venus").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 1), 90 * Time.deltaTime);
        GameObject.Find("Earth").transform.RotateAround(Vector3.zero, new Vector3(1, 1, 0), 80 * Time.deltaTime);
        GameObject.Find("Mars").transform.RotateAround(Vector3.zero, new Vector3(1, 0, 1), 70 * Time.deltaTime);
        GameObject.Find("Jupiter").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), 60 * Time.deltaTime);
        GameObject.Find("Saturn").transform.RotateAround(Vector3.zero, new Vector3(2, 1, 0), 50 * Time.deltaTime);
        GameObject.Find("Uranus").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 2), 40 * Time.deltaTime);
        GameObject.Find("Neptune").transform.RotateAround(Vector3.zero, new Vector3(1, 1, 1), 30 * Time.deltaTime);

        GameObject.Find("Mercury").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Venus").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Earth").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Mars").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Jupiter").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Saturn").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Uranus").transform.Rotate(Vector3.up * Time.deltaTime * 100);
        GameObject.Find("Neptune").transform.Rotate(Vector3.up * Time.deltaTime * 100); 
    }
}
```

**空对象的代码以及月球代码如下：**

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(Vector3.zero, new Vector3(1, 1, 0), 80 * Time.deltaTime);
	}
}
```
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(this.transform.parent.position, Vector3.up, 100 * Time.deltaTime);
        this.transform.Rotate(Vector3.up * Time.deltaTime * 100);
	}
}
```

效果图如下：
![这里写图片描述](https://img-blog.csdn.net/20180401231550192?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

### **2、编程实践**

- 阅读以下游戏脚本
Priests and Devils

> Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are
> 3 priests and 3 devils at one side of the river. They all want to get
> to the other side of this river, but there is only one boat and this
> boat can only carry two persons each time. And there must be one
> person steering the boat from one side to the other side. In the flash
> game, you can click on them to move them and click the go button to
> move the boat to the other direction. If the priests are out numbered
> by the devils on either side of the river, they get killed and the
> game is over. You can try it in many > ways. Keep all priests alive!
> Good luck!

- play the game ( http://www.flash-game.net/game/2535/priests-and-devils.html )
- 列出游戏中提及的事物（Objects）
- 用表格列出玩家动作表（规则表），注意，动作越少越好
- 请将游戏中对象做成预制
- 在 GenGameObjects 中创建 长方形、正方形、球 及其色彩代表游戏中的对象。
- 使用 C# 集合类型 有效组织对象
- 整个游戏仅 主摄像机 和 一个 Empty 对象， 其他对象必须代码动态生成！！！ 。 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的 通讯耦合 语句。 违背本条准则，不给分
- 请使用课件架构图编程，不接受非 MVC 结构程序
- 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！

#### 确定对象：
两岸：start、end，河流：water，船：boat，牧师：priest0、priest1、priest2，魔鬼：devil0、devil1、devil2。
由于只能用代码动态生成对象，所以做了五个cube、sphere类型的预制体：Boat、Coast、Water、Priest、Devil。

#### 分析MVC模式：
MVC是三个单词的首字母缩写，即Model(模型)、View(视图)、Controller(控制)，这个模式认为，程序不论简单或复杂，从结构上看，都可以分成三层。
1. 最上面的一层视图层直接面向用户，是提供给用户的操作界面，是程序的外壳。在这里就是UserGUI和ClickGUI，前一个负责用户界面生成与更新，后一个负责点击操作。
2. 中间一层控制层是接受用户从视图层输入的指令，选取数据层中的数据，对其进行处理。在这里就是BoatController、MyCharacterController、CoastController、FirstController、Director，其中BoatController控制船的数据与行为，MyCharacterController负责控制牧师与魔鬼，而CoastController则负责两岸的状态。然后三者都由FirstController控制。FirstController控制场景中所有对象与行为，如通信和用户点击。还有更高一层的Director，一个游戏中只能有一个实例，它控制着场景的创建、切换、销毁、游戏暂停、游戏退出等等最高层次的功能。
3. 最下面的一层就是数据层，是程序需要操作的数据或者信息。而在我这里则是各个游戏对象。

#### 代码展示：

**Director**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
    private static Director _instance;
    public SceneController currentSceneController { get; set; }

    public static Director getInstance()
    {
        return _instance ?? (_instance = new Director());
    }
}
```
Director的作用是保证游戏只有一个实例。同时Director的作用还有通信，任何Script都可以利用唯一一个currentSceneController来进行通信。而在我这里具体控制场景的任务就交给了SceneController场景控制器。

**SceneController**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SceneController
{
    void loadResources();
}
```
SceneController的作用是加载资源，而我把它作为一个接口，交给FirstController。SceneController其实还有其他功能，但在这里我就不多说了。

**CoastController**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastController
{
    readonly GameObject coast;
    readonly Vector3[] positions;
    readonly int type;
    
    MyCharacterController[] passengerPlaner;

    public CoastController(string name)
    {
        passengerPlaner = new MyCharacterController[6];

        if (name == "start")
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Coast", typeof(GameObject)), new Vector3(9, 1, 0), Quaternion.identity, null) as GameObject;
            positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0),
                new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};
            type = 1;
        }
        else
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Coast", typeof(GameObject)), new Vector3(-9, 1, 0), Quaternion.identity, null) as GameObject;
            positions = new Vector3[] {new Vector3(-6.5F,2.25F,0), new Vector3(-7.5F,2.25F,0), new Vector3(-8.5F,2.25F,0),
                new Vector3(-9.5F,2.25F,0), new Vector3(-10.5F,2.25F,0), new Vector3(-11.5F,2.25F,0)};
            type = -1;
        }
        coast.name = name;
    }

    public int getEmptyIndex()
    {
        for (int i = 0; i < passengerPlaner.Length; i++)
            if (passengerPlaner[i] == null)
                return i;
        return -1;
    }

    public Vector3 getEmptyPosition()
    {
        return positions[getEmptyIndex()];
    }

    public void getOnCoast(MyCharacterController characterCtrl)
    {
        passengerPlaner[getEmptyIndex()] = characterCtrl;
    }

    public MyCharacterController getOffCoast(string passenger_name)
    {
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
            {
                MyCharacterController charactorCtrl = passengerPlaner[i];
                passengerPlaner[i] = null;
                return charactorCtrl;
            }
        }
        return null;
    }

    public int getType()
    {
        return type;
    }

    public int[] getPassengersType()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] == null)
                continue;
            if (passengerPlaner[i].getType() == 0)
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public void reset()
    {
        passengerPlaner = new MyCharacterController[6];
    }
}
```
CoastController里面有一个GameObject成员，能利用预制体生成游戏对象。它具体控制两岸，表示岸的各种状态与属性，比如出发地、目的地的区别，还有上面有什么角色等等。它是作为一个容器给角色们承载而起作用的。因此有些特殊的函数，如getEmptyPosition等等。同样的容器还有BoatController。

**BoatController**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController
{
    readonly GameObject boat;
    readonly Moveable moveableScript;
    readonly Vector3 startPosition = new Vector3(5, 1, 0);
    readonly Vector3 endPosition = new Vector3(-5, 1, 0);
    readonly Vector3[] startPositions;
    readonly Vector3[] endPositions;
    
    int status;
    bool clickFlag = true;
    MyCharacterController[] passenger = new MyCharacterController[2];

    public BoatController()
    {
        status = 1;
        startPositions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
        endPositions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };
        boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), startPosition, Quaternion.identity, null) as GameObject;
        boat.name = "boat";
        moveableScript = boat.AddComponent(typeof(Moveable)) as Moveable;
        boat.AddComponent(typeof(ClickGUI));
    }


    public void Move()
    {
        if (clickFlag == false)
            return;
        if (status == -1)
            moveableScript.setDestination(startPosition);
        else
            moveableScript.setDestination(endPosition);
        status = 0 - status;
    }

    public int getEmptyIndex()
    {
        return passenger[0] == null ? 0 : (passenger[1] == null ? 1 : -1);
    }

    public bool isEmpty()
    {
        for (int i = 0; i < passenger.Length; i++)
            if (passenger[i] != null)
                return false;
        return true;
    }

    public Vector3 getEmptyPosition()
    {
        int emptyIndex = getEmptyIndex();
        return status == -1 ? endPositions[emptyIndex] : startPositions[emptyIndex];
    }

    public void GetOnBoat(MyCharacterController characterCtrl)
    {
        passenger[getEmptyIndex()] = characterCtrl;
    }

    public MyCharacterController GetOffBoat(string passenger_name)
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null && passenger[i].getName() == passenger_name)
            {
                MyCharacterController characendrCtrl = passenger[i];
                passenger[i] = null;
                return characendrCtrl;
            }
        }
        return null;
    }

    public GameObject getGameobj()
    {
        return boat;
    }

    public int getStatus()
    {
        return status;
    }

    public void setClickFlag(bool flag)
    {
        clickFlag = flag;
    }

    public int[] getPassengersType()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
                continue;
            if (passenger[i].getType() == 0)
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public void reset()
    {
        moveableScript.reset();
        clickFlag = true;
        if (status == -1)
            Move();
        passenger = new MyCharacterController[2];
    }
}
```
BoatController与CoastController的区别就在于有角色乘载时点击船船是会移动的，所以它在CoastController的大体框架上多出了一个Move函数，同时将ClickGUI挂载到这个类上。

**MyCharacterController**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController
{
    readonly GameObject character;
    readonly Moveable moveableScript;
    readonly ClickGUI clickGUI;
    readonly int characterType;
    
    bool _isOnBoat;
    bool clickFlag = true;
    CoastController coastController;
    
    public MyCharacterController(string name, int type)
    {
        character = Object.Instantiate(Resources.Load(type == 0 ? "Perfabs/Priest" : "Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        character.name = name;
        characterType = type;
        moveableScript = character.AddComponent(typeof(Moveable)) as Moveable;
        clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
        clickGUI.setController(this);
    }

    public void setPosition(Vector3 pos)
    {
        character.transform.position = pos;
    }

    public void moveToPosition(Vector3 destination)
    {
        if(clickFlag)
            moveableScript.setDestination(destination);
    }

    public int getType()
    {
        return characterType;
    }

    public string getName()
    {
        return character.name;
    }

    public void getOnBoat(BoatController boatCtrl)
    {
        coastController = null;
        character.transform.parent = boatCtrl.getGameobj().transform;
        _isOnBoat = true;
    }

    public void getOnCoast(CoastController coastCtrl)
    {
        coastController = coastCtrl;
        character.transform.parent = null;
        _isOnBoat = false;
    }

    public bool isOnBoat()
    {
        return _isOnBoat;
    }

    public void setClickFlag(bool flag)
    {
        clickFlag = flag;
    }

    public CoastController getCoastController()
    {
        return coastController;
    }

    public void reset()
    {
        clickFlag = true;
        moveableScript.reset();
        coastController = (Director.getInstance().currentSceneController as FirstController).startCoast;
        getOnCoast(coastController);
        setPosition(coastController.getEmptyPosition());
        coastController.getOnCoast(this);
    }
}
```
MyCharacterController控制的是牧师与魔鬼，有上下船、移动等等特殊行为。MyCharacterController还定义了一些方法提供给场景控制器来调用，方法名已经能够表明这个方法是做什么的了。

**UserAction**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UserAction
{
    void moveBoat();
    void characterIsClicked(MyCharacterController characterCtrl);
    void restart();
}
```
UserAction是一个接口，说明了用户的具有的行为，交给FirstController具体实现。

**FirstController**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController, UserAction
{
    UserGUI userGUI;

    public CoastController startCoast;
    public CoastController endCoast;
    public BoatController boat;
    private MyCharacterController[] characters;

    private int currentCount = 60;
    private System.Timers.Timer timer = new System.Timers.Timer(1000);

    void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new MyCharacterController[6];
        loadResources();
    }

    public void loadResources()
    {
        GameObject water = Instantiate(Resources.Load("Perfabs/Water", typeof(GameObject)), new Vector3(0, 0.5F, 0), Quaternion.identity, null) as GameObject;
        water.name = "water";
        startCoast = new CoastController("start");
        endCoast = new CoastController("end");
        boat = new BoatController();
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(count);
        timer.Start();
        loadCharacter();
    }

    private void loadCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            MyCharacterController cha = new MyCharacterController("priest" + i, 0);
            cha.setPosition(startCoast.getEmptyPosition());
            cha.getOnCoast(startCoast);
            startCoast.getOnCoast(cha);
            characters[i] = cha;
        }
        for (int i = 0; i < 3; i++)
        {
            MyCharacterController cha = new MyCharacterController("devil" + i, 1);
            cha.setPosition(startCoast.getEmptyPosition());
            cha.getOnCoast(startCoast);
            startCoast.getOnCoast(cha);
            characters[i + 3] = cha;
        }
    }

    public void count(object sender, System.Timers.ElapsedEventArgs e)
    {
        userGUI.setCount(currentCount);
        if (currentCount == 0)
            timer.Stop();
        currentCount--;
    }
    
    public void moveBoat()
    {
        if (boat.isEmpty())
            return;
        boat.Move();
        userGUI.status = check_game_over();
    }

    public void characterIsClicked(MyCharacterController characterCtrl)
    {
        if (characterCtrl.isOnBoat())
        {
            CoastController coast = boat.getStatus() == -1 ? endCoast : startCoast;
            boat.GetOffBoat(characterCtrl.getName());
            characterCtrl.moveToPosition(coast.getEmptyPosition());
            characterCtrl.getOnCoast(coast);
            coast.getOnCoast(characterCtrl);
        }
        else
        {
            if (boat.getEmptyIndex() == -1)
                return;
            CoastController coast = characterCtrl.getCoastController();
            if (coast.getType() != boat.getStatus())
                return;
            coast.getOffCoast(characterCtrl.getName());
            characterCtrl.moveToPosition(boat.getEmptyPosition());
            characterCtrl.getOnBoat(boat);
            boat.GetOnBoat(characterCtrl);
        }
        userGUI.status = check_game_over();
    }

    int check_game_over()
    {
        int start_priest = 0;
        int start_devil = 0;
        int end_priest = 0;
        int end_devil = 0;

        int[] startCount = startCoast.getPassengersType();
        start_priest += startCount[0];
        start_devil += startCount[1];

        int[] endCount = endCoast.getPassengersType();
        end_priest += endCount[0];
        end_devil += endCount[1];

        if (end_priest + end_devil == 6)
        {
            timer.Stop();
            return 2;
        }

        int[] boatCount = boat.getPassengersType();
        if (boat.getStatus() == -1)
        {
            end_priest += boatCount[0];
            end_devil += boatCount[1];
        }
        else
        {
            start_priest += boatCount[0];
            start_devil += boatCount[1];
        }
        if (start_priest < start_devil && start_priest > 0 || end_priest < end_devil && end_priest > 0)
        {
            boat.setClickFlag(false);
            for (int i = 0; i < characters.Length; i++)
                characters[i].setClickFlag(false);
            timer.Stop();
            return 1;
        }
        return 0;
    }

    public void restart()
    {
        currentCount = 60;
        userGUI.setCount(currentCount);
        timer.Start();
        boat.reset();
        startCoast.reset();
        endCoast.reset();
        for (int i = 0; i < characters.Length; i++)
            characters[i].reset();
    }
}
```
FirstController负责GUI层与数据层的联系，具有点击、计时、重新开始得到功能。它将view层与controller层的其他controller连接起来，进而接受用户输入，处理底层数据。

**UserGUI**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private UserAction userAction;
    public int status = 0;
    GUIStyle messageStyle, buttonStyle;
    private int currentCount = 0;
    
    void Start()
    {
        userAction = Director.getInstance().currentSceneController as UserAction;
        messageStyle = new GUIStyle();
        messageStyle.fontSize = 40;
        messageStyle.normal.textColor = Color.red;
        messageStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }

    public void setCount(int count)
    {
        currentCount = count;
        if (count == 0)
            status = 1;
    }

    private void OnGUI()
    {
        if (status > 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), status == 1 ? "Gameover!" : "You win!", messageStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 80, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                userAction.restart();
            }
        }
        else if(currentCount > 0)
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), currentCount.ToString(), messageStyle);
    }
}
```
UserGUI负责用户界面生成。

**ClickGUI**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGUI : MonoBehaviour
{
    UserAction action;
    MyCharacterController characterController;

    public void setController(MyCharacterController characterCtrl)
    {
        characterController = characterCtrl;
    }

    void Start()
    {
        action = Director.getInstance().currentSceneController as UserAction;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "boat")
            action.moveBoat();
        else
            action.characterIsClicked(characterController);
    }
}
```
ClickGUI负责用户点击产生的对应行为，它会调用currentSceneController来响应。

**Moveable**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    float move_speed = 20;
    
    int moving_status;
    Vector3 dest;
    Vector3 middle;

    void Update()
    {
        if (moving_status == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, middle, move_speed * Time.deltaTime);
            if (transform.position == middle)
                moving_status = 2;
        }
        else if (moving_status == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, move_speed * Time.deltaTime);
            if (transform.position == dest)
                moving_status = 0;
        }
    }
    public void setDestination(Vector3 _dest)
    {
        dest = _dest;
        middle = _dest;
        if (_dest.y == transform.position.y)
            moving_status = 2;
        else if (_dest.y < transform.position.y)
            middle.y = transform.position.y;
        else
            middle.x = transform.position.x;
        moving_status = 1;
    }

    public void reset()
    {
        moving_status = 0;
    }
}
```
Moveable控制具体的移动行为，决定了角色与船移动的方式与速度。

#### 游戏效果：
![这里写图片描述](https://img-blog.csdn.net/20180403202639725?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
