using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jlob.OpenErpNet;
/*
 * company : NAK
 * url: www.nak-mci.ir
 * author: mehdi samadi
 * version: 1.0
 */

[OpenErpMap("nak.sam.spr.log")]
public class NakSamSprLog
{

    [OpenErpMap("id")]
    public int id { get; set; }
    [OpenErpMap("SAM_SPR_ParentID")]
    public string SAM_SPR_ParentID { get; set; }
    [OpenErpMap("SAM_SPR_ParentDate")]
    public string SAM_SPR_ParentDate { get; set; }
    [OpenErpMap("SAM_SPR_ID")]
    public string SAM_SPR_ID { get; set; }
    [OpenErpMap("SPR_Type")]
    public string SPR_Type { get; set; }
    [OpenErpMap("Requested_By")]
    public string Requested_By { get; set; }
    [OpenErpMap("Supplier_ID")]
    public string Supplier_ID { get; set; }
    [OpenErpMap("Ammount")]
    public string Ammount { get; set; }
    [OpenErpMap("Request_Date")]
    public string Request_Date { get; set; }
    [OpenErpMap("Due_Date")]
    public string Due_Date { get; set; }
    [OpenErpMap("Site_ID")]
    public string Site_ID { get; set; }
    [OpenErpMap("SAM_Contract_ID")]
    public string SAM_Contract_ID { get; set; }
    [OpenErpMap("SAM_Contract_Summary")]
    public string SAM_Contract_Summary { get; set; }
    [OpenErpMap("Comment")]
    public string Comment { get; set; }
    [OpenErpMap("Payment_Validity_From")]
    public string Payment_Validity_From { get; set; }
    [OpenErpMap("Payment_Validity_To")]
    public string Payment_Validity_To { get; set; }
    [OpenErpMap("Analytical_Account")]
    public string Analytical_Account { get; set; }
    [OpenErpMap("Supplier_Acc_NO")]
    public string Supplier_Acc_NO { get; set; }
    [OpenErpMap("Supplier_Acc_Bank")]
    public string Supplier_Acc_Bank { get; set; }
    [OpenErpMap("Supplier_ACC_SHEBA")]
    public string Supplier_ACC_SHEBA { get; set; }
    [OpenErpMap("Supplier_Address")]
    public string Supplier_Address { get; set; }
    [OpenErpMap("Site_Address")]
    public string Site_Address { get; set; }
    [OpenErpMap("Approved_By")]
    public string Approved_By { get; set; }
    [OpenErpMap("Approved_By_Date")]
    public string Approved_By_Date { get; set; }
    [OpenErpMap("Reviewed_By")]
    public string Reviewed_By { get; set; }
    [OpenErpMap("Reviewed_By_Date")]
    public string Reviewed_By_Date { get; set; }
    [OpenErpMap("Initiated_By")]
    public string Initiated_By { get; set; }
    [OpenErpMap("Initiated_By_Date")]
    public string Initiated_By_Date { get; set; }
    [OpenErpMap("Tech_Approved_By")]
    public string Tech_Approved_By { get; set; }
    [OpenErpMap("Tech_Approved_By_Date")]
    public string Tech_Approved_By_Date { get; set; }
    [OpenErpMap("Is_On_Hold")]
    public string Is_On_Hold { get; set; }
    [OpenErpMap("Is_Check_Only")]
    public string Is_Check_Only { get; set; }
    [OpenErpMap("Is_Historic_Rec")]
    public string Is_Historic_Rec { get; set; }
    [OpenErpMap("SPR_Sequence")]
    public string SPR_Sequence { get; set; }
    [OpenErpMap("Total_SPR_Passed")]
    public string Total_SPR_Passed { get; set; }
    [OpenErpMap("Reserved_01")]
    public string Reserved_01 { get; set; }
    [OpenErpMap("Reserved_02")]
    public string Reserved_02 { get; set; }
    [OpenErpMap("Reserved_03")]
    public string Reserved_03 { get; set; }
    [OpenErpMap("Reserved_04")]
    public string Reserved_04 { get; set; }
    [OpenErpMap("reserved_4 ")]
    public string reserved_4 { get; set; }

    [OpenErpMap("Is_Net")]
    public string Is_Net { get; set; }

    [OpenErpMap("Is_Contract")]
    public string Is_Contract { get; set; }

    [OpenErpMap("Contract_Start_Date")]
    public string Contract_Start_Date { get; set; }

    [OpenErpMap("Contract_End_Date")]
    public string Contract_End_Date { get; set; }

    
    
}

