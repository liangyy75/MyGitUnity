### **1.���� ��Ϸ����GameObjects�� �� ��Դ��Assets������������ϵ��**

��Ϸ��������Դ����ϵ������Ϸ����ʵ������һ����������һ��ͳ���壬����Դ��ɣ�����Դ���Ա������Ϸ����ʹ�á�����������Ϸ�����Ǹ���һ��ĸ�����������
���������з��ȵȣ�����Դ��������ǣ�����������ͼ��ȵȡ�

### **2.�ܽ���Դ��������֯�Ľṹ**
��Ϸ����Ľṹ�Ǹ���������֮��Ĺ�ϵ,
��Դ�ļ����԰������塢�زġ�������Ԥ�衢�ű��������ȵ��ļ���Ȼ��ϸ�֣���ͼ��

![����дͼƬ����](http://img.blog.csdn.net/20180326214242499?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)![����дͼƬ����](http://img.blog.csdn.net/20180326214425605?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### **3.��дһ�����룬ʹ�� debug �������֤ MonoBehaviour ������Ϊ���¼�����������**

������Ϊ���� Awake() Start() Update() FixedUpdate() LateUpdate()
�����¼����� OnGUI() OnDisable() OnEnable()

�������£�
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

console��������
![����дͼƬ����](http://img.blog.csdn.net/20180326220009526?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

���console���������ĵ������ǿ��Եõ�MonoBehaviour ������Ϊ���¼������������ֱ��ǣ�

 - awake() : ���ű�ʵ��������ʱ�����Awake������Awake���������е���Ϸ���󱻳�ʼ�����֮��Żᱻ���ã��ڽű�ʵ�����������������У�Awake������ִ��һ�Ρ�
 - onEnable() : ����Ϸ�����Ѿ��������ڻ״̬��ʱ��Ż���ã���ʾ�ýű��Ƿ������ã���������ã��Զ����ø÷�����
 - start() : ��Update������һ�α�����ǰ�����Start������Start����ֻ�ڽű�ʵ��������ʱ�Ż�ִ�У�Start����������Awake����֮��ִ�С�
 - FixedUpdate() : ��MonoBehaviour�ǻ�Ծ״̬ʱ��ÿ���̶�ʱ����������һ�Σ�������Ϸ���е�֡�����޹ء�
 - Update() : Update��֡�����йأ�֡����Խ�ߣ����еĴ���Խ�ߣ�����ÿִ֡��һ�Σ���Ҫ������������Ϸ�ġ�
 - LateUpdate() : �����и�����ɺ�ŵ��õģ���Ҫ����ִ�нű������и��¡�
 - onGUI() :  ���ڳ��ֺʹ���GUI�¼�������ÿ֡���ö�Σ����MonoBehaviour��Ϊfalse��ʱ�򣬽�����ִ�С�
 - onDisable() : ��ʾ��Ϊ��������ʱ�����ø÷�����

### **4.���ҽű��ֲᣬ�˽� GameObject��Transform��Component ����**

#### **4.1�ֱ���ٷ������������������Description��**

> GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.

��Ϸ������ͳһ�ģ���������Ļ������󣬵��ߺͲ��������Ǳ���û��ʵ��̫��Ĺ��ܣ������ǳ䵱�����������ʵ�����������Ĺ��ܡ�
> Every object in a scene has a Transform. It's used to store and manipulate the position, rotation and scale of the object. Every Transform can have a parent, which allows you to apply position, rotation and scale hierarchically. 

�����е�ÿ��������һ��ת���������ڴ洢�Ͳ��������λ�á���ת�����š�ÿһ��ת������һ�����������������ֲ��Ӧ��λ�á���ת�����š�

> Components are the nuts & bolts of objects and behaviors in a game. They are the functional pieces of every GameObject.

�������Ϸ�ж������Ϊ��ϸ�ڡ�������ÿ��GameObject�Ĺ��ܲ��֡�

#### **4.2������ͼ�� table ����ʵ�壩�����ԡ�table �� Transform �����ԡ� table �Ĳ���**

![](https://pmlpml.github.io/unity3d-learning/images/ch02/ch02-homework.png)

table��������ԣ�activeInHierarchy����ʾGameObject�Ƿ��ڳ����д���active״̬����activeSelf��GameObject�ı��ػ״̬����isStatic�����༭��API��ָ����Ϸ�����Ƿ�Ϊ��̬����layer����Ϸ�������ڵ�ͼ�㡣ͼ��ķ�ΧΪ[0 �� 31]����scene����Ϸ���������ĳ�������tag����Ϸ����ı�ǩ����transform�����ӵ����GameObject��ת����

table��Transform�������У�Position��Rotation��Scale�����ĵ��п����˽�������Transform������

table�Ĳ����У�Mesh Filter��Box Collider��Mesh Renderer

#### **4.3�� UML ͼ���� ���ߵĹ�ϵ����ʹ�� UMLet 14.1.1 stand-alone�汾��ͼ��**

![����дͼƬ����](http://img.blog.csdn.net/20180326234249817?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbTBfMzc3ODI0NzM=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### **5.�������ѧϰ���ϣ���д�򵥴�����֤���¼�����ʵ�֣�**

unity���ṩ�˻�ȡ��������ַ�����

 1. ͨ���������ƣ�Find����) : static GameObject Find (string name) �����name�����ǵ����Ķ�������֣�Ҳ������hierarchy�е�һ��·����������ҵ��᷵�ظö���(���)������Ҳ����ͷ���null��
 2. ͨ����ǩ��ȡ������Ϸ����FindWithTag����) : static GameObject FindWithTag (string tag)  ����һ����tag����ʶ�Ļ�Ķ������û���ҵ���Ϊnull��
 3. ͨ����ǩ��ȡ�����Ϸ����FindGameObjectsWithTags����) : static GameObject[] FindGameObjectsWithTag (string tag) ����һ����tag����ʶ�Ļ����Ϸ������б����û���ҵ���Ϊnull��
 4. ͨ�����ͻ�ȡ������Ϸ����FindObjectOfType����) : static Object FindObjectOfType(Type type) ��������Ϊtype�Ļ�ĵ�һ����Ϸ����
 5. ͨ�����ͻ�ȡ�����Ϸ����FindObjectsOfType����) : static Object FindObjectsOfType(Type type) ��������Ϊtype�����еĻ����Ϸ�����б�

