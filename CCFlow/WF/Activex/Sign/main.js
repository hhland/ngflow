/***************** Stamp ******************************
 Explanation :
    WebSign_AddSeal  Add seal interface 
    1. Setting the seal  ( You can not call )
    2. Set the time stamp  ( Can be obtained from the server , May not be invoked )
    3. Setting chapters cover positions  ( You can set the offset relative to the form field position , Effectively prevent the seal dislocation at different resolutions )
    
WebSign  Obtaining support three seal 
   	1. Local stamp papers 
   	2.USBKey Smart Card key disk 
   	3. Remote server 
   	 This demo using the first method .
   	
 If you need to cover more than one stamp on a form , And each chapter bind different regions , Can refer to the development of the document 
WebSign的AddSeal Interface can be set or get the current chapter Name, 
 According to chapter Name, You can set the binding region of each chapter , And authentication data .
 Current examples . Use the default assignment Name, All seals are bound by default all the form data .
***********************************************/	
function WebSign_AddSeal(sealName, sealPostion,signData){
		try{	 		
				// Whether it has been sealed 
				var strObjectName ;
				strObjectName = DWebSignSeal.FindSeal("",0);
				while(strObjectName  != ""){
						if(sealName == strObjectName){
						alert(" Had been stamped with the seal of the current page :【"+sealName+"】 Please verify ");
						return false;
					}
					strObjectName = DWebSignSeal.FindSeal(strObjectName,0); 
				}
				
				// Set the current form field seal bound 
				Enc_onclick(signData);

				// Setting the seal , Can be OA Username 
				document.all.DWebSignSeal.SetCurrUser(" The seal ");
				// Server path 
				//	document.all.DWebSignSeal.HttpAddress = "http://127.0.0.1:8089/inc/seal_interface/";
				// Online only page ID ,SessionID
				//	document.all.DWebSignSeal.RemoteID = "0100018";
				// This location can be a good seal fixed 
				document.all.DWebSignSeal.SetPosition(-10,-50,sealPostion);
				// Call seal interface 
				document.all.DWebSignSeal.AddSeal("", "test");
		}catch(e) {
		  alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
		}
	}	
/******************* Full-screen handwriting ****************************
 Explanation :
    WebSign_HandWrite Add seal interface 
    1. Setting signer  ( You can not call )
    2. Setting the time signature  ( Can be obtained from the server , May not be invoked ) 
 
   	
 If you need to cover more than one signature on a form , And each signature to bind different regions , Can refer to the development of the document 
WebSign的HandWrite Interface can be set or get the current signature Name, 
 According signature Name, You can set each signature binding region , And authentication data .
 Current examples . Use the default assignment Name, All signatures are bound by default all the form data .
***********************************************/	
function WebSign_HandWrite(sealName, sealPostion,signData){
	try{ 
		// Set the current form field seal bound 
		Enc_onclick(signData);
		// Setting signer , Can be OA Username 
		document.all.DWebSignSeal.SetCurrUser(" Transcriber people 111");
		// Setting the time signature , Server can pass over 
 		//document.all.SetCurrTime("2006-02-07 11:11:11");
                 // Signatures calling interface 
		document.all.DWebSignSeal.SetPosition(100,10,sealPostion);
		if("" == document.all.DWebSignSeal.HandWrite(0,255,sealName)){
			 alert(" Full-screen signature failure ");
			 return false;
		} 
	}catch(e) {
		alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
	}
}

/******************* Pop-up handwriting ****************************
 Explanation :
    HandWritePop_onclick  Pop-up handwriting 
    1. Setting signer  ( You can not call )
    2. Setting the time signature  ( Can be obtained from the server , May not be invoked )
    3. Set signature cover positions  ( You can set the offset relative to the form field position , Effectively prevent the signature dislocation at different resolutions )
       	
 If you need to cover more than one signature on a form , And each signature to bind different regions , Can refer to the development of the document 
WebSign的HandWrite Interface can be set or get the current signature Name, 
 According signature Name, You can set each signature binding region , And authentication data .
 Current examples . Use the default assignment Name, All signatures are bound by default all the form data .
***********************************************/	
function HandWritePop_onclick(){
	try{ 
			// Set the current form field seal bound 
			Enc_onclick();
			// Setting signer , Can be OA Username 
			document.all.DWebSignSeal.SetCurrUser(" Pop handwriting people ");
			// Setting the time signature , Server can pass over 
			//document.all.SetCurrTime("2006-02-07 11:11:11");
			// Set the current position of the seal , With respect to sealPostion1 (<div id="handWritePostion1"> </div>)  Position at odds shift 0px, Shifted upward 0px
			// This location can be a good seal fixed 
			document.all.DWebSignSeal.SetPosition(0,0,"rPos");
			// Signatures calling interface 
			if("" == document.all.DWebSignSeal.HandWritePop(0,255,200, 400,300,"")){
				 alert(" Full-screen signature failure ");
				 return false;
			}
		}catch(e) {
		  alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
		}
}	
		
