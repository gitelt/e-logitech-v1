<%@ Register TagPrefix="uc1" TagName="rdSelectDateControl1" Src="../../Reports/SelectionControls/rdSelectDateControl1.ascx" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics.WebUI.UltraWebGrid, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics.WebUI.WebDateChooser, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page language="c#" CodeFile="AMS_EDI_Ocean.aspx.cs" AutoEventWireup="false" Inherits="IFF_MAIN.ASPX.OnLines.AMS.AMS_EDI_Ocean" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics.WebUI.WebDataInput, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics.WebUI.UltraWebToolbar, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics.WebUI.UltraWebTab, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>AMS_EDI_Ocean</title>
<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema><LINK href="/IFF_MAIN/ASPX/CSS/AppStyle.css" type=text/css rel=stylesheet >
<SCRIPT src="/IFF_MAIN/ASPX/jScripts/WebDateSet1.js" type=text/javascript></SCRIPT>

<SCRIPT src="../jscripts/ig_dropCalendar.js" type=text/javascript></SCRIPT>

<SCRIPT src="/IFF_MAIN/ASPX/jScripts/ig_editDrop1.js" type=text/javascript></SCRIPT>
<!--  #INCLUDE FILE="../../include/common.htm" -->		
<script language=javascript>

function vlsch(gn,ValueListID,cellId) {
	var cell=igtbl_getCellById(cellId);
	var row=igtbl_getRowById(cellId);

	if(cell.Column.Key=="Partner Name") {
		var list = igtbl_getElementById(ValueListID);
		row.getCellFromKey('Code').setValue(list.value);
	}
//	else if(cell.Column.Key=="Airline") {
//		var list = igtbl_getElementById(ValueListID);
//		row.getCellFromKey('Airline_Code').setValue(list.value);
//	}
	
}

function acuh(gridName,cellId) {

	document.all.btnValidation.src="../../../Images/mark_o.gif"

var row=igtbl_getRowById(cellId);
var s = row.getCellFromKey("a").getValue();
if (s=="a") {
	return false;
}

s = row.getCellFromKey("x").getValue();
if (s=="x") {
	return false;
}
row.getCellFromKey('e').setValue('e');
row.getCellFromKey('e').Element.style.backgroundColor = "Lavender";
row.ParentRow.getCellFromKey('e').setValue('e');
row.ParentRow.getCellFromKey('e').Element.style.backgroundColor = "Lavender";
}

function arih(gridName,rowId) {

var row=igtbl_getRowById(rowId);
row.getCellFromKey("a").setValue("a");
row.getCellFromKey("a").Element.style.backgroundColor = "LightGreen";

var oGrid = igtbl_getGridById(gridName);
var oRows = oGrid.Rows;

if(oRows.length == 1) {
	oRows.getRow(0).getCellFromKey('i1_item_number').setValue("001");
	return false;
}
a = oRows.getRow(oRows.length-2).getCellFromKey('i1_item_number').getValue();
a++;

strA = a.toString(10);

while(strA.length<3) {
	strA = "0"+strA;		
}

oRows.getRow(oRows.length-1).getCellFromKey('i1_item_number').setValue(strA);

return false;

}

function beemh(gridName,cellId) {
var band = igtbl_getBandById(cellId);
var cell=igtbl_getCellById(cellId);
var row=igtbl_getRowById(cellId);
var s = row.getCellFromKey("a").getValue();

if (s=="a") {
	return false;
}
}

function cch(gridName,cellId,button) {

var SelectedParent = 'url(../../../Images/mark_o.gif)';
var row=igtbl_getRowById(cellId);
var cell=igtbl_getCellById(cellId);
var oCell = igtbl_getCellById(cellId);
var cUrl = oCell.Element.style.backgroundImage;
var band = igtbl_getBandById(row.Id);

			if(cell.Column.Key=="Chk") {
				if(cUrl==SelectedParent) {

					oCell.Element.style.backgroundImage =  'url(../../../Images/mark_x.gif)';					
					row.getCellFromKey("x").setValue("x");
					row.getCellFromKey('e').setValue('e');
					row.getCellFromKey("x").Element.style.backgroundColor = "Red";
					row.getCellFromKey('e').Element.style.backgroundColor = "Lavender";
					
					row.setSelected(true);
				}
				else {
				
					oCell.Element.style.backgroundImage =  'url(../../../Images/mark_o.gif)';
					row.getCellFromKey("x").setValue('');
					row.getCellFromKey('e').setValue('e');
					row.getCellFromKey("x").Element.style.backgroundColor = "Lavender";
					row.getCellFromKey('e').Element.style.backgroundColor = "Lavender";
					row.setSelected(true);
				}
			}

}

function NumberingRows() {

var oGrid = igtbl_getGridById('UltraWebGrid1');
var oRows = oGrid.Rows;

	for(i=0; i<oRows.length; i++) {
				strA = i.toString(10);
				while(strA.length<3) {
					strA = "0"+strA;		
				oRow = oRows.getRow(i);
				oRow.getCellFromKey('i1_item_number').setValue(strA);
				}
		
	}

return false;

}

function SelectAllRows() {

var oGrid = igtbl_getGridById('UltraWebGrid1');
var oRows = oGrid.Rows;
oGrid.suspendUpdates(true);
	for(i=0; i<oRows.length; i++) {
		oRow = oRows.getRow(i);
		oRow.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_x.gif)';
		oRow.getCellFromKey("x").setValue("x");
		oRow.getCellFromKey('e').setValue('e');
		oRow.getCellFromKey("x").Element.style.backgroundColor = "Red";
		oRow.getCellFromKey('e').Element.style.backgroundColor = "Lavender";		
//		oRow.setSelected(true);
		
	}
	oUltraWebGrid1.suspendUpdates(false);
}

function unSelectAllRows() {
	
var oGrid = igtbl_getGridById('UltraWebGrid1');
var oRows = oGrid.Rows;
oGrid.suspendUpdates(true);
	for(i=0; i<oRows.length; i++) {
		oRow = oRows.getRow(i);
		oRow.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_o.gif)';
		oRow.getCellFromKey("x").setValue("");
		oRow.getCellFromKey('e').setValue('e');
		oRow.getCellFromKey("x").Element.style.backgroundColor = "Lavender";
		oRow.getCellFromKey('e').Element.style.backgroundColor = "Lavender";		
//		oRow.setSelected(true);
		
	}
	oUltraWebGrid1.suspendUpdates(false);

}

function gridRowDelete(strGrid) {
   igtbl_deleteSelRows(strGrid);

}

function gridRowDeleteAll(strGrid) {
	var oGrid = igtbl_getGridById(strGrid);
	var oRows = oGrid.Rows;

	for(i=(oRows.length-1); i>=0; i--) {
	 strRow = strGrid+"r_"+i;
	 igtbl_deleteRow(strGrid,strRow);	 
	}
}

function DeleteRows() {

var oGrid = igtbl_getGridById('UltraWebGrid1');
var oRows = oGrid.Rows;
oGrid.suspendUpdates(true);

	for(i=0; i<oRows.length; i++) {
		oRow = oRows.getRow(i);
		if( oRow.getCellFromKey('Chk').Element.style.backgroundImage ==  'url(../../../Images/mark_x.gif)' ) {
		oRow.setSelected(true);
		}		
	}

oUltraWebGrid1.suspendUpdates(false);
gridRowDelete(oGrid.Id);

}


function setState(a) {
var strN = a.name.split(':');
var myField = "txt_"+ strN[2].substr(strN[2].indexOf("dl_")+3,strN[2].length);
igtab_getTabById(strN[0]).findControl(myField).value =  igtab_getTabById(strN[0]).findControl(strN[2]).value;
}

function setPort(a) {
var strN = a.name.split(':');
var myField = "txt_"+ strN[2].substr(strN[2].indexOf("dl_")+3,strN[2].length);
igtab_getTabById(strN[0]).findControl(myField).value =  igtab_getTabById(strN[0]).findControl(strN[2]).value;
}

function setCountry(a) {
var strN = a.name.split(':');
var myField = "txt_"+ strN[2].substr(strN[2].indexOf("dl_")+3,strN[2].length);
igtab_getTabById(strN[0]).findControl(myField).value =  igtab_getTabById(strN[0]).findControl(strN[2]).value;
}		

var lookup_url_start = "lookup.aspx";
var lookup_url_startMaster = "lookupMaster.aspx";

