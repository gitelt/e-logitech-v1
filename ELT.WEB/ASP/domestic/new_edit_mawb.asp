<!--  #INCLUDE FILE="../include/transaction.txt" -->
<% 
    Response.CharSet = "UTF-8"
    Session.CodePage = "65001"
%>
<!--  #INCLUDE FILE="../include/connection.asp" -->
<!--  #INCLUDE FILE="../include/GOOFY_util_fun.inc" -->
<!--  #INCLUDE FILE="../include/header.asp" -->
<!--  #include file="../include/recent_file.asp" -->
<!--  #INCLUDE FILE="../include/GOOFY_Util_Ver_2.inc" -->
<%

''@DECLARATION''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Dim vDepCode, vArrCode
Dim MinApplied
MinApplied = -1
Dim qMAWB,vMAWB, vMAWBInfo,vHAWB,IVstrMsg,rs3,ErrMSG,AgentName
Dim qShipperName, vShipperInfo, vShipperName,qShipperAcct,vShipperAcct
Dim qConsigneeName, vConsigneeName, vConsigneeInfo,qConsigneeAcct,vConsigneeAcct
Dim qNotify,vWeightScalef
Dim vAgentInfo,vAgentIATACode,vAgentAcct,vDepartureAirport, vTo, vBy, vTo1, vBy1, vTo2, vBy2
Dim vOriginPortID,cMAWB
Dim vDestAirport,vFlightDate1,vFlightDate2
Dim vIssuedBy,vAccountInfo
Dim vCurrency, vChargeCode, vPPO_1, vCOLL_1, vPPO_2, vCOLL_2
Dim vDeclaredValueCarriage, vDeclaredValueCustoms,vInsuranceAMT
Dim vHandlingInfo, vSCI,vSignature,vPlaceExecuted
Dim aMAWB(2000), aMAWBInfo(2)
Dim aTranNo(3),aPiece(3),aGrossWeight(3),aAdjustedWeight(3),aKgLb(3),aRateClass(3),aItemNo(3)
Dim aDimension(3),aDemDetail(3),aChargeableWeight(3),aRateCharge(3),aTotal(3)
Dim aLength(3),aWidth(3),aHeight(3)
Dim vTotalPieces,vTotalGrossWeight,vTotalWeightCharge
Dim vDesc1,vDesc2
Dim vShowWeightChargeShipper,vShowWeightChargeConsignee
Dim aCarrierAgent(10),aCollectPrepaid(10),aChargeCode(10)
Dim aDesc(10),aChargeAmt(10),aVendor(10),aCost(10),aOtherCharge(5)
Dim vShowPrepaidOtherChargeShipper,vShowCollectOtherChargeShipper
Dim vShowPrepaidOtherChargeConsignee,vShowCollectOtherChargeConsignee
Dim cName,cAddress,cCity,cState,cZip,cCountry,cPhone,cIndex
Dim aHAWB(640),sHAWB(640)
Dim aCOLO(640),sCOLO(640)
Dim aShipper(640),sShipper(640),sIsMaster(640)
Dim aConsignee(640),sConsignee(640)
Dim aAgent(640),sAgent(640)
Dim aClass(640),sRateClass(640)
Dim aPCS(640),sPCS(640)
'/////////////// by iMoon 2/13/2007
DIM aWeightTran(640)
Dim aGW(640),sGW(640)
Dim aAW(640),sAW(640)
Dim aCW(640),sCW(640)
Dim aDW(640),sDW(640)
Dim addELTAcct(640),delELTAcct(640)
Dim aAgentName(4096),aAgentInfo(2),aAgentAcct(4096)
Dim aShipperName(4096),aShipperInfo(2),aShipperAcct(4096)
Dim aConsigneeName(4096),aConsigneeInfo(2),aConsigneeAcct(4096)
Dim aNotifyInfo(2),aNotify(4096)
Dim rs,rs1,SQL
Dim aChargeItemNo(256),aChargeItemName(256),aChargeItemDesc(256),aChargeItemNameig(256) 
DIM aChargeUnitPrice(256)
DIM vTotalHAWB,vQueueID
Dim NoHAWB,IsCOLO,tKgLb,aservicelevel
'//////////////////////////////////////////////////////////////////
DIM  sTotalPCS,sTotalGW,sTotalAW,sTotalDW,sTotalCW,GWPercent,AWPercent,DWPercent
DIM vUOM, vDefaultAgentName,vExecute,vExecutionDatePlace
DIM Save,AddOC,AddHAWB,Edit,DeleteOC,AdjustWeight
DIM DeleteMAWB,DeleteHAWB,fBook
DIM wCount,vAirOrgNum,vDefaultAgentInfo,vFFShipperAcct,pos
DIM vNotify,qNotifyName,vFFConsigneeAcct,vNotifyAcct,vDestCountry
DIM AddHAWBNo,vAddELTAcct,dHAWB,vDelELTAcct
DIM NoItemWC,NoItemOC,vTotalChgWT,ChargeItemInfo,dItemNo
DIM oIndex,vPrepaidOtherChargeAgent,vCollectOtherChargeAgent,vPrepaidOtherChargeCarrier
DIM vCollectOtherChargeCarrier,vPrepaidWeightCharge,vCollectWeightCharge,vPrepaidValuationCharge
DIM vCollectValuationCharge,vPrepaidTax,vCollectTax,vConversionRate,vCCCharge,vChargeDestination
DIM vPrepaidTotal,vCollectTotal,vFinalCollect
DIM NewMAWB,chIndex,mIndex,sIndex,aIndex,sCount,aCount,coIndex
Dim aColodeeName(64),aColodeeAcct(64)
Dim mMawbNo,mDepartureAirport, mTo, mBy, mTo1, mBy1, mTo2, mBy2,mDestAirport
Dim mFlight1,mFlight2,mETDDate,mFlightDate1,mFlightDate2,mCarrierDesc,mCount
Dim chkAvailTab,vAirline,vDefault_SalesRep,mIAC	
vDefault_SalesRep=session_user_lname	
Dim vSalesPerson,aSRName(1000),SRIndex,NoConsol
DIM sHsCount,sHsTotalPCS,sHsTotalGW,sHsTotalAW,sHsTotalDW,sHsTotalCW
DIM sHsPCS(640),sHsGW(640),sHsAW(640),sHsDW(640),sHsCW(640),sHsHAWB(640)
DIM sHdelELTAcct(640),sHsCOLO(640),sHsAgent(640),sHsShipper(640),sHsConsignee(640),tmpAgent(100),dict	
NoConsol=false

Dim vFileNo,vReferenceNumber

Dim vIACNum,vKnownShipper,vUnKnownShipper,vItemUnder16,vServiceLevel,vServiceLevel2,vDestAdvChargeDesc
Dim vCODAmount,vPickupCharge,vOriginAdvCharge,vOriginAdvChargeDesc,vDeliveryCharge,vDestAdvCharge
Dim vItemPrepaid,vItemCollect,vCODFee,vOtherCharge


''END OF DECLARATION''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


''@INITIALIZATION'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Set rs = Server.CreateObject("ADODB.Recordset")
Set rs1 = Server.CreateObject("ADODB.Recordset")
Set rs3 = Server.CreateObject("ADODB.Recordset")
Set rs4 = Server.CreateObject("ADODB.Recordset")

Save=Request.QueryString("Save")
AddOC=Request.QueryString("AddOC")
AddHAWB=Request.QueryString("AddHAWB")
Edit=Request.QueryString("Edit")
DeleteOC=Request.QueryString("DeleteOC")
DeleteHAWB=Request.QueryString("DeleteHAWB")
AdjustWeight=Request.QueryString("AdjustWeight")
vMAWB=Request.QueryString("MAWB")
vMAWB=Replace(vMAWB,"?"," ")
DeleteMAWB=Request.QueryString("DeleteMAWB")
fBook=Request.QueryString("fBook")

chkAvailTab = NOT AddHAWB ="no" AND NOT DeleteHAWB ="yes" AND NOT  AddOC = "yes" _
AND NOT DeleteOC = "yes" AND NOT AdjustWeight = "yes" AND NOT Edit ="yes" _
AND NOT SAVE = "yes"

''END OF INITIALIZATION''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

''@MAIN LOGIC''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    
    CALL REDIRECT_URL
    
    eltConn.BeginTrans
    
    CALL GET_SALES_PERSONS_FROM_USERS
    CALL GET_DEFAULT_WEIGHT_SCALE
    CALL MAIN_PROCESS
    CALL GET_faa_CODE 
    '// CALL GET_AGENT_SHIPPER_CONSIGNEE_VENDOR_INFO
    CALL GET_MAWB_NUMBER_FROM_TABLE( vMAWB )
    Call GET_DEP_ARR_CODE	
    CALL GET_CHARGE_ITEM_INFO
    CALL FINAL_SCREEN_PREPARE

    eltConn.CommitTrans

''END OF MAIN LOGIC''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


Sub REDIRECT_URL
    Dim SQL,rs
	Set rs = Server.CreateObject("ADODB.Recordset")
    If Edit = "yes" Then
        SQL = "SELECT master_type,is_inbound FROM mawb_number WHERE elt_account_number=" & elt_account_number _
            & " AND is_dome='Y' AND mawb_no='" & vMAWB & "'"
        
	    rs.CursorLocation = adUseClient	
	    rs.Open SQL,eltConn,adOpenForwardOnly,adLockReadOnly,adCmdText
        Set rs.ActiveConnection = Nothing
        
        If NOT rs.EOF AND NOT rs.BOF Then
            If rs("is_inbound") = "Y" Then
                Response.Write("<script> window.location.href='inbound_alert.asp?mode=edit&MAWB=" & vMAWB & "'; </script>")    
                Response.End()
            Else
                If rs(0).value = "DG" Then
                    Response.Write("<script> window.location.href='new_mawb_ground.asp?mode=edit&MAWB=" & vMAWB & "'; </script>")    
                    Response.End()
                End If
            End If
        Else
            Response.Write("<script> alert('The MAWB was not found!'); window.location.href='new_edit_mawb.asp'; </script>")    
        End If
		rs.close()
    End If
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:MAIN_PROCESS
'Purpose  of the procedure: The procedure is in charge of splitting the tasks related to MAWB, according to the user request to the page
'Group of the tasks that are performed within:				    
'Group 1 Editing/Saving/Deleting MAWB
'Group 2 Invoice Queue related 
'Group 3 Getting the MAWB related information from Screen
'Group 4 Getting the MAWB related information from DB
'Group 5 Getting/Posting/Adding/Removing/Calculating HAWBs to be consolidated
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB MAIN_PROCESS
	IF DeleteMAWB="yes" THEN
		CALL DELETE_MAWB( vMAWB )		
	'// by iMoon 2/21/2007		
	    CALL INVOICE_QUEUE_REFRESH( vMAWB )
		EXIT SUB
	END IF
	IF Save="yes" or AddOC="yes" or AddHAWB="yes" or DeleteOC="yes" or DeleteHAWB="yes" or AdjustWeight="yes" THEN
		CALL GET_MAWB_INFO_FROM_SCREEN
		if AddHAWB="yes" then
			CALL ADD_HAWB_INFO		
		end if	
		IF DeleteHAWB="yes" THEN
			CALL DELETE_HAWB_INFO
		END IF	
		CALL GET_ITEM_WEIGHT_CHARGE_INFO_SCREEN		
		IF AdjustWeight="yes" THEN
			CALL ADJUST_WEIGHT
		END IF
		
		CALL SET_ITEM_WEIGHT_TOTAL_AND_DESC	
		CALL GET_ITEM_OTHER_CHARGE_INFO_SCREEN	
		IF DeleteOC="yes" THEN
			CALL DELETE_OTHER_CHARGE_INFO
		END IF	
		CALL SET_ITEM_OTHER_CHARGE_INFO	
		if Save="yes" then
			CALL UPDATE_MAWB_TABLE
		end if
	ELSE
		IF NOT vMAWB="" THEN	
			CALL GET_MAWB_INFO_FROM_TABLE( vMAWB )			
		ELSE
			vPPO_1="Y"
			vPPO_2="Y"
			vDeclaredValueCarriage="NVD"
			vDeclaredValueCustoms="NCV"
			vInsuranceAMT="XXX"
			vShowWeightChargeShipper="Y"
			vShowWeightChargeConsignee="Y"
			vShowPrepaidOtherChargeShipper="Y"
			vShowCollectOtherChargeShipper="Y"
			vShowPrepaidOtherChargeConsignee="Y"
			vShowCollectOtherChargeConsignee="Y"
		END IF
		CALL SET_DETAULT_OTHER_CHARGE_ITEM_LINE	
	'// Added by Joon on Jan/26/2007 ///////////////////////////////////////////////
		GET_AGENT_GENERAL_INFORMAION
	'///////////////////////////////////////////////////////////////////////////////			
	END IF

	IF vMAWB="" THEN EXIT SUB
	IF not AddOC="yes" and not DeleteOC="yes" then
		CALL RESET_PIECE_WEIGHT
	END IF	
	
	CALL FIND_ALL_COLODEES
	CALL GET_SELECTED_HAWB_INFO
	CALL GET_AVAIL_HAWB	
	CALL RESET_WCOUNT'------------------wCount=0
	
	if not AddHAWB="yes" and not DeleteHAWB="yes" and not AdjustWeight="yes" and not AddOC="yes" and not DeleteOC="yes" then
		CALL GET_MAWB_WEIGHT_CHARGE_INFO_FROM_TABLE	
	end if
	if wCount=0 and not AddOC="yes" and not DeleteOC="yes" then
		CALL GET_DEP_ARR_CODE	
		CALL RECALC_ITEM_RATE_CHARGE	
	end if
	if AddHAWB="yes" or DeleteHAWB="yes" or AdjustWeight="yes" or Edit="yes" then
		CALL RECALC_ITEM_TOTAL  
	end if
	
	'//vAirline=cInt(Mid(vMAWB,1,3))
	vAirline = GetCarrierCode(vMAWB)
END SUB


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE: CHECK_SHOULD_INVOICE_QUEUED
'Purpose  of the procedure: The procedure is in charge of finding out whether the given HAWB should be
'create a entry into invoice queue or not.
'Tasks that are performed within:				    
'1.Check if the invoice entry should be created or not
'  For regular house airway bill it should be created no matter what, but for sub or master house it only
'  creates a invoice queue entry only when the house is set to create invoice queue entry.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function CHECK_SHOULD_INVOICE_QUEUED(hawb)
    dim rs,SQL
	Set rs = Server.CreateObject("ADODB.Recordset")
	SQL= "select isnull(is_sub,'N') as is_sub,isnull(is_master,'N') as is_master ,isnull(is_invoice_queued,'Y') as is_invoice_queued from hawb_master where elt_account_number = "& elt_account_number &" and is_dome='Y' And mawb_num='"&vMAWB&"' And hawb_num='"& hawb &"'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	If Not rs.EOF Then	
	    if rs("is_sub")="Y" OR  rs("is_master")="Y" then		 
			if  rs("is_invoice_queued") ="N" then
				CHECK_SHOULD_INVOICE_QUEUED=false
			else 
				CHECK_SHOULD_INVOICE_QUEUED=true
			end if 
		else 
			CHECK_SHOULD_INVOICE_QUEUED=true
		end if
	END IF 
	rs.close
	set rs=Nothing 
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_DEP_ARR_CODE
'Purpose  of the procedure: The procedure is in charge of retrieving the airport codes for departure
'and arrival
'Tasks that are performed within:									    
'1.find and store departure and arrival airport code in the variables
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Sub GET_DEP_ARR_CODE    
	SQL= "select Origin_Port_ID,Dest_Port_ID from mawb_number a where elt_account_number = " & elt_account_number & " and is_dome='Y' And mawb_no='"&vMAWB&"'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing

	If Not rs.EOF Then
	    vDepCode=rs("Origin_Port_ID")
        vArrCode =rs("Dest_Port_ID")
	 
	END IF 
	rs.close
End Sub 

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub GET_faa_CODE    
	SQL= "select faa_approval_no from Agent a where elt_account_number = '" & elt_account_number & "'"
	rs4.CursorLocation = adUseClient
	rs4.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs4.activeConnection = Nothing

	If Not rs4.EOF Then
	    mIAC=rs4("faa_approval_no")
	END IF 
	rs4.close

End Sub 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_SALES_PERSONS_FROM_USERS
'Purpose  of the procedure: The procedure is in charge of retrieving the list of salse persons to be 
'used.
'Tasks that are performed within:									    
'1.retrieve sales person from DB
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB GET_SALES_PERSONS_FROM_USERS

   SQL= "select code from all_code where elt_account_number = " & elt_account_number & " and type=22 order by code"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing

    SRIndex=0
	    do While Not rs.EOF
		    aSRName(SRIndex)=rs("code")	
		    rs.MoveNext
		    SRIndex=SRIndex+1
	    loop
	    rs.Close
	    
END SUB 

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:EXECUTION_STRING_CHANGE
'Purpose  of the procedure: The procedure is in charge of 
'Tasks that are performed within:									    
'1.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub EXECUTION_STRING_CHANGE
    Dim txtPos
    txtPos = InStr(UCase(vExecute),"AS AGENT OF")
    If txtPos>0 Then
		If InStr(vExecute,"CARRIER") = 0 Then
			vExecute = Left(vExecute, txtPos) & Replace(vExecute, chr(13), ", CARRIER" & chr(13), txtPos + 1, 1)
		End If
    End If
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:RESET_WCOUNT
'Purpose  of the procedure: The procedure is in charge of resetting the weight charge count on the screen
'Tasks that are performed within:									    
'1.set wCount to be 0
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB RESET_WCOUNT
	wCount = 0
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:get_mawb_info
'Purpose  of the procedure: The procedure is in charge of getting data for a MAWB from DB through a ajax procedure
'Tasks that are performed within:									    
'1.Return a MAWB information in a string 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function  get_mawb_info( MAWB )
	if MAWB = "" then Exit function
%>
<!--  #INCLUDE VIRTUAL="/ASP/ajaxFunctions/mawb_number_info.inc" -->
<%
	get_mawb_info =  mawbInfo
End Function

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GETVEXECUTE
'Purpose  of the procedure: The procedure is in charge of 
'Tasks that are performed within:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GETVEXECUTE(vMAWB)
	Dim mInfo
	Dim mDepartureAirportCode,mDepartureAirport, mTo, mBy, mTo1, mBy1, mTo2, mBy2,mDestAirport
	Dim mFlightDate1,mFlightDate2,IssuedBy,mServiceLevel,mFile
	
	if trim(vMAWB) = "" then
		vExecute=vExecutionDatePlace
		exit sub
	end if	
	mInfo = get_mawb_info( vMAWB )	
	if isnull(mInfo) then
		exit sub
	else
	end if

	pos=InStr(mInfo,chr(10))
	mAirOrgNum=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mDepartureAirportCode=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mDepartureAirport=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mTo=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mBy=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mTo1=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mBy1=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mTo2=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mBy2=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mDestAirport=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mFlightDate1=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mFlightDate2=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mCarrierDesc=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mExportDate=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mDestCountry=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	pos=InStr(mInfo,chr(10))
	mDepartureState=Left(mInfo,pos-1)
    mInfo=Mid(mInfo,pos+1,1000)
    pos=InStr(mInfo,chr(10))
    mFile=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
    pos=InStr(mInfo,chr(10))
    mServiceLevel=Left(mInfo,pos-1)
	mInfo=Mid(mInfo,pos+1,1000)
	
	IssuedBy = vAgentInfo
	pos=Instr(IssuedBy,chr(10))
	If pos>0 Then
		IssuedBy=Left(IssuedBy,pos-1)
	End If
	
	If Not IsNull(vExecute) And Trim(vExecute) <> "" Then
		vExecute = "AS AGENT OF " & mCarrierDesc & ", CARRIER" & chr(10) & vExecutionDatePlace
	Else
		vExecute = IssuedBy & chr(10) & mExportDate & " " & vExecute
	End If
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:SET_ITEM_WEIGHT_TOTAL_AND_DESC
'Purpose  of the procedure: The procedure is in charge of summing up and setting weight charge total 
'							information to the variables that will be displayed on the screen
'Tasks that are performed within:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB SET_ITEM_WEIGHT_TOTAL_AND_DESC
	vTotalPieces=0
	vTotalGrossWeight=0
	vTotalWeightCharge=0	
	for i=0 to NoItemWC-1
		vTotalPieces=vTotalPieces+aPiece(i)
		vTotalGrossWeight=vTotalGrossWeight+aGrossWeight(i)
		vTotalWeightCharge=vTotalWeightCharge+aTotal(i)
	next
	vDesc1=Request("txtDesc1")
	vDesc2=Request("txtDesc2")
	vPrepaidOtherChargeAgent=0
	vCollectOtherChargeAgent=0
	vPrepaidOtherChargeCarrier=0
	vCollectOtherChargeCarrier=0
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:SET_DETAULT_OTHER_CHARGE_ITEM_LINE
'Purpose  of the procedure: The procedure is in charge of merging two lines of other charge descriptions to one 
'							when there are more than 5 lines due to the printing page limit.
'Tasks that are performed within:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB SET_DETAULT_OTHER_CHARGE_ITEM_LINE
	if oIndex="" then oIndex=2
	NoItemOC=oIndex	
	if NoItemOC>5 then
		for i=0 to NoItemOC-1 Step 2
			aOtherCharge(Fix(i/2))=aDesc(i) & " " & FormatNumber(aChargeAmt(i),2) & "  " & aDesc(i+1) & " " & FormatNumber(aChargeAmt(i+1),2)
		next
	else
		for i=0 to NoItemOC-1
			aOtherCharge(i)=aDesc(i) & " " & FormatNumber(aChargeAmt(i),2)
		next
	end if
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_DEFAULT_WEIGHT_SCALE
'Purpose  of the procedure: The procedure is in charge of finding out weight scale that the user uses
'                           from the user profile
'Tasks that are performed within:
'1)Set the first entry of aKgLb to be the one in user profile
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_DEFAULT_WEIGHT_SCALE
	SQL= "select uom from user_profile where elt_account_number = " & elt_account_number
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	if not rs.EOF then
		vUOM=rs("uom")
		if vUOM="KG" then
			aKgLb(0)="K"
		else
			aKgLb(0)="L"
		end if
	end if
	rs.Close
END SUB
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_AGENT_GENERAL_INFORMAION
'Purpose  of the procedure: The procedure is in charge of finding out the address information of the agent 
'							in order to feed in to the screen
'Tasks that are performed within:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_AGENT_GENERAL_INFORMAION
	SQL= "select faa_approval_no,dba_name,business_address,business_city,business_state,business_zip,business_country,business_phone,agent_IATA_Code from agent where elt_account_number = " & elt_account_number
	'response.write SQL
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	If Not rs.EOF Then
		vAgentIATACode = rs("Agent_IATA_Code")		
		AgentName = rs("dba_name")
		vDefaultAgentName=AgentName
		AgentAddress=rs("business_address")
		AgentCity = rs("business_city")
		AgentState = rs("business_state")
		AgentZip = rs("business_zip")
		AgentCountry = rs("business_country")
		AgentPhone=rs("business_phone")

		vAgentInfo=AgentName & chr(10) & AgentAddress & chr(10) & AgentCity & "," & AgentState & " " & AgentZip & "," & AgentCountry	
		If checkBlank(vShipperAcct,"") = "" Then
			vShipperAcct=elt_account_number
			vShipperInfo=AgentName & chr(10) & AgentAddress & chr(10) & AgentCity & "," & AgentState & " " & AgentZip & "," & AgentCountry & chr(13) & AgentPhone
		End If    
		vDefaultAgentInfo=vAgentInfo
		vPlaceExecuted=AgentCity & "," & AgentState & " " & AgentZip & " " & AgentCountry
		If IsNull(vExecute) Or Trim(vExecute) = "" Or fBook = "yes" Then
			vExecute=AgentName & chr(10) & Date & " " & vPlaceExecuted
		End If
		vSignature="FOR " & AgentName
		vExecutionDatePlace=Date & " " & vPlaceExecuted
		aShipperName(1)=AgentName
		aShipperInfo(1)=elt_account_number & "-" & AgentName & chr(10) & AgentAddress & chr(10) & AgentCity & "," & AgentState & " " & AgentZip & "," & AgentCountry & chr(10) & AgentPhone
		aShipperAcct(1)=elt_account_number	
	End If
	rs.Close
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:DELETE_MAWB
'Purpose  of the procedure: The procedure is in charge of deleting the MAWB from DB when requested
'							
'Tasks that are performed within:
'1)Delete a MAWB from DB
'2)Delete all MAWB weight charge entry from DB
'3)Delete all MAWB other charge entry from DB
'4)Update all HAWBs MAWB information in the DB to be empty since the MAWB is deleted
'5)Reset the status of MAWB number to be usalbe ="N"
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB DELETE_MAWB( vMAWB )
	SQL= "select mawb_num from mawb_master where elt_account_number = " & elt_account_number & " and is_dome='Y' and mawb_num='" & vMAWB & "'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	If Not rs.EOF Then
		rs.close
		SQL= "delete from mawb_master where elt_account_number = " & elt_account_number & " and is_dome='Y' and mawb_num='" & vMAWB & "'"
		eltConn.Execute SQL
		SQL= "delete from mawb_weight_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "'"
		eltConn.Execute SQL
		SQL= "delete from mawb_other_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "'"
		eltConn.Execute SQL
		SQL= "update hawb_master set mawb_num = '' where elt_account_number = " & elt_account_number & " and is_dome='Y' and mawb_num='" & vMAWB & "'"
		eltConn.Execute SQL
		SQL= "update mawb_number set status='B',used = 'N' where elt_account_number = " & elt_account_number & " and is_dome='Y' and mawb_no='" & vMAWB & "'"
		eltConn.Execute SQL
	else
		rs.close
%>
<script language='JavaScript'>    alert('Could not find the MAWB'); location.href = 'new_edit_mawb.asp'; </script>
<%
	end if	
