<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userRequest.aspx.cs" Inherits="Cinema_2._0.update.userRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Request</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Button ID="btnClear" runat="server" Text="Clear All" Font-Bold="True" ForeColor="Red" OnClick="btnClear_Click" />
    </div>

        
        <table border="1">
            <tr>
                <th>STT</th>
                <th>ID</th>
                <th>IP</th>
                <th>Time</th>
                <th>Note</th>
            </tr>

                <%
                    for (int i = 0; i < listUser.Count; i++)
                    {
                     %>

                <tr>
                <td><%=i + 1 %></td>
                <td><%=listUser[i].id %></td>
                <td><%=listUser[i].ip %></td>
                <td><%=listUser[i].time %></td>
                <td><%=listUser[i].note %></td>
                </tr>

                <% } %>
        </table>

    </form>
</body>
</html>
