<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="listevent.aspx.cs" Inherits="Cinema_2._0.listevent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - Danh Sách Khuyến Mãi Của Các Cụm Rạp' property='og:title'/>
    <meta content='Đặt vé và tra cứu thông tin khuyến mãi, vé xem phim, phim mới, rạp phim tại khu vực của bạn.' property='og:description'/>
    <meta content='http://<%=Request.Url.Host %>/img/banner.jpg' property='og:image'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <br />
    <ul id="tabs-swipe-demo" class="tabs card">
        <%foreach (var p in listProducer)
            { %>
        <li class="tab <%= p.id == listProducer.First().id ? "selected" : "" %>">
            <a href='#<%=p.id %>' style="font-size: 15px !important; color: #3b3e4a;"><%=p.name %></a></li>
        <% }%>
    </ul>
    <%foreach (var p in listProducer)
        { %>
    <div id="<%=p.id %>" class="row">

        <%foreach (var e in p.listEvent)
            {
        %>

        <div class="col s12 m6 l6" style="position: relative;">
            <a href="detailevent.aspx?id=<%=e.id%>">
                <div class="card large waves-effect hoverable" style="height: 350px !important; width: 100% !important">
                    <div class="card-image waves-effect waves-block waves-effect" style="max-height: 70% !important; height: 70% !important">
                        <img src="<%=e.linkPoster%>" onerror="this.src='/img/image-not-found.png'" alt='<%=title + e.name %>' />
                    </div>
                    <div class="card-content" style="max-height: 16% !important; height: 30% !important">
                        <span class="card-title grey-text text-darken-4" ><%=e.name%></span>
                        <p class="card-content green-text" style="position: absolute; bottom: -15px !important; right: -15px !important">
                            <%="Thời gian : " + e.time%>
                        </p>
                    </div>
                </div>
            </a>
        </div>

        <%} %>
    </div>

    <%} %>
</asp:Content>