END SUB


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_SUB_HAWB_INFO
'Purpose  of the procedure: The procedure is in charge of getting all the Sub Houses that belong to a
'							HAWB of type Master House in order to show them on the screen below the HAWB						
'Tasks that are performed within:
'1)Get all the sub houses that belong to one HAWB consolidated in the MAWB
'2)Sumup all the charge information from the HAWBs to show on the screen
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_SUB_HAWB_INFO(subToNo)
    DIM rs
	Set rs = Server.CreateObject("ADODB.Recordset")
	SQL="select a.is_master, a.elt_account_number,a.hawb_num,a.agent_name,a.Shipper_Name,a.Consignee_Name, "
	SQL=SQL & " b.tran_no,b.rate_class,b.no_pieces,b.gross_weight,b.adjusted_weight,b.kg_lb,b.dimension,b.chargeable_weight from hawb_master a LEFT OUTER JOIN hawb_weight_charge b "
	SQL=SQL & " on (a.elt_account_number=b.elt_account_number) and (a.hawb_num =b.hawb_num) where "
	SQL=SQL & " (a.elt_account_number = " & elt_account_number & " or a.coloder_elt_acct=" & elt_account_number
	SQL=SQL & ") and isnull(a.is_sub,'N')='Y' and a.MAWB_NUM = '" & vMAWB &"' and a.sub_to_no = '" & subToNo& "' and a.is_dome='Y' order by a.hawb_num,b.tran_no"
	
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	sHsCount=0	
	Do While Not rs.EOF 
		If IsNull(rs("tran_no")) = False Then
			tran_no = CInt(rs("tran_no"))
		Else
			tran_no=0
		end if
		if tran_no>0 then
			If IsNull(rs("no_pieces")) = False Then
					sHsPCS(sHsCount) = cLng(rs("no_pieces"))
			End If
			If IsNull(rs("gross_weight")) = False Then
				sHsGW(sHsCount) = Cdbl(rs("gross_weight"))
			Else
				sHsGW(sHsCount)=0
			end if
			If IsNull(rs("adjusted_weight")) = False Then
				sHsAW(sHsCount) = Cdbl(rs("adjusted_weight"))
			Else
				sHsAW(sHsCount)=0
			end if
			If IsNull(rs("dimension")) = False Then
				sHsDW(sHsCount) = Cdbl(rs("dimension"))
			End If	
			If IsNull(rs("Chargeable_Weight")) = False Then
				sHsCW(sHsCount) = Cdbl(rs("Chargeable_Weight"))
			Else
				sHsCW(sHsCount)=0
			end if
			tKgLb=rs("kg_lb")
			if not tKgLb = aKgLb(0) then			
				if aKgLb(0)="K" then
					sHsGW(sHsCount)=FormatNumber(sHsGW(sHsCount)/2.20462262,2)
					sHsAW(sHsCount)=FormatNumber(sHsAW(sHsCount)/2.20462262,2)
					sHsCW(sHsCount)=FormatNumber(sHsCW(sHsCount)/2.20462262,2)
					sHsDW(sHsCount)=FormatNumber(sHsDW(sHsCount)/2.20462262,2)
				else
					sHsGW(sHsCount)=FormatNumber(sHsGW(sHsCount)*2.20462262,2)
					sHsAW(sHsCount)=FormatNumber(sHsAW(sHsCount)*2.20462262,2)
					sHsCW(sHsCount)=FormatNumber(sHsCW(sHsCount)*2.20462262,2)
					sHsDW(sHsCount)=FormatNumber(sHsDW(sHsCount)*2.20462262,2)
				end if
			end if
		end if
		if tran_no=0 or tran_no=1 then
			If IsNull(rs("hawb_num")) = False Then
				sHsHAWB(sHsCount)=rs("hawb_num")
				sHdelELTAcct(sHsCount)=rs("elt_account_number")
			End If
			tmpAcct=cLng(rs("elt_account_number"))			
			if Not tmpAcct=elt_account_number then
				xx=0
				Do while xx<coIndex
					if aColodeeAcct(xx)=tmpAcct then
						sHsCOLO(sHsCount)=aColodeeName(xx)
						exit do
					end if
					xx=xx+1
				loop
			end if
			If IsNull(rs("agent_name")) = False Then
				sHsAgent(sHsCount) = rs("agent_name")
				pos=Instr(sHsAgent(sHsCount),chr(10))
				if pos>0 then sHsAgent(sHsCount)=Mid(sHsAgent(sHsCount),1,pos-1)
			End If	
			sHsShipper(sHsCount) = rs("Shipper_name")
			sHsConsignee(sHsCount) = rs("Consignee_name")
		end if
		sHsTotalPCS=sHsTotalPCS+sHsPCS(sHsCount)
		sHsTotalGW=sHsTotalGW+cDbl(sHsGW(sHsCount))
		sHsTotalAW=sHsTotalAW+cDbl(sHsAW(sHsCount))
		sHsTotalDW=sHsTotalDW+cDbl(sHsDW(sHsCount))
		sHsTotalCW=sHsTotalCW+cDbl(sHsCW(sHsCount))		
		sHsCount=sHsCount+1
		rs.MoveNext
	Loop
	rs.Close
	set rs = nothing
END SUB


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:UPDATE_MAWB_TABLE
'Purpose  of the procedure: The procedure is in charge of creating/updating information of MAWB to the DB
'							HAWB of type Master House in order to show them on the screen below the HAWB						
'Tasks that are performed within:
'1)Create/Update all the field into DB
'2)Remove and resave all the weight charge/ Other charge items from screen to DB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB UPDATE_MAWB_TABLE
	DIM rs
	Set rs = Server.CreateObject("ADODB.Recordset")
	'// CALL INVOICE_QUEUE_REFRESH( vMAWB )  ///////////// disable invoicing
	'// CALL MAWB_INVOICE_QUEUE( vMAWB )	 ///////////// disable invoicing
	SQL= "select * from mawb_master where elt_account_number = " & elt_account_number & " and is_dome='Y' and MAWB_NUM = '" & vMAWB & "'"	
	rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
	If rs.EOF=true Then
		rs.AddNew
		rs("elt_account_number")=elt_account_number
		rs("MAWB_NUM")=vMAWB
		rs("date_executed")=Now
		'-----------------------------------------------------------------for the new record
		rs("CreatedBy")=session_user_lname	
		rs("CreatedDate")=Now
		rs("SalesPerson")=vSalesPerson	
		'-----------------------------------------------------------------------------------
		rs("is_dome")="Y"
		//change by stanley on 8/29/2007
		rs("master_type")="DA"
	End If		
	rs("DEP_AIRPORT_CODE") = vOriginPortID
	'// rs("master_agent")=vConsigneeAcct
	rs("airline_vendor_num")=vAirOrgNum
	rs("Shipper_Name") = vShipperName
	rs("Shipper_Info") = vShipperInfo
	rs("Shipper_Account_Number") = vShipperAcct
	rs("ff_shipper_acct") = vFFShipperAcct
	rs("Consignee_Name") = vConsigneeName
	rs("Consignee_Info") = vConsigneeInfo
	rs("Consignee_acct_num") = vConsigneeAcct
	rs("ff_consignee_acct") = vFFConsigneeAcct
	rs("Issue_Carrier_Agent") = vAgentInfo
	rs("Agent_IATA_Code") = vAgentIATACode
	
	rs("Notify_No") = ConvertAnyValue(vNotifyAcct,"Long",0)
	rs("Account_No") = vAgentAcct 
	rs("Departure_Airport") = vDepartureAirport
	rs("To_1") = vTo
	if vBy = "Select One" then vBy = ""
	rs("By_1") = vBy
	rs("To_2") = vTo1
	rs("By_2") = vBy1
	rs("To_3") = vTo2
	rs("By_3") = vBy2
	rs("Dest_Airport") = vDestAirport
	rs("Flight_Date_1") = vFlightDate1
	rs("Flight_Date_2") = vFlightDate2
	rs("IssuedBy")=vIssuedBy
	rs("Account_Info") = vAccountInfo
	rs("Currency") = vCurrency
	rs("Charge_Code") = vChargeCode
	rs("PPO_1") = vPPO_1
	rs("COLL_1") = vCOLL_1
	rs("PPO_2") = vPPO_2
	rs("COLL_2") = vCOLL_2
	rs("Declared_Value_Carriage") = vDeclaredValueCarriage
	rs("Declared_Value_Customs")= vDeclaredValueCustoms
	rs("Insurance_AMT")=vInsuranceAMT
	rs("Handling_Info")=vHandlingInfo
	rs("dest_country")=vDestCountry
	rs("SCI")=vSCI
	rs("total_pieces")=vTotalPieces
	rs("total_gross_weight")=vTotalGrossWeight
	rs("total_chargeable_weight")=vTotalChgWT
	rs("total_weight_charge_hawb")=vTotalWeightCharge
	rs("desc1")=vDesc1
	rs("desc2")=vDesc2
	rs("Weight_Scale")=vWeightScale
	rs("Prepaid_Weight_Charge") = vPrepaidWeightCharge
	rs("Collect_Weight_Charge") = vCollectWeightCharge
	rs("Prepaid_Due_Agent") = vPrepaidOtherChargeAgent
	rs("Collect_Due_Agent") = vCollectOtherChargeAgent
	rs("Prepaid_Due_Carrier") = vPrepaidOtherChargeCarrier
	rs("Collect_Due_Carrier") = vCollectOtherChargeCarrier
	rs("Prepaid_Total")=vPrepaidTotal
	rs("Collect_Total")=vCollectTotal
	rs("Prepaid_Valuation_Charge")=vPrepaidValuationCharge
	rs("Collect_Valuation_Charge")=vCollectValuationCharge
	rs("Prepaid_Tax")=vPrepaidTax
	rs("Collect_Tax")=vCollectTax
	rs("Currency_Conv_Rate")=vConversionRate
	rs("CC_Charge_Dest_Rate")=vCCCharge
	rs("Charge_at_Dest")=vChargeDestination
	rs("Total_Collect_Charge")=vFinalCollect	
	rs("show_weight_charge_shipper")="Y"
	rs("show_weight_charge_consignee")="Y"
	rs("show_prepaid_other_charge_shipper")="Y"
	rs("show_collect_other_charge_shipper")="Y"
	rs("show_prepaid_other_charge_consignee")="Y"
	rs("show_collect_other_charge_consignee")="Y"		
	rs("Signature")=vSignature
	rs("Date_Last_Modified")=Now
	rs("Execution")=vExecute
	rs("SalesPerson")=	vSalesPerson	
	rs("ModifiedBy")= session_user_lname
	rs("ModifiedDate")=Now	
	rs("reference_number") = vReferenceNumber
	
    rs("iac_num") = vIACNum
    rs("known_shipper") = vKnownShipper
    rs("unknown_shipper") = vUnKnownShipper
    rs("item_under_16") = vItemUnder16
    rs("service_level") = vServiceLevel
    rs("cod_amount") = vCODAmount
    rs("pickup_charge") = vPickupCharge
    rs("origin_adv_charge") = vOriginAdvCharge
    rs("origin_adv_charge_desc") = vOriginAdvChargeDesc
    rs("item_prepaid") = FormatNumberPlus(vItemPrepaid,2)
    rs("item_collect") = FormatNumberPlus(vItemCollect,2)
    rs("cod_fee") = vCODFee
    rs("other_charge") = vOtherCharge
    rs("delivery_charge") = vDeliveryCharge
    rs("dest_adv_charge") = vDestAdvCharge
    rs("dest_adv_charge_desc") = vDestAdvChargeDesc
        
	rs.Update
	rs.Close	
	
	SQL= "select used from mawb_number where elt_account_number = " & elt_account_number & " and is_dome='Y' and MAWB_No = '" & vMAWB & "'"
	rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
	If Not rs.EOF Then
		rs("used")="Y"
		rs.Update
	End If
	rs.Close
	SQL= "delete from mawb_weight_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "'"
	eltConn.Execute SQL
	for i=0 to NoItemWC-1
		SQL= "select * from mawb_weight_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "' and tran_no=" & i+1
		rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
		rs.AddNew
		rs("elt_account_number")=elt_account_number
		rs("mawb_num")=vMAWB
		rs("tran_no")=i+1
		rs("no_pieces")=aPiece(i)
		rs("gross_weight")=aGrossWeight(i)
		rs("kg_lb")=aKgLb(i)
		rs("rate_class")=aRateClass(i)
		rs("commodity_item_no")=aItemNo(i)
		rs("chargeable_weight")=Round(aChargeableWeight(i),0)
		rs("rate_charge")=aRateCharge(i)
		rs("total_charge")=Round(aTotal(i),2)		
		rs.Update
		rs.Close
	next	
	SQL= "delete from mawb_other_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "'"
	eltConn.Execute SQL
	for i=0 to NoItemOC-1
		SQL= "select * from mawb_other_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "' and tran_no=" & i+1
		rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
		rs.AddNew
		rs("elt_account_number")=elt_account_number
		rs("mawb_num")=vMAWB
		rs("tran_no")=i+1
		rs("coll_prepaid")=aCollectPrepaid(i)
		rs("carrier_agent")=aCarrierAgent(i)
		rs("charge_code")=aChargeCode(i)
		rs("charge_desc")=aDesc(i)
		rs("amt_mawb")=aChargeAmt(i)
		rs.Update
		rs.Close
	next
	Set rs = nothing
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_DEFAULT_SALES_PERSON_FROM_DB
'Purpose  of the procedure: The procedure is in charge of getting a Default Sales person that will be 
'filled in the screen
'Tasks that are performed within:									    
'1.Retrieve the Default sales person for the organization
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB GET_DEFAULT_SALES_PERSON_FROM_DB
On Error Resume Next:
  if isnull(vShipperAcct) or vShipperAcct = 0 then
   vSalesPerson ="" 
  else 
   SQL= "select SalesPerson from organization where elt_account_number = "& elt_account_number &" and org_account_number = "& vShipperAcct
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
       if not rs.EOF then	
         vSalesPerson = rs("SalesPerson")
       else vSalesPerson ="" 
       end if   
   rs.close
 end if 
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_MAWB_INFO_FROM_SCREEN
'Purpose  of the procedure: The procedure is in charge of getting all the MAWB information from the screen
'Tasks that are performed within:									    
'1.Retrieve all the information from the screen and store them into variables
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_MAWB_INFO_FROM_SCREEN
	vMAWB=Request("hmawb_num")
	vAirOrgNum=Request("hAirOrgNum")
	if vAirOrgNum="" then vAirOrgNum=0
	vDefaultAgentName=Request("hDefaultAgentName")
	vDefaultAgentInfo=Request("hDefaultAgentInfo")
	qShipperName=Request("txtShipperName")
	vShipperInfo=Trim(Request("txtShipperInfo"))
    vSalesPerson=Request("lstSalesRP")
    if vSalesPerson = "none" then
       Call GET_DEFAULT_SALES_PERSON_FROM_DB
    end if 	
	pos=0
	pos=instr(vShipperInfo,chr(13))
	if pos>0 then
		vShipperName=Mid(vShipperInfo,1,pos-1)
	else
		vShipperName=vShipperInfo
	end if
	vShipperAcct=Request("hShipperAcct")
	if not vShipperAcct="" then 
		vShipperAcct=cLng(vShipperAcct)
	else
		vShipperAcct=0
	end if
	vFFShipperAcct=Request.Form("txtShipperAcct").Item(1)
	qConsigneeName=Request("txtConsigneeName")
	vConsigneeInfo=Request("txtConsigneeInfo")
	pos=0
	pos=instr(vConsigneeInfo,chr(13))
	if pos>0 then
		vConsigneeName=Mid(vConsigneeInfo,1,pos-1)
	else
		vConsigneeName=vConsigneeInfo
	end if
	vNotify=Request("lstNotifyName")
	qNotifyName=Request("txtNotify")
	vConsigneeAcct=Request("hConsigneeAcct")
	if not vConsigneeAcct="" then
		vConsigneeAcct=cLng(vConsigneeAcct)
	else
		vConsigneeAcct=0
	end if
	vFFConsigneeAcct=Request("txtConsigneeAcct")	
	vNotifyAcct=Request("hNotifyAcct")
	vAgentInfo=Request("txtAgentInfo")
	vAgentIATACode=Request("txtAgentIATACode")
	vAgentAcct=Request("txtAgentAcct")
	vOriginPortID=Request("hOriginPortID")
	vDepartureAirport = Request("txtDepartureAirport")
	vAccountInfo=Request("txtBillToInfo")
	vTo=Request("txtTo")
	vBy=Request("txtBy")
	vTo1=Request("txtTo1")
	vBy1=Request("txtBy1")
	vTo2=Request("txtTo2")
	vBy2=Request("txtBy2")
	vDestAirport=Request("txtDestAirport")
	vFlightDate1=Request("txtFlightDate1")
	vFlightDate2=Request("txtFlightDate2")
	vIssuedBy=Request("txtIssuedBy")
	vCurrency=Request("lstCurrency")
	vChargeCode=Request("txtChargeCode")
	vChargeCode=Request("txtChargeCode")	
	vPPO_1 = Request("cPPO1")
	vCOLL_1 = Request("cCOLL1")
	vPPO_2 = Request("cPPO2")
	vCOLL_2 = Request("cCOLL2")
	vDeclaredValueCarriage=Request("txtDeclaredValueCarriage")	
	vDeclaredValueCustoms=Request("txtDeclaredValueCustoms")
	vInsuranceAMT=Request("txtInsuranceAMT")	
	vHandlingInfo=Request("txtHandlingInfo")
	vDestCountry=Request("txtDestCountry")
	vSCI=Request("txtSCI")
	vSignature=Request("txtSignature")
	vExecute=Request("txtExecute")
	vReferenceNumber = Request("txtReferenceNumber")
	
	vIACNum = Request("txtIACNum")
    vKnownShipper = Request("txtKnownShipper")
    vUnKnownShipper = Request("txtUnKnownShipper")
    vItemUnder16 = Request("txtItemUnder16")
    vServiceLevel = Request("txtServiceLevel")
    vCODAmount = Request("txtCODAmount")
	if vCODAmount = "" then
	vCODAmount="0"
	end if
    vPickupCharge = Request("txtPickupCharge")
	If vPickupCharge = "" then
	 vPickupCharge="0"
	 end if
    vOriginAdvCharge = Request("txtOriginAdvCharge")
	If vOriginAdvCharge = "" then
	 vOriginAdvCharge="0"
	 end if	
    vOriginAdvChargeDesc = Request("txtOriginAdvChargeDesc")
	If vOriginAdvChargeDesc = "" then
	 vOriginAdvChargeDesc="0"
	 end if
    vItemPrepaid = Request("txtItemPrepaid")
    vItemCollect = Request("txtItemCollect")
    vCODFee = Request("txtCODFee")
	If vCODFee = "" then
	 vCODFee="0"
	 end if
    vOtherCharge = Request("txtOtherCharge")
		If vOtherCharge = "" then
	 vOtherCharge="0"
	 end if
    vDeliveryCharge = Request("txtDeliveryCharge")
	If vDeliveryCharge = "" then
	 vDeliveryCharge="0"
	 end if
    vDestAdvCharge = Request("txtDestAdvCharge")
	If vDestAdvCharge = "" then
	 vDestAdvCharge="0"
	 end if
    vDestAdvChargeDesc = Request("txtDestAdvChargeDesc")
	
END SUB



'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GetFileNo
'Purpose  of the procedure: The procedure is in charge of file number that is assigned to this MAWB from DB
'Tasks that are performed within:									    
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub GetFileNo()
    Dim tempNo,pos,temSLevel
    tempNo = ""
	SQL= "select file# from mawb_number where elt_account_number = " & elt_account_number & " and is_dome='Y' and mawb_no = '" & vMAWB & "'"	
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	If Not rs.EOF Then
        tempNo = rs("file#").value
	End if
	rs.Close
	vFileNo = tempNo
    
    If IsNull(vAccountInfo) Or Trim(vAccountInfo) = "" Then
        vAccountInfo = "FILE# " & tempNo & chr(13) & vAccountInfo
    Else      
	    vAccountInfo = Replace(vAccountInfo,chr(10),"")
	    vAccountInfo = Trim(vAccountInfo)
        Do While InStr(vAccountInfo,chr(13)) = 1
	        vAccountInfo = Replace(vAccountInfo,chr(13),"",1,1)	       
	    Loop	    
	    Do While InStrRev(vAccountInfo,chr(13)) = Len(vAccountInfo) And Trim(vAccountInfo) <> ""
	        vAccountInfo = Replace(vAccountInfo,chr(13),"",Len(vAccountInfo),1)	        
	    Loop	    
	    If Instr(vAccountInfo, tempNo) > 0 And Instr(UCase(vAccountInfo), "FILE#") = 0 Then
	        vAccountInfo =  Replace(vAccountInfo,tempNo, "FILE# " & tempNo)
	    Elseif Instr(vAccountInfo, tempNo) = 0 Then
	        vAccountInfo = "FILE# " & tempNo & chr(13) & vAccountInfo
	    End If
	End If
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GetFileNumber
'Purpose  of the procedure: The procedure is in charge of file number that is assigned to this MAWB from DB
'Tasks that are performed within:									    
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Function GetFileNumber(mawb_num)
    Dim resVal,rs,SQL
    Set rs = Server.CreateObject("ADODB.Recordset")
    SQL = "SELECT File# from mawb_number where elt_account_number=" & elt_account_number _
        & " and is_dome='Y' AND mawb_no='" & mawb_num & "'"
    resVal = ""    
    Set rs = eltConn.execute (SQL)

    If Not rs.EOF And Not rs.BOF Then
        resVal = rs("File#").value
    End If        
    GetFileNumber = resVal
End Function


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:IsModified
'Purpose  of the procedure: The procedure is in charge of checking out where the document has been modifed 
'since last modified date
'Tasks that are performed within:									    
'1.check if the document is modifed if it is it returns true, and returns false otherwise
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Function IsModified()
    Dim resVal
    resVal = false
    SQL = "select CreatedDate,ModifiedDate from mawb_master where elt_account_number = " _
        & elt_account_number & " and is_dome='Y' and mawb_num = '" & vMAWB & "'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	If Not rs.EOF Then
		If rs("CreatedDate").value <> rs("ModifiedDate").value Then
		    resVal = true
		End If
	End If
    IsModified = resVal
    rs.close()
