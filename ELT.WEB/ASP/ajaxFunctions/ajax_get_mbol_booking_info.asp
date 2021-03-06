<!--  #INCLUDE FILE="../include/transaction.txt" -->
<% 
    Option Explicit
    Response.CharSet = "UTF-8"
    Session.CodePage = "65001"
%>
<!--  #INCLUDE VIRTUAL="/ASP/include/connection.asp" -->
<!--  #INCLUDE VIRTUAL="/ASP/include/AspFunctions.inc" -->
<%  
    Response.Expires = 0  
    Response.AddHeader "Pragma","no-cache"  
    Response.AddHeader "Cache-Control","no-cache,must-revalidate"  



    DIM elt_account_number,vMbol,vBook

    vMbol = Request.QueryString("MBOL")
    if isnull(vMbol) then vMbol = ""
    vBook = Request.QueryString("BOOK")
    if isnull(vBook) then vBook = ""

    elt_account_number = Request.Cookies("CurrentUserInfo")("elt_account_number")

    if vMbol <> "" then
	    call get_mbol_info( vMbol )
	    response.End
    end If

    if vBook <> "" then
	    call get_book_info( vBook )
    end If

    eltConn.Close
    Set eltConn = Nothing

%>

<%
Sub get_mbol_info( MBOL )

    Dim rs, SQL    
	Dim mbolInfo
	Dim oneBookingNum

    set rs=Server.CreateObject("ADODB.Recordset")
	
    if MBOL = "" then Exit sub

	set rs=Server.CreateObject("ADODB.Recordset")

	SQL= "select booking_num,departure_date,origin_port_location,dest_port_location,vsl_name,voyage_no," _
	    & "origin_port_state,mbol_num,move_type,receipt_place,delivery_place,file_no,dest_port_country,vsl_name " _
	    & "from ocean_booking_number where elt_account_number = " & elt_account_number & " and booking_num=N'" & MBOL & "'"
	
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	
	If Not rs.EOF Then			
		oneBookingNum=rs("booking_num")
		mbolInfo= oneBookingNum&chr(10) & rs("departure_date") & chr(10) & rs("origin_port_location") & chr(10) _
		    & rs("dest_port_location") & chr(10) & rs("vsl_name") & " " &  rs("voyage_no") & chr(10) _
		    & rs("origin_port_state") & chr(10) & rs("mbol_num") & chr(10) & rs("move_type") & chr(10) _
		    & rs("receipt_place") & chr(10) & rs("delivery_place") & chr(10) & rs("file_no") & chr(10) _
		    & rs("dest_port_country") & chr(10) & rs("vsl_name")
	End If
	rs.close
	set rs=nothing 
	response.write mbolInfo

End sub
%>

<%
Sub get_book_info( MBOL )

    Dim rs, SQL    
	
	Dim aBookingInfo

    set rs=Server.CreateObject("ADODB.Recordset")
	
    if MBOL = "" then Exit sub

	set rs=Server.CreateObject("ADODB.Recordset")

    SQL= "select booking_num,departure_date,origin_port_location,dest_port_location,vsl_name,voyage_no," _
        & "origin_port_state,mbol_num,move_type,receipt_place,delivery_place,file_no,dest_port_country " _
        & "from ocean_booking_number where elt_account_number = " & elt_account_number & " and booking_num=N'" & MBOL & "'"

	
	rs.CursorLocation = adUseClient
	rs.Open SQL, eltConn, adOpenForwardOnly, , adCmdText
	Set rs.activeConnection = Nothing
	
	If Not rs.EOF Then			
	    aBookingInfo=rs("departure_date") & chr(10) & rs("origin_port_location") & chr(10) _
	        & rs("dest_port_location") & chr(10) & rs("vsl_name") & " " & rs("voyage_no") & chr(10) _
	        & rs("origin_port_state") & chr(10) & rs("mbol_num") & chr(10) & rs("move_type") & chr(10) _
	        & rs("receipt_place") & chr(10) & rs("delivery_place") & chr(10) & rs("file_no") & chr(10) _
	        & rs("dest_port_country") & chr(10) & rs("vsl_name")
	End If
	rs.close
	set rs=nothing 
	response.write aBookingInfo

End sub
%>

