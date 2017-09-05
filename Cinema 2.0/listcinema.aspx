<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="listcinema.aspx.cs" Inherits="Cinema_2._0.listcinema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - Danh Sách Các Rạp Phim' property='og:title'/>
    <meta content='Đặt vé và tra cứu thông tin khuyến mãi, vé xem phim, phim mới, rạp phim tại khu vực của bạn.' property='og:description'/>
    <meta content='http://<%=Request.Url.Host %>/img/banner.jpg' property='og:image'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <ul class="collapsible" data-collapsible="accordion">
        <%foreach (var p in listCinema)
            { %>
        <li>
            <div class="collapsible-header" style="font-size: 18px !important"><i class="material-icons">stars</i><%=p.name %></div>
            <div class="collapsible-body white" style="padding-top: 0px">
                <div class="collection">
                    <%foreach (var c in p.listCinema)
                        { %>
                    <a href="detailcinema.aspx?id=<%=c.id %>" class="collection-item green-text" style="font-size: 17px !important"><i class="material-icons">location_on</i>
                        <%=c.name %>
                        <p class="blue-grey-text" style="font-size:14px !important">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=c.address %></p>
                    </a>
                    <%} %>
                </div>
            </div>
        </li>
        <%} %>
    </ul>

</asp:Content>