End Function
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_MAWB_INFO_FROM_TABLE
'Purpose  of the procedure: The procedure is in charge of retriving MAWB information from DB
'Tasks that are performed within:									    
'1.Retreive MAWB general information from DB
'2.Retrieve MAWB other charges from DB
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_MAWB_INFO_FROM_TABLE( vMAWB )
	DIM lAccountInfo
	lAccountInfo = ""		
	vOriginPortID=Request("hOriginPortID")
	vDepartureAirport = Request("txtDepartureAirport")
	vTo=Request("txtTo")
	vBy=Request("txtBy")
	vTo1=Request("txtTo1")
	vBy1=Request("txtBy1")
	vTo2=Request("txtTo2")
	vBy2=Request("txtBy2")
	vAccountInfo=Request("txtBillToInfo")
	vDestAirport=Request("txtDestAirport")
	vFlightDate1=Request("txtFlightDate1")
	vFlightDate2=Request("txtFlightDate2")
	vDestCountry=Request("txtDestCountry")
	if Request("txtServiceLevel")="Select One" then
	    vServiceLevel = ""
	else
	    vServiceLevel = Request("txtServiceLevel")
	end if
	vIssuedBy=Request("txtIssuedBy")
	vExecute=Request("htxtExecute")
	vDesc2="CONSOLIDATION AS PER" & chr(13) & "MANIFEST" & chr(13) & "FREIGHT PREPAID"
	SQL= "select * from mawb_master where elt_account_number = " & elt_account_number & " and is_dome='Y' AND master_type='DA' and MAWB_NUM = '" & vMAWB & "'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing	
	
	If Not rs.EOF Then		
		vSalesPerson=rs("SalesPerson")     		
		
		if(isnull(vSalesPerson))then 
			vSalesPerson=""
		end if 
		
		vAirOrgNum=rs("airline_vendor_num")
		vShipperAcct = rs("Shipper_Account_Number")
		vShipperName = rs("Shipper_Name")			
		vFFShipperAcct = rs("ff_shipper_acct")
		vShipperInfo = rs("Shipper_Info")
		vConsigneeInfo = rs("Consignee_Info")
		vConsigneeName = rs("Consignee_Name")
		vConsigneeAcct = ConvertAnyValue(rs("Consignee_acct_num"),"String","0")
		vFFConsigneeAcct = rs("ff_Consignee_acct")
		vAgentInfo = rs("Issue_Carrier_Agent")
		vAgentIATACode = rs("Agent_IATA_Code")
		vNotifyAcct = rs("Notify_No")
		vAgentAcct = rs("Account_No")
		vDepartureAirport = rs("Departure_Airport")
		vOriginPortID = rs("DEP_AIRPORT_CODE")			
		vTo = rs("to_1")
		vBy = rs("by_1")
		
		vIACNum = rs("iac_num").value
        vKnownShipper = rs("known_shipper").value
        vUnKnownShipper = rs("unknown_shipper").value
        vItemUnder16 = rs("item_under_16").value
        vServiceLevel2 = rs("service_level_other")				
        vServiceLevel = rs("service_level")
        vCODAmount = rs("cod_amount").value
        vPickupCharge = rs("pickup_charge").value
        vOriginAdvCharge = rs("origin_adv_charge").value
        vOriginAdvChargeDesc = rs("origin_adv_charge_desc").value
        vItemPrepaid = rs("item_prepaid").value
        vItemCollect = rs("item_collect").value
        vCODFee = rs("cod_fee").value
        vOtherCharge = rs("other_charge").value
        vDeliveryCharge = rs("delivery_charge").value
        vDestAdvCharge = rs("dest_adv_charge").value
        vDestAdvChargeDesc = rs("dest_adv_charge_desc").value

		if vBy = "Select One" then 
		    vBy = "" 
		End If
		vTo1 = rs("to_2")
		vBy1 = rs("by_2")
		vTo2 = rs("to_3")
		vBy2 = rs("by_2")
		vDestAirport = rs("Dest_Airport")
		
		vFlightDate1 = rs("Flight_Date_1")
		'Response.Write("test " & vFlightDate1)
		
		vFlightDate2 = rs("Flight_Date_2")
		vIssuedBy = rs("IssuedBy")
		lAccountInfo = rs("Account_Info")
		vChargeCode = rs("Charge_Code")
		vPPO_1 = rs("PPO_1")
		vCOLL_1 = rs("COLL_1")
		vPPO_2 = rs("PPO_2")
		vCOLL_2 = rs("COLL_2")
		vDeclaredValueCarriage = rs("Declared_Value_Carriage")
		vDeclaredValueCustoms = rs("Declared_Value_Customs")
		vInsuranceAMT = rs("Insurance_AMT")
		vHandlingInfo = rs("Handling_Info")
		vDestCountry=rs("dest_country")
		vTotalPieces=rs("total_pieces")
		vTotalGrossWeight=rs("total_gross_weight")
		vTotalWeightCharge=rs("total_weight_charge_hawb")
		vDesc1=rs("desc1")
		vDesc2=rs("desc2")
		vPrepaidWeightCharge=rs("Prepaid_Weight_Charge")
		if vPrepaidWeightCharge="" then
			vPrepaidWeightCharge=cdbl(vPrepaidWeightCharge)
		else
			vPrepaidWeightCharge=0
		end if
		vCollectWeightCharge=rs("Collect_Weight_Charge")
		if vCollectWeightCharge="" then
			vCollectWeightCharge=cdbl(vCollectWeightCharge)
		else
			vCollectWeightCharge=0
		end if
		vPrepaidOtherChargeAgent=FormatNumberPlus(rs("Prepaid_Due_Agent"),2)
		vCollectOtherChargeAgent=FormatNumberPlus(rs("Collect_Due_Agent"),2)
		vPrepaidOtherChargeCarrier=FormatNumberPlus(rs("Prepaid_Due_Carrier"),2)
		vCollectOtherChargeCarrier=FormatNumberPlus(rs("Collect_Due_Carrier"),2)
		vPrepaidTotal=FormatNumberPlus(rs("Prepaid_Total"),2)
		vCollectTotal=FormatNumberPlus(rs("Collect_Total"),2)
		vPrepaidValuationCharge=FormatNumberPlus(rs("Prepaid_Valuation_Charge"),2)
		vCollectValuationCharge=FormatNumberPlus(rs("Collect_Valuation_Charge"),2)
		vPrepaidTax=FormatNumberPlus(rs("Prepaid_Tax"),2)
		vCollectTax=FormatNumberPlus(rs("Collect_Tax"),2)
		vConversionRate=FormatNumberPlus(rs("Currency_Conv_Rate"),2)
		vCCCharge=FormatNumberPlus(rs("CC_Charge_Dest_Rate"),2)
		vChargeDestination=FormatNumberPlus(rs("Charge_at_Dest"),2)
		vFinalCollect=FormatNumberPlus(rs("Total_Collect_Charge"),2)
		vShowWeightChargeShipper=rs("show_weight_charge_shipper")
		vShowWeightChargeConsignee=rs("show_weight_charge_consignee")
		vShowPrepaidOtherChargeShipper=rs("show_prepaid_other_charge_shipper")
		vShowCollectOtherChargeShipper=rs("show_collect_other_charge_shipper")
		vShowPrepaidOtherChargeConsignee=rs("show_prepaid_other_charge_consignee")
		vShowCollectOtherChargeConsignee=rs("show_collect_other_charge_consignee")
		vSCI = rs("SCI")
		vSignature = rs("Signature")
		vExecute=rs("execution")		
		vReferenceNumber = rs("reference_number").value
		
    Else
		NewMAWB="Y"
		vPPO_1="Y"
		vDesc2=vDesc2 & chr(10) & ""
		vPPO_2="Y"
		vDeclaredValueCarriage="NVD"
		vDeclaredValueCustoms="NCV"
		vInsuranceAMT="XXX"
		vShowWeightChargeShipper="Y"
		vShowWeightChargeConsignee="Y"
		vShowPrepaidOtherChargeShipper="Y"
		vShowCollectOtherChargeShipper="Y"
		vShowPrepaidOtherChargeConsignee="Y"
		vShowCollectOtherChargeConsignee="Y"
    End If
    
    rs.Close	
		
		if Not NewMAWB="Y" then
			SQL= "select * from mawb_other_charge where elt_account_number = " & elt_account_number & " and mawb_num='" & vMAWB & "' order by tran_no"

			rs.CursorLocation = adUseClient
			rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
			Set rs.activeConnection = Nothing
			oIndex=0
			Do while Not rs.EOF
				aCollectPrepaid(oIndex)=rs("coll_prepaid")
				aCarrierAgent(oIndex)=rs("carrier_agent")
				aChargeCode(oIndex)=rs("charge_code")
				aDesc(oIndex)=rs("charge_desc")
				aChargeAmt(oIndex)=rs("amt_mawb")
				rs.MoveNext
				oIndex=oIndex+1
			Loop
			rs.Close
		else
			oIndex=2
			vAccountInfo = ""		
			Exit Sub
		end if		
		If vMawb = "" Then
			vAccountInfo = ""
		Else
			If Not lAccountInfo = ""  Then
			vAccountInfo = lAccountInfo
		End If
              
	End If
	
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:ADD_HAWB_INFO
'Purpose  of the procedure: The procedure is in charge of adding a HAWB to the MAWB
'Tasks that are performed within:									    
'1.Set booking information to the HAWB 
'2.If the HAWB is a Master House, change all the booking information portion of the sub hosues as well
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB ADD_HAWB_INFO
	AddHAWBNo=Request.QueryString("AddHAWBNo")
	vAddELTAcct=Request.QueryString("AddELTAcct")
	dim is_master_house	
	CALL CHECK_INVOICE_STATUS_HAWB(	AddHAWBNo,vAddELTAcct )	
	SQL= "select * from hawb_master where elt_account_number = " & vAddELTACCT & " and is_dome='Y' and HAWB_NUM='" & addHAWBNo & "'"
	rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
	If Not rs.EOF Then
		rs("MAWB_NUM")=vMAWB
		rs("Departure_Airport") = vDepartureAirport
		rs("To_1") = vTo
		if vBy = "Select One" then vBy = ""			
		rs("By_1") = vBy
		rs("To_2") = vTo1
		rs("By_2") = vBy1
		rs("To_3") = vTo2
		rs("By_3") = vBy2
		rs("Dest_Airport") = vDestAirport
		rs("Flight_Date_1") = vFlightDate1
		rs("Flight_Date_2") = vFlightDate2
		is_master_house=ConvertAnyValue(rs("is_master"),"String","N")
		rs.Update			
	end if
	rs.Close		
	if is_master_house="Y" then
			UPDATE_ALL_SUB_HOUSE_INFO vAddELTACCT,addHAWBNo,"N"
	end if			
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:DELETE_HAWB_INFO
'Purpose  of the procedure: The procedure is in charge of deleting a HAWB from the MAWB
'Tasks that are performed within:									    
'1.Set booking information to be empty
'2.If the HAWB is a Master House, change all the booking information portion of the sub hosues as well
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB DELETE_HAWB_INFO
		dHAWB=Request("dHAWB")
		vDelELTAcct=Request.QueryString("delELTAcct")
		dim is_master_house
		CALL CHECK_INVOICE_STATUS_HAWB(	dHAWB,vDelELTAcct )
		SQL= "select * from hawb_master where elt_account_number = " & vDelELTAcct & " and is_dome='Y' and HAWB_NUM='" & dHAWB & "'"
		rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
		If Not rs.EOF Then
			is_master_house=ConvertAnyValue(rs("is_master"),"String","N")
			rs("MAWB_NUM")=""
			rs("Departure_Airport") = ""
			rs("To_1") = ""
			rs("By_1") = ""
			rs("To_2") = ""
			rs("By_2") = ""
			rs("To_3") = ""
			rs("By_3") = ""
			rs("Dest_Airport") = ""
			rs("Flight_Date_1") = ""
			rs("Flight_Date_2") = ""
			rs.Update
		end if
		rs.Close		
		if is_master_house="Y" then
				UPDATE_ALL_SUB_HOUSE_INFO vDelELTAcct,dHAWB,"Y"
		end if 
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_ITEM_WEIGHT_CHARGE_INFO_SCREEN
'Purpose  of the procedure: The procedure is in charge of getting weight charge items from screen
'Tasks that are performed within:									    
'1.Get all the item charges from screen and save them to the varialbes 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_ITEM_WEIGHT_CHARGE_INFO_SCREEN
	NoItemWC=Request("hNoItemWC")
	if NoItemWC="" then NoItemWC=0
		vTotalChgWT=0
	for i=0 to NoItemWC-1
		aPiece(i)=Request("txtPiece" & i)
		if aPiece(i)="" then aPiece(i)=0
		aGrossWeight(i)=Request("txtGrossWeight" & i)
		if aGrossWeight(i)="" then aGrossWeight(i)=0
		aKgLb(i)=Request("lstKgLb" & i)
		aRateClass(i)=Request("txtRateClass" & i)
		aItemNo(i)=Request("txtItemNo" & i)
		aChargeableWeight(i)=Request("txtChargeableWeight" & i)
		if aChargeableWeight(i)="" then aChargeableWeight(i)=0
		vTotalChgWT=vTotalChgWT+cDbl(aChargeableWeight(i))
		aRateCharge(i)=Request("txtRateCharge" & i)		
		if aRateCharge(i)="" then aRateCharge(i)=0
		aTotal(i)=Request("txtTotal" & i)		
		if aTotal(i)="" then aTotal(i)=0
	next	
	vWeightScale = aKgLb(0)
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_ITEM_OTHER_CHARGE_INFO_SCREEN
'Purpose  of the procedure: The procedure is in charge of getting other charge items from screen
'Tasks that are performed within:									    
'1.Get all the other charges from screen and save them to the varialbes 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_ITEM_OTHER_CHARGE_INFO_SCREEN
	NoItemOC=Request("hNoItemOC")
	if NoItemOC="" then NoItemOC=0
	for i=0 to NoItemOC-1
		aCarrierAgent(i)=Request("lstCarrierAgent" & i)
		aCollectPrepaid(i)=Request("lstCollectPrepaid" & i)
		if aCollectPrepaid(i)="P" then
			vPPO_2="Y"
		end if
		if aCollectPrepaid(i)="C" then
			vCOLL_2 ="Y"
		end if
		ChargeItemInfo=Request("lstChargeCode" & i)
		pos=0
		pos=Instr(ChargeItemInfo,"-")
		if pos>0 then
			aChargeCode(i)=cInt(left(ChargeItemInfo,pos-1))
			aDesc(i)=Mid(ChargeItemInfo,pos+1,2000)
			pos=0
			pos=Instr(aDesc(i),"-")
			if (pos > 0) then
				aDesc(i)=LTRIM(Mid(aDesc(i),1,pos-1))
			end if
		end if		
		aChargeAmt(i)=Request("txtChargeAmt" & i)
		if aChargeAmt(i)="" then aChargeAmt(i)=0	
	next
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:ADJUST_WEIGHT
'Purpose  of the procedure: The procedure is in charge of fixing adjustable weight portion of the
'                            consolidated HAWBs on the screen                          
'Tasks that are performed within:									    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB ADJUST_WEIGHT
	Dim tHAWB,tAW,tRateClass,tmpTran_no
	NoHAWB=Request.QueryString("AdjustItemNo")	
	for k=0 to NoHAWB-1
		if not Request("txtsHAWB" & k)="" then
			tHAWB=Request("txtsHAWB" & k)
		end if	
		Call Check_Invoice_status_HAWB(	tHAWB, elt_account_number )	
		if not Request("txtsAW" & k)="" then
			tAW=(Request("txtsAW" & k))				
		else
			tAW=0
		end if			
		if not Request("txtsRateClass" & k)="" then
			tRateClass=Request("txtsRateClass" & k)
		else
			tRateClass=""
		end if		
		if not Request("txtWeightTran" & k)="" then
			tmpTran_no=Request("txtWeightTran" & k)
		else
			tmpTran_no=""
		end if					
		IsCOLO=""
		IsCOLO=Request("txtsCOLO" & k)		
		if IsCOLO="" then
			SQL= "select * from hawb_weight_charge where elt_account_number = " & elt_account_number & " And HAWB_NUM='" & tHAWB & "' and tran_no='" & tmpTran_no & "'"
			rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText			
			if not rs.EOF then
				tKgLb=rs("kg_lb")
				if not tKgLb=aKgLb(0) then
					if aKgLb(0)="K" then
						tAW=tAW*2.20462262
					else
						tAW=tAW/2.20462262
					end if
				end if					
				rs("Adjusted_Weight") = CDbl(tAW)
				rs.Update
			end if
			rs.Close
		end if
	next		
END SUB 
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:DELETE_OTHER_CHARGE_INFO
'Purpose  of the procedure: The procedure is in charge of deleting an other charge information                                               
'Tasks that are performed within:									    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB DELETE_OTHER_CHARGE_INFO
	dItemNo=Request.QueryString("dItemNo")
	for i=dItemNo to NoItemOC-1
		aCarrierAgent(i)=aCarrierAgent(i+1)
		aCollectPrepaid(i)=aCollectPrepaid(i+1)
		aChargeCode(i)=aChargeCode(i+1)
		aDesc(i)=aDesc(i+1)
		aChargeAmt(i)=aChargeAmt(i+1)	
	next
	NoItemOC=NoItemOC-1
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:SET_ITEM_OTHER_CHARGE_INFO
'Purpose  of the procedure: The procedure is in charge of setting the varialbes related to other charge information
'                           according to the values taken from screen                                            
'Tasks that are performed within:		
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUb SET_ITEM_OTHER_CHARGE_INFO
	oIndex=NoItemOC
	For i=0 To NoItemOC-1
		if aCarrierAgent(i)="A" and aCollectPrepaid(i)="P" then
			vPrepaidOtherChargeAgent=vPrepaidOtherChargeAgent+aChargeAmt(i)
		elseif aCarrierAgent(i)="A" and aCollectPrepaid(i)="C" then
			vCollectOtherChargeAgent=vCollectOtherChargeAgent+aChargeAmt(i)
		elseif aCarrierAgent(i)="C" and aCollectPrepaid(i)="P" then
			vPrepaidOtherChargeCarrier=vPrepaidOtherChargeCarrier+aChargeAmt(i)
		elseif aCarrierAgent(i)="C" and aCollectPrepaid(i)="C" then
			vCollectOtherChargeCarrier=vCollectOtherChargeCarrier+aChargeAmt(i)
		end if
	Next
	CALL SET_DETAULT_OTHER_CHARGE_ITEM_LINE		
	vShowWeightChargeShipper="Y"
	vShowWeightChargeConsignee="Y"
	vShowPrepaidOtherChargeShipper="Y"
	vShowCollectOtherChargeShipper="Y"
	vShowPrepaidOtherChargeConsignee="Y"
	vShowCollectOtherChargeConsignee="Y"	
	If vPPO_1="Y" Then
		vPrepaidWeightCharge=vTotalWeightCharge
	Else
		vCollectWeightCharge=vTotalWeightCharge
	End If
	vPrepaidValuationCharge=Request("txtPrepaidValuationCharge")
	if vPrepaidValuationCharge="" then vPrepaidValuationCharge=0
	vPrepaidValuationCharge=cdbl(vPrepaidValuationCharge)	
	vCollectValuationCharge=Request("txtCollectValuationCharge")
	if vCollectValuationCharge="" then vCollectValuationCharge=0
	vCollectValuationCharge=cdbl(vCollectValuationCharge)	
	vPrepaidTax=Request("txtPrepaidTax")
	if vPrepaidTax="" then vPrepaidTax=0
	vPrepaidTax=cdbl(vPrepaidTax)
	vCollectTax=Request("txtCollectTax")
	if vCollectTax="" then vCollectTax=0
	vCollectTax=cdbl(vCollectTax)
	vConversionRate=Request("txtConversionRate")
	if vConversionRate="" then vConversionRate=0
	vConversionRate=cdbl(vConversionRate)
	vCCCharge=Request("txtCCCharge")
	if vCCCharge="" then vCCCharge=0
	vCCCharge=cdbl(vCCCharge)	
	vChargeDestination=Request("txtChargeDestination")	
	if vChargeDestination="" then vChargeDestination=0
	vChargeDestination=cdbl(vChargeDestination)
	vPrepaidTotal=vPrepaidWeightCharge+vPrepaidValuationCharge+vPrepaidTax+vPrepaidOtherChargeAgent+vPrepaidOtherChargeCarrier
	vCollectTotal=vCollectWeightCharge+vCollectValuationCharge+vCollectTax+vCollectOtherChargeAgent+vCollectOtherChargeCarrier
	vFinalCollect=vCollectTotal+vChargeDestination
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_SELECTED_HAWB_INFO
'Purpose  of the procedure: The procedure is in charge of getting  HAWBs that have same MAWB # from DB						                                         
'Tasks that are performed within:						    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_SELECTED_HAWB_INFO
    DIM rs
	Set rs = Server.CreateObject("ADODB.Recordset")
	SQL="select a.sub_count, a.is_master, a.elt_account_number,a.hawb_num,a.agent_name,a.Shipper_Name,a.Consignee_Name, "
	SQL=SQL & " b.tran_no,b.rate_class,b.no_pieces,b.gross_weight,b.adjusted_weight,b.kg_lb,b.dimension,b.chargeable_weight from hawb_master a LEFT OUTER JOIN hawb_weight_charge b "
	SQL=SQL & " on (a.elt_account_number=b.elt_account_number) and (a.hawb_num =b.hawb_num) where "
	SQL=SQL & " (a.elt_account_number = " & elt_account_number & " or a.coloder_elt_acct=" & elt_account_number
	SQL=SQL & ") and isnull(a.is_sub,'N')='N' and a.MAWB_NUM = '" & vMAWB & "' and not (isnull(a.is_master,'N')='Y' and a.sub_count <=0) and a.is_dome='Y' order by a.hawb_num,b.tran_no"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	sCount=0	
	Do While Not rs.EOF 
		If IsNull(rs("tran_no")) = False Then
			tran_no = CInt(rs("tran_no"))
		Else
			tran_no=0
		end if
		if tran_no>0 then
			tKgLb=rs("kg_lb")
			If IsNull(rs("rate_class")) = False Then
				sRateClass(sCount) = rs("rate_class")
			End If	
			If IsNull(rs("no_pieces")) = False Then
					sPCS(sCount) = cLng(rs("no_pieces"))
			End If	
			If IsNull(rs("gross_weight")) = False Then
				sGW(sCount) = Cdbl(rs("gross_weight"))
			Else
				sGW(sCount)=0
			end if
			If IsNull(rs("adjusted_weight")) = False Then
				sAW(sCount) = Cdbl(rs("adjusted_weight"))
			Else
				sAW(sCount)=0
			end if
			If IsNull(rs("dimension")) = False Then
				sDW(sCount) = Cdbl(rs("dimension"))
			End If	
			If IsNull(rs("Chargeable_Weight")) = False Then
				sCW(sCount) = Cdbl(rs("Chargeable_Weight"))
			Else
				sCW(sCount)=0
			end if
			aWeightTran(sCount) = rs("tran_no")
			if  tKgLb <> aKgLb(0) then
				if aKgLb(0)="K" then
					sGW(sCount)=FormatNumber(sGW(sCount)/2.20462262,2)
					sAW(sCount)=FormatNumber(sAW(sCount)/2.20462262,2)
					sCW(sCount)=FormatNumber(sCW(sCount)/2.20462262,2)
					sDW(sCount)=FormatNumber(sDW(sCount)/2.20462262,2)
				else
					sGW(sCount)=FormatNumber(sGW(sCount)*2.20462262,2)
					sAW(sCount)=FormatNumber(sAW(sCount)*2.20462262,2)
					sCW(sCount)=FormatNumber(sCW(sCount)*2.20462262,2)
					sDW(sCount)=FormatNumber(sDW(sCount)*2.20462262,2)
				end if
			end if
		end if
		if tran_no=0 or tran_no=1 then
			If IsNull(rs("hawb_num")) = False Then
				sHAWB(sCount)=rs("hawb_num")
				delELTAcct(sCount)=rs("elt_account_number")
			End If	
			tmpAcct=cLng(rs("elt_account_number"))
			if Not tmpAcct=elt_account_number then
				xx=0
				Do while xx<coIndex
					if aColodeeAcct(xx)=tmpAcct then
						sCOLO(sCount)=aColodeeName(xx)
						exit do
					end if
					xx=xx+1
				loop
			end if
			If IsNull(rs("agent_name")) = False Then
			sAgent(sCount) = rs("agent_name")
				pos=Instr(sAgent(sCount),chr(10))
				if pos>0 then sAgent(sCount)=Mid(sAgent(sCount),1,pos-1)
			End If	
			sShipper(sCount) = rs("Shipper_name")
			sConsignee(sCount) = rs("Consignee_name")
		end if
		sTotalPCS=sTotalPCS+sPCS(sCount)
		sTotalGW=sTotalGW+cDbl(sGW(sCount))
		sTotalAW=sTotalAW+cDbl(sAW(sCount))
		sTotalDW=sTotalDW+cDbl(sDW(sCount))
		sTotalCW=sTotalCW+cDbl(sCW(sCount))
		if sTotalCW>0 then
			GWPercent=Round((sTotalGW/sTotalCW)*100,1)
			AWPercent=Round((sTotalAW/sTotalCW)*100,1)
			DWPercent=Round((sTotalDW/sTotalCW)*100,1)
		end if		
		sIsMaster(sCount)=ConvertAnyValue(rs("is_master"),"String","N")	
		sCount=sCount+1
		rs.MoveNext
	Loop
	rs.Close
	set rs = nothing
END SUB

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:PERFORM_CHECK_MISMATCH_ERROR
'Purpose  of the procedure: The procedure is in charge of alerting the mismatch of pieces between the total 
'of houses and the one on the mawb                                            
'Tasks that are performed within:						    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB PERFORM_CHECK_MISMATCH_ERROR
	DIM hCnt
	On Error Resume Next:
		hCnt = cInt(document.frmMAWB.hTotalHAWB.value)	
	if Not Save="yes" and not AddOC="yes" and not AddHAWB="yes" and not DeleteOC="yes" and not DeleteHAWB="yes" and not AdjustWeight="yes" and not DeleteMAWB="yes" then		
		if Not vTotalPieces="" and hCnt > 0 then
			if Not cLng(vTotalPieces)=sTotalPCS then
			ErrMSG="PCS mismatch between selected and saved! " & " ^ Selected Total Pieces:" & sTotalPCS & " ^ Saved Total Pieces: " & vTotalPieces
			end if
		end if
	end if
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_AVAIL_HAWB
'Purpose  of the procedure: The procedure is in charge of listing all the available house airway bill to the screen                                        
'Tasks that are performed within:
'1)Get all the available house airway bill informations from DB and store them to the variables
' INNER JOIN to LEFT OUTER JOIN TO get hawbs wo weight charge						    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB GET_AVAIL_HAWB
	DIM rs
	Set rs = Server.CreateObject("ADODB.Recordset")		
	SQL= "select a.elt_account_number,a.hawb_num,a.agent_name,a.Shipper_name,a.Consignee_name, "
	SQL=SQL & "b.tran_no,b.no_pieces,b.rate_class,b.kg_lb,b.gross_weight,b.adjusted_weight,b.chargeable_weight,b.dimension from hawb_master a INNER JOIN hawb_weight_charge b "
	SQL=SQL & " on (a.elt_account_number=b.elt_account_number) and (a.hawb_num=b.hawb_num) "
	SQL=SQL & " where (a.elt_account_number=" & elt_account_number &"or a.coloder_elt_acct=" & elt_account_number 
	SQL=SQL & ") and  isnull(a.is_sub,'N')='N' and a.MAWB_NUM = '' and not (a.elt_account_number=" &elt_account_number&" and isnull(colo,'')= 'Y') and not (isnull(a.is_master,'N')='Y' and a.sub_count <= 0) and a.is_dome='Y' order by a.hawb_num,b.tran_no" 
	
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	aCount=0
	Do While Not rs.EOF 
		If IsNull(rs("tran_no")) = False Then
			tran_no = CInt(rs("tran_no"))
		Else
			tran_no=0
		end if
		if tran_no>0 then
			tKgLb=rs("kg_lb")
			If IsNull(rs("no_pieces")) = False Then
				aPCS(aCount)=cdbl(rs("no_pieces"))
			End If
			If IsNull(rs("rate_class")) = False Then
				aClass(aCount)=rs("rate_class")
			End If
			If IsNull(rs("gross_weight")) = False Then
				aGW(aCount)=cdbl(rs("gross_weight"))
			Else
				aGW(aCount)=0
			end if
			If IsNull(rs("adjusted_weight")) = False Then
				aAW(aCount)=cdbl(rs("adjusted_weight"))
			Else
				aAW(aCount)=0
			end if
			If IsNull(rs("Chargeable_Weight")) = False Then
				aCW(aCount)=cdbl(rs("Chargeable_Weight"))
			Else
				aCW(aCount)=0
			end if
			If IsNull(rs("dimension")) = False Then
				aDW(aCount)=cdbl(rs("dimension"))
			End If			
			if not tKgLb=aKgLb(0) then
				if aKgLb(0)="K" then
					aGW(aCount)=FormatNumber(aGW(aCount)/2.20462262,2)
					aAW(aCount)=FormatNumber(aAW(aCount)/2.20462262,2)
					aCW(aCount)=FormatNumber(aCW(aCount)/2.20462262,2)
				else
					aGW(aCount)=FormatNumber(aGW(aCount)*2.20462262,2)
					aAW(aCount)=FormatNumber(aAW(aCount)*2.20462262,2)
					aCW(aCount)=FormatNumber(aCW(aCount)*2.20462262,2)
				end if
			end if
		end if
		if tran_no=0 or tran_no=1 then
			If IsNull(rs("hawb_num")) = False Then
				aHAWB(aCount) = rs("hawb_num")
				AddELTAcct(aCount)=rs("elt_account_number")
			End If	
			tmpAcct=cLng(rs("elt_account_number"))
			if Not tmpAcct=elt_account_number then
				xx=0
				Do while xx<coIndex				
					if aColodeeAcct(xx)=tmpAcct then
						aCOLO(aCount)=aColodeeName(xx)
						exit do
					end if
					xx=xx+1
				loop
			end if
			If IsNull(rs("agent_name")) = False Then
				aAgent(aCount) = rs("agent_name")
				pos=Instr(aAgent(tIndex),chr(10))
				if pos>0 then aAgent(aCount)=Mid(aAgent(aCount),1,pos-1)
			End If	
			aShipper(aCount) = rs("Shipper_name")
			aConsignee(aCount) = rs("Consignee_name")
		end if
		aCount=aCount+1
		rs.MoveNext
	Loop
	rs.Close
	set rs = nothing
END SUB

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_AVAIL_HAWB
'Purpose  of the procedure: The procedure is in charge of listing all the available house airway bill to the screen                                        
'Tasks that are performed within:
'1)Get all the available house airway bill informations from DB and store them to the variables						    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB GET_MAWB_WEIGHT_CHARGE_INFO_FROM_TABLE
		DIM rs
		Set rs = Server.CreateObject("ADODB.Recordset")
		wCount=0
		SQL="select no_pieces as p,rate_class,kg_lb,gross_weight as aw,chargeable_weight as cw,rate_charge,total_charge, Commodity_Item_No"
		SQL=SQL & " from mawb_weight_charge where elt_account_number=" & elt_account_number & " and mawb_num='" & vMAWB & "' order by tran_no"
		rs.CursorLocation = adUseClient
		rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
		Set rs.activeConnection = Nothing
		Do While Not rs.EOF and wCount<3
			If IsNull(rs("p")) = False Then
				aPiece(wCount) = cLng(rs("p"))
			End If
			If IsNull(rs("rate_class")) = False Then
				aRateClass(wCount) = rs("rate_class")
			End If
			If IsNull(rs("kg_lb")) = False Then
				akgLb(wCount) = rs("kg_lb")
			End If
			If IsNull(rs("aw")) = False Then
				aGrossWeight(wCount) = cDbl(rs("aw"))
			End If
			If IsNull(rs("cw")) = False Then
				aChargeableWeight(wCount) = cDbl(rs("cw"))
			End If
			aItemNo(wCount) = rs("Commodity_Item_No")
			aRateCharge(wCount)=rs("rate_charge")
			aTotal(wCount)=rs("total_charge")			
			rs.MoveNext
			wCount=wCount+1
		Loop
		rs.Close
		set rs = nothing
END SUB

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:UPDATE_ALL_SUB_HOUSE_INFO
'Purpose  of the procedure: The procedure is in charge of updating booking information for all the sub houses
'							belong to a Master House                                        
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB UPDATE_ALL_SUB_HOUSE_INFO(elt_acct,MasterHouse,CLR)
	
    dim rs, SQL, HAWBS(50),hhIndex	
	set rs= Server.CreateObject("ADODB.Recordset")
	elt_account_number = Request.Cookies("CurrentUserInfo")("elt_account_number")	
	SQL= "select hawb_num as hb from hawb_master  where (elt_account_number= "
	SQL= SQL& elt_acct & " or coloder_elt_acct="
	SQL= SQL& elt_acct & ") and is_dome='Y' and is_sub='Y' and sub_to_no='"& MasterHouse & "'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing	
	If Not rs.EOF Then
		hhIndex=0
		Do While Not rs.EOF
			HAWBS(hhIndex)=rs("hb")
			hhIndex=hhIndex+1
			rs.MoveNext		
		Loop			
	End If	
	rs.close	
	IF CLR<>"Y" THEN 
		For i =0 to hhIndex -1	
			SQL= "select mawb_num,airline_vendor_num,DEP_AIRPORT_CODE,Departure_Airport,To_1,by_1,To_2,By_2,To_3,By_3,Dest_Airport,Flight_Date_1,Flight_Date_2,export_date,dest_country,departure_state,IssuedBy,Execution from hawb_master where (elt_account_number= "
			SQL= SQL& elt_account_number & " or coloder_elt_acct="
			SQL= SQL& elt_account_number & ") and is_dome='Y' and is_sub='Y' and hawb_num='"& HAWBS(i) & "'"
			rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
			if not rs.EOF then
				rs("mawb_num")=vMAWB
				rs("airline_vendor_num")=vAirOrgNum
				rs("DEP_AIRPORT_CODE") = vOriginPortID
				rs("Departure_Airport") = vDepartureAirport
				rs("To_1") = vTo
				rs("by_1") = vBy 
				rs("To_2") = vTo1
				rs("By_2") = vBy1
				rs("To_3") = vTo2
				rs("By_3") = vBy2
				rs("Dest_Airport") = vDestAirport
				rs("Flight_Date_1") = vFlightDate1
				rs("Flight_Date_2") = vFlightDate2
				rs("export_date")=vExportDate
				rs("dest_country")=vDestCountry
				rs("departure_state")=vDepartureState
				rs("IssuedBy")=vIssuedBy   
				rs("Execution")=vExecute						
				rs.Update
			end if 		
			rs.close
		next
	ELSE 	
		For i =0 to hhIndex -1	
			SQL= "select mawb_num,airline_vendor_num,DEP_AIRPORT_CODE,Departure_Airport,To_1,by_1,To_2,By_2,To_3,By_3,Dest_Airport,Flight_Date_1,Flight_Date_2,export_date,dest_country,departure_state,IssuedBy,Execution from hawb_master where (elt_account_number= "
			SQL= SQL& elt_account_number & " or coloder_elt_acct="
			SQL= SQL& elt_account_number & ") and is_sub='Y' and is_dome='Y' and hawb_num='"& HAWBS(i) & "'"
			rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
			if not rs.EOF then
				rs("mawb_num")=""
				rs("airline_vendor_num")=0
				rs("DEP_AIRPORT_CODE") = 0
				rs("Departure_Airport") = ""
				rs("To_1") = ""
				rs("by_1") = ""
				rs("To_2") = ""
				rs("By_2") = ""
				rs("To_3") =""
				rs("By_3") = ""
				rs("Dest_Airport") = ""
				rs("Flight_Date_1") = ""
				rs("Flight_Date_2") = ""
				rs("export_date")=null
				rs("dest_country")=""
				rs("departure_state")=""
				rs("IssuedBy")=""
				rs("Execution")=""						
				rs.Update
			end if 		
			rs.close
		next
	END  IF 
	set rs=nothing 
