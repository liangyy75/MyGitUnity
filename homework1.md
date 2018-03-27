### **1.解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。**

游戏对象与资源的联系在于游戏对象实际上是一个容器，是一个统合体，由资源组成，而资源可以被多个游戏对象使用。区别在于游戏对象是更上一层的概念，可以是玩家
、环境、敌方等等，而资源则组成他们，包括声音、图像等等。

### **2.总结资源、对象组织的结构**
游戏对象的结构是父类与子类之间的关系,
资源文件可以包含字体、素材、场景、预设、脚本、动作等等文件，然后细分，如图：

![这里写图片描述](http://img.blog.csdn.net/20180326214242499?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)![这里写图片描述](http://img.blog.csdn.net/20180326214425605?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### **3.编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件**

基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
常用事件包括 OnGUI() OnDisable() OnEnable()

代码如下：
```
	// Use this for initialization
	void Start () {
        Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Update");
	}

    void Awake()
    {
        Debug.Log("Awake!");
    }

    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
    }

    void LateUpdate()
    {
        Debug.Log("LateUpdate");
    }

    void OnGUI()
    {
        Debug.Log("OnGUI");
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
    }
```

console输出结果：
![这里写图片描述](http://img.blog.csdn.net/20180326220009526?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

结合console输出结果与文档，我们可以得到MonoBehaviour 基本行为或事件触发的条件分别是：

 - awake() : 当脚本实例被加载时会调用Awake函数；Awake函数在所有的游戏对象被初始化完毕之后才会被调用；在脚本实例的整个生命周期中，Awake函数仅执行一次。
 - onEnable() : 当游戏对象已经启动处于活动状态的时候才会调用，表示该脚本是否起作用，如果起作用，自动调用该方法。
 - start() : 当Update函数第一次被调用前会调用Start函数；Start函数只在脚本实例被启用时才会执行；Start函数总是在Awake函数之后执行。
 - FixedUpdate() : 当MonoBehaviour是活跃状态时，每隔固定时间间隔就运行一次，它和游戏运行的帧速率无关。
 - Update() : Update和帧速率有关，帧速率越高，运行的次数越高，它是每帧执行一次，主要是用来更新游戏的。
 - LateUpdate() : 是所有更新完成后才调用的，主要用于执行脚本来进行更新。
 - onGUI() :  用于呈现和处理GUI事件，可能每帧调用多次，如果MonoBehaviour设为false的时候，将不会执行。
 - onDisable() : 表示行为不起作用时，调用该方法。

### **4.查找脚本手册，了解 GameObject，Transform，Component 对象**

#### **4.1分别翻译官方对三个对象的描述（Description）**

> GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.

游戏对象是统一的，代表人物的基本对象，道具和布景。它们本身没有实现太多的功能，但它们充当组件的容器，实现它们真正的功能。
> Every object in a scene has a Transform. It's used to store and manipulate the position, rotation and scale of the object. Every Transform can have a parent, which allows you to apply position, rotation and scale hierarchically. 

场景中的每个对象都有一个转换。它用于存储和操作对象的位置、旋转和缩放。每一个转换都有一个父级，它允许您分层地应用位置、旋转和缩放。

> Components are the nuts & bolts of objects and behaviors in a game. They are the functional pieces of every GameObject.

组件是游戏中对象和行为的细节。它们是每个GameObject的功能部分。

#### **4.2描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件**

![](https://pmlpml.github.io/unity3d-learning/images/ch02/ch02-homework.png)

table对象的属性：activeInHierarchy（表示GameObject是否在场景中处于active状态）、activeSelf（GameObject的本地活动状态）、isStatic（仅编辑器API，指定游戏对象是否为静态）、layer（游戏对象所在的图层。图层的范围为[0 … 31]）、scene（游戏对象所属的场景）、tag（游戏对象的标签）、transform（附加到这个GameObject的转换）

table的Transform的属性有：Position、Rotation、Scale，从文档中可以了解更多关于Transform的属性

table的部件有：Mesh Filter、Box Collider、Mesh Renderer

#### **4.3用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）**

![这里写图片描述](http://img.blog.csdn.net/20180326234249817?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### **5.整理相关学习资料，编写简单代码验证以下技术的实现：**

unity中提供了获取对象的五种方法：

 1. 通过对象名称（Find方法) : static GameObject Find (string name) 传入的name可以是单个的对象的名字，也可以是hierarchy中的一个路径名，如果找到会返回该对象(活动的)，如果找不到就返回null。
 2. 通过标签获取单个游戏对象（FindWithTag方法) : static GameObject FindWithTag (string tag)  返回一个用tag做标识的活动的对象，如果没有找到则为null。
 3. 通过标签获取多个游戏对象（FindGameObjectsWithTags方法) : static GameObject[] FindGameObjectsWithTag (string tag) 返回一个用tag做标识的活动的游戏物体的列表，如果没有找到则为null。
 4. 通过类型获取单个游戏对象（FindObjectOfType方法) : static Object FindObjectOfType(Type type) 返回类型为type的活动的第一个游戏对象
 5. 通过类型获取多个游戏对象（FindObjectsOfType方法) : static Object FindObjectsOfType(Type type) 返回类型为type的所有的活动的游戏对象列表

添加子对象 : public static GameObject CreatePrimitive(PrimitiveTypetype)

遍历对象树 : 
foreach (Transform child in transform) {  
    Debug.Log(child.gameObject.name);  
}  

清除所有子对象 : foreach (Transform child in transform) {  
    Destroy(child.gameObject);  
} 

### **6.资源预设（Prefabs）与 对象克隆 (clone)**

#### **预设（Prefabs）有什么好处？**

