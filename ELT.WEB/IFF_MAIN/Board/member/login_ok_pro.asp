<% Option Explicit %>
<% response.buffer = true %>

 

<% dim sql,rs %>

<!-- #include file="../inc/dbinfo.asp" -->
<!-- #include file="../inc/joint.asp" -->

<%
	dim num,join_id,join_pin,url,point,updatesql
	
'	h_url = Request("h_url")
	
	join_id = Request("join_id")
	join_pin = Request("join_pin")
	
	
	call f_member
	
	SQL = "Select * From member Where elt_account_number=" & elt_account_number & "AND id ='"& join_id &"'"
	Set rs = db.Execute(SQL)
	
	if rs.eof or rs.bof then Response.Redirect "../inc/error.asp?no=16"
	
'	SQL = "Select * From member Where id ='"& join_id &"' and pin='"& join_pin &"'"
'	Set rs = db.Execute(SQL)
	
		
'	if rs.eof or rs.bof then Response.Redirect "../inc/error.asp?no=2"
	



	'############# 로그인할려는 아이디가 이미 로그인 되어있는지 확인 ########################
	
	
	dim sql_info
	
	SQL_info = "Update view_login Set alive = 0, user_id ='invalid',user_name='invalid',login_time='"&now&"',u_time='"&now&"',logout = 1 where user_id='"&join_id&"' and ip<>'"&session_ip&"'"
	eltConn.Execute SQL_info		'### 강제종료를 하면서 같은 아이디로 사용중인 기존 사용자의 정보를 손님으로 고침

	'############# 로그인할려는 아이디가 이미 로그인 되어있는지 확인 끝부분 ########################

	session_uid = rs("id")
	session_pin = rs("pin")
	session_login_name = rs("name")
	session_email = rs("email")
	session_url = rs("url")
	session_level = rs("user_level")

	if level_select=1 then
		call point_up
	end if

	dim h_url1
	h_url = instr(q_info,"h_url=")
	h_url1 = len(q_info)
	h_url = h_url+5
	h_url = h_url1-h_url
	h_url = right(q_info,h_url)
	
	if rs("id")="admin" then
	 	 If elt_account_number = "80002000" then
		  session_admin = "admin"
	 	  session_level=1	
    	 End if 
	end if
	
	'### 쿠키 설정부분 시작
	dim auto_login
	auto_login = request("auto_login")
	
	if auto_login = "1" then
'		Response.Cookies("a_login").expires = #12/31/2020 00:00:00#
'		Response.Cookies("a_login")("auto_login") = 1
'		Response.Cookies("a_login")("id") = join_id
'		Response.Cookies("a_login")("pin") = join_pin
	else
'		Response.Cookies("a_login").expires = #12/31/2020 00:00:00#
'		Response.Cookies("a_login")("auto_login") = 0
'		Response.Cookies("a_login")("id") = ""
'		Response.Cookies("a_login")("pin") = ""
	end if
	'### 쿠키 설정 부분 끝
	
	
	
	'##################### 로그인하면 접속자정보에 invalid 에서 회원으로 정보변경 ################33
	
	UPDATESQL = "Update view_login Set alive = 0, user_id ='"&rs("id")&"',user_name='"&rs("name")&"',login_time='"&now&"',u_time='"&now&"' where ip='"&session_ip&"'" 
	eltConn.Execute UPDATESQL
	
	'##################### 로그인하면 접속자정보에 invalid 에서 회원으로 정보변경 ################33
	rs.close
	db.Close
	Set rs=nothing
	Set db=nothing
	
	
	h_url = instr(q_info,"h_url=")
	h_url1 = len(q_info)
	h_url = h_url+5
	h_url = h_url1-h_url
	h_url = right(q_info,h_url)
	
'	Response.Redirect h_url
	
%>
