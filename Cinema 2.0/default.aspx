<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Cinema_2._0._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM' property='og:title'/>
    <meta content='Đặt vé và tra cứu thông tin khuyến mãi, vé xem phim, phim mới, rạp phim tại khu vực của bạn.' property='og:description'/>
    <meta content='http://<%=Request.Url.Host %>/img/banner.jpg' property='og:image'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <br />
    <ul id="tabs-swipe-demo" class="tabs card">
        <li class="tab col s3 disabled">
            <a href='#' style="font-size:17px !important; color:#3b3e4a;">Chọn ngày xem : </a></li>
        <%foreach (var d in listDate)
            {
                dmy = d.Split('-');%>
        <li class="tab col s3">
            <a onclick='<%= date == d ? "" :  "window.location.href = \"" + Request.Url.AbsolutePath + "?date=" + d + "\";"%>' style="font-size: 17px !important; color: #3b3e4a;" class='<%=d == date ? "active" : "" %>' <%=d==date? "href='#" + d + "'": ""%>><%=dmy[2] + "/" + dmy[1] + "/" + dmy[0]  %></a></li>
        <%} %>
    </ul>
    <div class="row">

        <%for (int i = 0; i < listFilm.Count; i++)
            {
        %>

        <div class="col s12 m6 l4" style="position: relative;">
            <div class="card large waves-effect hoverable" style="width:100% !important">
                <div class="card-image waves-effect waves-block waves-effect" style="max-height: 70% !important">
                    <img class="activator" src="<%=listFilm[i].linkPoster%>" onerror="this.src='/img/image-not-found.png'" alt='<%=title + listFilm[i].name %>' />
                </div>
                <div class="card-content" style="max-height: 18% !important">
                    <span><i class="card-title material-icons activator right green-text">more_vert</i></span>
                    <a class="card-title grey-text text-darken-4" href="detailfilm.aspx?id=<%=listFilm[i].id%>"><%=listFilm[i].name%></a>
                    <p class="card-content" style="position: absolute; bottom: 15px !important; left: 15px !important">
                        <%="Thời lượng : " + listFilm[i].length%>
                    </p>
                </div>
                <div class="card-reveal">
                    <span><i class="card-title material-icons activator right green-text">close</i></span>
                    <span class="card-title grey-text text-darken-4"><%=listFilm[i].name%></span>
                    <p style="font-size: 16px">
                        <%=
                            "Khởi chiếu : " + (listFilm[i].premiere.HasValue ? listFilm[i].premiere.Value.ToString("dd/MM/yyyy") : "") +
                                "<br/>Thời lượng : " + listFilm[i].length +
                                "<br/>Thể loại   : " + listFilm[i].genre +
                                "<br/>Diễn viên  : " + listFilm[i].actor +
                                "<br/>Đạo diễn   : " + listFilm[i].director +
                                "<br/>Quốc gia   : " + listFilm[i].country +
                                "<br/>Điểm IMDB  : " + listFilm[i].imdb +
                                "<br/>Phân loại khán giả  : " + listFilm[i].classification
                        %>
                    </p>
                    <div class="card-action green waves-effect">
                        <a class="white-text" href="filmschedule.aspx?id=<%=listFilm[i].id+ "&date=" + date %>">Đặt vé</a>
                        <a class="white-text" href="detailfilm.aspx?id=<%=listFilm[i].id%>">Xem chi tiết</a>
                    </div>
                </div>
                <a href="filmschedule.aspx?id=<%=listFilm[i].id + "&date=" + date %>" class="waves-effect btn right green" style="position: absolute; bottom: 15px !important; right: 15px !important; border-radius: 18px">Đặt vé</a>
            </div>
        </div>

        <%} %>
    </div>


</asp:Content>
