//创建一个名为"Player"的游戏物体
//并给他添加刚体和立方体碰撞器.
player=new GameObject("Player");
player.AddComponent("Rigidbody");
player.AddComponent("BoxCollider");

//创建一个没有名称的游戏物体
//并给他添加刚体和立方体碰撞器.Transform总是被添加到该游戏物体.
player=new GameObject();
player.AddComponent("Rigidbody");
player.AddComponent("BoxCollider");
//添加名为FoobarScript的脚本到游戏物体
gameObject.AddComponent("FoobarScript");
//添加球形碰撞器到游戏物体
gameObject.AddComponent("FoobarCollider");
注意，没有RemoveComponent()，来移除组件，使用Object.Destroy.



//不激活该游戏物体.
gameObject.active=false;


//附加到这个游戏物体的动画组件（只读）（如果没有为null）
var other: GameObject;
other.animation.Play();

//附加到这个游戏物体的声音组件（只读）（如果没有为null）
var other: GameObject;
other.audio.Play();


//附加到这个游戏物体的相机（只读）（如果没有为null）
var other: GameObject;
other.camera.fieldOfView=45;

//附加到这个游戏物体的碰撞器（只读）（如果没有为null）
var other: GameObject;
other.collider.material.dynamicFriction=1;


//附加到这个游戏物体的恒定力（只读）（如果没有为null）
var other: GameObject;
other.constantForce.relativeForce=Vector3(0,0,1);

//附加到这个游戏物体的GUIText,GUITexture（只读）（如果没有为null）
var other: GameObject;
other.guiText.text="HelloWorld";


//附加到这个游戏物体的HingeJoint（只读）（如果没有为null）
var other: GameObject;
other.hingeJoint Spring.targetPosition=70;

//游戏物体所在的层，一个层在[0...32]之间.
Layer可以用来选择性的渲染或忽略投射.
//设置游戏物体到忽略投射物体的层上
gameObject.layer=2;


//附加到这个游戏物体的光影（只读）（如果没有为null）
var other: GameObject;
other.light.range=10;

//附加到这个游戏物体的网络视（只读）（如果没有为null）
var other: GameObject;
other.networkView.RPC("MyFunction",RPCMode.All,"someValue");


//附加到这个游戏物体的粒子发射器（只读）（如果没有为null）
var other: GameObject;
other.particleEmitter.emite=true;

//附加到这个游戏物体的渲染器（只读）（如果没有为null）
var other: GameObject;
other.renderer.material.color=Color.green;


//附加到这个游戏物体的刚体（只读）（如果没有为null）
var other: GameObject;
other.rigidbody.AddForce(1,1,1);

//标签可以用来标识一个游戏物体。标签在使用前必须在标签管理器中定义。
gameObject.tag="Player";


//附加到这物体的变换. （如果没有为null）
var other: GameObject;
other.transform.Translate(1,1,1);



//在这个游戏物体或其任何子上的每个MonoBehaviour上调用methodName方法。
//通过使用零参数，接收方法可以选择忽略parameter。如果options被设置为
SendMessageOptions.RequireReceiver，那么如果这个消息没有被任何组件接收时将打印一个
错误消息。
///使用值5调用函数ApplyDamage
gameObject.BroadcastMessage("ApplyDamage",5);
//所有附加到该游戏物体和其子物体上脚本中的
//ApplyDamage函数都将调用
function ApplyDamage(damage)

{
     print(damage)
}

 

 

//立即死亡触发器
//销毁任何进入到触发器的碰撞器，这些碰撞器被标记为Player.
function OnTriggerEnter(other: Collider)
{
    if(other.gameObject.CompareTag("Player"))
    {
         Destroy(other.gameObject);
     }
}

 


如果游戏物体有type类型的组件就返回它，否则返回null. 你可以使用这个函数
访问内置的组件或脚本.
GetComponent是防卫其他组件的主要方法。对于Javascript脚本的类型总是脚本显示
在工程视图中的名称。例如：
function Start()
{
     var curTransform: Transform;
     curTransform=gameObject.GetComponent(Transform);
     //这等同于
     curTransform=gameObject.transform;
}
function Update()

