using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jlob.OpenErpNet;


    [OpenErpMap("nak.sam.supplier.req")]
    class NakSamSupplier
    {
        [OpenErpMap("id")]
        public int id { get; set; }
        [OpenErpMap("name")]
            public string name {get; set;}
        [OpenErpMap("name_en")]
            public string name_en {get; set;}
        [OpenErpMap("is_company")]
            public bool is_company {get; set;}
        [OpenErpMap("sam_db_id")]
            public string sam_db_id {get; set;}
        [OpenErpMap("requested_by")]
            public string requested_by {get; set;}
        [OpenErpMap("request_type")]
            public string request_type {get; set;}
        [OpenErpMap("national_id")]
            public string national_id {get; set;}
        [OpenErpMap("economic_id")]
            public string economic_id {get; set;}
        [OpenErpMap("contact_ref")]
            public string contact_ref {get; set;}
        [OpenErpMap("bank_name")]
            public string bank_name {get; set;}
        [OpenErpMap("acc_owner")]
            public string acc_owner {get; set;}
        [OpenErpMap("acc_number")]
            public string acc_number {get; set;}
        [OpenErpMap("shaba_number")]
            public string shaba_number {get; set;}
        [OpenErpMap("sam_default")]
            public bool sam_default {get; set;}
        [OpenErpMap("note")]
            public string note {get; set;}
        [OpenErpMap("reserved_01")]
            public string reserved_01 {get; set;}
        [OpenErpMap("reserved_02")]
            public string reserved_02 {get; set;}
        [OpenErpMap("reserved_03")]
            public string reserved_03 {get; set;}
        [OpenErpMap("reserved_04")]
            public string reserved_04 {get; set;}
        [OpenErpMap("reserved_05")]
        public string reserved_05  { get; set; }
    [OpenErpMap("LL_ID")]
    public string LL_ID { get; set; }

    [OpenErpMap("address")]
    public string address { get; set; }

    [OpenErpMap("city")]
    public string city { get; set; }

    [OpenErpMap("country")]
    public string country { get; set; }
}

