<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="listfilm.aspx.cs" Inherits="Cinema_2._0.listfilm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - Danh Sách Phim Đang Chiếu Rạp' property='og:title'/>
    <meta content='Đặt vé và tra cứu thông tin khuyến mãi, vé xem phim, phim mới, rạp phim tại khu vực của bạn.' property='og:description'/>
    <meta content='http://<%=Request.Url.Host %>/img/banner.jpg' property='og:image'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <br />
    <ul id="tabs-swipe-demo" class="tabs card">
        <li class="tab col s6 l6 m6 selected" style="width:49% !important">
            <a href='#IsShow' style="font-size: 17px !important; color: #3b3e4a;">Đang chiếu</a></li>
        <li class="tab col s6 l6 m6" style="width:49% !important">
            <a href='#DontShow' style="font-size: 17px !important; color: #3b3e4a;">Sắp chiếu</a></li>
    </ul>
    <div id="IsShow" class="row">

        <%for (int i = 0; i < listFilmShow.Count; i++)
            {
        %>

        <div class="col s12 m6 l6" style="position: relative;">
            <div class="card large waves-effect hoverable" style="height: 350px !important; width: 100% !important">
                <div class="card-image waves-effect waves-block waves-effect" style="max-height: 70% !important; height: 70% !important">
                    <img class="activator" src="<%=listFilmShow[i].linkBanner%>" onerror="this.src='/img/image-not-found.png'" alt='<%=title + listFilmShow[i].name %>' />
                </div>
                <div class="card-content" style="max-height: 16% !important; height: 30% !important">
                    <span><i class="card-title material-icons activator right green-text">more_vert</i></span>
                    <a class="card-title grey-text text-darken-4" href="detailfilm.aspx?id=<%=listFilmShow[i].id%>"><%=listFilmShow[i].name%></a>
                    <p class="card-content" style="position: absolute; bottom: 0px !important; left: 15px !important">
                        <%="Khởi chiếu : " + (listFilmShow[i].premiere.HasValue ? listFilmShow[i].premiere.Value.ToString("dd/MM/yyyy") : "")%>
                    </p>
                    <a href="filmschedule.aspx?id=<%=listFilmShow[i].id%>" class="waves-effect btn right green" style="position: absolute; bottom: 15px !important; right: 15px !important; border-radius: 18px">Đặt vé</a>
                </div>
                <div class="card-reveal">
                    <span><i class="card-title material-icons activator right green-text">close</i></span>
                    <span class="card-title grey-text text-darken-4"><%=listFilmShow[i].name%></span>
                    <p style="font-size: 16px">
                        <%=
                            "Khởi chiếu : " + (listFilmShow[i].premiere.HasValue ? listFilmShow[i].premiere.Value.ToString("dd/MM/yyyy") : "") +
                                "<br/>Thời lượng : " + listFilmShow[i].length +
                                "<br/>Thể loại   : " + listFilmShow[i].genre +
                                "<br/>Diễn viên  : " + listFilmShow[i].actor +
                                "<br/>Đạo diễn   : " + listFilmShow[i].director +
                                "<br/>Quốc gia   : " + listFilmShow[i].country +
                                "<br/>Điểm IMDB  : " + listFilmShow[i].imdb +
                                "<br/>Phân loại khán giả  : " + listFilmShow[i].classification
                        %>
                    </p>
                    <div class="card-action green waves-effect">
                        <a class="white-text" href="filmschedule.aspx?id=<%=listFilmShow[i].id%>">Đặt vé</a>
                        <a class="white-text" href="detailfilm.aspx?id=<%=listFilmShow[i].id%>">Xem chi tiết</a>
                    </div>
                </div>
            </div>
        </div>

        <%} %>
    </div>

    
    <div id="DontShow" class="row">

        <%for (int i = 0; i < listFilmDontShow.Count; i++)
            {
        %>

        <div class="col s12 m12 l6" style="position: relative;">
            <div class="card large waves-effect hoverable" style="height: 350px !important; width: 100% !important">
                <div class="card-image waves-effect waves-block waves-effect" style="max-height: 70% !important; height: 70% !important">
                    <img class="activator" src="<%=listFilmDontShow[i].linkBanner%>" onerror="this.src='/img/image-not-found.png'" alt='<%=title + listFilmDontShow[i].name %>' />
                </div>
                <div class="card-content" style="max-height: 30% !important; height: 30% !important">
                    <span><i class="card-title material-icons activator right green-text">more_vert</i></span>
                    <a class="card-title grey-text text-darken-4" href="detailfilm.aspx?id=<%=listFilmDontShow[i].id%>"><%=listFilmDontShow[i].name%></a>
                    <p class="card-content" style="position: absolute; bottom: 0px !important; left: 15px !important">
                        <%="Khởi chiếu : " + (listFilmDontShow[i].premiere.HasValue ? listFilmDontShow[i].premiere.Value.ToString("dd/MM/yyyy") : "")%>
                    </p>
                    <a href="detailfilm.aspx?id=<%=listFilmDontShow[i].id%>" class="waves-effect btn right green" style="position: absolute; bottom: 15px !important; right: 15px !important; border-radius: 18px">Xem chi tiết</a>
                </div>
                <div class="card-reveal">
                    <span><i class="card-title material-icons activator right green-text">close</i></span>
                    <span class="card-title grey-text text-darken-4"><%=listFilmDontShow[i].name%></span>
                    <p style="font-size: 16px">
                        <%=
                            "Khởi chiếu : " + (listFilmDontShow[i].premiere.HasValue ? listFilmDontShow[i].premiere.Value.ToString("dd/MM/yyyy") : "") +
                                "<br/>Thời lượng : " + listFilmDontShow[i].length +
                                "<br/>Thể loại   : " + listFilmDontShow[i].genre +
                                "<br/>Diễn viên  : " + listFilmDontShow[i].actor +
                                "<br/>Đạo diễn   : " + listFilmDontShow[i].director +
                                "<br/>Quốc gia   : " + listFilmDontShow[i].country +
                                "<br/>Điểm IMDB  : " + listFilmDontShow[i].imdb +
                                "<br/>Phân loại khán giả  : " + listFilmDontShow[i].classification
                        %>
                    </p>
                    <div class="card-action green waves-effect">
                        <a class="white-text" href="detailfilm.aspx?id=<%=listFilmDontShow[i].id%>">Xem chi tiết</a>
                    </div>
                </div>
            </div>
        </div>

        <%} %>
    </div>

</asp:Content>
