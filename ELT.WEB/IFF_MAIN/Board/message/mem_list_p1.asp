 

<% m_box=request("m_box") %>


<!-- #include file="../inc/dbinfo.asp" -->
<!-- #include file="../inc/info_tb.asp" -->
<!-- #include file="../inc/joint.asp" -->
<HTML>
<HEAD>
<TITLE>Memo Box</TITLE>
<META HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=euc-kr">
<LINK rel="stylesheet" type="text/css" href="../inc/style.css">

<%
Dim Memo_list_0,Memo_list_1

If tb = "inno_1" Then
	Memo_list_0 = "mem_list_p0.asp"
	Memo_list_1 = "mem_list_p1.asp"
Else
	Memo_list_0 = "mem_list_0.asp"
	Memo_list_1 = "mem_list_1.asp"
End if
%>

</HEAD>
<BODY BGCOLOR=#FFFFFF LEFTMARGIN=0 TOPMARGIN=0 MARGINWIDTH=0 MARGINHEIGHT=0>

<table border="0" width="384" cellpadding="0" cellspacing="0">
<tr>
	<td width="384" height="70"><img src="img/m_list_title.gif"></td>
</tr>
<tr>
	<td>
	<table width="384" height="25" border="0" cellpadding="0" cellspacing="0" ID="Table1">
	<tr align="center" bgcolor="#999999">
		<td width="125"<% if m_box=0 then %> bgcolor="#333333"<% end if %>><a href="<%=Memo_list_0%>?m_box=0"><b><font color="<% if m_box=0 then %>#ffffff<% else %>#ffffff<% end if %>">On-Line</b></font></a></td>
		<td width="125"<% if m_box=1 then %> bgcolor="#333333"<% end if %>><a href="<%=Memo_list_1%>?m_box=1"><b><font color="<% if m_box=1 then %>#ffffff<% else %>#ffffff<% end if %>">On/Off-Line</font></b></a></td>
		<td width="134"<% if m_box=2 then %> bgcolor="#333333"<% end if %>><a href="<%=Memo_list_0%>?m_box=2"><b><font color="<% if m_box=2 then %>#ffffff<% else %>#ffffff<% end if %>"></b></a></td>
		</tr>
	</table>
	</td>
</tr>
<tr>
	<td width="384" height="22">
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td height="1" bgcolor="#333333"></td>
	</tr>
	<tr>
		<td height="1" bgcolor="#ffffff"></td>
	</tr>
	

	<tr>
		<td height="18" bgcolor="#333333">
		<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr align="center">
			<td><font color="#FFFFFF"><b>ID(Unique ID)</b></font></td>
			<td width="80"><font color="#FFFFFF"><b>Status</b></font></td>
		</tr></table></td>
	</tr>
	<tr>
		<td height="1" bgcolor="#ffffff"></td>
	</tr>
	<tr>
		<td height="1" bgcolor="#333333"></td>
	</tr>
	</table>
	</td>
</tr>
	<%
	

	Dim pagecount, recordCount,point,po_write,po_comment
	dim ss
	 

	pagesize = Request("pagesize")
	if pagesize = "" then
		pagesize=10
	end if

	page = Request("page")
	if page = "" then page = 1
	
	block=10

	sw = Request("sw")

	if sw = "" then
		SQL = "select count(num) as recCount from member"
		Set Rs = db.Execute(SQL)

		recordCount = Rs(0)
		pagecount = int((recordCount-1)/pagesize) +1
		id_num = recordCount - (Page -1) * PageSize

		SQL = "SELECT TOP " & pagesize & " * FROM member "
		if int(page) > 1 then
		SQL = SQL & " WHERE num not in "
		SQL = SQL & "(SELECT TOP " & ((page - 1) * pagesize) & " num FROM member "
		SQL = SQL & " ORDER BY num DESC)"
		end if
		SQL = SQL & " order by num desc" 
	else
		ss = Request("ss")
				
		SQL = "select count(num) from member where " & ss & " like '%" & sw & "%'"
		Set rs = db.Execute(SQL)

		recordCount = Rs(0)
		pagecount = int((recordCount-1)/pagesize) +1
		id_num = recordCount - (Page -1) * PageSize
  

		SQL = "SELECT TOP " & pagesize & " * FROM member"
		SQL = SQL & " WHERE " & ss & " like '%" & sw & "%'"
		if int(page) > 1 then
		SQL = SQL & " and num not in "
		SQL = SQL & "(SELECT TOP " & ((page - 1) * pagesize) & " num FROM member"
		SQL = SQL & " where " & ss & " like '%" & sw & "%' ORDER BY num DESC)"
		end if
		SQL = SQL & " order by num desc" 
	end if

  Set Rs = db.Execute(SQL)

if rs.eof or rs.bof then
	
	
	else
	
	Do until Rs.EOF
	
	
	r_id = rs("id")
    r_name = rs("name")
    user_level = rs("user_level")
    
    if sw<>"" then
		if ss = "id" then
			r_id = replace(Ucase(r_id),sw,"<b><font color=#ff7b00>"&sw&"</font></b>")
			r_id = replace(Lcase(r_id),sw,"<b><font color=#ff7b00>"&sw&"</font></b>")
		elseif ss = "name" then
			r_name = replace(Ucase(r_name),sw,"<b><font color=#ff7b00>"&sw&"</font></b>")
			r_name = replace(Lcase(r_name),sw,"<b><font color=#ff7b00>"&sw&"</font></b>")
		end if
	end if 
	
	SQL1 = "SELECT * FROM view_login where user_id='"&rs("id")&"'"
	Set rs1 = eltConn.execute (SQL1)
	
	if rs1.eof or rs1.bof then
		on_off="<font color=#999999>offline</font>"
	else
		on_off="online"
	end if
	
	if (m_box=0 and on_off="online") or m_box=1 then 
	
