通用BP组件调用格式:
=================================================


批量编辑组件Ens:
============================================================================================================
参数格式: http://loaclhost/Comm/Ens.aspx?EnsName=实体集合名称
Demo:http://loaclhost/Comm/Ens.aspx?EnsName=BP.Port.Emps


树实体编辑组件Tree:
============================================================================================================
参数格式: http://loaclhost/Comm/Tree.aspx?EnsName=实体集合名称
Demo:http://loaclhost/Comm/Tree.aspx?EnsName=BP.GPM.Depts



查询组件Search:
============================================================================================================
参数格式1: http://loaclhost/Comm/Search.aspx?EnsName=实体集合名称
Demo:http://loaclhost/Comm/Group.aspx?EnsName=BP.Port.Emps

参数格式2: http://loaclhost/Comm/Search.aspx?EnsName=实体集合名称&Key=查询关键字&查询字段名1=查询字段值1&查询字段名n=查询字段值n
Demo:http://loaclhost/Comm/Group.aspx?EnsName=BP.Port.Emps&FK_Dept=001&Key=周


分组分析组件Group:
============================================================================================================
参数格式1:http://loaclhost/Comm/Group.aspx?EnsName=实体集合名称
Demo:http://loaclhost/Comm/Group.aspx?EnsName=BP.Port.Emps

参数格式2: http://loaclhost/Comm/Search.aspx?EnsName=实体集合名称&Key=查询关键字&查询字段名1=查询字段值1&查询字段名n=查询字段值n
Demo:http://loaclhost/Comm/Group.aspx?EnsName=BP.Port.Emps&FK_Dept=001&Key=周



批处理组件Batch:
============================================================================================================
参数格式1:Batch.aspx?EnsName=实体集合名称.
demo:http://loaclhost/Comm/Batch.aspx?EnsName=BP.Port.Emps

参数格式2:Batch.aspx?EnsName=实体集合名称&查询字段1=查询字段值1&查询字段2=查询字段值2
demo:http://loaclhost/Comm/Batch.aspx?EnsName=BP.Port.Emps&FK_Dept=002


卡片组件UIEn:
============================================================================================================
新建一个实体参数格式:  http://localhost/Comm/UIEn.aspx?EnsName=实体集合名.
Demo: http://localhost/Comm/UIEn.aspx?EnsName=BP.WF.Port.Emps

新建一个实体给一些默认值:http://localhost/Comm/UIEn.aspx?EnsName=实体集合名&字段名1=字段值1&字段名n=字段值n
Demo:http://localhost/Comm/RefFunc/UIEn.aspx?EnsName=BP.WF.Port.Emps&FK_Dept=002

查看一个实体信息:http://localhost/Comm/UIEn.aspx?EnsName=实体集合名&PK=主键
Demo:http://localhost/Comm/RefFunc/UIEn.aspx?EnsName=BP.WF.Port.Emps&PK=002