END SUB 


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:RECALC_ITEM_RATE_CHARGE
'Purpose  of the procedure: The procedure is in charge of recalculating Weight charges for the MAWB with all 
'							the sub houses belong to the MAWB 
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB RECALC_ITEM_RATE_CHARGE
	On Error Resume Next:
	if aKgLb(0)="K" then 
		SQL= "Select c.rate_class,c.kg_lb, Sum(p) as p, sum(aw) as aw , sum(cw) as cw from (select  b.rate_class as rate_class, 'K' as kg_lb,"
		SQL=SQL &"sum(b.no_pieces)  as p,"
		SQL=SQL &"case when b.kg_lb ='L' then sum(b.adjusted_weight)/2.20462262  else sum(b.adjusted_weight) end as aw,"
		SQL=SQL &"case when b.kg_lb ='L' then sum(b.chargeable_weight)/2.20462262  else sum(b.chargeable_weight) end as cw "
		
		SQL=SQL & "from hawb_master a INNER JOIN hawb_weight_charge b "
		SQL=SQL & " on (a.elt_account_number=b.elt_account_number) and (a.hawb_num=b.hawb_num) "
		SQL=SQL & " where a.is_dome='Y' and isnull(a.is_sub,'N') ='N' and (a.elt_account_number = " & elt_account_number & " or a.coloder_elt_acct=" & elt_account_number
		SQL=SQL & ") and a.MAWB_NUM = '" & vMAWB & "' GROUP BY  b.rate_class,b.Kg_Lb"
		SQL=SQL & ")C GROUP BY rate_class,kg_lb"
	
	else 
		SQL= "Select c.rate_class,c.kg_lb, Sum(p) as p, sum(aw) as aw , sum(cw) as cw from (select b.rate_class as rate_class, 'L' as kg_lb,"
		SQL=SQL &"sum(b.no_pieces) as p,"
		SQL=SQL &"case when b.kg_lb ='K' then sum(b.adjusted_weight)*2.20462262 else sum(b.adjusted_weight) end as aw,"
		SQL=SQL &"case when b.kg_lb ='K' then sum(b.chargeable_weight)*2.20462262 else sum(b.chargeable_weight) end as cw "
		
		SQL=SQL & "from hawb_master a INNER JOIN hawb_weight_charge b "
		SQL=SQL & " on (a.elt_account_number=b.elt_account_number) and (a.hawb_num=b.hawb_num) "
		SQL=SQL & " where a.is_dome='Y' and isnull(a.is_sub,'N')='N' and (a.elt_account_number = " & elt_account_number & " or a.coloder_elt_acct=" & elt_account_number
		SQL=SQL & ") and a.MAWB_NUM = '" & vMAWB & "' GROUP BY  b.rate_class,b.Kg_Lb"
		SQL=SQL & ")C GROUP BY rate_class,kg_lb"   
	end if 	
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	Do While Not rs.EOF and wCount<3
		If IsNull(rs("p")) = False Then
			aPiece(wCount) = cLng(rs("p"))
		End If
		If IsNull(rs("rate_class")) = False Then
			aRateClass(wCount) = rs("rate_class")
		End If
		If IsNull(rs("kg_lb")) = False Then
			tkgLb=rs("kg_lb")
		End If
		If IsNull(rs("aw")) = False Then
			aGrossWeight(wCount) = cDbl(rs("aw"))
		Else
			aGrossWeight(wCount)=0
		end if
		If IsNull(rs("cw")) = False Then
			aChargeableWeight(wCount) = cDbl(rs("cw"))
		Else
			aChargeableWeight(wCount)=0
		end if
	
		if not tKgLb=aKgLb(0) then
			if aKgLb(0)="K" then
				aChargeableWeight(wCount)=FormatNumber(aChargeableWeight(wCount)/2.20462262,2)
				aGrossWeight(wCount)=FormatNumber(aGrossWeight(wCount)/2.20462262,2)
			else
				aChargeableWeight(wCount)=FormatNumber(aChargeableWeight(wCount)*2.20462262,2)
				aGrossWeight(wCount)=FormatNumber(aGrossWeight(wCount)*2.20462262,2)
			end if
		end if
		Set rs1 = Server.CreateObject("ADODB.Recordset")
		'// vAirline=cInt(Mid(vMAWB,1,3))
		vAirline = GetCarrierCode(vMAWB)
		DIM MinCharge
		dim MinApplied
		MinApplied=false		
		SQL="select weight_break,rate,kg_lb from all_rate_table where elt_account_number=" & elt_account_number
		SQL=SQl & " and rate_type=5 "
		SQL=SQL & " and (airline=" & vAirline & " or airline=-1) and origin_port='" & vDepCode & "'"
		SQL=SQL & " and dest_port='" & vArrCode & "' order by weight_break desc"
		rs1.CursorLocation = adUseClient
		rs1.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
		Set rs1.activeConnection = Nothing
		rIndex=0
		dim rFLAG
		rFLAG=true 
		DIM tmpWT 
		tmpWT=-1
		do while not rs1.EOF
			wb=cdbl(rs1("weight_break"))
			tKgLb=rs1("kg_lb")
			TempRate=cdbl(rs1("rate"))
			if wb=0 then MinCharge=TempRate
			if not wb=0 and not tKgLb=aKgLb(0) then
				if aKgLb(0)="K" then 'query K and DB L--searched amount is smaller relatively, so aChargeableWeight get bigger
					tmpWT=cdbl(aChargeableWeight(wCount))*2.20462262
					TempRate=TempRate*2.20462262
				else
					tmpWT=cdbl(aChargeableWeight(wCount))/2.20462262
					 TempRate=TempRate/2.20462262
				end if
			end if
			if tmpWT <> -1 then
				if tmpWT >= wb then
				  if rFLAG = true then 
						aRateCharge(wCount)=TempRate
						if wb=0 then 
							MinApplied=true 
							aRateCharge(wCount)=1
						end if 
						rFLAG=false
				  end if
				end if 
			 else 
				if aChargeableWeight(wCount) >= wb then 
				  if rFLAG = true then 
						aRateCharge(wCount)=TempRate
						if wb=0 then 
							MinApplied=true 
							aRateCharge(wCount)=1
						end if 
						rFLAG=false
				  end if
				end if 
			 end if 
			rs1.MoveNext
			rIndex=rIndex+1
		loop
		rs1.Close	
		if not aRateCharge(wCount)="" then		
			if tmpWT <> -1 then 			 
				tmpNum = cdbl(aChargeableWeight(wCount)) * cDBL(aRateCharge(wCount))
			else
				tmpNum = cdbl(aChargeableWeight(wCount)) * cDBL(aRateCharge(wCount))			
			end if 
			aTotal(wCount)=FormatNumber(tmpNum,2)			
			if tmpNum < MinCharge  then				
			 aRateCharge(wCount)=0
			 aTotal(wCount)=MinCharge
			end if 			
			if MinApplied=true then 			
			 aRateCharge(wCount)=0
			 aTotal(wCount)=MinCharge
			end if			
		end if	
	wCount=wCount+1
	rs.MoveNext
	Loop
	rs.Close
END SUB 


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:RECALC_ITEM_TOTAL
'Purpose  of the procedure: The procedure is in charge of recalculating all the total charges for the MAWB 
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB RECALC_ITEM_TOTAL
	vTotalPieces=0
	vTotalGrossWeight=0
	vTotalWeightCharge=0
	vPrepaidWeightCharge=0
	vCollectWeightCharge=0
	vPrepaidTotal=0	
	for i=0 to wCount-1
		vTotalPieces=vTotalPieces+aPiece(i)
		
		vTotalGrossWeight=vTotalGrossWeight+aGrossWeight(i)		
		if aTotal(i)<>"" then
			vTotalWeightCharge=vTotalWeightCharge+cDbl(aTotal(i))
		end if
	next
	If vPPO_1="Y" Then
		vPrepaidWeightCharge=vTotalWeightCharge
	Else
		vCollectWeightCharge=vTotalWeightCharge
	End If
	vPrepaidTotal=vPrepaidWeightCharge+vPrepaidValuationCharge+vPrepaidTax+vPrepaidOtherChargeAgent+vPrepaidOtherChargeCarrier
	vCollectTotal=vCollectWeightCharge+vCollectValuationCharge+vCollectTax+vCollectOtherChargeAgent+vCollectOtherChargeCarrier
	vFinalCollect=vCollectTotal+vChargeDestination		
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:RESET_PIECE_WEIGHT
'Purpose  of the procedure: The procedure is in charge of setting pieces, gross wiehgt , chargealbe weight to be empty
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB RESET_PIECE_WEIGHT
	for i=0 to 2
		aPiece(i)=0
		aGrossWeight(i)=0
		aChargeableWeight(i)=0
	next
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:FIND_ALL_COLODEES
'Purpose  of the procedure: The procedure is in charge of making a list of coloders with the coloders that are 
'                           listed in the client profile with their elt_account_number
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB FIND_ALL_COLODEES
	DIM rs
	Set rs = Server.CreateObject("ADODB.Recordset")
	coIndex=0
	SQL= "select tran_date,colodee_name,colodee_elt_acct from colo where coloder_elt_acct = " & elt_account_number & " order by colodee_name,tran_date"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
'	response.Write("-------------------"&SQL&"<BR>")
	Set rs.activeConnection = Nothing
	Do While Not rs.EOF
		'response.write rs("tran_date") & "<br>"
		aColodeeName(coIndex)=rs("colodee_name")
		'response.Write("-------------"&aColodeeName(coIndex)&"<br>")
		aColodeeAcct(coIndex)=cLng(rs("colodee_elt_acct"))
		coIndex=coIndex+1
		rs.MoveNext
	Loop
	rs.Close
	set rs = nothing	
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_AGENT_SHIPPER_CONSIGNEE_VENDOR_INFO
'Purpose  of the procedure: The procedure is in charge of 
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB GET_AGENT_SHIPPER_CONSIGNEE_VENDOR_INFO
	aIndex=1
	cIndex=1
	sIndex=1
	aAgentName(0)="Select One"
	aAgentInfo(0)=""
	aShipperName(0)="Select One"
	aShipperInfo(0)=""
	aConsigneeName(0)="Select One"
	aConsigneeInfo(0)=""
	SQL= "select dba_name,org_account_number,is_agent,is_shipper,is_consignee from organization where elt_account_number = " & elt_account_number & " and (account_status='A'"
	if Not vShipperAcct="" then
		SQL=SQL & " or org_account_number=" & vShipperAcct
	end if
	if Not vConsigneeAcct="" then
		SQL=SQL & " or org_account_number=" & vConsigneeAcct
	end if
	if Not vFFAgentAcct="" then
		SQL=SQL & " or org_account_number=" & vFFAgentAcct
	end if
	
	SQL=SQL & ") and ( is_shipper='Y' or is_consignee = 'Y' or is_agent = 'Y') order by dba_name"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	Do While Not rs.EOF
		cName=rs("DBA_NAME")
		cAcct=cLng(rs("org_account_number"))
		IsAgent=rs("is_agent")
		IsShipper=rs("is_shipper")
		IsConsignee=rs("is_consignee")
		rs.MoveNext
		if IsConsignee="Y" or IsAgent="Y" then
			aAgentName(aIndex) = cName
			aAgentAcct(aIndex)=cAcct
			aIndex = aIndex+1
		end if
		if IsShipper="Y" then
			aShipperName(sIndex) = cName
			aShipperAcct(sIndex)=cAcct
			sIndex = sIndex+1
		end if
		if IsConsignee="Y" or IsAgent="Y" then
			aConsigneeName(cIndex) = cName
			aConsigneeAcct(cIndex)=cAcct
			cIndex = cIndex+1
		end If
	Loop
	rs.Close
END SUB

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_MAWB_NUMBER_FROM_TABLE
'Purpose  of the procedure: The procedure is in charge of making a list of MAWB that can be assinged when
'creating a MAWB
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB GET_MAWB_NUMBER_FROM_TABLE( vMAWB )
	SQL= "select * from mawb_number where elt_account_number = " & elt_account_number & " and is_dome='Y' and master_type='DA' and status='B' and is_inbound='N' order by mawb_no"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	If Not rs.EOF Then
		mIndex=0
		aMAWB(0)="Select One"
		Do While Not rs.EOF
			mIndex = mIndex + 1
			aMAWB(mIndex) = rs("mawb_no")
			aservicelevel=rs("service_level")
			rs.MoveNext
		Loop
	End If
	rs.close
END SUB




''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_CHARGE_ITEM_INFO
'Purpose  of the procedure: The procedure is in charge of creating a list of  charge items
'created for the MAWB
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB GET_CHARGE_ITEM_INFO
	SQL= "select * from item_charge where elt_account_number = " & elt_account_number & " order by item_name"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	
	chIndex=1
	aChargeItemName(0)="Select One"
	aChargeItemNo(0)=0
	aChargeItemDesc(0)=""
	aChargeUnitPrice(0)=0 '// Unit_price by ig 10/21/2006
	aChargeItemNameig(0)="Select One"
	Do While Not rs.EOF
		aChargeItemName(chIndex)=rs("item_name")
		aChargeItemNo(chIndex)=cInt(rs("item_no"))
		aChargeItemDesc(chIndex)=rs("item_desc")
		aChargeUnitPrice(chIndex)=rs("unit_price") 
		if ( len(aChargeItemName(chIndex))) < 7 then	
			aChargeItemNameig(chIndex) = aChargeItemName(chIndex) & " " & string(7-len(aChargeItemName(chIndex)),"-") & " " & aChargeItemDesc(chIndex)
		else
			aChargeItemNameig(chIndex) = aChargeItemName(chIndex)
		end if
		
		chIndex=chIndex+1
		rs.MoveNext
	Loop
rs.Close
END SUB
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:FINAL_SCREEN_PREPARE
'Purpose  of the procedure: The procedure is in charge of creating a list of  charge items
'creating a MAWB
'Tasks that are performed within:
'1)					    
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB FINAL_SCREEN_PREPARE
	DIM tmpStr
	IF NOT vMAWB = "" AND NOT vMAWB = "0" THEN
		CALL CHECK_INVOICE_STATUS_MAWB(	vMAWB, elt_account_number )	
	END IF	
	Set rs=Nothing
	Set rs1=Nothing
	Set rs3=Nothing
	if  vNotifyAcct="" or isnull(vNotifyAcct) then vNotifyAcct="0"  
	If IsNull(vExecute) Or Trim(vExecute) = "" Then
        GETVEXECUTE(vMAWB)
    End If    
	tmpstr = "A"&chr(13)&chr(10)&"S"
	pos = inStr(vExecute,tmpstr)
	if pos > 0 then
		vExecute = replace(vExecute,tmpstr,"AS")
	end if	
END SUB

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:CHECK_INVOICE_STATUS_HAWB
'Purpose  of the procedure: The procedure is in charge of checking out whether the invoices belong to the 
'HAWB has been processed or not 
'Tasks that are performed within:									    
'1.Find all the invoice # belong to the HAWB and store on a array
'2.Make a message string that will be used in alerting the user when the user attempt to modify HAWB that
'are already processed.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

SUB CHECK_INVOICE_STATUS_HAWB( tvHAWB, t_elt_account_number )
	DIM invoiceNUM(100),ivIndex
	if tvHAWB = "" Then Exit sub
	
	ivIndex = 0				
	SQL="select invoice_no from invoice where elt_account_number=" & t_elt_account_number & " and air_ocean = 'A' and hawb_num='" & tvHAWB & "'"
	rs3.CursorLocation = adUseClient
	rs3.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs3.activeConnection = Nothing
	Do While Not rs3.EOF
		invoiceNUM(ivIndex) = rs3("invoice_no")
		ivIndex = ivIndex + 1										
		rs3.MoveNext
	Loop
	rs3.Close
	
	if ivIndex = 0	then
		SQL= "select invoice_no from invoice_hawb where elt_account_number = " & elt_account_number & " and hawb_num='" & tvHAWB & "'"
		rs3.CursorLocation = adUseClient
		rs3.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
		Set rs3.activeConnection = Nothing
		Do While Not rs3.EOF
			invoiceNUM(ivIndex) = rs3("invoice_no")
			ivIndex = ivIndex + 1										
			rs3.MoveNext
		Loop
		rs3.Close
	
		if ivIndex = 0	then
			SQL="select hawb_num from invoice_queue where elt_account_number=" & t_elt_account_number & " and hawb_num='" & tvHAWB & "'" & " and invoiced = 'Y' "
			rs3.CursorLocation = adUseClient
			rs3.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
			Set rs3.activeConnection = Nothing
			if Not rs3.EOF then
				invoiceNUM(ivIndex) = "(Unknown)"
				ivIndex = ivIndex + 1										
			end if
			rs3.Close
		end if
			
	end if
	
	DIM tmpIVstrMsg	
	tmpIVstrMsg = ""
	if ivIndex > 0 then
		for iii = 0 to ivIndex - 1
			tmpIVstrMsg = tmpIVstrMsg	& invoiceNUM(iii) & ","
		next
		tmpIVstrMsg = MID(tmpIVstrMsg,1,LEN(tmpIVstrMsg)-1)
%>
<script language="javascript">

    //////////////////////////////////////////
    alert('The HAWB#:' + '<%=tvHAWB%>' + ' was already Invoiced as IV #:' + '<%= tmpIVstrMsg %>' + '.\nPlease check Invoice Information & HAWB information later.');
    //////////////////////////////////////////
</script>
<%
	end if
End Sub	

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:CHECK_INVOICE_STATUS_MAWB
'Purpose  of the procedure: The procedure is in charge of checking out whether the invoices belong to the 
'MAWB has been processed or not 
'Tasks that are performed within:									    
'1.Find all the invoice # belong to the MAWB and store on a array
'2.Make a message string that will be used in alerting the user when the user attempt to modify HAWB that
'are already processed.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	

SUB CHECK_INVOICE_STATUS_MAWB( tvMAWB, t_elt_account_number )
	DIM invoiceNUM(100),ivIndex
	If Not tvMAWB = "" then
		ivIndex = 0				
		SQL="select invoice_no from invoice where elt_account_number=" & t_elt_account_number & " and air_ocean = 'A' and mawb_num='" & tvMAWB & "'"
		rs3.CursorLocation = adUseClient
		rs3.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
		Set rs3.activeConnection = Nothing
		Do While Not rs3.EOF
			invoiceNUM(ivIndex) = rs3("invoice_no")
			ivIndex = ivIndex + 1										
			rs3.MoveNext
		Loop
		rs3.Close
	
		if ivIndex = 0	then
			SQL="select mawb_num from invoice_queue where elt_account_number=" & t_elt_account_number & " and mawb_num='" & tvMAWB & "'" & " and invoiced = 'Y' "
			rs3.CursorLocation = adUseClient
			rs3.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
			Set rs3.activeConnection = Nothing
			if Not rs3.EOF then
				invoiceNUM(ivIndex) = "(Unknown)"
				ivIndex = ivIndex + 1										
			end if
			rs3.Close
		end if
				
		IVstrMsg = ""
		if ivIndex > 0 then
			for iii = 0 to ivIndex - 1
				IVstrMsg = IVstrMsg	& invoiceNUM(iii) & ","
			next
			IVstrMsg = MID(IVstrMsg,1,LEN(IVstrMsg)-1)
		end if
	End if		
End Sub		


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:GET_QUEUE_ID
'Purpose  of the procedure: The procedure is in charge of retrieving current queue id that will be assinged
'to the next invoice queue entry
'Tasks that are performed within:									    
'1.retreive the most updated queue id
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
FUNCTION GET_QUEUE_ID
	SQL="select max(queue_id) as queue_id from invoice_queue where elt_account_number=" & elt_account_number
	rs1.CursorLocation = adUseClient
	rs1.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs1.activeConnection = Nothing
	If Not rs1.EOF And IsNull(rs1("queue_id"))=False Then
		vQueueID=CLng(rs1("queue_id"))+1
	Else
		vQueueID=1
	End If
	rs1.close
	GET_QUEUE_ID = vQueueID
END FUNCTION


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:MAWB_INVOICE_QUEUE
'Purpose  of the procedure: The procedure is in charge of creating/updating invoice queue entries that belongs to a MAWB
'Tasks that are performed within:									    
'1.Delete all the MAWB invoice queue entries in the queue that belong to the MAWB that the HAWB is assigned
'2.Recreate all the MAWB invoice queues reflecting the changes made for the HAWB and the Sub HAWBs, and the other 
'HAWBS that belong to the MAWB. 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB MAWB_INVOICE_QUEUE( vMAWB )		
	DIM tHAWB	
	vTotalHAWB=Request("hTotalHAWB")
	if vTotalHAWB="" then vTotalHAWB=0	
	if vTotalHAWB=0 then	
		if vPPO_1="Y" or vPPO_2="Y" Then
			SQL="select * from invoice_queue where elt_account_number=" & elt_account_number & " and agent_shipper='S' and mawb_num='" & vMAWB & "' and bill_to_org_acct=" & vShipperAcct
			rs3.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
			if rs3.EOF then
				vQueueID = GET_QUEUE_ID() 
				rs3.AddNew
				rs3("elt_account_number")=elt_account_number
				rs3("queue_id")=vQueueID
				rs3("inqueue_date")=now
				rs3("agent_shipper")="S"
				rs3("mawb_num")=vMAWB
				rs3("bill_to")=vShipperName
				rs3("bill_to_org_acct")=vShipperAcct
				rs3("agent_name")=vConsigneeName
				rs3("agent_org_acct")=vConsigneeAcct					
				rs3("air_ocean")="D"
				rs3("master_only")="Y"
				rs3("invoiced")="N"
				rs3.Update
			end if
			rs3.close
		end if	
		if vCOLL_1="Y" or vCOLL_2="Y" then
			SQL="select * from invoice_queue where elt_account_number=" & elt_account_number & " and agent_shipper='A' and mawb_num='" & vMAWB & "' and bill_to_org_acct=" & vConsigneeAcct
			rs3.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
			if rs3.EOF then
				vQueueID = GET_QUEUE_ID() 
				rs3.AddNew
				rs3("elt_account_number")=elt_account_number
				rs3("queue_id")=vQueueID
				rs3("inqueue_date")=now
				rs3("agent_shipper")="A"
				rs3("mawb_num")=vMAWB
				
				If vBilltoParty = "C" Then
				    rs("bill_to")=vConsigneeName
				    rs("bill_to_org_acct")=vConsigneeAcct
				    rs("agent_name")=vConsigneeName
				    rs("agent_org_acct")=vConsigneeAcct
				    rs("agent_shipper")="S"
				Else
				    rs("bill_to")=GetBusinessName(ConvertAnyValue(vNotifyAcct,"Long",0))
				    rs("bill_to_org_acct")=vNotifyAcct
				    rs("agent_name")=GetBusinessName(ConvertAnyValue(vNotifyAcct,"Long",0))
				    rs("agent_org_acct")=vNotifyAcct
				    rs("agent_shipper")="3"
				End If
				
				rs("air_ocean")="D"

				rs3("master_only")="Y"
				rs3("invoiced")="N"
				rs3.Update
			end if
			rs3.close
		end if			
	else		
		dim atmpHAWB(100),tmpIndex
		Set dict = CreateObject("Scripting.Dictionary")
		tmpIndex = 0
		SQL = "select hawb_num,Agent_Name from hawb_master where ((( isnull(is_master,'N')='Y' or isnull(is_sub,'N')='Y') and isnull(is_invoice_queued,'Y') <> 'N') OR (( isnull(is_master,'N')='N' and isnull(is_sub,'N')='N'))) and elt_account_number = " & elt_account_number & " and is_dome='Y' and mawb_num='" & vMAWB & "'" 		
		rs.CursorLocation = adUseClient
		rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
		Set rs.activeConnection = Nothing
		Do while Not rs.EOF
			atmpHAWB(tmpIndex)=rs("hawb_num")
			tmpAgent(tmpIndex)=rs("Agent_Name")				
			tmpval=tmpAgent(tmpIndex)
			if not dict.Exists(tmpval) then		   
				dict.Add tmpval, 1				
			else 			
				dict(tmpval)=dict(tmpval)+1					
			end if 
			tmpIndex = tmpIndex + 1
			rs.MoveNext
		Loop
		rs.Close
		for i=0 to tmpIndex-1				
			tHAWB=atmpHAWB(i)						
			if CHECK_SHOULD_INVOICE_QUEUED(tHAWB )= true then						
				CALL HAWB_INVOICE_QUEUE	( tHAWB, vTotalHAWB,tmpAgent(i) )
			else						
			end if					
		next 
	
		SQL="select master_agent from invoice_queue where elt_account_number=" & elt_account_number & " and agent_shipper='A' and mawb_num='" & vMAWB & "' and bill_to_org_acct=" & vConsigneeAcct
		rs3.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
		if Not rs3.EOF then
			rs3("master_agent")="Y"
			rs3.Update
		end if
		rs3.close					
	end if
	set dict=nothing 
END SUB


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:INVOICE_QUEUE_REFRESH
'Purpose  of the procedure: The procedure is in charge of deleting all the invoice queue entries that 
'belong to the HAWB/MAWB
'Tasks that are performed within:									    
'1.delete all the queue entries that belong to the HAWB
'2.delete all the queue entries that belong to the MAWB
'3.Last query should be understoood 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
SUB INVOICE_QUEUE_REFRESH( vMAWB )
	DIM arr_queue_id(100),qu_index	
	qu_index = 0				
	SQL="select queue_id from invoice_queue where elt_account_number=" _
	    & elt_account_number & " and mawb_num='" & vMAWB & "'" & " and invoiced = 'N' "
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	Do While Not rs.EOF
		arr_queue_id(qu_index) = rs("queue_id")
		qu_index = qu_index + 1										
		rs.MoveNext
	Loop
	rs.Close	
	for iii = 0 to qu_index - 1
		SQL= "delete invoice_queue where elt_account_number=" & elt_account_number & " and mawb_num='" & vMAWB & "'" & " and invoiced = 'N' and queue_id=" & arr_queue_id(iii)
		eltConn.Execute SQL			
	next	
	SQL= "delete invoice_queue where elt_account_number=" & elt_account_number & " and agent_shipper='A' and mawb_num='" _ 
	    & vMAWB & "' and invoiced = 'N' and bill_to_org_acct not in (select agent_no from hbol_master where elt_account_number=" _
	    & elt_account_number & " and booking_num = '" & vMAWB & "' group by agent_no )"
	eltConn.Execute SQL