function searchOption() {
	
	var strOption = document.form1.drSearch.value;
	var strSearchKey = document.form1.txtSearchKey.value; 	

	if  ( strOption == 'Date')  {
		var strDate1 = igedit_getById("Webdatetimeedit1").getValueByMode(1);
		var strDate2 = igedit_getById("Webdatetimeedit2").getValueByMode(1);
		if ( !strDate1 ) { 
			alert('Please input the from date.');
			return false;	
		}
		if ( !strDate2 ) { 
			alert('Please input the from date.');
			return false;	
		}

        var lookup_url = lookup_url_startMaster + "?TABLE=ig_ocean_ams_edi_header" + "&FIELD=doc_number" +"&FILTER=Date"+"&FILTER1=" + strDate1+"&FILTER2=" + strDate2;
        openLookup( lookup_url, document.form1.txtSearchKey );                
			return false;	
		
	}
	
	if(strSearchKey=="") { 
		alert('Please input the search text.');
		document.form1.drSearch.selectedIndex = 0;
		return false; 
	 }	  
	if  ( strOption == 'Code') {
        var lookup_url = lookup_url_startMaster + "?TABLE=ig_ocean_ams_edi_header" + "&FIELD=doc_number" +"&FILTER=Code"+"&FILTER1=" + strSearchKey+"&FILTER2=''";
        openLookup( lookup_url, document.form1.txtSearchKey );                		
		return false;
	}
	if  ( strOption == 'Number') {
        var lookup_url = lookup_url_startMaster + "?TABLE=ig_ocean_ams_edi_header" + "&FIELD=doc_number" +"&FILTER=Number"+"&FILTER1=" + strSearchKey+"&FILTER2=''";
        openLookup( lookup_url, document.form1.txtSearchKey );                		
		return false;
	}
	if  ( strOption == 'Name')  {
	    var lookup_url = lookup_url_startMaster + "?TABLE=ig_ocean_ams_edi_header" + "&FIELD=doc_number" +"&FILTER=Name"+"&FILTER1=" + strSearchKey+"&FILTER2=''";
        openLookup( lookup_url, document.form1.txtSearchKey );                		
		return false;	
	}
	

}

function SearchCode ( field, lookup_name )
{
	
var ultraTab = igtab_getTabById("UltraWebTab1");
var htmlText = ultraTab.findControl(field);

	if(htmlText.value=="") { 
		alert('Please input the search text.');
		return true; 
	  }

    if ( htmlText && lookup_name )
    {
        // open lookup window with lookup results
        var lookup_url = lookup_url_start + "?TABLE=" + lookup_name + "&FIELD=" + field +"&FILTER=" + htmlText.value;
        openLookup( lookup_url, htmlText );                
    }
    else
    {
        return;
    }
    
}

function openLookup ( url, field )
{
	var winprops = "";

    if ( url && field )
    {
        // cleanup
//        field.value = "";

        // open window
        winprops =  "left="+event.screenX+",top="+event.screenY+",status=0,toolbar=0,location=0,directories=0,menubar=0,scrollbars=1,resize=0,width=450,height=350"
        lookup_window = window.open( url, "lookup_window", winprops );
    }

}

function formRest(tr,id) {

	var idText = id.getText();
	
	if(idText == 'New') {
		gridRowDeleteAll('UltraWebGrid1');
		__doPostBack("btnNew", "");   		
		return true;
	}
	else if(idText == 'Cancel' ) {
		gridRowDeleteAll('UltraWebGrid1');
		__doPostBack("btnCancel", "");  
		return true;
	}
	else if(idText == 'Reset' ) {
		return true;
	}
	else if(idText == 'Edit' ) {
		__doPostBack("btnEdit", "");   		
		return true;
	}
	else if(idText == 'Delete' ) {
		__doPostBack("btnDelete", "");   		
		return true;
	}
	else if ( idText == 'Save' ) {
			
			if(!dataValidation()) {
				return false;
			}

		NumberingRows();
		__doPostBack("btnSave", "");			
	}	
}

function dataValidation(how) {

if(how)
{
var v = document.all.btnValidation.src;
	if(v.indexOf("mark_o.gif") >= 0)	{
		alert('Please mark the Data Validation.');
		return false;
	}

	if(!Page_ClientValidate())  {
		return false;
	}			
}
var oGrid = igtbl_getGridById('UltraWebGrid1');
var oRows = oGrid.Rows;
var errMSG = "";

	if(oRows.length == 0) { 
	alert("Please enter the Bill Item.");
	return false;
	}
	

	var errMSG = new Array();
	
	for(i=0; i<oRows.length; i++) {
		
		oRow = oRows.getRow(i);
		i1_item_number = oRow.getCellFromKey('i1_item_number').getValue();			
		
		i2_quantity = oRow.getCellFromKey('i2_quantity').getValue();										
		i3_net_weight = oRow.getCellFromKey('i3_net_weight').getValue();									
		i4_volume = oRow.getCellFromKey('i4_volume').getValue();											
		i5_package_type = oRow.getCellFromKey('i5_package_type').getValue();								
		i6_comodity_code = oRow.getCellFromKey('i6_comodity_code').getValue();								
		i7_cash_value = oRow.getCellFromKey('i7_cash_value').getValue();									
		e1_equipment_number = oRow.getCellFromKey('e1_equipment_number').getValue();                      
		e2_seal_number1 = oRow.getCellFromKey('e2_seal_number1').getValue();								
		e3_seal_number2 = oRow.getCellFromKey('e3_seal_number2').getValue();								
		e4_length = oRow.getCellFromKey('e4_length').getValue();											
		e5_width = oRow.getCellFromKey('e5_width').getValue();											
		e6_height = oRow.getCellFromKey('e6_height').getValue();											
		e7_iso_equipment = oRow.getCellFromKey('e7_iso_equipment').getValue();                             
		e8_type_of_service = oRow.getCellFromKey('e8_type_of_service').getValue();                           
		e9_loaded_empty_total = oRow.getCellFromKey('e9_loaded_empty_total').getValue();						
		e10_equipment_desc_code = oRow.getCellFromKey('e10_equipment_desc_code').getValue();					
		d1_line_of_description = oRow.getCellFromKey('d1_line_of_description').getValue();                       
		m1_line_of_marks_and_numbers = oRow.getCellFromKey('m1_line_of_marks_and_numbers').getValue();		
		h1_hazard_code = oRow.getCellFromKey('h1_hazard_code').getValue();								
		h2_hazard_class = oRow.getCellFromKey('h2_hazard_class').getValue();								
		h3_hazard_description = oRow.getCellFromKey('h3_hazard_description').getValue();						
		h4_hazard_contact = oRow.getCellFromKey('h4_hazard_contact').getValue();                            
		h5_un_page_number = oRow.getCellFromKey('h5_un_page_number').getValue();							
		h6_flashpoint_temperature = oRow.getCellFromKey('h6_flashpoint_temperature').getValue();					
		h7_hazard_code_qualifier = oRow.getCellFromKey('h7_hazard_code_qualifier').getValue();					
		h8_hazard_unit_of_measure = oRow.getCellFromKey('h8_hazard_unit_of_measure').getValue();				
		h9_negative_indigator = oRow.getCellFromKey('h9_negative_indigator').getValue();						
		h10_hazard_label = oRow.getCellFromKey('h10_hazard_label').getValue();								
		h11_hazard_classification = oRow.getCellFromKey('h11_hazard_classification').getValue();
				
		if(i2_quantity == "" || i2_quantity == null)  { errMSG[i] = "(Bill Item) - item quantity \n"; }
		if(i3_net_weight == "" || i3_net_weight == null)  { errMSG[i]  += "(Bill Item) - Net Weight \n"; }
		if(i6_comodity_code == "" || i6_comodity_code == null)  { errMSG[i]  += "(Bill Item) - Comodity Code \n"; }
		if(i7_cash_value == "" || i7_cash_value == null)  { errMSG[i]  += "(Bill Item) - Cash value \n"; }
		if(e1_equipment_number == "" || e1_equipment_number == null)  { errMSG[i]  += "(Bill Item) - Equipment Number \n"; }
		if(e4_length == "" || e4_length == null)  { errMSG[i]  += "(Bill Item) - Length \n"; }
		if(e5_width == "" || e5_width == null)  { errMSG[i]  += "(Bill Item) - Width \n"; }
		if(e6_height == "" || e6_height == null)  { errMSG[i]  += "(Bill Item) - Height \n"; }
		if(e7_iso_equipment == "" || e7_iso_equipment == null)  { errMSG[i]  += "(Bill Item) - ISO equipment \n"; }
		if(e9_loaded_empty_total == "" || e9_loaded_empty_total == null)  { errMSG[i]  += "(Bill Item) - Loaded/Empty/Total \n"; }
		if(e10_equipment_desc_code == "" || e10_equipment_desc_code == null)  { errMSG[i]  +="(Bill Item) - Equipment Desc. Code \n"; }
		if(d1_line_of_description == "" || d1_line_of_description == null)  { errMSG[i]  += "(Bill Item) - Line of Desc. \n"; }
		if(m1_line_of_marks_and_numbers == "" || m1_line_of_marks_and_numbers == null)  { errMSG[i]  += "(Bill Item) - Line of Marks and Number \n"; }
		if(h1_hazard_code == "" || h1_hazard_code == null)  { errMSG[i]  += "(Bill Item) - Hazard Code : UN code \n"; }
		if(h6_flashpoint_temperature == "" || h6_flashpoint_temperature == null)  { errMSG[i]  += "(Bill Item) - Flashpoint Temperature \n"; }
		if(h7_hazard_code_qualifier == "" || h7_hazard_code_qualifier == null)  { errMSG[i]  += "(Bill Item) -Hazard Code Qualifier : 'I' - IMO classification \n"; }
		if(h8_hazard_unit_of_measure == "" || h8_hazard_unit_of_measure == null)  { errMSG[i]  += "(Bill Item) - Hazard Unit of Measure : 'CE' Degrees Celsius \n"; }
		if(h9_negative_indigator == "" || h9_negative_indigator == null)  { errMSG[i]  += "(Bill Item) - Negative Indigator : Y/N \n"; }	
		if(errMSG[i]) errMSG[i] = "Item No. :" +  i1_item_number + " - \n" + errMSG[i] ;
		
	}                                                                                                                  
		
		if(errMSG[0]) {
			alert("Required Data field error (Bill Item):\n" + errMSG) ;
			return false;
		}	
		
return true;

}

