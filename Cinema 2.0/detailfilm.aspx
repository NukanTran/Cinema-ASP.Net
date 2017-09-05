<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="detailfilm.aspx.cs" Inherits="Cinema_2._0.detailfilm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - PHIM : <%=detailFilm.name %>' property='og:title' />
    <meta content='<%=detailFilm.intro %>' property='og:description' />
    <meta content='<%=detailFilm.linkBanner %>' property='og:image' />
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <br />
    <div class="card" style="width: 80% !important">
        <div class="card-image">
            <img alt="<%=title + detailFilm.name %>" src="<%=detailFilm.linkBanner %>" onerror="this.src='/img/image-not-found.png'" />
        </div>
        <div class="card-content">
            <span class="card-title"><%=detailFilm.name %></span>
            <table>
                <tr>
                    <td style="padding: 3px">Khởi chiếu</td>
                    <td style="padding: 3px"><%=(detailFilm.premiere.HasValue ? detailFilm.premiere.Value.ToString("dd/MM/yyyy") : "") %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Thời lượng</td>
                    <td style="padding: 3px"><%=detailFilm.length %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Thể loại</td>
                    <td style="padding: 3px"><%=detailFilm.genre %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Diễn viên</td>
                    <td style="padding: 3px"><%=detailFilm.actor %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Đạo diễn</td>
                    <td style="padding: 3px"><%=detailFilm.director %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Quốc gia</td>
                    <td style="padding: 3px"><%=detailFilm.country %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Phân loại người xem</td>
                    <td style="padding: 3px"><%=detailFilm.classification %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Điểm IMDB</td>
                    <td style="padding: 3px"><%=detailFilm.imdb %></td>
                </tr>
            </table>
            <p>Nội Dung : </p>
            <p>
                <%=detailFilm.intro %>
            </p>
            <ul class="collapsible" data-collapsible="accordion">
                <li>
                    <div class="collapsible-header waves-effect center-align">
                        <h5 class="center-align green-text"><i class="material-icons">video_library</i>Xem Trailer</h5>
                    </div>
                    <div class="collapsible-body" style="padding: 0px">
                        <div class="video-container">
                            <iframe width="853" height="480" src="<%=detailFilm.linkTrailer %>" frameborder="0" allowfullscreen></iframe>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div class="card-action right-align">
            <a class="white-text btn green waves-effect" style="border-radius: 18px; right: 15px !important" href="filmschedule.aspx?id=<%=detailFilm.id %>">Đặt vé</a>
        </div>
    </div>
    <br />
    <div class="card" style="width: 80% !important">
    <div class="fb-comments" data-href="<%=Request.Url.AbsoluteUri.Split('&')[0] %>" data-order-by="reverse_time" data-numposts="10" data-width="100%"></div>
    </div>

</asp:Content>