����Ӷ��� : public static GameObject CreatePrimitive(PrimitiveTypetype)

���������� : 
foreach (Transform child in transform) {  
    Debug.Log(child.gameObject.name);  
}  

��������Ӷ��� : foreach (Transform child in transform) {  
    Destroy(child.gameObject);  
} 

### **6.��ԴԤ�裨Prefabs���� �����¡ (clone)**

#### **Ԥ�裨Prefabs����ʲô�ô���**

Prefab��һ����Դ���͡����洢����Ŀ��ͼ�е�һ�ֿɷ���ʹ�õ���Ϸ�����������Ϸ����Ҫ�ǳ��෴��ʹ�õĶ�����Դ��ʱ��Prefab����������֮�ء����ܹ��ŵ���������С�Ҳ�ܹ���ͬһ�������з��ö�Ρ�ȫ����Prefabʵ�����ӵ�ԭʼPrefab����������ԭʼPrefab�Ŀ�¡��������Ŀ�д��ڶ���ʵ��������Ҫ��Prefab�����˸Ķ���ȫ��Prefabʵ��������֮�����仯��

#### **Ԥ��������¡ (clone or copy or Instantiate of Unity Object) ��ϵ��**

�����¡�ı������¡���Ķ����໥֮���ǲ�Ӱ��ģ���Ԥ����ͬ��

#### **���� table Ԥ�ƣ�дһ�δ��뽫 table Ԥ����Դʵ��������Ϸ����**

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

### **7.���Խ������ģʽ��Composite Pattern / һ�����ģʽ����ʹ�� BroadcastMessage() �������Ӷ�������Ϣ**

���ģʽ���ֳ�֮Ϊ������-���塱ģʽ�����ڶ���ṹ��ģʽ�����ģʽ�ǽ�������ϳ����νṹ�Ա�ʾ������-���塱�Ĳ�νṹ����ʹ���û��Ե����������϶����ʹ�þ���һ���ԡ�

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

### **8. ���ʵ����С��Ϸ��������**

```
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ο�����
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

    // ��������
    private void ResetChessBoard()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                chessboard[i, j] = 0;
    }

    // �ж�˭Ӯ��
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