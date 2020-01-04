using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace igFunctions
{
	/// <summary>
	/// Error에 대한 요약 설명입니다.
	/// </summary>
	/// 
	

	public partial class Error : System.Web.UI.Page
	{
		

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblError.Text = Request.Params["ErrorMsg"];
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