End Sub
		

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'SUB PROCEDURE:HAWB_INVOICE_QUEUE
'Purpose  of the procedure: The procedure is in charge of creating/updating invoice queue entries that belongs to a HAWB
'Tasks that are performed within:									    
'1.Delete all the HAWB invoice queue entries in the queue that belong to the MAWB that the HAWB is assigned
'2.Recreate all the HAWB invoice reflecting the changes made for the HAWB and the Sub HAWBs, and the other 
'HAWBS that belong to the MAWB. 
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Sub HAWB_INVOICE_QUEUE( tmpHAWB, iiiCnt,tmpAt )
	DIM tvQueueID,tvShipperAcct,tvShipperName,tvFFAgentAcct,tvFFAgentName,tvPPO_1,tvPPO_2,tvCOLL_1,tvCOLL_2,tvMAWB,rs
	set rs=Server.CreateObject("ADODB.Recordset")
	SQL= "select * from hawb_master  where  elt_account_number = " & elt_account_number & " and is_dome='Y' and HAWB_NUM='" & tmpHAWB & "'"
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	if not rs.EOF Then		
			tvMAWB = rs("MAWB_NUM")
			tvShipperAcct = rs("Shipper_Account_Number")
			tvFFShipperAcct = rs("ff_shipper_acct")
			tvFFAgentName=rs("agent_name")
			tvFFAgentAcct=cLng(rs("agent_no"))
			tvShipperName=rs("shipper_name")
			tvPPO_1 = rs("PPO_1")
			tvCOLL_1 = rs("COLL_1")
			tvPPO_2 = rs("PPO_2")
			tvCOLL_2 = rs("COLL_2")
			
		if tvPPO_1="Y" or tvPPO_2="Y" then
			rs.close
						
			tvQueueID = GET_QUEUE_ID()
			
			SQL="select * from invoice_queue where elt_account_number=" & elt_account_number & " and agent_shipper='S' and hawb_num='" & tmpHAWB & "' and bill_to_org_acct=" & tvShipperAcct

			rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
			if rs.EOF then
				rs.AddNew
				rs("elt_account_number")=elt_account_number
				rs("queue_id")=tvQueueID
				rs("inqueue_date")=now
				rs("agent_shipper")="S"
				rs("hawb_num")=tmpHAWB
				rs("mawb_num")=tvMAWB
				rs("bill_to")=tvShipperName
				rs("bill_to_org_acct")=tvShipperAcct
				rs("agent_name")=tvFFAgentName
				rs("agent_org_acct")=tvFFAgentAcct
				rs("air_ocean")="D"
				rs("invoiced")="N"
				rs.Update
			end if
			rs.close
		else
			rs.close			
		end if	
		if tvCOLL_1="Y" or tvCOLL_2="Y" then
			tvQueueID = GET_QUEUE_ID()
			SQL="select * from invoice_queue where elt_account_number=" & elt_account_number & " and agent_shipper='A' and mawb_num='" & tvMAWB & "' and bill_to_org_acct=" & tvFFAgentAcct
			rs.Open SQL, eltConn, adOpenDynamic, adLockPessimistic, adCmdText
			if rs.EOF then
				rs.AddNew
				rs("elt_account_number")=elt_account_number
				rs("queue_id")=tvQueueID
				rs("inqueue_date")=now	
							
				if dict(tmpAt)=1 then				
					rs("hawb_num")=tmpHAWB
				else
					rs("hawb_num")="CONSOLIDATION"
				end if
				
				rs("mawb_num")=tvMAWB
				
				If vBilltoParty = "C" Then
				    rs("bill_to")=vConsigneeName
				    rs("bill_to_org_acct")=vConsigneeAcct
				    rs("agent_name")=vConsigneeName
				    rs("agent_org_acct")=vConsigneeAcct
				    rs("agent_shipper")="C"
				Else
				    rs("bill_to")=GetBusinessName(ConvertAnyValue(vNotifyAcct,"Long",0))
				    rs("bill_to_org_acct")=vNotifyAcct
				    rs("agent_name")=GetBusinessName(ConvertAnyValue(vNotifyAcct,"Long",0))
				    rs("agent_org_acct")=vNotifyAcct
				    rs("agent_shipper")="3"
				End If

				rs("air_ocean")="D"
				rs("invoiced")="N"
				rs.Update
			end if
			rs.close
		end if
	end if	
	set rs=nothing 
End Sub

