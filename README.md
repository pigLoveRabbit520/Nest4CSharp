# Nest4CSharp
an open source nest algorithm by C# based on SVGNest

## PolyTree
PolyTree是一种只读数据结构，只能用于接收来自剪裁和偏移操作的解决方案。它是Paths数据结构的一种替代方案，该数据结构也接收这些解决方案。PolyTree相对于Paths结构的两个主要优点是：它正确地表示了返回多边形的父子关系；它区分开放路径和封闭路径。然而，由于PolyTree是一个比Paths结构更复杂的结构，并且处理它的计算成本更高（Execute方法大约慢5-10%），因此仅当需要父子多边形关系时，或者当打开的路径被“剪裁”时，才应使用它。  
[PolyTree](https://documentation.help/polyclipping/_Body6.htm)

## CommonUtil.polygonOffset函数
这个函数会把多边形往外扩一段距离offset，并且在每两个相邻的原始顶点之间插入了一个新的顶点，从而形成了一个更加平滑的路径。  
例如一个多边形 (20, 320) (20, 370) (70, 370) (70, 320)，在往外扩了5个单位后，结果如图：

![alt text](/assets/image.png)
