<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CacheManager.Test.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <div>
        <h1>
            <asp:Label ID="txtCache" runat="server"></asp:Label>
        </h1>
        <p>
            <asp:Button ID="btnSet" runat="server" Text="设置缓存值" OnClick="btnSet_Click" />
            <asp:Button ID="btnGet" runat="server" Text="获取缓存值" OnClick="btnGet_Click" />
            <asp:Button ID="btnRemove" runat="server" Text="删除缓存值" OnClick="btnRemove_Click" />
        </p>
    </div>
    </form>
</body>
</html>
