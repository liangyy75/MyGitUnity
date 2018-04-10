### 1.参考 Fantasy Skybox FREE 构建自己的游戏场景
- <h4>资源准备</h4>
在菜单栏Assets的import package中有两个资源包：Environment、Characters，将它们下载导入。再到Asset Store中搜索Fantasy Skybox Free，将其下载导入(我在资源商店中没有找到这个资源，所以我就用Skybox Series Free替代了)。
提醒一下， 你可以在C:\Users\用户名AppData\Roaming\Unity\Asset Store中找到自己从Asset Store下载的资源。
- <h4>创建地形</h4>
先在Hierarchy窗口创建一个Terrain对象，然后在Terrain的Inspector窗口中对其进行修改，如下图：
![这里写图片描述](https://img-blog.csdn.net/20180409224533197?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
(1).1可以造山，将它选中后，再点击下面brushes中的图案，按住鼠标左键，就可以在Scene场景中“造山”，而brushes中的图案则会影响你造山的形状，Bursh Size代表刷子的大小，Opacity则是敏感程度。如果这时你按住shift键，就可以反过来磨平你之前造出的山。下面的也是一样，可以通过shift键来取消自己之前的造物。
(2).2可以制造等高地形，用它你可以制造一个高原，其中Height属性决定高原的高度。
(3).3可以将你造出的山峰的棱角磨平，可以让山峰变得更为平缓。
(4).4可以绘制纹理，在Textures哪里选择一个纹理之后就可以绘制你的Terrain的表面。选择的第一个纹理将是主纹理，其他纹理是在其基础上绘制的。
(5).5可以种树，在Trees选好预制体后可以在Scene场景中通过点击来种树。
(6).6可以种草，可以绘制Terrain的表面细节，具体操作与种树一样。
**我的地形构建如下：**
![这里写图片描述](https://img-blog.csdn.net/2018040923072921?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
(其实里面是有草地的，只是太小了观察不到。)
- <h4>创建天空盒</h4>
先创建一个Materials，命名为skybox，再将它的Shader改为Skybox/6 Sided，然后在资源Fantasy Skybox中找到下面六张图导入。
![这里写图片描述](https://img-blog.csdn.net/20180409232400317?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
它们的名称分别是FluffballDay加上Back/Bottom/Front/Left/Right/Top，然后根据名字将对应的图片放入到skybox中。
![这里写图片描述](https://img-blog.csdn.net/20180409232739604?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
在Tint Color中着色，可以形成类似阴天、黄昏的效果。
将Exposure的值改变，可以调整曝光，达到傍晚、黑夜的效果。
**我的Skybox如下：**
![这里写图片描述](https://img-blog.csdn.net/20180409233513253?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
在这里我是通过调整Exposure的值来形成傍晚的效果的，
- <h4>整合场景</h4>
为摄像机添加天空盒组件：在菜单栏Component中，选择rending中的skybox，再将自己之前创建的天空盒添加到skybox组件中。
创建自己的人物：从之前导入的资源Characters中选择ThirdPersonCharacter中的预制体，取消它的重力：
![这里写图片描述](https://img-blog.csdn.net/20180409234140507?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
调整它的位置，然后就可以在自己设置的场景中为所欲为了。
![这里写图片描述](https://img-blog.csdn.net/20180409234551684?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

### 2.写一个简单的总结，总结游戏对象的使用
在unity3d文档中GameObject有以下定义

> GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.

所以，一个游戏对象其实只是一个容器，它只有一些基础的属性，剩下的还得自己通过添加组件来构建它。我们可以给它们添加C# script，写出各种方法来定义它的行为和属性，控制它的动作，然后我们还可以给它们添加纹理，给他们各种外观，我们还可以添加其他组件来定义它的属性、行为，达到各种效果。游戏对象是由它拥有的组件来决定它的功能的。

### 3编程实践  牧师与魔鬼 动作分离版
#### UML图
之前我曾做了一份牧师与魔鬼的小游戏，但在之前的基础上添加了一个动作管理器。
我将自己修改的主要部分做成UML图，如下：
![这里写图片描述](https://img-blog.csdn.net/20180410201434611?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

 - ActionCallback接口是用来说明动作已完成。
 - ObjAction是所有动作的基类。ActionManager就是通过ObjAction这个接口来管理动作的。
 - MoveToAction是ObjAction的一个实现，它代表一个直线移动的动作。
 - SequenceAction也继承于ObjAction，它代表一系列的直线运动，即折线运动。
 - ActionManager是管理动作的类，负责执行、销毁动作。
 - FirstSceneActionManager是ActionManager的子类，FirstController就是通过它来管理所有动作的。在FirstSceneActionManager中对具体的需求做了封装，让代码更简洁。
 
#### 代码展示
 **ActionCallBack**
```
public interface ActionCallBack{
    void actionDone(ObjAction source);
}
```
**ObjAction**
```
public class ObjAction : ScriptableObject{
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject;
    public Transform transform;
    public ActionCallBack actionCallBack;

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}
```
**MoveToAction**
```
public class MoveToAction : ObjAction {
    public Vector3 target;
    public float speed;

    private MoveToAction() { }
    public static MoveToAction getAction(Vector3 target, float speed)
    {
        MoveToAction action = ScriptableObject.CreateInstance<MoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

	public override void Start () { }

    public override void Update ()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.actionCallBack.actionDone(this);
        }
    }
}
```
**SequenceAction**
```
public class SequenceAction : ObjAction, ActionCallBack {
    public List<ObjAction> objActions;
    public int repeat = 1;
    public int index = 0;

    public static SequenceAction getAction(List<ObjAction> objActions, int repeat, int index)
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.objActions = objActions;
        action.repeat = repeat;
        action.index = index;
        return action;
    }

    public override void Update()
    {
        if (objActions.Count == 0)
            return;
        if (index < objActions.Count)
        {
            objActions[index].Update();
        }
    }

    public void actionDone(ObjAction source)
    {
        source.destroy = false;
        if(++index >= objActions.Count)
        {
            index = 0;
            if (repeat > 0)
                repeat--;
            if (repeat == 0)
            {
                destroy = true;
                actionCallBack.actionDone(this);
            }
        }
    }

    public override void Start()
    {
        foreach (ObjAction action in objActions)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.actionCallBack = this;
            action.Start();
        }
    }

    private void OnDestroy()
    {
        foreach (ObjAction action in objActions)
            DestroyObject(action);
    }
}
```
**ActionManager**
```
public class ActionManager : MonoBehaviour, ActionCallBack {
    private Dictionary<int, ObjAction> actions = new Dictionary<int, ObjAction>();
    private List<ObjAction> waitingAdd = new List<ObjAction>();
    private List<int> waitingDelete = new List<int>();

    public void actionDone(ObjAction source)
    {
        
    }
    
	// Update is called once per frame
	void Update () {
        foreach (ObjAction action in waitingAdd)
            actions.Add(action.GetInstanceID(), action);
        waitingAdd.Clear();

        foreach (KeyValuePair<int, ObjAction> action in actions)
        {
            ObjAction objAction = action.Value;
            if (objAction.destroy)
                waitingDelete.Add(objAction.GetInstanceID());
            else if(objAction.enable)
                objAction.Update();
        }

        foreach (int key in waitingDelete)
        {
            ObjAction action = actions[key];
            actions.Remove(key);
            DestroyObject(action);
        }
        waitingDelete.Clear();
	}

    public void addAction(GameObject gameObject, ObjAction action, ActionCallBack actionCallBack)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.actionCallBack = actionCallBack;
        waitingAdd.Add(action);
        action.Start();
    }
}
```
**FirstSceneActionManager**
```
public class FirstSceneActionManager : ActionManager {
    public void moveBoat(BoatController boat)
    {
        MoveToAction action = MoveToAction.getAction(boat.getDestination(), boat.speed);
        this.addAction(boat.getGameobj(), action, this);
    }

    public void moveCharacter(MyCharacterController character, Vector3 dest)
    {
        Vector3 now = character.getPosition();
        Vector3 mid = now;
        if (dest.y > now.y)
            mid.y = dest.y;
        else
            mid.x = dest.x;
        ObjAction action1 = MoveToAction.getAction(mid, character.speed);
        ObjAction action2 = MoveToAction.getAction(dest, character.speed);
        ObjAction sequenceAction = SequenceAction.getAction(new List<ObjAction> { action1, action2 }, 1, 0);
        this.addAction(character.GetGameObject(), sequenceAction, this);
    }
}
```
最后就是一些细节上的修改了。