</script>
</HEAD>
<body MS_POSITIONING="FlowLayout">
<form id=form1 method=post runat="server">
<TABLE id=Table3 cellSpacing=0 cellPadding=0 align=left bgColor=#ffffff>
  <TBODY>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 16px"><asp:label id=Label8 runat="server" DESIGNTIMEDRAGDROP="132" Width="344px" ForeColor="Navy" Font-Italic="True" Font-Bold="True" Height="100%" Font-Size="Larger">Ocean AMS</asp:label><asp:label id=lblTask runat="server" Width="344px" ForeColor="DarkGreen" Font-Italic="True" Font-Bold="True" Height="100%" Font-Size="Larger"></asp:label></TD></TR>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 4px"><asp:label id=lblError runat="server" DESIGNTIMEDRAGDROP="9515" Width="856px" ForeColor="Red" Font-Italic="True" Font-Bold="True" Font-Underline="True"></asp:label></TD></TR>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 21px"><igtbar:ultrawebtoolbar id=UltraWebToolbar1 runat="server" Width="184px" ForeColor="Black" Font-Size="9pt" Font-Names="Arial" BorderWidth="2px" BorderStyle="Groove" BackColor="Silver" ImageDirectory="/ig_common/images/" MovableImage="ig_tb_move00.gif" ItemWidthDefault="80px">
<HoverStyle Cursor="Default" Font-Size="9pt" Font-Names="Arial" BorderStyle="Outset" ForeColor="Black" BackColor="Silver">
</HoverStyle>

<ClientSideEvents Click="formRest">
</ClientSideEvents>

<SelectedStyle Cursor="Default" Font-Size="9pt" Font-Names="Arial" BorderStyle="Inset" ForeColor="Black" BackColor="Silver">
</SelectedStyle>

<Items>
<igtbar:TBarButton Tag="" Key="Back" HoverImage="" ToolTip="" SelectedImage="" Text="&lt;&lt; Back" TargetURL="" DisabledImage="" TargetFrame="" Image=""></igtbar:TBarButton>
<igtbar:TBarButton Key="Reset" Text="Reset" Image="images/cancel.gif"></igtbar:TBarButton>
<igtbar:TBarButton Tag="" Key="New" HoverImage="" ToolTip="" SelectedImage="" Text="New" TargetURL="" DisabledImage="" TargetFrame="" Image="images/icon_new.gif"></igtbar:TBarButton>
<igtbar:TBarButton Key="Edit" Text="Edit"></igtbar:TBarButton>
<igtbar:TBarButton Tag="" Key="Cancel" HoverImage="" ToolTip="" SelectedImage="" Text="Cancel" TargetURL="" DisabledImage="" TargetFrame="" Image="images/BD14757_.GIF"></igtbar:TBarButton>
<igtbar:TBarButton Tag="" Key="Save" HoverImage="" ToolTip="" SelectedImage="" Text="Save" TargetURL="" DisabledImage="" TargetFrame="" Image="images/icon_save.gif"></igtbar:TBarButton>
<igtbar:TBarButton Key="Delete" Text="Delete"></igtbar:TBarButton>
</Items>

<DefaultStyle BorderWidth="1px" Font-Size="9pt" Font-Names="Arial" BorderColor="ControlLight" BorderStyle="Solid" ForeColor="Black" BackColor="ControlLight">
</DefaultStyle>
						</igtbar:ultrawebtoolbar></TD></TR>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 10px" bgColor=#cdcc9d 
    ></TD></TR>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 82px" vAlign=top>
      <TABLE id=Table6 
      style="BORDER-RIGHT: #cdcc9d 1px solid; BACKGROUND-POSITION: 0% 0%; BORDER-TOP: #cdcc9d 1px solid; FONT-SIZE: 9pt; BACKGROUND-ATTACHMENT: scroll; BACKGROUND-IMAGE: none; BORDER-LEFT: #cdcc9d 1px solid; WIDTH: 536px; BORDER-BOTTOM: #cdcc9d 1px solid; BACKGROUND-REPEAT: repeat; HEIGHT: 72px" 
      cellSpacing=5 cellPadding=0 align=left border=0 
      DESIGNTIMEDRAGDROP="627">
        <TR>
          <TD style="WIDTH: 106px; HEIGHT: 5px"><asp:label id=Label1 runat="server" Width="100%" ForeColor="Navy" Font-Italic="True" Font-Bold="True" Font-Size="10pt" Font-Names="Verdana"> AMS EDI Information</asp:label></TD>
          <TD style="WIDTH: 165px; HEIGHT: 5px" bgColor=palegoldenrod 
          ></TD>
          <TD style="HEIGHT: 5px"><asp:label id=lblDocNo runat="server" Width="136px" ForeColor="Red" Font-Italic="True" Font-Bold="True" Font-Underline="True"></asp:label></TD>
          <TD style="HEIGHT: 5px"></TD>
          <TD style="WIDTH: 81px; HEIGHT: 5px"></TD>
          <TD style="HEIGHT: 5px"></TD></TR>
        <TR>
          <TD style="WIDTH: 106px">search for</TD>
          <TD style="WIDTH: 165px"><asp:textbox id=txtSearchKey runat="server" DESIGNTIMEDRAGDROP="999" Width="160px" ForeColor="#0000C0" Font-Bold="True" BorderWidth="1px" BorderStyle="Solid" BackColor="White" BorderColor="#0000C0"></asp:textbox></TD>
          <TD></TD>
          <TD></TD>
          <TD style="WIDTH: 81px"></TD>
          <TD></TD></TR>
        <TR>
          <TD style="WIDTH: 106px">Creation Date</TD>
          <TD style="WIDTH: 165px"><uc1:rdselectdatecontrol1 id=RdSelectDateControl11 runat="server"></uc1:rdselectdatecontrol1></TD>
          <TD><asp:dropdownlist id=drSearch runat="server" Width="160px">
<asp:ListItem Value="Code">Search with Vessel Code</asp:ListItem>
<asp:ListItem Value="Name">Search with Vessel Name</asp:ListItem>
<asp:ListItem Value="Number">Search with Doc.Num.</asp:ListItem>
<asp:ListItem Value="Date">Search with Date</asp:ListItem>
</asp:dropdownlist></TD>
          <TD><asp:button id=btnSearch runat="server" Text="Go"></asp:button></TD>
          <TD style="WIDTH: 81px"></TD>
          <TD></TD></TR>
        <TR>
          <TD style="WIDTH: 106px; HEIGHT: 5px"><FONT 
            face=����></FONT></TD>
          <TD style="WIDTH: 165px; HEIGHT: 5px"><igtxt:webdatetimeedit id=Webdatetimeedit1 accessKey=e runat="server" Width="100%" ForeColor="DarkCyan" PromptChar=" " EditModeFormat="MM/dd/yyyy" Fields=""><BUTTONSAPPEARANCE CustomButtonDisplay="OnRight"></BUTTONSAPPEARANCE><SPINBUTTONS SpinOnReadOnly="True" Display="OnLeft"></SPINBUTTONS>
										</igtxt:webdatetimeedit></TD>
          <TD style="HEIGHT: 5px"><igtxt:webdatetimeedit id=Webdatetimeedit2 accessKey=e runat="server" Width="100%" ForeColor="DarkCyan" PromptChar=" " EditModeFormat="MM/dd/yyyy" Fields=""><BUTTONSAPPEARANCE CustomButtonDisplay="OnRight"></BUTTONSAPPEARANCE><SPINBUTTONS SpinOnReadOnly="True" Display="OnLeft"></SPINBUTTONS>
										</igtxt:webdatetimeedit></TD>
          <TD style="HEIGHT: 5px"><FONT 
face=����></FONT></TD>
          <TD style="WIDTH: 81px; HEIGHT: 5px"></TD>
          <TD style="HEIGHT: 5px"></TD></TR><!-- //-->
        <TR>
          <TD style="WIDTH: 106px"><FONT face=����><asp:label id=Label65 runat="server" Width="150px">Doc. Number(Auto gen.)</asp:label></FONT></TD>
          <TD><asp:textbox id=txt_doc_number runat="server" Width="160px" BorderWidth="1px" BorderStyle="Inset" BackColor="Yellow" ReadOnly="True"></asp:textbox></TD>
          <TD>Creation Date</TD>
          <TD><asp:textbox id=txt_creation_date runat="server" Width="160px" DESIGNTIMEDRAGDROP="64" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px"></asp:textbox></TD>
          <TD style="WIDTH: 81px"><FONT face=����><asp:label id=Label64 runat="server" Width="100px">AMS Sent Status</asp:label></FONT></TD>
          <TD><FONT face=����><asp:Image id=img_ams_sent runat="server" ImageUrl="../../../Images/mark_o.gif"></asp:Image></FONT></TD></TR>
        <TR>
          <TD style="WIDTH: 106px"><asp:label id=lblValidation runat="server" DESIGNTIMEDRAGDROP="67">Data Validation</asp:label></TD>
          <TD><asp:imagebutton id=btnValidation runat="server" ImageUrl="../../../Images/mark_o.gif"></asp:imagebutton></TD>
          <TD><FONT face=����><asp:imagebutton id=btnAMS_Send runat="server" ImageUrl="Images/datasourcegood.gif"></asp:imagebutton><asp:label id=lblAMS_Send runat="server" >AMS Send</asp:label></FONT></TD>
          <TD></TD>
          <TD style="WIDTH: 81px"></TD>
          <TD></TD></TR></TABLE>
      <P><FONT face=����></FONT>&nbsp;</P></TD></TR>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 2px" vAlign=top 
  ></TD></TR>
  <TR>
    <TD style="WIDTH: 866px; HEIGHT: 111px" vAlign=top><igtab:ultrawebtab id=UltraWebTab1 runat="server" Width="100%">