if wCount = 0 then wCount = 1 
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
     <!--  #INCLUDE FILE="../include/ScriptHeader.inc" -->
    <title>New/Edit MAWB</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <script language="javascript" src="../include/CollapsibleRows.js"></script>
    <link href="../css/elt_css.css" rel="stylesheet" type="text/css">
    <style type="text/css">
    <!--
    .style5 {color: #663366}
    .style6 {font-size: 10px}
.style10 {
	color: #c16b42;
	font-weight: bold;
}
.style11 {
	color: #c16b42;
	font-size: 12px;
}
.style15 {color: #c16b42}
    -->
</style>
    <script type="text/javascript" src='../Include/iMoonCombo.js'></script>
    <script language="javascript" type="text/javascript" src="/ASP/ajaxFunctions/ajax.js"></script>
    <script type="text/javascript" src="../include/JPED.js"></script>
    <script type="text/javascript">
        function showtip() { }

        function checkMaxRows(txtObj) {
            var nomoretext = false;
            var str = txtObj.value;
            var allowedRows = txtObj.getAttribute("rows");
            var allowedCols = txtObj.getAttribute("cols");
            var lines = str.split("\r\n");

            if (lines.length > allowedRows) {
                if (window.event.keyCode != 8 && (window.event.keyCode < 37 || window.event.keyCode > 40)) {
                    alert("Can't add more line!")
                    nomoretext = true;
                    txtObj.value = "";
                    for (var i = 0; i < allowedRows; i++) {
                        if (i == allowedRows - 1) {
                            txtObj.value = txtObj.value + lines[i];
                        }
                        else {
                            txtObj.value = txtObj.value + lines[i] + "\n";
                        }
                    }
                }
            }
            return !nomoretext;
        }

        var ComboBoxes = new Array('lstMAWB');


        function validateSalesRep() {

            var txtSalesRep = document.getElementById("txtSalesRep");
            var salesRep = txtSalesRep.value;
            if (salesRep != "") {
                return true;
            } else {
                return false;
            }
        }

        function SalesRPChange() { }

        function docModified(arg) { }

        function textLimit(field, maxlen) {
            if (field.value.length > maxlen + 1)
                alert('Your input has exceeded the maximum character!');
            if (field.value.length > maxlen)
                field.value = field.value.substring(0, maxlen);
        }

        function setChargeableWeight(index) {

            id1 = "txtGrossWeight" + index;
            id2 = "txtDimension" + index;
            id3 = "txtChargeableWeight" + index;

            gross = document.getElementById(id1).value;
            dimension = document.getElementById(id2).value;
            if (gross == "") {
                gross = "0";
            }
            if (dimension == "") {
                dimension = "0";
            }
            gross = parseFloat(gross);
            dimension = parseFloat(dimension);

            if (gross > dimension) {
                document.getElementById(id3).value = gross;
            } else {
                document.getElementById(id3).value = dimension;
            }
        }

        // Start of list change effect //////////////////////////////////////////////////////////////////

        function lstShipperNameChange(orgNum, orgName) {
            var hiddenObj = document.getElementById("hShipperAcct");
            var infoObj = document.getElementById("txtShipperInfo");
            var txtObj = document.getElementById("lstShipperName");
            var divObj = document.getElementById("lstShipperNameDiv")

            hiddenObj.value = orgNum;
            infoObj.value = getOrganizationInfo(orgNum);
            txtObj.value = orgName;

            document.getElementsByName("txtSignature").value = "AS AGENT FOR " + orgName;

            divObj.style.position = "absolute";
            divObj.style.visibility = "hidden";
            divObj.style.height = "0px";
            docModified(1);
        }

        function lstConsigneeNameChange(orgNum, orgName) {
            var hiddenObj = document.getElementById("hConsigneeAcct");
            var infoObj = document.getElementById("txtConsigneeInfo");
            var txtObj = document.getElementById("lstConsigneeName");
            var divObj = document.getElementById("lstConsigneeNameDiv");

            hiddenObj.value = orgNum;
            infoObj.value = getOrganizationInfo(orgNum);
            txtObj.value = orgName;
            divObj.style.position = "absolute";
            divObj.style.visibility = "hidden";
            divObj.style.height = "0px";
            docModified(1);
        }

        function lstNotifyNameChange(orgNum, orgName) {
            var hiddenObj = document.getElementById("hNotifyAcct");
            var infoObj = document.getElementById("txtBillToInfo");
            var txtObj = document.getElementById("lstNotifyName");
            var divObj = document.getElementById("lstNotifyNameDiv")

            hiddenObj.value = orgNum;
            infoObj.value = getOrganizationInfo(orgNum);
            txtObj.value = orgName;
            divObj.style.visibility = "hidden";
            divObj.style.position = "absolute";
            divObj.style.height = "0px";
            docModified(1);
        }

        function getOrganizationInfo(orgNum) {
            if (window.ActiveXObject) {
                try {
                    xmlHTTP = new ActiveXObject("Msxml2.XMLHTTP");
                } catch (error) {
                    try {
                        xmlHTTP = new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (error) { return ""; }
                }
            }
            else if (window.XMLHttpRequest) {
                xmlHTTP = new XMLHttpRequest();
            }
            else { return ""; }

            var url = "/ASP/ajaxFunctions/ajax_get_org_address_info.asp?type=B&org=" + orgNum;

            xmlHTTP.open("GET", url, false);
            xmlHTTP.send();

            return xmlHTTP.responseText;
        }

        var dep = "";
        var arp = "";
        var Unit = "";
        var wgt = "";
        var airline = "";
        var cusAcc = "";

        function catchRatingInfo(index) {

            airline = "<%=vAirline%>"
            dep = "<%=vDepCode%>";
            arp = "<%=vArrCode%>";

            if (document.frmMAWB.cCOLL1.checked) {
                cusAcc = document.getElementById("hConsigneeAcct").value;
            } else {
                cusAcc = document.getElementById("hShipperAcct").value;
            }

            //GETTING WEIGHT 
            if (index == 0) {
                wgt = document.frmMAWB.txtChargeableWeight0.value;
                Unit = frmMAWB.lstKgLb0.options[document.frmMAWB.lstKgLb0.selectedIndex].value;

                var WGT;
                try {
                    WGT = wgt.split(",");
                } catch (f) { }

                if (WGT.length > 1 && WGT) {
                    wgt = "";
                    for (var i = 0; i < WGT.length; i++) {
                        wgt += WGT[i];
                    }
                }

            } else {
                wgt = document.frmMAWB.txtChargeableWeight0.value;
                Unit = document.frmMAWB.lstKgLb0.options[document.frmMAWB.lstKgLb0.selectedIndex].value;
                var WGT;
                try {
                    WGT = wgt.split(",");
                } catch (f) { }

                if (WGT.length > 1 && WGT) {
                    wgt = "";
                    for (var i = 0; i < WGT.length; i++) {
                        wgt += WGT[i];
                    }
                }
            }
        }

        function getIATARate(index) {

            document.getElementById("hCurrentIndex").value = index;
            catchRatingInfo(index);

            var req = "";
            if (window.ActiveXObject) {
                try {
                    req = new ActiveXObject("Msxml2.XMLHTTP");
                } catch (error) {
                    try {
                        req = new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (error) { return ""; }
                }
            }
            else if (window.XMLHttpRequest) {
                req = new XMLHttpRequest();
            }
            else { return ""; }

            if (req) {
                req.onreadystatechange = processReqChange;
                req.open("GET", "/ASP/ajaxFunctions/ajax_iata_rate.asp?Unit=" + Unit + "&airline=" + airline + "&arp=" + arp + "&dep=" + dep + "&wgt=" + wgt, true);
                req.send();
            }
        }

        function processReqChange() {

            var index = document.getElementById("hCurrentIndex").value;
            var rateId = "txtRateCharge" + index;
            var totalId = "txtTotal" + index;

            if (req.readyState == 4) {
                if (req.status == 200) {
                    var result = req.responseText;
                    // document.getElementById("txtSignature").value=result;
                    var numericVar = parseFloat(result);

                    if (numericVar < 0) {
                        if (confirm("Minimum charge will be applied.\n Would you like to proceed?")) {
                            var tVal = numericVar * -1;
                            tVal = Math.round(tVal * 1000) / 1000;

                            document.getElementById(totalId).value = tVal.toFixed(2);
                            document.getElementById(rateId).value = "N/A";
                        } else {
                            document.getElementById(rateId).value = "N/A";
                        }
                        return;
                    }
                    CSRate = parseFloat(result);
                    //result=Math.round(result*1000)/1000;
                    if (document.getElementById(rateId).value != 0 && CSRate != null) {
                        //document.getElementById(rateId).value=CSRate;
                        bCalClick(index);
                    }
                    document.getElementById(rateId).value = result;
                    if (result == 0) {
                        document.getElementById(rateId).value = "N/A";
                    }
                    document.getElementById(rateId).focus();


                } else {
                    document.getElementById(rateId).value = "N/A";
                    document.getElementById(rateId).focus();
                }
            }
        }

        function alertAll() {
            alert("from alert all:" + dep + "--" + arp + "--" + Unit + "--" + airline + "wgt=" + wgt);
        }

        function toggleSub(obj, masterHouse) {

            id2 = "hCount" + masterHouse;
            count = document.getElementById(id2).value;
            var arr = obj.src.split("/");

            if (arr[arr.length - 1] == "Collapse.gif") {
                obj.src = "../Images/Expand.gif";

                for (i = 0; i < count; i++) {
                    id1 = "tr" + masterHouse + i;
                    document.getElementById(id1).style.display = "none";
                }
            } else {
                obj.src = "../Images/Collapse.gif";
                for (i = 0; i < count; i++) {
                    id1 = "tr" + masterHouse + i;
                    document.getElementById(id1).style.display = "";
                }
            }
            //document.getElementById(id1).style.display="none";
        }

        function checkMismatch() {
            if (parseInt("<%=sCount%>") > 0) {
                if (document.getElementById("txtTotalPCS").value != document.frmMAWB.txtPiece0.value) {
                    if (confirm("PCS mismatch between selected and saved!\n Would you like to proceed anyway? ")) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
            return true;
        }


    </script>
</head>
<body link="336699" vlink="336699" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0"
    onload=" initCollapsingRows(); <% If NOT chkAvailTab Then Response.write("toggleVisibility('nothing')") ELSE Response.write("expanded('toggleButton')")%>; self.focus();">
    <!-- tooltip placeholder -->
    <div id="tooltipcontent">
    </div>
    <!-- placeholder ends -->
    <form method="post" name="frmMAWB">
    <input type="image" style="position: absolute; visibility: hidden" onclick="return false;" />
    <table width="95%" border="0" align="center" cellpadding="2" cellspacing="0">
        <tr>
            <td width="46%" height="32" align="left" valign="middle" class="pageheader">
                New/Edit MASTER AIR WAYBILL
            </td>
            <td width="54%" align="right" valign="middle">
                <span class="bodyheader style5">FILE NO.</span><input name="txtJobNum" type="text"
                    class="lookup" size="24" value="Search Here" onkeydown="javascript: if(event.keyCode == 13) { lookupFile(); }"
                    onclick="javascript: this.value = ''; this.style.color='#000000'; "><img src="../images/icon_search.gif"
                        name="B1" width="33" height="27" align="absmiddle" style="cursor: hand" onclick="lookupFile()">
            </td>
        </tr>
    </table>
    <div class="selectarea">
        <table width="95%" height="40" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td class="select" style="height: 12px">
                    <img src="/ASP/Images/required.gif" align="absbottom" alt="" />Select Master AWB
                    No.
                </td>
                <td rowspan="2" align="right" valign="bottom">
                    <div id="print" style="width: 180px">
                        <img src="/ASP/Images/icon_printer_preview.gif" align="absbottom" alt="" />
                        <a href="javascript:void(0);" id="NewPrintVeiw1" >Master Air Waybill</a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <!-- //Start of Combobox// -->
                    
                    <%  iMoonDefaultValue = vMAWB %>
                    <%  iMoonComboBoxName =  "lstMAWB" %>
                    <%  iMoonComboBoxWidth =  "160px" %>

                    <script language="javascript"> function <%=iMoonComboBoxName%>_OnChangePlus() {	MAWBChange("new"); } 
                    function <% =iMoonComboBoxName%>_OnAddNewPlus() {	}
                    </script>
                    <div id="<%=iMoonComboBoxName%>_Container" class="ComboBox" style="width: <%=iMoonComboBoxWidth%>;
                        position: ; top: ; left: ; z-index: ;">
                        <input name="<%=iMoonComboBoxName%>:Text" type="text" id="<%=iMoonComboBoxName%>_Text"
                            class="ComboBox" autocomplete="off" style="width: <%=iMoonComboBoxWidth%>; vertical-align: middle"
                            value="<%=iMoonDefaultValue%>" />
                        <div id="<%=iMoonComboBoxName%>_Div" style="display: none; position: absolute; top: 0;
                            left: -140px; width: 17px">
                            <img id="<%=iMoonComboBoxName%>_Button" src="/ig_common/Images/combobox_drop.gif"
                                border="0" />
                        </div>
                    </div>
                    <div id="<%=iMoonComboBoxName%>_NewDiv" style="display: none; position: absolute;
                        top: 0; left: 0; width: 17px">
                        <img id="<%=iMoonComboBoxName%>_AddNewButton" src="/ig_common/Images/combobox_addnew.gif"
                            border="0" />
                    </div>
                    <!-- /End of Combobox/ -->
                    <select name="lstMAWB" id="lstMAWB" listsize="20" class="ComboBox" style="width: 160px;
                        display: none" tabindex="1" onchange="ComboBox_SimpleAttach(this, this.form['<%=iMoonComboBoxName%>_Text'])">
                        <% For i=0 to mIndex %>
                        <option value="<%= aMawb(i) %>" <% if aMawb(i)=vMAWB then response.write("selected") %>>
                            <%= aMawb(i)%>
                        </option>
                        <% next %>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" bordercolor="#997132"
        bgcolor="#997132" class="border1px">
        <tr>
            <td>
                <!-- start of scroll bar -->
                <input type="hidden" name="scrollPositionX">
                <input type="hidden" name="scrollPositionY">
                <!-- end of scroll bar -->
                <input type="hidden" name="hCurrentIndex" id="hCurrentIndex" value="<%= vMAWB %>">
                <input type="hidden" name="hmawb_num" value="<%= vMAWB %>">
                <input type="hidden" name="hAirOrgNum" value="<%= vAirOrgNum %>">
                <input type="hidden" name="hmawbinfo" value="<%= vMAWBInfo %>">
                <input type="hidden" name="hOriginPortID" value="<%= vOriginPortID %>">
                <input type="hidden" name="hDefaultAgentName" value="<%= vDefaultAgentName %>">
                <input type="hidden" name="hDefaultAgentInfo" value="<%= vDefaultAgentInfo %>">
                <input type="hidden" name="hNoItemWC" value="<%= wCount %>">
                <input type="hidden" name="hNoItemOC" value="<%= oIndex %>">
                <input type="hidden" name="hExecution" value="<%= vExecutionDatePlace %>">
                <input type="hidden" name="hTotalHAWB" value="<%= sCount %>">
                <input type="hidden" name="htxtExecute">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="24" align="center" valign="middle" bgcolor="eec983" class="bodyheader">
                            <table width="98%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="33%">
                                        &nbsp;
                                    </td>
                                    <td width="34%" align="center" valign="middle">
                                        <img src='../images/button_save_medium.gif' name='bSave' onclick="if (checkMismatch()){bsaveClick()}"
                                            style="cursor: hand">
                                    </td>
                                    <td width="10%" align="right" valign="middle">
                                        <a href="/ASP/domestic/new_edit_mawb.asp" target="_self">
                                            <img src="/ASP/Images/button_new.gif" width="42" height="17" border="0" style="cursor: hand"></a>
                                    </td>
                                    <td width="13%" align="right" valign="middle">
                                        <img src="../images/button_closebooking.gif" width="48" height="18" name="bClose"
                                            onclick="bCloseClick()" style="cursor: hand" />
                                        <% if mode_begin then %>
                                        <div style="width: 21px; display: inline; vertical-align: text-bottom" onmouseover="showtip('Clicking this will close the current bill or booking. Closing it means it will still be saved in the system, but not accessible through the dropdowns on the Operations screen.  Often old bills that are very rarely accessed are closed to help keep the dropdowns &ldquo;clean&rdquo;.')"
                                            onmouseout="hidetip()">
                                            <img src="../Images/button_info.gif" align="bottom" class="bodylistheader"></div>
                                        <% end if %>
                                    </td>
                                    <td width="10%" align="right" valign="middle">
                                        <img src='../images/button_delete_medium.gif' width='51' height='17' name='bDeleteMAWB'
                                            onclick='DeleteMAWB()' style="cursor: hand">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="1" bgcolor="#997132">
                        </td>
                    </tr>
                    <tr align="center" valign="middle">
                        <td bgcolor="#FFFFFF">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                class="bodycopy" style="padding-left: 10px; border-bottom: 1px solid #997132">
                                <tr align="left" valign="middle" bgcolor="#f3d9a8" class="bodycopy">
                                    <td width="31%" height="18" bgcolor="#f3f3f3">
                                        <strong>Shipper's Name and Address</strong>
                                    </td>
                                    <td width="20%" bgcolor="#f3f3f3">
                                        Shipper's Account No.
                                    </td>
                                    <td width="49%" bgcolor="#f3f3f3">
                                        Not Negotiable
                                    </td>
                                </tr>
                                <tr align="left" valign="middle" bgcolor="#FFFFFF">
                                    <td valign="top" bgcolor="#FFFFFF">
                                        <!-- Start JPED -->
                                        <input type="hidden" id="hShipperAcct" name="hShipperAcct" value="<%=vShipperAcct %>" />
                                        <div id="lstShipperNameDiv">
                                        </div>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <input type="text" autocomplete="off" id="lstShipperName" name="lstShipperName" value="<%=vShipperName %>"
                                                        class="shorttextfield" style="width: 285px; border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9;
                                                        border-left: 1px solid #7F9DB9; border-right: 0px solid #7F9DB9;" onkeyup="organizationFill(this,'Shipper','lstShipperNameChange',null,event)"
                                                        onfocus="initializeJPEDField(this,event);" />
                                                </td>
                                                <td>
                                                    <img src="/ig_common/Images/combobox_drop.gif" alt="" onclick="organizationFillAll('lstShipperName','Shipper','lstShipperNameChange',null,event)"
                                                        style="border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9; border-right: 1px solid #7F9DB9;
                                                        border-left: 0px solid #7F9DB9; cursor: hand;" />
                                                </td>
                                                <td>
                                                     <input type='hidden' id='quickAdd_output' />
                                                    <img src="/ig_common/Images/combobox_addnew.gif" alt="" border="0" style="cursor: hand"
                                                        onclick="quickAddClient('hShipperAcct','lstShipperName','txtShipperInfo')" />
                                                </td>
                                            </tr>
                                        </table>
                                        <textarea id="txtShipperInfo" name="txtShipperInfo" class="monotextarea" cols=""
                                            rows="5" style="width: 300px"><%=vShipperInfo %></textarea>
                                        <!-- End JPED -->
                                    </td>
                                    <td valign="top">
                                        <input name="txtShipperAcct" class="shorttextfield" value="<%= vFFShipperAcct %>"
                                            size="24" maxlength="16">
                                    </td>
                                    <td valign="top" bgcolor="#FFFFFF">
                                        <span class="bodyheader style11">Air Way Bill </span>
                                        <br>
                                        <br />
                                        <span class="bodyheader">Issued by</span><br />
                                        <textarea name="txtIssuedBy" style="width: 300px" rows="3" wrap="hard" class="monotextarea"
                                            onkeypress="javascript:return checkMaxRows(this);"><%= vIssuedBy %></textarea>
                                    </td>
                                </tr>
                                <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                    <td height="18" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>Consignee's Name</strong>
                                    </td>
                                    <td bgcolor="#f3f3f3" class="bodycopy">
                                        Consignee's Account No.
                                    </td>
                                    <td bgcolor="#f3f3f3">
                                        <strong>Accounting Information </strong>
                                    </td>
                                </tr>
                                <tr align="left" valign="middle" bgcolor="#FFFFFF">
                                    <td>
                                        <!-- Start JPED -->
                                        <input type="hidden" id="hConsigneeAcct" name="hConsigneeAcct" value="<%=vConsigneeAcct %>" />
                                        <div id="lstConsigneeNameDiv">
                                        </div>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <input type="text" autocomplete="off" id="lstConsigneeName" name="lstConsigneeName"
                                                        value="<%=vConsigneeName %>" class="shorttextfield" style="width: 285px; border-top: 1px solid #7F9DB9;
                                                        border-bottom: 1px solid #7F9DB9; border-left: 1px solid #7F9DB9; border-right: 0px solid #7F9DB9;"
                                                        onkeyup="organizationFill(this,'Consignee','lstConsigneeNameChange',null,event)" onfocus="initializeJPEDField(this,event);" />
                                                </td>
                                                <td>
                                                    <img src="/ig_common/Images/combobox_drop.gif" alt="" onclick="organizationFillAll('lstConsigneeName','Consignee','lstConsigneeNameChange',null,event)"
                                                        style="border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9; border-right: 1px solid #7F9DB9;
                                                        border-left: 0px solid #7F9DB9; cursor: hand;" />
                                                </td>
                                                <td>
                                                    <img src="/ig_common/Images/combobox_addnew.gif" alt="" border="0" style="cursor: hand"
                                                        onclick="quickAddClient('hConsigneeAcct','lstConsigneeName','txtConsigneeInfo')" />
                                                </td>
                                            </tr>
                                        </table>
                                        <textarea id="txtConsigneeInfo" name="txtConsigneeInfo" class="monotextarea" cols=""
                                            rows="5" style="width: 300px"><%=vConsigneeInfo %></textarea>
                                        <!-- End JPED -->
                                    </td>
                                    <td valign="top">
                                        <input name="txtConsigneeAcct" class="shorttextfield" value="<%= vFFConsigneeAcct %>"
                                            size="24" maxlength="16">
                                    </td>
                                    <td bgcolor="#FFFFFF" valign="top">
                                        <table cellspacing="0" cellpadding="0" border="0" class="bodycopy" width="100%">
                                            <tr>
                                                <td>
                                                    <!-- Start JPED -->
                                                    <input type="hidden" id="hNotifyAcct" name="hNotifyAcct" value="<%=vNotifyAcct %>" />
                                                    <div id="lstNotifyNameDiv">
                                                    </div>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <input type="text" autocomplete="off" id="lstNotifyName" name="lstNotifyName" value="<%= GetBusinessName(ConvertAnyValue(vNotifyAcct,"Long",0)) %>"
                                                                    class="shorttextfield" style="width: 285px; border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9;
                                                                    border-left: 1px solid #7F9DB9; border-right: 0px solid #7F9DB9;" onkeyup="organizationFill(this,'Notify','lstNotifyNameChange',null,event)"
                                                                    onfocus="initializeJPEDField(this,event);" />
                                                            </td>
                                                            <td>
                                                                <img src="/ig_common/Images/combobox_drop.gif" alt="" onclick="organizationFillAll('lstNotifyName','Notify','lstNotifyNameChange',null,event)"
                                                                    style="border-top: 1px solid #7F9DB9; border-bottom: 1px solid #7F9DB9; border-right: 1px solid #7F9DB9;
                                                                    border-left: 0px solid #7F9DB9; cursor: hand;" />
                                                            </td>
                                                            <td>
                                                                <img src="/ig_common/Images/combobox_addnew.gif" alt="" border="0" style="cursor: hand"
                                                                    onclick="quickAddClient('hNotifyAcct','lstNotifyName','txtBillToInfo')" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <textarea id="txtBillToInfo" name="txtBillToInfo" class="monotextarea" cols="" rows="5"
                                                        style="width: 300px"><%=vAccountInfo %></textarea>
                                                    <!-- End JPED -->
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                    <td height="18" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>FAA Indirect Air Carrier No. </strong>
                                    </td>
                                    <td height="18" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>Known Shippers </strong>
                                    </td>
                                    <td height="18" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>File No.</strong>
                                    </td>
                                </tr>
                                <tr align="left" valign="top" bgcolor="#FFFFFF">
                                    <td style="height: 18px">
                                        <input type="text" name="txtIACNum" class="shorttextfield" size="24" value="<%=vIACNum %>"
                                            id="txtIACNum" />
                                    </td>
                                    <td style="height: 18px">
                                        <input type="text" name="txtKnownShipper" class="shorttextfield" size="24" value="<%=vKnownShipper %>"
                                            id="txtKnownShipper" />
                                    </td>
                                    <td bgcolor="#FFFFFF" style="height: 18px">
                                        <input type="text" name="txtReferenceNumber" class="d_shorttextfield" size="27" value="<%=ConvertAnyValue(vReferenceNumber,"String",GetFileNumber(vMAWB))%>"
                                            readonly="readonly" />
                                    </td>
                                </tr>
                                <tr align="left" valign="top" bgcolor="#FFFFFF">
                                    <td height="18" valign="middle" bgcolor="#f3f3f3">
                                        <span class="bodyheader">Unknown Shippers </span>
                                    </td>
                                    <td valign="middle" bgcolor="#f3f3f3" class="bodyheader">
                                        <strong>Items Under 16 Ounces</strong>
                                    </td>
                                    <td>
                                        <input type="hidden" name="txtAgentIATACode" class="shorttextfield" value="<%= vAgentIATACode %>"
                                            size="17" maxlength="17">
                                        <input type="hidden" name="txtAgentAcct" class="shorttextfield" value="<%= vAgentAcct %>"
                                            size="17" maxlength="17">
                                        <textarea name="txtAgentInfo" cols="40" rows="3" wrap="hard" class="hiddenfield"
                                            onkeypress="return checkMaxRows(this);"><%= vAgentInfo %></textarea>
                                        <input type="hidden" name="txtDeclaredValueCustoms" class="shorttextfield" tabindex="11"
                                            value="<%= vDeclaredValueCustoms %>" size="12">
                                        <input type="hidden" name="txtInsuranceAmt" class="shorttextfield" value="<%= vInsuranceAMT %>"
                                            size="17" maxlength="17">
                                        <input type="hidden" name="txtSCI" class="shorttextfield" value="<%= vSCI %>" size="14">
                                        <input type="hidden" name="txtDestCountry" class="shorttextfield" value="<%= vDestCountry %>"
                                            size="20">
                                        <img src='../Images/icon_rate_on.gif' name="chkDfRate" width="0" height="8" id="chkDfRate"
                                            style="visibility: hidden" onclick="getIATARate(<%= i %>)" onmousedown="src='../Images/icon_rate_off.gif'"
                                            onmouseup="src='../Images/icon_rate_on.gif'" />
                                        <textarea name="txtDesc1" cols="22" rows="9" wrap="hard" class="hiddenfield" tabindex="3"
                                            onkeydown="return checkMaxRows(this);"><%= vDesc1 %></textarea>
                                    </td>
                                </tr>
                                <tr align="left" valign="top" bgcolor="#FFFFFF">
                                    <td>
                                        <span class="bodyheader">
                                            <input type="text" name="txtUnKnownShipper" class="shorttextfield" size="24" value="<%=vUnKnownShipper %>"
                                                id="txtUnKnownShipper" />
                                        </span>
                                    </td>
                                    <td valign="middle" bgcolor="#FFFFFF">
                                        <input type="text" name="txtItemUnder16" class="shorttextfield" size="24" value="<%=vItemUnder16 %>"
                                            id="txtItemUnder16" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="left" valign="top" bgcolor="#FFFFFF">
                                    <td height="18" colspan="2" valign="middle" bgcolor="#f3f3f3">
                                        &nbsp;<strong>Airport of Departure (Addr. of First Carrier) and Requested Routing</strong>
                                    </td>
                                    <td valign="middle" bgcolor="#FFFFFF">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="left" valign="top" bgcolor="#FFFFFF">
                                    <td colspan="2" valign="middle">
                                        <input name="txtDepartureAirport" class="shorttextfield" value="<%= vDepartureAirport %>"
                                            size="35" maxlength="35">
                                    </td>
                                    <td valign="middle">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#FFFFFF">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                                <tr align="left" valign="middle">
                                    <td colspan="5" valign="top">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodycopy">
                                            <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" class="bodycopy">
                                                    Routing and Destination
                                                </td>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                                <td width="15%" height="20" bgcolor="#f3f3f3">
                                                    &nbsp;<strong>To</strong>
                                                </td>
                                                <td width="41%" bgcolor="#f3f3f3">
                                                    &nbsp;<strong>By First Carrier</strong>
                                                </td>
                                                <td width="11%" bgcolor="#f3f3f3">
                                                    To
                                                </td>
                                                <td width="11%" bgcolor="#f3f3f3">
                                                    By
                                                </td>
                                                <td width="11%" bgcolor="#f3f3f3">
                                                    To
                                                </td>
                                                <td width="11%" bgcolor="#f3f3f3">
                                                    By
                                                </td>
                                            </tr>
                                            <tr align="left" valign="middle">
                                                <td>
                                                    <input name="txtTo" class="shorttextfield" value="<%= vTo %>" size="12" maxlength="12">
                                                </td>
                                                <td>
                                                    <input name="txtBy" class="shorttextfield" value="<%= vBy %>" size="20" maxlength="20">
                                                </td>
                                                <td>
                                                    <input name="txtTo1" class="shorttextfield" value="<%= vTo1 %>" size="8" maxlength="8">
                                                </td>
                                                <td>
                                                    <input name="txtBy1" class="shorttextfield" value="<%= vBy1 %>" size="8" maxlength="8">
                                                </td>
                                                <td>
                                                    <input name="txtTo2" class="shorttextfield" value="<%= vTo2 %>" size="8" maxlength="8">
                                                </td>
                                                <td>
                                                    <input name="txtBy2" class="shorttextfield" value="<%= vBy2 %>" size="8" maxlength="8">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2" valign="top">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodycopy">
                                            <tr align="center" valign="middle">
                                                <td bgcolor="#f3d9a8" class="bodycopy">
                                                    &nbsp;
                                                </td>
                                                <td bgcolor="#f3d9a8">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2" bgcolor="#f3d9a8">
                                                    <strong>WT/VAL</strong>
                                                </td>
                                                <td colspan="2" bgcolor="#f3d9a8">
                                                    <strong>Other</strong>
                                                </td>
                                            </tr>
                                            <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                                <td bgcolor="#f3f3f3">
                                                    Currency
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    CHGS<br>
                                                    Code
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    PPD
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    COLL
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    PPD
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    COLL
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <select name="lstCurrency" size="1" class="smallselect">
                                                        <option>USD</option>
                                                    </select>
                                                </td>
                                                <td align="left" valign="middle">
                                                    <input name="txtChargeCode" class="shorttextfield" value="<%= vChargeCode %>" size="4"
                                                        maxlength="4">
                                                </td>
                                                <td align="center" valign="middle">
                                                    <input tabindex="7" name="cPPO1" type="checkbox" <% if vPPO_1="Y" Then response.write("checked") %>
                                                        onclick="cPPO1Click(this.checked)" value="<%= vPPO_1 %>">
                                                </td>
                                                <td align="center" valign="middle">
                                                    <input tabindex="8" type="checkbox" name="cCOLL1" value="<%= vCOLL_1 %>" <% if vCOLL_1="Y" Then response.write("checked") %>
                                                        onclick="cCOLL1Click(this.checked)">
                                                </td>
                                                <td align="center" valign="middle">
                                                    <input tabindex="9" name="cPPO2" type="checkbox" <% if vPPO_2="Y" Then response.write("checked") %>
                                                        value="Y" onclick="cPPO2Click(this.checked)">
                                                </td>
                                                <td align="center" valign="middle">
                                                    <input tabindex="10" type="checkbox" name="cCOLL2" value="Y" <% if vCOLL_2="Y" Then response.write("checked") %>
                                                        onclick="cCOLL2Click(this.checked)">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="27%" colspan="3" valign="top">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="1" class="bodycopy">
                                            <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                                <td width="50%" height="20" bgcolor="#f3f3f3" class="bodycopy">
                                                    <strong>Declared Value for Carriage</strong>
                                                </td>
                                            </tr>
                                            <tr align="left" valign="middle">
                                                <td>
                                                    <input name="txtDeclaredValueCarriage" class="shorttextfield" value="<%=vDeclaredValueCarriage %>"
                                                        size="18" id="txtDeclaredValueCarriage">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left" valign="middle">
                                    <td colspan="5" valign="top">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodycopy">
                                            <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2" align="center" bgcolor="#f3d9a8">
                                                    For Carrier Only
                                                </td>
                                            </tr>
                                            <tr align="left" valign="middle" bgcolor="#f3d9a8">
                                                <td height="20" bgcolor="#f3f3f3">
                                                    &nbsp;<strong>Airport of Destination</strong><strong></strong>
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    &nbsp;<strong>Flight/Date</strong>
                                                </td>
                                                <td bgcolor="#f3f3f3">
                                                    &nbsp;<strong>Flight/Date</strong>
                                                </td>
                                            </tr>
                                            <tr align="left" valign="middle">
                                                <td>
                                                    <input name="txtDestAirport" class="shorttextfield" value="<%= vDestAirport %>" size="24"
                                                        maxlength="24">
                                                </td>
                                                <td>
                                                    <input name="txtFlightDate1" class="shorttextfiel dated" preset="shortdate" value="<%= vFlightDate1 %>"
                                                        size="17" maxlength="17" />
                                                </td>
                                                <td>
                                                    <input name="txtFlightDate2" class="shorttextfield date" preset="shortdate" value="<%= vFlightDate2 %>"
                                                        size="17" maxlength="17" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2" valign="top">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodycopy">
                                            <tr>
                                                <td height="20" align="left" valign="middle" bgcolor="#f3f3f3">
                                                    <span class="style10">Service Level </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <input type="text" name="txtServiceLevel" class="shorttextfield" size="30" value="<%=vServiceLevel %>"
                                                        id="txtServiceLevel" style="background-color: #cccccc" readonly="readOnly" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="3" valign="top" class="bodycopy">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#FFFFFF">
                            <table width="100%" border="0" cellpadding="1" cellspacing="2" bgcolor="#FFFFFF"
                                class="bodycopy">
                                <tr>
                                    <td height="20" align="left" valign="middle" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>Handling Information </strong>(Maximum of 125 characters or 2 lines)
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50%" align="left" valign="middle">
                                        <textarea name="txtHandlingInfo" id="txtHandlingInfo" cols="70" rows="2" class="monotextarea"
                                            onkeyup="textLimit(this,127);" wrap="HARD"><%= vHandlingInfo %></textarea>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="left" valign="middle">
                        <td height="2" bgcolor="#997132">
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF"
                    class="collapsible">
                    <thead>
                        <tr align="center" valign="middle">
                            <th height="22" colspan="13" bgcolor="#f3d9a8">
                                <strong><a name="available_hawb"></a><span class="style6"><font color="c16b42">AVAILABLE
                                    HOUSE AWB NO. </font></span></strong>&nbsp;&nbsp;&nbsp;<img src="../Images/Expand.gif"
                                        width="10" height="7" border="0" id="toggleButton" style="cursor: hand" onclick="toggleVisibility('toggleButton')" />
                                <% if mode_begin then %>
                                <div style="width: 21px; display: inline; vertical-align: middle" onmouseover="showtip('Clicking on the arrow will open up the area containing all HAWB in the system that are not consolidated to a MAWB.  If you wish to add a HAWB to this Master, find it and click Add on its line.  The flight info will be pushed automatically to the House from the Master.')"
                                    onmouseout="hidetip()">
                                    <img src="../Images/button_info.gif" align="middle" class="bodylistheader"></div>
                                <% end if %>
                            </th>
                        </tr>
                        <tr align="left" valign="middle">
                            <th height="1" colspan="13" bgcolor="#FFFFFF">
                            </th>
                        </tr>
                        <tr>
                            <th width="1" bgcolor="#f3d9a8">
                            </th>
                            <th width="125" height="20" align="left" bgcolor="#f3f3f3">
                                <strong>House AWB No.</strong>
                            </th>
                            <th width="84" bgcolor="#f3f3f3">
                                <strong>Coloadee</strong>
                            </th>
                            <th width="196" bgcolor="#f3f3f3">
                                <strong>Agent</strong>
                            </th>
                            <th width="172" bgcolor="#f3f3f3">
                                <strong>Shipper</strong>
                            </th>
                            <th width="144" bgcolor="#f3f3f3">
                                <strong>Consignee</strong>
                            </th>
                            <th width="35" bgcolor="#f3f3f3">
                                <strong>Pieces</strong>
                            </th>
                            <th width="37" bgcolor="#f3f3f3">
                                <strong>GW</strong>
                            </th>
                            <th width="39" bgcolor="#f3f3f3">
                                <strong>AW</strong>
                            </th>
                            <th width="37" bgcolor="#f3f3f3">
                                <strong>DW</strong>
                            </th>
                            <th width="37" bgcolor="#f3f3f3">
                                <strong>CW</strong>
                            </th>
                            <th bgcolor="#f3f3f3">
                                &nbsp;
                            </th>
                            <th bgcolor="#f3f3f3">
                                &nbsp;
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <% for j=0 to aCount-1 %>
                        <tr align="left" valign="middle">
                            <td>
                            </td>
                            <td>
                                <input name="txtaHAWB<%= j %>" type="text" class="d_shorttextfield" readonly value="<%= aHAWB(j) %>"
                                    size="13">
                            </td>
                            <td>
                                <input name="txtaCOLO<%= j %>" type="text" class="d_shorttextfield" readonly value="<%= aCOLO(j) %>"
                                    size="10">
                            </td>
                            <td>
                                <input name="txtaAgent<%= j %>" type="text" class="d_shorttextfield" readonly value="<%= aAgent(j) %>"
                                    size="24">
                            </td>
                            <td>
                                <input name="txtaShipper<%= j %>" type="text" class="d_shorttextfield" readonly value="<%= aShipper(j) %>"
                                    size="21">
                            </td>
                            <td>
                                <input name="txtaConsignee<%= j %>" type="text" class="d_shorttextfield" readonly
                                    value="<%= aConsignee(j) %>" size="21">
                            </td>
                            <td>
                                <input name="txtaPcs<%= j %>" type="text" class="readonlyright" readonly value="<%= aPCS(j) %>"
                                    size="4">
                            </td>
                            <td>
                                <input name="txtaGW<%= j %>" type="text" class="readonlyright" readonly value="<%= aGW(j) %>"
                                    size="7">
                            </td>
                            <td>
                                <input name="txtaAW<%= j %>" type="text" class="readonlyright" readonly value="<%= aAW(j) %>"
                                    size="7">
                            </td>
                            <td>
                                <input name="txtaDW<%= j %>" type="text" class="readonlyright" readonly value="<%= aDW(j) %>"
                                    size="7">
                            </td>
                            <td>
                                <input name="txtaCW<%= j %>" type="text" class="readonlyright" readonly value="<%= aCW(j) %>"
                                    size="7">
                            </td>
                            <td width="1" align="left">
                                &nbsp;
                            </td>
                            <% if not aHAWB(j)="" then %>
                            <td width="252" align="left">
                                <img src="../images/button_add.gif" width="37" height="17" onclick="AddToHAWB('<%= aHAWB(j) %>',<%= AddELTAcct(j) %>)"
                                    style="cursor: hand">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="../images/button_edit.gif"
                                        width="37" height="18" onclick="EditHAWB('<%= aHAWB(j) %>','<%= AddELTAcct(j) %>')"
                                        style="cursor: hand">
                            </td>
                            <% end if %>
                        </tr>
                        <% next %>
                    </tbody>
                </table>
                <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF"
                    class="bodycopy">
                    <tr align="left" valign="middle">
                        <td height="1" colspan="12" bgcolor="#997132">
                        </td>
                    </tr>
                    <tr align="center" valign="middle">
                        <td height="22" colspan="12" bgcolor="#f3d9a8">
                            <strong><a name="selected_hawb"></a><span class="style6"><font color="c16b42">SELECTED
                                HOUSE AWB NO.
                                <% if mode_begin then %>
                            </font></span></strong>
                            <div style="width: 21px; display: inline; vertical-align: middle" onmouseover="showtip('The House bills listed in this section are consolidated to this Master AWB.  You may click Remove to take them off of the consolidation.')"
                                onmouseout="hidetip()">
                                <img src="../Images/button_info.gif" align="middle" class="bodylistheader"></div>
                            <% end if %>
                            <strong><span class="style6"><font color="c16b42"></font></span></strong>
                        </td>
                    </tr>
                    <tr align="left" valign="middle">
                        <td height="1" colspan="12" bgcolor="#FFFFFF">
                        </td>
                    </tr>
                    <tr>
                        <td width="106" height="20" bgcolor="#f3f3f3">
                            <strong>House AWB No.</strong>
                        </td>
                        <td width="19" bgcolor="#f3f3f3">
                            &nbsp;
                        </td>
                        <td width="84" bgcolor="#f3f3f3">
                            <strong>Coloadee</strong>
                        </td>
                        <td width="138" bgcolor="#f3f3f3">
                            <strong>Agent</strong>
                        </td>
                        <td width="142" bgcolor="#f3f3f3">
                            <strong>Shipper</strong>
                        </td>
                        <td width="233" bgcolor="#f3f3f3">
                            <strong>Consignee</strong>
                        </td>
                        <td width="37" bgcolor="#f3f3f3">
                            <strong>Pieces</strong>
                        </td>
                        <td width="37" bgcolor="#f3f3f3">
                            <strong>GW</strong>
                        </td>
                        <td width="37" bgcolor="#f3f3f3">
                            <strong>AW</strong>
                        </td>
                        <td width="37" bgcolor="#f3f3f3">
                            <strong>DW</strong>
                        </td>
                        <td width="79" bgcolor="#f3f3f3">
                            <strong>CW</strong>
                        </td>
                        <td width="213" bgcolor="#f3f3f3">
                            &nbsp;
                        </td>
                    </tr>
                    <%'dim i%>
                    <% for i=0 to sCount-1 %>
                    <tr align="left" valign="middle">
                        <td>
                            <input name="txtsHAWB<%= i %>" type="text" class="d_shorttextfield" value="<%= sHAWB(i) %>"
                                size="19" readonly>
                        </td>
                        <%GET_SUB_HAWB_INFO(sHAWB(i))%>
                        <td align="center" <% if sIsMaster(i)="Y" then response.Write("style='visibility:visible'") else response.Write("style='visibility:hidden'") end if %>>
                            <img src="../Images/Expand.gif" width="10" height="7" border="0" <% if sHsCount > 0 then response.Write(" style='cursor:hand;visibility:visible'") else response.Write(" style='cursor:hand;visibility:hidden'") end if%>
                                onclick='toggleSub(this,"<%=sHAWB(i)%>")' />
                        </td>
                        <td>
                            <input name="txtsCOLO<%= i %>" type="text" class="d_shorttextfield" value="<%= sCOLO(i) %>"
                                size="12" readonly>
                        </td>
                        <td>
                            <input name="txtsAgent<%= i %>" type="text" class="d_shorttextfield" value="<%= sAgent(i) %>"
                                size="24" readonly>
                        </td>
                        <td>
                            <input name="txtsShipper<%= i %>" type="text" class="d_shorttextfield" value="<%= sShipper(i) %>"
                                size="21" readonly>
                        </td>
                        <td>
                            <input name="txtsConsignee<%= i %>" type="text" class="d_shorttextfield" value="<%= sConsignee(i) %>"
                                size="21" readonly><input name="txtWeightTran<%= i %>" type="hidden" value="<%= aWeightTran(i) %>">
                        </td>
                        <td>
                            <input name="txtsPcs<%= i %>" type="text" class="readonlyright" value="<%= sPCS(i) %>"
                                size="4" readonly>
                        </td>
                        <td>
                            <input name="txtsGW<%= i %>" type="text" class="readonlyright" value="<%= sGW(i) %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtsAW<%= i %>" type="text" <% if elt_account_number = cstr(delELTAcct(i)) then response.Write(" class='numberfield'") else  response.Write(" class='d_numberfield' readonly")  end if  %>
                                value="<%= sAW(i) %>" size="7">
                        </td>
                        <td>
                            <input name="txtsDW<%= i %>" type="text" class="readonlyright" value="<%= sDW(i) %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtsCW<%= i %>" type="text" class="readonlyright" value="<%= sCW(i) %>"
                                size="7" readonly>
                        </td>
                        <% if not sHAWB(i)="" then %>
                        <td width="213" align="left">
                            <img src="../images/button_edit.gif" width="37" height="18" onclick="EditHAWB('<%= sHAWB(i) %>','<%= delELTAcct(i) %>')"
                                style="cursor: hand">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="../images/button_remove.gif"
                                    width="55" height="17" onclick="DeleteHAWB('<%= sHAWB(i) %>','<%= delELTAcct(i) %>')"
                                    style="cursor: hand">
                        </td>
                        <% end if %>
                    </tr>
                    <%if sIsMaster(i)="Y" then %>
                    <% for k=0 to sHsCount-1 %>
                    <tr id="tr<%=sHAWB(i)&k%>" align="left" style="display: none" valign="middle">
                        <td bgcolor="#f3d9a8">
                            &nbsp;
                        </td>
                        <td bgcolor="#f3d9a8">
                            &nbsp;
                        </td>
                        <td bgcolor="#f3d9a8">
                            <strong>Sub House No.</strong>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input name="txtsSHShipper<%= k %>" type="text" class="d_shorttextfield" value="<%= sHsHAWB(k) %>"
                                size="21" readonly>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input name="txtsSHConsignee<%= k %>" type="text" class="d_shorttextfield" value="<%= sHsShipper(k) %>"
                                size="21" readonly>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input name="txtsConsignee<%= k %>" type="text" class="d_shorttextfield" value="<%= sHsConsignee(k) %>"
                                size="21" readonly>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input name="txtsPcs<%= k %>" type="text" class="readonlyright" value="<%= sHsPCS(k) %>"
                                size="4" readonly>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input type="text" class="readonlyright" value="<%= sHsGW(k) %>" size="7" readonly>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input type="text" class="readonlyright" value="<%= sHsAW(k) %>" size="7">
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input type="text" class="readonlyright" value="<%= sHsDW(k) %>" size="7" readonly>
                        </td>
                        <td bgcolor="#f3d9a8">
                            <input type="text" class="readonlyright" value="<%=sHsCW(k) %>" size="7" readonly>
                        </td>
                        <% if not sHsHAWB(k)="" then %>
                        <td bgcolor="#f3d9a8" width="213" align="left">
                            <img src="../images/button_edit.gif" width="37" height="18" onclick="EditHAWB('<%= sHsHAWB(k) %>','<%= sHdelELTAcct(k) %>')"
                                style="cursor: hand"><input type="hidden" id="hCount<%=sHAWB(i)%>" value="<%=sHsCount%>" />
                        </td>
                        <% end if %>
                    </tr>
                    <%next%>
                    <%end if%>
                    <% next %>
                    <tr align="left" valign="middle">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <strong>Total</strong>
                        </td>
                        <td>
                            <input name="txtTotalPCS" type="text" class="readonlyright" value="<%= sTotalPCS %>"
                                size="4" readonly>
                        </td>
                        <td>
                            <input name="txtTotalGW" type="text" class="readonlyright" value="<%= sTotalGW %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtTotalAW" type="text" class="readonlyright" value="<%= sTotalAW %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtTotalCW" type="text" class="readonlyright" value="<%= sTotalDW %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtTotalCW2" type="text" class="readonlyright" value="<%= sTotalCW %>"
                                size="7" readonly>
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="left" valign="middle">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <strong>%</strong>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input name="txtGWPercent" type="text" class="readonlyright" value="<%= GWPercent %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtAWPercent" type="text" class="readonlyright" value="<%= AWPercent %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <input name="txtDWPercent" type="text" class="readonlyright" value="<%= DWPercent %>"
                                size="7" readonly>
                        </td>
                        <td>
                            <% if  Not GWPercent  = 0 or  Not AWPercent  = 0 or  Not DWPercent  = 0 then %>
                            <input name="txtCWPercent" type="text" class="readonlyright" value="100" size="7"
                                readonly>
                            <% end if %>
                        </td>
                        <td align="left" valign="middle">
                            <%if   sCount > 0 then%>
                            <img src="../images/button_adjust.gif" width="51" height="18" onclick="AdjustWeight(<%= sCount %>)"
                                style="cursor: hand">
                            <%end if%>
                        </td>
                    </tr>
                    <tr align="left" valign="middle">
                        <td height="1" colspan="12" bgcolor="#997132">
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                    <tr align="left" valign="middle" bgcolor="#f3d9a8">
                        <td height="22" colspan="9" align="center" valign="middle" bgcolor="#f3d9a8">
                            <input type="hidden" id="Pieces">
                            <input type="hidden" id="KgLb">
                            <input type="hidden" id="GrossWeight">
                            <input type="hidden" id="ChargeableWeight">
                            <input type="hidden" id="RateCharge">
                            <input type="hidden" id="TotalCharge">
                            <span class="style6"><strong><font color="c16b42">WEIGHT CHARGE</font></strong></span>
                        </td>
                    </tr>
                    <tr align="left" valign="middle">
                        <td height="1" colspan="11" bgcolor="#FFFFFF">
                        </td>
                    </tr>
                    <tr align="left" valign="middle" bgcolor="#f3d9a8" class="bodyheader">
                        <td width="114" height="20" bgcolor="#f3f3f3">
                            No of pieces
                        </td>
                        <td width="114" bgcolor="#f3f3f3">
                            Gross Weight
                        </td>
                        <td width="91" bgcolor="#f3f3f3">
                            KG/LB
                        </td>
                        <td width="114" bgcolor="#f3f3f3">
                            Rate Class
                        </td>
                        <td width="161" bgcolor="#f3f3f3">
                            Commodity Item No.
                        </td>
                        <td width="105" bgcolor="#f3f3f3">
                            Chargeable Weight
                        </td>
                        <td width="102" bgcolor="#f3f3f3">
                            Rate/Charge
                        </td>
                        <td width="62" bgcolor="#f3f3f3">
                            &nbsp;
                        </td>
                        <td width="171" align="left" bgcolor="#f3f3f3">
                            Total
                        </td>
                    </tr>
                    <% for i=0 to wCount-1 %>
                    <tr align="left" valign="middle">
                        <td>
                            <input name="txtPiece<%= i %>" class="shorttextfield Pieces" id="Pieces" value="<%= aPiece(i) %>"
                                size="5" maxlength="4" style="behavior: url(../include/igNumDotChkLeft.htc)">
                        </td>
                        <td>
                            <input name="txtGrossWeight<%= i %>" class="shorttextfield GrossWeight" id="GrossWeight" value="<%=aGrossWeight(i)%>"
                                size="10" maxlength="7" style="behavior: url(../include/igNumDotChkLeft.htc)">
                        </td>
                        <td>
                            <select id="select" size="1" class="smallselect" name="lstKgLb<%= i %>" onchange="ScaleChange(<%= i %>)">
                                <option value="L">lb</option>
                                <option value="K" <% if aKgLb(0)="K" Then response.write("selected") %>>kg</option>
                            </select>
                        </td>
                        <td>
                            <input name="txtRateClass<%= i %>" class="shorttextfield" value="<%=aRateClass(i)%>"
                                size="6" maxlength="1">
                        </td>
                        <td>
                            <input name="txtItemNo<%= i %>" class="shorttextfield" value="<%=aItemNo(i)%>" size="12"
                                maxlength="7">
                        </td>
                        <td>
                            <input name="txtChargeableWeight<%= i %>" class="shorttextfield ChargeableWeight" id="ChargeableWeight"
                                value="<%= aChargeableWeight(i) %>" size="12" maxlength="7" style="behavior: url(../include/igNumDotChkLeft.htc)">
                        </td>
                        <td>
                            <input name="txtRateCharge<%= i %>" class="shorttextfield RateCharge" id="RateCharge" value="<% if aRateCharge(i)="0" then response.Write("N/A")else response.Write(aRateCharge(i))end if  %>"
                                size="12" maxlength="8" style="behavior: url(../include/igNumDotChkLeft.htc)">
                        </td>
                        <td>
                            <img src="../images/button_cal.gif" width="37" height="18" name="bCal<%= i %>" onclick="bCalClick(<%= i %>)"
                                value="Cal" style="cursor: hand">
                        </td>
                        <td width="171" align="left">
                            <input name="txtTotal<%= i %>" class="shorttextfield TotalCharge" id="TotalCharge" value="<%=formatNumber(aTotal(i),2) %>"
                                size="16" maxlength="12" style="behavior: url(../include/igNumDotChkLeft.htc)">
                        </td>
                    </tr>
                    <% next %>
                </table>
                <table width="100%" border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF"
                    class="bodycopy">
                    <tr align="left" valign="middle">
                        <td height="1" colspan="12" bgcolor="#FFFFFF">
                        </td>
                    </tr>
                    <tr bgcolor="#ffffff">
                        <td height="1" colspan="12">
                        </td>
                    </tr>
                    <tr bgcolor="#f3d9a8">
                        <td width="17%" height="20" colspan="1" bgcolor="f3f3f3">
                            <strong>Nature and Quantity of Goods</strong>
                        </td>
                        <td width="80%" height="20" bgcolor="f3f3f3">
                            &nbsp;
                        </td>
                        <td width="3%" colspan="10" bgcolor="#f3f3f3">
                        </td>
                    </tr>
                    <tr align="left" valign="top" bgcolor="#FFFFFF">
                        <td bgcolor="f3f3f3" height="20" colspan="1">
                            <textarea name="txtDesc2" cols="34" rows="14" wrap="hard" class="monotextarea" tabindex="3"
                                onkeyup="Desc2KeyUp()"><%= vDesc2 %></textarea>
                        </td>
                        <td bgcolor="f3f3f3" height="20" colspan="1">
                            &nbsp;
                        </td>
                        <td height="20" colspan="10" bgcolor="f3f3f3">
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    class="bodycopy">
                    <input type="hidden" id="ChargeItem">
                    <input type="hidden" id="ChargeVendor">
                    <input type="hidden" id="ChargeAmt">
                    <input type="hidden" id="ChargeCost">
                    <input type="hidden" id="ItemDesc">
                    <tr align="left" valign="middle">
                        <td height="1" colspan="10" bgcolor="#997132">
                        </td>
                    </tr>
                    <tr align="center" valign="middle" bgcolor="eec983">
                        <td height="22" colspan="10" bgcolor="#f3d9a8">
                            <strong><a name="add_oc"></a><font color="c16b42">OTHER CHARGE</font></strong>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodycopy" id="OtherCharge">
                                <tr bgcolor="#f3d9a8">
                                    <td width="98" height="20" bgcolor="#f3f3f3">
                                        <strong>Carrier/Agent</strong>
                                    </td>
                                    <td width="98" bgcolor="#f3f3f3">
                                        <strong>Collect/Prepaid</strong>
                                    </td>
                                    <td width="270" bgcolor="#f3f3f3">
                                        <strong>Charge Item</strong>
                                    </td>
                                    <td width="276" bgcolor="#f3f3f3">
                                        <strong>Description</strong>
                                    </td>
                                    <td width="151" bgcolor="#f3f3f3">
                                        <strong>Charge Amount</strong>
                                    </td>
                                    <td width="" bgcolor="#f3f3f3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <% for i=0 to oIndex-1 %>
                                <tr id="aRow<%=i %>">
                                    <td>
                                        <select name="lstCarrierAgent<%= i %>" size="1" style="width: 40px" tabindex="<%= i*7+4 %>"
                                            class="smallselect">
                                            <option value="C" selected>C</option>
                                            <option value="A" <% if aCarrierAgent(i)="A" Then response.write("selected") %>>A</option>
                                        </select>
                                    </td>
                                    <td>
                                        <select name="lstCollectPrepaid<%= i %>" size="1" style="width: 40px" tabindex="<%= i*7+5 %>"
                                            class="smallselect">
                                            <option value="P" selected>P</option>
                                            <option value="C" <% if aCollectPrepaid(i)="C" Then response.write("selected") %>>C</option>
                                        </select>
                                    </td>
                                    <td>
                                        <select tabindex="<%= i*7+6 %>" id="ChargeItem" size="1" name="lstChargeCode<%= i %>"
                                            onchange="ChargeItemChange(<%= i %>)" class="smallselect ChargeItem" style="width: 270px">
                                            <% for j=0 to chIndex-1 %>
                                            <option value="<%= aChargeItemNo(j) & "-" & aChargeItemDesc(j)  & "-" & aChargeUnitPrice(j) %>"
                                                <% if cInt(aChargeCode(i))=aChargeItemNo(j) then response.write("selected") %>>
                                                <%= aChargeItemNameig(j) %>
                                            </option>
                                            <% next %>
                                        </select>
                                    </td>
                                    <td>
                                        <input tabindex="<%= i*7+7 %>" id="ItemDesc" name="txtChargeDesc<%= i %>" size="45"
                                            value="<%= aDesc(i) %>" class="shorttextfield ItemDesc">
                                    </td>
                                    <td>
                                        <input tabindex="<%= i*7+8 %>" id="ChargeAmt" name="txtChargeAmt<%= i %>" size="12"
                                            value="<%if isnumeric(aChargeAmt(i)) then response.Write(formatnumber(aChargeAmt(i),2))else response.Write(formatnumber(aChargeAmt(i),2)) end if   %>"
                                            class="numberalign ChargeAmt" style="behavior: url(../include/igNumDotChkLeft.htc)">
                                    </td>
                                    <!--            <td width="224" align="left" valign="middle"><img src="../images/button_delete.gif" width="50" height="17" onClick="removeTableRow(<%= i %>)"  style="cursor:hand"></td> -->
                                    <td width="224" align="left" valign="middle">
                                        <img src="../images/button_delete.gif" width="50" height="17" onclick="DeleteOC(<%= i %>)"
                                            style="cursor: hand">
                                    </td>
                                </tr>
                                <% next %>
                            </table>
                        </td>
                    </tr>
                    <tr align="right" bgcolor="f3f3f3">
                        <td colspan="5" align="left" bgcolor="f3f3f3">
                            <img src="../images/button_addcharge.gif" width="113" height="18" tabindex="<%= i*7+11 %>"
                                type="button" name="bAddOC" onclick="AddOC()" style="cursor: hand">
                        </td>
                        <!--            <td width="30%"><img src="../images/button_addcharge.gif" width="113" height="18" tabindex=<%= i*7+11 %> type="button" name="bAddOC" onClick="addTableRow()"  style="cursor:hand"></td> -->
                        <td width="21%" align="left">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    class="bodycopy">
                    <tr align="left" valign="middle">
                        <td height="1" colspan="10" bgcolor="#997132">
                        </td>
                    </tr>
                    <tr align="center" valign="middle">
                        <td width="50%" height="22" align="left" valign="top" bgcolor="#FFFFFF">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodycopy">
                                <tr align="left" valign="middle" bgcolor="eec983">
                                    <td width="50%" height="20" align="center" bgcolor="#f3d9a8">
                                        <strong>Prepaid</strong>
                                    </td>
                                    <td width="50%" align="center" bgcolor="#f3d9a8">
                                        <strong>Collect</strong>
                                    </td>
                                </tr>
                                <tr align="center" valign="middle">
                                    <td colspan="2" bgcolor="#f3f3f3">
                                        Weight Charge
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtPrepaidWeightCharge" class="readonlyright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vPrepaidWeightCharge) then response.Write(formatnumber(vPrepaidWeightCharge,2))else response.Write(vPrepaidWeightCharge) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCollectWeightCharge" class="readonlyright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vCollectWeightCharge) then response.Write(formatnumber(vCollectWeightCharge,2))else response.Write(vCollectWeightCharge) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                                <tr align="center" valign="middle">
                                    <td colspan="2" bgcolor="#f3f3f3">
                                        Valuation Charge
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtPrepaidValuationCharge" class="readonlyright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vPrepaidValuationCharge) then response.Write(formatnumber(vPrepaidValuationCharge,2))else response.Write(vPrepaidValuationCharge) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCollectValuationCharge" class="readonlyright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vCollectValuationCharge) then response.Write(formatnumber(vCollectValuationCharge,2))else response.Write(vCollectValuationCharge) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                                <tr align="center" valign="middle">
                                    <td colspan="2" bgcolor="#f3f3f3">
                                        Tax
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtPrepaidTax" class="readonlyright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vPrepaidTax) then response.Write(formatnumber(vPrepaidTax,2))else response.Write(vPrepaidTax) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCollectTax" class="readonlyright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vCollectTax) then response.Write(formatnumber(vCollectTax,2))else response.Write(vCollectTax) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                                <tr align="center" valign="middle">
                                    <td colspan="2" bgcolor="#f3f3f3">
                                        Total Other Charges Due Agent
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtPrepaidOtherChargeAgent" class="readonlyright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vPrepaidOtherChargeAgent) then response.Write(formatnumber(vPrepaidOtherChargeAgent,2))else response.Write(vPrepaidOtherChargeAgent) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCollectOtherChargeAgent" class="readonlyright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vCollectOtherChargeAgent) then response.Write(formatnumber(vCollectOtherChargeAgent,2))else response.Write(vCollectOtherChargeAgent) end if  %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                                <tr align="center" valign="middle">
                                    <td colspan="2" bgcolor="#f3f3f3">
                                        Total Other Charges Due Carrier
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtPrepaidOtherChargeCarrier" class="readonlyright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vPrepaidOtherChargeCarrier) then response.Write(formatnumber(vPrepaidOtherChargeCarrier,2))else response.Write(vPrepaidOtherChargeCarrier) end if  %>"
                                            size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCollectOtherChargeCarrier" class="readonlyright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vCollectOtherChargeCarrier) then response.Write(formatnumber(vCollectOtherChargeCarrier,2))else response.Write(vCollectOtherChargeCarrier) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td height="20" bgcolor="#f3f3f3" class="bodyheader style11">
                                        C.O.D.
                                    </td>
                                    <td bgcolor="#f3f3f3">
                                        <input name="txtCODAmount" class="shorttextfield" value="<%=FormatNumberPlus(ConvertAnyValue(vCODAmount,"Amount",0),2) %>"
                                            size="14" maxlength="14" id="txtCODAmount" style="behavior: url(../include/igNumDotChkLeft.htc);
                                            width: 70px; text-align: right" />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td height="20" bgcolor="#f3d9a8">
                                        <strong>Total Prepaid</strong>
                                    </td>
                                    <td bgcolor="#f3d9a8">
                                        <strong>Total Collect</strong>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtPrepaidTotal" class="readonlyboldright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vPrepaidTotal) then response.Write(formatnumber(vPrepaidTotal,2))else response.Write(vPrepaidTotal) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCollectTotal" class="readonlyboldright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vCollectTotal) then response.Write(formatnumber(vCollectTotal,2))else response.Write(vCollectTotal) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td height="20" bgcolor="#f3f3f3">
                                        Currency Conversion Rates
                                    </td>
                                    <td height="20" align="center" bgcolor="#f3f3f3">
                                        CC Charges in Dest. Currency
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <input name="txtConversionRate" class="readonlyright" readonly="readonly" style="width: 70px"
                                            value="<%=vConversionRate   %>" size="14" maxlength="14">
                                    </td>
                                    <td>
                                        <input name="txtCCCharge" class="readonlyright" readonly="readonly" style="width: 70px"
                                            value="<%if isnumeric(vCCCharge) then response.Write(formatnumber(vCCCharge,2))else response.Write(formatnumber(vCCCharge,2)) end if   %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%" valign="top" bgcolor="ffffff">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                class="bodycopy">
                                <tr bgcolor="#f3d9a8">
                                    <td height="20" colspan="4" align="center" bgcolor="#f3d9a8" class="bodyheader style15">
                                        Pickup Zone
                                    </td>
                                </tr>
                                <tr class="bodyheader">
                                    <td height="20" bgcolor="#f3f3f3">
                                        Pickup Charges
                                    </td>
                                    <td height="20" bgcolor="#f3f3f3">
                                        Origin ADV. CHGS.
                                    </td>
                                    <td height="20" bgcolor="#f3f3f3">
                                        Description of Origin Advance
                                    </td>
                                    <td height="20" bgcolor="#f3f3f3">
                                        Item Prepaid
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="text" name="txtPickupCharge" class="shorttextfield" size="16" value="<%=formatNumberPlus(ConvertAnyValue(vPickupCharge,"Amount",0),2) %>"
                                            id="txtPickupCharge" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                    <td>
                                        <input type="text" name="txtOriginAdvCharge" class="shorttextfield" size="16" value="<%=formatNumberPlus(ConvertAnyValue(vOriginAdvCharge,"Amount",0),2) %>"
                                            id="txtOriginAdvCharge" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                    <td>
                                        <input type="text" name="txtOriginAdvChargeDesc" class="shorttextfield" size="32"
                                            value="<%=vOriginAdvChargeDesc %>" id="txtOriginAdvChargeDesc" />
                                    </td>
                                    <td>
                                        <input type="text" name="txtItemPrepaid" class="shorttextfield" size="12" value="<%=vItemPrepaid %>"
                                            id="txtItemPrepaid" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                </tr>
                                <tr class="bodyheader">
                                    <td height="20" colspan="4" align="center" bgcolor="#f3d9a8" class="bodyheader style15">
                                        Delivery Zone
                                    </td>
                                </tr>
                                <tr class="bodyheader">
                                    <td bgcolor="#f3f3f3">
                                        Delivery Charges
                                    </td>
                                    <td bgcolor="#f3f3f3">
                                        Dest. ADV. CHGS.
                                    </td>
                                    <td bgcolor="#f3f3f3">
                                        Description of Destination Advance
                                    </td>
                                    <td bgcolor="#f3f3f3">
                                        Item Collect
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="text" name="txtDeliveryCharge" class="shorttextfield" value="<%=formatNumberPlus(ConvertAnyValue(vDeliveryCharge,"Amount",0),2) %>"
                                            size="16" maxlength="16" id="txtDeliveryCharge" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                    <td>
                                        <input type="text" name="txtDestAdvCharge" class="shorttextfield" size="16" value="<%=formatNumberPlus(ConvertAnyValue(vDestAdvCharge,"Amount",0),2) %>"
                                            id="txtDestAdvCharge" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                    <td>
                                        <input type="text" name="txtDestAdvChargeDesc" class="shorttextfield" size="32" value="<%=vDestAdvChargeDesc %>"
                                            id="txtDestAdvChargeDesc" />
                                    </td>
                                    <td>
                                        <input type="text" name="txtItemCollect" class="shorttextfield" size="12" value="<%=vItemCollect %>"
                                            id="txtItemCollect" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                </tr>
                                <tr class="bodyheader">
                                    <td bgcolor="#f3f3f3">
                                        C.O.D. Fee
                                    </td>
                                    <td height="20" colspan="3" bgcolor="#f3f3f3">
                                        Other Charges and Description
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 20px">
                                        <input name="txtCODFee" type="text" class="shorttextfield" value="<%=formatNumberPlus(ConvertAnyValue(vCODFee,"Amount",0),2) %>"
                                            size="16" maxlength="16" id="txtCODFee" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                    <td colspan="3" style="height: 20px">
                                        <input type="text" name="txtOtherCharge" class="shorttextfield" size="71" value="<%=formatNumberPlus(ConvertAnyValue(vOtherCharge,"Amount",0),2) %>"
                                            id="txtOtherCharge" style="behavior: url(../include/igNumDotChkLeft.htc)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="50%">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="50%" height="20" align="center" bgcolor="#f3f3f3" class="bodycopy">
                                        For Carriers Use only at Destination
                                    </td>
                                    <td width="50%" align="center" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>Charges at Destination</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bodycopy">
                                        <input type="hidden" name="txtOtherCharge1" colspan="2" class="readonly" value="<%if isnumeric(aOtherCharge(0)) then response.Write(formatnumber(aOtherCharge(0),2))else response.Write(aOtherCharge(0)) end if   %>"
                                            size="75" maxlength="45">
                                        <input type="hidden" name="txtOtherCharge2" colspan="2" class="readonly" value="<%if isnumeric(aOtherCharge(1)) then response.Write(formatnumber(aOtherCharge(1),2))else response.Write(aOtherCharge(1)) end if   %>"
                                            size="75" maxlength="45">
                                        <input type="hidden" name="txtOtherCharge3" colspan="2" class="readonly" value="<%if isnumeric(aOtherCharge(2) ) then response.Write(formatnumber(aOtherCharge(2) ,2))else response.Write(aOtherCharge(2) ) end if %>"
                                            size="75" maxlength="45">
                                        <input type="hidden" name="txtOtherCharge4" colspan="2" class="readonly" value="<%if isnumeric(aOtherCharge(3)) then response.Write(formatnumber(aOtherCharge(3),2))else response.Write(aOtherCharge(3)) end if   %>"
                                            size="75" maxlength="45">
                                        <input type="hidden" name="txtOtherCharge5" colspan="2" class="readonly" value="<%if isnumeric(aOtherCharge(4)) then response.Write(formatnumber(aOtherCharge(4),2))else response.Write(aOtherCharge(4)) end if   %>"
                                            size="75" maxlength="45">
                                        <textarea name="txtSignature" cols="50" rows="2" wrap="hard" class="hiddenfield"><%= vSignature %></textarea>
                                        <input type="hidden" name="txtEmpolyee" class="readonly" readonly="readonly" value="<%=GetUserFLName(user_id) %>" />
                                        <textarea name="txtExecute" cols="50" rows="3" wrap="hard" class="hiddenfield"><%= vExecute %></textarea>
                                    </td>
                                    <td align="center">
                                        <input name="txtChargeDestination" class="readonlyboldright" readonly="readonly"
                                            style="width: 70px" value="<%if isnumeric(vChargeDestination) then response.Write(formatnumber(vChargeDestination,2))else response.Write(vChargeDestination) end if  %>"
                                            size="14" maxlength="14">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="32%" height="20" bgcolor="#f3f3f3" class="bodycopy">
                                        <strong>Total Collect Charges</strong>
                                    </td>
                                    <td width="68%" bgcolor="#FFFFFF" class="bodycopy">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bodycopy">
                                        <input name="txtFinalCollect" class="readonlyboldright" value="<% if isnumeric(vFinalCollect) then response.Write(formatnumber(vFinalCollect,2))else response.Write(vFinalCollect) end if   %>"
                                            size="24" maxlength="24">
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="1" bgcolor="#997132">
                        </td>
                    </tr>
                    <tr align="left" valign="middle">
                        <td height="32" colspan="2" bgcolor="#f3f3f3" class="bodycopy">
                            <table width="100%" align="right">
                                <tr>
                                    <td class="bodycopy" align="right">
                                        <strong>Sales Person</strong>
                                        <select name="lstSalesRP" size="1" class="smallselect" style="width: 200px">
                                            <option value="none">Select One</option>
                                            <% For i=0 To SRIndex-1 %>
                                            <option value="<%= aSRName(i)%>" <%
  	                    if vSalesPerson = aSRName(i) then response.write("selected") %>>
                                                <%= aSRName(i) %>
                                            </option>
                                            <%  Next  %>
                                        </select>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="1" bgcolor="#997132">
                        </td>
                    </tr>
                    <tr align="center" valign="middle">
                        <td height="22" colspan="2" bgcolor="eec983">
                            <table width="98%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="26%">
                                        &nbsp;
                                    </td>
                                    <td width="49%" align="center" valign="middle">
                                        <img src='../images/button_save_medium.gif' name='bSave' onclick="if (checkMismatch()){bsaveClick()}"
                                            style="cursor: hand">
                                    </td>
                                    <td width="13%" align="right" valign="middle">
                                        <a href="/ASP/domestic/new_edit_mawb.asp" target="_self">
                                            <img src="/ASP/Images/button_new.gif" width="42" height="17" border="0" style="cursor: hand"></a>
                                    </td>
                                    <td width="12%" align="right" valign="middle">
                                        <img src='../images/button_delete_medium.gif' width='51' height='17' name='bDeleteMAWB'
                                            onclick='DeleteMAWB()' style="cursor: hand">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="32" align="right" valign="bottom">
                <div id="print">
                    <img src="/ASP/Images/icon_printer_preview.gif" width="45" height="29" align="absbottom"><a
                    href="javascript:void(0);" id="NewPrintVeiw1" >Air Waybill</a></div>
            </td>
        </tr>
    </table>
    </form>
    <%

    %>