Prefab是一种资源类型——存储在项目视图中的一种可反复使用的游戏对象。因而当游戏中须要非常多反复使用的对象、资源等时，Prefab就有了用武之地。它能够放到多个场景中。也能够在同一个场景中放置多次。全部的Prefab实例链接到原始Prefab，本质上是原始Prefab的克隆。不论项目中存在多少实例。仅仅要对Prefab进行了改动。全部Prefab实例都将随之发生变化。

#### **预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？**

对象克隆的本体与克隆出的对象相互之间是不影响的，而预设则不同。

#### **制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象**

```
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject table;

	// Use this for initialization
	void Start () {
        Debug.Log("Start");
        GameObject newTable = (GameObject)Instantiate(table);
        newTable.name = "newTable";
        newTable.transform.position = new Vector3(0, 0, UnityEngine.Random.Range(4, 8));
        newTable.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Update");
	}
}
```

### **7.尝试解释组合模式（Composite Pattern / 一种设计模式）。使用 BroadcastMessage() 方法向子对象发送消息**

组合模式，又称之为“部分-整体”模式，属于对象结构型模式。组合模式是将对象组合成树形结构以表示“部分-整体”的层次结构，它使得用户对单个对象和组合对象的使用具有一致性。

```
public class Homework : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Start");
        this.BroadcastMessage("testMethod", "hi");
	}

    public void testMethod(string message)
    {
        print(message);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Update");
	}
}
```

### **8. 编程实践，小游戏：井字棋**

```
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 参考博客
// https://blog.csdn.net/shendw818/article/details/79689656
// https://blog.csdn.net/zzj051319/article/details/62040762
// https://blog.csdn.net/alva112358/article/details/79675793

public class Tic_Tac_Toe : MonoBehaviour {
    private int[,] chessboard = new int[3, 3];
    int state = 1;

    private void Start()
    {
        ResetChessBoard();
    }

    private void OnGUI()
    {
        int midWidth = Screen.width / 2;
        int midHeight = Screen.height / 2;
        int buttonEdge = Screen.height / 5;

        GUIStyle redStyle = new GUIStyle();
        redStyle.fontSize = buttonEdge / 2;
        redStyle.fontStyle = FontStyle.Bold;
        redStyle.normal.textColor = Color.red;

        GUIStyle blueStyle = new GUIStyle();
        blueStyle.fontSize = buttonEdge / 2;
        blueStyle.fontStyle = FontStyle.Bold;
        blueStyle.normal.textColor = Color.blue;

        GUIStyle blackStyle = new GUIStyle();
        blackStyle.fontSize = buttonEdge / 2;
        blackStyle.fontStyle = FontStyle.Bold;
        blackStyle.normal.textColor = Color.black;

        GUIStyle anotherBlue = new GUIStyle(blueStyle);
        anotherBlue.fontSize = buttonEdge;

        GUIStyle anotherRed = new GUIStyle(redStyle);
        anotherRed.fontSize = buttonEdge;

        int winner = WhetherWin();
        if (winner == 1)
        {
            if (GUI.Button(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Blue Win", blackStyle))
                ResetChessBoard();
        }
        else if (winner == 2)
        {
            if (GUI.Button(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Red Win", redStyle))
                ResetChessBoard();
        }
        else if (winner == 0)
        {
            GUI.Label(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Welcome!", blackStyle);
        }
        else if (winner == 3)
        {
            if (GUI.Button(new Rect(midWidth - buttonEdge * 1.5f, 0.5f * buttonEdge, 3 * buttonEdge, 4.5f * buttonEdge), "Tied", blackStyle))
                ResetChessBoard();
        }

        if (GUI.Button(new Rect(midWidth - 0.5f * buttonEdge, midHeight + 1.75f * buttonEdge, buttonEdge, 0.5f * buttonEdge), "Reset"))
            ResetChessBoard();

        for (int i = 0; i < 3; i++)
            for(int j = 0; j < 3; j++)
            {
                float x = midWidth - 1.5f * buttonEdge + j * buttonEdge;
                float y = midHeight - 1.5f * buttonEdge + i * buttonEdge;
                if (chessboard[i, j] == 1)
                    GUI.Button(new Rect(x, y, buttonEdge, buttonEdge), "O", anotherBlue);
                else if (chessboard[i, j] == 2)
                    GUI.Button(new Rect(x, y, buttonEdge, buttonEdge), "X", anotherRed);
                if(GUI.Button(new Rect(x, y, buttonEdge, buttonEdge), ""))
                {
                    if (state == 0)
                        chessboard[i, j] = 1;
                    else if (state == 1)
                        chessboard[i, j] = 2;
                    state = 1 - state;
                }
            }
    }

    // 重置棋盘
    private void ResetChessBoard()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                chessboard[i, j] = 0;
    }

    // 判断谁赢了
    private int WhetherWin()
    {
        int count = 0;
        for(int i = 0; i < 3; i++)
        {
            if (chessboard[i, 0] != 0 && chessboard[i, 0] == chessboard[i, 1] && chessboard[i, 0] == chessboard[i, 2])
                return chessboard[i, 0];
            if (chessboard[0, i] != 0 && chessboard[0, i] == chessboard[1, i] && chessboard[0, i] == chessboard[2, i])
                return chessboard[0, i];
            for (int j = 0; j < 3; j++)
                if (chessboard[i, j] != 0)
                    count++;
        }
        if (chessboard[1, 1] != 0 && chessboard[0, 0] == chessboard[1, 1] && chessboard[1, 1] == chessboard[2, 2])
            return chessboard[0, 0];
        if (chessboard[1, 1] != 0 && chessboard[2, 0] == chessboard[1, 1] && chessboard[0, 2] == chessboard[1, 1])
            return chessboard[0, 2];
        if (count == 9)
            return 3;
        return 0;
    }
}
```