<DefaultTabStyle Font-Italic="True" ForeColor="ActiveCaptionText" BackColor="LightSlateGray">
</DefaultTabStyle>

<SelectedTabStyle Font-Underline="True" Font-Italic="True" Font-Bold="True" ForeColor="Navy" BackColor="Khaki">
</SelectedTabStyle>

<Tabs>
<igtab:Tab Text="Vessel Info.">
<ContentTemplate>
<TABLE id=Table5 style="BORDER-RIGHT: thin groove; BORDER-TOP: thin groove; BORDER-LEFT: thin groove; WIDTH: 100%; BORDER-BOTTOM: thin groove; HEIGHT: 188px">
<TR>
<TD>
<TABLE id=Table1 style="WIDTH: 418px; HEIGHT: 171px" DESIGNTIMEDRAGDROP="2834">
<TR>
<TD>
<asp:Label id=Label3 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="2653">Vessel Code *</asp:Label></TD>
<TD>
<asp:textbox id=txt_v1_vessel_code runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="7"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq1 runat="server" Display="None" ControlToValidate="txt_v1_vessel_code" ErrorMessage="(Vessel Info.) - Vessel Code"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label4 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="2904">Voyage Number *</asp:Label></TD>
<TD>
<asp:textbox id=txt_v2_voyage_number runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq2 runat="server" DESIGNTIMEDRAGDROP="18921" Display="None" ControlToValidate="txt_v2_voyage_number" ErrorMessage="(Vessel Info.) - Voyage Number"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label6 runat="server" Font-Italic="True" ForeColor="#C04000">Vessel Name</asp:Label></TD>
<TD>
<asp:textbox id=txt_v3_vessel_name runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="23"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label5 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="3280">SCAC Code *</asp:Label></TD>
<TD>
<asp:textbox id=txt_v4_scac_code runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq3 runat="server" Display="None" ControlToValidate="txt_v4_scac_code" ErrorMessage="(Vessel Info.) - SCAC Code"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label7 runat="server" Font-Italic="True" ForeColor="#C04000" DESIGNTIMEDRAGDROP="4032">Vessel Flag</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_v5_vessel_flag runat="server" Width="100%" DESIGNTIMEDRAGDROP="1222" BackColor="#FFFFC0"></asp:dropdownlist></TD>
<TD>
<asp:textbox id=txt_v5_vessel_flag runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="2"></asp:textbox></TD></TR>
<TR>
<TD>
<asp:Label id=Label9 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="4784">First US Port of Discharge *</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_v6_first_us_port_of_discharge runat="server" Width="100%" BackColor="#FFFFC0"></asp:dropdownlist></TD>
<TD>
<asp:textbox id=txt_v6_first_us_port_of_discharge runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox>
<asp:RequiredFieldValidator id=rq4 runat="server" Display="None" ControlToValidate="txt_v6_first_us_port_of_discharge" ErrorMessage="(Vessel Info.) - First US Port of Discharge"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label10 runat="server" ForeColor="Blue">Last Foreign POL *</asp:Label></TD>
<TD>
<asp:textbox id=txt_v7_last_foreign_pol_s runat="server" Font-Bold="True" ForeColor="#0000C0" Width="140px" BackColor="#FFFFC0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#0000C0"></asp:textbox><INPUT id=Button2 onclick="javascript:SearchCode('txt_v7_last_foreign_pol_s','ig_schedule_k');" type=button value=Go name=Go></TD>
<TD>
<asp:textbox id=txt_v7_last_foreign_pol runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox>
<asp:RequiredFieldValidator id=rq5 runat="server" Display="None" ControlToValidate="txt_v7_last_foreign_pol" ErrorMessage="(Vessel Info.) - Last Foreign POL"></asp:RequiredFieldValidator></TD></TR>
<TR></TR></TABLE>
<asp:RequiredFieldValidator id=rq_txt_v3_vessel_name runat="server" Display="None" ControlToValidate="txt_v3_vessel_name" ErrorMessage="(Vessel Info.) - Vessel Name"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator id=rq_txt_v5_vessel_flag runat="server" Display="None" ControlToValidate="txt_v5_vessel_flag" ErrorMessage="(Vessel Info.) - Vessel Flag"></asp:RequiredFieldValidator></TD>
<TD>
<TABLE id=Table4 style="WIDTH: 444px; HEIGHT: 169px">
<TR>
<TD>
<asp:Label id=Label11 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="5536">Port of Discharge *</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_p1_port_of_discharge runat="server" Width="120px" BackColor="#FFFFC0"></asp:dropdownlist></TD>
<TD>
<asp:textbox id=txt_p1_port_of_discharge runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox>
<asp:RequiredFieldValidator id=rq6 runat="server" Display="None" ControlToValidate="txt_p1_port_of_discharge" ErrorMessage="(Vessel Info.) - Port of Discharge"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label12 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="5912">Est. Date of Arrival *</asp:Label></TD>
<TD>
<igtxt:webdatetimeedit id=txt_p2_estimated_date_of_arrival accessKey=e runat="server" ForeColor="DarkCyan" Width="120px" DESIGNTIMEDRAGDROP="3489" BackColor="#FFFFC0" Fields="" EditModeFormat="MM/dd/yyyy" PromptChar=" ">
<SpinButtons Display="OnLeft" SpinOnReadOnly="True">
</SpinButtons>
</igtxt:webdatetimeedit></TD>
<TD>
<asp:RequiredFieldValidator id=rq7 runat="server" Display="None" ControlToValidate="txt_p2_estimated_date_of_arrival" ErrorMessage="(Vessel Info.) - Est. Date of Arrival"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label13 runat="server" DESIGNTIMEDRAGDROP="6288">Terminal Operator Code</asp:Label></TD>
<TD>
<asp:textbox id=txt_p3_terminal_operator_code_s runat="server" Font-Bold="True" ForeColor="#0000C0" Width="140px" BackColor="#FFFFC0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#0000C0"></asp:textbox><INPUT id=Button3 onclick="javascript:SearchCode('txt_p3_terminal_operator_code_s','ig_schedule_k');" type=button value=Go name=Go></TD>
<TD>
<asp:textbox id=txt_p3_terminal_operator_code runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox></TD></TR>
<TR>
<TD>
<asp:Label id=Label14 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="6664">Port of Load *</asp:Label></TD>
<TD>
<asp:textbox id=txt_l1_port_of_load_s runat="server" Font-Bold="True" ForeColor="#0000C0" Width="140px" BackColor="#FFFFC0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#0000C0"></asp:textbox><INPUT id=Button4 onclick="javascript:SearchCode('txt_l1_port_of_load_s','ig_schedule_k');" type=button value=Go name=Go></TD>
<TD>
<asp:textbox id=txt_l1_port_of_load runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px"></asp:textbox>
<asp:RequiredFieldValidator id=rq8 runat="server" Display="None" ControlToValidate="txt_l1_port_of_load" ErrorMessage="(Vessel Info.) - Port of Load"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label15 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="7040">Load Date *</asp:Label></TD>
<TD>
<igtxt:webdatetimeedit id=txt_l2_load_date accessKey=e runat="server" ForeColor="DarkCyan" Width="120px" DESIGNTIMEDRAGDROP="3771" BackColor="#FFFFC0" Fields="" EditModeFormat="MM/dd/yyyy" PromptChar=" ">
<SpinButtons Display="OnLeft" SpinOnReadOnly="True">
</SpinButtons>
</igtxt:webdatetimeedit></TD>
<TD>
<asp:RequiredFieldValidator id=rq9 runat="server" Display="None" ControlToValidate="txt_l2_load_date" ErrorMessage="(Vessel Info.) - Load Date"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label16 runat="server">Load Time(HH:MM:00)</asp:Label></TD>
<TD>
<igtxt:webdatetimeedit id=txt_l3_load_time accessKey=e runat="server" ForeColor="DarkCyan" Width="80px" BackColor="#FFFFC0" Fields="" EditModeFormat="HH:mm:ss" PromptChar=" " DisplayModeFormat="HH:mm:ss">
<SpinButtons Display="OnLeft" SpinOnReadOnly="True">
</SpinButtons>
</igtxt:webdatetimeedit></TD>
<TD></TD></TR>
<TR>
<TD></TD>
<TD></TD>
<TD></TD></TR>
<TR></TR></TABLE>
<P>&nbsp;</P></TD></TR></TABLE>
</ContentTemplate>
</igtab:Tab>
<igtab:Tab Text="Bill Header Info.">
<ContentTemplate>
<TABLE id=Table8 style="BORDER-RIGHT: thin groove; BORDER-TOP: thin groove; BORDER-LEFT: thin groove; WIDTH: 100%; BORDER-BOTTOM: thin groove">
<TR>
<TD>
<TABLE id=Table24 style="WIDTH: 410px; HEIGHT: 255px" DESIGNTIMEDRAGDROP="382">
<TR>
<TD>
<asp:Label id=Label17 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="7868">Bill of Lading Number *</asp:Label></TD>
<TD>
<asp:textbox id=txt_b1_bill_of_lading_number runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="12"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq10 runat="server" DESIGNTIMEDRAGDROP="7127" Display="None" ControlToValidate="txt_b1_bill_of_lading_number" ErrorMessage="(Bill Header Info.) - Bill of Lading Number"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label18 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="8677">Port of Loading *</asp:Label>
<asp:RequiredFieldValidator id=rq11 runat="server" DESIGNTIMEDRAGDROP="7654" Display="None" ControlToValidate="txt_b2_port_of_loading" ErrorMessage="(Bill Header Info.) - Port of Loading"></asp:RequiredFieldValidator></TD>
<TD>
<asp:textbox id=txt_b2_port_of_loading_s runat="server" Font-Bold="True" ForeColor="#0000C0" Width="140px" BackColor="#FFFFC0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#0000C0"></asp:textbox><INPUT id=Button2 onclick="javascript:SearchCode('txt_b2_port_of_loading_s','ig_schedule_k');" type=button value=Go name=Go></TD>
<TD>
<asp:textbox id=txt_b2_port_of_loading runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox></TD></TR>
<TR>
<TD>
<asp:Label id=Label19 runat="server" DESIGNTIMEDRAGDROP="9092">Place of Final Destination</asp:Label></TD>
<TD>
<asp:textbox id=txt_b3_place_of_final_destination_s runat="server" Font-Bold="True" ForeColor="#0000C0" Width="140px" BackColor="#FFFFC0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#0000C0"></asp:textbox><INPUT id=Button1 onclick="javascript:SearchCode('txt_b3_place_of_final_destination_s','ig_schedule_k');" type=button value=Go name=Go></TD>
<TD>
<asp:textbox id=txt_b3_place_of_final_destination runat="server" Width="40px" DESIGNTIMEDRAGDROP="1673" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox></TD></TR>
<TR>
<TD>
<asp:Label id=Label20 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="9507">Place of Receipt *</asp:Label></TD>
<TD>
<asp:textbox id=txt_b4_place_of_receipt runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="17"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq12 runat="server" DESIGNTIMEDRAGDROP="8436" Display="None" ControlToValidate="txt_b4_place_of_receipt" ErrorMessage="(Bill Header Info.) - Place of Receipt"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label21 runat="server" Font-Italic="True" ForeColor="#C04000" DESIGNTIMEDRAGDROP="9922">B/Lading Status Code</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_b5_b_lading_status_code runat="server" Width="100%" DESIGNTIMEDRAGDROP="1807" BackColor="#FFFFC0">
<asp:ListItem></asp:ListItem>
<asp:ListItem Value="0">Reqular Bill</asp:ListItem>
<asp:ListItem Value="B">FROB</asp:ListItem>
</asp:dropdownlist></TD>
<TD>
<asp:textbox id=txt_b5_b_lading_status_code runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="1"></asp:textbox></TD></TR>
<TR>
<TD>
<asp:Label id=Label22 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="10337">B/Lading Issuer SCAC Code *</asp:Label></TD>
<TD>
<asp:textbox id=txt_b6_b_lading_issuer_scac_code runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq13 runat="server" DESIGNTIMEDRAGDROP="8827" Display="None" ControlToValidate="txt_b6_b_lading_issuer_scac_code" ErrorMessage="(Bill Header Info.) - B/Lading Issuer SCAC Code"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label23 runat="server" Font-Italic="True" ForeColor="#C04000" DESIGNTIMEDRAGDROP="10752">SNP1</asp:Label></TD>
<TD>
<asp:textbox id=txt_b7_snp1 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label24 runat="server" Font-Italic="True" ForeColor="#C04000">SNP2</asp:Label></TD>
<TD>
<asp:textbox id=txt_b8_snp2 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label25 runat="server" ForeColor="Blue" DESIGNTIMEDRAGDROP="11583">Manifested Units *</asp:Label></TD>
<TD align=left>
<asp:textbox id=txt_b9_manifested_units runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox>&nbsp;
<asp:Label id=Label2 runat="server" ForeColor="Red" DESIGNTIMEDRAGDROP="3143">(ex. CTNS, BOXES)</asp:Label></TD>
<TD>
<asp:RequiredFieldValidator id=rq14 runat="server" DESIGNTIMEDRAGDROP="9611" Display="None" ControlToValidate="txt_b9_manifested_units" ErrorMessage="(Bill Header Info.) - Manifested Units"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label26 runat="server" ForeColor="Blue">Total Gross Weight *</asp:Label></TD>
<TD>
<igtxt:WebNumericEdit id=txt_b10_total_gross_weight runat="server" BackColor="#FFFFC0" DataMode="Decimal"></igtxt:WebNumericEdit></TD>
<TD>
<asp:RequiredFieldValidator id=rq15 runat="server" Display="None" ControlToValidate="txt_b10_total_gross_weight" ErrorMessage="(Bill Header Info.) - Total Gross Weight"></asp:RequiredFieldValidator></TD></TR></TABLE>
<asp:RequiredFieldValidator id=rq_txt_b5_b_lading_status_code runat="server" Display="None" ControlToValidate="txt_b5_b_lading_status_code" ErrorMessage="(Bill Header Info.) - B/Lading Status Code"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator id=rq_txt_b7_snp1 runat="server" Display="None" ControlToValidate="txt_b7_snp1" ErrorMessage="(Bill Header Info.) - SNP1"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator id=rq_txt_b8_snp2 runat="server" Display="None" ControlToValidate="txt_b8_snp2" ErrorMessage="(Bill Header Info.) - SNP2"></asp:RequiredFieldValidator></TD>
<TD vAlign=top align=left>
<TABLE id=Table7 style="WIDTH: 355px; HEIGHT: 258px">
<TR>
<TD>
<asp:Label id=Label27 runat="server" DESIGNTIMEDRAGDROP="12414">Booking Number</asp:Label></TD>
<TD>
<asp:textbox id=txt_b11_booking_number runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="30"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label28 runat="server" Width="162px" DESIGNTIMEDRAGDROP="12830">Master Ocean Bill Number</asp:Label></TD>
<TD>
<asp:textbox id=txt_b12_master_ocean_bill_number runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="16"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label29 runat="server" Width="162px" DESIGNTIMEDRAGDROP="13246">SNP3</asp:Label></TD>
<TD>
<asp:textbox id=txt_b13_agency_unique_code runat="server" Width="160px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="5"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label30 runat="server" Width="162px" DESIGNTIMEDRAGDROP="14082">SNP4</asp:Label></TD>
<TD>
<asp:textbox id=txt_b14_snp3 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label31 runat="server" Width="162px" DESIGNTIMEDRAGDROP="14497">SNP5</asp:Label></TD>
<TD>
<asp:textbox id=txt_b15_snp4 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label32 runat="server" Width="162px" DESIGNTIMEDRAGDROP="14912">SNP6</asp:Label></TD>
<TD>
<asp:textbox id=txt_b16_snp5 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label33 runat="server" Width="162px" DESIGNTIMEDRAGDROP="15359">SNP7</asp:Label></TD>
<TD>
<asp:textbox id=txt_b17_snp6 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label34 runat="server" Width="162px" DESIGNTIMEDRAGDROP="15822">SNP8</asp:Label></TD>
<TD>
<asp:textbox id=txt_b18_snp7 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label35 runat="server" Width="162px" DESIGNTIMEDRAGDROP="16285">SNP9</asp:Label></TD>
<TD>
<asp:textbox id=txt_b19_snp8 runat="server" Width="40px" BackColor="#FFFFC0" BorderStyle="Inset" BorderWidth="1px" MaxLength="4"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label36 runat="server" Width="162px">Weight Unit</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_b20_weight_unit runat="server" Width="100%" BackColor="#FFFFC0">
<asp:ListItem Value="  ">  </asp:ListItem>
<asp:ListItem Value="KG">Kg</asp:ListItem>
<asp:ListItem Value="LB">Lb</asp:ListItem>
</asp:dropdownlist></TD>
<TD>
<asp:textbox id=txt_b20_weight_unit runat="server" Width="40px" BackColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" MaxLength="2"></asp:textbox></TD></TR></TABLE></TD></TR></TABLE>
</ContentTemplate>
</igtab:Tab>
<igtab:Tab Text="Shipper Info.">
<ContentTemplate>
<TABLE id=Table10 style="BORDER-RIGHT: thin groove; BORDER-TOP: thin groove; BORDER-LEFT: thin groove; WIDTH: 100%; BORDER-BOTTOM: thin groove">
<TR>
<TD>
<TABLE id=Table2 style="WIDTH: 442px; HEIGHT: 198px">
<TR>
<TD>
<asp:Label id=Label37 runat="server" DESIGNTIMEDRAGDROP="17100" ForeColor="Blue">Shipper Name *</asp:Label></TD>
<TD>
<asp:textbox id=txt_s1_shipper_name runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq16 runat="server" DESIGNTIMEDRAGDROP="12293" ErrorMessage="(Shipper Info.) - Shipper Name" ControlToValidate="txt_s1_shipper_name" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label38 runat="server" DESIGNTIMEDRAGDROP="17464" ForeColor="Blue">Shipper Address 1 *</asp:Label></TD>
<TD>
<asp:textbox id=txt_s2_shipper_address1 runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq17 runat="server" DESIGNTIMEDRAGDROP="12763" ErrorMessage="(Shipper Info.) - Shipper Address 1" ControlToValidate="txt_s2_shipper_address1" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label39 runat="server" DESIGNTIMEDRAGDROP="17940">Shipper Address 2 </asp:Label></TD>
<TD>
<asp:textbox id=txt_s3_shipper_address2 runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label40 runat="server" DESIGNTIMEDRAGDROP="18415" ForeColor="Blue">Shipper City *</asp:Label></TD>
<TD>
<asp:textbox id=txt_s4_shipper_city runat="server" Width="160px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="19"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq18 runat="server" DESIGNTIMEDRAGDROP="13376" ErrorMessage="(Shipper Info.) - Shipper City" ControlToValidate="txt_s4_shipper_city" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label41 runat="server" DESIGNTIMEDRAGDROP="18891">Shipper State Province</asp:Label></TD>
<TD>
<asp:textbox id=txt_s5_shipper_state_province runat="server" Width="160px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="2"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label42 runat="server" DESIGNTIMEDRAGDROP="19367">Shipper Postal Code</asp:Label></TD>
<TD>
<asp:textbox id=txt_s6_shipper_postal_code runat="server" Width="160px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label43 runat="server" DESIGNTIMEDRAGDROP="19843">Shipper Telephone Fax</asp:Label></TD>
<TD>
<asp:textbox id=txt_s7_shipper_telephone_fax runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label44 runat="server" DESIGNTIMEDRAGDROP="20318" Width="186px" ForeColor="Blue">Shipper ISO Country Code *</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_s8_shipper_iso_country_code runat="server" DESIGNTIMEDRAGDROP="1222" Width="170px" BackColor="#FFFFC0"></asp:dropdownlist>
<asp:textbox id=txt_s8_shipper_iso_country_code runat="server" Width="40px" BorderWidth="1px" BorderStyle="Inset" BackColor="#E0E0E0" MaxLength="2"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq19 runat="server" ErrorMessage="(Shipper Info.) - ISO Country Code" ControlToValidate="txt_s8_shipper_iso_country_code" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label45 runat="server" Width="186px">Shipper Contact Name</asp:Label></TD>
<TD>
<asp:textbox id=txt_s9_shipper_contact_name runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR></TABLE></TD></TR></TABLE>
</ContentTemplate>
</igtab:Tab>
<igtab:Tab Text="Consignee Info.">
<ContentTemplate>
<TABLE id=Table11 style="BORDER-RIGHT: thin groove; BORDER-TOP: thin groove; BORDER-LEFT: thin groove; WIDTH: 100%; BORDER-BOTTOM: thin groove">
<TR>
<TD>
<TABLE id=Table2>
<TR>
<TD>
<asp:Label id=Label46 runat="server" DESIGNTIMEDRAGDROP="21158" ForeColor="Blue">Consignee Name *</asp:Label></TD>
<TD>
<asp:textbox id=txt_c1_consignee_name runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq20 runat="server" DESIGNTIMEDRAGDROP="14693" ErrorMessage="(Consignee Info.) - Consignee Name" ControlToValidate="txt_c1_consignee_name" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label47 runat="server" DESIGNTIMEDRAGDROP="21542" ForeColor="Blue">Consignee Address 1 *</asp:Label></TD>
<TD>
<asp:textbox id=txt_c2_consignee_address1 runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq21 runat="server" DESIGNTIMEDRAGDROP="15102" ErrorMessage="(Consignee Info.) - Consignee Address 1" ControlToValidate="txt_c2_consignee_address1" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label48 runat="server" DESIGNTIMEDRAGDROP="22045">Consignee Address 2</asp:Label></TD>
<TD>
<asp:textbox id=txt_c3_consignee_address2 runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label49 runat="server" DESIGNTIMEDRAGDROP="22547" ForeColor="Blue">Consignee City *</asp:Label></TD>
<TD>
<asp:textbox id=txt_c4_consignee_city runat="server" Width="160px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="19"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq22 runat="server" DESIGNTIMEDRAGDROP="15715" ErrorMessage="(Consignee Info.) - Consignee City" ControlToValidate="txt_c4_consignee_city" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label50 runat="server" DESIGNTIMEDRAGDROP="23049" ForeColor="#C04000" Font-Italic="True">Consignee State/Province</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_c5_consignee_state_province runat="server" DESIGNTIMEDRAGDROP="1222" Width="170px" BackColor="#FFFFC0"></asp:dropdownlist>
<asp:textbox id=txt_c5_consignee_state_province runat="server" Width="40px" BorderWidth="1px" BorderStyle="Inset" BackColor="#E0E0E0"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label51 runat="server" DESIGNTIMEDRAGDROP="23551" ForeColor="#C04000" Font-Italic="True">Consignee Postal Code</asp:Label></TD>
<TD>
<asp:textbox id=txt_c6_consignee_postal_code runat="server" Width="70px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="9"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label52 runat="server" DESIGNTIMEDRAGDROP="24054">Consignee Telephone/Fax</asp:Label></TD>
<TD>
<asp:textbox id=txt_c7_consignee_telephone_fax runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label53 runat="server" DESIGNTIMEDRAGDROP="24556" ForeColor="Blue">Consignee ISO Country Code *</asp:Label></TD>
<TD>
<asp:textbox id=txt_c8_consignee_iso_country_code runat="server" Width="40px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="2"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq23 runat="server" ErrorMessage="(Consignee Info.) - Consignee ISO Country Code" ControlToValidate="txt_c8_consignee_iso_country_code" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label54 runat="server">Consignee Contact Name</asp:Label></TD>
<TD>
<asp:textbox id=txt_c9_consignee_contact_name runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR></TABLE>
<asp:RequiredFieldValidator id=rq_txt_c5_consignee_state_province runat="server" ErrorMessage="(Consignee Info.) - Consignee State/Province" ControlToValidate="txt_c5_consignee_state_province" Display="None"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator id=rq_txt_c6_consignee_postal_code runat="server" ErrorMessage="(Consignee Info.) - Consignee Postal Code" ControlToValidate="txt_c6_consignee_postal_code" Display="None"></asp:RequiredFieldValidator></TD></TR></TABLE>
</ContentTemplate>
</igtab:Tab>
<igtab:Tab Text="Notify Info.">
<ContentTemplate>
<TABLE id=Table12 style="BORDER-RIGHT: thin groove; BORDER-TOP: thin groove; BORDER-LEFT: thin groove; WIDTH: 100%; BORDER-BOTTOM: thin groove">
<TR>
<TD>
<TABLE id=Table23>
<TR>
<TD>
<asp:Label id=Label55 runat="server" DESIGNTIMEDRAGDROP="25442" ForeColor="Blue">Notify Name *</asp:Label></TD>
<TD>
<asp:textbox id=txt_n1_notify_name runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq24 runat="server" DESIGNTIMEDRAGDROP="17040" ErrorMessage="(Notify Info.) - Notify Name" ControlToValidate="txt_n1_notify_name" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label56 runat="server" DESIGNTIMEDRAGDROP="25845" ForeColor="Blue">Notify Address 1 *</asp:Label></TD>
<TD>
<asp:textbox id=txt_n2_notify_address1 runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq25 runat="server" DESIGNTIMEDRAGDROP="17458" ErrorMessage="(Notify Info.) - Notify Address 1" ControlToValidate="txt_n2_notify_address1" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label57 runat="server" DESIGNTIMEDRAGDROP="26375">Notify Address 2</asp:Label></TD>
<TD>
<asp:textbox id=txt_n3_notify_address2 runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label58 runat="server" DESIGNTIMEDRAGDROP="26904" ForeColor="Blue">Notify City *</asp:Label></TD>
<TD>
<asp:textbox id=txt_n4_notify_city runat="server" Width="160px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="19"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq26 runat="server" DESIGNTIMEDRAGDROP="18084" ErrorMessage="(Notify Info.) - Notify City" ControlToValidate="txt_n4_notify_city" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label59 runat="server" DESIGNTIMEDRAGDROP="27433" ForeColor="#C04000" Font-Italic="True">Notify State/Province</asp:Label></TD>
<TD>
<asp:dropdownlist id=dl_n5_notify_state_province runat="server" DESIGNTIMEDRAGDROP="1222" Width="170px" BackColor="#FFFFC0"></asp:dropdownlist>
<asp:textbox id=txt_n5_notify_state_province runat="server" Width="40px" BorderWidth="1px" BorderStyle="Inset" BackColor="#E0E0E0"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label60 runat="server" DESIGNTIMEDRAGDROP="27962" ForeColor="#C04000" Font-Italic="True">Notify postal code</asp:Label></TD>
<TD>
<asp:textbox id=txt_n6_notify_postal_code runat="server" Width="70px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="9"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label61 runat="server" DESIGNTIMEDRAGDROP="28491" ForeColor="#C04000" Font-Italic="True">Notify Telephone/Fax</asp:Label></TD>
<TD>
<asp:textbox id=txt_n7_notify_telephone_fax runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR>
<TR>
<TD>
<asp:Label id=Label62 runat="server" DESIGNTIMEDRAGDROP="29020" ForeColor="Blue">Notify ISO Country Code *</asp:Label></TD>
<TD>
<asp:textbox id=txt_n8_notify_iso_country_code runat="server" Width="40px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="2"></asp:textbox></TD>
<TD>
<asp:RequiredFieldValidator id=rq27 runat="server" ErrorMessage="(Notify Info.) - Notify ISO Country Code" ControlToValidate="txt_n8_notify_iso_country_code" Display="None"></asp:RequiredFieldValidator></TD></TR>
<TR>
<TD>
<asp:Label id=Label63 runat="server">Notify Party Contact Name</asp:Label></TD>
<TD>
<asp:textbox id=txt_n9_notify_party_contact_name runat="server" Width="250px" BorderWidth="1px" BorderStyle="Inset" BackColor="#FFFFC0" MaxLength="35"></asp:textbox></TD>
<TD></TD></TR></TABLE>
<asp:RequiredFieldValidator id=rq_txt_n5_notify_state_province runat="server" ErrorMessage="(Notify Info.) - Notify State/Province" ControlToValidate="txt_n5_notify_state_province" Display="None"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator id=rq_txt_n6_notify_postal_code runat="server" ErrorMessage="(Notify Info.) - Notify postal code" ControlToValidate="txt_n6_notify_postal_code" Display="None"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator id=rq_txt_n7_notify_telephone_fax runat="server" ErrorMessage="(Notify Info.) - Notify Telephone/Fax" ControlToValidate="txt_n7_notify_telephone_fax" Display="None"></asp:RequiredFieldValidator></TD></TR></TABLE>
</ContentTemplate>
</igtab:Tab>
</Tabs>
</igtab:ultrawebtab></TD></TR>
  <TR>
    <TD style="HEIGHT: 293px" vAlign=top align=left width="99%" 
    ><igtbl:ultrawebgrid id=UltraWebGrid1 runat="server" Width="860px" Height="300px">
