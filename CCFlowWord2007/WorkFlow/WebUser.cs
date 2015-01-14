using System;
using System.IO;
using BP.DA;
using BP.WF;
using BP.Port;
using CCFlowWord2007.WorkFlow;

namespace BP.Web
{
    public class WebUser
    {
        #region  Personal property 
        private static int _FK_Node;
        /// <summary>
        ///  What execution ?
        /// </summary>
        public static string DoWhat;
        public static bool isLogin;
        /// <summary>
        ///  Personnel Number 
        /// </summary>
        public static string No;
        public static string Name;
        public static string Pass;
        public static Dept HisDept;
        public static string FK_DeptOfShiJu
        {
            get
            {
                return FK_Dept.Substring(0, 2);
            }
        }
       
        public static string FK_Dept;
        public static string FK_DeptName;
        public static Int64 WorkID;
        public static Int64 FID=0;
        private static string _FK_Flow;
        private static string _FK_FlowName;
        private static string _FK_NodeName;
        /// <summary>
        ///  The current process node 
        /// </summary>
        public static WFNode CurrentNode;
        public static string FK_Flow
        {
            get
            {
                return _FK_Flow;
            }
            set
            {
                _FK_Flow = value;
            }
        }
        public static string FK_FlowName
        {
            get
            {
                return _FK_FlowName;
            }
            set
            {
                _FK_FlowName = value;
            }
        }
        /// <summary>
        ///  Node Name 
        /// </summary>
        public static string FK_NodeName
        {
            get
            {
                return _FK_NodeName;
            }
            set
            {
                _FK_NodeName = value;
            }
        }

        public static int FK_Node
        {
            get
            {
                if (FK_Flow == null)
                    return 0;

                if (_FK_Node == 0 || _FK_Node == 1)
                {
                    _FK_Node = int.Parse(FK_Flow + "01");
                }

                return _FK_Node;
            }
            set
            {
                _FK_Node = value;
            }
        }

        public static bool IsStartNode
        {
            get
            {
                return int.Parse(FK_Flow + "01") == FK_Node;
            }
        }
        public static bool IsSavePass;
        public static bool IsSaveInfo;

        public static string DoType;
        public static string SID;
        private static Work _HisWork;
        public static Work HisWork
        {
            get
            {
                if (WorkID == 0)
                    return null;

                return _HisWork ?? (_HisWork = new Work(FK_Node, WorkID));
            }
            set
            {

                _HisWork = value;
                WorkID = _HisWork.OID;
            }
        }
        public static CCFlowWord2007.Ribbon1 HisRib;
        #endregion

        #region  Public property 
        public static string AppServWorkID
        {
            get
            {
                return DBAccess.GetWebConfigByKey("WorkID");
            }
        }
        public static string AppServFK_Flow
        {
            get
            {
                return DBAccess.GetWebConfigByKey("FK_Flow");
            }
        }
        public static string AppServFtpUser
        {
            get
            {
                return DBAccess.GetWebConfigByKey("FtpUser");
            }
        }
        public static string AppServFtpPass
        {
            get
            {
                return DBAccess.GetWebConfigByKey("FtpPass");
            }
        }
        #endregion

        #region Methods

        public static bool LoadProfile()
        {
            if (Profile.IsExitProfile)
            {
                Profile.ProfileDoc = null;
                try
                {
                    if (Directory.Exists(Glo.PathOfTInstall) == false)
                        Directory.CreateDirectory(Glo.PathOfTInstall);

                    No = Profile.GetValByKey("No");
                    Name = Profile.GetValByKey("Name");
                    FK_Dept = Profile.GetValByKey("FK_Dept");

                    //FK_Node = Profile.GetValIntByKey("FK_Node");
                    //WorkID = Profile.GetValIntByKey("WorkID");
                    //FK_Flow = Profile.GetValByKey("FK_Flow");

                    FK_Node = 0; // Profile.GetValIntByKey("FK_Node");
                    WorkID = 0; // Profile.GetValIntByKey("WorkID");
                    FK_Flow = null; // nuProfile.GetValByKey("FK_Flow");

                    switch (DoType)
                    {
                        case DoTypeConst.DoStartFlowByTemple: // I want to open ppt.
                        case DoTypeConst.DoStartFlow: // I want to open ppt.
                            break;
                        default:
                            break;
                    }


                    if (DoType != null)
                    {
                        DoType = "";  //  Clear labeling .
                        WriterIt();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("@ Error loading time personal confidence , Possible destruction of personal information files :" + ex.Message);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="emp"></param>
        public static void Sigin(Emp emp)
        {
            isLogin = true;
            No = emp.No;
            Name = emp.Name;
            FK_Dept = emp.FK_Dept;
            DoType = "";
            CurrentNode = null; // Blank 

            GetFtpInfomation();

            WriterIt();
        }

        /// <summary>
        ///  Sign out 
        /// </summary>
        public static void SignOut()
        {
            No = null;
            Name = null;
            Pass = null;
            WorkID = 0;
            FK_Dept = null;
            FK_DeptName = null;
            FK_Flow = null;
            FK_Node = 0;
            HisDept = null;
            isLogin = false;
            IsSaveInfo = false;
            IsSavePass = false;
            SID = null;

            if (File.Exists(Glo.Profile))
                File.Delete(Glo.Profile);
        }

        /// <summary>
        ///  Write to file 
        /// </summary>
        public static void WriterIt()
        {
            WriterIt(DoType, FK_Flow, FK_Node, WorkID);
        }

        /// <summary>
        ///  Write to file 
        /// </summary>
        /// <param name="dotype"></param>
        /// <param name="fk_flow"></param>
        /// <param name="fk_node"></param>
        /// <param name="workid"></param>
        public static void WriterIt(string dotype, string fk_flow, int fk_node, Int64 workid)
        {
            if (IsSavePass == false)
                return;

            string strLocalPath = Glo.PathOfTInstall;
            if (Directory.Exists(strLocalPath) == false)
                Directory.CreateDirectory(strLocalPath);

            string strContent = "@No=" + No + "@Name=" + Name + "@FK_Dept=" +
                FK_Dept + "@FK_DeptName=" + FK_DeptName
                + "@WorkID=" + workid + "@FK_Flow=" +
             fk_flow + "@FK_Node=" + fk_node + "@DoType=" + dotype;
            File.WriteAllText(Glo.Profile, strContent);
        }

        public static void WriterCookes()
        {
            if (IsSaveInfo == false)
                return;

            string strLocalPath = Glo.PathOfTInstall;
            if (Directory.Exists(strLocalPath) == false)
                Directory.CreateDirectory(strLocalPath);

            string strContent = "@No=" + No + "@Name=" + Name;
            File.WriteAllText(Glo.ProfileLogin, strContent);
        }

        /// <summary>
        ///  Get FTP Information 
        /// </summary>
        private static void GetFtpInfomation()
        {
            Glo.FtpIP = DBAccess.GetWebConfigByKey("FtpIP");
            Glo.FtpUser = DBAccess.GetWebConfigByKey("FtpUser");
            Glo.FtpPass = DBAccess.GetWebConfigByKey("FtpPass");
        }

        /// <summary>
        ///  Get process node information 
        /// </summary>
        /// <param name="nodeId"> Node id</param>
        public static void RetrieveWFNode(int nodeId)
        {
            try
            {
                CurrentNode = new WFNode(nodeId);
            }
            catch (Exception ex)
            {
                throw new Exception(" An error occurred while obtaining the process node ", ex);
            }
        }

        #endregion
    }
}
