﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ELT.DA
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PRDDB")]
	public partial class PRODDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public PRODDBDataContext() : 
				base(global::ELT.DA.Properties.Settings.Default.PRDDBConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PRODDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PRODDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PRODDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PRODDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<agent> agents
		{
			get
			{
				return this.GetTable<agent>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.agent")]
	public partial class agent
	{
		
		private decimal _elt_account_number;
		
		private string _dba_name;
		
		private System.Nullable<char> _class_code;
		
		private System.Nullable<System.DateTime> _date_opened;
		
		private string _business_legal_name;
		
		private string _business_fed_taxid;
		
		private string _business_st_taxid;
		
		private string _usppi;
		
		private string _business_address;
		
		private string _business_address_add;
		
		private string _business_city;
		
		private string _business_province;
		
		private string _business_state;
		
		private string _business_zip;
		
		private string _business_country;
		
		private string _country_code;
		
		private string _business_phone;
		
		private string _business_fax;
		
		private string _business_url;
		
		private string _owner_ssn;
		
		private string _owner_lname;
		
		private string _owner_fname;
		
		private string _owner_mname;
		
		private string _owner_mail_address;
		
		private string _owner_mail_city;
		
		private string _owner_mail_state;
		
		private string _owner_mail_zip;
		
		private string _owner_mail_country;
		
		private string _owner_phone;
		
		private string _owner_email;
		
		private System.Nullable<char> _account_statue;
		
		private string _Agent_IATA_Code;
		
		private string _OTI_Code;
		
		private string _board_name;
		
		private string _iv_statement;
		
		private string _faa_approval_no;
		
		private System.Nullable<decimal> _maxuser;
		
		private string _is_dome;
		
		private string _is_intl;
		
		private string _faa_approval_date;
		
		private string _is_cartage;
		
		private string _is_warehouse;
		
		private string _is_accounting;
		
		private string _is_exporter;
		
		private string _is_aes;
		
		public agent()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_elt_account_number", DbType="Decimal(8,0) NOT NULL")]
		public decimal elt_account_number
		{
			get
			{
				return this._elt_account_number;
			}
			set
			{
				if ((this._elt_account_number != value))
				{
					this._elt_account_number = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dba_name", DbType="NVarChar(128) NOT NULL", CanBeNull=false)]
		public string dba_name
		{
			get
			{
				return this._dba_name;
			}
			set
			{
				if ((this._dba_name != value))
				{
					this._dba_name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_class_code", DbType="NChar(1)")]
		public System.Nullable<char> class_code
		{
			get
			{
				return this._class_code;
			}
			set
			{
				if ((this._class_code != value))
				{
					this._class_code = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_date_opened", DbType="DateTime")]
		public System.Nullable<System.DateTime> date_opened
		{
			get
			{
				return this._date_opened;
			}
			set
			{
				if ((this._date_opened != value))
				{
					this._date_opened = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_legal_name", DbType="NVarChar(128)")]
		public string business_legal_name
		{
			get
			{
				return this._business_legal_name;
			}
			set
			{
				if ((this._business_legal_name != value))
				{
					this._business_legal_name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_fed_taxid", DbType="NVarChar(16)")]
		public string business_fed_taxid
		{
			get
			{
				return this._business_fed_taxid;
			}
			set
			{
				if ((this._business_fed_taxid != value))
				{
					this._business_fed_taxid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_st_taxid", DbType="NVarChar(16)")]
		public string business_st_taxid
		{
			get
			{
				return this._business_st_taxid;
			}
			set
			{
				if ((this._business_st_taxid != value))
				{
					this._business_st_taxid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_usppi", DbType="NVarChar(64)")]
		public string usppi
		{
			get
			{
				return this._usppi;
			}
			set
			{
				if ((this._usppi != value))
				{
					this._usppi = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_address", DbType="NVarChar(128)")]
		public string business_address
		{
			get
			{
				return this._business_address;
			}
			set
			{
				if ((this._business_address != value))
				{
					this._business_address = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_address_add", DbType="NVarChar(128)")]
		public string business_address_add
		{
			get
			{
				return this._business_address_add;
			}
			set
			{
				if ((this._business_address_add != value))
				{
					this._business_address_add = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_city", DbType="NVarChar(64)")]
		public string business_city
		{
			get
			{
				return this._business_city;
			}
			set
			{
				if ((this._business_city != value))
				{
					this._business_city = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_province", DbType="NVarChar(128)")]
		public string business_province
		{
			get
			{
				return this._business_province;
			}
			set
			{
				if ((this._business_province != value))
				{
					this._business_province = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_state", DbType="NVarChar(128)")]
		public string business_state
		{
			get
			{
				return this._business_state;
			}
			set
			{
				if ((this._business_state != value))
				{
					this._business_state = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_zip", DbType="NVarChar(32)")]
		public string business_zip
		{
			get
			{
				return this._business_zip;
			}
			set
			{
				if ((this._business_zip != value))
				{
					this._business_zip = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_country", DbType="NVarChar(32)")]
		public string business_country
		{
			get
			{
				return this._business_country;
			}
			set
			{
				if ((this._business_country != value))
				{
					this._business_country = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_country_code", DbType="NVarChar(2)")]
		public string country_code
		{
			get
			{
				return this._country_code;
			}
			set
			{
				if ((this._country_code != value))
				{
					this._country_code = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_phone", DbType="NVarChar(32)")]
		public string business_phone
		{
			get
			{
				return this._business_phone;
			}
			set
			{
				if ((this._business_phone != value))
				{
					this._business_phone = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_fax", DbType="NVarChar(32)")]
		public string business_fax
		{
			get
			{
				return this._business_fax;
			}
			set
			{
				if ((this._business_fax != value))
				{
					this._business_fax = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_business_url", DbType="NVarChar(64)")]
		public string business_url
		{
			get
			{
				return this._business_url;
			}
			set
			{
				if ((this._business_url != value))
				{
					this._business_url = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_ssn", DbType="NVarChar(9)")]
		public string owner_ssn
		{
			get
			{
				return this._owner_ssn;
			}
			set
			{
				if ((this._owner_ssn != value))
				{
					this._owner_ssn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_lname", DbType="NVarChar(64)")]
		public string owner_lname
		{
			get
			{
				return this._owner_lname;
			}
			set
			{
				if ((this._owner_lname != value))
				{
					this._owner_lname = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_fname", DbType="NVarChar(64)")]
		public string owner_fname
		{
			get
			{
				return this._owner_fname;
			}
			set
			{
				if ((this._owner_fname != value))
				{
					this._owner_fname = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_mname", DbType="NVarChar(32)")]
		public string owner_mname
		{
			get
			{
				return this._owner_mname;
			}
			set
			{
				if ((this._owner_mname != value))
				{
					this._owner_mname = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_mail_address", DbType="NVarChar(128)")]
		public string owner_mail_address
		{
			get
			{
				return this._owner_mail_address;
			}
			set
			{
				if ((this._owner_mail_address != value))
				{
					this._owner_mail_address = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_mail_city", DbType="NVarChar(64)")]
		public string owner_mail_city
		{
			get
			{
				return this._owner_mail_city;
			}
			set
			{
				if ((this._owner_mail_city != value))
				{
					this._owner_mail_city = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_mail_state", DbType="NVarChar(128)")]
		public string owner_mail_state
		{
			get
			{
				return this._owner_mail_state;
			}
			set
			{
				if ((this._owner_mail_state != value))
				{
					this._owner_mail_state = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_mail_zip", DbType="NVarChar(32)")]
		public string owner_mail_zip
		{
			get
			{
				return this._owner_mail_zip;
			}
			set
			{
				if ((this._owner_mail_zip != value))
				{
					this._owner_mail_zip = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_mail_country", DbType="NVarChar(32)")]
		public string owner_mail_country
		{
			get
			{
				return this._owner_mail_country;
			}
			set
			{
				if ((this._owner_mail_country != value))
				{
					this._owner_mail_country = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_phone", DbType="NVarChar(32)")]
		public string owner_phone
		{
			get
			{
				return this._owner_phone;
			}
			set
			{
				if ((this._owner_phone != value))
				{
					this._owner_phone = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_owner_email", DbType="NVarChar(128)")]
		public string owner_email
		{
			get
			{
				return this._owner_email;
			}
			set
			{
				if ((this._owner_email != value))
				{
					this._owner_email = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_account_statue", DbType="NChar(1)")]
		public System.Nullable<char> account_statue
		{
			get
			{
				return this._account_statue;
			}
			set
			{
				if ((this._account_statue != value))
				{
					this._account_statue = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Agent_IATA_Code", DbType="NVarChar(32)")]
		public string Agent_IATA_Code
		{
			get
			{
				return this._Agent_IATA_Code;
			}
			set
			{
				if ((this._Agent_IATA_Code != value))
				{
					this._Agent_IATA_Code = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OTI_Code", DbType="NVarChar(50)")]
		public string OTI_Code
		{
			get
			{
				return this._OTI_Code;
			}
			set
			{
				if ((this._OTI_Code != value))
				{
					this._OTI_Code = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_board_name", DbType="NVarChar(32)")]
		public string board_name
		{
			get
			{
				return this._board_name;
			}
			set
			{
				if ((this._board_name != value))
				{
					this._board_name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_iv_statement", DbType="NVarChar(1024)")]
		public string iv_statement
		{
			get
			{
				return this._iv_statement;
			}
			set
			{
				if ((this._iv_statement != value))
				{
					this._iv_statement = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_faa_approval_no", DbType="NVarChar(32)")]
		public string faa_approval_no
		{
			get
			{
				return this._faa_approval_no;
			}
			set
			{
				if ((this._faa_approval_no != value))
				{
					this._faa_approval_no = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_maxuser", DbType="Decimal(18,0)")]
		public System.Nullable<decimal> maxuser
		{
			get
			{
				return this._maxuser;
			}
			set
			{
				if ((this._maxuser != value))
				{
					this._maxuser = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_dome", DbType="NVarChar(1) NOT NULL", CanBeNull=false)]
		public string is_dome
		{
			get
			{
				return this._is_dome;
			}
			set
			{
				if ((this._is_dome != value))
				{
					this._is_dome = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_intl", DbType="NVarChar(1) NOT NULL", CanBeNull=false)]
		public string is_intl
		{
			get
			{
				return this._is_intl;
			}
			set
			{
				if ((this._is_intl != value))
				{
					this._is_intl = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_faa_approval_date", DbType="NVarChar(64)")]
		public string faa_approval_date
		{
			get
			{
				return this._faa_approval_date;
			}
			set
			{
				if ((this._faa_approval_date != value))
				{
					this._faa_approval_date = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_cartage", DbType="NVarChar(1)")]
		public string is_cartage
		{
			get
			{
				return this._is_cartage;
			}
			set
			{
				if ((this._is_cartage != value))
				{
					this._is_cartage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_warehouse", DbType="NVarChar(1)")]
		public string is_warehouse
		{
			get
			{
				return this._is_warehouse;
			}
			set
			{
				if ((this._is_warehouse != value))
				{
					this._is_warehouse = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_accounting", DbType="NVarChar(1)")]
		public string is_accounting
		{
			get
			{
				return this._is_accounting;
			}
			set
			{
				if ((this._is_accounting != value))
				{
					this._is_accounting = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_exporter", DbType="NVarChar(1)")]
		public string is_exporter
		{
			get
			{
				return this._is_exporter;
			}
			set
			{
				if ((this._is_exporter != value))
				{
					this._is_exporter = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_is_aes", DbType="NVarChar(1)")]
		public string is_aes
		{
			get
			{
				return this._is_aes;
			}
			set
			{
				if ((this._is_aes != value))
				{
					this._is_aes = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