{
     //为访问附加在同一游戏物体上
     //其他脚本内的公用变量和函数
     //(ScriptName为Javascript文件名)
     var other: ScriptName=gameObject.GetComponent(ScriptName);
     //调用该脚本中的DoSomething函数
     other DoSomething();
     //设置其他脚本实例中的另一个变量
     other.someVariable=5;
}

 

//返回type类型的组件，这个组件位于这个游戏物体或任何它的子物体上，使用深度优先搜索。
//只有激活的组件被返回。
var script: ScriptName=gameObject.GetComponentInChildren(ScriptName);
script.DoSomething();    

 

 

//返回该游戏物体上所有type类型的组件。
//在这个游戏物体和所有它的子物体上
//的HingeJoints上禁用弹簧
var hingeJoints=gameObject.GetComponents(HingeJoint);
for(var joint: HingeJoint in hingeJoints)

{
     joint.useSpring=false;
}



//返回所有type类型的组件，这些组件位于该游戏物体或任何它的子物体上。
//只有激活的组件被返回。
//在这个游戏物体和所有它的子物体上
//的所有HingeJoints上禁用弹簧
var hingeJoints=gameObject.GetComponentsInChildren(HingeJoint);
for(var joint: HingeJoint in hingeJoints)

{
     joint.useSpring=false;
}

 

//在一个特定的时间采样动画，用于任何动画目的。
//出于性能考虑建议使用Animation接口，这将在给定的time采用animation，任何被动化的组件属性都将被这个采样值替换，多数时候你会使用Animation.Play. SampleAnimation
//用于当你需要以无序方式或给予一些特殊的输入在帧之间跳跃时使用。参见：Aniamtion
//通过采样每一帧或动画剪辑
var clip.AniamtionClip
function Update()
{
    gameObject.sampleAnimation(clip, clip.length-Time.time);
}


//设置这个物体和所以子游戏物体的机会状态。
gameObject.SetActiveRecursion(true);

  

//用几何的网格渲染器和适当的碰撞器创建一个游戏物体。
///在场景中创建一个平面，球体和立方体
function Start()
{
     var plane:GameObject= GameObject.CreatePrimitive(PrimitiveType.Plane);

     var cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
     cube.transform.position=Vector3(0,0.5,0);

      var sphere=GameObject.CreatePrimitive(PrimitiveType.Sphere);
     sphere.transform.position=Vector3(0,1.5,0);

     var capsule=GameObject.CreatePrimitive(PrimitiveType.Capsule);
     capsule.transform.position=Vector3(2,1,0);

      var cylinder=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
     cylinder.transform.position=Vector3(-2,1,0);
}

  

static function Find(name: string): GameObject

描述：依据name查找物体并返回它.
如果没有物体具有名称name返回null. 如果name包含'/'字符它将像一个路径名一样穿
越层次，这个函数只返回激活的游戏物体。
出于性能考虑建议不要在每帧中都是有该函数，而是在开始时调用并在成员变量中缓存结果
或者用GameObject.FindWithTag.
//这返回场景中名为Hand的游戏物体.
hand=GameObject.Find("Hand");
//这将返回名为Hand的游戏物体.
//在层次试图中Hand也许没有父！
hand=GameObject.Find("/Hand");
//这将返回名为Hand的游戏物体.
//它是Arm>Monster的子.
//在层次试图中Monster也许没有父！
hand=GameObject.Find("/Monster/Arm/Hand");
//这将返回名为Hand的游戏物体.
//它是Arm>Monster的子.
//Monster有父.
hand=GameObject.Find("/Monster/Arm/Hand");
这个函数最常用与在加载时自动链接引用到其他物体，例如，在MonoBehaviour.Awake
或MonoBehaviour.Start内部. 处于性能考虑你不应该在每帧中调用这个函数，例如
MonoBehaviour.Update内. 一个通用的模式是在MonoBehaviour.Start内将一个游戏物体赋给
一个变量. 并在MonoBehaviour.Update中使用这个变量.
//在Start中找到Hand并在每帧中选择它
private var hand: GameObject;
function Start()
 