/***************** Form submission ******************************
 Explanation :
    submit  Submit Form 
     Calling WebSign的GetStoreData() Interface to obtain the seal of all the data ( Seal data + Certificate data + Signature data ...)
     This value is assigned to Hiddle Variable , Saved to the database .
***********************************************/		
 
function submit_onclick(){
try{
	var v = document.all.DWebSignSeal.GetStoreData();
	if(v.length < 200){
		alert(" Must be sealed before they can submit ");
		return false;
	}
	document.all.form1.sealdata.value = v;
	}catch(e) {
		alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
		return false;
	}
}
 
 
/***********************************************
 Explanation :
    Enc_onclick  The main setting binding form fields .
    WebSign的SetSignData Interface supports two way data binding :
    1. String data 
    2. Form fields 
     Once the data is changed ,WebSign Automatically check , And prompt changes .
***********************************************/	

function Enc_onclick(tex_name) {		
	try{
		// Empty the contents of the original binding 	
		document.all.DWebSignSeal.SetSignData("-");		
		// str To be binding string data 
		//var str = "";
		 // Form binding domain settings 
		 // Communications Unit 
		 document.all.DWebSignSeal.SetSignData("+LIST:laiwendanwei;");
		 // Date of communication 
		 document.all.DWebSignSeal.SetSignData("+LIST:laiwenDate;");
		 // Subject 
		 document.all.DWebSignSeal.SetSignData("+LIST:shiyou;");
		 // Time requirements 
		 document.all.DWebSignSeal.SetSignData("+LIST:time;");
		  // Opinion 
		 document.all.DWebSignSeal.SetSignData("+LIST:"+tex_name+";");
		 
		/* Organize themselves according to the form field content to bind content , Current examples are only bound to do with the form field 
		 If the binding string data , Need to do the following call 
			document.all.DWebSignSeal.SetSignData("+DATA:"+str);		
		*/
	}catch(e) {
		alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
	}
	}
	
/***********************************************
 Explanation :
    SetUI  Set the user interface style 
***********************************************/	
function SetUI() {
	try{
		 document.all.DWebSignSeal.TipBKLeftColor = 29087;
		 document.all.DWebSignSeal.TipBKRightColor = 65443;
		 document.all.DWebSignSeal.TipLineColor = 65535;
		 document.all.DWebSignSeal.TipTitleColor = 32323;
		 document.all.DWebSignSeal.TipTextColor = 323;
	}catch(e) {
		alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
	}
 }



 function ShowSeal() {

     document.all.DWebSignSeal.ShowWebSeals();
 }
 function GetAllSeal() {
     var strSealName = document.all.DWebSignSeal.AddSeal("", "");
     alert(strSealName);
     strSealName = document.all.DWebSignSeal.FindSeal("", 0);
 }
 function addseal(objectname) {
     /* vSealName: Seal Name 
     vSealPostion: Seal binding position 
     vSealSignData: Data binding seal 
     */
     var vSealName = objectname;
     var vSealPostion = objectname + "sealpostion";
     var vSealSignData = objectname;
     WebSign_AddSeal(vSealName, vSealPostion, vSealSignData);
     Change('SealData');
 }
 function handwrite(objectname) {
     /* vSealName: Seal Name 
     vSealPostion: Seal binding position 
     vSealSignData: Data binding seal 
     */
     var vSealName = objectname + "handwrite";
     var vSealPostion = objectname + "sealpostion";
     var vSealSignData = objectname;
     WebSign_HandWrite(vSealName, vSealPostion, vSealSignData);
 }
 function checkData() {
     try {
         var strObjectName;
         strObjectName = document.all.DWebSignSeal.FindSeal("", 0);
         while (strObjectName != "") {
             //document.all.DWebSignSeal.VerifyDoc(strObjectName); 
             strObjectName = document.all.DWebSignSeal.FindSeal(strObjectName, 0);
         }
     } catch (e) {
         //alert(" Control is not installed , Please refresh this page , Controls will be automatically downloaded .\r\n Or download the installer to install ." +e);
     }
 }
 function GetValue_onclick() {
     var v = document.all.DWebSignSeal.GetStoreData();

     var valueData = document.getElementById('SealData').value;
     if (v == valueData) {
         alert(" Must be sealed before they can submit ");
         return false;
     }

     if (v.length == "") {
         alert(" Must be sealed before they can submit ");
         return false;
     }
     document.all.SealData.value = v;
 }
 function SetValue_onclick() {
     document.all.DWebSignSeal.SetStoreData(document.all.SealData.value);
     document.all.DWebSignSeal.ShowWebSeals();
 }
 function popshouxie_onClick() {
     //	document.all.DWebSignSeal.SetCurrUser(" The seal ");
     //	document.all.DWebSignSeal.HttpAddress = "127.0.0.1:1127";				
     //	alert(document.all.DWebSignSeal.Login());

     HandWritePop_onclick();
 }
