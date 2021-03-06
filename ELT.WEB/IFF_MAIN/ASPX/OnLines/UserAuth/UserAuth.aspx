<%@ Page language="c#" Inherits="IFF_MAIN.ASPX.OnLines.UserAuth.UserAuth" CodeFile="UserAuth.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics.WebUI.UltraWebToolbar, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics.WebUI.UltraWebGrid, Version=11.1.20111.2064, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>User Authorization</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
		<meta content=C# name=CODE_LANGUAGE>
		<meta content=JavaScript name=vs_defaultClientScript>
		<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
		<LINK href="/IFF_MAIN/ASPX/CSS/AppStyle.css" type=text/css rel=stylesheet>
		<script language=javascript>

function viewPop(Url) {
var strJavaPop = "";
window.open(Url,'popup','menubar=0, scrollbars=1, staus=0, resizable=1, titlebar=0, toolbar=0, hotkey=0,closeable=0'); 
}
		
function ReqDataCheck() {
	if( document.form1.ComboBox1.value != "") {
		document.form1.txtNum.value = document.form1.ComboBox1.selectedIndex;
		return true;
	 }
}

function cch(gridName,cellId,button) {

var SelectedChild = 'url(../../../Images/mark_o.gif)';
var SelectedParent = 'url(../../../Images/mark_o.gif)';
var row=igtbl_getRowById(cellId);
var cell=igtbl_getCellById(cellId);
var oCell = igtbl_getCellById(cellId);
var cUrl = oCell.Element.style.backgroundImage;

			if(cell.Column.Key=="Chk") {
				if(cUrl==SelectedChild) {
					oCell.Element.style.backgroundImage =  'url(../../../Images/mark_x.gif)';
					row.setSelected(true);
				}
				else {
					oCell.Element.style.backgroundImage =  'url(../../../Images/mark_o.gif)';
					row.setSelected(true);
				}
			}
}


function SelectAllRows(strGrid) {

var oGrid = igtbl_getGridById(strGrid);
var oRows = oGrid.Rows;
oGrid.suspendUpdates(true);
	for(i=0; i<oRows.length; i++) {
		oRow = oRows.getRow(i);
		oRow.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_x.gif)';
	}		
	
	oUltraWebGrid2.suspendUpdates(false);
}

function unSelectAllRows(strGrid) {
	
var oGrid = igtbl_getGridById(strGrid);
var oRows = oGrid.Rows;
oGrid.suspendUpdates(true);
	for(i=0; i<oRows.length; i++) {
		oRow = oRows.getRow(i);
		oRow.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_o.gif)';
	}		
		
	oUltraWebGrid2.suspendUpdates(false);

}

function toLeftRow() {
var oGrid1 = igtbl_getGridById('UltraWebGrid1');
var oGrid2 = igtbl_getGridById('UltraWebGrid2');
var oRows1 = oGrid1.Rows;
var oRows2 = oGrid2.Rows;
var errString = "";

	for(i=0; i<oRows2.length; i++) {
		oRow = oRows2.getRow(i);
		if( oRow.getCellFromKey('Chk').Element.style.backgroundImage ==  'url(../../../Images/mark_x.gif)' ) {
			keyVal = oRow.getCellFromKey('Page_Id').getValue();			
			keyValModule = oRow.getCellFromKey('Module').getValue();			
			keyValPage = oRow.getCellFromKey('Page').getValue();			
			if(!findDupKey(oGrid1,keyVal)) {
				errString = errString+ keyValModule + '->' + keyValPage +"\n";
				oRow.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_o.gif)';
			}
		}
	}

	if (errString.length > 0) {
		alert(errString + " Aready exists.");
		return;
	}
		
	for(i=0; i<oRows2.length; i++) {
		oRow = oRows2.getRow(i);
		if( oRow.getCellFromKey('Chk').Element.style.backgroundImage ==  'url(../../../Images/mark_x.gif)' ) {
			rowObj = igtbl_addNew(oGrid1.Id,'0');
			rowObj.getCellFromKey('Page_Id').setValue(oRow.getCellFromKey('Page_Id').getValue());
			rowObj.getCellFromKey('Module').setValue(oRow.getCellFromKey('Module').getValue());
			rowObj.getCellFromKey('Page').setValue(oRow.getCellFromKey('Page').getValue());
			rowObj.getCellFromKey('Description').setValue(oRow.getCellFromKey('Description').getValue());
			rowObj.getCellFromKey('Type').setValue(oRow.getCellFromKey('Type').getValue());
			rowObj.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_x.gif)' 			
		}
	}

	DeleteRows(oGrid2.Id);
	
}