{
     hand=GameObject.Find("/Monster/Arm/Hand");
}
function Update()
{
     hand.transform.Rotate(0,100*Time.deltaTime,0);
}

 

function FindGameObjectsWithTag(tag: string): GameObject[]

描述：返回标记为tag的激活物体列表，如果没有发现返回null.
标签在使用前必须在标签管理中定义。
//在所有标记为"Respawn"的物体位置处
//实例化respawnPrefab
 
var respawnPrefab: GameObject;
var respawns=GameObject.FindGameObjectsWithTag("Respawn");
for(var respawn in respawns)
Instantiate(respawnPrefab, respawn.position, respawn.rotation);
//打印最接近的敌人的名称
print(FindClosestEnemy().name);
//找到最近的敌人的名称
function FindClosestEnemy(): GameObject
{
     //找到所以标记为Enemy的游戏物体
     var gos: GameObject[]
     gos=GameObject.FindGameObjectsWithTag("Enemy");
     var closest: GameObject;
     var distance=Mathf.Infinity;
     var position=transform.position;
     //遍历它们找到最近的一个
     for(var go: GameObject in gos)
     {
           var diff=(go.transform.position-position);
           var curDistance=diff.sqrMagnitude;
           if(curDistance<distance)

 

           {
               closest=go;
               distance=curDistance;
            }
       }
      return closest;
}

 

返回标记为tag的一个激活游戏物体，如果没有发现返回null.
标签在使用前必须在标签管理中定义。
//在标记为"Respawn"的物体位置处
//实例化一个respawnPrefab
var respawnPrefab: GameObject;
var respawns=GameObject.FindWithTag("Respawn");
Instantiate(respawnPrefab, respawn.position, respawn.rotation);
 

主材质的纹理缩放。
这与使用带有"_MainTex"名称的GetTextureScale或SetTextureScale相同。
function Update()

{
     var scalex=Mathf.Cus(Timetime)*0.5+1;
     var scaleY=Mathf.Sin(Timetime)*0.5+1;
     renderer.material.mainTextureScale=Vector2(scaleX,scaleY);
}

参见：SetTextureScale.GetTextureScale.

 

在这个材质中有多少个pass（只读）.
这个最常用在使用GL类之间绘制的代码中（只限于Unity Pro）. 例如，Image Effects使用
材质来实现屏幕后期处理. 对材质中的每一个pass（参考SetPass）它们激活并绘制一个全屏
四边形。
这里是一个全屏图形效果的例子，它反转颜色。添加这个脚本到相机并在播放模式中
查看。
private var mat: Material;
function Start()
{
mat=new Material(
"Shader"Hidden/Invert"{"+
"SubShader{"+
"Pass{"+
"ZTestAlways Cull Off ZWrite Off"+
"SetTexture[_RenderTex]{combine one-texture}"+
"}"+
"{"+
"}"+
);
}
function OnRenderImage(source: RenderTexture, dest: RenderTexture){
RenderTexture.active=dest;
source.SetGlobalShaderProperty("_RenderTex");
GL.PushMatrix();
GL.LoadOrtho();
//对于材质中的每个pass（这里只有一个）
for(var i=0; i<mat.passCount; ++i){
//激活pass
mat.SetPass(i);
//绘制一个四边形
GL.Begin(GLQUADS);
GL.TEXCoord2(0,0); GL.Vertex3(0,0,0.1);
GL.TEXCoord2(1,0); GL.Vertex3(1,0,0.1);
GL.TEXCoord2(1,1); GL.Vertex3(1,1,0.1);
GL.TEXCoord2(0,1); GL.Vertex3(0,1,0.1);
GL.End();
}
GL.PopMatrix();
}

参见：SetPass函数，GL类，ShaderLab documentation.

 

 

该材质使用的着色器。
//按下空格键时，
//在Diffuse和Transparent/Diffuse着色器之间切换
private var shader1=Shader.Find("Diffuse");
private var shader2=Shader.Find("Transparent/Diffuse");
function Update()

{
      if(Input.GetButtonDown("Jump"))

      {
           if(renderer.material.shader--shader1)
               rendere.material.shader=shader2;
           else
               renderer.material.shader=shader1;
       }
}