%>
<tr>
	<td height="20">
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr align="center">
		<td><a href="m_write.asp?r_id=<%=rs("id")%>&r_name=<%=rs("name")%>"><%=r_name%>(<%=r_id%>)</a></td>
<!--		<td width="80"><%=user_level%></td> -->
		<td width="80"><%=on_off%></td>
	</tr></table></td>
</tr>
<%
	end if
	
	Rs.Movenext
	Loop
end if
%>
<tr>
	<td height="8">
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td height="1" bgcolor="#ffffff"></td>
	</tr>
	<tr>
		<td height="4" bgcolor="#333333"></td>
	</tr>
	<tr>
		<td height="1" bgcolor="#ffffff"></td>
	</tr>
	<tr>
		<td height="1" bgcolor="#333333"></td>
	</tr>
	</table></td>
</tr>
<% '############### ������ �׺���̼� �κ� ���� #################################### %>
<tr>
	<td align="center">	<% If Rs.BOF Then %>
	
	<%
		Else
		If Int(Page) <> 1 Then 
	%>
		<a href="<%=Memo_list_1%>?m_box=<%=m_box%>&page=<%=Page-1%><% if sw<>"" then %>&st=<%=st%>&sc=<%=sc%>&sn=<%=sn%>&sw=<%=sw%><% end if %>" onFocus="this.blur()"><font color="#000000" style="font-size:8pt;">[prv]</font></a>
	<%
		end if
		
		First_Page = Int((Page-1)/Block)*Block+1
		If First_Page <> 1 Then
	%>
			[<a href="<%=Memo_list_1%>?m_box=<%=m_box%>&page=1<% if sw<>"" then %>&st=<%=st%>&sc=<%=sc%>&sn=<%=sn%>&sw=<%=sw%><% end if %>" onFocus="this.blur()"><font color="#000000" style="font-size:8pt;">1</font></a>]&nbsp;..
	<%
		end if
		
		If PageCount - First_Page < Block Then
			End_Page = PageCount
		Else
			End_Page = First_Page + Block - 1
		End If

		For i = First_Page To End_Page
		If Int(Page) = i Then
	%>
			[<font color="#FF0000" style="font-size:8pt;"><b><%=i%></b></font>]
	<% Else %>
			[<a href="<%=Memo_list_1%>?m_box=<%=m_box%>&page=<%=i%><% if sw<>"" then %>&st=<%=st%>&sc=<%=sc%>&sn=<%=sn%>&sw=<%=sw%><% end if %>" onFocus="this.blur()"><font color="#000000" style="font-size:8pt;"><%=i%></font></a>]
	<%
		End If
		Next
		
		If End_Page <> PageCount Then
	%>
	&nbsp;..&nbsp;[<a href="<%=Memo_list_1%>?m_box=<%=m_box%>&page=<%=PageCount%><% if sw<>"" then %>&st=<%=st%>&sc=<%=sc%>&sn=<%=sn%>&sw=<%=sw%><% end if %>" onFocus="this.blur()"><font color="000000" style="font-size:8pt;"><%=PageCount%></font></a>]
	<%
		end if
		
		If Int(Page) <> PageCount Then
	%>
	&nbsp;<a href="<%=Memo_list_1%>?m_box=<%=m_box%>&page=<%=page+1%><% if sw<>"" then %>&st=<%=st%>&sc=<%=sc%>&sn=<%=sn%>&sw=<%=sw%><% end if %>" onFocus="this.blur()"><font color="#000000" style="font-size:8pt;">[next]</font></a>
	<%
		End If
		End If
	%></td>
</tr>

<% '############### ������ �׺���̼� �κ� �� #################################### %>
<Script Language="javascript">
<!--

function search_submit()
{
	if (document.inno_search.sw.value == "") {
		alert("Please enter a keyword.");
		document.inno_search.sw.focus();
		return;
	}
	
	document.inno_search.submit();

}

//-->
</Script>
<form name="inno_search" Method="post" ID="form1">
<tr>
	<td class="font1" align="right" style="word-break:break-all;padding:5px;">
	<select name="ss" align="absmiddle" ID="Select1">
	<option value="id"<% if ss="id" then %> selected<% end if %>>ID</option>
	<option value="name"<% if ss="name" then %> selected<% end if %>>Name</option>
	</select>
	<input type="text" size="15" name="sw" class="form_input" value="<% if sw<>"" then %><%=sw%><% end if %>" ID="Text1">
	<input type="hidden" name="page" value="<%=page%>" ID="Hidden1">
	<input type="hidden" name="pagesize" value="<%=pagesize%>" ID="Hidden2">
	
	<input type="button" class="but" value="Search" onClick="javascript:search_submit();" id=button1 name=button1><% if sw<>"" then %> <input type="button" class="but" value="All" onClick="location.href='<%=Memo_list_1%>?m_box=1'" id="Button2" name=all_view><% end if %></td>
</tr>
</form>
</table>
</body>
</html>