function AddRow(strGrid) {
var oGrid1 = igtbl_getGridById(strGrid);
var oRows1 = oGrid1.Rows;
var errString = "";
igtbl_addNew(oGrid1.Id,'0');	
}

function findDupKey(oGrid,keyVal) {
var oRows = oGrid.Rows;

	for(j=0; j<oRows.length; j++) {
		oRowTmp = oRows.getRow(j);
			if( oRowTmp.getCellFromKey('Page_Id').getValue() == keyVal) {
				return false;
			}			
	}

return true;

}


function toRightRow() {
var oGrid1 = igtbl_getGridById('UltraWebGrid1');
var oGrid2 = igtbl_getGridById('UltraWebGrid2');
var oRows1 = oGrid1.Rows;
var oRows2 = oGrid2.Rows;

	for(i=0; i<oRows1.length; i++) {
		oRow = oRows1.getRow(i);
		if( oRow.getCellFromKey('Chk').Element.style.backgroundImage ==  'url(../../../Images/mark_x.gif)' ) {
			rowObj = igtbl_addNew(oGrid2.Id,'0');
			rowObj.getCellFromKey('Page_Id').setValue(oRow.getCellFromKey('Page_Id').getValue());
			rowObj.getCellFromKey('Module').setValue(oRow.getCellFromKey('Module').getValue());
			rowObj.getCellFromKey('Page').setValue(oRow.getCellFromKey('Page').getValue());
			rowObj.getCellFromKey('Description').setValue(oRow.getCellFromKey('Description').getValue());
			rowObj.getCellFromKey('Type').setValue(oRow.getCellFromKey('Type').getValue());
			rowObj.getCellFromKey('Chk').Element.style.backgroundImage =  'url(../../../Images/mark_x.gif)' 			
		}
	}		
	DeleteRows(oGrid1.Id);	
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

function DeleteRows(strGrid) {

var oGrid = igtbl_getGridById(strGrid);
var oRows = oGrid.Rows;
oGrid.suspendUpdates(true);

	for(i=oRows.length-1; i>=0; i--) {
		oRow = oRows.getRow(i);
		if( oRow.getCellFromKey('Chk').Element.style.backgroundImage ==  'url(../../../Images/mark_x.gif)' ) {
		if( oRow )oRow.deleteRow();
		}
	}		
	

oGrid.suspendUpdates(false);

}

function beemh(gridName,cellId) {
var cell=igtbl_getCellById(cellId);
var row=igtbl_getRowById(cellId);

	if(cell.Column.Key=="Chk") {
		igtbl_EndEditMode(gridName);	
		return true;			
	}
}

		</script>
<!--  #INCLUDE FILE="../../include/common.htm" -->
</HEAD>
	<body bottommargin="0" topmargin="0">
		<form id=form1 method=post runat="server">
			<table height=12 cellSpacing=0 cellPadding=0 width="99%" border=0>
				<tr>
					<td vAlign=top align=center><IMG height=6 src="../../../images/spacer.gif" width=200><IMG height=7 src=
					<%     
                    if(Request.UrlReferrer != null && windowName != "PopWin" ) {
                    Response.Write("'../../../images/pointer_md.gif'"); }
                    %> width=11><IMG height=6 src="../../../images/spacer.gif" width=227></td>
				</tr>
			</table>
		
			<TABLE id=Table3 cellSpacing=0 cellPadding=0 width="99%" align=left bgColor=#ffffff>
				<TR>
					<TD style="WIDTH: 1311px; HEIGHT: 16px"><asp:label id=Label8 runat="server" Font-Bold="True" DESIGNTIMEDRAGDROP="214" Width="344px"
							ForeColor="Black" Height="100%" Font-Size="15px">User Authorized Page</asp:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 11px; width: 1311px;" scope="HEIGHT: 12px"><asp:label id=lblError runat="server" Font-Bold="True" DESIGNTIMEDRAGDROP="9515" Width="100%"
							ForeColor="Red" Font-Underline="True" Font-Italic="True"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 1311px; HEIGHT: 14px" bgColor="#dddded"></TD>
				</TR>
                <tr>
                    <td style="height: 50px; width: 1311px;" valign="top">
						<TABLE id=Table8 style="HEIGHT: 50px" cellSpacing=0 cellPadding=0 width="100%"
							border=0>
							<TR>
								<TD vAlign=top style="width: 50%">
									<TABLE id=Table1 cellSpacing=0 cellPadding=0 width="100%"
										border=0 height="100%">
										<TR>
											<TD style="WIDTH: 514px; height: 32px;" vAlign=top align=left>
												<TABLE id=Table6 style="WIDTH: 510px; HEIGHT: 24px" height=24 cellSpacing=2 cellPadding=0
													align=left border=0 DESIGNTIMEDRAGDROP="627">
													<TR>
														<TD align=right style="height: 20px"><asp:label id=Label1 runat="server" Width="100%" Height="19px">Login ID :</asp:label></TD>
														<TD vAlign=top style="height: 20px">
                                                            <asp:DropDownList ID="ComboBox1" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ComboBox1_SelectedIndexChanged">
                                                            </asp:DropDownList></TD>
														<TD vAlign=top align=right style="height: 20px"><asp:imagebutton id=btnGo runat="server" ImageUrl="../../../images/button_go.gif" OnClick="btnGo_Click2" Visible="False"></asp:imagebutton>
                                                        </TD>
													</TR>
													<TR>
														<TD style="height: 19px;" align=right>
                                                            Copy&nbsp; from User :</TD>
														<TD style="WIDTH: 200px; height: 19px;" align=right><asp:DropDownList ID="DropDownList2" runat="server" Width="100%">
                                                        </asp:DropDownList></TD>
														<TD align="right" style="height: 19px"></TD>
													</TR>
                                                    <tr>
                                                        <td align="right" style="height: 19px">
                                                            <asp:DropDownList ID="DropDownList1" runat="server" Width="100%" Visible="False">
                                                            </asp:DropDownList></td>
                                                        <td align="right" style="width: 200px; height: 19px">
                                                            <asp:imagebutton id=btnCopy runat="server" ImageUrl="../../../images/button_go.gif" OnClick="btnCopy_Click"></asp:imagebutton></td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="btnMake" runat="server" ImageUrl="../../../images/button_go.gif"
                                                                OnClick="btnMake_Click" Visible="False" /></td>
                                                    </tr>
												</TABLE>
											</TD>
											<TD vAlign=top align=left style="height: 32px"></TD>
										</TR>
                                        <tr>
                                            <td align="left" style="width: 514px; height: 32px" valign="top">
                                            </td>
                                            <td align="left" style="height: 32px" valign="top">
                                            </td>
                                        </tr>
									</TABLE>
								</TD>
								<TD width="50%" align=right></TD>
							</TR>
						</TABLE>
                    </td>
                </tr>
				<TR>
					<TD style="width: 1311px;" vAlign=top><asp:panel id=Panel1 runat="server" HorizontalAlign="Left" Width="100%">
      <TABLE id=Table2 cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TR>
          <TD>
<asp:label id=strUserInfo runat="server" Font-Size="10px" ForeColor="Navy" Width="359px" Font-Bold="True" BackColor="#DDDDED" Font-Names="Verdana">Allowed pages to this user</asp:label></TD>
          <TD style="WIDTH: 16px" width=16></TD>
          <TD>
<asp:label id=Label7 runat="server" Font-Size="10px" ForeColor="Navy" Width="115px" Font-Bold="True" BackColor="#DDDDED" Font-Names="Verdana">All pages</asp:label></TD></TR>
        <TR>
          <TD width="40%" style="height: 20px">
<asp:ImageButton id=btnBack runat="server" ImageUrl="../../../images/button_back.gif" Visible="False"></asp:ImageButton><FONT 
            face=����>&nbsp; </FONT>
<asp:ImageButton id=btnSave runat="server" ImageUrl="../../../images/button_save.gif" OnClick="btnSave_Click1"></asp:ImageButton></TD>
          <TD style="WIDTH: 16px; height: 20px;" width=16></TD>
          <TD width="60%" style="height: 20px">&nbsp;&nbsp;
<asp:ImageButton id=btnReloadAll runat="server" ImageUrl="../../../images/button_reload_all.gif"></asp:ImageButton></TD></TR>
        <TR>
          <TD vAlign=top align=left style="height: 263px">
<igtbl:ultrawebgrid id=UltraWebGrid1 runat="server" Height="400px" Width="100%" OnInitializeLayout="UltraWebGrid1_InitializeLayout1" >
											<DisplayLayout AllowDeleteDefault="Yes" ColWidthDefault="80px" AllowAddNewDefault="Yes" RowHeightDefault="18px"
												Version="4.00" HeaderClickActionDefault="SortMulti" BorderCollapseDefault="Separate" RowSelectorsDefault="No"
												Name="UltraWebGrid1" TableLayout="Fixed" ViewType="Hierarchical">
												<AddNewBox ButtonConnectorColor="Silver" ButtonConnectorStyle="Solid">
													<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

													</Style>
                                                    <ButtonStyle BackColor="Gray" BorderColor="White" BorderStyle="Outset" BorderWidth="1px"
                                                        Cursor="Hand">
                                                    </ButtonStyle>
												</AddNewBox>
												<Pager Alignment="Center">
													<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

													</Style>
												</Pager>
												<HeaderStyleDefault BorderStyle="Solid" ForeColor="Black" BackColor="#CBD6A6" BorderWidth="1px" Font-Names="Tahoma" Font-Size="8pt" HorizontalAlign="Left">
													<BorderDetails WidthLeft="1px" ColorLeft="White" ColorTop="White" WidthTop="1px"></BorderDetails>
                                                    <Padding Left="5px" Right="5px" />
												</HeaderStyleDefault>
												<FrameStyle Width="100%" BorderWidth="1px" Font-Size="8pt" Font-Names="Tahoma"
													BorderStyle="Solid" Height="400px" BackColor="#FAFCF1" Cursor="Hand"></FrameStyle>
												<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
													<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
												</FooterStyleDefault>
												<ClientSideEvents AfterCellUpdateHandler="acuh" BeforeEnterEditModeHandler="beemh" CellClickHandler="cch"></ClientSideEvents>
												<GroupByBox ButtonConnectorColor="Silver" ButtonConnectorStyle="Solid">
													<BandLabelStyle BackColor="Gray" BorderColor="White" BorderStyle="Outset" BorderWidth="1px" Cursor="Default"></BandLabelStyle>
                                                    <Style BackColor="DarkGray" BorderColor="White" BorderStyle="Outset" BorderWidth="1px"></Style>
												</GroupByBox>
												<EditCellStyleDefault BorderWidth="0px" BorderStyle="None" Font-Names="Tahoma" Font-Size="8pt" HorizontalAlign="Left" VerticalAlign="Middle"></EditCellStyleDefault>
												<RowAlternateStyleDefault BackColor="#E0E0E0">
												</RowAlternateStyleDefault>
												<RowStyleDefault BorderWidth="1px" BorderColor="#AAB883" BorderStyle="Solid" BackColor="White" Font-Names="Tahoma" Font-Size="8pt" ForeColor="#333333" HorizontalAlign="Left" VerticalAlign="Middle">
													<Padding Left="7px" Right="7px"></Padding>
													<BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
												</RowStyleDefault>
                                                <GroupByRowStyleDefault BackColor="DarkGray" BorderColor="White" BorderStyle="Outset"
                                                    BorderWidth="1px">
                                                </GroupByRowStyleDefault>
                                                <ActivationObject BorderColor="170, 184, 131">
                                                </ActivationObject>
                                                <RowExpAreaStyleDefault BackColor="WhiteSmoke">
                                                </RowExpAreaStyleDefault>
                                                <SelectedGroupByRowStyleDefault BackColor="#CF5F5B" BorderColor="White" BorderStyle="Outset"
                                                    BorderWidth="1px" ForeColor="White">
                                                </SelectedGroupByRowStyleDefault>
                                                <ImageUrls CollapseImage="./ig_treeXPMinus.GIF" CurrentEditRowImage="./arrow_brown2_beveled.gif"
                                                    CurrentRowImage="./arrow_brown2_beveled.gif" ExpandImage="./ig_treeXPPlus.GIF" />
                                                <RowSelectorStyleDefault BackColor="White" BorderStyle="None" BorderWidth="1px">
                                                </RowSelectorStyleDefault>
                                                <SelectedRowStyleDefault BackColor="#BECA98" ForeColor="White">
                                                </SelectedRowStyleDefault>
											</DisplayLayout>
											<Bands>
												<igtbl:UltraGridBand AddButtonCaption="Column0Column1Column2" Key="Column0Column1Column2">
                                                    <AddNewRow View="NotSet" Visible="NotSet">
                                                    </AddNewRow>
                                                </igtbl:UltraGridBand>
											</Bands>
										</igtbl:ultrawebgrid></TD>
          <TD style="WIDTH: 16px;"><INPUT onclick=toLeftRow() type=button value="<<" DESIGNTIMEDRAGDROP="576"><INPUT onclick=toRightRow() type=button value=">>"></TD>
          <TD vAlign=top align=left style="height: 263px">
<igtbl:ultrawebgrid id=UltraWebGrid2 runat="server" Height="400px" Width="100%" DESIGNTIMEDRAGDROP="1508" OnInitializeLayout="UltraWebGrid2_InitializeLayout">
											<DisplayLayout AllowDeleteDefault="Yes" ColWidthDefault="80px" AllowAddNewDefault="Yes" RowHeightDefault="18px"
												Version="4.00" HeaderClickActionDefault="SortMulti" BorderCollapseDefault="Separate" RowSelectorsDefault="No"
												Name="UltraWebGrid2" TableLayout="Fixed" ViewType="Hierarchical">
												<AddNewBox ButtonConnectorColor="Silver" ButtonConnectorStyle="Solid">
													<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

													</Style>
                                                    <ButtonStyle BackColor="Gray" BorderColor="White" BorderStyle="Outset" BorderWidth="1px"
                                                        Cursor="Hand">
                                                    </ButtonStyle>
												</AddNewBox>
												<Pager Alignment="Center">
													<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

													</Style>
												</Pager>
												<HeaderStyleDefault BorderStyle="Solid" ForeColor="Black" BackColor="#CBD6A6" BorderWidth="1px" Font-Names="Tahoma" Font-Size="8pt" HorizontalAlign="Left">
													<BorderDetails WidthLeft="1px" ColorLeft="White" ColorTop="White" WidthTop="1px"></BorderDetails>
                                                    <Padding Left="5px" Right="5px" />
												</HeaderStyleDefault>
												<FrameStyle Width="100%" BorderWidth="1px" Font-Size="8pt" Font-Names="Tahoma"
													BorderStyle="Solid" Height="400px" BackColor="#FAFCF1" Cursor="Hand"></FrameStyle>
												<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
													<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
												</FooterStyleDefault>
												<ClientSideEvents AfterCellUpdateHandler="acuh" CellClickHandler="cch"></ClientSideEvents>
												<GroupByBox ButtonConnectorColor="Silver" ButtonConnectorStyle="Solid">
													<BandLabelStyle BackColor="Gray" BorderColor="White" BorderStyle="Outset" BorderWidth="1px" Cursor="Default"></BandLabelStyle>
                                                    <Style BackColor="DarkGray" BorderColor="White" BorderStyle="Outset" BorderWidth="1px"></Style>
												</GroupByBox>
												<EditCellStyleDefault BorderWidth="0px" BorderStyle="None" Font-Names="Tahoma" Font-Size="8pt" HorizontalAlign="Left" VerticalAlign="Middle"></EditCellStyleDefault>
												<RowAlternateStyleDefault BackColor="#E0E0E0">
												</RowAlternateStyleDefault>
												<RowStyleDefault BorderWidth="1px" BorderColor="#AAB883" BorderStyle="Solid" BackColor="White" Font-Names="Tahoma" Font-Size="8pt" ForeColor="#333333" HorizontalAlign="Left" VerticalAlign="Middle">
													<Padding Left="7px" Right="7px"></Padding>
													<BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
												</RowStyleDefault>
                                                <GroupByRowStyleDefault BackColor="DarkGray" BorderColor="White" BorderStyle="Outset"
                                                    BorderWidth="1px">
                                                </GroupByRowStyleDefault>
                                                <ActivationObject BorderColor="170, 184, 131">
                                                </ActivationObject>
                                                <RowExpAreaStyleDefault BackColor="WhiteSmoke">
                                                </RowExpAreaStyleDefault>
                                                <SelectedGroupByRowStyleDefault BackColor="#CF5F5B" BorderColor="White" BorderStyle="Outset"
                                                    BorderWidth="1px" ForeColor="White">
                                                </SelectedGroupByRowStyleDefault>
                                                <ImageUrls CollapseImage="./ig_treeXPMinus.GIF" CurrentEditRowImage="./arrow_brown2_beveled.gif"
                                                    CurrentRowImage="./arrow_brown2_beveled.gif" ExpandImage="./ig_treeXPPlus.GIF" />
                                                <RowSelectorStyleDefault BackColor="White" BorderStyle="None" BorderWidth="1px">
                                                </RowSelectorStyleDefault>
                                                <SelectedRowStyleDefault BackColor="#BECA98" ForeColor="White">
                                                </SelectedRowStyleDefault>
											</DisplayLayout>
											<Bands>
												<igtbl:UltraGridBand AddButtonCaption="Column0Column1Column2" Key="Column0Column1Column2">
                                                    <AddNewRow View="NotSet" Visible="NotSet">
                                                    </AddNewRow>
                                                </igtbl:UltraGridBand>
											</Bands>
										</igtbl:ultrawebgrid></TD></TR>
        <TR>
          <TD style="HEIGHT: 5px" vAlign=top align=left><IMG 
            onclick="SelectAllRows('UltraWebGrid1')" alt="Select All" 
            src="../../../images/button_selectall.gif"><IMG 
            onclick="unSelectAllRows('UltraWebGrid1')" alt="Clear All" 
            src="../../../images/button_clear.gif"><IMG 
            onclick="DeleteRows('UltraWebGrid1')" alt="Delete Checked" 
            src="../../../images/button_delete_ckitem.gif" 
            DESIGNTIMEDRAGDROP="189">&nbsp;<IMG 
            onclick="AddRow('UltraWebGrid1')" alt="Add Item" 
            src="../../../images/button_add_ig.gif"></TD>
          <TD style="WIDTH: 16px; HEIGHT: 5px"></TD>
          <TD style="HEIGHT: 5px; width: 639px;" vAlign=top align=left><IMG 
            onclick="SelectAllRows('UltraWebGrid2')" alt="Select All" 
            src="../../../images/button_selectall.gif"><IMG 
            onclick="unSelectAllRows('UltraWebGrid2')" alt="Clear All" 
            src="../../../images/button_clear.gif"></TD></TR></TABLE>
						</asp:panel></TD>
				</TR>
			</TABLE>
			<asp:textbox id=txtNum runat="server" Width="1px" Height="1px"></asp:textbox></form>
	</body>
<!--  #INCLUDE FILE="../../include/StatusFooter.htm" -->
</HTML>
