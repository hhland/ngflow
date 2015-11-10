using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BP.WF;
using BP.DA;
using BP.Port;
using BP.Web;
using BP.WF.Data;
using BP.En;
using BP.Sys;

namespace BP.WF
{
    /// <summary>
    ///  Form Engine api
    /// </summary>
    public class CCFormAPI:Dev2Interface
    {
        /// <summary>
        ///  Generate reports 
        /// </summary>
        /// <param name="templeteFilePath"> Template path </param>
        /// <param name="ds"> Data Sources </param>
        /// <returns> Path generation documents </returns>
        public static void Frm_GenerBill(string templeteFullFile, string saveToDir, string saveFileName,
            BillFileType fileType, DataSet ds, string fk_mapData)
        {

            MapData md = new MapData(fk_mapData);
            GEEntity entity = md.GenerGEEntityByDataSet(ds);

            BP.Pub.RTFEngine rtf = new BP.Pub.RTFEngine();
            rtf.HisEns.Clear();
            rtf.EnsDataDtls.Clear();

            rtf.HisEns.AddEntity(entity);
            var dtls = entity.Dtls;

            foreach (var item in dtls)
                rtf.EnsDataDtls.Add(item);

            rtf.MakeDoc(templeteFullFile, saveToDir, saveFileName, null, false);
        }
    }
}
