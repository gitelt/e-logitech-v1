﻿<!--  #INCLUDE FILE="../include/transaction.txt" -->
<% Option Explicit %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Shipping Notice</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!--  #INCLUDE FILE="../include/connection.asp" -->
    <!--  #INCLUDE FILE="../include/header.asp" -->
     <!--  #INCLUDE FILE="../include/ScriptHeader.inc" -->
    <!--  #INCLUDE FILE="../include/GOOFY_data_manager.inc" -->
    <!--  #INCLUDE FILE="../include/GOOFY_Util_fun.inc" -->
    <!--  #INCLUDE FILE="../include/SaveBinaryFile.asp" -->
    <%
    Response.CharSet = "UTF-8"
    Session.CodePage = "65001"
    
    Dim vYourName, vYourEmail, vBillType, vBillNum, vMode, vDBA
    Dim vMAWB, vHAWB, feData, SQL, i, j, aAllData, aOrgData, aHouseData
    
    vBillType = Request.QueryString("type")
    vBillNum = Request.QueryString("no")
    vMode = Request.QueryString("mode")
    vDBA = GetSQLResult("select dba_name from agent where elt_account_number=" & elt_account_number, "dba_name")
    vYourName = login_name
    vYourEmail = GetSQLResult("SELECT user_email FROM users WHERE userid=" & user_id & " AND elt_account_number=" & elt_account_number, Null)
    
    eltConn.BeginTrans()
    
    Set feData = new DataManager
    
    If vMode = "search" Then
    
        If vBillType = "house" Then
        
            SQL = "select 'N' as isDirect,b.booking_num,b.mbol_num,b.hbol_num,b.shipper_acct_num as agent_no,b.shipper_name as agent_name,ISNULL(c.invoice_no,-1) as invoice_no,d.owner_email,d.edt,d.agent_elt_acct,e.MsgTxt " _
                & "FROM mbol_master a LEFT OUTER JOIN hbol_master b ON (a.elt_account_number=b.elt_account_number AND a.booking_num=b.booking_num) " _
                & "LEFT OUTER JOIN invoice c ON (b.elt_account_number=c.elt_account_number AND b.booking_num=c.mawb_num AND b.hbol_num=c.hawb_num AND c.air_ocean='O' AND c.import_export='E') " _
                & "LEFT OUTER JOIN organization d ON (b.elt_account_number=d.elt_account_number AND b.agent_no=d.org_account_number) " _
                & "LEFT OUTER JOIN greetMessage e ON (b.elt_account_number=e.AgentID AND MsgType='OE/Shipping Notice') " _
                & "WHERE a.elt_account_number=" & elt_account_number & " AND b.booking_num=(SELECT booking_num FROM hbol_master " _
                & "WHERE elt_account_number=" & elt_account_number & " AND hbol_num=N'" & vBillNum & "') AND ISNULL(b.agent_no,0)<>0 ORDER BY b.agent_name, b.hbol_num"

        Elseif vBillType = "master" Then
        
            SQL = "select 'N' as isDirect,b.booking_num,b.mbol_num,b.hbol_num,b.shipper_acct_num as agent_no,b.shipper_name as agent_name,ISNULL(c.invoice_no,-1) as invoice_no,d.owner_email,d.edt,d.agent_elt_acct,e.MsgTxt " _
                & "FROM mbol_master a LEFT OUTER JOIN hbol_master b ON (a.elt_account_number=b.elt_account_number AND a.booking_num=b.booking_num) " _
                & "LEFT OUTER JOIN invoice c ON (b.elt_account_number=c.elt_account_number AND b.booking_num=c.mawb_num AND b.hbol_num=c.hawb_num AND c.air_ocean='O' AND c.import_export='E') " _
                & "LEFT OUTER JOIN organization d ON (b.elt_account_number=d.elt_account_number AND b.agent_no=d.org_account_number) " _
                & "LEFT OUTER JOIN greetMessage e ON (b.elt_account_number=e.AgentID AND MsgType='OE/Shipping Notice') " _
                & "WHERE a.elt_account_number=" & elt_account_number & " AND b.booking_num=N'" & vBillNum & "' AND ISNULL(b.agent_no,0)<>0 ORDER BY b.agent_name, b.hbol_num"
            
            '// direct shipment 
            If Not IsDataExist(SQL) Then
                SQL = "select 'Y' as isDirect,a.booking_num,a.mbol_num,'' AS hbol_num,a.shipper_acct_num as agent_no,a.shipper_name as agent_name,ISNULL(c.invoice_no,-1) as invoice_no,d.owner_email,d.edt,d.agent_elt_acct,e.MsgTxt " _
                    & "FROM mbol_master a LEFT OUTER JOIN invoice c ON (a.elt_account_number=c.elt_account_number AND a.booking_num=c.mawb_num AND ISNULL(c.hawb_num,'')='' AND c.air_ocean='O' AND c.import_export='E') " _
                    & "LEFT OUTER JOIN organization d ON (a.elt_account_number=d.elt_account_number AND a.consignee_acct_num=d.org_account_number) " _
                    & "LEFT OUTER JOIN greetMessage e ON (a.elt_account_number=e.AgentID AND MsgType='OE/Shipping Notice') " _
                    & "WHERE a.elt_account_number=" & elt_account_number & " AND a.booking_num=N'" & vBillNum & "' AND ISNULL(a.consignee_acct_num,0)<>0 ORDER BY a.consignee_name"
            End If
            
        Elseif vBillType = "file" Then
        
            SQL = "select 'N' as isDirect,b.booking_num,b.mbol_num,b.hbol_num,b.shipper_acct_num as agent_no,b.shipper_name as agent_name,ISNULL(c.invoice_no,-1) as invoice_no,d.owner_email,d.edt,d.agent_elt_acct,e.MsgTxt " _
                & "FROM mbol_master a LEFT OUTER JOIN hbol_master b ON (a.elt_account_number=b.elt_account_number AND a.booking_num=b.booking_num) " _
                & "LEFT OUTER JOIN invoice c ON (b.elt_account_number=c.elt_account_number AND b.booking_num=c.mawb_num AND b.hbol_num=c.hawb_num AND c.air_ocean='O' AND c.import_export='E') " _
                & "LEFT OUTER JOIN organization d ON (b.elt_account_number=d.elt_account_number AND b.agent_no=d.org_account_number) " _
                & "LEFT OUTER JOIN greetMessage e ON (b.elt_account_number=e.AgentID AND MsgType='OE/Shipping Notice') " _
                & "LEFT OUTER JOIN ocean_booking_number f ON (a.elt_account_number=f.elt_account_number AND a.booking_num=f.booking_num) " _
                & "WHERE a.elt_account_number=" & elt_account_number & " AND f.file_no=N'" & vBillNum & "' AND ISNULL(b.agent_no,0)<>0 ORDER BY b.agent_name, b.hbol_num"
            
            '// direct shipment 
            If Not IsDataExist(SQL) Then
                SQL = "select 'Y' as isDirect,a.booking_num,a.mbol_num,'' AS hbol_num,a.shipper_acct_num as agent_no,a.shipper_name as agent_name,ISNULL(c.invoice_no,-1) as invoice_no,d.owner_email,d.edt,d.agent_elt_acct,e.MsgTxt " _
                    & "FROM mbol_master a LEFT OUTER JOIN invoice c ON (a.elt_account_number=c.elt_account_number AND a.booking_num=c.mawb_num AND ISNULL(c.hawb_num,'')='' AND c.air_ocean='O' AND c.import_export='E') " _
                    & "LEFT OUTER JOIN organization d ON (a.elt_account_number=d.elt_account_number AND a.consignee_acct_num=d.org_account_number) " _
                    & "LEFT OUTER JOIN greetMessage e ON (a.elt_account_number=e.AgentID AND MsgType='OE/Shipping Notice') " _
                    & "LEFT OUTER JOIN ocean_booking_number f ON (a.elt_account_number=f.elt_account_number AND a.booking_num=f.booking_num) " _
                    & "WHERE a.elt_account_number=" & elt_account_number & " AND f.file_no=N'" & vBillNum & "' AND ISNULL(a.consignee_acct_num,0)<>0 ORDER BY a.consignee_name"
            End If
        Else

        End If

        feData.SetDataList(SQL)
        Set aAllData = feData.getDataList
        Set aOrgData = DataListGroupBy(aAllData,"agent_no")
        
    End If
    
    eltConn.CommitTrans()
    
    
    Function GetUserFileList(vOrgNum)
        Dim aUserFileList, i, SQL, rs
        Set aUserFileList = Server.CreateObject("System.Collections.ArrayList")
        
        If IsNull(vOrgNum) Then
            vOrgNum = "-1"
        End If
        
        SQL = "SELECT [file_name] FROM user_files WHERE elt_account_number=" _
            & elt_account_number & " AND org_no=" & vOrgNum & " AND file_checked='Y'"
        
        Set rs = Server.CreateObject("ADODB.RecordSet")
        rs.Open SQL, eltConn, adOpenForwardOnly,adLockReadOnly,adCmdText
        '// Set rs.activeConnection = Nothing
        
        Do While Not rs.EOF And Not rs.BOF
            aUserFileList.Add rs("file_name")
            rs.MoveNext
        Loop
        Set GetUserFileList = aUserFileList
    End Function
    
    Function DataListSelect(aDataList, vField, vValue)
        Dim newDataList, i
        Set newDataList = Server.CreateObject("System.Collections.ArrayList")
        
        For i = 0 To aDataList.Count-1
            If aDataList(i)(vField) = vValue Then
                newDataList.Add aDataList(i)
            End If
        Next
        Set DataListSelect = newDataList
    End Function
   
    Function DataListGroupBy(aDataList, vField)
        Dim aValueList,newDataList,i,j
        
        Set aValueList = Server.CreateObject("System.Collections.ArrayList")
        Set newDataList = Server.CreateObject("System.Collections.ArrayList")
        
        For i = 0 To aDataList.Count-1
            If Not aValueList.Contains(aDataList(i)(vField)) Then
                newDataList.Add aDataList(i)
                aValueList.Add aDataList(i)(vField)
            End If
        Next
        
        Set DataListGroupBy = newDataList
    End Function
    
    %>
    
    <link href="../css/elt_css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {color: #663366}
        body {
	        margin-left: 0px;
	        margin-right: 0px;
	        margin-bottom: 0px;
        }
        a:hover {
	        color: #CC3300;
        }
    </style>

    <script type="text/jscript">
    
        function selectSearchType()
        {
            // lstSearchNumChange(-1,'');
        }
        
        function lstSearchNumChange(argV,argL){
	        var typeValue = document.getElementById("lstSearchType").value
	        var url = "shipper_notice.asp?mode=search&type=" + typeValue + "&no=" + argV;
            self.window.location.href = encodeURI(url);
        }

        function searchNumFill(obj,eType,changeFunction,vHeight){
            var qStr = obj.value;
            var keyCode = event.keyCode;
            var typeValue = document.getElementById("lstSearchType").value

            if(qStr != "" && keyCode != 229 && keyCode != 27 && typeValue != ""){
                var url = "/ASP/ajaxFunctions/ajax_get_bills.asp?mode=list&export=" + eType +"&qStr=" 
                    + qStr + "&type=" + typeValue;
                FillOutJPED(obj,url,changeFunction,vHeight);
            }
        }
        
        function searchNumFillAll(objName,eType,changeFunction,vHeight){
            var obj = document.getElementById(objName);
            var typeValue = document.getElementById("lstSearchType").value;
            
            if(typeValue != "")
            {
                var url = "/ASP/ajaxFunctions/ajax_get_bills.asp?mode=list&type=" 
                    + typeValue + "&export=" + eType;
                FillOutJPED(obj,url,changeFunction,vHeight);
            }
        }
        
        function ShowEmailHistory(){
            viewPop("/IFF_MAIN/ASPX/MISC/EmailHistory.aspx?title=Shipping Notice&ao=O&ie=E");
        }
        
        function LoadPage(){
            findSelect(document.getElementById("lstSearchType"),"<%=vBillType %>");
            document.getElementById("lstSearchNum").value = "<%=vBillNum %>";
            document.getElementById("hSearchNum").value = "<%=vBillNum %>";
        }
        
        function findSelect(oSelect,selVal)
        {
            oSelect.options.selectedIndex = 0;

            for(var i=0;i<oSelect.options.length;i++)
            {
                if(oSelect.options[i].value == selVal)
                {
                    oSelect.options[i].selected = true;
                    break;
                }
            }
        }
        
        var arg10;

        function receiveAttachedFile(returnValue){                
            var htmlStr = returnValue.length + " Attached File(s)";            
            document.getElementById("aAttachedFiles" + arg10).innerHTML = returnValue.length + " Attached File(s)";
        }

        function UploadFile(arg){
            arg10=arg;
            try{
                var winArg = "dialogWidth:550px; dialogHeight:400px; help:0; status:0; scroll:1; center:1; Sunken;";
                var returnValue = showModalDialog("upload_file.asp?WindowName=FileUpload&OrgNo=" + arg, "FileUpload", winArg);
                var htmlStr = returnValue.length + " Attached File(s)";
            
                document.getElementById("aAttachedFiles" + arg).innerHTML = returnValue.length + " Attached File(s)";
            }catch(err){}
        }
        
        function chkOrgClick(arg,thisObj){
            var tableObj = document.getElementById("tblOrg" + arg)
            var checkVal = thisObj.checked;
            
            if(!checkVal){
                makeAllDiabled(tableObj);
            }
            
            else{
                makeAllEnabled(tableObj);
            }
        }
        <% If agent_status = "A" Or agent_status = "T" Then %>
        function SendClick(){
            if(ValidateForm()){
                var argS = "menubar=0,toolbar=0,height=200,width=450,hotkeys=0,scrollbars=1,resizable=1";
                var popUpWindow = window.open("loading.html", "MailSend", argS);
                setTimeout(SendClickFormSubmit,100);
            }
        }
        
        function SendClickFormSubmit(){
            document.form1.action = "shipper_notice_send.asp?WindowName=MailSend";
            document.form1.method = "POST";
            document.form1.encoding = "application/x-www-form-urlencoded";
            document.form1.target = "MailSend";
            document.form1.submit();
        }
        <% Else %>
        function SendClick(){
            alert("Premium subscription is needed for this feature.");
        }
        <% End If %>
        function ValidateForm(){

            if(document.getElementById("txtYourName").value == ""){
                alert("Please, enter your name.");
                return false;
            }
            if(document.getElementById("txtYourEmail").value == ""){
                alert("Please, enter your name.");
                return false;
            }
            var aOrgCheck = document.getElementsByName("chkOrg");
            var aEmailNames = document.getElementsByName("txtEmailName");
            var aEmailTos = document.getElementsByName("txtEmailTo");
            
            if(aOrgCheck.length == 0){
                alert("No recipients are selected.");
                return false;
            }
            
            for(var i=0; i<aEmailNames.length; i++){
                if(aOrgCheck[i].checked){
                    if(aEmailNames[i].value == ""){
                        alert("Please, enter reciepient's name.");
                        return false;
                    }
                    if(aEmailTos[i].value == ""){
                        alert("Please, enter reciepient's email address.");
                        return false;
                    }
                }
            }
            return true;
        }
        
    </script>

    <script type="text/jscript" src="../include/JPTableDOM.js"></script>

    <script type="text/javascript" src="../include/tooltips.js"></script>

    <script type="text/javascript" language="javascript" src="/ASP/ajaxFunctions/ajax.js"></script>

    <script type="text/javascript" src="../include/JPED.js"></script>

