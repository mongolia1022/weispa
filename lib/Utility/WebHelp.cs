using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace com.ccfw.Utility
{
    public class WebHelp
    {
        public static void BindCheckBoxList(List<CustomListItem> listItem, CheckBoxList cbList)
        {
            if (cbList != null)
            {
                cbList.Items.Clear();
                if ((listItem != null) && (listItem.Count != 0))
                {
                    foreach (CustomListItem item in listItem)
                    {
                        cbList.Items.Add(new ListItem(item.Text, item.Value));
                    }
                }
            }
        }

        public static void BindDropDownList(List<CustomListItem> listItem, DropDownList ddl)
        {
            BindDropDownList(listItem, ddl, 0);
        }

        public static void BindDropDownList(List<CustomListItem> listItem, DropDownList ddl, int iSelectedIndex)
        {
            BindDropDownList(listItem, ddl, "");
            if (ddl.Items.Count > 0)
            {
                try
                {
                    ddl.SelectedIndex = iSelectedIndex;
                }
                catch
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        public static void BindDropDownList(List<CustomListItem> listItem, DropDownList ddl, string sPreText)
        {
            ddl.Items.Clear();
            if (!string.IsNullOrEmpty(sPreText))
            {
                ddl.Items.Add(new ListItem(sPreText, "0"));
            }
            if (((ddl != null) && (listItem != null)) && (listItem.Count != 0))
            {
                foreach (CustomListItem item in listItem)
                {
                    ddl.Items.Add(new ListItem(item.Text, item.Value));
                }
                if (ddl.Items.Count > 0)
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        public static void BindDropDownList(DataTable dt, DropDownList ddl, string sValueField, string sTextField)
        {
            BindDropDownList(dt, ddl, sValueField, sTextField, 0);
        }

        public static void BindDropDownList(DataView dv, DropDownList ddl, string sValueField, string sTextField)
        {
            BindDropDownList(dv, ddl, sValueField, sTextField, 0);
        }

        public static void BindDropDownList(DataTable dt, DropDownList ddl, string sValueField, string sTextField, int iSelectedIndex)
        {
            BindDropDownList(dt.DefaultView, ddl, sValueField, sTextField, iSelectedIndex);
        }

        public static void BindDropDownList(DataTable dt, DropDownList ddl, string sValueField, string sTextField, string sPreText)
        {
            BindDropDownList(dt.DefaultView, ddl, sValueField, sTextField, sPreText);
        }

        public static void BindDropDownList(DataView dv, DropDownList ddl, string sValueField, string sTextField, int iSelectedIndex)
        {
            BindDropDownList(dv, ddl, sValueField, sTextField, "");
            if (ddl.Items.Count > 0)
            {
                try
                {
                    ddl.SelectedIndex = iSelectedIndex;
                }
                catch
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        public static void BindDropDownList(DataView dv, DropDownList ddl, string sValueField, string sTextField, string sPreText)
        {
            ddl.Items.Clear();
            if ((ddl != null) && (dv != null))
            {
                if (!CString.IsEmpty(sPreText))
                {
                    ddl.Items.Add(new ListItem(sPreText, "0"));
                }
                foreach (DataRowView view in dv)
                {
                    ddl.Items.Add(new ListItem(view[sTextField].ToString(), view[sValueField].ToString()));
                }
                if (ddl.Items.Count > 0)
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        public static void BindListBox(List<CustomListItem> list, ListBox listBox)
        {
            listBox.Items.Clear();
            if (((list != null) && (listBox != null)) && (list.Count != 0))
            {
                foreach (CustomListItem item in list)
                {
                    listBox.Items.Add(new ListItem(item.Text, item.Value));
                }
            }
        }

        private static List<CustomTreeNode> FindChild(List<CustomTreeNode> listNode, string strParentID)
        {
            return FindChild(listNode, strParentID, false);
        }

        private static List<CustomTreeNode> FindChild(List<CustomTreeNode> listNode, string strParentID, bool bGetSelf)
        {
            List<CustomTreeNode> list = new List<CustomTreeNode>();
            if (listNode != null)
            {
                if (listNode.Count == 0)
                {
                    return list;
                }
                if (!bGetSelf)
                {
                    foreach (CustomTreeNode node in listNode)
                    {
                        if (node.ParentID == strParentID)
                        {
                            list.Add(node);
                        }
                    }
                    return list;
                }
                foreach (CustomTreeNode node in listNode)
                {
                    if (node.Value == strParentID)
                    {
                        list.Add(node);
                    }
                }
            }
            return list;
        }

        private static TreeNode FindNodeByNodeValue(TreeNode parentNode, string nodeValue)
        {
            TreeNode node = null;
            foreach (TreeNode node2 in parentNode.ChildNodes)
            {
                if (node2.Value.Equals(nodeValue))
                {
                    return node2;
                }
                node = FindNodeByNodeValue(node2, nodeValue);
                if (node != null)
                {
                    return node;
                }
            }
            return node;
        }

        public static TreeNode FindNodeByNodeValue(TreeView tv, string nodeValue)
        {
            TreeNode node = null;
            foreach (TreeNode node2 in tv.Nodes)
            {
                if (node2.Value.Equals(nodeValue))
                {
                    return node2;
                }
                node = FindNodeByNodeValue(node2, nodeValue);
                if (node != null)
                {
                    return node;
                }
            }
            return node;
        }

        public static string GetIP()
        {
            string str;
            string str2 = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if ((str2 != null) && (str2 != ""))
            {
                str2 = str2.Replace("'", "");
                if (str2.IndexOf(",") >= 0)
                {
                    str = str2.Split(new char[] { ',' })[0];
                }
                else
                {
                    str = str2;
                }
            }
            else
            {
                str = "";
            }
            if (str == "")
            {
                str = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                str = str.Replace("'", "");
            }
            return str;
        }

        public static string GetPageNumbers(int curPage, int RecordCount, int PageSize, string url, int extendPage, bool isRewrite)
        {
            int num = 1;
            int num2 = 1;
            if (PageSize < 1)
            {
                PageSize = 1;
            }
            int num3 = (RecordCount / PageSize) + 1;
            if ((RecordCount % PageSize) == 0)
            {
                num3--;
            }
            if (num3 < 1)
            {
                num3 = 1;
            }
            if (curPage < 1)
            {
                curPage = 1;
            }
            if (curPage > num3)
            {
                curPage = num3;
            }
            string format = "<a href=\"{0}\" class=\"first\">首页</a>&nbsp;<a href=\"{1}\" class=\"prev\">上一页</a>&nbsp;";
            string str2 = "<a href=\"{0}\" class=\"next\">下一页</a>&nbsp;<a href=\"{1}\" class=\"last\">尾页</a>";
            if (isRewrite)
            {
                format = string.Format(format, string.Format(url, 1), string.Format(url, curPage - 1));
                str2 = string.Format(str2, string.Format(url, curPage + 1), string.Format(url, num3));
                if (curPage == 1)
                {
                    format = "";
                }
                if (curPage == num3)
                {
                    str2 = "";
                }
            }
            else
            {
                if (url.IndexOf("?") > 0)
                {
                    url = url + "&";
                }
                else
                {
                    url = url + "?";
                }
                format = string.Format(format, url + "pn=1", url + string.Format("pn={0}", curPage - 1));
                str2 = string.Format(str2, url + string.Format("pn={0}", curPage + 1), url + string.Format("pn={0}", num3));
                if (curPage == 1)
                {
                    format = "";
                }
                if (curPage == num3)
                {
                    str2 = "";
                }
            }
            if (num3 < 1)
            {
                num3 = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            if (num3 > extendPage)
            {
                if ((curPage - (extendPage / 2)) > 0)
                {
                    if ((curPage + (extendPage / 2)) < num3)
                    {
                        num = curPage - (extendPage / 2);
                        num2 = (num + extendPage) - 1;
                    }
                    else
                    {
                        num2 = num3;
                        num = (num2 - extendPage) + 1;
                    }
                }
                else
                {
                    num2 = extendPage;
                }
            }
            else
            {
                num = 1;
                num2 = num3;
            }
            StringBuilder builder = new StringBuilder("");
            builder.Append(format);
            for (int i = num; i <= num2; i++)
            {
                if (i == curPage)
                {
                    builder.Append("<strong style=\"color:Red\">");
                    builder.Append(i);
                    builder.Append("</strong>&nbsp;");
                }
                else
                {
                    builder.Append("<a href=\"");
                    if (isRewrite)
                    {
                        builder.Append(string.Format(url, i));
                    }
                    else
                    {
                        builder.Append(url);
                        builder.Append("pn=");
                        builder.Append(i);
                    }
                    builder.Append("\">");
                    builder.Append(i);
                    builder.Append("</a>");
                    builder.Append("&nbsp;");
                }
            }
            builder.Append(str2);
            return builder.ToString();
        }

        public static ArrayList GetRequestPost(HttpContext context)
        {
            int index = 0;
            ArrayList list = new ArrayList();
            string[] allKeys = context.Request.Form.AllKeys;
            for (index = 0; index < allKeys.Length; index++)
            {
                list.Add(allKeys[index] + "=" + context.Request.Form[allKeys[index]]);
            }
            return list;
        }

        public uint GetServerIpAddresses()
        {
            return BitConverter.ToUInt32(IPAddress.Parse(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString()).GetAddressBytes(), 0);
        }

        public static int GetTimeDelay(DateTime dtStart, DateTime dtEnd)
        {
            int num = 0;
            num += (((dtEnd.Hour - dtStart.Hour) * 60) * 60) * 0x3e8;
            num += ((dtEnd.Minute - dtStart.Minute) * 60) * 0x3e8;
            num += (dtEnd.Second - dtStart.Second) * 0x3e8;
            return (num + (dtEnd.Millisecond - dtStart.Millisecond));
        }

        public static string GetTreeViewCheckBoxValue(TreeNodeCollection nodes)
        {
            string str = "";
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    str = str + node.Value + ",";
                }
                if (node.ChildNodes.Count > 0)
                {
                    str = str + GetTreeViewCheckBoxValue(node.ChildNodes);
                }
            }
            return str;
        }

        public static string GetTreeViewCheckBoxValue(TreeView trv)
        {
            return GetTreeViewCheckBoxValue(trv.Nodes);
        }

        private static void LoadChildNode(List<CustomTreeNode> listTreeNode, TreeNode ParentNode)
        {
            List<CustomTreeNode> list = FindChild(listTreeNode, ParentNode.Value);
            foreach (CustomTreeNode node in list)
            {
                TreeNode child = new TreeNode
                {
                    Value = node.Value,
                    Text = node.Text
                };
                ParentNode.ChildNodes.Add(child);
                LoadChildNode(listTreeNode, child);
            }
        }

        private static void LoadChildNode(DataTable dt, TreeNode ParentNode, string sValueField, string sTextField)
        {
            foreach (DataRowView view in dt.DefaultView)
            {
                if (view["ParentID"].ToString() == ParentNode.Value)
                {
                    TreeNode child = new TreeNode
                    {
                        Value = view[sValueField].ToString(),
                        Text = view[sTextField].ToString()
                    };
                    ParentNode.ChildNodes.Add(child);
                    LoadChildNode(dt, child, sValueField, sTextField);
                }
            }
        }

        public static void LoadNode(TreeNode node, List<CustomTreeNode> listTreeNode, string strParentID)
        {
            node.ChildNodes.Clear();
            List<CustomTreeNode> list = FindChild(listTreeNode, strParentID);
            foreach (CustomTreeNode node2 in list)
            {
                TreeNode child = new TreeNode
                {
                    Value = node2.Value,
                    Text = node2.Text
                };
                node.ChildNodes.Add(child);
                LoadChildNode(listTreeNode, child);
            }
        }

        public static void LoadTree(TreeView trv, List<CustomTreeNode> listTreeNode, string strParentID)
        {
            LoadTree(trv, listTreeNode, strParentID, false);
        }

        public static void LoadTree(TreeView trv, List<CustomTreeNode> listTreeNode, string strParentID, bool bGetSelf)
        {
            trv.Nodes.Clear();
            List<CustomTreeNode> list = FindChild(listTreeNode, strParentID, bGetSelf);
            foreach (CustomTreeNode node in list)
            {
                TreeNode child = new TreeNode
                {
                    Value = node.Value,
                    Text = node.Text
                };
                trv.Nodes.Add(child);
                LoadChildNode(listTreeNode, child);
            }
        }

        public static void LoadTree(TreeView trv, DataTable dt, string sValueField, string sTextField, int iParentID)
        {
            trv.Nodes.Clear();
            foreach (DataRowView view in dt.DefaultView)
            {
                if (view["ParentID"].ToString() == iParentID.ToString())
                {
                    TreeNode child = new TreeNode
                    {
                        Value = view[sValueField].ToString(),
                        Text = view[sTextField].ToString()
                    };
                    trv.Nodes.Add(child);
                    LoadChildNode(dt, child, sValueField, sTextField);
                }
            }
        }

        public static string ToMd5(string clearString)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(clearString);
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes));
        }
    }

    public class CustomListItem
    {
        private string _Text;
        private string _Value;

        public CustomListItem()
        {
            this._Text = "";
            this._Value = "";
        }

        public CustomListItem(string sText, string sValue)
        {
            this._Text = "";
            this._Value = "";
            this._Text = sText;
            this._Value = sValue;
        }

        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
            }
        }

        public string Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }
    }

    public class CustomTreeNode
    {
        private string _ParentID = "";
        private string _Text = "";
        private string _Value = "";

        public string ParentID
        {
            get
            {
                return this._ParentID;
            }
            set
            {
                this._ParentID = value;
            }
        }

        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
            }
        }

        public string Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }
    }
}