<DisplayLayout AllowDeleteDefault="Yes" ColWidthDefault="80px" AllowAddNewDefault="Yes" RowHeightDefault="20px" Version="4.00" ViewType="Hierarchical" HeaderClickActionDefault="SortMulti" BorderCollapseDefault="Separate" Name="UltraWebGrid1" CellClickActionDefault="Edit" NoDataMessage="Please select a rate type. And then click the execute button." AllowUpdateDefault="Yes">

<AddNewBox Hidden="False" Prompt="Enter New Information..." Location="Top">

<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" StyleBottom="Outset" WidthTop="1px" StyleTop="Outset" StyleRight="Outset" StyleLeft="Outset" ColorLeft="White">
</BorderDetails>

</Style>

<ButtonStyle Width="70px" Cursor="Hand" BackgroundImage="../../../Images/button_add.gif" CustomRules="background-repeat:no-repeat">
</ButtonStyle>

</AddNewBox>

<Pager>

<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

</Style>

</Pager>

<HeaderStyleDefault BorderStyle="Solid" ForeColor="Black" BackColor="#F9ECD4" Height="17px">

<BorderDetails WidthLeft="0px" StyleBottom="Solid" ColorBottom="249, 236, 212" WidthRight="0px" StyleTop="None" StyleRight="None" WidthBottom="1px" StyleLeft="None">
</BorderDetails>