</body>

<script language="javascript">
    function trim(sourceString) { return sourceString.replace(/(?:^\s+|\s+$)/ig, ""); }
    function bCloseClick() {
        var mawb = '<%=vMAWB%>';
        if (mawb == '') {
            alert('Please select a Master AWB No.');
            return false;
        }
        if (!confirm("Do you really want to close this Master AWB No. '" + mawb + "' ?")) { return false; }
        if (close_mawb(mawb)) {
            alert("Master AWB No. : " + mawb + " was closed successfully.");
            window.location = "new_edit_mawb.asp";
        } else {
            alert("Some error was occured when closing.");
            return false;
        }
    }

    function close_mawb(mawb) {
        if (window.ActiveXObject) {
            try {
                xmlHTTP = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
                try {
                    xmlHTTP = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (f) {
                    return true;
                }
            }
        } else if (window.XMLHttpRequest) {
            xmlHTTP = new XMLHttpRequest();
        } else { return true; }

        var url = "/ASP/ajaxFunctions/ajax_mawb_close.asp"
				+ "?n=" + mawb;

        try {
            xmlHTTP.open("get", url, false);
            xmlHTTP.send();
            var sourceCode = xmlHTTP.responseText;
            if (sourceCode) {
                //			alert(sourceCode);					
                if (trim(sourceCode) == 'ok') {
                    return true;
                }
                else {
                    switch (trim(sourceCode)) {
                        default:
                            break;
                    }
                    return false;
                }
            }

        } catch (e) { return false; }
    }
</script>
<script language="javascript" type="text/javascript" src="/ASP/ajaxFunctions/ajax_ig_call_db.js">  </script>
<script language="javascript">
    function search_mawb(jobNo) {
        if (window.ActiveXObject) {
            try {
                xmlHTTP = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
                try {
                    xmlHTTP = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (f) {
                    return true;
                }
            }
        } else if (window.XMLHttpRequest) {
            xmlHTTP = new XMLHttpRequest();
        } else { return true; }

        var url = "/ASP/ajaxFunctions/ajax_mawb_search.asp" + "?j=" + jobNo;

        try {
            xmlHTTP.open("get", url, false);
            xmlHTTP.send();
            var sourceCode = xmlHTTP.responseText;
            if (sourceCode) {
                if (sourceCode == 'no') {
                    return false;
                } else {
                    return sourceCode;
                }
            }
        } catch (e) { return false; }
    }
</script>
<script language="vbscript">

/////////////////////////////
Sub Lookup()    'never used
/////////////////////////////
DIM mIndex, existMAWB,MAWB
 mIndex = "<%=mIndex%>"
	MAWB=UCase(document.frmMAWB.txtSMAWB.value)

	if NOT TRIM(MAWB) = "" then
		existMAWB = false
		For i=0 to mIndex
			if MAWB = UCase(document.frmMAWB.lstMAWB.item(i).text) then
				existMAWB = true
				exit for
			end if
		Next	
		
		if existMAWB then
				fillMAWBInfo(i)
				document.frmMAWB.action="new_edit_mawb.asp?Edit=yes&mawb=" & document.frmMAWB.txtSMAWB.value  & "&WindowName=" & window.name
				document.frmMAWB.method="POST"
				document.frmMAWB.target = "_self"
				document.frmMAWB.txtsMAWB.value = ""
				frmMAWB.submit()
		else
				msgbox "MAWB # " & MAWB & " does not exist."
		end if		
	END IF
End Sub
</script>
<script type="text/javascript">
function lookupFile() {
    var mIndex, existMJob, JobNo, mFile,last_Chr10_Index,MAWB,fileno;
	var JobNo = document.frmMAWB.txtJobNum.value.toUpperCase();
    var fileno = document.frmMAWB.txtJobNum.value;
	 if (JobNo.trim() != "" && fileno != "Search Here") {
	        existMJob = false;
		    MAWB = search_mawb(JobNo);
		    if (MAWB != false)
                existMJob = true;
		
		    if (existMJob) {
                    document.frmMAWB.txtJobNum.value = "";
                    document.frmMAWB.action = "new_edit_mawb.asp?Edit=yes&mawb="
    +encodeURIComponent(MAWB) + "&WindowName=" + window.name;
                    document.frmMAWB.method = "POST";
                    document.frmMAWB.target = "_self";
                    frmMAWB.submit();
                }
		    else
                    alert("File# " + JobNo + " does not exist.");
		}
        else
            alert("Please enter a File No!");

    }

    function bsaveClick() {
    var To0=document.frmMAWB.txtTo.value;
    var To1=document.frmMAWB.txtTo1.value;
    var To2=document.frmMAWB.txtTo2.value;
    if (To0.length>3 || To1.length >3 || To2.length >3 ){
	    alert( "The To Port has three characters!");
	    return;
    }

    var sindex=document.frmMAWB.lstMAWB.selectedIndex;

    if (sindex == 0 ){
       alert( "Please select a MAWB");
        return;
    }
    var OK=true;

    if ( document.frmMAWB.hNoItemWC.value != "") {
        var WC=parseInt(document.frmMAWB.hNoItemWC.value);
        var OC = parseInt(document.frmMAWB.hNoItemOC.value);
    
        if (sindex>0 ){
	        for (var i=0 ;i< WC; i++){
		        if (!IsNumeric($("input.Pieces").get(i).value)) {
			        alert( "Please enter a Numeric Value for PIECEs!");
			        OK=false;
			        return false;
                }
		        else if (!IsNumeric($("input.GrossWeight").get(i).value)) {
			        alert( "Please enter a Numeric Value for GROSS WEIGHT!");
			        OK=false;
			        return false;
		        }
		        else if (!IsNumeric($("input.ChargeableWeight").get(i).value)) {
			        alert( "Please enter a Numeric Value for CHARGEABLE WEIGHT!");
			        OK=false;
			        return false;
		        }
		        else if ($("input.RateCharge").get(i).value!="" 
                        && !IsNumeric($("input.RateCharge").get(i).value)) {
			        if ($("input.RateCharge").get(i).value!="N/A" ) 
				        $("input.RateCharge").get(i).value=0;
			        else{
				        alert( "Please enter a Numeric Value for RATE/CHARGE!");
				        OK=false;
			            return false;
			        }
		        }
	        }
	        if (OK) {
		        for (var i=0 ;i< OC;i++){
			        var cAmt=$("input.ChargeAmt").get(i).value;
			        if ( cAmt!="" && !IsNumeric(cAmt)){
				        alert( "Please enter a Numeric Value for CHARGE AMT!");
				        OK=false;
			           return false;
			        }
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtPrepaidValuationCharge.value!="" 
                    && !IsNumeric(document.frmMAWB.txtPrepaidValuationCharge.value) ){
			        alert ("Please enter a Numeric Value for VALUATION CHARGE!");
			        OK=false;
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtCollectValuationCharge.value!="" 
                    && !IsNumeric(document.frmMAWB.txtCollectValuationCharge.value)){
			        alert ("Please enter a Numeric Value for VALUATION CHARGE!");
			        OK=false;
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtPrepaidTax.value!="" 
                    && !IsNumeric(document.frmMAWB.txtPrepaidTax.value)){
			        alert( "Please enter a Numeric Value for TAX!");
			        OK=false;
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtCollectTax.value!="" 
                    && !IsNumeric(document.frmMAWB.txtCollectTax.value)){
			        alert( "Please enter a Numeric Value for TAX!");
			        OK=false;
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtConversionRate.value!="" 
                    && !IsNumeric(document.frmMAWB.txtConversionRate.value) ){
		            alert("Please enter a Numeric Value for CONVERSION RATE!");
			        OK=false;
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtCCCharge.value!="" && !IsNumeric(document.frmMAWB.txtCCCharge.value) ){
			        alert( "Please enter a Numeric Value for CC CHARGE!");
			        OK=false;
		        }
	        }
	        if (OK) {
		        if (document.frmMAWB.txtChargeDestination.value!="" 
                    && !IsNumeric(document.frmMAWB.txtChargeDestination.value)) {
			        alert("Please enter a Numeric Value for DESTINATION CHARGE!");
			        OK=false;
		        }
	        }
	        if (OK) {
                var sss=document.frmMAWB.lstMAWB.selectedIndex;
                var vMAWB=document.frmMAWB.lstMAWB.item(sss).text;

                if( !CHECK_IV_STATUS( vMAWB ) )
	                return false;

		        document.frmMAWB.action="new_edit_mawb.asp?Save=yes&MAWB=" 
		            + encodeURIComponent(vMAWB) + "&WindowName=" + window.name;
		        document.frmMAWB.method="POST";
		        document.frmMAWB.target = "_self";
		        frmMAWB.submit();
	        }
        }
        else
	        alert( "Please select a MAWB number!");
    }
}

</script>
<script language="vbscript">


Sub bMAWBClick()'never used
	MAWB=document.frmMAWB.txtMAWB.value
	document.frmMAWB.action="new_edit_mawb.asp?mawb=" & MAWB  & "&WindowName=" & window.name
	document.frmMAWB.method="POST"
	document.frmMAWB.target = "_self"
	frmMAWB.submit()
End Sub

Sub bShipperNameClick()'never used
	document.frmMAWB.action="new_edit_mawb.asp?sname=" & Document.frmMAWB.txtShipperName.Value  & "&WindowName=" & window.name
	document.frmMAWB.method="POST"
	document.frmMAWB.txtShipperInfo.Value=""
	document.frmMAWB.target = "_self"
	frmMAWB.submit()
End Sub
Sub bConsigneeNameClick()'never used
	document.frmMAWB.action="new_edit_mawb.asp?coname=" & Document.frmMAWB.txtConsigneeName.Value  & "&WindowName=" & window.name
	document.frmMAWB.method="POST"
	document.frmMAWB.txtConsigneeInfo.Value=""
	document.frmMAWB.target = "_self"
	frmMAWB.submit()
End Sub
Sub bNotifyClick()'never used
	document.frmMAWB.action="new_edit_mawb.asp?Notify=" & Document.frmMAWB.txtNotify.Value  & "&WindowName=" & window.name
	document.frmMAWB.method="POST"
	document.frmMAWB.target = "_self"
	frmMAWB.submit()
End Sub

Sub fillMAWBInfo(sindex)'never used
    vMAWB=Document.frmMAWB.lstMAWB.item(sindex).text
    mInfo=Document.frmMAWB.lstMAWB.item(sindex).value

    if not Document.frmMAWB.lstMAWB.item(sindex).text="Select One" then
	    Document.frmMAWB.hmawb_num.Value=Document.frmMAWB.lstMAWB.item(sindex).text
    else
	    Document.frmMAWB.hmawb_num.Value=""
    end if
    mCarrierDesc=""

    if not mInfo="" then

	    pos=InStr(mInfo,chr(10))
	    mAirOrgNum=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mDepartureAirportCode=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mDepartureAirport=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mTo=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mBy=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mTo1=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mBy1=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mTo2=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mBy2=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mDestAirport=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mFlightDate1=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mFlightDate2=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mCarrierDesc=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mExportDate=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mDestCountry=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	    pos=InStr(mInfo,chr(10))
	    mDepartureState=Left(mInfo,pos-1)
        mInfo=Mid(mInfo,pos+1,1000)
        pos=InStr(mInfo,chr(10))
        mFile=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
        pos=InStr(mInfo,chr(10))
        mServiceLevel=Left(mInfo,pos-1)
	    mInfo=Mid(mInfo,pos+1,1000)
	
	    AccountInfo=document.frmMAWB.txtBillToInfo.Value
	    pos=0
	    pos=instr(AccountInfo,"FILE#")
	    if pos>0 then
		    pos=instr(AccountInfo,chr(10))
		    if pos>0 then
			    AccountInfo="FILE# " & mFile & chr(10) & Mid(AccountInfo,pos+1,200)
		    else
			    AccountInfo="FILE# " & mFile
		    end if
	    else
		    pos=instr(AccountInfo,"NOTIFY:")
		    if pos>0 then
			    AccountInfo="FILE# " & mFile & chr(10) & Mid(AccountInfo,pos,200)
		    else
			    AccountInfo="FILE# " & mFile
		    end if
	    end if
    else
	    mDepartureAirportCode=""
	    mDepartureAirport=""
	    mTo=""
	    mBy=""
	    mTo1=""
	    mBy1=""
	    mTo2=""
	    mBy2=""
	    mDestAirport=""
	    mFlightDate1=""
	    mFlightDate2=""
	    mCarrierDesc=""
	    mExportDate=""
	    mDestCountry=""
	    mDepartureState=""
	    mFile=""
    end if
    //'document.frmMAWB.txtBillToInfo.Value=AccountInfo & "^^"
    document.frmMAWB.hAirOrgNum.Value=mAirOrgNum
    document.frmMAWB.hOriginPortID.Value=mDepartureAirportCode
    document.frmMAWB.txtDepartureAirport.Value=mDepartureAirport
    document.frmMAWB.txtTo.Value=mTo
    document.frmMAWB.txtBy.Value=mBy
    document.frmMAWB.txtTo1.Value=mTo1
    document.frmMAWB.txtBy1.Value=mBy1
    document.frmMAWB.txtTo2.Value=mTo2
    document.frmMAWB.txtBy2.Value=mBy2
    document.frmMAWB.txtDestAirport.Value=mDestAirport
    document.frmMAWB.txtFlightDate1.Value=mFlightDate1
    document.frmMAWB.txtFlightDate2.Value=mFlightDate2
    document.frmMAWB.txtDestCountry.value=mDestCountry
    IssuedBy=mCarrierDesc
    document.frmMAWB.txtIssuedBy.Value=mCarrierDesc

    Agent= document.frmMAWB.txtExecute.Value

    pos=0
    pos=instr(Agent,chr(10))
    if pos>0 then
	    Agent=Mid(Agent,1,pos-1)
    end if

    pos=instr(mCarrierDesc,chr(10))
    if pos>0 then
	    Carrier=Mid(mCarrierDesc,1,pos-1)
    else
	    Carrier=mCarrierDesc
    end if

    if Mid(Agent,1,11) = "AS AGENT OF" then
	    document.frmMAWB.htxtExecute.Value= "AS AGENT OF " & Carrier & chr(10) & "<%= vExecutionDatePlace %>"
    else
	    document.frmMAWB.htxtExecute.Value= Agent & chr(10) & "AS AGENT OF " & Carrier & chr(10) & "<%= vExecutionDatePlace %>"
    end if

End sub

</script>
<script type="text/javascript">
function MAWBChange(arg) {
    var sindex, mInfo, pos;
    var mDepartureAirportCode, mDepartureAirport, mTo, mBy, mTo1, mBy1, mTo2, mBy2, mDestAirport;
    var mFlightDate1, mFlightDate2, IssuedBy, AccountInfo;

    sindex = document.frmMAWB.lstMAWB.selectedIndex;

    if (sindex == 0)
        return;

    var vMAWB = document.frmMAWB.lstMAWB.item(sindex).text;
    // mInfo=Document.frmMAWB.lstMAWB.item(sindex).value

    var mInfo = get_mawb_booking_info(vMAWB);


    if (document.frmMAWB.lstMAWB.item(sindex).text != "Select One")
        document.frmMAWB.hmawb_num.value = document.frmMAWB.lstMAWB.item(sindex).text;
    else
        document.frmMAWB.hmawb_num.value = "";

    var mCarrierDesc = "";
    if (mInfo != "") {
        var pos = mInfo.indexOf('\n');
        var mAirOrgNum = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mDepartureAirportCode = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mDepartureAirport = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mTo = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mBy = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mTo1 = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mBy1 = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mTo2 = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mBy2 = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mDestAirport = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mFlightDate1 = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mFlightDate2 = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mCarrierDesc = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mExportDate = mInfo.substring(0, pos);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        var mDestCountry = mInfo.substring(0, pos);
        var mDepartureState = mInfo.substring(pos + 1, 1000);
        mInfo = mInfo.substring(pos + 1, 1000);

        pos = mInfo.indexOf('\n');
        mFile = mInfo.substring(pos + 1, 1000);

        mInfo = mInfo.substring(pos + 1, 1000);
        pos = mInfo.indexOf('\n');
        var mServiceLevel=mInfo.substring(0, pos);
	    mInfo = mInfo.substring(pos + 1, 1000);
    }
    else{
	    mDepartureAirportCode="";
	    mDepartureAirport="";
	    mTo="";
	    mBy="";
	    mTo1="";
	    mBy1="";
	    mTo2="";
	    mBy2="";
	    mDestAirport="";
	    mFlightDate1="";
	    mFlightDate2="";
	    mCarrierDesc="";
	    mExportDate="";
	    mDestCountry="";
	    mDepartureState="";
	    mFile="";
	    mServiceLevel="";
    }

    document.frmMAWB.hAirOrgNum.Value=mAirOrgNum;
    document.frmMAWB.hOriginPortID.Value=mDepartureAirportCode;
    document.frmMAWB.txtDepartureAirport.Value=mDepartureAirport;
    document.frmMAWB.txtTo.value=mTo;
    document.frmMAWB.txtBy.value = mBy;
    document.frmMAWB.txtTo1.value = mTo1;
    document.frmMAWB.txtBy1.value = mBy1;
    document.frmMAWB.txtTo2.value = mTo2;
    document.frmMAWB.txtBy2.value = mBy2;
    document.frmMAWB.txtDestAirport.value = mDestAirport;
    document.frmMAWB.txtFlightDate1.value = mFlightDate1;
    document.frmMAWB.txtFlightDate2.value = mFlightDate2;
    document.frmMAWB.txtDestCountry.value = mDestCountry;
    IssuedBy=mCarrierDesc;
    document.frmMAWB.txtIssuedBy.value = mCarrierDesc;
    document.frmMAWB.txtServiceLevel.value = mServiceLevel;
    var Agent = document.frmMAWB.txtExecute.value;

    pos = 0;
    pos = Agent.indexOf('\n');
    if (pos > 0)
        Agent = Agent.substring(0, pos);

    pos = mCarrierDesc.indexOf('\n');
    if (pos > 0)
        Carrier = mCarrierDesc.substring(0, pos);
    else
        Carrier = mCarrierDesc;

    if (Agent.substring(0, 11) == "AS AGENT OF")
	    document.frmMAWB.txtExecute.value = "AS AGENT OF " + Carrier + "\n" + "<%= vExecutionDatePlace %>";
    else
	    document.frmMAWB.txtExecute.value= Agent + "\n" + "AS AGENT OF "+ Carrier+ "\n" +  "<%= vExecutionDatePlace %>";


// Modified by Joon on Jan/30/2007 ////////////////////////////////////////////////////////////////

     if (arg == "new") {
        document.frmMAWB.action = "new_edit_mawb.asp?Edit=yes&MAWB=" + encodeURIComponent(vMAWB) + "&WindowName=" + window.name;
        document.frmMAWB.method = "POST";
        document.frmMAWB.target = "_self";
        frmMAWB.submit();
    }
}

function cPPO2Click(isChecked) {
    if (!isChecked) {
        document.frmMAWB.cPPO2.value = "";
        document.frmMAWB.cCOLL2.checked = true;
        document.frmMAWB.cCOLL2.value = "Y";
    }
    else {
        document.frmMAWB.cPPO2.value = "Y";
        document.frmMAWB.cCOLL2.checked = false;
        document.frmMAWB.cCOLL2.value = "";
    }
}

function cCOLL2Click(isChecked) {
    if (!isChecked) {
        document.frmMAWB.cCOLL2.value = "";
        document.frmMAWB.cPPO2.checked = true;
        document.frmMAWB.cPPO2.value = "Y";
    }
    else {
        document.frmMAWB.cCOLL2.value = "Y";
        document.frmMAWB.cPPO2.checked = false;
        document.frmMAWB.cPPO2.value = "";
    }
}


// Modified by Joon on Feb/5/2007 ////////////////////////////////////////////
function cPPO1Click(isChecked) {
    var DescArray, k, Desc2;
    if (!isChecked) {
        document.frmMAWB.cPPO1.value = "";
        document.frmMAWB.cCOLL1.checked = true;
        document.frmMAWB.cCOLL1.value = "Y";
        Desc2 = document.frmMAWB.txtDesc2.value;

        DescArray = Desc2.split('\n');
        Desc2 = "";
        for (var k = 0; k < DescArray.length; k++) {
            if (DescArray[k].toUpperCase().indexOf("FREIGHT") >= 0)
                Desc2 = Desc2 + "FREIGHT COLLECT";
            else
                Desc2 = Desc2 + DescArray[k];

            if (k < DescArray.length)
                Desc2 = Desc2 + '\n';
        }

        if (Desc2.indexOf("FREIGHT") == 0)
            Desc2 = Desc2 + "FREIGHT COLLECT";
    }
    else {
        document.frmMAWB.cPPO1.value = "Y";
        document.frmMAWB.cCOLL1.checked = false;
        document.frmMAWB.cCOLL1.value = "";
        Desc2 = document.frmMAWB.txtDesc2.value;

        DescArray = Desc2.split('\n');
        Desc2 = "";
        for (var k = 0; k < DescArray.length; k++) {
            if (DescArray[k].toUpperCase().indexOf("FREIGHT") >= 0)
                Desc2 = Desc2 + "FREIGHT PREPAID"
            else
                Desc2 = Desc2 + DescArray[k];

            if (k < DescArray.length)
                Desc2 = Desc2 + '\n';
        }

        if (Desc2.indexOf("FREIGHT") == 0)
            Desc2 = Desc2 + "FREIGHT PREPAID";
    }
    document.frmMAWB.txtDesc2.value = Desc2;
}

// Modified by Joon on Feb/5/2007 ////////////////////////////////////////////
function cCOLL1Click(isChecked) {
    var DescArray, k, Desc2;
    if (!isChecked) {
        document.frmMAWB.cCOLL1.value = "";
        document.frmMAWB.cPPO1.checked = true;
        document.frmMAWB.cPPO1.value = "Y";
        Desc2 = document.frmMAWB.txtDesc2.value;
        DescArray = Desc2.split('\n');
        Desc2 = "";
        for (var k = 0; k < DescArray.length; k++) {
            if (DescArray[k].toUpperCase().indexOf("FREIGHT") >= 0)
                Desc2 = Desc2 + "FREIGHT PREPAID";
            else
                Desc2 = Desc2 + DescArray[k];

            if (k < DescArray.length)
                Desc2 = Desc2 + '\n';
        }
        if (Desc2.indexOf("FREIGHT") == 0)
            Desc2 = Desc2 + "FREIGHT PREPAID";
    }
    else {
        document.frmMAWB.cCOLL1.value = "Y";
        document.frmMAWB.cPPO1.checked = false;
        document.frmMAWB.cPPO1.value = "";
        Desc2 = document.frmMAWB.txtDesc2.value;
        DescArray = Desc2.split('\n');
        Desc2 = "";
        for (var k = 0; k < DescArray.length; k++) {
            if (DescArray[k].toUpperCase().indexOf("FREIGHT") >= 0)
                Desc2 = Desc2 + "FREIGHT COLLECT";
            else
                Desc2 = Desc2 + DescArray[k];

            if (k < DescArray.length)
                Desc2 = Desc2 + '\n';
        }

        if (Desc2.indexOf("FREIGHT") == 0)
            Desc2 = Desc2 + "FREIGHT COLLECT";
    }
    document.frmMAWB.txtDesc2.value = Desc2;
}
</script>
<script type="text/vbscript">

Sub LaserClick(Copy) 'never used

    jPopUpPDF()
    sindex=Document.frmMAWB.lstMAWB.Selectedindex
    if sindex = 0 then
	    exit sub
    end if
    MAWB=Document.frmMAWB.lstMAWB.item(sindex).text
    document.frmMAWB.action="mawb_pdf.asp?mawb=" & MAWB & "&Copy=" & Copy & "&WindowName=popupNew" 
    document.frmMAWB.method="POST"
    document.frmMAWB.target="popUpPDF"
    frmMAWB.submit()

End Sub

</script>
<script type="text/javascript">


    function AddToHAWB(addHAWB, addELTAcct) {
        document.frmMAWB.action = "new_edit_mawb.asp?addHAWB=yes&addHAWBNo=" + addHAWB + "&addELTAcct=" + addELTAcct + "#selected_hawb" + "&WindowName=" + window.name;
        document.frmMAWB.method = "POST";
        document.frmMAWB.target = "_self";
        frmMAWB.submit();
    }
    function Desc2KeyUp() { // converted from vbscript
        var Info = document.frmMAWB.txtDesc2.value;
        var MyArray = Info.split('\n');
        var dd = MyArray.length - 1;
        if (dd > 13) {
            alert("Please go to Other Description session to continue!");
            Info = ""
            for (var i = 0; i <= 13; i++) {
                Info = Info + MyArray[i];
            }
            document.frmMAWB.txtDesc2.value = Info;
            document.frmMAWB.txtDesc1.focus();
        }
        Info = "";
        for (var i = 0; i <= dd; i++) {
            Info = Info + MyArray[i];
        }
        if (Info.length > 260) {
            Info = Info.substring(0, 261);
            document.frmMAWB.txtDesc2.value = Info;
            document.frmMAWB.txtDesc1.focus();
        }
    }
    function DeleteHAWB(HAWB, delELTAcct) {
        document.frmMAWB.action = "new_edit_mawb.asp?DeleteHAWB=yes&dHAWB=" + encodeURIComponent(HAWB)
        + "&delELTAcct=" + delELTAcct + "#selected_hawb" + "&WindowName=" + window.name;
        document.frmMAWB.method = "POST";
        document.frmMAWB.target = "_self";

        frmMAWB.submit();
    }


    function AdjustWeight(sCount) {
        document.frmMAWB.action = "new_edit_mawb.asp?AdjustWeight=yes&AdjustItemNo=" + sCount + "#selected_hawb" + "&WindowName=" + window.name;
        document.frmMAWB.method = "POST";
        document.frmMAWB.target = "_self";

        frmMAWB.submit();
    }
    function bCalClick(i) {
        var ItemNo = i;
        var Pieces = $("input.Pieces").get(ItemNo).value;
        if (Pieces == "")
            Pieces = 0;
        var ChargeableWeight = $("input.ChargeableWeight").get(ItemNo).value;
        var GrossWeight = $("input.GrossWeight").get(ItemNo).value;
        if (ChargeableWeight == "")
            ChargeableWeight = 0;
        var RateCharge = $("input.RateCharge").get(ItemNo).value;
        if (RateCharge == "")
            RateCharge = 0;
        if (!IsNumeric(Pieces))
            alert("Please enter a numeric value for PIECES");
        else if (!IsNumeric(ChargeableWeight))
            alert("Please enter a numeric value for Chargeable Weight");
        else if (!IsNumeric(RateCharge)) {
            alert("Please enter a numeric value for RateCharge");
            $("input.RateCharge").get(ItemNo).value = "";
        }
        else {
            var TotalCharge = parseFloat(RateCharge) * parseFloat(ChargeableWeight);
            $("input.TotalCharge").get(ItemNo).value = (Math.round(TotalCharge));
        }
    }
    function AddOC() {
	    var NoItem = parseInt(document.frmMAWB.hNoItemOC.value);
	    document.frmMAWB.hNoItemOC.value = NoItem + 1;
	    document.frmMAWB.action="new_edit_mawb.asp?AddOC=yes#add_oc" + "&WindowName=" + window.name;
	    document.frmMAWB.method="POST";
	    document.frmMAWB.target = "_self";
	    frmMAWB.submit();
    }

function DeleteOC(ItemNo){
    if (document.frmMAWB.hNoItemOC.value>0 
                && parseInt(document.frmMAWB.hNoItemOC.value)!=ItemNo ){
        if (confirm("Are you sure you want to delete this Other Charge? \r\nContinue?")) {
            document.frmMAWB.action = "new_edit_mawb.asp?DeleteOC=yes&dItemNo=" + ItemNo + "#add_oc" + "&WindowName=" + window.name;
            document.frmMAWB.method = "POST";
            document.frmMAWB.target = "_self";
            document.frmMAWB.submit();
        }
    }
}

    </script>
<script type="text/vbscript">


Sub key()   'never used
MAWB=document.frmMAWB.txtMAWB.value
if Window.event.Keycode=13 then
	document.frmMAWB.action="new_edit_mawb.asp?mawb=" & MAWB  & "&WindowName=" & window.name
	document.frmMAWB.method="POST"
	document.frmMAWB.target = "_self"
	frmMAWB.submit()
end if
End Sub
</script>
<script type="text/javascript">
//////////////////////////////////
// Unit_Price by ig 10/21/2006
//////////////////////////////////
    function GET_ITEM_UNIT_PRICE(tmpBuf) {
        var ItemUnitPrice, pos;

        ItemUnitPrice = 0;

        pos = tmpBuf.indexOf("-");
        if (pos > 0)
            ItemUnitPrice = tmpBuf.substring(pos + 1, 200);

        return ItemUnitPrice;

    }
    function SET_UNIT_PRICE(obj, val) {
        obj.value = parseFloat(val).toFixed(2); //  FormatNumber(val,2,,,0)
    }

    function ChargeItemChange(index) {
        var AgentName, ItemDesc;
        var sindex = $("select.ChargeItem").get(index).selectedIndex; // Document.all("ChargeItem").item(index+1).Selectedindex
        var ItemInfo = $("select.ChargeItem>option").get(sindex).value;

        var pos = ItemInfo.indexOf("-");
        if (pos >= 0)
            ItemDesc = ItemInfo.substring(pos + 1, 200);

        ///////////////////////////////
        // Unit_Price by ig 10/21/2006
        var ItemUnitPrice = GET_ITEM_UNIT_PRICE(ItemDesc);

        pos = ItemDesc.indexOf("-");
        if (pos >= 0)
            ItemDesc = ItemDesc.substring(0, pos - 1);
        ///////////////////////////////
        if (ItemDesc == "-0")
            ItemDesc = "";

        if (sindex >= 0) {
            $("input.ItemDesc").get(index).value = ItemDesc.trim();
            // Unit_Price by ig 10/21/2006
            SET_UNIT_PRICE($("input.ChargeAmt").get(index), ItemUnitPrice);
        }
        else
            $("input.ItemDesc").get(index).value = "";

    }
    function CHECK_IV_STATUS(tvMAWB) {
        if (tvMAWB == "" || tvMAWB == "0") {
            return true;
        }

        var IVstrMSG = "<%=IVstrMsg%>";
        if (IVstrMSG != "") {
            if (confirm("Invoice No. " + IVstrMSG + " for MAWB#:" + tvMAWB + " was processed already.\r\nDo you want to continue?"))
                return true;
            else
                return false;
        }
        return true;
    }

    function DeleteMAWB() {
        var sindex = document.frmMAWB.lstMAWB.selectedIndex;
        if (sindex > 0) {
            var MAWB = document.frmMAWB.lstMAWB.item(sindex).text;
            if (confirm("Do you really want to delete MAWB " + MAWB + "? \r\nContinue?")) {
                if (!CHECK_IV_STATUS(MAWB))
                    return;

                document.frmMAWB.action = "new_edit_mawb.asp?DeleteMAWB=yes&MAWB=" + encodeURIComponent(MAWB) + "&WindowName=" + window.name;
                document.frmMAWB.method = "POST";
                document.frmMAWB.target = "_self";
                frmMAWB.submit();
            }
        }
    }
    function EditHAWB(HAWB,COLODee){
	if (parseFloat(COLODee).toFixed(0)!=parseFloat(<%= elt_account_number %>).toFixed(0) )
	    window.location.href = "view_print.asp?hawb=" + encodeURIComponent(HAWB) + "&sType=house&COLODee=" + encodeURIComponent(COLODee);
	else
	    window.top.location.href = "/DomesticFreight/HouseAirBill/mode=search&HAWB=" + HAWB;

}

function ScaleChange(ItemNo){
	var Scale=$("select.KgLb").get(ItemNo).value;
	var GW=$("input.GrossWeight").get(ItemNo).value;
	var CW=$("input.ChargeableWeight").get(ItemNo).value;
	
    if (GW!="")
    	GW=parseFloat(GW);
	else
		GW=0;

	if (CW!="")
		CW=parseFloat(CW);
	else
		CW=0;


	if (Scale == "K" ){
		GW = GW / 2.20462262;
		CW = CW / 2.20462262;
	}
	else{
		GW = GW * 2.20462262;
		CW = CW * 2.20462262;
	}
	$("input.GrossWeight").get(ItemNo).value = parseFloat(GW).toFixed(2); 
	$("input.ChargeableWeight").get(ItemNo).value = parseFloat(CW).toFixed(2); 
	getIATARate(ItemNo);

}

function NewPrintVeiw(){
     var props,HAWB,sindex,MAWB;
     sindex = document.frmMAWB.lstMAWB.selectedIndex;
    MAWB = document.frmMAWB.lstMAWB.item(sindex).text;
    cMAWB= frmMAWB.hmawb_num.value;

    if (MAWB != ""  && MAWB != "Select One" ){
        props = "scrollBars=no,resizable=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes,width=880,height=650";
         window.open ("view_print.asp?sType=master&mawb="+ MAWB, "PrintBeta", props);
      }
      else if (cMAWB != ""  ){
             props = "scrollBars=no,resizable=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes,width=880,height=650";
        window.open ("view_print.asp?sType=master&mawb=" + cMAWB, "PrintBeta", props);
	  }
      else
		   alert("Please, select Master AWB NO. to view PDF");
    }

</script>


     <script>
        
$(document).ready(function (){
           
    if(parent.PrepPDFPrintOptions==undefined)
    {    
        $("#NewPrintVeiw1").click(
           function () { 
               if(confirm("You cannot print this document in a popup mode. Would you like to try in full page mode?"))
               {
                   opener.top.location.href="/DomesticFreight/MasterAirBill/"+window.location.href.split("?")[1];
                   window.close();
               }
               else
               { 
                   window.close();
               } 
           });
    }
});

    </script>
<script type="text/vbscript">

Sub MenuMouseOver()
  document.frmMAWB.lstMAWB.style.visibility="hidden"
End Sub
Sub MenuMouseOut()
  document.frmMAWB.lstMAWB.style.visibility="visible"
End Sub


</script>
<script type="text/javascript">
    var fBook = "<%= fBook %>";

    if (fBook == "yes")
        MAWBChange("");

</script>
<% if MinApplied <> -1  then 
	'response.Write("<script language='javascript'></script>")
	end if 
%>
<!-- //for Tooltip// -->
<script language="JavaScript" type="text/javascript" src="../include/tooltips.js" />
<!--  #INCLUDE FILE="../include/StatusFooter.asp" -->
</html>
