<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="detailcinema.aspx.cs" Inherits="Cinema_2._0.detailcinema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - RẠP : <%=detailCinema.name %>' property='og:title' />
    <meta content='<%=detailCinema.intro %>' property='og:description' />
    <meta content='<%=detailCinema.linkImage %>' property='og:image' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">


    <br />
    <div class="card" style="width: 80% !important">
        <div class="card-image">
            <img alt="<%=title + detailCinema.name %>" src="<%=detailCinema.linkImage %>" onerror="this.src='/img/image-not-found.png'" />
        </div>
        <div class="card-content">
            <span class="card-title"><%=detailCinema.name %></span>
            <table>
                <tr>
                    <td style="padding: 3px">Thuộc cụm rạp</td>
                    <td style="padding: 3px"><%=detailCinema.Producer.name%></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Thành phố</td>
                    <td style="padding: 3px"><%=detailCinema.Location.name %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Điện thoại</td>
                    <td style="padding: 3px"><%=detailCinema.phoneNumber %></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Địa chỉ</td>
                    <td style="padding: 3px"><%=detailCinema.address %></td>
                </tr>
            </table>
            <p>Giới thiệu : </p>
            <p>
                <%=detailCinema.intro %>
            </p>

            <div class="collection">
                <h5 class="collection-header center-align green-text"><i class="material-icons">maps</i>Bản Đồ</h5>
                <div class="collection-item" id="map" style="height: 400px !important"></div>
            </div>
        </div>
    </div>
    <br />
    <div class="card" style="width: 80% !important">
        <div class="fb-comments" data-href="<%=Request.Url.AbsoluteUri.Split('&')[0] %>" data-order-by="reverse_time" data-numposts="10" data-width="100%"></div>
    </div>

    <script>
        function initMap() {
            var uluru = {lat: <%=detailCinema.latitude%>, lng: <%=detailCinema.longitude%>};
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: uluru
            });
            var marker = new google.maps.Marker({
                position: uluru,
                map: map
            });
        }
    </script>
    <script async defer
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBbz5qhlToGpQDCeS6sbH9moCRUG-o4rf8&callback=initMap">
    </script>

</asp:Content>