</HeaderStyleDefault>

<RowSelectorStyleDefault Cursor="Hand" BackColor="#F9ECD4" CustomRules="background-position:center center;background-repeat:no-repeat">

<BorderDetails WidthLeft="0px" ColorBottom="224, 224, 224" WidthTop="0px" WidthRight="0px" WidthBottom="0px">
</BorderDetails>

</RowSelectorStyleDefault>

<FrameStyle Width="860px" BorderWidth="1px" Font-Size="8pt" Font-Names="Tahoma" BorderColor="#CEA556" BorderStyle="Solid" Height="300px">
</FrameStyle>

<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="#CFDDF0">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

</FooterStyleDefault>

<ClientSideEvents AfterCellUpdateHandler="acuh" ValueListSelChangeHandler="vlsch" BeforeEnterEditModeHandler="beemh" CellClickHandler="cch" AfterRowInsertHandler="arih">
</ClientSideEvents>

<GroupByBox>

<BandLabelStyle BackColor="White">
</BandLabelStyle>

</GroupByBox>

<EditCellStyleDefault BorderWidth="0px" BorderStyle="None">
</EditCellStyleDefault>

<RowStyleDefault BorderWidth="1px" BorderColor="#F9ECD4" BorderStyle="Solid">

<Padding Left="3px">
</Padding>

