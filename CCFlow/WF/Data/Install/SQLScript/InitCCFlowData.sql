/******  Object :  View WF_EmpWorks     Scripts Date : 03/12/2011 21:42:50 ******/;
/*  WF_EmpWorks  */;
CREATE VIEW  WF_EmpWorks
(
PRI,
WorkID,
IsRead,
Starter,
StarterName,
WFState, 
FK_Dept,
DeptName,
FK_Flow,
FlowName,
PWorkID,
PFlowNo,
FK_Node,
NodeName,
Title,
RDT,
ADT,
SDT,
FK_Emp,
FID,
FK_FlowSort,
SDTOfNode,
PressTimes,
GuestNo,
GuestName,
BillNo,
FlowNote,
TodoEmps,
TodoEmpsNum,
TaskSta,
ListType,
Sender,
AtPara
)
AS

SELECT A.PRI,A.WorkID,B.IsRead, A.Starter,
A.StarterName,
A.WFState,
A.FK_Dept,A.DeptName, A.FK_Flow, A.FlowName,A.PWorkID,
A.PFlowNo,
B.FK_Node, B.FK_NodeText AS NodeName, A.Title, A.RDT, B.RDT AS ADT, 
B.SDT, B.FK_Emp,B.FID ,A.FK_FlowSort,A.SDTOfNode,B.PressTimes,
A.GuestNo,
A.GuestName,
A.BillNo,
A.FlowNote,
A.TodoEmps,
A.TodoEmpsNum,
A.TaskSta,
0 as ListType,
B.Sender,
B.AtPara
FROM  WF_GenerWorkFlow A, WF_GenerWorkerlist B
WHERE     (B.IsEnable = 1) AND (B.IsPass = 0)
 AND A.WorkID = B.WorkID AND A.FK_Node = B.FK_Node 
 UNION
SELECT A.PRI,A.WorkID,B.Sta AS IsRead, A.Starter,
A.StarterName,
2 AS WFState,
A.FK_Dept,A.DeptName, A.FK_Flow, A.FlowName,A.PWorkID,
A.PFlowNo,
B.FK_Node, B.NodeName, A.Title, A.RDT, B.RDT AS ADT, 
B.RDT AS SDT, B.CCTo as FK_Emp,B.FID ,A.FK_FlowSort,A.SDTOfNode, 0 as PressTimes,
A.GuestNo,
A.GuestName,
A.BillNo,
A.FlowNote,
A.TodoEmps,
A.TodoEmpsNum,
0 AS TaskSta,
1 as ListType,
B.Rec as Sender,
'@IsCC=1' as AtPara
  FROM WF_GenerWorkFlow A, WF_CCList B WHERE A.WorkID=B.WorkID AND  B.Sta <=1 ;
  
   
 