参见：Shader.Find方法，Material, ShaderLab documentation.

 

 

从一个源shader字符串创建一个材质。
如果你有一个实现自定义特效的脚本，你需要使用着色器和材质实现所有的图像设置。
在你的脚本内使用这个函数创建一个自定义的着色器和材质。在创建材质后，使用SetColor，
SetTexture, SetFloat, SetVector, SetMatrix来设置着色器属性值。
//创建一个附加混合材质并用它来渲染
var color=Color.white;
function Start()
{
var shader Text=
"shader"Alpha Additive"{"+
Properties{_Color("Main Color", Color)=(1,1,1,0)}"+
"SubShader {"+
"Tags {"Queue"="Transparent"}"+
"Pass {"+
"Blend One One ZWrite Off ColorMask RGB"+
"Material {Diffuse[_Color]Ambient[_Color]}"+
"Lighting On"+
"SetTexture[_Dummy]{combine primary double, primary}"+
"}"+
"}"+
"}";
renderer.material=new Material(shaderText);
renderer.material.color=color;
}

 

获取一个命名的颜色值。
数多shader使用超过一个颜色，使用GetColor来获取propertyName颜色。
Unity内置着色器使用的普通颜色名称；
"_Color"为材质的主颜色。这也能够通过color属性访问。
"_SpecColor"为材质的反射颜色（在specular/glossy/vertexlit着色器中使用）。
"_Emission"为材质的散射颜色（用在reflective着色器中使用）。
print(renderder.material.GetColor("_SpecColor));

 

 

获取一个命名纹理。
数多shader使用超过一个纹理。使用GetTexture来获取propertyName纹理。
Unity内置着色器使用的普通纹理名称；
"_MainTex"为主散射纹理. 这也能够通过mainTexture属性访问。
"_BumpMap"为法线贴图。
"_LightMap"为光照贴图。
 
"_Cube"为发射立方体贴图。
function Start()

{
   var tex=renderer.material.GetTexture("_BumpMap");
   if(tex)
        print("My bumpmap is "+ tex.name);
   else
        print("I have no bumpmap!");
}

function GetTextureOffset(propertyName: string): Vector2

function GetTextureScale(propertyName: string): Vector2

 

Mathf.Lerp 插值
当t = 0返回from，当t = 1 返回to。当t = 0.5 返回from和to的平均值。
// 在一秒内从minimum渐变到maximum

var minimum = 10.0;
var maximum = 20.0;

function Update () {
    transform.position = Vector3(Mathf.Lerp(minimum, maximum, Time.time), 0, 0);
}

//像弹簧一样跟随目标物体
var target : Transform;
var smooth = 5.0;
function Update () {
    transform.position = Vector3.Lerp (
    transform.position, target.position,
    Time.deltaTime * smooth);
}

//混合两个材质
var material1: Material;
var material2: Material;
var duration=2.0;
function Start()
{
    //首先使用第一个材质
    renderer.material=material[];
}
function Update()
{
    //随着时间来回变化材质
    var lerp=Mathf.PingPong(Time.time, duration)/duration;
    renderer.material.Lerp(material1, materail2, lerp);
}


设置一个命名的颜色值。
数多shader使用超过一个颜色。使用SetColor来获取propertyName颜色.
Unity内置着色器使用的普通颜色名称；
"_Color"为材质的主颜色. 这也能够通过color属性访问.
"_SpecColor"为材质的反射颜色（在specular/glossy/vertexlit着色器中使用）.
"_Emission"为材质的散射颜色（用在vertexlit着色器中）.
"_ReflectColor"为材质的反射颜色（用在reflective着色器中）.
function Start()

{
    //设置Glossy着色器这样可以使用反射颜色
    renderer.material.shader=Shader.Find("Glossy");
    //设置红色的高光
    renderer.material.SetColor("_SpecColor", Color.red);
}

 

 

SetFloat(propertyName: string, value: float): void

 


描述：设置一个命名的浮点值。
function Start()

 

{
     //在这个材质上使用Glossy着色器
     renderer.material.shader=Shader.Find("Glossy");
}
function Update()

 

{
     //动画Shininess值
     var shininess=Mathf.PingPong(Time.time, 1.0);
     renderer.material.SetFloat("_Shininess, shininess);
}

 

Mathf.PingPong 乒乓
function Update () {
    // Set the x position to loop between 0 and 3
    //设置x位置循环在0和3之间
    transform.position = Vector3(
    Mathf.PingPong(Time.time, 3), transform.position.y, transform.position.z);
} 



//如果当前变换z轴接近目标小于5度的时候，打印"close"
var target : Transform;
function Update () {
    var targetDir = target.position - transform.position;
    var forward = transform.forward;
    var angle = Vector3.Angle(targetDir, forward);
    if (angle < 5.0)
        print("close");
}



Vector3.ClampMagnitude 限制长度
返回向量的长度，最大不超过maxLength所指示的长度。

也就是说，钳制向量长度到一个特定的长度。

var abc : Vector3;

function Start ()
{
    abc=Vector3(0,10,0);
    abc=Vector3.ClampMagnitude(abc, 2);
    //abc返回的是Vector3(0,2,0)
    abc=Vector3.ClampMagnitude(abc, 12);
    //abc返回的是Vector3(0,10,0)
 }

Vector3.Dot 点乘
对于normalized向量，如果他们指向在完全相同的方向，Dot返回1。如果他们指向完全相反的方向，返回-1。对于其他的情况返回一个数（例如：如果是垂直的Dot返回0）。对于任意长度的向量，Dot返回值是相同的：当向量之间的角度减小，它们得到更大的值。
// detects if other transform is behind this object
//检测其他变换是否在这个物体的后面

var other : Transform;
function Update() {
    if (other) {
        var forward = transform.TransformDirection(Vector3.forward);
        var toOther = other.position - transform.position;
        if (Vector3.Dot(forward,toOther) < 0)
            print ("The other transform is behind me!");
    }
}

Vector3.forward 向前
写Vector3(0, 0, 1)的简码,也就是向z轴。
transform.position += Vector3.forward * Time.deltaTime;


Vector3.magnitude 长度
向量的长度是(x*x+y*y+z*z)的平方根。

Vector3.Max 最大
返回一个由两个向量的最大组件组成的向量
var a : Vector3 = Vector3(1, 2, 3);
var b : Vector3 = Vector3(4, 3, 2);
print (Vector3.Max(a,b)); 
// prints (4.0,3.0,3.0)


Vector3.one
Vector3(1, 1, 1)的简码。
transform.position = Vector3.one;


Vector3.operator * 运算符
// make the vector twice longer: prints (2.0,4.0,6.0)
//使该向量变长2倍
print (Vector3(1,2,3) * 2.0);


Vector3.operator +
// prints (5.0,7.0,9.0)
print (Vector3(1,2,3) + Vector3(4,5,6));


static function Project (vector : Vector3, onNormal : Vector3) : Vector3
投射一个向量到另一个。
返回一个向量，这个向量由vector投射到onNormal。 返回0如果onNormal几乎等于或等于0；
即vector垂直投射到onNormal与原点连线的线上的点

Transform就是U3D所封装的矩阵运算，所实现的功能不过就是物体矩阵的运算罢了


 

SimplePath 提供任何类型的高级寻路功能。

包含寻路、转向、路径平滑、动态障碍物避免、地形编辑。
用户手册查看地址：http://www.alexkring.com/SimplePath/SimplePath.pdf
API：http://www.alexkring.com/SimplePath/Doxygen/html/index.html
Demo Video：http://www.youtube.com/watch?v=sTb59eKp6qM
500 Agents Pathfinding + Dynamic Obstacle Avoidance：http://youtu.be/8ZNaNdOFXRw

//加载test下所有资源
function Start () 
{
//需将textres转为Texture2D[]
var textures:Object[] = Resources.LoadAll("_texture/test");
Debug.Log("textures.Length: " + textures.Length);
}

 

//基于时间滚动主纹理
var scrollSpeed=0.5;
function Update()

{
     var offset=Time.time*scrollspeed;
     renderer.material.mainTextureOffset=Vector2(offset,0);
}

参见：SetTextureOffset.GetTextureOffset.