<BorderDetails WidthLeft="0px" WidthTop="0px" WidthRight="1px" StyleLeft="None">
</BorderDetails>

</RowStyleDefault>

</DisplayLayout>

<Bands>
<igtbl:UltraGridBand>
<RowEditTemplate>
<P style="BACKGROUND-COLOR: #ccff66" align=center>
<TABLE id=Table18 cellSpacing=2 cellPadding=1 width=300 border=0>
<TR>
<TD></TD>
<TD></TD>
<TD></TD></TR>
<TR>
<TD></TD>
<TD>
<TABLE id=Table2 style="WIDTH: 558px; HEIGHT: 376px" cellSpacing=1 cellPadding=1 width=558 bgColor=#cccc66 border=0>
<TR>
<TD></TD>
<TD vAlign=top align=left>
<TABLE id=Table1 style="WIDTH: 246px; HEIGHT: 361px" cellSpacing=1 cellPadding=1 width=246 border=0 DESIGNTIMEDRAGDROP="822">
<TR>
<TD bgColor=#99cc66></TD>
<TD bgColor=#99cc66></TD></TR>
<TR>
<TD>Quantity</TD>
<TD><INPUT id=txt_i2_quantity style="WIDTH: 100px" type=text maxLength=10 name=txt_i2_quantity columnKey="i2_quantity"></TD></TR>
<TR>
<TD>Net Weight</TD>
<TD><INPUT id=txt_i3_net_weight style="WIDTH: 100px" type=text maxLength=10 name=txt_i3_net_weight columnKey="i3_net_weight"></TD></TR>
<TR>
<TD>Volume</TD>
<TD><INPUT id=txt_i4_volume style="WIDTH: 100px" type=text maxLength=10 name=txt_i4_volume columnKey="i4_volume"></TD></TR>
<TR>
<TD>Package Type</TD>
<TD><INPUT id=txt_i5_package_type style="WIDTH: 40px" type=text maxLength=3 name=txt_i5_package_type columnKey="i5_package_type"></TD></TR>
<TR>
<TD>Comodity Code</TD>
<TD><INPUT id=txt_i6_comodity_code style="WIDTH: 100px" type=text maxLength=11 name=txt_i6_comodity_code columnKey="i6_comodity_code"></TD></TR>
<TR>
<TD>Cash Value</TD>
<TD><INPUT id=txt_i7_cash_value style="WIDTH: 100px" type=text maxLength=8 name=txt_i7_cash_value columnKey="i7_cash_value"></TD></TR>
<TR>
<TD>Equipment Number</TD>
<TD><INPUT id=txt_e1_equipment_number style="WIDTH: 100px" type=text maxLength=14 name=txt_e1_equipment_number columnKey="e1_equipment_number"></TD></TR>
<TR>
<TD>Seal Number 1</TD>
<TD><INPUT id=txt_e2_seal_number1 style="WIDTH: 100px" type=text maxLength=15 name=txt_e2_seal_number1 columnKey="e2_seal_number1"></TD></TR>
<TR>
<TD>Seal Number 2</TD>
<TD><INPUT id=txt_e3_seal_number2 style="WIDTH: 100px" type=text maxLength=15 name=txt_e3_seal_number2 columnKey="e3_seal_number2"></TD></TR>
<TR>
<TD>Length</TD>
<TD><INPUT id=txt_e4_length style="WIDTH: 40px" type=text maxLength=5 name=txt_e4_length columnKey="e4_length"></TD></TR>
<TR>
<TD>Width</TD>
<TD><INPUT id=txt_e5_width style="WIDTH: 40px" type=text maxLength=8 name=txt_e5_width columnKey="e5_width"></TD></TR>
<TR>
<TD>Height</TD>
<TD><INPUT id=txt_e6_height style="WIDTH: 40px" type=text maxLength=8 name=txt_e6_height columnKey="e6_height"></TD></TR>
<TR>
<TD>ISO Equipment</TD>
<TD><INPUT id=txt_e7_iso_equipment style="WIDTH: 40px" type=text maxLength=4 name=txt_e7_iso_equipment columnKey="e7_iso_equipment"></TD></TR>
<TR>
<TD>Type of Service</TD>
<TD><INPUT id=txt_e8_type_of_service style="WIDTH: 40px" type=text maxLength=2 name=txt_e8_type_of_service columnKey="e8_type_of_service"></TD></TR>
<TR>
<TD>Loaded/Empty/Total</TD>
<TD><SELECT name=txt_e9_loaded_empty_total columnKey="e9_loaded_empty_total"> <OPTION value=" " selected></OPTION> <OPTION value=L>Loaded</OPTION> <OPTION value=E>Empty</OPTION> <OPTION value=T>Total</OPTION></SELECT> 
<TR>
<TD bgColor=#99cc66></TD>
<TD bgColor=#99cc66></TD></TR>
<TR>
<TD>Equipment Desc. Code</TD>
<TD><INPUT id=txt_e10_equipment_desc_code style="WIDTH: 40px" type=text maxLength=2 name=txt_e10_equipment_desc_code columnKey="e10_equipment_desc_code"></TD></TR></TABLE></TD>
<TD vAlign=top align=left>
<TABLE id=Table15 style="WIDTH: 391px; HEIGHT: 221px" cellSpacing=1 cellPadding=1 width=391 border=0>
<TR>
<TD bgColor=#99cc66></TD>
<TD bgColor=#99cc66></TD></TR>
<TR>
<TD>Line of Description</TD>
<TD><INPUT id=txt_d1_line_of_description style="WIDTH: 250px" type=text maxLength=45 name=txt_d1_line_of_description columnKey="d1_line_of_description"></TD></TR>
<TR>
<TD>Line of Marks and Numbers</TD>
<TD><INPUT id=txt_m1_line_of_marks_and_numbers style="WIDTH: 250px" type=text maxLength=45 name=txt_m1_line_of_marks_and_numbers columnKey="m1_line_of_marks_and_numbers"></TD></TR>
<TR>
<TD bgColor=#99cc66></TD>
<TD bgColor=#99cc66></TD></TR>
<TR>
<TD>Hazard Code</TD>
<TD><INPUT id=txt_h1_hazard_code style="WIDTH: 100px" type=text maxLength=10 name=txt_h1_hazard_code columnKey="h1_hazard_code"></TD></TR>
<TR>
<TD>Hazard Class</TD>
<TD><INPUT id=txt_h2_hazard_class style="WIDTH: 40px" type=text maxLength=4 name=txt_h2_hazard_class columnKey="h2_hazard_class"></TD></TR>
<TR>
<TD>Hazard Description</TD>
<TD><INPUT id=txt_h3_hazard_description style="WIDTH: 250px" type=text maxLength=30 name=txt_h3_hazard_description columnKey="h3_hazard_description"></TD></TR>
<TR>
<TD>Hazard Contact</TD>
<TD><INPUT id=txt_h4_hazard_contact style="WIDTH: 100px" type=text maxLength=24 name=txt_h4_hazard_contact columnKey="h4_hazard_contact"></TD></TR>
<TR>
<TD>UN Page Number</TD>
<TD><INPUT id=txt_h5_un_page_number style="WIDTH: 40px" type=text maxLength=6 name=txt_h5_un_page_number columnKey="h5_un_page_number"></TD></TR>
<TR>
<TD>Flashpoint Temperature</TD>
<TD><INPUT id=txt_h6_flashpoint_temperature style="WIDTH: 40px" type=text maxLength=3 name=txt_h6_flashpoint_temperature columnKey="h6_flashpoint_temperature"></TD></TR>
<TR>
<TD>Hazard Code Qualifier</TD>
<TD><INPUT id=txt_h7_hazard_code_qualifier style="WIDTH: 40px" type=text maxLength=1 name=txt_h7_hazard_code_qualifier columnKey="h7_hazard_code_qualifier"></TD></TR>
<TR>
<TD>Hazard Unit of Measure</TD>
<TD><INPUT id=txt_h8_hazard_unit_of_measure style="WIDTH: 40px" type=text maxLength=2 name=txt_h8_hazard_unit_of_measure columnKey="h8_hazard_unit_of_measure"></TD></TR>
<TR>
<TD>Negative Indigator</TD>
<TD><SELECT name=txt_h9_negative_indigator columnKey="h9_negative_indigator"> <OPTION value=" " selected></OPTION> <OPTION value=Y>Y</OPTION> <OPTION value=N>N</OPTION></SELECT> </TD></TR>
<TR>
<TD>Hazard Label</TD>
<TD><INPUT id=txt_h10_hazard_label style="WIDTH: 250px" type=text maxLength=30 name=txt_h10_hazard_label columnKey="h10_hazard_label"></TD></TR>
<TR>
<TD>Hazard Classification</TD>
<TD><INPUT id=txt_h11_hazard_classification style="WIDTH: 250px" type=text maxLength=30 name=txt_h11_hazard_classification columnKey="h11_hazard_classification"></TD></TR></TABLE></TD></TR></TABLE></TD>
<TD></TD></TR>
<TR>
<TD></TD>
<TD></TD>
<TD></TD></TR></TABLE><INPUT id=igtbl_reOkBtn style="WIDTH: 50px" onclick=igtbl_gRowEditButtonClick(event); type=button value=OK>&nbsp; <INPUT id=igtbl_reCancelBtn style="WIDTH: 50px" onclick=igtbl_gRowEditButtonClick(event); type=button value=Cancel> </P>
</RowEditTemplate>
</igtbl:UltraGridBand>
</Bands>
</igtbl:ultrawebgrid></TD></TR>
  <TR>
    <TD style="HEIGHT: 22px" vAlign=top align=left width="99%" 
    ><IMG style="CURSOR: hand" onclick=javascript:SelectAllRows() alt="Select All" src="Images/button_selectall.GIF" width="61" height="17" > <IMG style="CURSOR: hand" onclick=javascript:unSelectAllRows() alt="Clear All" src="Images/button_clear.gif"  DESIGNTIMEDRAGDROP="1538" width="56" height="17"> <IMG style="CURSOR: hand" onclick=javascript:DeleteRows() alt="Delete selected rows" src="Images/button_delete.gif"  DESIGNTIMEDRAGDROP="415" width="50" height="17"> <IMG style="CURSOR: hand" onclick=javascript:NumberingRows() alt=Numbering src="Images/circle_1.gif" width="23" height="22" > </TD></TR>
  <TR>
    <TD style="HEIGHT: 100%" vAlign=top align=left width="99%" 
    ><FONT face=����></FONT></TD></TR></TBODY></TABLE>
