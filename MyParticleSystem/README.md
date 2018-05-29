[toc]
### 1.作业要求
> 参考 http://i-remember.fr/en 这类网站，使用粒子流编程控制制作一些效果。
### 2.实现过程
这次作业实话说相对于之前的几次模块小游戏编程来说是非常的轻松的，因为前几次要求的代码量大，而且都是一些完整的小游戏，还要求用上一些编程模式，而现在只要专心于一个效果，而且最重要的是最终效果很好看。
我参考了两篇博客：[Unity3d 粒子光环制作](https://blog.csdn.net/shendw818/article/details/79920911)、[Unity3D学习笔记（9）—— 粒子光环](https://blog.csdn.net/simba_scorpio/article/details/51251126)
然后看看效果图：
![粒子光环效果图](https://img-blog.csdn.net/20180529220159387?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

> 可以看到效果图中光环是有最大半径和最小半径，而且粒子都是集中于平均半径上的，而且粒子是分成两部分的，一部分顺时针旋转，一部分逆时针旋转。而且粒子颜色是循环变化的，粒子的位置是游离的，而不是简单的旋转。

##### 2.1游戏对象建立
先在Hierarchy界面建立一个空对象，命名为Main，然后右键点击Main，选择Effects，选择Particle System，再将生成的对象重命名为Ring。
![这里写图片描述](https://img-blog.csdn.net/20180529221244881?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L20wXzM3NzgyNDcz/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
##### 2.2圆环制备
新建脚本，命名为MyRing.cs，利用脚本对粒子的行为进行控制。

 - 先提供一些公有变量，方便我们通过外部通过这些参数来设置粒子系统的属性。
 
```
    public ParticleSystem _particleSystem;      // 粒子系统对象。
    public int maxParticleNumber = 10000;       // 发射的最大粒子数
    public float pingPong = 0.05f;              // 游离范围
    public float particleSize = 0.05f;          // 粒子的大小
    public float maxRadius = 10, minRadius = 5; // 粒子旋转半径范围
    public float particleSpeed = 0.05f;         // 粒子运动的速度
```

 - 然后写一些私有变量，用来控制粒子的位置、颜色。

```
    private float[] particleAngles;         // 每个粒子的位置偏移角
    private float[] particleRadius;         // 每个粒子的运动半径
    private float time = 0;
    private ParticleSystem.Particle[] particleArray;
    public Color[] colors = { new Color(255, 255, 255), new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 255, 0), new Color(0, 255, 255), new Color(255, 0, 255), new Color(0, 0, 0) };
    private float colorTimeOut = 0;
    private Gradient gradient;              // 颜色控制器
    private GradientAlphaKey[] alphaKeys;
    private GradientColorKey[] colorKeys;
```

 -  最后在start函数里面初始化刚才的那些各种属性，为那些数组申请内存，然后设置各个粒子的初始位置，通过随机生成的半径、角度来控制粒子的位置。同时初始化梯度颜色控制器。
 
```
    void Start()
    {
        // 初始化粒子数组、粒子角度数组、粒子半径数组
        _particleSystem = GetComponent<ParticleSystem>();
        particleArray = new ParticleSystem.Particle[maxParticleNumber];
        particleAngles = new float[maxParticleNumber];
        particleRadius = new float[maxParticleNumber];

        // 初始化粒子系统
        _particleSystem.startSpeed = 0;
        _particleSystem.loop = false;
        _particleSystem.maxParticles = maxParticleNumber;
        _particleSystem.Emit(maxParticleNumber);            // 发射粒子
        _particleSystem.GetParticles(particleArray);

        // 初始化各粒子位置
        for (int i = 0; i < maxParticleNumber; i++)
        {
            // 随机生成每个粒子距离中心的半径，同时希望粒子集中于平均半径附近
            float midRadius = (maxRadius + minRadius) / 2;
            float rate1 = Random.Range(1.0f, midRadius / minRadius);
            float rate2 = Random.Range(midRadius / maxRadius, 1.0f);
            particleRadius[i] = Random.Range(minRadius * rate1, maxRadius * rate2);
            // 设置粒子大小
            particleArray[i].startSize = particleSize;
            // 随机生成每个粒子的角度
            particleAngles[i] = Random.Range(0, 360);
            float rad = particleAngles[i] / 180 * Mathf.PI;
            particleArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0); // 放置粒子
        }

        _particleSystem.SetParticles(particleArray, particleArray.Length);

        // 初始化梯度颜色控制器
        alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.6f;
        colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1; colorKeys[1].color = Color.white;
        gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
    }
```

将粒子集中于平均半径的做法就是像上面一样，先求出平均半径，然后取一个位于1和平均半径与最大/最小半径的比值的区间的随机数，再乘以最大半径/最小半径，作为上限/下限就可以随机生成一些集中于平均半径的随机数，作为粒子的位置。这个原理是可以通过概率学证明的，再这里我就不多说了。

##### 2.3旋转
- 上面的代码生成的粒子已经是圆形了，虽然一下子就会散开，但只要在update里面限制住每个粒子的位置就行了，而粒子的旋转也是通过改变粒子的位置来实现的，而在这里就要将粒子分为两份，一份逆时针旋转，一份顺时针旋转，由粒子在粒子数组中的下标来决定。

```
	void Update()
	{
		for (int i = 0; i < particleArray.Length; i++)
		{
			// 更改粒子角度(一半在逆时针，一半在顺时针)
			particleAngles[i] = (particleAngles[i] + (i % 2 == 0 ? particleSpeed : 0 - particleSpeed) + 360) % 360;
			// 设置粒子位置
            float rad = particleAngles[i] / 180 * Mathf.PI;
            particleArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0);
		}
        _particleSystem.SetParticles(particleArray, particleArray.Length);
	}
```

- 令粒子有不同的速度，让它们看起来更灵动些。
可以通过改变粒子速度的增量来改变粒子的速度，将其分为10个层级，如下：

```
particleAngles[i] = (particleAngles[i] + (i % 10 + 1) * (i % 2 == 0 ? particleSpeed : 0 - particleSpeed) + 360) % 360;
```

- 给粒子添加粒子波动效果，而且波动是是在一定范围内的。
在Update函数的for循环里面添加这样的语句，利用unity的PingPong方法实现粒子抖动，PingPong可以使值在一定范围内波动，具体的描述可以看官方文档。

```
time += Time.deltaTime;
particleRadius[i] += (Mathf.PingPong(time / minRadius / maxRadius, pingPong) - pingPong / 2);
```

- 更改粒子的颜色，并改变粒子的透明度。
可以通过更改GradientColorKey的color属性，然后用Gradient重新渲染一下，再根据位置给每个粒子设置颜色和透明度，代码如下：

```
colorKeys[0].time = 0;
colorKeys[1].time = 1;
colorKeys[0].color = colors[(int)colorTimeOut % colors.Length];
colorKeys[1].color = colors[(int)colorTimeOut % colors.Length];
gradient.SetKeys(colorKeys, alphaKeys);
particleArray[i].startColor = gradient.Evaluate(particleAngles[i] / 360.0f);
```

##### 2.4改进
这个时候可以看到大致的粒子效果 —— 一个旋转而且颜色变幻的圆环了，但是由于unity的粒子系统不够给力，只要粒子太小粒子的颜色就暗淡下来了，所以这个圆环很暗，所以为了让圆环清晰些，我们可以用Glow11插件(听说还可以用shader方法，但由于我是初学者，还是走一些捷径吧)。
首先下载Glow11插件，然后，通过Assets -> Import Package->Custom Package，找到Glow11插件，将其导入。
选定摄像机，在Inspector面板中，在Add Component处，找到Glow11脚本，将其增加到摄像机的Inspector面板中（记得在脚本那里打钩）。 
最后找到粒子系统的Renderer处的Material，将其改为Default-Material即可。

### 3.源代码

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRing : MonoBehaviour
{
    public ParticleSystem _particleSystem;      // 粒子系统对象。
    public int maxParticleNumber = 10000;       // 发射的最大粒子数
    public float pingPong = 0.05f;              // 游离范围
    public float particleSize = 0.05f;          // 粒子的大小
    public float maxRadius = 10, minRadius = 5; // 粒子旋转半径范围
    public float particleSpeed = 0.05f;         // 粒子运动的速度

    private float[] particleAngles;         // 每个粒子的位置偏移角
    private float[] particleRadius;         // 每个粒子的运动半径
    private float time = 0;
    private ParticleSystem.Particle[] particleArray;
    public Color[] colors = { new Color(255, 255, 255), new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 255, 0), new Color(0, 255, 255), new Color(255, 0, 255), new Color(0, 0, 0) };
    private float colorTimeOut = 0;
    private Gradient gradient;              // 颜色控制器
    private GradientAlphaKey[] alphaKeys;
    private GradientColorKey[] colorKeys;

    // Use this for initialization
    void Start()
    {
        // 初始化粒子数组、粒子角度数组、粒子半径数组
        _particleSystem = GetComponent<ParticleSystem>();
        particleArray = new ParticleSystem.Particle[maxParticleNumber];
        particleAngles = new float[maxParticleNumber];
        particleRadius = new float[maxParticleNumber];

        // 初始化粒子系统
        _particleSystem.startSpeed = 0;
        _particleSystem.loop = false;
        _particleSystem.maxParticles = maxParticleNumber;
        _particleSystem.Emit(maxParticleNumber);            // 发射粒子
        _particleSystem.GetParticles(particleArray);

        // 初始化各粒子位置
        for (int i = 0; i < maxParticleNumber; i++)
        {
            // 随机生成每个粒子距离中心的半径，同时希望粒子集中于平均半径附近
            float midRadius = (maxRadius + minRadius) / 2;
            float rate1 = Random.Range(1.0f, midRadius / minRadius);
            float rate2 = Random.Range(midRadius / maxRadius, 1.0f);
            particleRadius[i] = Random.Range(minRadius * rate1, maxRadius * rate2);
            // 设置粒子大小
            particleArray[i].startSize = particleSize;
            // 随机生成每个粒子的角度
            particleAngles[i] = Random.Range(0, 360);
            float rad = particleAngles[i] / 180 * Mathf.PI;
            particleArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0); // 放置粒子
        }

        _particleSystem.SetParticles(particleArray, particleArray.Length);

        // 初始化梯度颜色控制器
        alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.6f;
        colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1; colorKeys[1].color = Color.white;
        gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
    }

    // Update is called once per frame
    void Update()
    {
        colorTimeOut += Time.deltaTime;
        for (int i = 0; i < particleArray.Length; i++)
        {
            time += Time.deltaTime;
            // 更改粒子角度(一半在逆时针，一半在顺时针，而且速度有10个层级)
            particleAngles[i] = (particleAngles[i] + (i % 10 + 1) * (i % 2 == 0 ? particleSpeed : 0 - particleSpeed) + 360) % 360;
            // 更改粒子半径(造成粒子波动效果，而且波动是在一定范围内的)
            particleRadius[i] += (Mathf.PingPong(time / minRadius / maxRadius, pingPong) - pingPong / 2);
            // 更改粒子颜色
            // particleArray[i].startColor = colors[(int)colorTimeOut % colors.Length];
            colorKeys[0].time = 0;
            colorKeys[1].time = 1;
            colorKeys[0].color = colors[(int)colorTimeOut % colors.Length];
            colorKeys[1].color = colors[(int)colorTimeOut % colors.Length];
            gradient.SetKeys(colorKeys, alphaKeys);
            particleArray[i].startColor = gradient.Evaluate(particleAngles[i] / 360.0f);
            // 设置粒子位置
            float rad = particleAngles[i] / 180 * Mathf.PI;
            particleArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0);
        }
        _particleSystem.SetParticles(particleArray, particleArray.Length);
    }
}
```