</head>
<body style="margin: 0px 0px 0px 0px;" onload="LoadPage()">
    <!-- tooltip placeholder -->
    <div id="tooltipcontent">
    </div>
    <!-- placeholder ends -->
    <div style="text-align: center">
        <table style="width: 95%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td valign="middle" class="pageheader">
                    Shipping Notice
                </td>
            </tr>
        </table>
    </div>
    <div class="selectarea" style="text-align: center">
        <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td style="height: 15px; width: 200px" valign="top" colspan="2">
                    <span class="select">Select AWB Type and No.</span></td>
            </tr>
            <tr>
                <td style="width: 130px">
                    <select id="lstSearchType" class="bodyheader" style="width: 120px" onchange="javascript:selectSearchType();">
                        <option value="master">Ocean Booking No.</option>
                        <option value="file">File No.</option>
                        <option value="house">House B/L No.</option>
                        
                    </select>
                </td>
                <td>
                    <!-- Start JPED -->
                    <input type="hidden" id="hSearchNum" name="hSearchNum" />
                    <div id="lstSearchNumDiv">
                    </div>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <input type="text" autocomplete="off" id="lstSearchNum" name="lstSearchNum" value=""
                                    class="shorttextfield" style="width: 140px; border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9;
                                    border-left: 1px solid #7F9DB9; border-right: 0px solid #7F9DB9;" onkeyup="searchNumFill(this,'O','lstSearchNumChange',200,event);"
                                    onfocus="initializeJPEDField(this,event);" /></td>
                            <td>
                                <img src="/ig_common/Images/combobox_drop.gif" alt="" onclick="searchNumFillAll('lstSearchNum','O','lstSearchNumChange',200);"
                                    style="border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9; border-right: 1px solid #7F9DB9;
                                    border-left: 0px solid #7F9DB9; cursor: hand;" /></td>
                        </tr>
                    </table>
                    <!-- End JPED -->
                </td>
                <td align="right">
                    <span class="goto">
                        <img src="/ASP/Images/icon_email_history.gif" align="absbottom" alt="" /><a
                            href="javascript:;" onclick="ShowEmailHistory()">View Email History</a></span>
                </td>
            </tr>
        </table>
    </div>
    <form id="form1" name="form1" accept-charset="UTF-8" action="">
        <table align="center" bgcolor="#6D8C80" border="0" bordercolor="#6D8C80" cellpadding="0"
            cellspacing="0" class="border1px" width="95%">
            <tr>
                <td height="100%">
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr bgcolor="#edd3cf">
                            <td align="center" bgcolor="#BFD0C9" class="bodyheader" colspan="6" height="24" valign="middle">
                                <span class="pageheader">
                                    <img height="18" name="bSend" onclick="SendClick()" src="../images/button_send_email.gif"
                                        style="cursor: hand" width="101" /></span></td>
                        </tr>
                        <tr bgcolor="#6D8C80">
                            <td align="left" class="bodyheader" colspan="6" height="1" valign="top">
                            </td>
                        </tr>
                        <tr align="center" bgcolor="#f0e7ef" valign="middle">
                            <td align="center" bgcolor="#eeeeee" class="bodyheader" colspan="6" height="24">
                                <br />
                                <table border="0" cellpadding="0" cellspacing="0" width="65%">
                                    <tr>
                                        <td align="left" valign="middle" width="50%">
                                        </td>
                                        <td align="right" class="bodyheader" height="28" width="50%">
                                            <img align="absBottom" src="/ASP/Images/required.gif" />Required field</td>
                                    </tr>
                                </table>
                                <table bgcolor="#edd3cf" border="0" bordercolor="#6D8C80" cellpadding="2" cellspacing="0"
                                    class="border1px" width="65%">
                                    <tr align="left" bgcolor="#E0EDE8" valign="middle">
                                        <td align="left" class="bodycopy" valign="middle" width="1">
                                            &nbsp;</td>
                                        <td align="left" class="bodyheader" height="20" valign="middle"
                                            width="222">
                                            Your Name</td>
                                        <td align="left" class="bodycopy" valign="middle" width="408">
                                        </td>
                                        <td align="left" class="bodycopy" valign="middle" width="408">
                                        </td>
                                    </tr>
                                    <tr align="left" bgcolor="#f3f3f3" valign="middle">
                                        <td align="left" bgcolor="#ffffff" class="bodycopy" valign="middle">
                                            &nbsp;</td>
                                        <td align="left" bgcolor="#ffffff" class="bodycopy" colspan="3" valign="middle">
                                            <input class="shorttextfield" id="txtYourName" name="txtYourName" size="28" value="<%= vYourName %>" />
                                        </td>
                                    </tr>
                                    <tr align="left" bgcolor="#E0EDE8" valign="middle">
                                        <td align="left" class="bodycopy" height="20" valign="middle">
                                            &nbsp;</td>
                                        <td align="left" class="bodyheader" height="20" valign="middle">
                                            From</td>
                                        <td align="left" class="bodycopy" height="20" valign="middle">
                                            &nbsp;</td>
                                        <td align="left" class="bodycopy" valign="middle">
                                            &nbsp;</td>
                                    </tr>
                                    <tr align="left" bgcolor="#f3f3f3" valign="middle">
                                        <td align="left" bgcolor="#ffffff" class="bodycopy" valign="middle">
                                            &nbsp;</td>
                                        <td align="left" bgcolor="#ffffff" class="bodycopy" colspan="3" valign="middle">
                                            <input class="shorttextfield" id="txtYourEmail" name="txtYourEmail" size="45" type="text"
                                                value="<%= vYourEmail %>" /></td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: #eeeeee" class="bodycopy">
                                <% If Not IsEmpty(aOrgData) Then %>
                                <div style="height: 20px" class="bodyheader">
                                    <% If aOrgData.Count = 0 Then %>
                                    No Data Found.
                                    <% Else %>
                                    Booking Number:
                                    <%=aOrgData(0)("booking_num") %>
                                    <input type="hidden" id="hMAWB" name="hMAWB" value="<%=aOrgData(0)("booking_num") %>" />
                                    <% End If %>
                                </div>
                                <!-------------------------------------- Sorted by Agent List Starts --------------------------------->
                                <% For i = 0 To aOrgData.Count-1 %>
                                <div style="border: solid 1px #cdcdcd; padding: 4px; background-color: #ffffff">
                                    <table class="bodycopy" cellpadding="0" cellspacing="0" border="0" style="width: 90%">
                                        <tr>
                                            <td valign="top" style="width: 35px">
                                                <input type="checkbox" id="chkOrg<%=i %>" name="chkOrg" onclick="chkOrgClick(<%=i %>,this)"
                                                    value="<%=aOrgData(i)("agent_no") %>" checked="checked" /></td>
                                            <td>
                                                <table id="tblOrg<%=i %>" class="bodycopy" cellpadding="0" cellspacing="0" border="0"
                                                    style="width: 100%">
                                                    <tr>
                                                        <td style="width: 160px" class="bodyheader">
                                                            AGENT</td>
                                                        <td>
                                                            <input type="text" class="shorttextfield" size="80" id="txtEmailName<%=i %>" name="txtEmailName"
                                                                value="<%=aOrgData(i)("agent_name") %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="bodyheader">
                                                            <img src="/ASP/Images/required.gif" align="absbottom" alt="" />TO</td>
                                                        <td>
                                                            <input type="text" class="shorttextfield" size="80" id="txtEmailTo<%=i %>" name="txtEmailTo"
                                                                value="<%=aOrgData(i)("owner_email") %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="bodyheader">
                                                            CC</td>
                                                        <td>
                                                            <input type="text" class="shorttextfield" size="80" id="txtEmailCC<%=i %>" name="txtEmailCC"
                                                                value="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="bodyheader">
                                                            SUBJECT</td>
                                                        <td>
                                                            <input type="text" class="shorttextfield" size="80" id="txtEmailSubject<%=i %>" name="txtEmailSubject"
                                                                value="Shipping Notice: <%=vDBA %> Booking No:<%=aOrgData(i)("booking_num") %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="bodyheader" valign="top">
                                                        </td>
                                                        <td>
                                                            
                                                            <ul style="display: inline; float: none;">

                                                                <% If aOrgData(i)("isDirect") <> "Y" Then %>
                                                                <!-- Consolidated houses where Agents are assigned to -->
                                                                
                                                                <% Set aHouseData = DataListGroupBy(DataListSelect(aAllData, "agent_no", aOrgData(i)("agent_no")),"hbol_num") %>
                                                                <% For j = 0 To aHouseData.Count-1 %>
                                                                <li style="display: inline; float: left; list-style: none; width: 200px; overflow: hidden">
                                                                    <input type="checkbox" id="chkHousePDF<%=i %><%=j %>" name="chkHousePDF<%=aOrgData(i)("agent_no") %>"
                                                                        value="<%=aHouseData(j)("hbol_num") %>" checked="checked" />
                                                                    <a href="hbol_pdf.asp?HBOL=<%=aHouseData(j)("hbol_num") %>&Copy=CONSIGNEE">
                                                                        HBOL:
                                                                        <%=aHouseData(j)("hbol_num") %>
                                                                    </a>&nbsp;&nbsp;&nbsp;</li>
                                                                <% If aHouseData(j)("invoice_no") <> "-1" Then %>
                                                                <li style="display: inline; float: left; list-style: none; width: 200px; overflow: hidden">
                                                                    <input type="checkbox" id="chkInvoicePDF<%=i %><%=j %>" name="chkInvoicePDF<%=aOrgData(i)("agent_no") %>"
                                                                        value="<%=aHouseData(j)("invoice_no") %>" checked="checked" />
                                                                    <a href="../acct_tasks/invoice_pdf.asp?InvoiceNo=<%=aHouseData(j)("invoice_no") %>">
                                                                        INV:
                                                                        <%=aHouseData(j)("invoice_no") %>
                                                                    </a>&nbsp;&nbsp;&nbsp;</li>
                                                                <% End If %>
                                                                <% Next %>
                                                                
                                                                <% Elseif aOrgData(i)("invoice_no") <> "-1" Then %>
                                                                <!-- Direct Master with a invoice created -->
                                                                <li style="display: inline; float: left; list-style: none; width: 200px; overflow: hidden">
                                                                    <input type="checkbox" id="chkInvoicePDF<%=i %>" name="chkInvoicePDF<%=aOrgData(i)("agent_no") %>"
                                                                        value="<%=aOrgData(j)("invoice_no") %>" checked="checked" />
                                                                    <a href="../acct_tasks/invoice_pdf.asp?InvoiceNo=<%=aOrgData(j)("invoice_no") %>">
                                                                        INV:
                                                                        <%=aOrgData(j)("invoice_no") %>
                                                                    </a>&nbsp;&nbsp;&nbsp;</li>
                                                                <% End If %>
                                                            </ul>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 4px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="bodyheader">
                                                            ATTACHMENT</td>
                                                        <td>
                                                            <a href="javascript:void(UploadFile('<%=aOrgData(i)("agent_no")%>'));" id="aAttachedFiles<%=aOrgData(i)("agent_no")%>">
                                                                <%=GetUserFileList(aOrgData(i)("agent_no")).Count %>
                                                                Attached File(s) </a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 4px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="bodyheader" valign="top">
                                                            MESSAGE</td>
                                                        <td>
                                                            <textarea cols="100" rows="4" class="multilinetextfield" id="txtMessage<%=i %>" name="txtMessage"><%=aOrgData(i)("MsgTxt") %></textarea>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" align="right">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <% Next %>
                                <!-------------------------------------- Sorted by Agent List Ends --------------------------------->
                                <% End If %>
                            </td>
                        </tr>
                        <tr bgcolor="#6D8C80">
                            <td align="left" class="bodyheader" colspan="6" height="1" valign="top">
                            </td>
                        </tr>
                        <tr bgcolor="#edd3cf">
                            <td align="center" bgcolor="#BFD0C9" class="bodyheader" colspan="6" height="24" valign="middle">
                                <span class="pageheader">
                                    <img height="18" name="bSend" onclick="SendClick()" src="../images/button_send_email.gif"
                                        style="cursor: hand" width="101" /></span></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </form>
</body>
<% Set feData = Nothing %>
<!--  #INCLUDE FILE="../include/StatusFooter.asp" -->
</html>