<P><FONT face=����></FONT>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P>&nbsp;</P>
<P><asp:button id=btnNew runat="server" DESIGNTIMEDRAGDROP="1398" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnSave runat="server" DESIGNTIMEDRAGDROP="1397" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnCancel runat="server" DESIGNTIMEDRAGDROP="1400" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnShow runat="server" DESIGNTIMEDRAGDROP="1401" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnEdit runat="server" DESIGNTIMEDRAGDROP="1402" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnDelete runat="server" DESIGNTIMEDRAGDROP="1403" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnDoSchedule runat="server" DESIGNTIMEDRAGDROP="1396" Width="60px" Font-Size="Larger" Visible="False"></asp:button><asp:button id=btnTmpNoDelete runat="server" DESIGNTIMEDRAGDROP="1404" Width="60px" Font-Size="Larger" Visible="False"></asp:button></P>
<P>
<igsch:WebCalendar id=CustomDropDownCalendar runat="server" Width="150px">
<Layout FooterFormat="Today: {0:d}" TitleFormat="Month" ShowYearDropDown="False" PrevMonthImageUrl="ig_cal_blueP0.gif" ShowMonthDropDown="False" NextMonthImageUrl="ig_cal_blueN0.gif">

<DayStyle BackColor="#7AA7E0">
</DayStyle>

<FooterStyle Height="16pt" Font-Size="8pt" ForeColor="#505080" BackgroundImage="ig_cal_blue2.gif">

<BorderDetails ColorTop="LightSteelBlue" WidthTop="1px" StyleTop="Solid">
</BorderDetails>

</FooterStyle>

<SelectedDayStyle BackColor="SteelBlue">
</SelectedDayStyle>

<OtherMonthDayStyle ForeColor="SlateGray">
</OtherMonthDayStyle>

<NextPrevStyle BackgroundImage="ig_cal_blue1.gif">
</NextPrevStyle>

<CalendarStyle Width="150px" BorderWidth="1px" Font-Size="9pt" Font-Names="Verdana" BorderColor="SteelBlue" BorderStyle="Solid" BackColor="#E0EEFF">
</CalendarStyle>

<TodayDayStyle BackColor="#97B0F0">
</TodayDayStyle>

<DayHeaderStyle Height="1pt" Font-Size="8pt" Font-Bold="True" ForeColor="#606090" BackColor="#7AA7E0">

<BorderDetails StyleBottom="Solid" ColorBottom="LightSteelBlue" WidthBottom="1px">
</BorderDetails>

</DayHeaderStyle>

<TitleStyle Height="18pt" Font-Size="10pt" Font-Bold="True" ForeColor="#505080" BackgroundImage="ig_cal_blue1.gif" BackColor="#CCDDFF">
</TitleStyle>

</Layout>
</igsch:WebCalendar></P>
<P><igtxt:webnumericedit id=WebNumericEdit1 runat="server"></igtxt:webnumericedit></P>
<P><asp:validationsummary id=ValidationSummary1 runat="server" Width="344px" ForeColor="Blue" DisplayMode="List" ShowSummary="False" HeaderText="Required Data field error (AMS Header):" ShowMessageBox="True"></asp:validationsummary></P></form>
<SCRIPT type=text/javascript>
			ig_initDropCalendar("CustomDropDownCalendar Webdatetimeedit1 Webdatetimeedit2");
		</SCRIPT>













  </body>
</HTML>
