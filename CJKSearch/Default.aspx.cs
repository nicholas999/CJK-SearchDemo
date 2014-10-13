using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CJKSearch.Helper;

namespace CJKSearch
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Search_Click(object sender, EventArgs e)
        {
            string keyword = this.keyword.Text;
            string sql = "SELECT docid, title FROM pages";
            if (!string.IsNullOrEmpty(keyword))
            {
                if (keyword.Contains(" "))
                {
                    string connector = this.connector.SelectedItem.Value;
                    string[] words = keyword.Split(new char[] { ' ' });
                    sql += " WHERE pages MATCH('";
                    if (words.Length > 0)
                    {
                        for (int i = 0; i < words.Length; i++)
                        {
                            if (i > 0)
                            {
                                sql += " " + connector + " ";
                            }
                            sql += words[i];
                        }
                    }
                    sql += "')";
                }
                else
                {
                    sql += " WHERE pages MATCH '" + keyword + "'";
                }
            }

            DataSet ds = SQLiteHelper.GetData(sql);

            PagedDataSource ps = new PagedDataSource();
            ps.DataSource = ds.Tables[0].DefaultView;
            ps.AllowPaging = true;

            ps.PageSize = 100;
            this.Results.DataSource = ps;
            this.Results.DataBind();
        }
    }
}