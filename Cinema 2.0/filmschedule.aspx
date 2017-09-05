<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="filmschedule.aspx.cs" Inherits="Cinema_2._0.filmschedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - LỊCH : <%=detailFilm.name %>' property='og:title' />
    <meta content='<%=detailFilm.intro %>' property='og:description' />
    <meta content='<%=detailFilm.linkBanner %>' property='og:image' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <img alt="<%=title + detailFilm.name %>" onerror="this.src='/img/image-not-found.png'" style="display: none" src="<%=detailFilm.linkBanner %>" />
    <br />
    <div style="width: 80% !important">
        <div class="card">
            <div class="card-image">
                <div class="video-container">
                    <iframe width="853" height="480" src="<%=detailFilm.linkTrailer %>" frameborder="0" allowfullscreen></iframe>
                </div>
            </div>
            <div class="card-content">
                <span class="card-title"><%=detailFilm.name %></span>
                <p>
                    <%=detailFilm.intro %>
                </p>
            </div>
            <div class="card-action right-align">
                <a class="white-text btn green" style="border-radius: 18px; right: 15px !important" href="detailfilm.aspx?id=<%=detailFilm.id%>">Xem chi tiết</a>
            </div>
        </div>
        <div class="row card">
            <div class="col s12">
                <ul class="tabs">
                    <%foreach (var i in listDate)
                        {
                            dmy = i.Key.Split('-');%>
                    <li class="tab col green-text"><a style="font-size: 16px" class="green-text <%= ((date == "" && i.Key == listDate.First().Key) || date == i.Key)? "active": "" %>" href="#<%=i.Key %>"><%=dmy[2] + "/" + dmy[1] + "/" + dmy[0]  %></a></li>
                    <%} %>
                </ul>
                <h5 <%=listDate.Count > 0 ? "style='display:none'" : "" %> class="center-align green-text">Thật xui xẻo
                    <br />
                    Hiện tại không có lịch chiếu ở khu vực của bạn!<br />
                    <br />
                </h5>
            </div>

            <%foreach (var i in listDate)
                { %>
            <div id="<%=i.Key %>" class="col s12">

                <ul class="collapsible" data-collapsible="accordion">

                    <%foreach (var producer in i.Value)
                        { %>

                    <li>
                        <div class="collapsible-header" style="font-size: 17px !important"><i class="material-icons">stars</i><%=producer.Key %></div>
                        <div class="collapsible-body white" style="padding: 8px !important">
                            <ul class="collapsible" data-collapsible="accordion">
                                <%foreach (var cinema in producer.Value)
                                    {%>
                                <li>
                                    <div class="collapsible-header green-text" style="font-size: 17px !important">
                                        <i class="material-icons">location_on</i><%=cinema.Key %>
                                        <a class="right blue-grey-text" style="font-size: 14px" href="detailcinema.aspx?id=<%=cinema.Value[0].Cinema.id %>">Xem thông tin rạp<i class="large material-icons bt">info</i></a>
                                    </div>
                                    <div class="collapsible-body blue-grey">
                                        <%foreach (var schedule in cinema.Value)
                                            { %>
                                        <a href="<%=schedule.linkTicket == "null" ? "javascript:Materialize.toast('Rạp không hỗ trợ đặt vé online.\nBạn vui lòng liên hệ trực tiếp!', 3000, 'rounded')" : schedule.linkTicket%>"
                                            target="_blank" class="waves-effect waves-light btn-large green <%=DateTime.ParseExact(schedule.dateTime, "yyyy-MM-dd HH:mm", null) < DateTime.Now ? "disabled" : "" %>"
                                            style="font-size: 20px !important; border-radius: 27px; margin: 8px !important">
                                            <i class="material-icons right">today</i><%=schedule.dateTime.Replace(i.Key, "") + " - "%>
                                            <span class='<%=schedule.slot.ToUpper().Contains("2D") ? "yellow-text" : "red-text text-darken-4"%>'>
                                                <%=schedule.slot %></span></a>
                                        <%} %>
                                    </div>
                                </li>
                                <%} %>
                            </ul>
                        </div>
                    </li>

                    <%} %>
                </ul>
            </div>
            <%} %>
        </div>
    </div>

    <br />
    <div class="card" style="width: 80% !important">
        <div class="fb-comments" data-href="<%=Request.Url.AbsoluteUri.Split('&')[0] %>" data-order-by="reverse_time" data-numposts="10" data-width="100%"></div>
    </div>

</asp:Content>
