<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CJKSearch.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="keyword" runat="server"></asp:TextBox>
        <asp:DropDownList ID="connector" runat="server">
            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
            <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="Search" runat="server" Text="Search within book" OnClick="Search_Click" />
        <asp:DataList ID="Results" runat="server">
            <HeaderTemplate>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#E2EEF5">
                    <tr>
                        <th>docid</th>
                        <th>title</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem,"docid") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem,"title") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:DataList>
    </form>
</body>
</html>
